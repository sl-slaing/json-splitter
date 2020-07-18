using System.IO;

namespace json_splitter
{
    public interface IStreamFactory
    {
        TextWriter OpenWrite(string fileName);
    }
}