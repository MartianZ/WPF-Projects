namespace IdSharp.Inspection
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=1)]
    internal struct LameTag
    {
        public byte Quality;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst=4, ArraySubType=UnmanagedType.AsAny)]
        public byte[] Encoder;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst=5, ArraySubType=UnmanagedType.AsAny)]
        public byte[] VersionString;
        public byte TagRevision_EncodingMethod;
        public byte Lowpass;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst=8, ArraySubType=UnmanagedType.AsAny)]
        public byte[] ReplayGain;
        public byte EncodingFlags_ATHType;
        public byte Bitrate;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst=3, ArraySubType=UnmanagedType.AsAny)]
        public byte[] EncoderDelays;
        public byte MiscInfo;
        public byte MP3Gain;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst=2, ArraySubType=UnmanagedType.AsAny)]
        public byte[] Surround_Preset;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst=4, ArraySubType=UnmanagedType.AsAny)]
        public byte[] MusicLength;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst=2, ArraySubType=UnmanagedType.AsAny)]
        public byte[] MusicCRC;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst=2, ArraySubType=UnmanagedType.AsAny)]
        public byte[] InfoTagCRC;
        public byte NoiseShaping;
        public byte StereoMode;
        public static LameTag FromBinaryReader(BinaryReader br)
        {
            LameTag tag1 = new LameTag();
            tag1.Quality = br.ReadByte();
            tag1.Encoder = br.ReadBytes(4);
            tag1.VersionString = br.ReadBytes(5);
            tag1.TagRevision_EncodingMethod = (byte) (br.ReadByte() & 15);
            tag1.Lowpass = br.ReadByte();
            tag1.ReplayGain = br.ReadBytes(8);
            tag1.EncodingFlags_ATHType = (byte) (br.ReadByte() & 15);
            tag1.Bitrate = br.ReadByte();
            tag1.EncoderDelays = br.ReadBytes(3);
            tag1.MiscInfo = br.ReadByte();
            tag1.MP3Gain = br.ReadByte();
            tag1.Surround_Preset = br.ReadBytes(2);
            tag1.MusicLength = br.ReadBytes(4);
            tag1.MusicCRC = br.ReadBytes(2);
            tag1.InfoTagCRC = br.ReadBytes(2);
            tag1.NoiseShaping = (byte) (tag1.MiscInfo & 3);
            tag1.StereoMode = (byte) ((tag1.MiscInfo & 0x1c) >> 2);
            return tag1;
        }

    }
}

