using System;
using System.Collections.Generic;

namespace LaptopShoppingSite.DAL.DomainClasses
{
    public partial class Brand
    {
        public Brand()
        {
            Products = new HashSet<Product>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public byte[]? Timer { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
