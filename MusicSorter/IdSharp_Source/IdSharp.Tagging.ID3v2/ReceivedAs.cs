namespace IdSharp.Tagging.ID3v2
{
    using System;
    using System.Runtime.InteropServices;

    [Guid("652A237B-A0B6-4ed4-ACDA-C3DD7F299195"), ComVisible(true)]
    public enum ReceivedAs : byte
    {
        AsNoteSheets = 5,
        AsNoteSheetsInABookWithOtherSheets = 6,
        CompressedAudioOnCD = 2,
        FileOverTheInternet = 3,
        MusicOnOtherMedia = 7,
        NonmusicalMerchandise = 8,
        Other = 0,
        StandardCDAlbumWithOtherSongs = 1,
        StreamOverTheInternet = 4
    }
}

