using System;
using System.Collections.Generic;

namespace LaptopShoppingSite.DAL.DomainClasses
{
    public partial class Customer
    {
        public int Id { get; set; }
        public string Firstname { get; set; } = null!;
        public string Lastname { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Hash { get; set; } = null!;
        public string Salt { get; set; } = null!;
    }
}
