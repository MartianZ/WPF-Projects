namespace IdSharp.Tagging.ID3v2
{
    using System;
    using System.Runtime.InteropServices;

    [ComVisible(true), Guid("5B1EB98C-0C7A-4396-98CE-A0B78EFDEBE3")]
    public enum ID3v2TagVersion : byte
    {
        ID3v22 = 2,
        ID3v23 = 3,
        ID3v24 = 4
    }
}

