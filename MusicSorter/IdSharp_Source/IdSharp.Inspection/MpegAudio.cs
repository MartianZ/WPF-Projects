namespace IdSharp.Inspection
{
    using IdSharp.Tagging.ID3v1;
    using IdSharp.Tagging.ID3v2;
    using System;
    using System.IO;
    using System.Text;

    internal sealed class MpegAudio
    {
        static MpegAudio()
        {
            MpegAudio.MPEG_VERSION = new string[] { "MPEG 2.5", "MPEG ?", "MPEG 2", "MPEG 1" };
            MpegAudio.MPEG_LAYER = new string[] { "Layer ?", "Layer III", "Layer II", "Layer I" };
            ushort[][] numArrayArray1 = new ushort[4][];
            numArrayArray1[0] = new ushort[] { 0x2b11, 0x2ee0, 0x1f40, 0 };
            numArrayArray1[1] = new ushort[4];
            numArrayArray1[2] = new ushort[] { 0x5622, 0x5dc0, 0x3e80, 0 };
            numArrayArray1[3] = new ushort[] { 0xac44, 0xbb80, 0x7d00, 0 };
            MpegAudio.MPEG_SAMPLE_RATE = numArrayArray1;
            ushort[][][] numArrayArrayArray1 = new ushort[4][][];
            numArrayArrayArray1[0] = new ushort[][] { new ushort[] { 8, 0x10, 0x18, 0x20, 40, 0x30, 0x38, 0x40, 80, 0x60, 0x70, 0x80, 0x90, 160 }, new ushort[] { 8, 0x10, 0x18, 0x20, 40, 0x30, 0x38, 0x40, 80, 0x60, 0x70, 0x80, 0x90, 160 }, new ushort[] { 0x20, 0x30, 0x38, 0x40, 80, 0x60, 0x70, 0x80, 0x90, 160, 0xb0, 0xc0, 0xe0, 0x100 } };
            ushort[][] numArrayArray3 = new ushort[3][];
            numArrayArray3[0] = new ushort[14];
            numArrayArray3[1] = new ushort[14];
            numArrayArray3[2] = new ushort[14];
            numArrayArrayArray1[1] = numArrayArray3;
            numArrayArrayArray1[2] = new ushort[][] { new ushort[] { 8, 0x10, 0x18, 0x20, 40, 0x30, 0x38, 0x40, 80, 0x60, 0x70, 0x80, 0x90, 160 }, new ushort[] { 8, 0x10, 0x18, 0x20, 40, 0x30, 0x38, 0x40, 80, 0x60, 0x70, 0x80, 0x90, 160 }, new ushort[] { 0x20, 0x30, 0x38, 0x40, 80, 0x60, 0x70, 0x80, 0x90, 160, 0xb0, 0xc0, 0xe0, 0x100 } };
            numArrayArrayArray1[3] = new ushort[][] { new ushort[] { 0x20, 40, 0x30, 0x38, 0x40, 80, 0x60, 0x70, 0x80, 160, 0xc0, 0xe0, 0x100, 320 }, new ushort[] { 0x20, 0x30, 0x38, 0x40, 80, 0x60, 0x70, 0x80, 160, 0xc0, 0xe0, 0x100, 320, 0x180 }, new ushort[] { 0x20, 0x40, 0x60, 0x80, 160, 0xc0, 0xe0, 0x100, 0x120, 320, 0x160, 0x180, 0x1a0, 0x1c0 } };
            MpegAudio.BitrateTable = numArrayArrayArray1;
        }

        public MpegAudio(string path)
        {
            this.ResetData();
            using (BinaryReader reader1 = new BinaryReader(File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read)))
            {
                int num1 = ID3v2Helper.GetTagSize(reader1.BaseStream);
                this.m_FileLength = reader1.BaseStream.Length;
                reader1.BaseStream.Seek((long) num1, SeekOrigin.Begin);
                byte[] buffer1 = reader1.ReadBytes(0xd82);
                this.FindFrame(buffer1, ref this.m_VBR);
                this.m_VendorID = this.FindVendorID(buffer1);
                if (!this.m_Frame.Found)
                {
                    reader1.BaseStream.Seek((this.m_FileLength - num1) / ((long) 2), SeekOrigin.Begin);
                    buffer1 = reader1.ReadBytes(0xd82);
                    this.FindFrame(buffer1, ref this.m_VBR);
                }
                if (this.m_Frame.Found && string.IsNullOrEmpty(this.m_VendorID))
                {
                    reader1.BaseStream.Seek((long) -(buffer1.Length + ID3v1Helper.GetTagSize(reader1.BaseStream)), SeekOrigin.End);
                    buffer1 = reader1.ReadBytes(0xd82);
                    this.FindFrame(buffer1, ref this.m_VBR);
                    this.m_VendorID = this.FindVendorID(buffer1);
                }
            }
            if (!this.m_Frame.Found)
            {
                this.ResetData();
            }
        }

        private void DecodeHeader(byte[] headerData)
        {
            this.m_Frame.Data = new byte[headerData.Length];
            Buffer.BlockCopy(headerData, 0, this.m_Frame.Data, 0, headerData.Length);
            this.m_Frame.VersionID = (MpegVersion) ((byte) ((headerData[1] >> 3) & 3));
            this.m_Frame.LayerID = (MpegLayer) ((byte) ((headerData[1] >> 1) & 3));
            this.m_Frame.ProtectionBit = (headerData[1] & 1) != 1;
            this.m_Frame.BitRateID = (byte) (headerData[2] >> 4);
            this.m_Frame.SampleRateID = (SampleRateLevel) ((ushort) ((headerData[2] >> 2) & 3));
            this.m_Frame.PaddingBit = ((headerData[2] >> 1) & 1) == 1;
            this.m_Frame.PrivateBit = (headerData[2] & 1) == 1;
            this.m_Frame.ModeID = (MpegChannel) ((byte) ((headerData[3] >> 6) & 3));
            this.m_Frame.ModeExtensionID = (JointStereoExtensionMode) ((byte) ((headerData[3] >> 4) & 3));
            this.m_Frame.CopyrightBit = ((headerData[3] >> 3) & 1) == 1;
            this.m_Frame.OriginalBit = ((headerData[3] >> 2) & 1) == 1;
            this.m_Frame.EmphasisID = (Emphasis) ((byte) (headerData[3] & 3));
        }

        private void FindFrame(byte[] data, ref VBRData vbrHeader)
        {
            byte[] buffer1 = new byte[4];
            Buffer.BlockCopy(data, 0, buffer1, 0, 4);
            int num1 = data.Length - 4;
            for (int num2 = 0; num2 < num1; num2++)
            {
                if (this.IsFrameHeader(buffer1))
                {
                    this.DecodeHeader(buffer1);
                    int num3 = num2 + this.GetFrameLength(this.m_Frame);
                    if ((num3 < num1) && this.ValidFrameAt(num3, data))
                    {
                        this.m_Frame.Found = true;
                        this.m_Frame.Position = num2;
                        this.m_Frame.Size = this.GetFrameLength(this.m_Frame);
                        this.m_Frame.Xing = this.IsXing(num2 + buffer1.Length, data);
                        vbrHeader = this.FindVBR(num2 + this.GetVBRFrameOffset(this.m_Frame), data);
                        return;
                    }
                }
                buffer1[0] = buffer1[1];
                buffer1[1] = buffer1[2];
                buffer1[2] = buffer1[3];
                buffer1[3] = data[4 + num2];
            }
        }

        private VBRData FindVBR(int index, byte[] data)
        {
            string text1 = string.Format("{0}{1}{2}{3}", new object[] { (char) data[index], (char) data[index + 1], (char) data[index + 2], (char) data[index + 3] });
            if (text1 == VBRHeaderID.Xing)
            {
                return this.GetXingInfo(index, data);
            }
            if (text1 == VBRHeaderID.FhG)
            {
                return this.GetFhGInfo(index, data);
            }
            return new VBRData();
        }

        private string FindVendorID(byte[] data)
        {
            string text1 = "";
            int num1 = data.Length;
            for (int num2 = 0; num2 <= (num1 - 8); num2++)
            {
                string text2 = string.Format("{0}{1}{2}{3}", new object[] { (char) data[(num1 - num2) - 8], (char) data[(num1 - num2) - 7], (char) data[(num1 - num2) - 6], (char) data[(num1 - num2) - 5] });
                if (text2 == VBRVendorID.LAME)
                {
                    return (text2 + string.Format("{0}{1}{2}{3}", new object[] { (char) data[(num1 - num2) - 4], (char) data[(num1 - num2) - 3], (char) data[(num1 - num2) - 2], (char) data[(num1 - num2) - 1] }));
                }
                if (text2 == VBRVendorID.GoGoNew)
                {
                    return text2;
                }
            }
            return text1;
        }

        private ushort GetBitRate(FrameData Frame)
        {
            return MpegAudio.BitrateTable[(int) Frame.VersionID][((int) Frame.LayerID) - 1][Frame.BitRateID - 1];
        }

        private MpegEncoder GetCBREncoderID()
        {
            string text1;
            MpegEncoder encoder1 = MpegEncoder.FhG;
            if (!string.IsNullOrEmpty(this.m_VendorID) && (this.m_VendorID.Length >= 4))
            {
                text1 = this.m_VendorID.Substring(0, 4);
            }
            else
            {
                text1 = "";
            }
            if (this.m_Frame.OriginalBit && this.m_Frame.ProtectionBit)
            {
                encoder1 = MpegEncoder.LAME;
            }
            if ((this.GetBitRate(this.m_Frame) <= 160) && (this.m_Frame.ModeID == MpegChannel.Stereo))
            {
                encoder1 = MpegEncoder.Blade;
            }
            if ((this.m_Frame.CopyrightBit && this.m_Frame.OriginalBit) && !this.m_Frame.ProtectionBit)
            {
                encoder1 = MpegEncoder.Xing;
            }
            if (this.m_Frame.Xing && this.m_Frame.OriginalBit)
            {
                encoder1 = MpegEncoder.Xing;
            }
            if (this.m_Frame.LayerID == MpegLayer.LayerII)
            {
                encoder1 = MpegEncoder.QDesign;
            }
            if ((this.m_Frame.ModeID == MpegChannel.DualChannel) && this.m_Frame.ProtectionBit)
            {
                encoder1 = MpegEncoder.Shine;
            }
            if (text1 == VBRVendorID.LAME)
            {
                encoder1 = MpegEncoder.LAME;
            }
            if (text1 == VBRVendorID.GoGoNew)
            {
                encoder1 = MpegEncoder.GoGo;
            }
            return encoder1;
        }

        private byte GetCoefficient(FrameData Frame)
        {
            if (Frame.VersionID == MpegVersion.Version1)
            {
                if (Frame.LayerID == MpegLayer.LayerI)
                {
                    return 0x30;
                }
                return 0x90;
            }
            if (Frame.LayerID == MpegLayer.LayerI)
            {
                return 0x18;
            }
            if (Frame.LayerID == MpegLayer.LayerII)
            {
                return 0x90;
            }
            return 0x48;
        }

        private MpegEncoder GetEncoderID()
        {
            MpegEncoder encoder1 = MpegEncoder.Unknown;
            if (!this.m_Frame.Found)
            {
                return encoder1;
            }
            if (this.m_VBR.Found)
            {
                return this.GetVBREncoderID();
            }
            return this.GetCBREncoderID();
        }

        private VBRData GetFhGInfo(int index, byte[] data)
        {
            VBRData data1 = new VBRData();
            data1.Found = true;
            data1.ID = Encoding.ASCII.GetBytes(VBRHeaderID.FhG);
            data1.Scale = data[index + 9];
            data1.Bytes = (((data[index + 10] * 0x1000000) + (data[index + 11] * 0x10000)) + (data[index + 12] * 0x100)) + data[index + 13];
            data1.Frames = (((data[index + 14] * 0x1000000) + (data[index + 15] * 0x10000)) + (data[index + 0x10] * 0x100)) + data[index + 0x11];
            return data1;
        }

        private ushort GetFrameLength(FrameData frame)
        {
            ushort num2 = this.GetCoefficient(frame);
            ushort num3 = this.GetBitRate(frame);
            ushort num4 = this.GetSampleRate(frame);
            ushort num5 = this.GetPadding(frame);
            return (ushort) ((((num2 * num3) * 0x3e8) / num4) + num5);
        }

        private byte GetPadding(FrameData Frame)
        {
            if (Frame.PaddingBit)
            {
                if (Frame.LayerID == MpegLayer.LayerI)
                {
                    return 4;
                }
                return 1;
            }
            return 0;
        }

        private ushort GetSampleRate(FrameData Frame)
        {
            return MpegAudio.MPEG_SAMPLE_RATE[(int) Frame.VersionID][(int) Frame.SampleRateID];
        }

        private MpegEncoder GetVBREncoderID()
        {
            MpegEncoder encoder1 = MpegEncoder.Unknown;
            string text1 = this.m_VBR.VendorID.Substring(0, 4);
            if (text1 == VBRVendorID.LAME)
            {
                encoder1 = MpegEncoder.LAME;
            }
            if (text1 == VBRVendorID.GoGoNew)
            {
                encoder1 = MpegEncoder.GoGo;
            }
            if (text1 == VBRVendorID.GoGoOld)
            {
                encoder1 = MpegEncoder.GoGo;
            }
            if (((Encoding.ASCII.GetString(this.m_VBR.ID) == VBRHeaderID.Xing) && (text1 != VBRVendorID.LAME)) && ((text1 != VBRVendorID.GoGoNew) && (text1 != VBRVendorID.GoGoOld)))
            {
                encoder1 = MpegEncoder.Xing;
            }
            if (Encoding.ASCII.GetString(this.m_VBR.ID) == VBRHeaderID.FhG)
            {
                encoder1 = MpegEncoder.FhG;
            }
            if (text1 == VBRVendorID.LAME)
            {
                encoder1 = MpegEncoder.LAME;
            }
            return encoder1;
        }

        private byte GetVBRFrameOffset(FrameData Frame)
        {
            if (Frame.VersionID == MpegVersion.Version1)
            {
                if (Frame.ModeID != MpegChannel.Mono)
                {
                    return 0x24;
                }
                return 0x15;
            }
            if (Frame.ModeID != MpegChannel.Mono)
            {
                return 0x15;
            }
            return 13;
        }

        private VBRData GetXingInfo(int index, byte[] data)
        {
            VBRData data1;
            data1.Found = true;
            data1.ID = Encoding.ASCII.GetBytes(VBRHeaderID.Xing);
            data1.Frames = (((data[index + 8] * 0x1000000) + (data[index + 9] * 0x10000)) + (data[index + 10] * 0x100)) + data[index + 11];
            data1.Bytes = (((data[index + 12] * 0x1000000) + (data[index + 13] * 0x10000)) + (data[index + 14] * 0x100)) + data[index + 15];
            data1.Scale = data[index + 0x77];
            data1.VendorID = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}", new object[] { (char) data[index + 120], (char) data[index + 0x79], (char) data[index + 0x7a], (char) data[index + 0x7b], (char) data[index + 0x7c], (char) data[index + 0x7d], (char) data[index + 0x7e], (char) data[index + 0x7f] });
            return data1;
        }

        private bool IsFrameHeader(byte[] headerData)
        {
            if (((((headerData[0] & 0xff) == 0xff) && ((headerData[1] & 0xe0) == 0xe0)) && ((((headerData[1] >> 3) & 3) != 1) && (((headerData[1] >> 1) & 3) != 0))) && ((((headerData[2] & 240) != 240) && ((headerData[2] & 240) != 0)) && ((((headerData[2] >> 2) & 3) != 3) && ((headerData[3] & 3) != 2))))
            {
                return true;
            }
            return false;
        }

        private bool IsXing(int index, byte[] data)
        {
            return (((((data[index] == 0) && (data[index + 1] == 0)) && ((data[index + 2] == 0) && (data[index + 3] == 0))) && (data[index + 4] == 0)) && (data[index + 5] == 0));
        }

        private void ResetData()
        {
            this.m_FileLength = 0;
            this.m_VendorID = "";
            this.m_Frame.VersionID = MpegVersion.Unknown;
            this.m_Frame.SampleRateID = SampleRateLevel.Unknown;
            this.m_Frame.ModeID = MpegChannel.Unknown;
            this.m_Frame.ModeExtensionID = JointStereoExtensionMode.Unknown;
            this.m_Frame.EmphasisID = Emphasis.Unknown;
        }

        private bool ValidFrameAt(int index, byte[] data)
        {
            byte[] buffer1 = new byte[] { data[index], data[index + 1], data[index + 2], data[index + 3] };
            return this.IsFrameHeader(buffer1);
        }


        public string Encoder
        {
            get
            {
                string text1 = "";
                string text2 = this.GetEncoderID().ToString();
                if (!string.IsNullOrEmpty(this.m_VBR.VendorID))
                {
                    text1 = this.m_VBR.VendorID;
                }
                if (!string.IsNullOrEmpty(this.m_VendorID))
                {
                    text1 = this.m_VendorID;
                }
                if ((((this.GetEncoderID() == MpegEncoder.LAME) && (text1.Length >= 8)) && (char.IsDigit(text1, 4) && (text1[5] == '.'))) && (char.IsDigit(text1, 6) && char.IsDigit(text1, 7)))
                {
                    text2 = text2 + " " + text1.Substring(4, 4);
                }
                return text2;
            }
        }

        public string Layer
        {
            get
            {
                return MpegAudio.MPEG_LAYER[(int) this.m_Frame.LayerID];
            }
        }

        public string Version
        {
            get
            {
                return MpegAudio.MPEG_VERSION[(int) this.m_Frame.VersionID];
            }
        }


        private static readonly ushort[][][] BitrateTable;
        private long m_FileLength;
        private FrameData m_Frame;
        private VBRData m_VBR;
        private string m_VendorID;
        private const int MaxMpegFrameLength = 0x6c1;
        private static readonly string[] MPEG_LAYER;
        private static readonly ushort[][] MPEG_SAMPLE_RATE;
        private static readonly string[] MPEG_VERSION;
    }
}

