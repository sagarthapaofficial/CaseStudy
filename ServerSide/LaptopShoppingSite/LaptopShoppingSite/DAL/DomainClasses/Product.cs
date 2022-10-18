using System;
using System.Collections.Generic;

namespace LaptopShoppingSite.DAL.DomainClasses
{
    public partial class Product
    {

        public string Id { get; set; } = null!;
        public int BrandID { get; set; }
        public byte[]? Timer { get; set; }
        public string ProductName { get; set; } = null!;
        public string GraphicName { get; set; } = null!;
        public decimal costPrice { get; set; }
        public decimal MRSP { get; set; }
        public int QtyOnHand { get; set; }
        public int QtyOnBackOrder { get; set; }
        public string Description { get; set; } = null!;

    }
}
