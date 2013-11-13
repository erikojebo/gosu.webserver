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
                            //using (var reader = new StreamReader(stream))
                            using (var writer = new StreamWriter(stream))
                            {
                                string[] headerLines;

                                var buffer = new byte[1];
                                var request = string.Empty;

                                while (true)
                                {
                                    stream.Read(buffer, 0, buffer.Length);

                                    request += Encoding.ASCII.GetString(buffer);

                                    if (request.EndsWith("\r\n\r\n") || request.EndsWith("\n\n"))
                                    {
                                        headerLines = request.Split('\n');

                                        for (int i = 0; i < headerLines.Length; i++)
                                        {
                                            headerLines[i] = headerLines[i].Trim();
                                        }

                                        break;
                                    }
                                }

                                Console.WriteLine(string.Join(Environment.NewLine, headerLines));

                                var contentLengthHeader = headerLines.FirstOrDefault(x => x.ToLower().StartsWith("content-length:"));

                                if (contentLengthHeader != null)
                                {
                                    var contentLength = int.Parse(contentLengthHeader.Split(':')[1]);

                                    var bodyBuffer = new byte[contentLength];

                                    stream.Read(bodyBuffer, 0, bodyBuffer.Length);

                                    var body = Encoding.ASCII.GetString(bodyBuffer);

                                    Console.WriteLine(body);
                                }

                                var bodyString = "har kommer bodyn";

                                writer.Write(string.Format("HTTP/0.9 200 OK\r\nSet-Cookie: foo=bar\r\nDate: {0}\r\nContent-Length: {1}\r\n\r\n{2}", DateTime.Now, bodyString.Length, bodyString));
                            }
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