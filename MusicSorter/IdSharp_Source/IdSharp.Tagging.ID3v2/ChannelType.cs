namespace IdSharp.Tagging.ID3v2
{
    using System;

    internal enum ChannelType : byte
    {
        BackCenter = 7,
        BackLeft = 5,
        BackRight = 4,
        FrontCenter = 6,
        FrontLeft = 3,
        FrontRight = 2,
        MasterVolume = 1,
        Other = 0,
        Subwoofer = 8
    }
}

