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
                Name = "You"
            };
            user.AddBuddy(buddy1);
            user.AddBuddy(buddy2);
            user.AddBuddy(buddy3);

            Conversation conv1 = new Conversation()
            {
                Id = 1,
                Owner = user
            };
            conv1.AddBuddy(buddy1);
            conv1.AddBuddy(buddy3);
            conv1.AddMessage(new Message() { Text = "Hi", Sender = buddy1, Time = new DateTime(2015, 2, 16, 12, 55, 12) });
            conv1.AddMessage(new Message() { Text = "muajsdjklagfrgh", Sender = buddy3, Time = new DateTime(2015, 2, 16, 13, 0, 0) });

            Conversation conv2 = new Conversation()
            {
                Id = 2,
                Owner = user
            };
            conv2.AddBuddy(buddy2);
            conv2.AddMessage(new Message() { Text = "Hi", Sender = buddy2, Time = DateTime.Now });

            user.AddConversation( conv1);
            user.AddConversation( conv2 );

            return user;
        }
    }
}
