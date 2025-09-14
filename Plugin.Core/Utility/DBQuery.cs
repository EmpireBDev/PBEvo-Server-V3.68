using System.Collections.Generic;

namespace Plugin.Core.Utility
{
    public class DBQuery
    {
        private readonly List<string> Tables;
        private readonly List<object> Values;
        public DBQuery()
        {
            Tables = new List<string>();
            Values = new List<object>();
        }
        public void AddQuery(string table, object value)
        {
            Tables.Add(table);
            Values.Add(value);
        }
        public string[] GetTables()
        {
            return Tables.ToArray();
        }
        public object[] GetValues()
        {
            return Values.ToArray();
        }
    }
}