namespace IdSharp.Tagging.ID3v2.Frames
{
    using System;
    using System.ComponentModel;

    public interface IPopularimeter : IFrame, INotifyPropertyChanged
    {
        long PlayCount { get; set; }

        byte Rating { get; set; }

        string UserEmail { get; set; }

    }
}

