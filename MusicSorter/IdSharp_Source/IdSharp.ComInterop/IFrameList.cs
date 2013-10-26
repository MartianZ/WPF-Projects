namespace IdSharp.ComInterop
{
    using System;
    using System.Reflection;
    using System.Runtime.InteropServices;

    [ComVisible(true), Guid("945CF2AE-163B-4247-B286-E752EB1B709B")]
    public interface IFrameList
    {
        [DispId(800)]
        object AddNew();
        [DispId(0x321)]
        int Add(object value);
        [DispId(0x322)]
        void Clear();
        [DispId(0x323)]
        void Remove(object value);
        [DispId(0x324)]
        void RemoveAt(int index);
        [DispId(0x325)]
        object this[int index] { get; set; }
        [DispId(0x326)]
        int Count { get; }
    }
}

