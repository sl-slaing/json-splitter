using System.IO;

namespace json_splitter
{
    public interface IInputFactory
    {
        TextReader GetInput(Arguments args);
    }
}