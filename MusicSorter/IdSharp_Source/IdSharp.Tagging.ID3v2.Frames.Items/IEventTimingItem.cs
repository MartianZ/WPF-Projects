namespace IdSharp.Tagging.ID3v2.Frames.Items
{
    using IdSharp.Tagging.ID3v2;
    using System;
    using System.ComponentModel;

    public interface IEventTimingItem : INotifyPropertyChanged
    {
        MusicEvent EventType { get; set; }

        int Timestamp { get; set; }

    }
}

