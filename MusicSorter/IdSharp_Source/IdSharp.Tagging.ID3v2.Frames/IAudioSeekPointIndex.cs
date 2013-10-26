namespace IdSharp.Tagging.ID3v2.Frames
{
    using System;
    using System.ComponentModel;

    public interface IAudioSeekPointIndex : IFrame, INotifyPropertyChanged
    {
        byte BitsPerIndexPoint { get; set; }

        BindingList<short> FractionAtIndex { get; }

        int IndexedDataLength { get; set; }

        int IndexedDataStart { get; set; }

    }
}

