using Equifax.CSV.Importer.Models;
using System.Collections.Generic;

namespace Equifax.CSV.Importer.Data.Abstract
{
    public interface IMemberRepository
    {
        void AddMany(IEnumerable<Member> data);
    }
}
