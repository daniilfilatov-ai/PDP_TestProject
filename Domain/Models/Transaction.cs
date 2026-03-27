namespace PDP_TestProject.Domain.Models
{
    public class Transaction
    {
        public string SellerId { get; set; } = string.Empty;
        public decimal TotalPrice => (decimal)Items.Sum(i => i.Quantity * i.UnitPrice);
        public decimal? Change { get; set; }

        public List<TransactionItem> Items { get; set; } = [];
    }
}
