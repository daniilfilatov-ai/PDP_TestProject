using Microsoft.Extensions.Logging;
using PDP_TestProject.Application.Interfaces;
using PDP_TestProject.Domain.Interfaces;

namespace PDP_TestProject.Application.Services;

public class TransactionImportService(
    IFileReader fileReader,
    ITransactionParser parser,
    ITransactionRepository repository,
    ILogger <TransactionImportService> logger)
{
    public async Task ImportAsync(string filePath)
    {
        logger.LogInformation("Starting import of transactions from file: {FilePath}", filePath);

        var rawData = await fileReader.ReadTextAsync(filePath);

        var transactions = parser.Parse(rawData);

        logger.LogInformation("Parsed {Count} transactions from file: {FilePath}", transactions.Count(), filePath);
        await repository.SaveAsync(transactions);

        logger.LogInformation("Successfully imported transactions from file: {FilePath}", filePath);

    }
}
