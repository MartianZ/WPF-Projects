namespace IdSharp.Tagging.ID3v2.Frames
{
    using System;
    using System.ComponentModel;

    public interface ISignature : IFrame, INotifyPropertyChanged
    {
        byte GroupSymbol { get; set; }

        byte[] SignatureData { get; set; }

    }
}

