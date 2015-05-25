using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Finisar.SQLite;

namespace Chat.Model
{

    public class SQLiteDatabase : Database
    {
        private SQLiteConnection connection;

        public SQLiteDatabase()
        {
            connection = new SQLiteConnection("Data Source=Chat.db3;Version=3;New=True;Compress=True;");
        }

        public override void Open()
        {
            connection.Open();
        }

        public override void Close()
        {
            connection.Close();
        }

        public override List<string[]> ExecuteSQLQuery(string query)
        {
            SQLiteCommand sql_cmd = connection.CreateCommand();

            sql_cmd.CommandText = query;

            SQLiteDataReader reader = sql_cmd.ExecuteReader();

            List<string[]> resultSet = new List<string[]>();

            if (reader.Read())
            {
                do
                {
                    string[] row = new string[reader.FieldCount];
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        row[i] = reader[i].ToString();
                    }
                    resultSet.Add(row);

                } while (reader.Read());

            }

            reader.Close();
            reader.Dispose();

            sql_cmd.Dispose();

            return resultSet;
        }
    }
}
