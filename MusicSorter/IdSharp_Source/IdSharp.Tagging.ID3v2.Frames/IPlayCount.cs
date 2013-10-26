namespace IdSharp.Tagging.ID3v2.Frames
{
    using System;
    using System.ComponentModel;

    public interface IPlayCount : IFrame, INotifyPropertyChanged
    {
        Nullable<long> Value { get; set; }

    }
}

