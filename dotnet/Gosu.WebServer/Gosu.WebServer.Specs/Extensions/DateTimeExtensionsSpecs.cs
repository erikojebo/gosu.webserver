using System;
using NUnit.Framework;

namespace Gosu.WebServer.Specs.Extensions
{
    [TestFixture]
    public class DateTimeExtensionsSpecs
    {
        [Test]
        public void ToHttpStandardFormat_follows_the_HTTP_standard_format()
        {
            var dateTime = new DateTime(1999, 12, 31, 23, 59, 59);
            var utcOffset = TimeZone.CurrentTimeZone.GetUtcOffset(dateTime);

            // The ToString should show GMT, so make sure to pass in a date time
            // that is adjusted so that the GMT adjusted version of it is the date time
            // above
            var dateTimeInLocalTimeZone = dateTime.Add(utcOffset);

            Assert.AreEqual("Fri, 31 Dec 1999 23:59:59 GMT", dateTimeInLocalTimeZone.ToHttpStandardFormat());
        } 
    }
}