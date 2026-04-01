using PDP_TestProject.Domain.Interfaces;
using PDP_TestProject.Domain.Models;

namespace PDP_TestProject.Domain.Services;

public class TransactionImportService(
    IFileReader fileReader,
    ITransactionParser parser,
    ITransactionRepository repository)
{
    public async Task ImportAsync(string filePath)
    {
        var rawData = await fileReader.ReadTextAsync(filePath);

        var transactions = parser.Parse(rawData).ToList();

        if (transactions.Count > 0)
        {
            await repository.SaveAsync(transactions);
        }
    }
}
