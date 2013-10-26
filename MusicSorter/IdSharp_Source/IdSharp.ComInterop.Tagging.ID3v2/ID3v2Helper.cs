namespace IdSharp.ComInterop.Tagging.ID3v2
{
    using IdSharp.Tagging.ID3v2;
    using System;
    using System.IO;
    using System.Runtime.InteropServices;

    [ClassInterface(ClassInterfaceType.None), ComVisible(true), Guid("7D36BAD1-7FB0-47a7-98AB-4DE12BD512F6")]
    public class ID3v2Helper : IID3v2Helper
    {
        public IID3v2 CreateID3v2()
        {
            return IdSharp.Tagging.ID3v2.ID3v2Helper.CreateID3v2();
        }

        public IID3v2 CreateID3v2FromFile(string path)
        {
            return IdSharp.Tagging.ID3v2.ID3v2Helper.CreateID3v2(path);
        }

        public IID3v2 CreateID3v2FromStream(Stream stream)
        {
            return IdSharp.Tagging.ID3v2.ID3v2Helper.CreateID3v2(stream);
        }

        public bool DoesTagExist(string path)
        {
            return IdSharp.Tagging.ID3v2.ID3v2Helper.DoesTagExist(path);
        }

        public bool DoesTagExistInStream(Stream stream)
        {
            return IdSharp.Tagging.ID3v2.ID3v2Helper.DoesTagExist(stream);
        }

        public int GetTagSize(string path)
        {
            return IdSharp.Tagging.ID3v2.ID3v2Helper.GetTagSize(path);
        }

        public int GetTagSizeFromStream(Stream stream)
        {
            return IdSharp.Tagging.ID3v2.ID3v2Helper.GetTagSize(stream);
        }

        public bool RemoveTag(string path)
        {
            return IdSharp.Tagging.ID3v2.ID3v2Helper.RemoveTag(path);
        }

    }
}

