namespace IdSharp.Tagging.ID3v1
{
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.InteropServices;

    [ComVisible(true), Guid("BF70B165-8790-4930-A2AA-C960503129D0")]
    public interface IID3v1 : INotifyPropertyChanged
    {
        [DispId(1)]
        string Title { get; set; }
        [DispId(2)]
        string Artist { get; set; }
        [DispId(3)]
        string Album { get; set; }
        [DispId(4)]
        string Year { get; set; }
        [DispId(5)]
        string Comment { get; set; }
        [DispId(6)]
        int TrackNumber { get; set; }
        [DispId(7)]
        int GenreIndex { get; set; }
        [DispId(8)]
        void Read(string path);
        [DispId(9)]
        void ReadStream(Stream stream);
        [DispId(10)]
        void Save(string path);
        [DispId(11)]
        void Reset();
        [DispId(12)]
        ID3v1TagVersion TagVersion { get; set; }
    }
}

