using System;
using System.Globalization;

namespace Gosu.WebServer
{
    public static class DateTimeExtensions
    {
         public static string ToHttpStandardFormat(this DateTime dateTime)
         {
             var gmtDateTime = TimeZoneInfo.ConvertTimeToUtc(dateTime);

             return gmtDateTime.ToString("ddd, dd MMM yyyy HH:mm:ss", new CultureInfo("en-US")) + " GMT";
         }
    }
}