﻿using System.IO;
using System.Linq;

namespace json_splitter
{
    public class CsvStream : IOutputStream
    {
        private readonly TextWriter writer;
        private readonly bool includeHeaders;
        private string[] csvColumnOrder;

        public CsvStream(TextWriter writer, bool includeHeaders)
        {
            this.writer = writer;
            this.includeHeaders = includeHeaders;
        }

        public void Write(IRelationalObject data)
        {
            if (csvColumnOrder == null)
            {
                csvColumnOrder = CreateCsvColumnOrder(data);
                if (includeHeaders)
                {
                    writer.WriteLine(FormatAsCsv(csvColumnOrder));
                }
            }

            writer.WriteLine(FormatDataAsCsv(data, csvColumnOrder));
        }

        private string FormatAsCsv<T>(T[] csvColumnOrder)
        {
            return string.Join(",", csvColumnOrder.ToArray());
        }

        private string[] CreateCsvColumnOrder(IRelationalObject relationalObject)
        {
            return relationalObject.Data.Keys.ToArray();
        }

        private string FormatDataAsCsv(IRelationalObject relationalObject, string[] columnOrder)
        {
            return FormatAsCsv(columnOrder.Select(column => relationalObject.Data[column]).ToArray());
        }

        public void Dispose()
        {
            writer.Flush();
            writer.Close();
        }
    }
}