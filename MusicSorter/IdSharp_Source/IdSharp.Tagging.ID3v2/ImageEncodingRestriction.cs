namespace IdSharp.Tagging.ID3v2
{
    using System;
    using System.Runtime.InteropServices;

    [Guid("D70FE966-3DB1-4a8c-9626-4E5498E19785"), ComVisible(true)]
    public enum ImageEncodingRestriction : byte
    {
        NoRestrictions = 0,
        PngOrJpg = 1
    }
}

