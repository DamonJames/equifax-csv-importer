using System.Data;

namespace Equifax.CSV.Importer.Data.Abstract
{
    public interface IConnectionFactory
    {
        IDbConnection CreateLocalConnection();
    }
}
