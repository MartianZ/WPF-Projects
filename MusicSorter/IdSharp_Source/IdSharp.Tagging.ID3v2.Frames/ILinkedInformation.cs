namespace IdSharp.Tagging.ID3v2.Frames
{
    using System;
    using System.ComponentModel;

    public interface ILinkedInformation : IFrame, INotifyPropertyChanged
    {
        byte[] AdditionalData { get; set; }

        string FrameIdentifier { get; set; }

        string Url { get; set; }

    }
}

