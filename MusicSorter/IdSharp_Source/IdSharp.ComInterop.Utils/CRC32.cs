namespace IdSharp.ComInterop.Utils
{
    using IdSharp.Utils;
    using System;
    using System.IO;
    using System.Runtime.InteropServices;

    [ComVisible(true), ClassInterface(ClassInterfaceType.None), Guid("16F30935-7830-4776-B612-75A814A48F0C")]
    public class CRC32 : ICRC32
    {
        public string CalculateFromByteArray(byte[] data)
        {
            return IdSharp.Utils.CRC32.Calculate(data);
        }

        public string CalculateFromFile(string path)
        {
            return IdSharp.Utils.CRC32.Calculate(path);
        }

        public string CalculateFromStream(Stream stream)
        {
            return IdSharp.Utils.CRC32.Calculate(stream);
        }

        public int CalculateInt32FromByteArray(byte[] data)
        {
            return IdSharp.Utils.CRC32.CalculateInt32(data);
        }

        public int CalculateInt32FromFile(string path)
        {
            return IdSharp.Utils.CRC32.CalculateInt32(path);
        }

        public int CalculateInt32FromStream(Stream stream)
        {
            return IdSharp.Utils.CRC32.CalculateInt32(stream);
        }

    }
}

