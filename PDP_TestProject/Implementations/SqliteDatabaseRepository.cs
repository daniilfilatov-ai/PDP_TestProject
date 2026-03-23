using Microsoft.Data.Sqlite;
using PDP_TestProject.Interfaces;
using PDP_TestProject.Models;

namespace PDP_TestProject.Implementations
{
    public class SqliteDatabaseRepository : IDataRepository<Transaction>
    {
        private readonly string _connectionString = "Data Source=output.db";

        public SqliteDatabaseRepository()
        {
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var command = connection.CreateCommand();

            command.CommandText =
            @"
                CREATE TABLE IF NOT EXISTS Transactions (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    SellerID TEXT,
                    TotalPrice REAL,
                    Change REAL,
                    TransactionDate TEXT
                );
                
                CREATE TABLE IF NOT EXISTS TransactionItems (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    TransactionId INTEGER,
                    ItemName TEXT,
                    ItemCode TEXT,
                    Quantity INTEGER,
                    UnitPrice REAL,
                    FOREIGN KEY (TransactionId) REFERENCES Transactions(Id)
                );

            ";
            command.ExecuteNonQuery();
        }

        public void SaveData(IEnumerable<Transaction> data)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            using var transaction = connection.BeginTransaction();

            foreach (var trans in data)
            {
                var cmdTrans = connection.CreateCommand();
                cmdTrans.Transaction = transaction;

                cmdTrans.CommandText =
                @"
                    INSERT INTO Transactions (SellerID, TotalPrice, Change, TransactionDate)
                    VALUES ($sellerID, $totalPrice, $change, $transactionDate);
                    SELECT last_insert_rowid();
                ";
                cmdTrans.Parameters.AddWithValue("$sellerID", trans.SellerID ?? (object)DBNull.Value);
                cmdTrans.Parameters.AddWithValue("$totalPrice", trans.TotalPrice ?? (object)DBNull.Value);
                cmdTrans.Parameters.AddWithValue("$change", trans.Change ?? (object)DBNull.Value);
                cmdTrans.Parameters.AddWithValue("$transactionDate", trans.TransactionDate ?? (object)DBNull.Value);

                long newTransactionId = (long)cmdTrans.ExecuteScalar()!;

                foreach (var item in trans.Items)
                {
                    var cmdItem = connection.CreateCommand();
                    cmdItem.Transaction = transaction;
                    cmdItem.CommandText =
                        @"
                            INSERT INTO TransactionItems (TransactionId, ItemName, ItemCode, Quantity, UnitPrice)
                            VALUES ($transactionId, $itemName, $itemCode, $quantity, $unitPrice);
                        ";
                    cmdItem.Parameters.AddWithValue("$transactionId", newTransactionId);
                    cmdItem.Parameters.AddWithValue("$itemName", item.ItemName ?? (object)DBNull.Value);
                    cmdItem.Parameters.AddWithValue("$itemCode", item.ItemCode ?? (object)DBNull.Value);
                    cmdItem.Parameters.AddWithValue("$quantity", item.Quantity ?? (object)DBNull.Value); 
                    cmdItem.Parameters.AddWithValue("$unitPrice", item.UnitPrice ?? (object)DBNull.Value);

                    cmdItem.ExecuteNonQuery();
                }
            }
            transaction.Commit();
        }

    }
}
