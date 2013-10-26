namespace IdSharp.Utils
{
    using System;
    using System.IO;

    public static class CRC32
    {
        static CRC32()
        {
            CRC32.crc32Table = new uint[0x100];
            uint num1 = 0xedb88320;
            for (uint num2 = 0; num2 < 0x100; num2 += 1)
            {
                uint num3 = num2;
                for (uint num4 = 8; num4 > 0; num4 -= 1)
                {
                    if ((num3 & 1) == 1)
                    {
                        num3 = (num3 >> 1) ^ num1;
                    }
                    else
                    {
                        num3 = num3 >> 1;
                    }
                }
                CRC32.crc32Table[(int) ((IntPtr) num2)] = num3;
            }
        }

        public static string Calculate(Stream stream)
        {
            return string.Format("{0:X8}", (uint) CRC32.CalculateInt32(stream));
        }

        public static string Calculate(string path)
        {
            return string.Format("{0:X8}", (uint) CRC32.CalculateInt32(path));
        }

        public static string Calculate(byte[] data)
        {
            return string.Format("{0:X8}", (uint) CRC32.CalculateInt32(data));
        }

        public static int CalculateInt32(Stream stream)
        {
            stream.Position = 0;
            uint num1 = uint.MaxValue;
            byte[] buffer1 = new byte[0x400];
            for (int num2 = stream.Read(buffer1, 0, 0x400); num2 > 0; num2 = stream.Read(buffer1, 0, 0x400))
            {
                for (int num3 = 0; num3 < num2; num3++)
                {
                    num1 = (num1 >> 8) ^ CRC32.crc32Table[(int) ((IntPtr) (buffer1[num3] ^ (num1 & 0xff)))];
                }
            }
            return (int) ~num1;
        }

        public static int CalculateInt32(string path)
        {
            using (FileStream stream1 = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                return CRC32.CalculateInt32(stream1);
            }
        }

        public static int CalculateInt32(byte[] data)
        {
            using (MemoryStream stream1 = new MemoryStream(data))
            {
                return CRC32.CalculateInt32(stream1);
            }
        }


        private const int BUFFER_SIZE = 0x400;
        private static readonly uint[] crc32Table;
    }
}

