using System.Collections.Generic;

namespace json_splitter
{
    public interface IDataConfiguration
    {
        IReadOnlyDictionary<string, IDataConfiguration> Relationships { get; }
        ProcessConfiguration Process { get; }
        FileConfiguration File { get; }
    }
}
