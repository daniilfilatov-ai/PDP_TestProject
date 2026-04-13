using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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
    .AddLogging(builder =>
    {
        builder.AddConsole(options =>
        {
            options.TimestampFormat = "[yyyy-MM-dd HH:mm:ss] ";
        });
        builder.SetMinimumLevel(LogLevel.Information);
    })
    .AddTransient<IFileReader, LocalFileReader>()
    .AddTransient<ITransactionParser, DefaultJsonStyleParser>()
    .AddTransient<ITransactionRepository, SqliteTransactionRepository>()
    .AddTransient<TransactionImportService>()
    .BuildServiceProvider();

var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

try
{
    var repository = serviceProvider.GetRequiredService<ITransactionRepository>();
    await repository.InitializeAsync();

    var importService = serviceProvider.GetRequiredService<TransactionImportService>();
    var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "input.json");


    await importService.ImportAsync(filePath);
    return 0;
}
catch (InvalidOperationException ex)
{
    logger.LogCritical(ex, "An error occurred during the import process: {Message}", ex.Message);
    return 1;
}
catch (Exception ex)
{
    logger.LogCritical(ex, "An unexpected error occurred: {Message}", ex.Message);
    return 2;
}
