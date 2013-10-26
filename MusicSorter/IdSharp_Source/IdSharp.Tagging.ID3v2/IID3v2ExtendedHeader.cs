namespace IdSharp.Tagging.ID3v2
{
    using System;
    using System.IO;

    public interface IID3v2ExtendedHeader
    {
        byte[] GetBytes(ID3v2TagVersion tagVersion);
        void ReadFrom(TagReadingInfo tagReadingInfo, Stream stream);

        int CRC32 { get; }

        bool IsCRCDataPresent { get; set; }

        bool IsTagAnUpdate { get; set; }

        bool IsTagRestricted { get; set; }

        int PaddingSize { get; set; }

        int SizeExcludingSizeBytes { get; }

        int SizeIncludingSizeBytes { get; }

        ITagRestrictions TagRestrictions { get; }

    }
}

