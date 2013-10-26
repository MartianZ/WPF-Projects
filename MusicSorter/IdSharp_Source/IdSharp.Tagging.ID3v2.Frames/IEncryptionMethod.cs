namespace IdSharp.Tagging.ID3v2.Frames
{
    using System;
    using System.ComponentModel;

    public interface IEncryptionMethod : IFrame, INotifyPropertyChanged
    {
        byte[] EncryptionData { get; set; }

        byte MethodSymbol { get; set; }

        string OwnerIdentifier { get; set; }

    }
}

