using Equifax.CSV.Importer.Logic.Abstract;
using Equifax.CSV.Importer.Data.Abstract;
using Equifax.CSV.Importer.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Equifax.CSV.Importer.Logic.Concrete
{
    public class MemberService : IMemberService
    {
        private readonly IMemberRepository _memberRepository;

        public MemberService(IMemberRepository memberRepository)
        {
            _memberRepository = memberRepository;
        }

        public Task<IEnumerable<Member>> GetMembersAsync()
        {
            return _memberRepository.GetAsync();
        }

        public SuccessModel PersistMembers(IEnumerable<Member> members)
        {
            try
            {
                _memberRepository.AddMany(members);
                return new SuccessModel { Success = true };
            }
            catch
            {
                return new SuccessModel
                {
                    Success = false,
                    Message = "Unable to persist members to the database"
                };
            }
        }
    }
}
