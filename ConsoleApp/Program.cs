using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using PDP_TestProject.Application.InputStyles;
using PDP_TestProject.Domain.Interfaces;
using PDP_TestProject.Domain.Services;
using PDP_TestProject.Infrastructure.Data;
using PDP_TestProject.Infrastructure.Files;


var configuration = new ConfigurationBuilder()
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

var serviceProvider = new ServiceCollection()
    .AddSingleton<IConfiguration>(configuration)
    .AddTransient<IFileReader, LocalFileReader>()
    .AddTransient<ITransactionParser, DefaultJsonStyleParser>()
    .AddTransient<ITransactionRepository, SqliteTransactionRepository>()
    .AddTransient<TransactionImportService>()
    .BuildServiceProvider();

var importService = serviceProvider.GetRequiredService<TransactionImportService>();
string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "input.json");


await importService.ImportAsync(filePath);
