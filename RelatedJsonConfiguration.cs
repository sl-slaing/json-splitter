using System.Collections.Generic;
using System.Linq;

namespace json_splitter
{
    public class RelatedJsonConfiguration : IDataConfiguration, IRelatedDataConfiguration
    {
        public string TableName { get; set; }
        public string ForeignKeyColumnName { get; set; }
        public string ForeignKeyPropertyName { get; set; }
        public Dictionary<string, RelatedJsonConfiguration> Relationships { get; set; }

        public SqlConfiguration Sql { get; set; }
        public ProcessConfiguration Process { get; set; }

        IReadOnlyDictionary<string, IRelatedDataConfiguration> IDataConfiguration.Relationships 
            => Relationships.ToDictionary(pair => pair.Key, pair => (IRelatedDataConfiguration)pair.Value);
    }
}
