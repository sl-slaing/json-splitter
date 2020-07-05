using System.IO;

namespace json_splitter
{
    public interface IConfigurationRepository
    {
        RelatedJsonConfiguration ReadConfiguration(string path);
        RelatedJsonConfiguration ReadConfiguration(TextReader reader);
    }
}