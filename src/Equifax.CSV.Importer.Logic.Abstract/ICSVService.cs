using Equifax.CSV.Importer.Models;
using System.IO;

namespace Equifax.CSV.Importer.Logic.Abstract
{
    public interface ICSVService
    {
        CSVSuccessModel ReadFile(Stream file);
    }
}
