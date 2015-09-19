using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chat.Model
{
    public abstract class Database
    {
        /// <summary>
        /// Execute an sql query
        /// </summary>
        /// <param name="query">a sql query</param>
        /// <returns>a list of result rows</returns>
        public abstract List<string[]> ExecuteSQLQuery(string query);

        /// <summary>
        /// Open the database connection
        /// </summary>
        public abstract void Open();

        /// <summary>
        /// Close the database connection
        /// </summary>
        public abstract void Close();

        public abstract string LastInsertedId(string tableName);

        /// <summary>
        /// Escape string, i.e. replace \ by \\ and ' by ''
        /// </summary>
        /// <param name="text">text to escape</param>
        /// <returns>escaped text</returns>
        public virtual string Escape(string text)
        {
            return text.Replace("\\", "\\\\").Replace("'", "''");
        }

        /// <summary>
        /// Properly format a DateTime according to the conventions of the used database
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public abstract string FormatDateTime(DateTime dt);
    }
}
