namespace IdSharp.Tagging.ID3v2
{
    using System;
    using System.Runtime.InteropServices;

    [ComVisible(true), Guid("D7ADEAC4-A9DD-4a69-A4BC-6B9163D44833")]
    public enum AudioScramblingMode
    {
        Default,
        Unsynchronization,
        Scrambling,
        None
    }
}

