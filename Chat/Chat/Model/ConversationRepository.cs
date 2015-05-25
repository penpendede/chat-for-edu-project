using Chat.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chat.Model
{
    public class ConversationRepository : IRepository<Conversation>
    {
        private DatabaseController dbController;

        private List<Conversation> loaded;

        // check if conversation is contained in database
        public bool Contains(Conversation obj)
        {
            // empty object -> not contained
            if (obj == null)
            {
                return false;
            }
            // Id not set -> not contained
            if (obj.Id == null)
            {
                return false;
            }
            // ID in loaded obj -> contained
            if (loaded.Where(o => o.Id == obj.Id).Count() > 0)
            {
                return true;
            }
            // ID found -> contained
            if (dbController.Database.ExecuteSQLQuery("SELECT COUNT(id) FROM conversation WHERE id = " + obj.Id + ";")[0][0] != "0")
            {
                return true;
            }
            // default: not contained
            return false;
        }

        //List<Conversation> GetByParticipant(User user)
        //{
        //    List<Conversation> conversations = new List<Conversation>();

        //    if (user != null)
        //    {
        //        List<string[]> result = dbController.Database.ExecuteSQLQuery("SELECT conversationid FROM conversation_has_user WHERE userid = " + user.Id + ";");
        //        foreach (string[] row in result)
        //        {
        //            conversations.Add(GetById(Int32.Parse(row[0])));
        //        }
        //    }
        //    return conversations;
        //}

        // get a single Conversation by ID
        public Conversation GetById(int id)
        {
            // if already fetched serve from memory
            if (loaded.Where(c => c.Id == id).Count() > 0)
            {
                return loaded.Where(c => c.Id == id).First();
            }

            List<string[]> result = dbController.Database.ExecuteSQLQuery("SELECT active, ownerid FROM conversation WHERE id = " + id + ";");

            Conversation conversation = new Conversation()
            {
                Id = id,
                Owner = dbController.UserLocalRepo.GetById(Int32.Parse(result[0][1]))
            };

            conversation.SetActive(Boolean.Parse(result[0][0]));


            List<string[]> resultBuddies = dbController.Database.ExecuteSQLQuery("SELECT userid FROM conversation_has_user WHERE conversationid = " + id + ";");

            foreach (string[] row in resultBuddies)
            {
                conversation.AddBuddy(dbController.UserRemoteRepo.GetById(Int32.Parse(row[0])));
            }

            List<string[]> resultMessages = dbController.Database.ExecuteSQLQuery("SELECT id FROM message WHERE conversationid = " + id + ";");

            foreach (string[] row in resultMessages)
            {
                conversation.AddMessage(dbController.MessageRepo.GetById(Int32.Parse(row[0])));
            }

            // store future changes
            conversation.BuddyAdd += (conv, bud) => Update(conv);
            conversation.BuddyRemove += (conv, bud) => Update(conv);
            conversation.ChangeActive += (conv, act) => Update(conv);
            conversation.MessageAdd += (conv, mes) => dbController.MessageRepo.Insert(mes);

            // store
            loaded.Add(conversation);

            return conversation;
        }

        // Insert conversation into database
        public void Insert(Conversation obj)
        {
            // Add conversation to conversation table
            dbController.Database.ExecuteSQLQuery("INSERT INTO conversation (id, active) VALUES (" + obj.Id + ", " + obj.Active + ");");

            // add all conversationId/userID pairs to conversation_has_user
            foreach (UserRemote buddy in obj.Buddies)
            {
                dbController.Database.ExecuteSQLQuery("INSERT INTO conversation_has_user (conversationid, userid) VALUES (" + obj.Id + ", " + buddy.Id + ");");
            }
            dbController.Database.ExecuteSQLQuery("INSERT INTO conversation_has_user (conversationid, userid) VALUES (" + obj.Id + ", " + obj.Owner.Id + ");");

            // todo: set id
            // todo: store
        }

        // Remove conversation: Obtain conversation's ID, then remove by ID
        public void Remove(Conversation obj)
        {
            if (obj != null)
            {
                RemoveById(obj.Id);
            }
        }

        // Remove conversation by ID
        public void RemoveById(int id)
        {
            // remove from conversation_has_user
            dbController.Database.ExecuteSQLQuery("DELETE FROM conversation_has_user WHERE conversationid = " + id + ";");
            // remove conversation itself
            dbController.Database.ExecuteSQLQuery("DELETE FROM conversation WHERE id = " + id + ";");
        }

        // update a single conversation
        public void Update(Conversation obj)
        {
            if (obj != null)
            {
                // update activity 
                dbController.Database.ExecuteSQLQuery("UPDATE conversation SET active = " + obj.Active + " WHERE id = " + obj.Id + ";");

                // rebuild buddy list ()
                dbController.Database.ExecuteSQLQuery("DELETE FROM conversation_has_user WHERE conversationid = " + obj.Id + ";");
                foreach (UserRemote buddy in obj.Buddies)
                {
                    dbController.Database.ExecuteSQLQuery("INSERT INTO conversation_has_user (conversationid, userid) VALUES (" + obj.Id + ", " + buddy.Id + ");");
                }
            }
        }

        // insert new conversation or update existing one
        public void InsertOrUpdate(Conversation obj)
        {
            if (Contains(obj))
            {
                Update(obj);
            }
            else
            {
                Insert(obj);
            }
        }
    }
}
