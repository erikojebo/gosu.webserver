using System;
using System.Linq;

namespace Gosu.WebServer
{
    public class HttpHeader
    {
        public HttpHeader(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public readonly string Name;
        public readonly string Value;

        public override string ToString()
        {
            return string.Format("{0}: {1}", Name, Value);
        }

        public override int GetHashCode()
        {
            return (Name + Value).GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = obj as HttpHeader;

            if (other == null)
                return false;

            return Name == other.Name &&
                   Value == other.Value;
        }

        public static HttpHeader Parse(string headerLine)
        {
            var separatorIndex = headerLine.IndexOf(':');

            var name = headerLine.Substring(0, separatorIndex);
            var value = headerLine.Substring(separatorIndex + 1);

            var valueLines = value.Split(new [] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim());

            var concatenatedValuestring = string.Join(" ", valueLines);

            return new HttpHeader(name, concatenatedValuestring);
        }
    }
}