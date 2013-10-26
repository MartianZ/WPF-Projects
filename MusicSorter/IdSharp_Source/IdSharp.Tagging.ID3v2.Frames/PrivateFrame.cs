namespace IdSharp.Tagging.ID3v2.Frames
{
    using IdSharp.Tagging.ID3v2;
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.CompilerServices;

    internal sealed class PrivateFrame : IPrivateFrame, IFrame, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public PrivateFrame()
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
            if ((this.m_PrivateData == null) || (this.m_PrivateData.Length == 0))
            {
                return new byte[0];
            }
            using (MemoryStream stream1 = new MemoryStream())
            {
                Utils.Write(stream1, Utils.ISO88591GetBytes(this.OwnerIdentifier));
                stream1.WriteByte(0);
                Utils.Write(stream1, this.m_PrivateData);
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
                    return "PRIV";
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
                if (num1 > 0)
                {
                    this.PrivateData = Utils.Read(stream, num1);
                }
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

        public byte[] PrivateData
        {
            get
            {
                if (this.m_PrivateData == null)
                {
                    return null;
                }
                return (byte[]) this.m_PrivateData.Clone();
            }
            set
            {
                if (value == null)
                {
                    this.m_PrivateData = null;
                }
                else
                {
                    this.m_PrivateData = (byte[]) value.Clone();
                }
                this.FirePropertyChanged("PrivateData");
            }
        }


        private IdSharp.Tagging.ID3v2.FrameHeader m_FrameHeader;
        private string m_OwnerIdentifier;
        private byte[] m_PrivateData;
    }
}

