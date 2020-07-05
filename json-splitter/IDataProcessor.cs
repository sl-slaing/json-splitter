using Newtonsoft.Json.Linq;

namespace json_splitter
{
    public interface IDataProcessor
    {
        void ProcessData(IDataConfiguration config, JObject jsonData);
    }
}
