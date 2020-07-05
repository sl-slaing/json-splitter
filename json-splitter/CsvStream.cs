using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;

namespace json_splitter
{
    public class CsvStream : IOutputStream
    {
        private readonly IDisposable underlyingWriter;
        private readonly CsvWriter writer;
        private readonly bool includeHeaders;
        private string[] csvColumnOrder;

        public CsvStream(TextWriter writer, bool includeHeaders)
        {
            if (writer == null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            this.underlyingWriter = writer;
            this.writer = new CsvWriter(
                writer,
                new CsvConfiguration(CultureInfo.CurrentCulture));
            this.includeHeaders = includeHeaders;
        }

        public void Write(IRelationalObject data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (csvColumnOrder == null)
            {
                csvColumnOrder = CreateCsvColumnOrder(data);
                if (includeHeaders)
                {
                    WriteHeader(csvColumnOrder);
                }
            }

            WriteRecord(data.Data);
        }

        private void WriteHeader(string[] csvColumnOrder)
        {
            foreach (var field in csvColumnOrder)
            {
                writer.WriteField(field);
            }
            writer.NextRecord();
        }

        private void WriteRecord(IReadOnlyDictionary<string, object> data)
        {
            foreach (var field in csvColumnOrder)
            {
                writer.WriteField(data[field] ?? "");
            }
            writer.NextRecord();
        }

        private string[] CreateCsvColumnOrder(IRelationalObject relationalObject)
        {
            return relationalObject.Data.Keys.ToArray();
        }

        public void Dispose()
        {
            writer.Flush();
            underlyingWriter.Dispose();
        }
    }
}
