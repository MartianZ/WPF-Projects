namespace IdSharp.Tagging.ID3v2
{
    using System;

    public interface IFrameHeader
    {
        int DecompressedSize { get; set; }

        Nullable<byte> EncryptionMethod { get; set; }

        int FrameSize { get; }

        int FrameSizeExcludingAdditions { get; }

        int FrameSizeTotal { get; }

        Nullable<byte> GroupingIdentity { get; set; }

        bool IsCompressed { get; set; }

        bool IsFileAlterPreservation { get; set; }

        bool IsReadOnly { get; set; }

        bool IsTagAlterPreservation { get; set; }

        bool UsesUnsynchronization { get; set; }

    }
}

