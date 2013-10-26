namespace IdSharp.Tagging.ID3v2.Frames
{
    using System;
    using System.ComponentModel;

    public interface IMpegLookupTable : IFrame, INotifyPropertyChanged
    {
        int BytesBetweenReference { get; set; }

        int FramesBetweenReference { get; set; }

        BindingList<IMpegLookupTableItem> Items { get; }

        int MillisecondsBetweenReference { get; set; }

    }
}

