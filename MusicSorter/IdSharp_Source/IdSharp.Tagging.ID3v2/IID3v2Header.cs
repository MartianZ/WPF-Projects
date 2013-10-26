namespace IdSharp.Tagging.ID3v2
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices;

    [ComVisible(true), Guid("8FA23100-C415-45f1-B845-DD49C3569B78")]
    public interface IID3v2Header
    {
        ID3v2TagVersion TagVersion { get; set; }
        byte TagVersionRevision { get; set; }
        int TagSize { get; }
        bool UsesUnsynchronization { get; set; }
        bool HasExtendedHeader { get; set; }
        bool IsExperimental { get; set; }
        bool IsCompressed { get; set; }
        bool IsFooterPresent { get; set; }
        void Read(Stream stream);
        byte[] GetBytes();
    }
}

