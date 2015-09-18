using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chat.Model
{
    public abstract class Database
    {
        public abstract List<string[]> ExecuteSQLQuery(string query);

        public abstract void Open();
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

        public abstract string FormatDateTime(DateTime dt);
    }
}
