using Newtonsoft.Json;
using System;
using System.IO;

namespace json_splitter
{
    public class OutputStreamFactory : IOutputStreamFactory
    {
        private readonly RowFormat format;
        private readonly bool csvHeader;
        private readonly JsonSerializer serialiser;

        public OutputStreamFactory(RowFormat format, bool csvHeader, JsonSerializer serialiser)
        {
            this.format = format;
            this.csvHeader = csvHeader;
            this.serialiser= serialiser;
        }

        public IOutputStream Create(TextWriter writer)
        {
            switch (format)
            {
                case RowFormat.Csv:
                    return new CsvStream(writer, csvHeader);
                case RowFormat.Json:
                    return new JsonStream(writer, serialiser);
                default:
                    throw new InvalidOperationException($"Unsupported row format: {format}");
            }
        }
    }
}