namespace IdSharp.Tagging.ID3v2.Frames
{
    using System;
    using System.ComponentModel;

    public interface ISeekNextTag : IFrame, INotifyPropertyChanged
    {
        int MinimumOffsetToNextTag { get; set; }

    }
}

