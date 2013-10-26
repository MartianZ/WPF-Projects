namespace IdSharp.Inspection
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=1)]
    internal struct VBRData
    {
        public bool Found;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst=4, ArraySubType=UnmanagedType.AsAny)]
        public byte[] ID;
        public int Frames;
        public int Bytes;
        public byte Scale;
        public string VendorID;
    }
}

