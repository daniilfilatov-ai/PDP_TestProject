namespace PDP_TestProject.Domain.Models;

public class Transaction
{
    public required string SellerId { get; set; }
    public decimal TotalPrice {  get; set; }
    public decimal? Change { get; set; }

    public List<TransactionItem> Items { get; set; } = [];
}
