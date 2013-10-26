namespace IdSharp.Tagging.ID3v2.Frames
{
    using IdSharp.Tagging.ID3v2;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.InteropServices;

    [ComVisible(true), Guid("6887CCDE-7BBB-4f3c-9FFA-36594B36EC4F")]
    public interface IAttachedPicture : IFrame, INotifyPropertyChanged, ITextEncoding
    {
        [DispId(1)]
        string MimeType { get; set; }
        [DispId(2)]
        IdSharp.Tagging.ID3v2.PictureType PictureType { get; set; }
        [DispId(3)]
        string Description { get; set; }
        [DispId(4)]
        byte[] PictureData { get; set; }
        [DispId(5)]
        Image Picture { get; set; }
        [DispId(6)]
        string PictureExtension { get; }
    }
}

