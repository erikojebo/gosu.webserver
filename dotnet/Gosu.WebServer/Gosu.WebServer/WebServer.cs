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
                            var request = HttpRequest.Parse(stream);

                            Console.WriteLine(request);

                            const string responseBody = "har kommer bodyn";

                            var dateTime = DateTime.Now.ToHttpStandardFormat();
                            var responseString = string.Format("HTTP/0.9 200 OK\r\nSet-Cookie: foo=bar\r\nDate: {0}\r\nConnection: close\r\nContent-Length: {1}\r\n\r\n{2}", dateTime, responseBody.Length, responseBody);

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