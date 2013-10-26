namespace zlib
{
    using System;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text;

    internal static class SupportClass
    {
        public static object Deserialize(BinaryReader binaryReader)
        {
            BinaryFormatter formatter1 = new BinaryFormatter();
            return formatter1.Deserialize(binaryReader.BaseStream);
        }

        public static double Identity(double literal)
        {
            return literal;
        }

        public static long Identity(long literal)
        {
            return literal;
        }

        public static float Identity(float literal)
        {
            return literal;
        }

        public static ulong Identity(ulong literal)
        {
            return literal;
        }

        public static int ReadInput(Stream sourceStream, byte[] target, int start, int count)
        {
            if (target.Length == 0)
            {
                return 0;
            }
            byte[] buffer1 = new byte[target.Length];
            int num1 = sourceStream.Read(buffer1, start, count);
            if (num1 == 0)
            {
                return -1;
            }
            for (int num2 = start; num2 < (start + num1); num2++)
            {
                target[num2] = buffer1[num2];
            }
            return num1;
        }

        public static int ReadInput(TextReader sourceTextReader, byte[] target, int start, int count)
        {
            if (target.Length == 0)
            {
                return 0;
            }
            char[] chArray1 = new char[target.Length];
            int num1 = sourceTextReader.Read(chArray1, start, count);
            if (num1 == 0)
            {
                return -1;
            }
            for (int num2 = start; num2 < (start + num1); num2++)
            {
                target[num2] = (byte) chArray1[num2];
            }
            return num1;
        }

        public static void Serialize(BinaryWriter binaryWriter, object objectToSend)
        {
            new BinaryFormatter().Serialize(binaryWriter.BaseStream, objectToSend);
        }

        public static void Serialize(Stream stream, object objectToSend)
        {
            new BinaryFormatter().Serialize(stream, objectToSend);
        }

        public static byte[] ToByteArray(string sourceString)
        {
            return Encoding.UTF8.GetBytes(sourceString);
        }

        public static char[] ToCharArray(byte[] byteArray)
        {
            return Encoding.UTF8.GetChars(byteArray);
        }

        public static int URShift(int number, int bits)
        {
            if (number >= 0)
            {
                return (number >> (bits & 0x1f));
            }
            return ((number >> (bits & 0x1f)) + (2 << (~bits & 0x1f)));
        }

        public static int URShift(int number, long bits)
        {
            return SupportClass.URShift(number, (int) bits);
        }

        public static long URShift(long number, int bits)
        {
            if (number >= 0)
            {
                return (number >> (bits & 0x3f));
            }
            return ((number >> (bits & 0x3f)) + (2 << (~bits & 0x3f)));
        }

        public static long URShift(long number, long bits)
        {
            return SupportClass.URShift(number, (int) bits);
        }

        public static void WriteStackTrace(Exception throwable, TextWriter stream)
        {
            stream.Write(throwable.StackTrace);
            stream.Flush();
        }

    }
}

