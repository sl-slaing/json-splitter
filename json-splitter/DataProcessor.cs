using Newtonsoft.Json.Linq;
using System;

namespace json_splitter
{
    public class DataProcessor : IDataProcessor
    {
        private readonly IDataSenderFactory senderFactory;
        private readonly IRelationalObjectReader objectReader;

        public DataProcessor(IDataSenderFactory senderFactory, IRelationalObjectReader objectReader)
        {
            if (senderFactory == null)
            {
                throw new ArgumentNullException(nameof(senderFactory));
            }

            if (objectReader == null)
            {
                throw new ArgumentNullException(nameof(objectReader));
            }

            this.senderFactory = senderFactory;
            this.objectReader = objectReader;
        }

        public void ProcessData(IDataConfiguration config, JObject jsonData)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            if (jsonData == null)
            {
                throw new ArgumentNullException(nameof(jsonData));
            }

            ProcessData(
                config,
                objectReader.ReadJson(jsonData));
        }

        private void ProcessData(IDataConfiguration config, IRelationalObject data)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

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
