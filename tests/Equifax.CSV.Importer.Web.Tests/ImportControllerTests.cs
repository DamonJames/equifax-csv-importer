using Equifax.CSV.Importer.Web.Controllers;
using Equifax.CSV.Importer.Logic.Abstract;
using Equifax.CSV.Importer.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AutoFixture;
using System.IO;
using Xunit;
using Moq;

namespace Equifax.CSV.Importer.Web.Tests
{
    public class ImportControllerTests
    {
        private readonly Mock<ICSVService> _csvServiceMock = new Mock<ICSVService>();
        private readonly Mock<IMemberService> _memberServiceMock = new Mock<IMemberService>();
        private readonly Mock<IFormFile> _formFileMock = new Mock<IFormFile>();

        private readonly MemoryStream _memoryStream = new MemoryStream();

        private ImportController _importController;

        public ImportControllerTests()
        {
            _importController = new ImportController(_csvServiceMock.Object, _memberServiceMock.Object);
        }

        [Fact]
        public async void Index_NoMembersReturned_ReturnsView()
        {
            _memberServiceMock.Setup(x => x.GetMembersAsync()).ReturnsAsync((IEnumerable<Member>)null);

            var sut = await _importController.Index();

            Assert.IsType<ViewResult>(sut);
        }

        [Fact]
        public async void Index_MembersReturned_ReturnsView()
        {
            var fixture = new Fixture();

            var members = fixture.CreateMany<Member>();

            _memberServiceMock.Setup(x => x.GetMembersAsync()).ReturnsAsync(members);

            var sut = await _importController.Index();

            Assert.IsType<ViewResult>(sut);
        }

        [Fact]
        public void Submit_NullFile_ReturnsIndexWithError()
        {
            var sut = _importController.Submit(null);
            
            Assert.IsType<ViewResult>(sut);

            Assert.True(_importController.ModelState.ErrorCount == 1);
        }

        [Fact]
        public void Submit_WrongFileFormat_ReturnsIndexWithError()
        {
            _formFileMock.Setup(x => x.FileName).Returns("abc.docx");
            _formFileMock.Setup(x => x.Length).Returns(2);

            var sut = _importController.Submit(_formFileMock.Object);

            Assert.IsType<ViewResult>(sut);

            Assert.True(_importController.ModelState.ErrorCount == 1);
        }

        [Fact]
        public void Submit_FailedConversion_ReturnsIndexWithError()
        {
            _formFileMock.Setup(x => x.FileName).Returns("abc.csv");
            _formFileMock.Setup(x => x.Length).Returns(2);
            _formFileMock.Setup(x => x.OpenReadStream()).Returns(_memoryStream);

            _csvServiceMock.Setup(x => x.ReadFile(_memoryStream)).Returns(new CSVSuccessModel { Success = false, Message = "Failed" });
            
            var sut = _importController.Submit(_formFileMock.Object);

            Assert.IsType<ViewResult>(sut);

            Assert.True(_importController.ModelState.ErrorCount == 1);
        }

        [Fact]
        public void Submit_FailedDatebaseInsert_ReturnsIndexWithError()
        {
            _formFileMock.Setup(x => x.FileName).Returns("abc.csv");
            _formFileMock.Setup(x => x.Length).Returns(2);
            _formFileMock.Setup(x => x.OpenReadStream()).Returns(_memoryStream);

            _csvServiceMock.Setup(x => x.ReadFile(_memoryStream)).Returns(new CSVSuccessModel { Success = true, Members = new List<Member>() });
            _memberServiceMock.Setup(x => x.PersistMembers(It.IsAny<IEnumerable<Member>>())).Returns(new SuccessModel { Success = false, Message = "Failed" });

            var sut = _importController.Submit(_formFileMock.Object);

            Assert.IsType<ViewResult>(sut);

            Assert.True(_importController.ModelState.ErrorCount == 1);
        }

        [Fact]
        public void Submit_SuccessfulConversionAndInsert_ReturnsIndexWithNoError()
        {
            _formFileMock.Setup(x => x.FileName).Returns("abc.csv");
            _formFileMock.Setup(x => x.Length).Returns(2);
            _formFileMock.Setup(x => x.OpenReadStream()).Returns(_memoryStream);

            _csvServiceMock.Setup(x => x.ReadFile(_memoryStream)).Returns(new CSVSuccessModel { Success = true, Members = new List<Member>() });
            _memberServiceMock.Setup(x => x.PersistMembers(It.IsAny<IEnumerable<Member>>())).Returns(new SuccessModel { Success = true });

            var sut = _importController.Submit(_formFileMock.Object);

            Assert.IsType<RedirectToActionResult>(sut);

            Assert.True(_importController.ModelState.ErrorCount == 0);
        }
    }
}
