using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;
using PDP_TestProject.Domain.Interfaces;
using PDP_TestProject.Domain.Models;

namespace PDP_TestProject.Infrastructure.Data;

public class SqliteTransactionRepository : ITransactionRepository
{
    private readonly string _connectionString;

    public SqliteTransactionRepository(IOptions<DatabaseOptions> options)
    {
        _connectionString = options.Value.ConnectionString;
        
        InitializeDatabaseAsync().GetAwaiter().GetResult();
    }

    private async Task InitializeDatabaseAsync()
    {
        await using var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync();

        await using var command = connection.CreateCommand();
        command.CommandText = @"
                DROP TABLE IF EXISTS TransactionItems;
                DROP TABLE IF EXISTS Transactions;

                CREATE TABLE IF NOT EXISTS Transactions (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    SellerId TEXT,
                    TotalPrice REAL,
                    Change REAL);
                CREATE TABLE IF NOT EXISTS TransactionItems (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    TransactionId INTEGER,
                    ItemName TEXT,
                    ItemCode TEXT,
                    Quantity INTEGER,
                    UnitPrice REAL,
                    FOREIGN KEY(TransactionId) REFERENCES Transactions(Id));
                ";

        await command.ExecuteNonQueryAsync();
    }

    public async Task SaveAsync(IEnumerable<Transaction> transactions)
    {
        var list = transactions.ToList();
        if (list.Count == 0) return;

        await using var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync();
        await using var tx = (SqliteTransaction) await connection.BeginTransactionAsync();

        var transParams = list.SelectMany((t,i) => new[]
        {
            new SqliteParameter($"@SellerId{i}", t.SellerId),
            new SqliteParameter($"@TotalPrice{i}", t.TotalPrice),
            new SqliteParameter($"@Change{i}", t.Change ?? (object)DBNull.Value)
        }).ToArray();

        var transValues = string.Join(", ", list.Select((t, i) => $"(@SellerId{i}, @TotalPrice{i}, @Change{i})"));
        var transCmd = connection.CreateCommand();
        transCmd.Transaction = tx;
        transCmd.CommandText = $"INSERT INTO Transactions (SellerId, TotalPrice, Change) VALUES {transValues}; SELECT last_insert_rowid();";
        transCmd.Parameters.AddRange(transParams);

        var isertedIds = new List<long>();
        await using var reader = await transCmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            isertedIds.Add(reader.GetInt64(0));
        }

        var itemsWithTransId = list.Zip(isertedIds, (t, id) => new { Trans = t, TransId = id })
            .SelectMany(x => x.Trans.Items.Select(i => new { Item = i, TransId =  x.TransId}))
            .ToList();

        if(itemsWithTransId.Count > 0)
        {
            var itemParams = itemsWithTransId.SelectMany((t, i) => new[]
            {
                new SqliteParameter($"@TransactionId{i}", t.TransId),
                new SqliteParameter($"@ItemName{i}", t.Item.ItemName),
                new SqliteParameter($"@ItemCode{i}", t.Item.ItemCode),
                new SqliteParameter($"@Quantity{i}", t.Item.Quantity),
                new SqliteParameter($"@UnitPrice{i}", t.Item.UnitPrice)
            }).ToArray();

            var itemValues = string.Join(", ", itemsWithTransId.Select((_, i) => $"(@TransactionId{i}, @ItemName{i}, @ItemCode{i}, @Quantity{i}, @UnitPrice{i})"));
            var itemCmd = connection.CreateCommand();
            itemCmd.Transaction = tx;
            itemCmd.CommandText = $"INSERT INTO TransactionItems (TransactionId, ItemName, ItemCode, Quantity, UnitPrice) VALUES {itemValues};";
            itemCmd.Parameters.AddRange(itemParams);

            await itemCmd.ExecuteNonQueryAsync();
        }
        await tx.CommitAsync();
    }
}
