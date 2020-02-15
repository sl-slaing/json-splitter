using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace json_splitter
{
    public class RelationalObjectReader : IRelationalObjectReader
    {
        public RelationalObject ReadJson(JObject jsonData)
        {
            return ReadJson(jsonData, null);
        }

        private RelationalObject ReadJson(JObject jsonData, string name)
        {
            var data = new Dictionary<string, object>();
            var relationships = new List<JProperty>();

            foreach (var property in jsonData.Properties())
            {
                if (property.Value.Type == JTokenType.Object || property.Value.Type == JTokenType.Array)
                {
                    relationships.Add(property);
                    continue;
                }

                data.Add(property.Name, property.Value.ToObject<object>());
            }

            return new RelationalObject
            {
                RelationshipName = name,
                Data = data,
                RelatedData = ReadRelationships(relationships).ToList()
            };
        }

        private IEnumerable<RelationalObject> ReadRelationships(List<JProperty> relationships)
        {
            foreach (var relationship in relationships)
            {
                var relationshipData = relationship.Value;
                var dataItems = new List<JObject>();

                if (relationshipData.Type == JTokenType.Array)
                {
                    foreach (var relationshipItem in relationshipData.AsJEnumerable())
                    {
                        dataItems.Add((JObject)relationshipItem);
                    }
                }
                else
                {
                    dataItems.Add((JObject)relationshipData);
                }

                foreach (var dataItem in dataItems)
                {
                    yield return ReadJson(dataItem, relationship.Name);
                }
            }
        }
    }
}
