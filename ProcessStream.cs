using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace json_splitter
{
    public class ProcessStream : IDisposable
    {
        private readonly ProcessConfiguration config;
        private readonly JsonSerializer serialiser;
        private Process process;
        private StreamWriter writer;
        private string[] csvColumnOrder;

        public ProcessStream(ProcessConfiguration config, JsonSerializer serialiser)
        {
            this.config = config;
            this.serialiser = serialiser;
        }

        public void Dispose()
        {
            if (process == null || process.HasExited || writer == null)
            {
                return;
            }

            writer.Flush();
            writer.Close();
            writer = null;
            process.WaitForExit();
            process = null;
        }

        internal void SendData(IRelationalObject relationalObject)
        {
            if (process == null || writer == null)
            {
                StartProcess();
            }

            if (process.HasExited)
            {
                throw new InvalidOperationException($"Target process has exited: {process.Id} ({config.FileName} {config.Arguments})");
            }

            if (csvColumnOrder == null)
            {
                csvColumnOrder = CreateCsvColumnOrder(relationalObject);
                if (config.CsvHeader)
                {
                    writer.WriteLine(FormatAsCsv(csvColumnOrder));
                }
            }

            switch (config.Format)
            {
                case RowFormat.Csv:
                    writer.WriteLine(FormatDataAsCsv(relationalObject, csvColumnOrder));
                    break;
                case RowFormat.Json:
                    WriteJson(relationalObject);
                    break;
                default:
                    throw new InvalidOperationException("Unsupported row format: " + config.Format);
            }
        }

        private string FormatAsCsv<T>(T[] csvColumnOrder)
        {
            return string.Join(",", csvColumnOrder.ToArray());
        }

        private string[] CreateCsvColumnOrder(IRelationalObject relationalObject)
        {
            return relationalObject.Data.Keys.ToArray();
        }

        private void WriteJson(IRelationalObject relationalObject)
        {
            var jsonWriter = new JsonTextWriter(writer);
            serialiser.Serialize(jsonWriter, relationalObject.Data);
            writer.WriteLine();
        }

        private string FormatDataAsCsv(IRelationalObject relationalObject, string[] columnOrder)
        {
            return FormatAsCsv(columnOrder.Select(column => relationalObject.Data[column]).ToArray());
        }

        private void StartProcess()
        {
            process = new Process
            {
                StartInfo =
                {
                    FileName = config.FileName,
                    Arguments = config.Arguments,
                    WorkingDirectory = config.WorkingDirectory,
                    CreateNoWindow = true,
                    RedirectStandardInput = true,
                    UseShellExecute = false,
                    StandardInputEncoding = Encoding.UTF8
                }
            };

            if (!process.Start())
            {
                throw new InvalidOperationException($"Could not start process: {config.FileName} {config.Arguments}");
            }

            writer = process.StandardInput;
        }
    }
}