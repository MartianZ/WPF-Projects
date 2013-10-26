namespace IdSharp.Inspection
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=1)]
    internal struct FrameData
    {
        public bool Found;
        public int Position;
        public ushort Size;
        public bool Xing;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst=4, ArraySubType=UnmanagedType.AsAny)]
        public byte[] Data;
        public MpegVersion VersionID;
        public MpegLayer LayerID;
        public bool ProtectionBit;
        public ushort BitRateID;
        public SampleRateLevel SampleRateID;
        public bool PaddingBit;
        public bool PrivateBit;
        public MpegChannel ModeID;
        public JointStereoExtensionMode ModeExtensionID;
        public bool CopyrightBit;
        public bool OriginalBit;
        public Emphasis EmphasisID;
    }
}

