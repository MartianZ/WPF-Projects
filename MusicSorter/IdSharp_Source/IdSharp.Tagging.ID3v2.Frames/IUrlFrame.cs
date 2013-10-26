namespace IdSharp.Tagging.ID3v2.Frames
{
    using System;
    using System.ComponentModel;

    public interface IUrlFrame : IFrame, INotifyPropertyChanged
    {
        string Value { get; set; }

    }
}

