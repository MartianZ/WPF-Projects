namespace IdSharp.Tagging.ID3v2
{
    using System;
    using System.Diagnostics;
    using System.IO;

    internal sealed class ID3v2ExtendedHeader : IID3v2ExtendedHeader
    {
        public ID3v2ExtendedHeader()
        {
            this.Clear();
        }

        public ID3v2ExtendedHeader(TagReadingInfo tagReadingInfo, Stream stream)
        {
            this.ReadFrom(tagReadingInfo, stream);
        }

        private void Clear()
        {
            this.m_IsCRCDataPresent = false;
            this.m_PaddingSize = 0;
            this.m_TotalFrameCRC = 0;
        }

        public byte[] GetBytes(ID3v2TagVersion tagVersion)
        {
            byte[] buffer1 = new byte[this.m_IsCRCDataPresent ? 14 : 10];
            buffer1[0] = 0;
            buffer1[1] = 0;
            buffer1[2] = 0;
            buffer1[3] = this.m_IsCRCDataPresent ? ((byte) 10) : ((byte) 6);
            buffer1[4] = this.m_IsCRCDataPresent ? ((byte) 0x80) : ((byte) 0);
            buffer1[5] = 0;
            buffer1[6] = (byte) ((this.m_PaddingSize >> 0x18) & 0xff);
            buffer1[7] = (byte) ((this.m_PaddingSize >> 0x10) & 0xff);
            buffer1[8] = (byte) ((this.m_PaddingSize >> 8) & 0xff);
            buffer1[9] = (byte) (this.m_PaddingSize & 0xff);
            if (this.m_IsCRCDataPresent)
            {
                buffer1[10] = (byte) ((this.m_TotalFrameCRC >> 0x18) & 0xff);
                buffer1[11] = (byte) ((this.m_TotalFrameCRC >> 0x10) & 0xff);
                buffer1[12] = (byte) ((this.m_TotalFrameCRC >> 8) & 0xff);
                buffer1[13] = (byte) (this.m_TotalFrameCRC & 0xff);
            }
            return buffer1;
        }

        public void ReadFrom(TagReadingInfo tagReadingInfo, Stream stream)
        {
            int num1 = Utils.ReadInt32(stream);
            ID3v2TagVersion version1 = tagReadingInfo.TagVersion;
            if (num1 >= 0x41000000)
            {
                string text1 = string.Format("FrameID found when expected extended header at position {0}", stream.Position - 4);
                Trace.WriteLine(text1);
                stream.Seek((long) (-4), SeekOrigin.Current);
                this.m_IsCRCDataPresent = false;
                this.m_PaddingSize = 0;
                this.m_TotalFrameCRC = 0;
            }
            else
            {
                byte num2 = Utils.ReadByte(stream);
                Utils.ReadByte(stream);
                this.m_IsCRCDataPresent = (num2 & 0x80) == 0x80;
                this.m_PaddingSize = Utils.ReadInt32(stream);
                if (this.m_IsCRCDataPresent)
                {
                    this.m_TotalFrameCRC = Utils.ReadInt32(stream);
                }
                else
                {
                    this.m_TotalFrameCRC = 0;
                }
            }
        }


        public int CRC32
        {
            get
            {
                return this.m_TotalFrameCRC;
            }
            set
            {
                if (this.m_TotalFrameCRC != value)
                {
                    this.m_TotalFrameCRC = value;
                }
            }
        }

        public bool IsCRCDataPresent
        {
            get
            {
                return this.m_IsCRCDataPresent;
            }
            set
            {
                if (this.m_IsCRCDataPresent != value)
                {
                    this.m_IsCRCDataPresent = value;
                    if (!value)
                    {
                        this.CRC32 = 0;
                    }
                }
            }
        }

        public bool IsTagAnUpdate
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

        public bool IsTagRestricted
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

        public int PaddingSize
        {
            get
            {
                return this.m_PaddingSize;
            }
            set
            {
                if (this.m_PaddingSize != value)
                {
                    this.m_PaddingSize = value;
                }
            }
        }

        public int SizeExcludingSizeBytes
        {
            get
            {
                if (!this.m_IsCRCDataPresent)
                {
                    return 6;
                }
                return 10;
            }
        }

        public int SizeIncludingSizeBytes
        {
            get
            {
                return ((this.m_IsCRCDataPresent ? 10 : 6) + 4);
            }
        }

        public ITagRestrictions TagRestrictions
        {
            get
            {
                throw new NotImplementedException();
            }
        }


        private bool m_IsCRCDataPresent;
        private int m_PaddingSize;
        private int m_TotalFrameCRC;
    }
}

