using System;

namespace json_splitter
{
    internal interface IDataStream : IDisposable
    {
        void PushData(IRelationalObject relationalObject);
    }
}