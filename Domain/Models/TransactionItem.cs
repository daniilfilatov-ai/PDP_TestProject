namespace PDP_TestProject.Domain.Models
{
    public class TransactionItem
    {
        public string ItemName { get; set; } = string.Empty;
        public string ItemCode { get; set; } = string.Empty;    
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
