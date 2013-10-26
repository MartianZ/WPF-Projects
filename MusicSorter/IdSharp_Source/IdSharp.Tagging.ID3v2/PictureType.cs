namespace IdSharp.Tagging.ID3v2
{
    using System;
    using System.Runtime.InteropServices;

    [Guid("21720D9F-E868-4a1d-BABA-65F53163B29C"), ComVisible(true)]
    public enum PictureType : byte
    {
        ABrightColoredFish = 0x11,
        ArtistPerformer = 8,
        BandArtistLogo = 0x13,
        BandOrchestra = 10,
        Composer = 11,
        Conductor = 9,
        CoverBack = 4,
        CoverFront = 3,
        DuringPerformance = 15,
        DuringRecording = 14,
        FileIcon32x32Png = 1,
        Illustration = 0x12,
        LeadArtistPerformer = 7,
        LeafletPage = 5,
        Lyricist = 12,
        MediaLabelSideOfCD = 6,
        MovieVideoScreenCapture = 0x10,
        Other = 0,
        OtherFileIcon = 2,
        PublisherStudioLogo = 20,
        RecordingLocation = 13
    }
}

