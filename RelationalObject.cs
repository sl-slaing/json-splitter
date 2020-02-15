using System.Collections.Generic;

namespace json_splitter
{
    public class RelationalObject : IRelationalObject
    {
        public string RelationshipName { get; set; }
        public IReadOnlyDictionary<string, object> Data { get; set; }
        public IReadOnlyCollection<RelationalObject> RelatedData { get; set; }

        public IRelationalObject WithForeignKey(IRelatedDataConfiguration config, IReadOnlyDictionary<string, object> parentData)
        {
            if (parentData == null || config.ForeignKeyPropertyName == null)
            {
                return this;
            }

            var augmentedData = new Dictionary<string, object>(Data);
            augmentedData.Add(config.ForeignKeyColumnName, parentData[config.ForeignKeyPropertyName]);
            return new RelationalObject
            {
                Data = augmentedData,
                RelationshipName = RelationshipName,
                RelatedData = RelatedData
            };
        }
    }
}
