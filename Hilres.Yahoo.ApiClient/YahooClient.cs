// <copyright file="YahooClient.cs" company="None">
// Free and open source code.
// </copyright>

namespace Hilres.Yahoo.ApiClient;

using Microsoft.Extensions.Logging;

/// <summary>
/// Yahoo client class.
/// </summary>
public sealed partial class YahooClient
{
    private readonly HttpClient apiHttpClient = new();
    private readonly ILogger? logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="YahooClient"/> class.
    /// </summary>
    /// <param name="logger">ILogger.</param>
    public YahooClient(ILogger? logger = null)
    {
        this.logger = logger;

        const string UserAgent = "user-agent";
        const string UserAgentText = "Mozilla/5.0 (X11; U; Linux i686) Gecko/20071127 Firefox/2.0.0.11";

        this.apiHttpClient.DefaultRequestHeaders.Add(UserAgent, UserAgentText);
    }

    /// <summary>
    /// Gets the current eastern time.
    /// </summary>
    public static DateTimeOffset EasternTimeNow => TimeZoneInfo.ConvertTime(DateTimeOffset.Now, Constant.EasternTimeZone);

    /// <summary>
    /// Convert the interval into a string.
    /// </summary>
    /// <param name="interval">StockInterval.</param>
    /// <returns>string.</returns>
    private static string ToIntervalString(YahooInterval? interval) => interval switch
    {
        null => "1d",
        YahooInterval.Daily => "1d",
        YahooInterval.Weekly => "1wk",
        YahooInterval.Monthly => "1mo",
        YahooInterval.Quorterly => "3mo",
        _ => throw new NotImplementedException(interval.ToString()),
    };
}