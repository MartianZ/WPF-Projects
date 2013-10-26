namespace IdSharp.Tagging.ID3v2.Frames
{
    using IdSharp.Tagging.ID3v2;
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.CompilerServices;

    internal sealed class AudioEncryption : IAudioEncryption, IFrame, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public AudioEncryption()
        {
            this.m_FrameHeader = new IdSharp.Tagging.ID3v2.FrameHeader();
        }

        private void FirePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler1 = this.PropertyChanged;
            if (handler1 != null)
            {
                handler1(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public byte[] GetBytes(ID3v2TagVersion tagVersion)
        {
            using (MemoryStream stream1 = new MemoryStream())
            {
                Utils.Write(stream1, Utils.GetStringBytes(tagVersion, EncodingType.ISO88591, this.OwnerIdentifier, true));
                Utils.Write(stream1, Utils.Get2Bytes(this.PreviewStart));
                Utils.Write(stream1, Utils.Get2Bytes(this.PreviewLength));
                if (this.m_EncryptionInfo != null)
                {
                    Utils.Write(stream1, this.m_EncryptionInfo);
                }
                return this.m_FrameHeader.GetBytes(stream1, tagVersion, this.GetFrameID(tagVersion));
            }
        }

        public string GetFrameID(ID3v2TagVersion tagVersion)
        {
            switch (tagVersion)
            {
                case ID3v2TagVersion.ID3v22:
                    return "CRA";

                case ID3v2TagVersion.ID3v23:
                case ID3v2TagVersion.ID3v24:
                    return "AENC";
            }
            throw new ArgumentException("Unknown tag version");
        }

        public void Read(TagReadingInfo tagReadingInfo, Stream stream)
        {
            this.m_FrameHeader.Read(tagReadingInfo, ref stream);
            int num1 = this.m_FrameHeader.FrameSizeExcludingAdditions;
            if (num1 > 0)
            {
                this.OwnerIdentifier = Utils.ReadString(EncodingType.ISO88591, stream, ref num1);
                if (num1 >= 4)
                {
                    this.PreviewStart = Utils.ReadInt16(stream, ref num1);
                    this.PreviewLength = Utils.ReadInt16(stream, ref num1);
                    if (num1 > 0)
                    {
                        this.EncryptionInfo = Utils.Read(stream, num1);
                        num1 = 0;
                    }
                    else
                    {
                        this.EncryptionInfo = null;
                    }
                }
                else
                {
                    this.PreviewStart = 0;
                    this.PreviewLength = 0;
                    this.EncryptionInfo = null;
                }
            }
            else
            {
                this.OwnerIdentifier = null;
                this.PreviewStart = 0;
                this.PreviewLength = 0;
                this.EncryptionInfo = null;
            }
            if (num1 != 0)
            {
                stream.Seek((long) num1, SeekOrigin.Current);
            }
        }


        public byte[] EncryptionInfo
        {
            get
            {
                if (this.m_EncryptionInfo == null)
                {
                    return null;
                }
                return (byte[]) this.m_EncryptionInfo.Clone();
            }
            set
            {
                if (value == null)
                {
                    this.m_EncryptionInfo = null;
                }
                else
                {
                    this.m_EncryptionInfo = (byte[]) value.Clone();
                }
                this.FirePropertyChanged("EncryptionInfo");
            }
        }

        public IFrameHeader FrameHeader
        {
            get
            {
                return this.m_FrameHeader;
            }
        }

        public string OwnerIdentifier
        {
            get
            {
                return this.m_OwnerIdentifier;
            }
            set
            {
                this.m_OwnerIdentifier = value;
                this.FirePropertyChanged("OwnerIdentifier");
            }
        }

        public short PreviewLength
        {
            get
            {
                return this.m_PreviewLength;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("value", value, "Value cannot be less than 0");
                }
                this.m_PreviewLength = value;
                this.FirePropertyChanged("PreviewLength");
            }
        }

        public short PreviewStart
        {
            get
            {
                return this.m_PreviewStart;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("value", value, "Value cannot be less than 0");
                }
                this.m_PreviewStart = value;
                this.FirePropertyChanged("PreviewStart");
            }
        }


        private byte[] m_EncryptionInfo;
        private IdSharp.Tagging.ID3v2.FrameHeader m_FrameHeader;
        private string m_OwnerIdentifier;
        private short m_PreviewLength;
        private short m_PreviewStart;
    }
}

