namespace IdSharp.Tagging.ID3v2.Frames
{
    using System;
    using System.ComponentModel;

    public interface IRecommendedBufferSize : IFrame, INotifyPropertyChanged
    {
        int BufferSize { get; set; }

        bool EmbeddedInfo { get; set; }

        Nullable<int> OffsetToNextTag { get; set; }

    }
}

