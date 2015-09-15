using Chat.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Chat.Controller
{
    public enum MessageType
    {
        INVALID = -1,
        MSG,
        BINARY,
        CLOSE
        
    }

    public class NetworkMessageInterpreter
    {
        static public UserRemote GetSender(Dictionary<string, string> message, UserLocal userLocal)
        {
            UserRemote buddy = null;

            if (message.ContainsKey("UserFrom")) {
                buddy = userLocal.Buddies.FirstOrDefault(b => b.Name == message["UserFrom"]);
                if (buddy == null)
                {
                    buddy = new UserRemote() { Name = message["UserFrom"] };
                }

                if (message.ContainsKey("IpFrom"))
                {
                    buddy.IP = message["IpFrom"];
                }
                if (message.ContainsKey("PortFrom"))
                {
                    buddy.Port = Int32.Parse(message["PortFrom"]);
                }
            }

            return buddy;
        }

        static public MessageType GetType(Dictionary<string, string> message)
        {
            if (message.ContainsKey("Type"))
            {
                try
                {
                    return (MessageType)Enum.Parse(typeof(MessageType), message["Type"]);
                }
                catch { }
            }

            return MessageType.INVALID;
        }

        static public Dictionary<string, string> Deserialize(string message)
        {
            Dictionary<string, string> parseResult = new Dictionary<string, string>();
            Regex initialPartPattern = new Regex("\\A\\s*{\\s*\"");
            Regex finalPartPattern = new Regex("\"\\s*}\\s*\\z");
            Regex messagePartSeparatorPattern = new Regex("\"\\s*,\\s*\"");
            Regex keyValuePartSeparatorPattern = new Regex("\"\\s*:\\s*\"");

            // check if message is present, starts with '{' and ends with '}'
            if (!string.IsNullOrEmpty(message))
            {
                string withoutInitialPart = initialPartPattern.Replace(message, "");
                string withoutInitialAndFinalPart = finalPartPattern.Replace(withoutInitialPart, "");
                string[] messageParts = messagePartSeparatorPattern.Split(withoutInitialAndFinalPart);
                for (int i = 0; i < messageParts.Length; i++)
                {
                    string[] keyValue = keyValuePartSeparatorPattern.Split(messageParts[i]);
                    parseResult.Add(keyValue[0], keyValue[1]);
                }
            }
            return parseResult;
        }
        static public string Serialize(Dictionary<string, string> messageData)
        {
            bool first = true;
            string serializedMessage = "{";
            foreach (var keyValuePair in messageData)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    serializedMessage += ",";
                }
                serializedMessage += "\"" + keyValuePair.Key + "\":\"" + keyValuePair.Value + "\"";
            }
            serializedMessage += "}";
            return serializedMessage;
        }

        static void AddStandardParts(Dictionary<string, string> dict, User sender, User recipient)
        {
            dict.Add("UserFrom", sender.Name);
            dict.Add("IpFrom", sender.IP);
            dict.Add("PortFrom", sender.Port.ToString());
            
            dict.Add("UserTo", recipient.Name);
            dict.Add("IpTo", recipient.IP);
            dict.Add("PortTo", recipient.Port.ToString());
        }

        static public Dictionary<string, string> SerializeTextMessage(Message message, User recipient)
        {
            Dictionary<string, string> dict = new Dictionary<string,string>();
            dict.Add("Type", "MSG");

            AddStandardParts(dict, message.Sender, recipient);

            dict.Add("Content", message.Text);
            dict.Add("TimeStamp", message.Time.ToString());

            // Conversation muss noch angegeben sein
            return dict;
        }

        static public Dictionary<string, string> SerializeQuitMessage(string message, UserLocal userLocal, UserRemote recipient)
        {
            Dictionary<string,string> dict = new Dictionary<string,string>();
            dict.Add("Type", "CLOSE");

            AddStandardParts(dict, userLocal, recipient);

            dict.Add("Content", message);
            
            return dict;
        }

        static public Dictionary<string, string> SerializeBinaryMessage(Message message, UserLocal userLocal, UserRemote recipient)
        {
            Dictionary<string,string> dict = new Dictionary<string,string>();
            dict.Add("Type", "BINARY");

            AddStandardParts(dict, userLocal, recipient);

            return dict;
        }

        static private bool DictionaryHasRequiredKeys(Dictionary<string, string> message)
        {
            return (message.ContainsKey("Content") && message.ContainsKey("UserFrom") 
                && message.ContainsKey("Type") && message.ContainsKey("UserTo"));
        }

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
                throw new ArgumentException("Required key missing");
            }
            return msg;
        }

        static public string DeserializeQuitMessage(Dictionary<string, string> message, UserLocal userLocal, out UserRemote fromUser)
        {
            if (NetworkMessageInterpreter.DictionaryHasRequiredKeys(message))
            {
                fromUser = userLocal.Buddies.FirstOrDefault(b => b.Name == message["UserFrom"]);
                return message["Content"];
            }
            else
            {
                throw new ArgumentException("Required key missing");
            }
        }

        static public byte[] DeserializeBinaryMessage(Dictionary<string, string> message, UserLocal userLocal, out UserRemote fromUser)
        {
            if (NetworkMessageInterpreter.DictionaryHasRequiredKeys(message))
            {
                fromUser = userLocal.Buddies.FirstOrDefault(b => b.Name == message["UserFrom"]);
                return Convert.FromBase64String(message["Content"]);
            }
            else
            {
                throw new ArgumentException("Required key missing");
            }
        }
    }
}