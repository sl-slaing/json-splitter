using System.IO;

namespace json_splitter
{
    public interface IOutputStreamFactory
    {
        IOutputStream Create(TextWriter writer);
    }
}
