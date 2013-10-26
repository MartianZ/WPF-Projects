namespace IdSharp.Tagging.ID3v2
{
    using System;
    using System.Runtime.InteropServices;

    [Guid("15621FA4-B6C4-4a24-A7F2-6F402EB84E69"), ComVisible(true)]
    public enum TimestampFormat : byte
    {
        Milliseconds = 2,
        MpegFrames = 1
    }
}

