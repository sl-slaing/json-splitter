using json_splitter;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace json_splitter_tests
{
    [TestFixture]
    public class CsvStreamTests
    {
        [Test]
        public void Write_WhenGivenNullData_ShouldThrow()
        {
            var writer = new StringWriter();
            var stream = new CsvStream(writer, true);

            Assert.That(() => stream.Write(null), Throws.ArgumentNullException);
        }

        [Test]
        public void Write_WhenHeadersShouldBeIncluded_ShouldWriteColumnsHeaders()
        {
            var writer = new StringWriter();
            var stream = new CsvStream(writer, true);
            var data = new RelationalObject
            {
                Data = new Dictionary<string, object>
                {
                    { "prop1", "value1" },
                    { "prop2", "value2" }
                }
            };

            stream.Write(data);

            var lines = writer.GetStringBuilder().ToString().Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);
            var firstLine = lines.FirstOrDefault();
            Assert.That(firstLine, Is.EqualTo("prop1,prop2"));
        }

        [Test]
        public void Write_WhenHeadersShouldBeIncluded_ShouldWriteData()
        {
            var writer = new StringWriter();
            var stream = new CsvStream(writer, true);
            var data = new RelationalObject
            {
                Data = new Dictionary<string, object>
                {
                    { "prop1", "value1" },
                    { "prop2", "value2" }
                }
            };

            stream.Write(data);

            var lines = writer.GetStringBuilder().ToString().Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);
            var secondLine = lines.Skip(1).FirstOrDefault();
            Assert.That(secondLine, Is.EqualTo("value1,value2"));
            Assert.That(lines.Count(), Is.EqualTo(2));
        }

        [Test]
        public void Write_WhenHeadersShouldBeIncludedAndWritingSecondDataLine_ShouldWriteDataInSameOrderAsHeaderRow()
        {
            var writer = new StringWriter();
            var stream = new CsvStream(writer, true);
            var data1 = new RelationalObject
            {
                Data = new Dictionary<string, object>
                {
                    { "prop1", "value1.1" },
                    { "prop2", "value2.1" }
                }
            };
            var data2 = new RelationalObject
            {
                Data = new Dictionary<string, object>
                {
                    { "prop2", "value2.2" },
                    { "prop1", "value1.2" }
                }
            };

            stream.Write(data1);
            stream.Write(data2);

            var lines = writer.GetStringBuilder().ToString().Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);
            var secondLine = lines.Skip(1).FirstOrDefault();
            var thirdLine = lines.Skip(2).FirstOrDefault();
            Assert.That(secondLine, Is.EqualTo("value1.1,value2.1"));
            Assert.That(thirdLine, Is.EqualTo("value1.2,value2.2"));
            Assert.That(lines.Count(), Is.EqualTo(3));
        }

        [Test]
        public void Write_WhenHeadersShouldNotBeIncluded_ShouldWriteData()
        {
            var writer = new StringWriter();
            var stream = new CsvStream(writer, false);
            var data = new RelationalObject
            {
                Data = new Dictionary<string, object>
                {
                    { "prop1", "value1" },
                    { "prop2", "value2" }
                }
            };

            stream.Write(data);

            var lines = writer.GetStringBuilder().ToString().Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);
            var firstLine = lines.FirstOrDefault();
            Assert.That(firstLine, Is.EqualTo("value1,value2"));
            Assert.That(lines.Count(), Is.EqualTo(1));
        }

        [Test]
        public void Write_WhenPropertyContainsAComma_ShouldWriteHeaderInQuotes()
        {
            var writer = new StringWriter();
            var stream = new CsvStream(writer, true);
            var data = new RelationalObject
            {
                Data = new Dictionary<string, object>
                {
                    { "prop1,2", "value1" }
                }
            };

            stream.Write(data);

            var lines = writer.GetStringBuilder().ToString().Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);
            var firstLine = lines.FirstOrDefault();
            Assert.That(firstLine, Is.EqualTo("\"prop1,2\""));
            Assert.That(lines.Count(), Is.EqualTo(2));
        }

        [Test]
        public void Write_WhenPropertyContainsADoubleQuote_ShouldWriteHeaderWithQuotesEscaped()
        {
            var writer = new StringWriter();
            var stream = new CsvStream(writer, true);
            var data = new RelationalObject
            {
                Data = new Dictionary<string, object>
                {
                    { "prop \"1\"", "value1" }
                }
            };

            stream.Write(data);

            var lines = writer.GetStringBuilder().ToString().Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);
            var firstLine = lines.FirstOrDefault();
            Assert.That(firstLine, Is.EqualTo("\"prop \"\"1\"\"\""));
            Assert.That(lines.Count(), Is.EqualTo(2));
        }

        [Test]
        public void Write_WhenPropertyStartsAndEndsWithADoubleQuote_ShouldWriteHeaderWithoutDoubleQuote()
        {
            var writer = new StringWriter();
            var stream = new CsvStream(writer, true);
            var data = new RelationalObject
            {
                Data = new Dictionary<string, object>
                {
                    { "\"prop 1\"", "value1" }
                }
            };

            stream.Write(data);

            var lines = writer.GetStringBuilder().ToString().Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);
            var firstLine = lines.FirstOrDefault();
            Assert.That(firstLine, Is.EqualTo("\"\"\"prop 1\"\"\""));
            Assert.That(lines.Count(), Is.EqualTo(2));
        }

        [Test]
        public void Write_WhenPropertyContainsADoubleQuoteAndComma_ShouldWriteHeaderWithQuotesAndExistingQuotesEscaped()
        {
            var writer = new StringWriter();
            var stream = new CsvStream(writer, true);
            var data = new RelationalObject
            {
                Data = new Dictionary<string, object>
                {
                    { "prop,1 \"2\"", "value1" }
                }
            };

            stream.Write(data);

            var lines = writer.GetStringBuilder().ToString().Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);
            var firstLine = lines.FirstOrDefault();
            Assert.That(firstLine, Is.EqualTo("\"prop,1 \"\"2\"\"\""));
            Assert.That(lines.Count(), Is.EqualTo(2));
        }

        [Test]
        public void Write_WhenPropertyHasNullValue_ShouldWriteEmptyValue()
        {
            var writer = new StringWriter();
            var stream = new CsvStream(writer, false);
            var data = new RelationalObject
            {
                Data = new Dictionary<string, object>
                {
                    { "null-prop", null },
                    { "empty-prop", "" }
                }
            };

            stream.Write(data);

            var lines = writer.GetStringBuilder().ToString().Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);
            var firstLine = lines.FirstOrDefault();
            Assert.That(firstLine, Is.EqualTo(","));
            Assert.That(lines.Count(), Is.EqualTo(1));
        }
    }
}
