namespace ExampleConsoleApp;

using Hilres.Yahoo.ApiClient;
using Microsoft.Extensions.Logging;

internal class Program
{
    private static async Task Main()
    {
        using var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder
                .AddFilter("Hilres", LogLevel.Trace)
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

        var parser = await client.GetPricesParserAsync("msft", new(2022, 1, 3), new(2022, 1, 8));
        logger.LogInformation("IsSuccessful = {IsSuccessful}  StatusCode = {StatusCode}", parser.IsSuccessful, parser.StatusCode);

        if (parser.IsSuccessful)
        {
            var prices = await parser.Prices
                    .Select(p => new MyPrice(p.Date, p.AdjClose, p.Volume))
                    .ToDictionaryAsync(p => p.Date)
                    .ConfigureAwait(false);

            Console.WriteLine(prices[new(2022, 1, 4)]);
            Console.WriteLine(prices[new(2022, 1, 6)]);
        }

        logger.LogInformation("End");
        Console.WriteLine("Done");
    }

    public record MyPrice(DateOnly Date, double? AdjClose, long? Volume);
}