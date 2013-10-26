namespace IdSharp.Tagging.ID3v2.Frames
{
    using IdSharp.Tagging.ID3v2;
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.CompilerServices;

    internal sealed class LinkedInformation : ILinkedInformation, IFrame, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public LinkedInformation()
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
            if ((this.m_AdditionalData == null) && (this.m_AdditionalData.Length == 0))
            {
                return new byte[0];
            }
            if (tagVersion == ID3v2TagVersion.ID3v22)
            {
                if ((this.m_FrameIdentifier == null) || (this.m_FrameIdentifier.Length != 3))
                {
                    return new byte[0];
                }
            }
            else if ((this.m_FrameIdentifier == null) || (this.m_FrameIdentifier.Length != 4))
            {
                return new byte[0];
            }
            using (MemoryStream stream1 = new MemoryStream())
            {
                Utils.Write(stream1, Utils.ISO88591GetBytes(this.m_FrameIdentifier));
                Utils.Write(stream1, Utils.GetStringBytes(tagVersion, EncodingType.ISO88591, this.m_Url, true));
                Utils.Write(stream1, this.m_AdditionalData);
                return this.m_FrameHeader.GetBytes(stream1, tagVersion, this.GetFrameID(tagVersion));
            }
        }

        public string GetFrameID(ID3v2TagVersion tagVersion)
        {
            switch (tagVersion)
            {
                case ID3v2TagVersion.ID3v22:
                    return "LNK";

                case ID3v2TagVersion.ID3v23:
                case ID3v2TagVersion.ID3v24:
                    return "LINK";
            }
            throw new ArgumentException("Unknown tag version");
        }

        public void Read(TagReadingInfo tagReadingInfo, Stream stream)
        {
            this.m_FrameHeader.Read(tagReadingInfo, ref stream);
            int num1 = this.m_FrameHeader.FrameSizeExcludingAdditions;
            int num2 = (tagReadingInfo.TagVersion == ID3v2TagVersion.ID3v22) ? 3 : 4;
            if (num1 > num2)
            {
                this.FrameIdentifier = Utils.ReadString(EncodingType.ISO88591, stream, num2);
                num1 -= num2;
                this.Url = Utils.ReadString(EncodingType.ISO88591, stream, ref num1);
                this.AdditionalData = Utils.Read(stream, num1);
            }
            else
            {
                stream.Seek((long) num1, SeekOrigin.Current);
            }
        }


        public byte[] AdditionalData
        {
            get
            {
                if (this.m_AdditionalData == null)
                {
                    return null;
                }
                return (byte[]) this.m_AdditionalData.Clone();
            }
            set
            {
                if (value == null)
                {
                    this.m_AdditionalData = null;
                }
                else
                {
                    this.m_AdditionalData = (byte[]) value.Clone();
                }
                this.FirePropertyChanged("AdditionalData");
            }
        }

        public IFrameHeader FrameHeader
        {
            get
            {
                return this.m_FrameHeader;
            }
        }

        public string FrameIdentifier
        {
            get
            {
                return this.m_FrameIdentifier;
            }
            set
            {
                this.m_FrameIdentifier = value;
                this.FirePropertyChanged("FrameIdentifier");
            }
        }

        public string Url
        {
            get
            {
                return this.m_Url;
            }
            set
            {
                this.m_Url = value;
                this.FirePropertyChanged("Url");
            }
        }


        private byte[] m_AdditionalData;
        private IdSharp.Tagging.ID3v2.FrameHeader m_FrameHeader;
        private string m_FrameIdentifier;
        private string m_Url;
    }
}

