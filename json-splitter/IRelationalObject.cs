using System.Collections.Generic;

namespace json_splitter
{
    public interface IRelationalObject
    {
        IReadOnlyDictionary<string, object> Data { get; }
        IReadOnlyCollection<RelationalObject> Children { get; }
        IRelationalObject Parent { get; }
        string RelationshipName { get; }

        IRelationalObject WithForeignKey(IBindingConfiguration binding);
    }
}