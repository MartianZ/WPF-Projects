namespace IdSharp.Tagging.ID3v2.Frames
{
    using System;
    using System.ComponentModel;

    public interface IUniqueFileIdentifier : IFrame, INotifyPropertyChanged
    {
        byte[] Identifier { get; set; }

        string OwnerIdentifier { get; set; }

    }
}

