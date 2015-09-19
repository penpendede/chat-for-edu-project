using Chat.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Chat.Controller
{
    /// <summary>
    /// Messages can currently be invalid or of one of the types MSG, BINARY and CLOSE 
    /// </summary>
    public enum MessageType
    {
        INVALID = -1,
        MSG,
        BINARY,
        CLOSE
    }

    /// <summary>
    /// Static class that provides functions to assemble/disassemble JSON-formatted messages sent via network
    /// </summary>
    public class NetworkMessageInterpreter
    {
        /// <summary>
        /// Get buddy (of type UserRemote) who sent the message
        /// </summary>
        /// <param name="message">Message, already processed into a Dictionary</param>
        /// <param name="userLocal">The local user (to look up if buddy is already present)</param>
        /// <returns>the message's sender</returns>
        static public UserRemote GetSender(Dictionary<string, string> message, UserLocal userLocal)
        {
            UserRemote buddy = null;

            if (message.ContainsKey("UserFrom")) {
                // lambda takes a buddy and returns the truth value for "buddy's name is message's UserFrom"
                buddy = _BuddyByName(userLocal, message["UserFrom"]);

                // if messages's UserFrom is not in the buddy list create a new Buddy
                if (buddy == null)
                {
                    buddy = new UserRemote() { Name = message["UserFrom"] };
                }

                // add information that may or may not be available
                if (message.ContainsKey("IpFrom"))
                {
                    buddy.IP = message["IpFrom"];
                }
                if (message.ContainsKey("PortFrom"))
                {
                    buddy.Port = Int32.Parse(message["PortFrom"]);
                }
            }

            // intentionally returns null if message has no UserFrom
            return buddy;
        }

        /// <summary>
        /// Obtain the message type
        /// </summary>
        /// <param name="message">Message, already processed into a Dictionary</param>
        /// <returns></returns>
        static public MessageType GetType(Dictionary<string, string> message)
        {
            if (message.ContainsKey("Type"))
            {
                try
                {
                    // Parse message's type as a MessageType's name and return the corresponding value of type MessageType
                    return (MessageType)Enum.Parse(typeof(MessageType), message["Type"]);
                }
                catch { } // catche parser failure if it occurs; nothing to be done here, fall through to returning Messagetype.INVALID
            }

            return MessageType.INVALID;
        }

        /// <summary>
        /// Deserialize JSON-encoded message into a dictionary representing its key-value pairs
        /// </summary>
        /// <param name="message">JSON-encoded message</param>
        /// <returns>Dictionary representing the message</returns>
        static public Dictionary<string, string> Deserialize(string message)
        {
            Dictionary<string, string> parseResult = new Dictionary<string, string>();

            // beginning of string, possible whitespace, opening curly brace, possible whitespace, quotation mark
            // i.e. the initial part of the string that is to be removed
            Regex initialPartPattern = new Regex("\\A\\s*{\\s*\"");

            // quotation mark, possible whitespace, closing curly brace, possible whitespace, end of string
            // i.e. the final part of the string that is to be removed
            Regex finalPartPattern = new Regex("\"\\s*}\\s*\\z");

            // quotation mark, possible whitespace, comma, possible whitespace, quotation mark
            // i.e. the part of the string that separates the individual key-value pairs
            Regex messagePartSeparatorPattern = new Regex("\"\\s*,\\s*\"");

            // quotation mark, possible whitespace, colon, possible whitespace, quotation mark
            // i.e. the part of a key-value pair that separates key and value
            Regex keyValuePartSeparatorPattern = new Regex("\"\\s*:\\s*\"");

            // only parse if there is a message to parse
            if (!string.IsNullOrEmpty(message))
            {
                // replace initial part of string by an empty one, effectively removing it
                string withoutInitialPart = initialPartPattern.Replace(message, "");

                // same for final part
                string withoutInitialAndFinalPart = finalPartPattern.Replace(withoutInitialPart, "");

                // split remaining string at message part separator
                string[] messageParts = messagePartSeparatorPattern.Split(withoutInitialAndFinalPart);

                // for each message part
                for (int i = 0; i < messageParts.Length; i++)
                {
                    // separate key from value
                    string[] keyValue = keyValuePartSeparatorPattern.Split(messageParts[i]);

                    // make them a Dictionary entry
                    parseResult.Add(keyValue[0], keyValue[1]);
                }
            }
            // return parse result (if message was null or empty that's an empty Dictionary)
            return parseResult;
        }

        /// <summary>
        /// Serialize a dictionary containing key-value pairs into a JSON string
        /// </summary>
        /// <param name="messageData"></param>
        /// <returns></returns>
        static public string Serialize(Dictionary<string, string> messageData)
        {
            bool first = true;
            string serializedMessage = "{";
            // iterate over all key-value paris
            foreach (var keyValuePair in messageData)
            {
                // precede all but the first key-value pair representation by a comma
                if (first)
                {
                    first = false;
                }
                else
                {
                    serializedMessage += ",";
                }
                // then simply add a quotation "<key>":"<value>"
                serializedMessage += "\"" + keyValuePair.Key + "\":\"" + keyValuePair.Value + "\"";
            }
            serializedMessage += "}";
            return serializedMessage;
        }

        /// <summary>
        /// Add what we consider standard parts of the message (to a message in Dictionary representation)
        /// </summary>
        /// <param name="dict">The dicionary where the standard parts are added to.</param>
        /// <param name="sender">The dedicated sender of the message</param>
        /// <param name="recipient">The dedicated recipent of the message</param>
        static private void _addStandardParts(Dictionary<string, string> dict, User sender, User recipient)
        {
            // "From" information
            dict.Add("UserFrom", sender.Name);
            dict.Add("IpFrom", sender.IP);
            dict.Add("PortFrom", sender.Port.ToString());
            
            // "To" information
            dict.Add("UserTo", recipient.Name);
            dict.Add("IpTo", recipient.IP);
            dict.Add("PortTo", recipient.Port.ToString());
        }

        /// <summary>
        /// Serialize a text message
        /// </summary>
        /// <param name="message">The text message of type Message</param>
        /// <param name="recipient">The dedicated recipent</param>
        /// <returns></returns>
        static public Dictionary<string, string> SerializeTextMessage(Message message, User recipient)
        {
            Dictionary<string, string> dict = new Dictionary<string,string>();
            dict.Add("Type", "MSG");

            _addStandardParts(dict, message.Sender, recipient);

            dict.Add("Content", message.Text);
            dict.Add("TimeStamp", message.Time.ToString());  // timestamp becomes the current time

            // Conversation muss noch angegeben sein
            return dict;
        }

        /// <summary>
        /// Serialize quit message
        /// </summary>
        /// <param name="quitMessage">a good-bye message</param>
        /// <param name="userLocal">local user</param>
        /// <param name="recipient">message's recipent</param>
        /// <returns></returns>
        static public Dictionary<string, string> SerializeQuitMessage(string quitMessage, UserLocal userLocal, UserRemote recipient)
        {
            Dictionary<string,string> dict = new Dictionary<string,string>();
            dict.Add("Type", "CLOSE");

            _addStandardParts(dict, userLocal, recipient);

            dict.Add("Content", quitMessage); // We have a non-empty content here as we assume a quit messages like "gn8"
            
            return dict;
        }

        /// <summary>
        /// Serialize binary message
        /// </summary>
        /// <param name="message">message of type Message</param>
        /// <param name="userLocal">local user</param>
        /// <param name="recipient">message's recipent</param>
        /// <returns></returns>
        static public Dictionary<string, string> SerializeBinaryMessage(Message message, UserLocal userLocal, UserRemote recipient)
        {
            Dictionary<string,string> dict = new Dictionary<string,string>();
            dict.Add("Type", "BINARY");

            _addStandardParts(dict, userLocal, recipient);

            // TODO: binary messages are not fully implemented yet
            return dict;
        }

        /// <summary>
        /// Check if all required keys are present
        /// </summary>
        /// <param name="message">The truth value of "all required keys are present"</param>
        /// <returns></returns>
        static private bool DictionaryHasRequiredKeys(Dictionary<string, string> message)
        {
            return (message.ContainsKey("Content") && message.ContainsKey("UserFrom") 
                && message.ContainsKey("Type") && message.ContainsKey("UserTo"));
        }

        /// <summary>
        /// Deserialize text message (already deserialized into Dictionary) into a message of type Message
        /// </summary>
        /// <param name="message">the message in Dictionary representation</param>
        /// <param name="userLocal">the local user</param>
        /// <returns></returns>
        static public Message DeserializeTextMessage(Dictionary<string, string> message, UserLocal userLocal)
        {
            Message msg = new Message();

            if (NetworkMessageInterpreter.DictionaryHasRequiredKeys(message) && message["Type"] == "MSG")
            {
                msg.Sender = GetSender(message, userLocal);
                msg.Text = message["Content"];
                if (message.ContainsKey("Timestamp"))
                {
                    msg.Time = DateTime.Parse(message["TimeStamp"]);
                }
                // TODO: Zuordnung zu Gruppenchat implementieren
            }
            else
            {
                throw new ArgumentException("Required key missing or message type differs from 'MSG'");
            }
            return msg;
        }

        /// <summary>
        /// Deserialize quit message (already deserialized into Dictionary) into a string
        /// </summary>
        /// <param name="message">the message in Dictionary representation</param>
        /// <param name="userLocal">the local user</param>
        /// <param name="fromUser">the user who sent the message (to be set by this method)</param>
        /// <returns>The quit message as a string</returns>
        static public string DeserializeQuitMessage(Dictionary<string, string> message, UserLocal userLocal, out UserRemote fromUser)
        {
            if (NetworkMessageInterpreter.DictionaryHasRequiredKeys(message) && message["Type"] == "CLOSE")
            {
                
                fromUser = _BuddyByName(userLocal, message["UserFrom"]);
                return message["Content"];
            }
            else
            {
                throw new ArgumentException("Required key missing or message type differs from 'CLOSE'");
            }
        }

        /// <summary>
        /// Deserialize binary message (already deserialized into Dictionary) to byte array
        /// </summary>
        /// <param name="message">the message in Dictionary representation</param>
        /// <param name="userLocal">the local user</param>
        /// <param name="fromUser">the user who sent the message (to be set by this method)</param>
        /// <returns>The binary message converted into a byte array</returns>
        static public byte[] DeserializeBinaryMessage(Dictionary<string, string> message, UserLocal userLocal, out UserRemote fromUser)
        {
            if (NetworkMessageInterpreter.DictionaryHasRequiredKeys(message))
            {
                fromUser = _BuddyByName(userLocal, message["UserFrom"]);
                return Convert.FromBase64String(message["Content"]);
            }
            else
            {
                throw new ArgumentException("Required key missing");
            }
        }

        /// <summary>
        /// Find a buddy the given name
        /// </summary>
        /// <param name="userLocal">the local user (who may or may not have a buddy by that the given name</param>
        /// <param name="buddyName">the buddy's name</param>
        /// <returns></returns>
        static private UserRemote _BuddyByName(UserLocal userLocal, string buddyName)
        {
            // the lambda takes a buddy and yields the truth value of "buddy's name is message's UserFrom"
            return userLocal.Buddies.FirstOrDefault(b => b.Name == buddyName);
        }
    }
}