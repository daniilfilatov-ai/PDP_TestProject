namespace PDP_TestProject.Models
{
    public class Transaction
    {
        public int? TransactionId { get; set; }
        public string? SellerID { get; set; }
        public decimal? TotalPrice { get; set; }
        public decimal? Change { get; set; }
        public DateOnly? TransactionDate { get; set; }
        public List<TransactionItem> Items { get; set; } = new List<TransactionItem>();
    }

    public class TransactionItem
    {
        public string? ItemName { get; set; }
        public string? ItemCode { get; set; }
        public int? Quantity { get; set; }
        public decimal? UnitPrice { get; set; }
    }
}
