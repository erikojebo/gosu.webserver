using System.IO;
using System.Text;

namespace Gosu.WebServer
{
    public static class StreamExtentions
    {
         public static string ReadUntilFirstBlankLineAfterContent(this Stream stream)
         {
             var sb = new StringBuilder();

             var consecutiveWhitespace = "";
             var hasReachedFirstNonWhitespaceContent = false;
             var buffer = new byte[1];

             while (true)
             {
                 var readBytes = stream.Read(buffer, 0, buffer.Length);

                 if (readBytes <= 0)
                     break;

                 var currentChar = Encoding.ASCII.GetString(buffer);

                 var isCurrentCharWhitespace = string.IsNullOrWhiteSpace(currentChar);

                 if (!hasReachedFirstNonWhitespaceContent && isCurrentCharWhitespace)
                     continue;
                 if (isCurrentCharWhitespace)
                 {
                     consecutiveWhitespace += currentChar;
                 }
                 else
                 {
                     hasReachedFirstNonWhitespaceContent = true;
                     consecutiveWhitespace = string.Empty;
                 }

                 if (IsDoubleLineBreak(consecutiveWhitespace))
                     break;

                 sb.Append(currentChar);
             }

             return sb.ToString();
         }

         private static bool IsDoubleLineBreak(string consecutiveWhitespace)
         {
             return consecutiveWhitespace.EndsWith("\n\n") ||
                    consecutiveWhitespace.EndsWith("\r\r") ||
                    consecutiveWhitespace.EndsWith("\r\n\r\n");
         }
    }
}