namespace IdSharp.Tagging.ID3v2.Frames
{
    using IdSharp.Tagging.ID3v2;
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.InteropServices;

    [Guid("566A1DEE-D0F6-45a0-A215-0769893E5410"), ComVisible(true)]
    public interface IFrame : INotifyPropertyChanged
    {
        [DispId(0x3e8)]
        IFrameHeader FrameHeader { get; }
        [DispId(0x3e9)]
        string GetFrameID(ID3v2TagVersion tagVersion);
        [DispId(0x3ea)]
        void Read(TagReadingInfo tagReadingInfo, Stream stream);
        [DispId(0x3eb)]
        byte[] GetBytes(ID3v2TagVersion tagVersion);
    }
}

