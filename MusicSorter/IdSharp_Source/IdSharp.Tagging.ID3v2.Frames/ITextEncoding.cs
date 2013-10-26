namespace IdSharp.Tagging.ID3v2.Frames
{
    using IdSharp.Tagging.ID3v2;
    using System;
    using System.Runtime.InteropServices;

    [ComVisible(true), Guid("6748D4CF-AD6E-4319-A44A-785F7ACD9867")]
    public interface ITextEncoding
    {
        [DispId(0x44c)]
        EncodingType TextEncoding { get; set; }
    }
}

