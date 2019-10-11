using Member = Equifax.CSV.Importer.Models.Member;
using Equifax.CSV.Importer.Data.Abstract;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Linq;
using System.Data;
using System;
using Dapper;

namespace Equifax.CSV.Importer.Data.Concrete
{
    public class MemberRepository : IMemberRepository
    {
        IConnectionFactory _connectionFactory;

        public MemberRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<Member>> GetAsync()
        {
            const string sql = @"SELECT * FROM [experian].[dbo].[Members]";

            using (var connection = _connectionFactory.CreateLocalConnection())
            {
                return await connection.QueryAsync<Member>(sql);
            }
        }

        public void AddMany(IEnumerable<Member> data)
        {
            using (var connection = _connectionFactory.CreateLocalConnection() as SqlConnection)
            {
                try
                {
                    connection.Open();

                    var dt = new DataTable();

                    dt.Columns.Add("Id", typeof(Guid));
                    dt.Columns.Add("Name");
                    dt.Columns.Add("DateOfBirth");
                    dt.Columns.Add("Height");
                    dt.Columns.Add("Balance");

                    foreach (var mem in data)
                    {
                        dt.Rows.Add(mem.Id, mem.Name, mem.DateOfBirth, mem.Height, mem.Balance);
                    };

                    using (var bulkCopy = new SqlBulkCopy(connection,
                    SqlBulkCopyOptions.TableLock |
                    SqlBulkCopyOptions.FireTriggers,
                    null))
                    {
                        bulkCopy.DestinationTableName = "[experian].[dbo].[Members]";
                        bulkCopy.BatchSize = data?.Count() ?? 0;
                        
                        bulkCopy.WriteToServer(dt);
                    }
                    connection.Close();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }
    }
}
