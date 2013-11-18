using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Gosu.Commons.Extensions;
using Gosu.WebServer.Exceptions;
using System.Linq;

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
            var bytes = Encoding.ASCII.GetBytes(request);

            using (var stream = new MemoryStream(bytes))
            {
                return Parse(stream);
            }
        }

        public static HttpRequest Parse(Stream stream)
        {
            try
            {
                var headerString = stream.ReadUntilFirstBlankLineAfterContent();
                var headerLines = headerString.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

                var parsedRequest = new HttpRequest();

                ParseStartLine(headerLines[0], parsedRequest);
                ParseHeader(headerLines, parsedRequest);
                ParseBody(stream, parsedRequest);

                return parsedRequest;
            }
            catch (Exception e)
            {
                throw new InvalidHttpRequestException("An error occurred while parsing the request. The request is most likely malformed.", e);
            }
        }

        private static void ParseBody(Stream stream, HttpRequest parsedRequest)
        {
            var contentLengthHeader = parsedRequest.Headers.FirstOrDefault(x => x.Name.ToLower() == "content-length");

            if (contentLengthHeader == null)
            {
                return;
            }

            var contentLength = int.Parse(contentLengthHeader.Value);

            var body = new byte[contentLength];

            stream.Read(body, 0, body.Length);

            parsedRequest.Body = body;
        }

        private static void ParseHeader(string[] headerLines, HttpRequest parsedRequest)
        {
            var currentHeaderLines = new List<string>();

            for (int i = 1; i < headerLines.Length; i++)
            {
                var hasNextLineLeadingWhitespace = i < headerLines.Length - 1 &&
                                                   (headerLines[i + 1].StartsWith("\t") || headerLines[i + 1].StartsWith(" "));

                currentHeaderLines.Add(headerLines[i]);

                if (hasNextLineLeadingWhitespace)
                {
                    continue;
                }

                var currentHeaderString = string.Join(Environment.NewLine, currentHeaderLines);
                parsedRequest.Headers.Add(HttpHeader.Parse(currentHeaderString));
                currentHeaderLines.Clear();
            }
        }

        private static void ParseStartLine(string startLine, HttpRequest parsedRequest)
        {
            var startLineParts = startLine.Split(' ');
            var methodString = startLineParts[0];
            var protocolVersion = startLineParts[2].Split('/')[1];

            parsedRequest.Method = (HttpRequestMethod)Enum.Parse(typeof(HttpRequestMethod), methodString.Capitalize());
            parsedRequest.Path = startLineParts[1];
            parsedRequest.ProtocolVersion = protocolVersion;
        }
    }
}