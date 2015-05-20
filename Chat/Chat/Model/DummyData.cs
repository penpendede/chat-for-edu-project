using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chat.Model
{
    public class DummyData
    {
        public static UserLocal CreateUserLocalTest1() {
            UserRemote buddy1 = new UserRemote()
            {
                Id = 1,
                Name = "Gal"
            };

            UserRemote buddy2 = new UserRemote()
            {
                Id = 2,
                Name = "Guy"
            };
            UserRemote buddy3 = new UserRemote()
            {
                Id = 3,
                Name = "AlienMutantSquirrel"
            };

            UserLocal user = new UserLocal()
            {
                Id = 4,
                Name = "You",
                Buddies = new List<UserRemote>() { buddy1, buddy2, buddy3 }
            };

            Conversation conv1 = new Conversation()
            {
                Id = 1,
                Users = new List<User>() { user, buddy1, buddy3 }, // NOTE: does user needs to be included here?
                Messages = new List<Message>() { 
                    new Message() { Text = "Hi", Sender = buddy1, Time = new DateTime(2015, 2, 16, 12, 55, 12) },
                    new Message() { Text = "muajsdjklagfrgh", Sender = buddy3, Time = new DateTime(2015, 2, 16, 13, 0, 0)
                    }
                }
            };

            Conversation conv2 = new Conversation()
            {
                Id = 2,
                Users = new List<User>() { user, buddy2 },
                Messages = new List<Message>() {
                    new Message() { Text = "Hi", Sender = buddy2, Time = DateTime.Now }
                }
            };

            user.Conversations = new List<Conversation>() { conv1, conv2 };

            return user;
        }
    }
}
