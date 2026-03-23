using PDP_TestProject.Interfaces;
using PDP_TestProject.Models;

namespace PDP_TestProject.Implementations
{
    public class TransactionFormatter : IDataFormatter<string, Transaction>
    {
        public Transaction Format (string inputData)
        {
            var transaction = new Transaction();
            TransactionItem? currentItem = null;

            var pairs = inputData.Split([',', '\n', '\r'], StringSplitOptions.RemoveEmptyEntries);

            foreach (var pair in pairs)
            {
                var parts = pair.Split(':', 2);
                if (parts.Length != 2) continue;

                var key = parts[0].Trim().ToLower();
                var value = parts[1].Trim();

                if(key == "transactionid" && int.TryParse(value, out int ti))
                {
                    transaction.TransactionId = ti;
                }
                else if (key == "sellerid")
                {
                    transaction.SellerID = value;
                }
                else if(key == "totalprice" && decimal.TryParse(value, out decimal tp))
                {
                    transaction.TotalPrice = tp;
                }
                else if (key == "change" && decimal.TryParse(value, out decimal c)) 
                {
                    transaction.Change = c;
                }
                else if (key == "transactiondate" && DateOnly.TryParse(value, out DateOnly dt))
                {
                    transaction.TransactionDate = dt;
                }
                else if (key == "item")
                {
                    if (currentItem != null)
                    {
                        transaction.Items.Add(currentItem);
                    }
                    currentItem = new TransactionItem { ItemName = value };
                }
                else if (currentItem != null)
                {
                    if (key == "itemcode")
                    {
                        currentItem.ItemCode = value;
                    }
                    else if (key == "quantity" && int.TryParse(value, out int q))
                    {
                        currentItem.Quantity = q;
                    }
                    else if (key == "unitprice" && decimal.TryParse(value, out decimal up))
                    {
                        currentItem.UnitPrice = up;
                    }
                }
            }
            if(currentItem != null)
            {
                transaction.Items.Add(currentItem);
            }

            return transaction;
            
        }
    }
}
