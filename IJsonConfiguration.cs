using System.Collections.Generic;

namespace json_splitter
{
    public interface IDataConfiguration
    {
        string TableName { get; }
        IReadOnlyDictionary<string, IRelatedDataConfiguration> Relationships { get; }
        SqlConfiguration Sql { get; }
        ProcessConfiguration Process { get; }
    }
}
