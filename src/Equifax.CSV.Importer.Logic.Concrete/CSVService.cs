using Equifax.CSV.Importer.Logic.Abstract;
using Equifax.CSV.Importer.Models;
using System.Threading.Tasks;
using CsvHelper;
using System.IO;

namespace Equifax.CSV.Importer.Logic.Concrete
{
    public class CSVService : ICSVService
    {
        public async Task ReadFile(string file)
        {
            TextReader reader = new StreamReader("");
            var csvReader = new CsvReader(reader);
            var records = csvReader.GetRecords<Member>();
        }
    }
}
