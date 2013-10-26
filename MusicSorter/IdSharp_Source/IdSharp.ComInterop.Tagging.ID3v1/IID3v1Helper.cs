namespace IdSharp.ComInterop.Tagging.ID3v1
{
    using IdSharp.Tagging.ID3v1;
    using System;
    using System.IO;
    using System.Runtime.InteropServices;

    [ComVisible(true), Guid("F83D0B22-8304-4b2b-82A7-59F68B63B1CC")]
    public interface IID3v1Helper
    {
        IID3v1 CreateID3v1();
        IID3v1 CreateID3v1FromFile(string path);
        IID3v1 CreateID3v1FromStream(Stream stream);
        int GetTagSizeFromStream(Stream stream);
        int GetTagSize(string path);
        bool DoesTagExistInStream(Stream stream);
        bool DoesTagExist(string path);
        bool RemoveTag(string path);
    }
}

