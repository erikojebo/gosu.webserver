using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;

namespace Gosu.WebServer
{
    public class Request
    {
        public IList<HttpHeader> Headers { get; set; }
        public byte[] Body { get; set; }
    }

    public class HttpHeader
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}