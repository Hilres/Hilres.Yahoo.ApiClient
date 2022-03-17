// <copyright file="HelperExtensions.cs" company="None">
// Free and open source code.
// </copyright>

namespace Hilres.Yahoo.ApiClient;

using System;

/// <summary>
/// For parsing data.
/// </summary>
internal static class HelperExtensions
{
    /// <summary>
    /// Convert date time offset to Unix time stamp.
    /// </summary>
    /// <param name="date">Date to convert.</param>
    /// <returns>Unix time stamp in string form.</returns>
    internal static string ToUnixTimestamp(this DateTimeOffset date)
    {
        return date.ToUnixTimeSeconds().ToString();
    }

    /// <summary>
    /// Convert date time to Unix time stamp.
    /// </summary>
    /// <param name="date">Date to convert.</param>
    /// <returns>Unix time stamp in string form.</returns>
    internal static string ToUnixTimestamp(this DateTime date)
    {
        return new DateTimeOffset(date).ToUnixTimestamp();
    }

    /// <summary>
    /// Convert date only to Unix time stamp.
    /// </summary>
    /// <param name="date">Date to convert.</param>
    /// <returns>Unix time stamp in string form.</returns>
    internal static string ToUnixTimestamp(this DateOnly date)
    {
        return date.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc).ToUnixTimestamp();
    }
}