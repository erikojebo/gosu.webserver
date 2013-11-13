namespace Gosu.WebServer
{
    public class HttpHeader
    {
        private HttpHeader(string name, string value)
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

        public static HttpHeader Parse(string headerLine)
        {
            var separatorIndex = headerLine.IndexOf(':');

            var name = headerLine.Substring(0, separatorIndex - 1);
            var value = headerLine.Substring(separatorIndex);

            return new HttpHeader(name, value);
        }
    }
}