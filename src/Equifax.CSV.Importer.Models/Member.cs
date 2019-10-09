using System;

namespace Equifax.CSV.Importer.Models
{
    public class Member
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int Height { get; set; }
        public double Balance { get; set; }
    }
}
