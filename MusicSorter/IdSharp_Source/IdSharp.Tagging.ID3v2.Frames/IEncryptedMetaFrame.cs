namespace IdSharp.Tagging.ID3v2.Frames
{
    using System;
    using System.ComponentModel;

    public interface IEncryptedMetaFrame : IFrame, INotifyPropertyChanged
    {
        string ContentExplanation { get; set; }

        byte[] EncryptedData { get; set; }

        string OwnerIdentifier { get; set; }

    }
}

