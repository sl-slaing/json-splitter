using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace json_splitter
{
    [ExcludeFromCodeCoverage]
    public class RelatedJsonConfiguration : IDataConfiguration
    {
        public string ForeignKeyColumnName { get; set; }
        public string ForeignKeyPropertyName { get; set; }
        public Dictionary<string, RelatedJsonConfiguration> Relationships { get; set; }

        public SqlConfiguration Sql { get; set; }
        public ProcessConfiguration Process { get; set; }
        public FileConfiguration File { get; set; }

        IReadOnlyDictionary<string, IDataConfiguration> IDataConfiguration.Relationships 
            => Relationships.ToDictionary(pair => pair.Key, pair => (IDataConfiguration)pair.Value);
    }
}
