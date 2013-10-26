namespace IdSharp.Tagging.ID3v2
{
    using System;
    using System.Runtime.InteropServices;

    [Guid("87388051-39FB-43ac-9C74-006B4A374383"), ComVisible(true)]
    public enum TextFieldsSizeRestriction : byte
    {
        NoMore1024Chars = 1,
        NoMore128Chars = 2,
        NoMore30Chars = 3,
        NoRestrictions = 0
    }
}

