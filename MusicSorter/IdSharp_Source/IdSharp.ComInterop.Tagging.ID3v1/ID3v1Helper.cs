namespace IdSharp.ComInterop.Tagging.ID3v1
{
    using IdSharp.Tagging.ID3v1;
    using System;
    using System.IO;
    using System.Runtime.InteropServices;

    [ClassInterface(ClassInterfaceType.None), Guid("C8D0F2FF-634D-4484-A045-FE5EF19A1B2C"), ComVisible(true)]
    public class ID3v1Helper : IID3v1Helper
    {
        public IID3v1 CreateID3v1()
        {
            return IdSharp.Tagging.ID3v1.ID3v1Helper.CreateID3v1();
        }

        public IID3v1 CreateID3v1FromFile(string path)
        {
            return IdSharp.Tagging.ID3v1.ID3v1Helper.CreateID3v1(path);
        }

        public IID3v1 CreateID3v1FromStream(Stream stream)
        {
            return IdSharp.Tagging.ID3v1.ID3v1Helper.CreateID3v1(stream);
        }

        public bool DoesTagExist(string path)
        {
            return IdSharp.Tagging.ID3v1.ID3v1Helper.DoesTagExist(path);
        }

        public bool DoesTagExistInStream(Stream stream)
        {
            return IdSharp.Tagging.ID3v1.ID3v1Helper.DoesTagExist(stream);
        }

        public int GetTagSize(string path)
        {
            return IdSharp.Tagging.ID3v1.ID3v1Helper.GetTagSize(path);
        }

        public int GetTagSizeFromStream(Stream stream)
        {
            return IdSharp.Tagging.ID3v1.ID3v1Helper.GetTagSize(stream);
        }

        public bool RemoveTag(string path)
        {
            return IdSharp.Tagging.ID3v1.ID3v1Helper.RemoveTag(path);
        }

    }
}

