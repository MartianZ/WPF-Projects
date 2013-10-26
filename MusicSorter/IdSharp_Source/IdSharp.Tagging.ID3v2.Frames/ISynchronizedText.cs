namespace IdSharp.Tagging.ID3v2.Frames
{
    using IdSharp.Tagging.ID3v2;
    using System;
    using System.ComponentModel;

    public interface ISynchronizedText : IFrame, INotifyPropertyChanged, ITextEncoding
    {
        string ContentDescriptor { get; set; }

        TextContentType ContentType { get; set; }

        BindingList<ISynchronizedTextItem> Items { get; }

        string LanguageCode { get; set; }

        IdSharp.Tagging.ID3v2.TimestampFormat TimestampFormat { get; set; }

    }
}

