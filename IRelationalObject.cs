using System.Collections.Generic;

namespace json_splitter
{
    public interface IRelationalObject
    {
        IReadOnlyDictionary<string, object> Data { get; }
        IReadOnlyCollection<RelationalObject> RelatedData { get; }
        string RelationshipName { get; }

        IRelationalObject WithForeignKey(IRelatedDataConfiguration config, IReadOnlyDictionary<string, object> parentData);
    }
}