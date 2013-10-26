namespace IdSharp.Tagging.ID3v2.Frames
{
    using System;
    using System.ComponentModel;

    public interface ITXXXFrame : ITextFrame, IFrame, INotifyPropertyChanged, ITextEncoding
    {
        string Description { get; set; }

    }
}

