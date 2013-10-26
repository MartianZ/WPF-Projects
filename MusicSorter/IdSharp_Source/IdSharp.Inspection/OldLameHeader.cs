namespace IdSharp.Inspection
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=1)]
    internal struct OldLameHeader
    {
        public byte UnusedByte;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst=4, ArraySubType=UnmanagedType.AsAny)]
        public byte[] Encoder;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst=0x10, ArraySubType=UnmanagedType.AsAny)]
        public byte[] VersionString;
        public static OldLameHeader FromBinaryReader(BinaryReader br)
        {
            OldLameHeader header1 = new OldLameHeader();
            header1.UnusedByte = br.ReadByte();
            header1.Encoder = br.ReadBytes(4);
            header1.VersionString = br.ReadBytes(0x10);
            return header1;
        }

    }
}

