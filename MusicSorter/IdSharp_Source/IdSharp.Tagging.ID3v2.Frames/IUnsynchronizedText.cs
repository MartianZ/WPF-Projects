namespace IdSharp.Tagging.ID3v2.Frames
{
    using System;
    using System.ComponentModel;

    public interface IUnsynchronizedText : IFrame, INotifyPropertyChanged, ITextEncoding
    {
        string ContentDescriptor { get; set; }

        string LanguageCode { get; set; }

        string Text { get; set; }

    }
}

