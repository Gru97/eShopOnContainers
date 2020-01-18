namespace Ordering.QueryModel
{
    public class OrderItem
    {
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Discount { get; set; }
    }
}