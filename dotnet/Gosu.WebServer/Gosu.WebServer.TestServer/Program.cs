using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Gosu.WebServer.TestServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var thread = new Thread(StartServer);

            thread.Start();
            thread.Join();
        }

        private static void StartServer()
        {
            var server = new WebServer();
            server.Start();
        }
    }
}
