using System;
using System.Collections.Generic;
using System.Text;
using Gosu.Commons.Extensions;
using Gosu.WebServer.Exceptions;

namespace Gosu.WebServer
{
    public class HttpRequest
    {
        public HttpRequest()
        {
            Headers = new List<HttpHeader>();
        }

        public IList<HttpHeader> Headers { get; set; }
        public HttpRequestMethod Method { get; set; }
        public byte[] Body { get; set; }
        public string Path { get; set; }
        public string ProtocolVersion { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();

            foreach (var httpHeader in Headers)
            {
                sb.AppendLine(httpHeader.ToString());
            }

            sb.AppendLine();

            sb.AppendLine(Encoding.ASCII.GetString(Body));

            return sb.ToString();
        }

        public static HttpRequest Parse(string request)
        {
            try
            {
                var startLineParts = request.Split(' ');
                var methodString = startLineParts[0];
                var protocolVersion = startLineParts[2].Split('/')[1];

                var parsedRequest = new HttpRequest();

                parsedRequest.Method = (HttpRequestMethod)Enum.Parse(typeof(HttpRequestMethod), methodString.Capitalize());
                parsedRequest.Path = startLineParts[1];
                parsedRequest.ProtocolVersion = protocolVersion;

                return parsedRequest;
            }
            catch (Exception e)
            {
                throw new InvalidHttpRequestException("An error occurred while parsing the request. The request is most likely malformed.", e);
            }
        }
    }
}