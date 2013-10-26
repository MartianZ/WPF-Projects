namespace IdSharp.Tagging.ID3v2.Frames.Items
{
    using System;
    using System.ComponentModel;

    public interface IMusicianCreditsItem : INotifyPropertyChanged
    {
        string Artists { get; set; }

        string Instrument { get; set; }

    }
}

