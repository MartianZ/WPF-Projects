namespace IdSharp.Tagging.ID3v2.Frames
{
    using IdSharp.Tagging.ID3v2;
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.CompilerServices;

    internal sealed class EncryptionMethod : IEncryptionMethod, IFrame, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public EncryptionMethod()
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
                Utils.Write(stream1, Utils.ISO88591GetBytes(this.m_OwnerIdentifier));
                stream1.WriteByte(0);
                stream1.WriteByte(this.m_MethodSymbol);
                if (this.m_EncryptionData != null)
                {
                    Utils.Write(stream1, this.m_EncryptionData);
                }
                return this.m_FrameHeader.GetBytes(stream1, tagVersion, this.GetFrameID(tagVersion));
            }
        }

        public string GetFrameID(ID3v2TagVersion tagVersion)
        {
            switch (tagVersion)
            {
                case ID3v2TagVersion.ID3v22:
                    return null;

                case ID3v2TagVersion.ID3v23:
                case ID3v2TagVersion.ID3v24:
                    return "ENCR";
            }
            throw new ArgumentException("Unknown tag version");
        }

        public void Read(TagReadingInfo tagReadingInfo, Stream stream)
        {
            this.Reset();
            this.m_FrameHeader.Read(tagReadingInfo, ref stream);
            int num1 = this.m_FrameHeader.FrameSizeExcludingAdditions;
            if (num1 > 0)
            {
                this.OwnerIdentifier = Utils.ReadString(EncodingType.ISO88591, stream, ref num1);
                if (num1 > 0)
                {
                    this.MethodSymbol = Utils.ReadByte(stream, ref num1);
                    if (num1 > 0)
                    {
                        this.EncryptionData = Utils.Read(stream, num1);
                        num1 = 0;
                    }
                }
            }
            if (num1 != 0)
            {
                stream.Seek((long) num1, SeekOrigin.Current);
            }
        }

        private void Reset()
        {
            this.OwnerIdentifier = null;
            this.MethodSymbol = 0;
            this.EncryptionData = null;
        }


        public byte[] EncryptionData
        {
            get
            {
                if (this.m_EncryptionData == null)
                {
                    return null;
                }
                return (byte[]) this.m_EncryptionData.Clone();
            }
            set
            {
                if (value == null)
                {
                    this.m_EncryptionData = null;
                }
                else
                {
                    this.m_EncryptionData = (byte[]) value.Clone();
                }
                this.FirePropertyChanged("EncryptionData");
            }
        }

        public IFrameHeader FrameHeader
        {
            get
            {
                return this.m_FrameHeader;
            }
        }

        public byte MethodSymbol
        {
            get
            {
                return this.m_MethodSymbol;
            }
            set
            {
                this.m_MethodSymbol = value;
                this.FirePropertyChanged("MethodSymbol");
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


        private byte[] m_EncryptionData;
        private IdSharp.Tagging.ID3v2.FrameHeader m_FrameHeader;
        private byte m_MethodSymbol;
        private string m_OwnerIdentifier;
    }
}

