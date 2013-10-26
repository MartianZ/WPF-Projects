namespace IdSharp.Tagging.ID3v2.Frames
{
    using System;
    using System.ComponentModel;

    public interface ITextFrame : IFrame, INotifyPropertyChanged, ITextEncoding
    {
        string Value { get; set; }

    }
}

