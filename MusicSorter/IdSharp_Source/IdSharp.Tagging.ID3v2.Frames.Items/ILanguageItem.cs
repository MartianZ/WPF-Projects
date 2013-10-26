namespace IdSharp.Tagging.ID3v2.Frames.Items
{
    using System;
    using System.ComponentModel;

    public interface ILanguageItem : INotifyPropertyChanged
    {
        string LanguageCode { get; set; }

        string LanguageDisplay { get; }

    }
}

