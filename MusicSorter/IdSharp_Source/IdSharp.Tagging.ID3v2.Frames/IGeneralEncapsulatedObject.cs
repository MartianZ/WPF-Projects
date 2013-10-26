namespace IdSharp.Tagging.ID3v2.Frames
{
    using System;
    using System.ComponentModel;

    public interface IGeneralEncapsulatedObject : IFrame, INotifyPropertyChanged, ITextEncoding
    {
        string Description { get; set; }

        byte[] EncapsulatedObject { get; set; }

        string FileName { get; set; }

        string MimeType { get; set; }

    }
}

