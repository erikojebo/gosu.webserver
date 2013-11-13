using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gosu.WebServer.Exceptions;
using NUnit.Framework;

namespace Gosu.WebServer.Specs
{
    [TestFixture]
    public class HttpRequestSpecs
    {
        [TestCase("GET /path/ HTTP/1.1", HttpRequestMethod.Get)]
        [TestCase("POST /path/ HTTP/1.1", HttpRequestMethod.Post)]
        [TestCase("PUT /path/ HTTP/1.1", HttpRequestMethod.Put)]
        [TestCase("DELETE /path/ HTTP/1.1", HttpRequestMethod.Delete)]
        [TestCase("CONNECT /path/ HTTP/1.1", HttpRequestMethod.Connect)]
        [TestCase("HEAD /path/ HTTP/1.1", HttpRequestMethod.Head)]
        [TestCase("OPTIONS /path/ HTTP/1.1", HttpRequestMethod.Options)]
        public void Parsing_request_with_only_start_line_sets_method(string requestString, HttpRequestMethod expectedMethod)
        {
            var request = HttpRequest.Parse(requestString);

            Assert.AreEqual(expectedMethod, request.Method);
        }
        
        [Test]
        public void Parsing_request_with_only_start_line_sets_path()
        {
            var request = HttpRequest.Parse("GET /path/ HTTP/1.1");

            Assert.AreEqual("/path/", request.Path);
        }
        
        [Test]
        public void Parsing_request_with_only_start_line_sets_protocol_version()
        {
            var request = HttpRequest.Parse("GET /path/ HTTP/1.1");

            Assert.AreEqual("1.1", request.ProtocolVersion);
        }

        [Test]
        [ExpectedException(typeof(InvalidHttpRequestException))]
        public void Parsing_invalid_request_throws_exception()
        {
            HttpRequest.Parse("invalid request");
        }
    }
}
