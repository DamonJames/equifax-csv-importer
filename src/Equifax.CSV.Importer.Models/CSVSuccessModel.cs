﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Equifax.CSV.Importer.Models
{
    public class CSVSuccessModel : SuccessModel
    {
        public IEnumerable<Member> Members { get; set; }
    }
}
