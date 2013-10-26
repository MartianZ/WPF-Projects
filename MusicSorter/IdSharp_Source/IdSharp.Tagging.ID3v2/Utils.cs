namespace IdSharp.Tagging.ID3v2
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Text;
    using zlib;

    internal static class Utils
    {
        static Utils()
        {
            Utils.m_ISO88591 = Encoding.GetEncoding(0x6faf);
        }

        public static long ConvertToInt64(byte[] byteArray)
        {
            long num1 = 0;
            for (int num2 = 0; num2 < byteArray.Length; num2++)
            {
                num1 = num1 << 8;
                num1 += byteArray[num2];
            }
            return num1;
        }

        public static byte[] ConvertToUnsynchronized(byte[] data)
        {
            using (MemoryStream stream1 = new MemoryStream((int) (data.Length * 1.05)))
            {
                for (int num1 = 0; num1 < data.Length; num1++)
                {
                    stream1.WriteByte(data[num1]);
                    if (((data[num1] == 0xff) && (num1 != (data.Length - 1))) && ((data[num1 + 1] == 0) || ((data[num1 + 1] & 0xe0) == 0xe0)))
                    {
                        stream1.WriteByte(0);
                    }
                }
                return stream1.ToArray();
            }
        }

        private static void CopyStream(Stream input, Stream output, int size)
        {
            byte[] buffer1 = new byte[size];
            input.Read(buffer1, 0, size);
            output.Write(buffer1, 0, size);
            output.Flush();
        }

        public static Stream DecompressFrame(Stream stream, int compressedSize)
        {
            Stream stream1 = new MemoryStream();
            ZOutputStream stream2 = new ZOutputStream(stream1);
            Utils.CopyStream(stream, stream2, compressedSize);
            stream1.Position = 0;
            return stream1;
        }

        public static byte[] Get2Bytes(short value)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException("value", value, "Value cannot be less than 0");
            }
            return Utils.Get2Bytes((ushort) value);
        }

        public static byte[] Get2Bytes(ushort value)
        {
            return new byte[] { ((byte) ((value >> 8) & 0xff)), ((byte) (value & 0xff)) };
        }

        public static byte[] Get4Bytes(int value)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException("value", value, "Value cannot be less than 0");
            }
            return Utils.Get4Bytes((uint) value);
        }

        public static byte[] Get4Bytes(uint value)
        {
            return new byte[] { ((byte) ((value >> 0x18) & 0xff)), ((byte) ((value >> 0x10) & 0xff)), ((byte) ((value >> 8) & 0xff)), ((byte) (value & 0xff)) };
        }

        public static byte[] Get8Bytes(ulong value)
        {
            return new byte[] { ((byte) ((value >> 0x38) & 0xff)), ((byte) ((value >> 0x30) & 0xff)), ((byte) ((value >> 40) & 0xff)), ((byte) ((value >> 0x20) & 0xff)), ((byte) ((value >> 0x18) & 0xff)), ((byte) ((value >> 0x10) & 0xff)), ((byte) ((value >> 8) & 0xff)), ((byte) (value & 0xff)) };
        }

        public static byte[] GetBytesDecimal(decimal decimalValue, int bytes)
        {
            byte[] buffer1 = Utils.GetBytesMinimal((ulong) decimalValue);
            if (buffer1.Length == bytes)
            {
                return buffer1;
            }
            if (buffer1.Length > bytes)
            {
                byte[] buffer2 = new byte[bytes];
                int num1 = buffer1.Length - bytes;
                for (int num2 = 0; num1 < buffer1.Length; num2++)
                {
                    buffer2[num2] = buffer1[num1];
                    num1++;
                }
                return buffer2;
            }
            byte[] buffer3 = new byte[bytes];
            int num3 = bytes - buffer1.Length;
            for (int num4 = 0; num3 < bytes; num4++)
            {
                buffer3[num3] = buffer1[num4];
                num3++;
            }
            return buffer3;
        }

        public static byte[] GetBytesMinimal(long value)
        {
            return Utils.GetBytesMinimal((ulong) value);
        }

        public static byte[] GetBytesMinimal(ulong value)
        {
            if (value <= 0xff)
            {
                return new byte[] { ((byte) value) };
            }
            if (value <= 0xffff)
            {
                return Utils.Get2Bytes((ushort) value);
            }
            if (value <= 0xffffffff)
            {
                return Utils.Get4Bytes((uint) value);
            }
            return Utils.Get8Bytes(value);
        }

        public static byte[] GetStringBytes(ID3v2TagVersion tagVersion, EncodingType encodingType, string value, bool isTerminated)
        {
            List<byte> list1 = new List<byte>();
            switch (tagVersion)
            {
                case ID3v2TagVersion.ID3v22:
                {
                    EncodingType type1 = encodingType;
                    if (type1 == EncodingType.Unicode)
                    {
                        if (!string.IsNullOrEmpty(value))
                        {
                            list1.Add(0xff);
                            list1.Add(0xfe);
                            list1.AddRange(Encoding.Unicode.GetBytes(value));
                        }
                        if (isTerminated)
                        {
                            list1.AddRange(new byte[2]);
                        }
                        break;
                    }
                    list1.AddRange(Utils.ISO88591GetBytes(value));
                    if (isTerminated)
                    {
                        list1.Add(0);
                    }
                    break;
                }
                case ID3v2TagVersion.ID3v23:
                {
                    EncodingType type2 = encodingType;
                    if (type2 != EncodingType.Unicode)
                    {
                        list1.AddRange(Utils.ISO88591GetBytes(value));
                        if (isTerminated)
                        {
                            list1.Add(0);
                        }
                        break;
                    }
                    if (!string.IsNullOrEmpty(value))
                    {
                        list1.Add(0xff);
                        list1.Add(0xfe);
                        list1.AddRange(Encoding.Unicode.GetBytes(value));
                    }
                    if (isTerminated)
                    {
                        list1.AddRange(new byte[2]);
                    }
                    break;
                }
                case ID3v2TagVersion.ID3v24:
                    switch (encodingType)
                    {
                        case EncodingType.Unicode:
                            if (!string.IsNullOrEmpty(value))
                            {
                                list1.Add(0xff);
                                list1.Add(0xfe);
                                list1.AddRange(Encoding.Unicode.GetBytes(value));
                            }
                            if (isTerminated)
                            {
                                list1.AddRange(new byte[2]);
                            }
                            break;

                        case EncodingType.UTF16BE:
                            if (!string.IsNullOrEmpty(value))
                            {
                                list1.AddRange(Encoding.BigEndianUnicode.GetBytes(value));
                            }
                            if (isTerminated)
                            {
                                list1.AddRange(new byte[2]);
                            }
                            break;

                        case EncodingType.UTF8:
                            if (!string.IsNullOrEmpty(value))
                            {
                                list1.AddRange(Encoding.UTF8.GetBytes(value));
                            }
                            if (isTerminated)
                            {
                                list1.Add(0);
                            }
                            break;
                    }
                    list1.AddRange(Utils.ISO88591GetBytes(value));
                    if (isTerminated)
                    {
                        list1.Add(0);
                    }
                    break;

                default:
                    throw new ArgumentException("Unknown tag version");
            }
            return list1.ToArray();
        }

        public static bool IsBitSet(byte byteToCheck, byte bitToCheck)
        {
            return (((byteToCheck >> (bitToCheck & 0x1f)) & 1) == 1);
        }

        public static byte[] ISO88591GetBytes(string value)
        {
            if (value == null)
            {
                return new byte[0];
            }
            return Utils.m_ISO88591.GetBytes(value);
        }

        public static string ISO88591GetString(byte[] value)
        {
            if (value == null)
            {
                return string.Empty;
            }
            return Utils.m_ISO88591.GetString(value);
        }

        public static byte[] Read(Stream stream, int count)
        {
            byte[] buffer1 = new byte[count];
            if (stream.Read(buffer1, 0, count) != count)
            {
                string text1 = string.Format("Attempted to read past the end of the stream when requesting {0} bytes at position {1}", count, stream.Position);
                Trace.WriteLine(text1);
                throw new InvalidDataException(text1);
            }
            return buffer1;
        }

        public static byte[] Read(Stream stream, int count, ref int bytesLeft)
        {
            bytesLeft -= count;
            return Utils.Read(stream, count);
        }

        public static byte ReadByte(Stream stream)
        {
            int num1 = stream.ReadByte();
            if (num1 == -1)
            {
                string text1 = string.Format("Attempted to read past the end of the stream at position {0}", stream.Position);
                Trace.WriteLine(text1);
                throw new InvalidDataException(text1);
            }
            return (byte) num1;
        }

        public static byte ReadByte(Stream stream, ref int bytesLeft)
        {
            if (bytesLeft > 0)
            {
                bytesLeft -= 1;
                return Utils.ReadByte(stream);
            }
            string text1 = string.Format("Attempted to read past the end of the frame at position {0}", stream.Position);
            Trace.WriteLine(text1);
            throw new InvalidDataException(text1);
        }

        public static short ReadInt16(Stream stream, ref int bytesLeft)
        {
            byte[] buffer1 = Utils.Read(stream, 2);
            bytesLeft -= 2;
            return (short) ((buffer1[0] << 8) + buffer1[1]);
        }

        public static int ReadInt24(Stream stream)
        {
            byte[] buffer1 = Utils.Read(stream, 3);
            return (((buffer1[0] << 0x10) + (buffer1[1] << 8)) + buffer1[2]);
        }

        public static int ReadInt24Unsynchronized(Stream stream)
        {
            byte[] buffer1 = Utils.ReadUnsynchronized(stream, 3);
            return (((buffer1[0] << 0x10) + (buffer1[1] << 8)) + buffer1[2]);
        }

        public static int ReadInt32(Stream stream)
        {
            byte[] buffer1 = Utils.Read(stream, 4);
            return ((((buffer1[0] << 0x18) + (buffer1[1] << 0x10)) + (buffer1[2] << 8)) + buffer1[3]);
        }

        public static int ReadInt32SyncSafe(Stream stream)
        {
            byte[] buffer1 = Utils.Read(stream, 4);
            return ((((buffer1[0] << 0x15) + (buffer1[1] << 14)) + (buffer1[2] << 7)) + buffer1[3]);
        }

        public static int ReadInt32Unsynchronized(Stream stream)
        {
            byte[] buffer1 = Utils.ReadUnsynchronized(stream, 4);
            return ((((buffer1[0] << 0x18) + (buffer1[1] << 0x10)) + (buffer1[2] << 8)) + buffer1[3]);
        }

        public static string ReadString(EncodingType textEncoding, Stream stream, ref int bytesLeft)
        {
            string text1;
            if (bytesLeft > 0)
            {
                List<byte> list1 = new List<byte>();
                if (textEncoding != EncodingType.ISO88591)
                {
                    if (textEncoding == EncodingType.Unicode)
                    {
                        byte num2;
                        byte num3;
                        do
                        {
                            num2 = Utils.ReadByte(stream);
                            list1.Add(num2);
                            bytesLeft -= 1;
                            if (bytesLeft == 0)
                            {
                                return "";
                            }
                            num3 = Utils.ReadByte(stream);
                            list1.Add(num3);
                            bytesLeft -= 1;
                        }
                        while ((bytesLeft != 0) && ((num2 != 0) || (num3 != 0)));
                        byte[] buffer1 = list1.ToArray();
                        if (buffer1.Length >= 2)
                        {
                            if ((buffer1[0] == 0xff) && (buffer1[1] == 0xfe))
                            {
                                text1 = Encoding.Unicode.GetString(buffer1, 2, buffer1.Length - 2);
                                goto Label_027D;
                            }
                            if ((buffer1[0] == 0xfe) && (buffer1[1] == 0xff))
                            {
                                text1 = Encoding.BigEndianUnicode.GetString(buffer1, 2, buffer1.Length - 2);
                                goto Label_027D;
                            }
                            text1 = Encoding.Unicode.GetString(buffer1, 0, buffer1.Length);
                            goto Label_027D;
                        }
                        text1 = Encoding.Unicode.GetString(buffer1, 0, buffer1.Length);
                        goto Label_027D;
                    }
                    if (textEncoding == EncodingType.UTF16BE)
                    {
                        byte num4;
                        byte num5;
                        do
                        {
                            num4 = Utils.ReadByte(stream);
                            list1.Add(num4);
                            bytesLeft -= 1;
                            if (bytesLeft == 0)
                            {
                                return "";
                            }
                            num5 = Utils.ReadByte(stream);
                            list1.Add(num5);
                            bytesLeft -= 1;
                        }
                        while ((bytesLeft != 0) && ((num4 != 0) || (num5 != 0)));
                        byte[] buffer2 = list1.ToArray();
                        if (buffer2.Length >= 2)
                        {
                            if ((buffer2[0] == 0xfe) && (buffer2[1] == 0xff))
                            {
                                text1 = Encoding.BigEndianUnicode.GetString(buffer2, 2, buffer2.Length - 2);
                                goto Label_027D;
                            }
                            text1 = Encoding.BigEndianUnicode.GetString(buffer2, 0, buffer2.Length);
                            goto Label_027D;
                        }
                        text1 = Encoding.BigEndianUnicode.GetString(buffer2, 0, buffer2.Length);
                        goto Label_027D;
                    }
                    if (textEncoding != EncodingType.UTF8)
                    {
                        string text2 = string.Format("Text Encoding '{0}' unknown at position {1}", textEncoding, stream.Position);
                        Trace.WriteLine(text2);
                        return "";
                    }
                    byte num6 = Utils.ReadByte(stream);
                    bytesLeft -= 1;
                    if (bytesLeft != 0)
                    {
                        while (num6 != 0)
                        {
                            list1.Add(num6);
                            num6 = Utils.ReadByte(stream);
                            bytesLeft -= 1;
                            if (bytesLeft == 0)
                            {
                                return "";
                            }
                        }
                        text1 = Encoding.UTF8.GetString(list1.ToArray());
                        goto Label_027D;
                    }
                    return "";
                }
                byte num1 = Utils.ReadByte(stream);
                bytesLeft -= 1;
                if (bytesLeft != 0)
                {
                    while (num1 != 0)
                    {
                        list1.Add(num1);
                        num1 = Utils.ReadByte(stream);
                        bytesLeft -= 1;
                        if (bytesLeft == 0)
                        {
                            if (num1 != 0)
                            {
                                list1.Add(num1);
                            }
                            return Utils.ISO88591GetString(list1.ToArray());
                        }
                    }
                    text1 = Utils.ISO88591GetString(list1.ToArray());
                    goto Label_027D;
                }
            }
            return "";
        Label_027D:
            return text1.TrimEnd(new char[1]);
        }

        public static string ReadString(EncodingType textEncoding, Stream stream, int length)
        {
            string text1;
            byte[] buffer1 = Utils.Read(stream, length);
            if (textEncoding == EncodingType.ISO88591)
            {
                text1 = Utils.ISO88591GetString(buffer1);
            }
            else if (textEncoding == EncodingType.Unicode)
            {
                if (length > 2)
                {
                    if (buffer1.Length >= 2)
                    {
                        if ((buffer1[0] == 0xff) && (buffer1[1] == 0xfe))
                        {
                            text1 = Encoding.Unicode.GetString(buffer1, 2, buffer1.Length - 2);
                        }
                        else if ((buffer1[0] == 0xfe) && (buffer1[1] == 0xff))
                        {
                            text1 = Encoding.BigEndianUnicode.GetString(buffer1, 2, buffer1.Length - 2);
                        }
                        else
                        {
                            text1 = Encoding.Unicode.GetString(buffer1, 0, buffer1.Length);
                        }
                    }
                    else
                    {
                        text1 = Encoding.Unicode.GetString(buffer1, 0, buffer1.Length);
                    }
                }
                else
                {
                    text1 = "";
                }
            }
            else if (textEncoding == EncodingType.UTF16BE)
            {
                if (buffer1.Length >= 2)
                {
                    if ((buffer1[0] == 0xfe) && (buffer1[1] == 0xff))
                    {
                        text1 = Encoding.BigEndianUnicode.GetString(buffer1, 2, buffer1.Length - 2);
                    }
                    else
                    {
                        text1 = Encoding.BigEndianUnicode.GetString(buffer1, 0, buffer1.Length);
                    }
                }
                else
                {
                    text1 = Encoding.BigEndianUnicode.GetString(buffer1, 0, buffer1.Length);
                }
            }
            else if (textEncoding == EncodingType.UTF8)
            {
                text1 = Encoding.UTF8.GetString(buffer1, 0, length);
            }
            else
            {
                string text2 = string.Format("Text Encoding '{0}' unknown at position {1}", textEncoding, stream.Position);
                Trace.WriteLine(text2);
                return "";
            }
            return text1.TrimEnd(new char[1]);
        }

        public static byte[] ReadUnsynchronized(byte[] stream)
        {
            using (MemoryStream stream1 = new MemoryStream(stream.Length))
            {
                int num1 = 0;
                int num2 = 0;
                while (num1 < stream.Length)
                {
                    byte num3 = stream[num2++];
                    stream1.WriteByte(num3);
                    if (num3 == 0xff)
                    {
                        num3 = stream[num2++];
                        if (num3 != 0)
                        {
                            stream1.WriteByte(num3);
                            num1++;
                        }
                    }
                    num1++;
                }
                return stream1.ToArray();
            }
        }

        public static byte[] ReadUnsynchronized(Stream stream, int size)
        {
            using (MemoryStream stream1 = new MemoryStream(size))
            {
                for (int num1 = 0; num1 < size; num1++)
                {
                    byte num2 = Utils.ReadByte(stream);
                    stream1.WriteByte(num2);
                    if (num2 == 0xff)
                    {
                        num2 = Utils.ReadByte(stream);
                        if (num2 != 0)
                        {
                            stream1.WriteByte(num2);
                            num1++;
                        }
                    }
                }
                return stream1.ToArray();
            }
        }

        public static Stream ReadUnsynchronizedStream(Stream stream, int length)
        {
            Stream stream1 = new MemoryStream(Utils.ReadUnsynchronized(stream, length), 0, length);
            stream1.Position = 0;
            return stream1;
        }

        public static void ReplaceBytes(string path, int bytesToRemove, byte[] bytesToAdd)
        {
            byte[] buffer1 = new byte[8];
            Random random1 = new Random();
            for (int num1 = 0; num1 < buffer1.Length; num1++)
            {
                buffer1[num1] = (byte) random1.Next(0x41, 0x5b);
            }
            string text1 = Encoding.ASCII.GetString(buffer1);
            string text2 = string.Format("{0}.{1}.tmp", path, text1);
            File.Move(path, text2);
            byte[] buffer2 = new byte[0x7fff];
            using (FileStream stream1 = File.Open(text2, FileMode.Open, FileAccess.Read, FileShare.None))
            {
                using (FileStream stream2 = File.Open(path, FileMode.CreateNew, FileAccess.Write, FileShare.None))
                {
                    stream2.Write(bytesToAdd, 0, bytesToAdd.Length);
                    stream1.Position = bytesToRemove;
                    for (int num2 = stream1.Read(buffer2, 0, 0x7fff); num2 > 0; num2 = stream1.Read(buffer2, 0, 0x7fff))
                    {
                        stream2.Write(buffer2, 0, num2);
                    }
                }
            }
            File.Delete(text2);
        }

        public static void Write(MemoryStream targetStream, byte[] byteArray)
        {
            targetStream.Write(byteArray, 0, byteArray.Length);
        }


        private static Encoding m_ISO88591;
    }
}

