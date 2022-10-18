using System;
using System.Collections.Generic;

namespace LaptopShoppingSite.DAL.DomainClasses
{
    public partial class Branch
    {
        public int Id { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? Region { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
    }
}
