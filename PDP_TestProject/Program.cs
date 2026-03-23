using Microsoft.Extensions.DependencyInjection;
using PDP_TestProject.Implementations;
using PDP_TestProject.Interfaces;
using PDP_TestProject.Services;
using PDP_TestProject.Models;

var serviceProvider = new ServiceCollection()
    .AddTransient<IDataSource<string>, FileDataSource>()
    .AddTransient<IDataFormatter<string, Transaction>, TransactionFormatter>()
    .AddTransient<IDataRepository<Transaction>, SqliteDatabaseRepository>()
    .AddTransient<DataProcessingService<string, Transaction>>()
    .BuildServiceProvider();

var processingService = serviceProvider.GetRequiredService<DataProcessingService<string, Transaction>>();

string rootFolder = AppDomain.CurrentDomain.BaseDirectory;
string filePath = Path.Combine(rootFolder, "input.txt");

if (!File.Exists(filePath))
{
    throw new FileNotFoundException($"The file at path {filePath} was not found.");
}

processingService.Process(filePath);

Console.WriteLine("Operation success");