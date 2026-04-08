using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using PDP_TestProject.Application.InputStyles;
using PDP_TestProject.Application.Services;
using PDP_TestProject.Application.Interfaces;
using PDP_TestProject.Domain.Interfaces;
using PDP_TestProject.Infrastructure.Data;
using PDP_TestProject.Infrastructure.Files;


var configuration = new ConfigurationBuilder()
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

var serviceProvider = new ServiceCollection()
    .Configure<DatabaseOptions>(configuration.GetSection("Database"))
    .AddTransient<IFileReader, LocalFileReader>()
    .AddTransient<ITransactionParser, DefaultJsonStyleParser>()
    .AddTransient<ITransactionRepository, SqliteTransactionRepository>()
    .AddTransient<TransactionImportService>()
    .BuildServiceProvider();

var repository = serviceProvider.GetRequiredService<ITransactionRepository>();
await repository.InitializeAsync();

var importService = serviceProvider.GetRequiredService<TransactionImportService>();
var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "input.json");


await importService.ImportAsync(filePath);
