namespace IdSharp.Tagging.ID3v2
{
    using System;
    using System.Runtime.InteropServices;

    [ComVisible(true), Guid("E5716B40-9420-44df-A97D-DC397AB2A0E8")]
    public enum MusicEvent : byte
    {
        AudioEnd = 0xfd,
        AudioFileEnds = 0xfe,
        EndOfInitialSilence = 1,
        InterludeStart = 8,
        IntroEnd = 0x10,
        IntroStart = 2,
        KeyChange = 11,
        MainPartEnd = 0x11,
        MainPartStart = 3,
        MomentaryUnwantedNoise = 13,
        OutroEnd = 5,
        OutroStart = 4,
        Padding = 0,
        Profanity = 0x15,
        ProfanityEnd = 0x16,
        RefrainEnd = 0x13,
        RefrainStart = 7,
        SustainedNoise = 14,
        SustainedNoiseEnd = 15,
        ThemeEnd = 20,
        ThemeStart = 9,
        TimeChange = 12,
        UserEvent1 = 0xe0,
        UserEvent10 = 0xe9,
        UserEvent11 = 0xea,
        UserEvent12 = 0xeb,
        UserEvent13 = 0xec,
        UserEvent14 = 0xed,
        UserEvent15 = 0xee,
        UserEvent16 = 0xef,
        UserEvent2 = 0xe1,
        UserEvent3 = 0xe2,
        UserEvent4 = 0xe3,
        UserEvent5 = 0xe4,
        UserEvent6 = 0xe5,
        UserEvent7 = 0xe6,
        UserEvent8 = 0xe7,
        UserEvent9 = 0xe8,
        VariationStart = 10,
        VerseEnd = 0x12,
        VerseStart = 6
    }
}

