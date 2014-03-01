using System.Net;
using NUnit.Framework;

namespace Gosu.WebServer.Specs
{
    [TestFixture]
    public class HttpResponseSpecs
    {
        [Test]
        public void First_line_is_http_version_followed_by_status_code_and_description()
        {
            var response = new HttpResponse(HttpStatus.OK);

            var responseString = response.ToString();

            Assert.IsTrue(responseString.StartsWith("HTTP/1.1 200 OK\r\n"));
        }
    }
}