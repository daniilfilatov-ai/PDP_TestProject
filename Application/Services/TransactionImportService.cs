using PDP_TestProject.Application.Interfaces;
using PDP_TestProject.Domain.Interfaces;

namespace PDP_TestProject.Application.Services;

public class TransactionImportService(
    IFileReader fileReader,
    ITransactionParser parser,
    ITransactionRepository repository)
{
    public async Task ImportAsync(string filePath)
    {
        var rawData = await fileReader.ReadTextAsync(filePath);

        var transactions = parser.Parse(rawData);

        await repository.SaveAsync(transactions);
        
    }
}
