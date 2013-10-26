namespace IdSharp.Tagging.ID3v2.Frames
{
    using System;
    using System.ComponentModel;

    public interface IPrivateFrame : IFrame, INotifyPropertyChanged
    {
        string OwnerIdentifier { get; set; }

        byte[] PrivateData { get; set; }

    }
}

