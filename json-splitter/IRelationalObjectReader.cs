using Newtonsoft.Json.Linq;

namespace json_splitter
{
    public interface IRelationalObjectReader
    {
        RelationalObject ReadJson(JObject jsonData);
    }
}