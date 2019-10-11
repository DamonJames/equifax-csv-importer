using Equifax.CSV.Importer.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Equifax.CSV.Importer.Logic.Abstract
{
    public interface IMemberService
    {
        Task<IEnumerable<Member>> GetMembersAsync();
        SuccessModel PersistMembers(IEnumerable<Member> members);
    }
}
