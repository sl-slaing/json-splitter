using Newtonsoft.Json;
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
            this.jsonWriter = new JsonTextWriter(writer);
            this.writer = writer;
            this.serialiser = serialiser;
        }

        public void Write(IRelationalObject data)
        {
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
