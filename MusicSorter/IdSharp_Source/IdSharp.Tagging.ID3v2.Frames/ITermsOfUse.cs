namespace IdSharp.Tagging.ID3v2.Frames
{
    using System;
    using System.ComponentModel;

    public interface ITermsOfUse : IFrame, INotifyPropertyChanged, ITextEncoding
    {
        string LanguageCode { get; set; }

        string Value { get; set; }

    }
}

