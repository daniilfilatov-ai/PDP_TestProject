using System.Text.Json;
using PDP_TestProject.Application.Interfaces;
using PDP_TestProject.Domain.Models;

namespace PDP_TestProject.Application.InputStyles;

public class DefaultJsonStyleParser : ITransactionParser
{
    private readonly JsonSerializerOptions _options;
    public DefaultJsonStyleParser()
    {
        _options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }
    private class TransactionDto
    {
        public required string SellerId { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal Change { get; set; }
        public required List<TransactionItemDto> Items { get; set; }
    }

    private class TransactionItemDto
    {
        public required string ItemName { get; set; }
        public required string ItemCode { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }

    public IEnumerable<Transaction> Parse(string rawData)
    {
        if(string.IsNullOrWhiteSpace(rawData))
        {
            throw new InvalidOperationException("The input file is empty or contains no data");
        }

        var dtos = JsonSerializer.Deserialize<List<TransactionDto>>(rawData, _options);

        if (dtos == null || dtos.Count == 0)
        {
            throw new InvalidOperationException("The input file contains invalid JSON or no transactions");
        }

        return dtos.Select(dto => new Transaction
        {
            SellerId = dto.SellerId,
            TotalPrice = dto.Items.Sum(i => i.Quantity * i.UnitPrice),
            Change = dto.Change,
            Items = dto.Items.Select(itemDto => new TransactionItem
            {
                ItemName = itemDto.ItemName,
                ItemCode = itemDto.ItemCode,
                Quantity = itemDto.Quantity,
                UnitPrice = itemDto.UnitPrice
            }).ToList()
        });
    }
}
