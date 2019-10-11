using Equifax.CSV.Importer.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Equifax.CSV.Importer.Data.Abstract
{
    public interface IMemberRepository
    {
        void AddMany(IEnumerable<Member> data);
        Task<IEnumerable<Member>> GetAsync();
    }
}
