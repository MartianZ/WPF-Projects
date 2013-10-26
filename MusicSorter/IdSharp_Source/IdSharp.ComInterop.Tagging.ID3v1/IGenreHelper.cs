namespace IdSharp.ComInterop.Tagging.ID3v1
{
    using System;
    using System.Runtime.InteropServices;

    [ComVisible(true), Guid("932ABF8E-8740-45d3-AB29-52C731331852")]
    public interface IGenreHelper
    {
        [DispId(1)]
        string[] GenreByIndex { get; }
        [DispId(2)]
        int GetGenreIndex(string genre);
    }
}

