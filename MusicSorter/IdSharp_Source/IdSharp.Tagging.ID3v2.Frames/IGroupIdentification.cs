namespace IdSharp.Tagging.ID3v2.Frames
{
    using System;
    using System.ComponentModel;

    public interface IGroupIdentification : IFrame, INotifyPropertyChanged
    {
        byte[] GroupDependentData { get; set; }

        byte GroupSymbol { get; set; }

        string OwnerIdentifier { get; set; }

    }
}

