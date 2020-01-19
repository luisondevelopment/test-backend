using System.Data.Common;
using System.Data.SqlClient;
using TestBackend.CrossCutting;

namespace TestBackend.Infrastructure
{
    public class ConnectionFactory
    {
        public static DbConnection GetTestBackendOpenConnection()
        {
            var connection = new SqlConnection(ConnectionStrings.TestbackendConnectionString);

            if (connection.State != System.Data.ConnectionState.Open)
                connection.Open();

            return connection;
        }
    }
}
