namespace IdSharp.Tagging.ID3v2.Frames
{
    using System;
    using System.ComponentModel;

    public interface IComments : IFrame, INotifyPropertyChanged, ITextEncoding
    {
        string Description { get; set; }

        string LanguageCode { get; set; }

        string Value { get; set; }

    }
}

