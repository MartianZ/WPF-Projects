namespace IdSharp.Tagging.ID3v2.Frames
{
    using System;
    using System.ComponentModel;

    public interface IAudioEncryption : IFrame, INotifyPropertyChanged
    {
        byte[] EncryptionInfo { get; set; }

        string OwnerIdentifier { get; set; }

        short PreviewLength { get; set; }

        short PreviewStart { get; set; }

    }
}

