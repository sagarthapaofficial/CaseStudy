namespace LaptopShoppingSite.DAL.Helpers
{
    public class OrderDetailsHelper
    {
        public int OrderId { get; set; }
        public string ProductId { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public int QtySold { get; set; }
        public int QtyOrdered { get; set; }
        public int QtyBackOrdered { get; set; }
        public decimal SellingPrice { get; set; }
        public string OrderDate { get; set; }
    }
}

