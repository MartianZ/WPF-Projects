namespace IdSharp.Tagging.ID3v2.Frames.Items
{
    using System;
    using System.ComponentModel;

    public interface ISynchronizedTextItem : INotifyPropertyChanged
    {
        string Text { get; set; }

        int Timestamp { get; set; }

    }
}

