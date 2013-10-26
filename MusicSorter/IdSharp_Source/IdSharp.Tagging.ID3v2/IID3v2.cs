namespace IdSharp.Tagging.ID3v2
{
    using IdSharp.Utils;
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.InteropServices;

    [Guid("2E3A01D6-C3DE-4e43-B67F-F3E067C7FF23"), ComVisible(true)]
    public interface IID3v2 : IFrameContainer, INotifyPropertyChanged, INotifyInvalidData
    {
        [DispId(0x65)]
        IID3v2Header Header { get; }
        [DispId(0x66)]
        IID3v2ExtendedHeader ExtendedHeader { get; }
        [DispId(0x67)]
        void Read(string path);
        [DispId(0x68)]
        void ReadStream(Stream stream);
        [DispId(0x69)]
        void Save(string path);
        [DispId(0x6a)]
        void SaveEncoding(string path, EncodingType encodingType);
        [DispId(0x6b)]
        byte[] GetBytes(int minimumSize);
    }
}

