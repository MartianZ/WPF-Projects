namespace IdSharp.Tagging.ID3v2.Frames
{
    using IdSharp.Tagging.ID3v2;
    using System;
    using System.ComponentModel;

    public interface IPositionSynchronization : IFrame, INotifyPropertyChanged
    {
        int Position { get; set; }

        IdSharp.Tagging.ID3v2.TimestampFormat TimestampFormat { get; set; }

    }
}

