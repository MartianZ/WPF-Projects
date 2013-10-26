namespace IdSharp.Tagging.ID3v2
{
    using System;
    using System.IO;

    public static class ID3v2Helper
    {
        public static IID3v2 CreateID3v2()
        {
            return new IdSharp.Tagging.ID3v2.ID3v2();
        }

        public static IID3v2 CreateID3v2(Stream stream)
        {
            IID3v2 iidv1 = new IdSharp.Tagging.ID3v2.ID3v2();
            iidv1.ReadStream(stream);
            return iidv1;
        }

        public static IID3v2 CreateID3v2(string path)
        {
            IID3v2 iidv1 = new IdSharp.Tagging.ID3v2.ID3v2();
            iidv1.Read(path);
            return iidv1;
        }

        public static bool DoesTagExist(Stream stream)
        {
            return (ID3v2Helper.GetTagSize(stream) != 0);
        }

        public static bool DoesTagExist(string path)
        {
            return (ID3v2Helper.GetTagSize(path) != 0);
        }

        public static int GetTagSize(Stream stream)
        {
            if (stream.Length >= 0x10)
            {
                stream.Position = 0;
                byte[] buffer1 = Utils.Read(stream, 3);
                if (((buffer1[0] != 0x49) || (buffer1[1] != 0x44)) || (buffer1[2] != 0x33))
                {
                    return 0;
                }
                IID3v2Header header1 = new ID3v2Header(stream, false);
                int num1 = header1.TagSize;
                if (num1 != 0)
                {
                    return ((num1 + 10) + (header1.IsFooterPresent ? 10 : 0));
                }
            }
            return 0;
        }

        public static int GetTagSize(string path)
        {
            using (FileStream stream1 = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                return ID3v2Helper.GetTagSize(stream1);
            }
        }

        public static bool RemoveTag(string path)
        {
            int num1 = ID3v2Helper.GetTagSize(path);
            if (num1 > 0)
            {
                Utils.ReplaceBytes(path, num1, new byte[0]);
                return true;
            }
            return false;
        }

    }
}

