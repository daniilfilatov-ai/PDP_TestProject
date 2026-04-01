using System.Text.Json;
using PDP_TestProject.Domain.Interfaces;
using PDP_TestProject.Domain.Models;

namespace PDP_TestProject.Application.InputStyles;

public class DefaultJsonStyleParser : ITransactionParser
{
    private class TransactionDto
    {
        public string? SellerId { get; set; }
        public decimal? Change { get; set; }
        public List<TransactionItemDto>? Items { get; set; }
    }

    private class TransactionItemDto
    {
        public string? ItemName { get; set; }
        public string? ItemCode { get; set; }
        public int? Quantity { get; set; }
        public decimal? UnitPrice { get; set; }
    }

    public IEnumerable<Transaction> Parse(string rawData)
    {
        if(string.IsNullOrWhiteSpace(rawData))
        {
            return Enumerable.Empty<Transaction>();
        }

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var dtos = JsonSerializer.Deserialize<List<TransactionDto>>(rawData, options);

        if (dtos == null)
        {
            return Enumerable.Empty<Transaction>();
        }

        var domainTransactions = new List<Transaction>();

        foreach (var dto in dtos)
        {
            var transaction = new Transaction
            {
                SellerId = dto.SellerId ?? string.Empty,
                Change = dto.Change
            };

            if(dto.Items != null)
            {
                foreach (var itemDto in dto.Items)
                {
                    transaction.Items.Add(new TransactionItem
                    {
                        ItemName = itemDto.ItemName ?? string.Empty,
                        ItemCode = itemDto.ItemCode ?? string.Empty,
                        Quantity = itemDto.Quantity ?? 0,
                        UnitPrice = itemDto.UnitPrice ?? 0m
                    });
                }
            }
            domainTransactions.Add(transaction);
        }
        return domainTransactions;
    }
}
