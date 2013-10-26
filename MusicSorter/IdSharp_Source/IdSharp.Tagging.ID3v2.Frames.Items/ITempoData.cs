namespace IdSharp.Tagging.ID3v2.Frames.Items
{
    using System;
    using System.ComponentModel;

    public interface ITempoData : INotifyPropertyChanged
    {
        short TempoCode { get; set; }

        int Timestamp { get; set; }

    }
}

