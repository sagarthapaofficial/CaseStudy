using System;
using System.Collections.Generic;

namespace LaptopShoppingSite.DAL.DomainClasses
{
    public partial class Order
    {
        public Order()
        {
            OrderLineItems = new HashSet<OrderLineItem>();
        }

        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal OrderAmount { get; set; }
        public int UserId { get; set; }

        public virtual ICollection<OrderLineItem> OrderLineItems { get; set; }
    }
}
