namespace ExampleConsoleApp;

using System.Reflection;
using Hilres.Yahoo.ApiClient;
using Microsoft.Extensions.Logging;

internal class Program
{
    private static async Task Main()
    {
        using var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder
                .AddFilter("Hilres", LogLevel.Warning)
                .AddFilter("Microsoft", LogLevel.Warning)
                .AddFilter("System", LogLevel.Warning)
                .AddConsole();
        });
        ILogger logger = loggerFactory.CreateLogger<Program>();

        logger.LogInformation("Start");

        var client = new YahooClient(loggerFactory.CreateLogger<YahooClient>());

        var result = await client.GetPricesAsync("msft", new(2022, 1, 3), new(2022, 1, 8));
        logger.LogInformation("IsSuccessful = {IsSuccessful}  StatusCode = {StatusCode}", result.IsSuccessful, result.StatusCode);

        if (result.IsSuccessful)
        {
            foreach (var item in result.Prices)
            {
                Console.WriteLine(item);
            }
        }

        await DisplayAllSP500(client);

        logger.LogInformation("End");
        Console.WriteLine("Done");
    }

    private static async Task DisplayAllSP500(YahooClient client)
    {
        using var stream = Assembly
                                .GetExecutingAssembly()
                                .GetManifestResourceStream("ExampleConsoleApp.SP500SymbolsAsOf20221025.txt")
                                ?? throw new NullReferenceException();

        using var reader = new StreamReader(stream);

        string? symbol;
        while ((symbol = reader.ReadLine()) != null)
        {
            await DisplayStockPrice(client, symbol);
        }
    }

    private static async Task DisplayStockPrice(YahooClient client, string symbol)
    {
        var parser = await client.GetPricesParserAsync(symbol, new(2022, 1, 3), new(2022, 1, 8));
        Console.WriteLine($"{symbol,-7} IsSuccessful = {parser.IsSuccessful}  StatusCode = {parser.StatusCode}");

        if (parser.IsSuccessful)
        {
            await foreach (var item in parser.Prices.ConfigureAwait(false))
            {
                Console.WriteLine($"        {item.Date} {item.AdjClose} {item.Volume}");
            }
        }
    }

    public record MyPrice(DateOnly Date, double? AdjClose, long? Volume);
}