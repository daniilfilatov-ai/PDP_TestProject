namespace PDP_TestProject.Domain.Models;

public class Transaction
{
    public required string SellerId { get; set; }
    public decimal TotalPrice => (decimal)Items.Sum(i => i.Quantity * i.UnitPrice);
    public decimal? Change { get; set; }

    public List<TransactionItem> Items { get; set; } = [];
}
