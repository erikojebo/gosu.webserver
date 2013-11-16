using NUnit.Framework;

namespace Gosu.WebServer.Specs
{
    [TestFixture]
    public class HttpHeaderSpecs
    {
        [Test]
        public void Single_line_header_without_leading_whitespace_for_the_value_is_split_at_the_colon()
        {
            Assert.AreEqual(new HttpHeader("Content-Length", "11"), HttpHeader.Parse("Content-Length:11"));
        }

        [Test]
        public void Leading_whitespace_for_the_header_value_is_ignored()
        {
            Assert.AreEqual(new HttpHeader("Content-Length", "11"), HttpHeader.Parse("Content-Length:   \t  11"));
        }

        [Test]
        public void Trailing_whitespace_for_the_header_value_is_ignored()
        {
            Assert.AreEqual(new HttpHeader("Content-Length", "11"), HttpHeader.Parse("Content-Length: 11\t   "));
        }

        [Test]
        public void Multiline_header_value_with_leading_tabs_on_second_line_is_concatenated_with_single_space()
        {
            Assert.AreEqual(new HttpHeader("Header-With-Multiline-Value", "First row second row"), HttpHeader.Parse("Header-With-Multiline-Value: First row\r\n\t\tsecond row"));
        }
        
        [Test]
        public void Multiline_header_value_with_leading_spaces_on_second_line_is_concatenated_with_single_space()
        {
            Assert.AreEqual(new HttpHeader("Header-With-Multiline-Value", "First row second row"), HttpHeader.Parse("Header-With-Multiline-Value: First row\r\n  second row"));
        }
    }
}