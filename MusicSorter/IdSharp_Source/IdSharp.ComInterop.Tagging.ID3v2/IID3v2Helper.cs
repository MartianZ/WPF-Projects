namespace IdSharp.ComInterop.Tagging.ID3v2
{
    using IdSharp.Tagging.ID3v2;
    using System;
    using System.IO;
    using System.Runtime.InteropServices;

    [Guid("D104A69F-F6B3-4a44-980B-7FBF4BA81623"), ComVisible(true)]
    public interface IID3v2Helper
    {
        IID3v2 CreateID3v2();
        IID3v2 CreateID3v2FromFile(string path);
        IID3v2 CreateID3v2FromStream(Stream stream);
        int GetTagSizeFromStream(Stream stream);
        int GetTagSize(string path);
        bool DoesTagExistInStream(Stream stream);
        bool DoesTagExist(string path);
        bool RemoveTag(string path);
    }
}

