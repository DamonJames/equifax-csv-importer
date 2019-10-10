using Member = Equifax.CSV.Importer.Models.Member;
using Equifax.CSV.Importer.Data.Abstract;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using FastMember;

namespace Equifax.CSV.Importer.Data.Concrete
{
    public class MemberRepository : IMemberRepository
    {
        IConnectionFactory _connectionFactory;

        public MemberRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public void AddMany(IEnumerable<Member> data)
        {
            using (var connection = _connectionFactory.CreateLocalConnection())
            {
                var bulkCopy = new SqlBulkCopy(connection as SqlConnection,
                    SqlBulkCopyOptions.TableLock |
                    SqlBulkCopyOptions.FireTriggers |
                    SqlBulkCopyOptions.UseInternalTransaction, null);

                bulkCopy.DestinationTableName = "[dbo].[Members]";
                bulkCopy.BatchSize = data?.Count() ?? 0;

                connection.Open();

                bulkCopy.ColumnMappings.Add("col1", "Id");
                bulkCopy.ColumnMappings.Add("col2", "Name");
                bulkCopy.ColumnMappings.Add("col3", "DateOfBirth");
                bulkCopy.ColumnMappings.Add("col4", "Height");
                bulkCopy.ColumnMappings.Add("col5", "Balance");

                var records = data.Select(mem => new { col1 = mem.Id, col2 = mem.Name, col3 = mem.DateOfBirth, col4 = mem.Height, col5 = mem.Balance });

                using (var reader = ObjectReader.Create(records, "col1", "col2", "col3", "col4", "col5"))
                {
                    bulkCopy.WriteToServer(reader);
                };

                connection.Close();
            }
        }
    }
}
