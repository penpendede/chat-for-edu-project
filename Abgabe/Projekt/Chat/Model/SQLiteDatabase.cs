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

        /// <summary>
        /// Initialize SQLite db
        /// </summary>
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

        /// <summary>
        /// Open db
        /// </summary>
        public override void Open()
        {
            connection.Open();
        }

        /// <summary>
        /// close db
        /// </summary>
        public override void Close()
        {
            connection.Close();
        }

        /// <summary>
        /// modify SQL command to match database used
        /// </summary>
        /// <param name="query">incoming query</param>
        /// <returns>modified query</returns>
        private string _modifySQL(string query)
        {
            Regex pattern = new Regex("auto_increment", RegexOptions.IgnoreCase);

            query = pattern.Replace(query, "");

            return query;
        }

        /// <summary>
        /// Execute SQL query
        /// </summary>
        /// <param name="query">query to perform</param>
        /// <returns>query result as a list of string arrays</returns>
        public override List<string[]> ExecuteSQLQuery(string query)
        {
            query = _modifySQL(query);

            SQLiteCommand sql_cmd = connection.CreateCommand();

            sql_cmd.CommandText = query;

            SQLiteDataReader reader = sql_cmd.ExecuteReader();

            List<string[]> resultSet = new List<string[]>();

            while (reader.Read())
            {
                string[] row = new string[reader.FieldCount];
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    row[i] = reader[i].ToString();
                }
                resultSet.Add(row);

            }

            reader.Close();
            reader.Dispose();

            sql_cmd.Dispose();

            return resultSet;
        }

        /// <summary>
        /// find id that was last inserted into db
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public override string LastInsertedId(string tableName)
        {
            return ExecuteSQLQuery("SELECT last_insert_rowid() FROM " + tableName + ";")[0][0];
        }

        /// <summary>
        /// turn date and time into a representation
        /// </summary>
        /// <param name="dt">date and time as DateTime object</param>
        /// <returns></returns>
        public override string FormatDateTime(DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}
