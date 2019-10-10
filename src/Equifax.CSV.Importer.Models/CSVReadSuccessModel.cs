using System;
using System.Collections.Generic;
using System.Text;

namespace Equifax.CSV.Importer.Models
{
    public class CSVReadSuccessModel
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public IEnumerable<Member> Members { get; set; }
    }
}
