﻿namespace PrimeNumber.Client.Helpers
{
    public static class DateTimeExtensions
    {
        // Convert datetime to Unix epoch
        public static long ToUnixTime(this DateTime dateTime)
        {
            DateTimeOffset dto = new DateTimeOffset(dateTime.ToUniversalTime());
            return dto.ToUnixTimeSeconds();
        }

        // Convert datetime to UNIX time including miliseconds
        public static long ToUnixTimeMilliSeconds(this DateTime dateTime)
        {
            DateTimeOffset dto = new DateTimeOffset(dateTime.ToUniversalTime());
            return dto.ToUnixTimeMilliseconds();
        }
    }
}
