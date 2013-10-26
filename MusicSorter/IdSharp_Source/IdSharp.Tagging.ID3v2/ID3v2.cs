namespace IdSharp.Tagging.ID3v2
{
    using IdSharp.Tagging.ID3v2.Frames;
    using IdSharp.Utils;
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.IO;

    internal sealed class ID3v2 : FrameContainer, IID3v2, IFrameContainer, INotifyPropertyChanged, INotifyInvalidData
    {
        public ID3v2()
        {
            this.m_ID3v2Header = new ID3v2Header();
            this.m_ID3v2ExtendedHeader = new ID3v2ExtendedHeader();
        }

        public byte[] GetBytes(int minimumSize)
        {
            ID3v2TagVersion version1 = this.m_ID3v2Header.TagVersion;
            using (MemoryStream stream1 = new MemoryStream())
            {
                byte[] buffer1 = base.GetBytes(version1);
                int num1 = buffer1.Length;
                this.m_ID3v2Header.UsesUnsynchronization = false;
                this.m_ID3v2Header.IsExperimental = true;
                if (this.m_ID3v2Header.HasExtendedHeader)
                {
                    num1 += this.m_ID3v2ExtendedHeader.SizeExcludingSizeBytes + 4;
                }
                int num2 = minimumSize - (num1 + 10);
                if (num2 < 0)
                {
                    num2 = 0x7d0;
                }
                num1 += num2;
                this.m_ID3v2Header.TagSize = num1;
                byte[] buffer2 = this.m_ID3v2Header.GetBytes();
                stream1.Write(buffer2, 0, buffer2.Length);
                if (this.m_ID3v2Header.HasExtendedHeader)
                {
                    if (this.m_ID3v2ExtendedHeader.IsCRCDataPresent)
                    {
                        this.m_ID3v2ExtendedHeader.CRC32 = CRC32.CalculateInt32(buffer1);
                    }
                    this.m_ID3v2ExtendedHeader.PaddingSize = num2;
                    byte[] buffer3 = this.m_ID3v2ExtendedHeader.GetBytes(version1);
                    stream1.Write(buffer3, 0, buffer3.Length);
                }
                stream1.Write(buffer1, 0, buffer1.Length);
                byte[] buffer4 = new byte[num2];
                stream1.Write(buffer4, 0, num2);
                stream1.Position = 0;
                new IdSharp.Tagging.ID3v2.ID3v2().ReadStream(stream1);
                return stream1.ToArray();
            }
        }

        public void Read(string path)
        {
            try
            {
                using (Stream stream1 = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    if (stream1.Length >= 10)
                    {
                        this.ReadStream(stream1);
                    }
                }
            }
            catch (Exception exception1)
            {
                throw new Exception(string.Format("Error reading '{0}'", path), exception1);
            }
        }

        public void ReadStream(Stream stream)
        {
            if (Utils.ReadString(EncodingType.ISO88591, stream, 3) == "ID3")
            {
                int num2;
                int num3;
                this.m_ID3v2Header = new ID3v2Header(stream, false);
                TagReadingInfo info1 = new TagReadingInfo(this.m_ID3v2Header.TagVersion);
                if (this.m_ID3v2Header.UsesUnsynchronization)
                {
                    info1.TagVersionOptions = TagVersionOptions.Unsynchronized;
                }
                else
                {
                    info1.TagVersionOptions = TagVersionOptions.None;
                }
                if (this.m_ID3v2Header.HasExtendedHeader)
                {
                    this.m_ID3v2ExtendedHeader = new ID3v2ExtendedHeader(info1, stream);
                }
                int num1 = (info1.TagVersion == ID3v2TagVersion.ID3v22) ? 3 : 4;
                if (this.m_ID3v2Header.TagVersion != ID3v2TagVersion.ID3v24)
                {
                    if (this.m_ID3v2Header.TagVersion == ID3v2TagVersion.ID3v22)
                    {
                        bool flag4 = true;
                        num2 = 0;
                        num3 = (this.m_ID3v2Header.TagSize - this.m_ID3v2ExtendedHeader.SizeIncludingSizeBytes) - num1;
                        long num8 = stream.Position;
                        Utils.Read(stream, num1);
                        UnknownFrame frame1 = new UnknownFrame(null, info1, stream);
                        num2 += frame1.FrameHeader.FrameSizeTotal;
                        if (num2 < num3)
                        {
                            byte[] buffer2 = Utils.Read(stream, num1);
                            if (((buffer2[0] < 0x30) || (buffer2[0] > 90)) && (((buffer2[1] >= 0x30) && (buffer2[1] <= 90)) && ((buffer2[2] >= 0x30) && (buffer2[2] <= 90))))
                            {
                                Trace.WriteLine("ID3v2.2 frame size off by 1 byte");
                                flag4 = false;
                            }
                        }
                        stream.Position = num8;
                        if (!flag4)
                        {
                            info1.TagVersionOptions |= TagVersionOptions.AddOneByteToSize;
                        }
                    }
                }
                else
                {
                    bool flag1 = true;
                    num2 = 0;
                    num3 = (this.m_ID3v2Header.TagSize - this.m_ID3v2ExtendedHeader.SizeIncludingSizeBytes) - num1;
                    long num4 = stream.Position;
                    while (num2 < num3)
                    {
                        byte[] buffer1 = Utils.Read(stream, num1);
                        if ((((buffer1[0] < 0x30) || (buffer1[0] > 90)) || ((buffer1[1] < 0x30) || (buffer1[1] > 90))) || (((buffer1[2] < 0x30) || (buffer1[2] > 90)) || ((buffer1[3] < 0x30) || (buffer1[3] > 90))))
                        {
                            if ((buffer1[0] != 0) && (buffer1[0] != 0xff))
                            {
                            }
                            break;
                        }
                        int num5 = Utils.ReadInt32(stream);
                        if (num5 > 0xff)
                        {
                            if ((num5 & 0x80) == 0x80)
                            {
                                flag1 = false;
                                break;
                            }
                            if ((num5 & 0x8000) == 0x8000)
                            {
                                flag1 = false;
                                break;
                            }
                            if ((num5 & 0x800000) == 0x800000)
                            {
                                flag1 = false;
                                break;
                            }
                            if (((num2 + num5) + 10) == this.m_ID3v2Header.TagSize)
                            {
                                flag1 = false;
                                break;
                            }
                            stream.Seek((long) (-4), SeekOrigin.Current);
                            int num6 = Utils.ReadInt32SyncSafe(stream);
                            long num7 = stream.Position;
                            bool flag2 = true;
                            bool flag3 = true;
                            if (((num7 + num5) + 2) >= num3)
                            {
                                flag1 = true;
                                break;
                            }
                            stream.Seek((num7 + num5) + 2, SeekOrigin.Begin);
                            buffer1 = Utils.Read(stream, num1);
                            if ((((buffer1[0] < 0x30) || (buffer1[0] > 90)) || ((buffer1[1] < 0x30) || (buffer1[1] > 90))) || (((buffer1[2] < 0x30) || (buffer1[2] > 90)) || ((buffer1[3] < 0x30) || (buffer1[3] > 90))))
                            {
                                flag3 = false;
                            }
                            stream.Seek((num7 + num6) + 2, SeekOrigin.Begin);
                            buffer1 = Utils.Read(stream, num1);
                            if ((((buffer1[0] < 0x30) || (buffer1[0] > 90)) || ((buffer1[1] < 0x30) || (buffer1[1] > 90))) || (((buffer1[2] < 0x30) || (buffer1[2] > 90)) || ((buffer1[3] < 0x30) || (buffer1[3] > 90))))
                            {
                                flag2 = false;
                            }
                            if (flag3 != flag2)
                            {
                                flag1 = flag2;
                            }
                            break;
                        }
                        stream.Seek((long) (num5 + 2), SeekOrigin.Current);
                        num2 += num5 + 10;
                    }
                    stream.Position = num4;
                    if (!flag1)
                    {
                        info1.TagVersionOptions |= TagVersionOptions.UseNonSyncSafeFrameSizeID3v24;
                    }
                }
                num3 = (this.m_ID3v2Header.TagSize - this.m_ID3v2ExtendedHeader.SizeIncludingSizeBytes) - num1;
                base.Read(stream, this.m_ID3v2Header.TagVersion, info1, num3, num1);
            }
        }

        public void Save(string path)
        {
            int num1 = ID3v2Helper.GetTagSize(path);
            byte[] buffer1 = this.GetBytes(num1);
            if (buffer1.Length < num1)
            {
                throw new ApplicationException("GetBytes() returned a size less than the minimum size");
            }
            if (buffer1.Length > num1)
            {
                Utils.ReplaceBytes(path, num1, buffer1);
            }
            else
            {
                using (FileStream stream1 = File.Open(path, FileMode.Open, FileAccess.Write, FileShare.None))
                {
                    stream1.Write(buffer1, 0, buffer1.Length);
                }
            }
        }

        public void SaveEncoding(string path, EncodingType encodingType)
        {
            throw new Exception("The method or operation is not implemented.");
        }


        public IID3v2ExtendedHeader ExtendedHeader
        {
            get
            {
                return this.m_ID3v2ExtendedHeader;
            }
        }

        public IID3v2Header Header
        {
            get
            {
                return this.m_ID3v2Header;
            }
        }


        private ID3v2ExtendedHeader m_ID3v2ExtendedHeader;
        private ID3v2Header m_ID3v2Header;
    }
}

