namespace PDP_TestProject.Domain.Models;

public class TransactionItem
{
    public required string ItemName { get; set; } 
    public required string ItemCode { get; set; } 
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
