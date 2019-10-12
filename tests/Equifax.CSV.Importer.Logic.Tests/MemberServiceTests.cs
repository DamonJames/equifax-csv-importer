using Equifax.CSV.Importer.Logic.Concrete;
using Equifax.CSV.Importer.Data.Abstract;
using Equifax.CSV.Importer.Models;
using System.Collections.Generic;
using System;
using Xunit;
using Moq;

namespace Equifax.CSV.Importer.Logic.Tests
{
    public class MemberServiceTests
    {
        private readonly Mock<IMemberRepository> _memberRepositoryMock = new Mock<IMemberRepository>();

        private MemberService _memberService;

        public MemberServiceTests()
        {
            _memberService = new MemberService(_memberRepositoryMock.Object);
        }

        [Fact]
        public async void GetMembersAsync_ReturnsListOfMembers()
        {
            _memberRepositoryMock.Setup(x => x.GetAsync()).ReturnsAsync(new List<Member>());

            var sut = await _memberService.GetMembersAsync();

            Assert.IsAssignableFrom<IEnumerable<Member>>(sut);
            Assert.NotNull(sut);
        }

        [Fact]
        public async void GetMembersAsync_ReturnsNull()
        {
            _memberRepositoryMock.Setup(x => x.GetAsync()).ReturnsAsync((IEnumerable<Member>)null);

            var sut = await _memberService.GetMembersAsync();

            Assert.Null(sut);
        }

        [Fact]
        public void PersistMembers_SuccessfulInsert_ReturnsSuccessModelWithTrue()
        {
            _memberRepositoryMock.Setup(x => x.AddMany(It.IsAny<IEnumerable<Member>>()));

            var sut = _memberService.PersistMembers(new List<Member>());

            Assert.IsType<SuccessModel>(sut);
            Assert.True(sut.Success);
        }

        [Fact]
        public void PersistMembers_UnsuccessfulInsert_ReturnsSuccessModelWithFalse()
        {
            _memberRepositoryMock.Setup(x => x.AddMany(It.IsAny<IEnumerable<Member>>())).Throws(new Exception());

            var sut = _memberService.PersistMembers(new List<Member>());

            Assert.IsType<SuccessModel>(sut);
            Assert.False(sut.Success);
        }
    }
}
