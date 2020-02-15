using Newtonsoft.Json;
using System;
using System.IO;

namespace json_splitter
{
    public class FileStream : IDisposable
    {
        private readonly TextWriter writer;
        private readonly JsonSerializer serialiser;

        public FileStream(string fileName, JsonSerializer serialiser)
        {
            this.writer = new StreamWriter(fileName, false);
            this.serialiser = serialiser;
        }

        public void Dispose()
        {
            writer.Flush();
            writer.Close();
        }

        public void SendData(IRelationalObject relationalObject)
        {
            var jsonWriter = new JsonTextWriter(writer);
            serialiser.Serialize(jsonWriter, relationalObject.Data);
            writer.WriteLine();
        }
    }
}