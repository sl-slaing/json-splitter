using System;
using System.Collections.Generic;

namespace json_splitter
{
    public class RelationalObject : IRelationalObject
    {
        public string RelationshipName { get; set; }
        public IReadOnlyDictionary<string, object> Data { get; set; }
        public IReadOnlyCollection<RelationalObject> Children { get; set; }
        public IRelationalObject Parent { get; set; }

        public IRelationalObject WithForeignKey(IBindingConfiguration config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            if (Parent == null || config.ForeignKeyPropertyName == null)
            {
                return this;
            }

            if (Parent.Data == null)
            {
                throw new InvalidOperationException("Current object has no data");
            }

            if (!Parent.Data.ContainsKey(config.ForeignKeyPropertyName))
            {
                throw new ArgumentException($"There is no property in the parent data with the property name '{config.ForeignKeyPropertyName}'", "ForeignKeyPropertyName");
            }

            var augmentedData = new Dictionary<string, object>(Data);
            augmentedData.Add(config.ForeignKeyColumnName, Parent.Data[config.ForeignKeyPropertyName]);
            return new RelationalObject
            {
                Data = augmentedData,
                RelationshipName = RelationshipName,
                Children = Children
            };
        }
    }
}
