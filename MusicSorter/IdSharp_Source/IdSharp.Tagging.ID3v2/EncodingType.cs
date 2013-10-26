namespace IdSharp.Tagging.ID3v2
{
    using System;
    using System.Runtime.InteropServices;

    [ComVisible(true), Guid("E2E6218B-5903-4bd1-92C3-706A7338FE8F")]
    public enum EncodingType : byte
    {
        ISO88591 = 0,
        Unicode = 1,
        UTF16BE = 2,
        UTF8 = 3
    }
}

