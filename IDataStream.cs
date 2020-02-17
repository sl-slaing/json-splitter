using System;

namespace json_splitter
{
    internal interface ISqlDataStream : IDisposable
    {
        void PushData(IRelationalObject relationalObject);
    }
}