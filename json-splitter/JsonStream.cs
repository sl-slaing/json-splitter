using Newtonsoft.Json;
using System;
using System.IO;

namespace json_splitter
{
    public class JsonStream : IOutputStream
    {
        private readonly JsonTextWriter jsonWriter;
        private readonly TextWriter writer;
        private readonly JsonSerializer serialiser;

        public JsonStream(TextWriter writer, JsonSerializer serialiser)
        {
            if (writer == null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            if (serialiser == null)
            {
                throw new ArgumentNullException(nameof(serialiser));
            }

            this.jsonWriter = new JsonTextWriter(writer);
            this.writer = writer;
            this.serialiser = serialiser;
        }

        public void Write(IRelationalObject data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            serialiser.Serialize(jsonWriter, data.Data);
            writer.WriteLine();
        }

        public void Dispose()
        {
            writer.Flush();
            writer.Dispose();
        }
    }
}
