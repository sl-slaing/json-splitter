using System.IO;

namespace json_splitter
{
    public class StreamFactory : IStreamFactory
    {
        public TextWriter OpenWrite(string fileName)
        {
            return new StreamWriter(fileName, false);
        }
    }
}
