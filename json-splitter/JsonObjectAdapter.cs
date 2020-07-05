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
            return ReadJson(jsonData, null, null);
        }

        private RelationalObject ReadJson(JObject jsonData, string name, IRelationalObject parent)
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

            var obj = new RelationalObject
            {
                RelationshipName = name,
                Data = data,
                Parent = parent
            };

            obj.Children = ReadRelationships(relationships, obj).ToList();

            return obj;
        }

        private IEnumerable<RelationalObject> ReadRelationships(List<JProperty> relationships, IRelationalObject parent)
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
                    yield return ReadJson(dataItem, relationship.Name, parent);
                }
            }
        }
    }
}
