namespace IdSharp.Tagging.ID3v2.Frames.Items
{
    using System;
    using System.ComponentModel;

    public interface IMpegLookupTableItem : INotifyPropertyChanged
    {
        long DeviationInBytes { get; set; }

        long DeviationInMilliseconds { get; set; }

    }
}

