namespace IdSharp.Tagging.ID3v2
{
    using System;
    using System.Runtime.InteropServices;

    [ComVisible(true), Guid("D111D8A9-A14F-4835-B318-46A3E787E845")]
    public enum TextContentType : byte
    {
        Chord = 5,
        Event = 4,
        Lyrics = 1,
        MovementPartName = 3,
        Other = 0,
        TextTranscription = 2,
        TriviaPopup = 6,
        URLsToImages = 8,
        URLsToWebpages = 7
    }
}

