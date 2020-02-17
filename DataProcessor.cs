using Newtonsoft.Json.Linq;

namespace json_splitter
{
    public class DataProcessor : IDataProcessor
    {
        private readonly IDataSenderFactory senderFactory;
        private readonly IRelationalObjectReader objectReader;

        public DataProcessor(IDataSenderFactory senderFactory, IRelationalObjectReader objectReader)
        {
            this.senderFactory = senderFactory;
            this.objectReader = objectReader;
        }

        public void ProcessData(IDataConfiguration config, JObject jsonData)
        {
            var data = objectReader.ReadJson(jsonData);

            ProcessData(config, data);
        }

        private void ProcessData(IDataConfiguration config, IRelationalObject data)
        {
            var sender = senderFactory.GetDataSender(config);

            sender.SendData(data);
    
            foreach (var relationship in data.Children)
            {
                var relationshipConfig = config.Relationships[relationship.RelationshipName];
                ProcessData(relationshipConfig, relationship);
            }
        }
    }
}
