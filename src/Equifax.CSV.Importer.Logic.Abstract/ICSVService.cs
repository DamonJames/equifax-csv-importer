using Equifax.CSV.Importer.Models;
using System.IO;

namespace Equifax.CSV.Importer.Logic.Abstract
{
    public interface ICSVService
    {
        CSVReadSuccessModel ReadFile(Stream file);
    }
}
