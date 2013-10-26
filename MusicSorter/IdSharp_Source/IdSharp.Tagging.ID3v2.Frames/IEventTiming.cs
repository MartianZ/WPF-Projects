namespace IdSharp.Tagging.ID3v2.Frames
{
    using IdSharp.Tagging.ID3v2;
    using System;
    using System.ComponentModel;

    public interface IEventTiming : IFrame, INotifyPropertyChanged
    {
        BindingList<IEventTimingItem> Items { get; }

        IdSharp.Tagging.ID3v2.TimestampFormat TimestampFormat { get; set; }

    }
}

