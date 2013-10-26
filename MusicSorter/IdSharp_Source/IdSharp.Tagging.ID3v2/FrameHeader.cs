namespace IdSharp.Tagging.ID3v2
{
    using System;
    using System.IO;

    internal sealed class FrameHeader : IFrameHeader
    {
        public byte[] GetBytes(MemoryStream frameData, ID3v2TagVersion tagVersion, string frameID)
        {
            byte[] buffer2;
            this.m_FrameSizeExcludingAdditions = (int) frameData.Length;
            if (frameID == null)
            {
                return new byte[0];
            }
            byte[] buffer1 = Utils.ISO88591GetBytes(frameID);
            if (tagVersion == ID3v2TagVersion.ID3v22)
            {
                if (buffer1.Length != 3)
                {
                    throw new ArgumentException(string.Format("FrameID must be 3 bytes from ID3v2.2 ({0} bytes passed)", buffer1.Length));
                }
                buffer2 = new byte[] { buffer1[0], buffer1[1], buffer1[2], (byte) ((this.m_FrameSizeExcludingAdditions >> 0x10) & 0xff), (byte) ((this.m_FrameSizeExcludingAdditions >> 8) & 0xff), (byte) (this.m_FrameSizeExcludingAdditions & 0xff) };
            }
            else if (tagVersion == ID3v2TagVersion.ID3v23)
            {
                int num1 = 10;
                byte num2 = (byte) (((this.m_IsTagAlterPreservation ? 0x80 : 0) + (this.m_IsFileAlterPreservation ? 0x40 : 0)) + (this.m_IsReadOnly ? 0x20 : 0));
                byte num3 = (byte) (((this.m_IsCompressed ? 0x80 : 0) + (this.m_EncryptionMethod.HasValue ? 0x40 : 0)) + (this.m_GroupingIdentity.HasValue ? 0x20 : 0));
                if (this.m_IsCompressed)
                {
                    num1 += 4;
                }
                if (this.m_EncryptionMethod.HasValue)
                {
                    num1++;
                }
                if (this.m_GroupingIdentity.HasValue)
                {
                    num1++;
                }
                int num4 = this.m_FrameSizeExcludingAdditions + (num1 - 10);
                buffer2 = new byte[num1];
                if (buffer1.Length != 4)
                {
                    throw new ArgumentException(string.Format("FrameID must be 4 bytes ({0} bytes passed)", buffer1.Length));
                }
                buffer2[0] = buffer1[0];
                buffer2[1] = buffer1[1];
                buffer2[2] = buffer1[2];
                buffer2[3] = buffer1[3];
                buffer2[4] = (byte) ((num4 >> 0x18) & 0xff);
                buffer2[5] = (byte) ((num4 >> 0x10) & 0xff);
                buffer2[6] = (byte) ((num4 >> 8) & 0xff);
                buffer2[7] = (byte) (num4 & 0xff);
                buffer2[8] = num2;
                buffer2[9] = num3;
                int num5 = 10;
                if (this.m_IsCompressed)
                {
                    buffer2[num5++] = (byte) (this.DecompressedSize >> 0x18);
                    buffer2[num5++] = (byte) (this.DecompressedSize >> 0x10);
                    buffer2[num5++] = (byte) (this.DecompressedSize >> 8);
                    buffer2[num5++] = (byte) this.DecompressedSize;
                }
                if (this.m_EncryptionMethod.HasValue)
                {
                    buffer2[num5++] = this.m_EncryptionMethod.Value;
                }
                if (this.m_GroupingIdentity.HasValue)
                {
                    buffer2[num5] = this.m_GroupingIdentity.Value;
                }
            }
            else
            {
                if (tagVersion != ID3v2TagVersion.ID3v24)
                {
                    throw new ArgumentOutOfRangeException("tagVersion", tagVersion, "Unknown tag version");
                }
                int num6 = 10;
                byte num7 = (byte) (((this.m_IsTagAlterPreservation ? 0x40 : 0) + (this.m_IsFileAlterPreservation ? 0x20 : 0)) + (this.m_IsReadOnly ? 0x10 : 0));
                byte num8 = (byte) (((this.m_GroupingIdentity.HasValue ? 0x40 : 0) + (this.m_IsCompressed ? 8 : 0)) + (this.m_EncryptionMethod.HasValue ? 4 : 0));
                if (this.m_IsCompressed)
                {
                    num6 += 4;
                }
                if (this.m_EncryptionMethod.HasValue)
                {
                    num6++;
                }
                if (this.m_GroupingIdentity.HasValue)
                {
                    num6++;
                }
                int num9 = this.m_FrameSizeExcludingAdditions + (num6 - 10);
                buffer2 = new byte[num6];
                if (buffer1.Length != 4)
                {
                    throw new ArgumentException(string.Format("FrameID must be 4 bytes ({0} bytes passed)", buffer1.Length));
                }
                buffer2[0] = buffer1[0];
                buffer2[1] = buffer1[1];
                buffer2[2] = buffer1[2];
                buffer2[3] = buffer1[3];
                buffer2[4] = (byte) ((num9 >> 0x15) & 0x7f);
                buffer2[5] = (byte) ((num9 >> 14) & 0x7f);
                buffer2[6] = (byte) ((num9 >> 7) & 0x7f);
                buffer2[7] = (byte) (num9 & 0x7f);
                buffer2[8] = num7;
                buffer2[9] = num8;
                int num10 = 10;
                if (this.m_GroupingIdentity.HasValue)
                {
                    buffer2[num10++] = this.m_GroupingIdentity.Value;
                }
                if (this.m_IsCompressed)
                {
                    buffer2[num10++] = (byte) (this.DecompressedSize >> 0x18);
                    buffer2[num10++] = (byte) (this.DecompressedSize >> 0x10);
                    buffer2[num10++] = (byte) (this.DecompressedSize >> 8);
                    buffer2[num10++] = (byte) this.DecompressedSize;
                }
                if (this.m_EncryptionMethod.HasValue)
                {
                    buffer2[num10++] = this.m_EncryptionMethod.Value;
                }
            }
            using (MemoryStream stream1 = new MemoryStream())
            {
                Utils.Write(stream1, buffer2);
                Utils.Write(stream1, frameData.ToArray());
                return stream1.ToArray();
            }
        }

        public void Read(TagReadingInfo tagReadingInfo, ref Stream stream)
        {
            this.m_TagVersion = tagReadingInfo.TagVersion;
            bool flag1 = (tagReadingInfo.TagVersionOptions & TagVersionOptions.Unsynchronized) == TagVersionOptions.Unsynchronized;
            if (tagReadingInfo.TagVersion == ID3v2TagVersion.ID3v23)
            {
                if (!flag1)
                {
                    this.m_FrameSize = Utils.ReadInt32(stream);
                }
                else
                {
                    this.m_FrameSize = Utils.ReadInt32Unsynchronized(stream);
                }
                this.m_FrameSizeExcludingAdditions = this.m_FrameSize;
                byte num1 = Utils.ReadByte(stream);
                byte num2 = Utils.ReadByte(stream);
                this.IsTagAlterPreservation = (num1 & 0x80) == 0x80;
                this.IsFileAlterPreservation = (num1 & 0x40) == 0x40;
                this.IsReadOnly = (num1 & 0x20) == 0x20;
                this.IsCompressed = (num2 & 0x80) == 0x80;
                bool flag2 = (num2 & 0x40) == 0x40;
                bool flag3 = (num2 & 0x20) == 0x20;
                if (this.IsCompressed)
                {
                    this.DecompressedSize = Utils.ReadInt32(stream);
                    this.m_FrameSizeExcludingAdditions -= 4;
                }
                else
                {
                    this.DecompressedSize = 0;
                }
                if (flag2)
                {
                    this.EncryptionMethod = new Nullable<byte>(Utils.ReadByte(stream));
                    this.m_FrameSizeExcludingAdditions--;
                }
                else
                {
                    this.EncryptionMethod = new Nullable<byte>();
                }
                if (flag3)
                {
                    this.GroupingIdentity = new Nullable<byte>(Utils.ReadByte(stream));
                    this.m_FrameSizeExcludingAdditions--;
                }
                else
                {
                    this.GroupingIdentity = new Nullable<byte>();
                }
                if (flag1)
                {
                    stream = Utils.ReadUnsynchronizedStream(stream, this.m_FrameSize);
                }
            }
            else if (tagReadingInfo.TagVersion == ID3v2TagVersion.ID3v22)
            {
                if (!flag1)
                {
                    this.m_FrameSize = Utils.ReadInt24(stream);
                }
                else
                {
                    this.m_FrameSize = Utils.ReadInt24Unsynchronized(stream);
                }
                if ((tagReadingInfo.TagVersionOptions & TagVersionOptions.AddOneByteToSize) == TagVersionOptions.AddOneByteToSize)
                {
                    this.m_FrameSize++;
                }
                this.m_FrameSizeExcludingAdditions = this.m_FrameSize;
                this.IsTagAlterPreservation = false;
                this.IsFileAlterPreservation = false;
                this.IsReadOnly = false;
                this.IsCompressed = false;
                this.DecompressedSize = 0;
                this.EncryptionMethod = new Nullable<byte>();
                this.GroupingIdentity = new Nullable<byte>();
                if (flag1)
                {
                    stream = Utils.ReadUnsynchronizedStream(stream, this.m_FrameSize);
                }
            }
            else if (tagReadingInfo.TagVersion == ID3v2TagVersion.ID3v24)
            {
                if ((tagReadingInfo.TagVersionOptions & TagVersionOptions.UseNonSyncSafeFrameSizeID3v24) == TagVersionOptions.UseNonSyncSafeFrameSizeID3v24)
                {
                    this.m_FrameSize = Utils.ReadInt32(stream);
                }
                else
                {
                    this.m_FrameSize = Utils.ReadInt32SyncSafe(stream);
                }
                this.m_FrameSizeExcludingAdditions = this.m_FrameSize;
                Utils.ReadByte(stream);
                Utils.ReadByte(stream);
            }
            if (this.IsCompressed)
            {
                stream = Utils.DecompressFrame(stream, this.FrameSizeExcludingAdditions);
                this.IsCompressed = false;
                this.DecompressedSize = 0;
                this.m_FrameSizeExcludingAdditions = (int) stream.Length;
            }
        }


        public int DecompressedSize
        {
            get
            {
                if (this.m_TagVersion != ID3v2TagVersion.ID3v22)
                {
                    return this.m_DecompressedSize;
                }
                return 0;
            }
            set
            {
                if (this.m_TagVersion != ID3v2TagVersion.ID3v22)
                {
                    this.m_DecompressedSize = value;
                }
                else
                {
                    this.m_DecompressedSize = 0;
                }
            }
        }

        public Nullable<byte> EncryptionMethod
        {
            get
            {
                if (this.m_TagVersion != ID3v2TagVersion.ID3v22)
                {
                    return this.m_EncryptionMethod;
                }
                return new Nullable<byte>();
            }
            set
            {
                if (this.m_TagVersion != ID3v2TagVersion.ID3v22)
                {
                    this.m_EncryptionMethod = value;
                }
                else
                {
                    this.m_EncryptionMethod = new Nullable<byte>();
                }
            }
        }

        public int FrameSize
        {
            get
            {
                return this.m_FrameSize;
            }
        }

        public int FrameSizeExcludingAdditions
        {
            get
            {
                return this.m_FrameSizeExcludingAdditions;
            }
        }

        public int FrameSizeTotal
        {
            get
            {
                return (this.m_FrameSize + ((this.m_TagVersion == ID3v2TagVersion.ID3v22) ? 6 : 10));
            }
        }

        public Nullable<byte> GroupingIdentity
        {
            get
            {
                if (this.m_TagVersion != ID3v2TagVersion.ID3v22)
                {
                    return this.m_GroupingIdentity;
                }
                return new Nullable<byte>();
            }
            set
            {
                if (this.m_TagVersion != ID3v2TagVersion.ID3v22)
                {
                    this.m_GroupingIdentity = value;
                }
                else
                {
                    this.m_GroupingIdentity = new Nullable<byte>();
                }
            }
        }

        public bool IsCompressed
        {
            get
            {
                if (this.m_TagVersion != ID3v2TagVersion.ID3v22)
                {
                    return this.m_IsCompressed;
                }
                return false;
            }
            set
            {
                if (this.m_TagVersion != ID3v2TagVersion.ID3v22)
                {
                    this.m_IsCompressed = value;
                }
                else
                {
                    this.m_IsCompressed = false;
                }
            }
        }

        public bool IsFileAlterPreservation
        {
            get
            {
                if (this.m_TagVersion != ID3v2TagVersion.ID3v22)
                {
                    return this.m_IsFileAlterPreservation;
                }
                return false;
            }
            set
            {
                if (this.m_TagVersion != ID3v2TagVersion.ID3v22)
                {
                    this.m_IsFileAlterPreservation = value;
                }
                else
                {
                    this.m_IsFileAlterPreservation = false;
                }
            }
        }

        public bool IsReadOnly
        {
            get
            {
                if (this.m_TagVersion != ID3v2TagVersion.ID3v22)
                {
                    return this.m_IsReadOnly;
                }
                return false;
            }
            set
            {
                if (this.m_TagVersion != ID3v2TagVersion.ID3v22)
                {
                    this.m_IsReadOnly = value;
                }
                else
                {
                    this.m_IsReadOnly = false;
                }
            }
        }

        public bool IsTagAlterPreservation
        {
            get
            {
                if (this.m_TagVersion != ID3v2TagVersion.ID3v22)
                {
                    return this.m_IsTagAlterPreservation;
                }
                return false;
            }
            set
            {
                if (this.m_TagVersion != ID3v2TagVersion.ID3v22)
                {
                    this.m_IsTagAlterPreservation = value;
                }
                else
                {
                    this.m_IsTagAlterPreservation = false;
                }
            }
        }

        public ID3v2TagVersion TagVersion
        {
            get
            {
                return this.m_TagVersion;
            }
        }

        public bool UsesUnsynchronization
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }


        private int m_DecompressedSize;
        private Nullable<byte> m_EncryptionMethod;
        private int m_FrameSize;
        private int m_FrameSizeExcludingAdditions;
        private Nullable<byte> m_GroupingIdentity;
        private bool m_IsCompressed;
        private bool m_IsFileAlterPreservation;
        private bool m_IsReadOnly;
        private bool m_IsTagAlterPreservation;
        private ID3v2TagVersion m_TagVersion;
    }
}

