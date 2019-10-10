using Equifax.CSV.Importer.Data.Abstract;
using Equifax.CSV.Importer.Models;
using System.Data.SqlClient;
using System.Data;

namespace Equifax.CSV.Importer.Data.Concrete
{
    public class SqlConnectionFactory : IConnectionFactory
    {
        private readonly ConnectionStrings _connectionStrings;

        public SqlConnectionFactory(ConnectionStrings connectionStrings)
        {
            _connectionStrings = connectionStrings;
        }

        public IDbConnection CreateLocalConnection()
        {
            return new SqlConnection(_connectionStrings.Local);
        }
    }
}
