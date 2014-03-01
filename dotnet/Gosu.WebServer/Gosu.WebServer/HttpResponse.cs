using System.Text;

namespace Gosu.WebServer
{
    public class HttpResponse
    {
        public readonly HttpStatus HttpStatus;

        public HttpResponse(HttpStatus httpStatus)
        {
            HttpStatus = httpStatus;
        }

        public override string ToString()
        {
            var response = new StringBuilder();

            response.AppendFormat("HTTP/1.1 {0} {1}\r\n", HttpStatus.StatusCode, HttpStatus.Description);

            return response.ToString();
        }
    }
}