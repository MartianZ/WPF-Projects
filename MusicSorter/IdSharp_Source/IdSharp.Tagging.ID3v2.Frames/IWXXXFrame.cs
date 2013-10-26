namespace IdSharp.Tagging.ID3v2.Frames
{
    using System;
    using System.ComponentModel;

    public interface IWXXXFrame : IFrame, INotifyPropertyChanged, ITextEncoding
    {
        string Description { get; set; }

        string Value { get; set; }

    }
}

