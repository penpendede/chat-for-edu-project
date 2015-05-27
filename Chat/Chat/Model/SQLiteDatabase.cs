using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using Finisar.SQLite;
using System.Data.SQLite;
using System.Text.RegularExpressions;

namespace Chat.Model
{

    public class SQLiteDatabase : Database
    {
        private SQLiteConnection connection;

        public SQLiteDatabase()
        {
            string connectionString = "Data Source=Chat.db3;Version=3;Compress=True;";

            if (!System.IO.File.Exists("Chat.db3"))
            {
                connectionString += "New=True;";
            }

            connection = new SQLiteConnection(connectionString);
            Open();
        }

        public override void Open()
        {
            connection.Open();
        }

        public override void Close()
        {
            connection.Close();
        }

        private string _modifySQL(string query)
        {
            Regex pattern = new Regex("auto_increment", RegexOptions.IgnoreCase);

            query = pattern.Replace(query, "");

            return query;
        }

        public override List<string[]> ExecuteSQLQuery(string query)
        {
            query = _modifySQL(query);

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

        public override string LastInsertedId(string tableName)
        {
            return ExecuteSQLQuery("SELECT last_insert_rowid() FROM " + tableName + ";")[0][0];
        }

        public override string FormatDateTime(DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}
