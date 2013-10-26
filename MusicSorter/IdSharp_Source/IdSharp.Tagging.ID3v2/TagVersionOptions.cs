namespace IdSharp.Tagging.ID3v2
{
    using System;
    using System.Runtime.InteropServices;

    [Flags, ComVisible(true), Guid("82E49486-676F-42d1-8D33-587E4D7CEAC8")]
    public enum TagVersionOptions
    {
        AddOneByteToSize = 2,
        None = 0,
        Unsynchronized = 4,
        UseNonSyncSafeFrameSizeID3v24 = 1
    }
}

