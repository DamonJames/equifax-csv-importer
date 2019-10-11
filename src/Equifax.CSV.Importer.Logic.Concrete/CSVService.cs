using Equifax.CSV.Importer.Logic.Abstract;
using Equifax.CSV.Importer.Data.Abstract;
using Equifax.CSV.Importer.Models;
using CsvHelper;
using System.IO;

namespace Equifax.CSV.Importer.Logic.Concrete
{
    public class CSVService : ICSVService
    {
        private readonly IMemberRepository _memberRepository;

        public CSVService(IMemberRepository memberRepository)
        {
            _memberRepository = memberRepository;
        }

        public CSVSuccessModel ReadFile(Stream file)
        {
            var model = new CSVSuccessModel { Success = false };

            TextReader reader = new StreamReader(file);
            var csvReader = new CsvReader(reader);

            try
            {
                model.Members = csvReader.GetRecords<Member>();
                model.Success = true;
                return model;
            }
            catch
            {
                model.Message = "Unable to read from the CSV file provided";
                return model;
            }
        }
    }
}
