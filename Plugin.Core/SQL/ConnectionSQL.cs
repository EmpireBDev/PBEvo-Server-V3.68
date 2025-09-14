using Npgsql;
using System.Runtime.Remoting.Contexts;

namespace Plugin.Core.SQL
{
    [Synchronization]
    public class ConnectionSQL
    {
        private static ConnectionSQL SQL = new ConnectionSQL();
        protected NpgsqlConnectionStringBuilder ConnBuilder;
        public ConnectionSQL()
        {
            ConnBuilder = new NpgsqlConnectionStringBuilder() 
            { 
                Database = ConfigLoader.DatabaseName, 
                Host = ConfigLoader.DatabaseHost, 
                Username = ConfigLoader.DatabaseUsername, 
                Password = ConfigLoader.DatabasePassword, 
                Port = ConfigLoader.DatabasePort 
            };
        }
        public static ConnectionSQL GetInstance()
        {
            return SQL;
        }
        public NpgsqlConnection Conn()
        {
            return new NpgsqlConnection(ConnBuilder.ConnectionString);
        }
    }
}