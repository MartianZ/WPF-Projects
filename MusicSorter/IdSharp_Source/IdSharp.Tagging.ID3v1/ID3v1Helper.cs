namespace IdSharp.Tagging.ID3v1
{
    using IdSharp.Utils;
    using System;
    using System.IO;

    public static class ID3v1Helper
    {
        public static IID3v1 CreateID3v1()
        {
            return new IdSharp.Tagging.ID3v1.ID3v1();
        }

        public static IID3v1 CreateID3v1(Stream stream)
        {
            Guard.ArgumentNotNull(stream, "stream");
            IID3v1 iidv1 = new IdSharp.Tagging.ID3v1.ID3v1();
            iidv1.ReadStream(stream);
            return iidv1;
        }

        public static IID3v1 CreateID3v1(string path)
        {
            Guard.ArgumentNotNullOrEmptyString(path, "path");
            IID3v1 iidv1 = new IdSharp.Tagging.ID3v1.ID3v1();
            iidv1.Read(path);
            return iidv1;
        }

        public static bool DoesTagExist(Stream stream)
        {
            Guard.ArgumentNotNull(stream, "stream");
            return (ID3v1Helper.GetTagSize(stream) != 0);
        }

        public static bool DoesTagExist(string path)
        {
            Guard.ArgumentNotNull(path, "path");
            return (ID3v1Helper.GetTagSize(path) != 0);
        }

        public static int GetTagSize(Stream stream)
        {
            Guard.ArgumentNotNull(stream, "stream");
            if (stream.Length >= 0x80)
            {
                stream.Seek((long) (-128), SeekOrigin.End);
                byte[] buffer1 = new byte[3];
                stream.Read(buffer1, 0, 3);
                if (((buffer1[0] == 0x54) && (buffer1[1] == 0x41)) && (buffer1[2] == 0x47))
                {
                    return 0x80;
                }
            }
            return 0;
        }

        public static int GetTagSize(string path)
        {
            Guard.ArgumentNotNullOrEmptyString(path, "path");
            using (FileStream stream1 = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                return ID3v1Helper.GetTagSize(stream1);
            }
        }

        public static bool RemoveTag(string path)
        {
            Guard.ArgumentNotNull(path, "path");
            using (FileStream stream1 = File.Open(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
            {
                if (!ID3v1Helper.DoesTagExist(stream1))
                {
                    return false;
                }
                stream1.SetLength(stream1.Length - 0x80);
            }
            return true;
        }

    }
}

