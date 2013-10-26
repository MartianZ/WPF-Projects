namespace IdSharp.Tagging.ID3v2.Frames
{
    using System;
    using System.ComponentModel;

    public interface IMusicCDIdentifier : IFrame, INotifyPropertyChanged
    {
        byte[] TOC { get; set; }

    }
}

