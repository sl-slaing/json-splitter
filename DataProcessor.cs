using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace json_splitter
{
    public class DataProcessor : IDataProcessor
    {
        private readonly IDataSender sender;
        private readonly IRelationalObjectReader objectReader;

        public DataProcessor(IDataSender sender, IRelationalObjectReader objectReader)
        {
            this.sender = sender;
            this.objectReader = objectReader;
        }

        public void ProcessData(IRelatedDataConfiguration config, JObject jsonData)
        {
            var data = objectReader.ReadJson(jsonData);

            ProcessData(config, data, null);
        }

        private void ProcessData(IRelatedDataConfiguration config, RelationalObject data, IReadOnlyDictionary<string, object> parentData)
        {
            sender.SendData(config, data.WithForeignKey(config, parentData));

            foreach (var relationship in data.RelatedData)
            {
                var relationshipConfig = config.Relationships[relationship.RelationshipName];
                ProcessData(relationshipConfig, relationship, data.Data);
            }
        }
    }
}
