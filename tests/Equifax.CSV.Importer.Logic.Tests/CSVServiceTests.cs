using Equifax.CSV.Importer.Logic.Concrete;
using Equifax.CSV.Importer.Models;
using System.IO;
using Xunit;

namespace Equifax.CSV.Importer.Logic.Tests
{
    public class CSVServiceTests
    {
        private readonly MemoryStream _memoryStream = new MemoryStream();
        private CSVService _csvService = new CSVService();

        [Fact]
        public void ReadFile_SuccessfulConversion_ReturnsSuccessModelWithTrue()
        {
            var sut = _csvService.ReadFile(_memoryStream);

            Assert.IsType<CSVSuccessModel>(sut);
            Assert.True(sut.Success);
        }

        [Fact]
        public void ReadFile_FailedConversion_ReturnsSuccessModelWithFalse()
        {
            var sut = _csvService.ReadFile(null);

            Assert.IsType<CSVSuccessModel>(sut);
            Assert.False(sut.Success);
        }
    }
}
