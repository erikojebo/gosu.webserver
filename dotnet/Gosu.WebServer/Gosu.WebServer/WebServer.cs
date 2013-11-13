using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using System.Text;

namespace Gosu.WebServer
{
    public class WebServer
    {
        public void Start(int port = 16161)
        {
            var listener = new TcpListener(IPAddress.Parse("127.0.0.1"), port);

            listener.Start();

            while (true)
            {
                try
                {
                    using (var client = listener.AcceptTcpClient())
                    {
                        using (var stream = client.GetStream())
                        {
                            string[] headerLines;

                            var buffer = new byte[1];
                            var requestString = string.Empty;

                            var request = new HttpRequest();

                            while (true)
                            {
                                stream.Read(buffer, 0, buffer.Length);

                                requestString += Encoding.ASCII.GetString(buffer);

                                if (requestString.EndsWith("\r\n\r\n") || requestString.EndsWith("\n\n"))
                                {
                                    headerLines = requestString.Split('\n');

                                    for (int i = 1; i < headerLines.Length; i++)
                                    {
                                        if (string.IsNullOrWhiteSpace(headerLines[i]))
                                            continue;

                                        request.Headers.Add(HttpHeader.Parse(headerLines[i]));
                                        headerLines[i] = headerLines[i].Trim();
                                    }

                                    break;
                                }
                            }

                            var contentLengthHeader = headerLines.FirstOrDefault(x => x.ToLower().StartsWith("content-length:"));

                            if (contentLengthHeader != null)
                            {
                                var contentLength = int.Parse(contentLengthHeader.Split(':')[1]);

                                var bodyBuffer = new byte[contentLength];

                                stream.Read(bodyBuffer, 0, bodyBuffer.Length);

                                request.Body = bodyBuffer;

                                Console.WriteLine(request);
                            }

                            const string responseBody = "har kommer bodyn";

                            var responseString = string.Format("HTTP/0.9 200 OK\r\nSet-Cookie: foo=bar\r\nDate: {0}\r\nContent-Length: {1}\r\n\r\n{2}", DateTime.Now, responseBody.Length, responseBody);

                            var responseBytes = Encoding.ASCII.GetBytes(responseString);

                            stream.Write(responseBytes, 0, responseBytes.Length);
                        }

                        client.Close();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }
    }
}