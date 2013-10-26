namespace IdSharp.Utils
{
    using System;
    using System.Runtime.CompilerServices;

    public interface INotifyInvalidData
    {
        event InvalidDataEventHandler InvalidData;
    }
}

