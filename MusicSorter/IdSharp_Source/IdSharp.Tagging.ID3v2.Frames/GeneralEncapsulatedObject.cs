namespace IdSharp.Tagging.ID3v2.Frames
{
    using IdSharp.Tagging.ID3v2;
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.CompilerServices;

    internal sealed class GeneralEncapsulatedObject : IGeneralEncapsulatedObject, IFrame, INotifyPropertyChanged, ITextEncoding
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public GeneralEncapsulatedObject()
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
            if ((this.m_EncapsulatedObject == null) || (this.m_EncapsulatedObject.Length == 0))
            {
                return new byte[0];
            }
            using (MemoryStream stream1 = new MemoryStream())
            {
                stream1.WriteByte((byte) this.TextEncoding);
                Utils.Write(stream1, Utils.GetStringBytes(tagVersion, EncodingType.ISO88591, this.MimeType, true));
                Utils.Write(stream1, Utils.GetStringBytes(tagVersion, this.TextEncoding, this.FileName, true));
                Utils.Write(stream1, Utils.GetStringBytes(tagVersion, this.TextEncoding, this.Description, true));
                stream1.Write(this.m_EncapsulatedObject, 0, this.m_EncapsulatedObject.Length);
                return this.m_FrameHeader.GetBytes(stream1, tagVersion, this.GetFrameID(tagVersion));
            }
        }

        public string GetFrameID(ID3v2TagVersion tagVersion)
        {
            switch (tagVersion)
            {
                case ID3v2TagVersion.ID3v22:
                    return "GEO";

                case ID3v2TagVersion.ID3v23:
                case ID3v2TagVersion.ID3v24:
                    return "GEOB";
            }
            throw new ArgumentException("Unknown tag version");
        }

        public void Read(TagReadingInfo tagReadingInfo, Stream stream)
        {
            this.m_FrameHeader.Read(tagReadingInfo, ref stream);
            int num1 = this.m_FrameHeader.FrameSizeExcludingAdditions;
            if (num1 >= 4)
            {
                this.TextEncoding = (EncodingType) Utils.ReadByte(stream, ref num1);
                this.MimeType = Utils.ReadString(EncodingType.ISO88591, stream, ref num1);
                if (num1 > 0)
                {
                    this.FileName = Utils.ReadString(this.TextEncoding, stream, ref num1);
                    if (num1 > 0)
                    {
                        this.Description = Utils.ReadString(this.TextEncoding, stream, ref num1);
                        if (num1 > 0)
                        {
                            this.EncapsulatedObject = Utils.Read(stream, num1);
                            num1 = 0;
                        }
                    }
                }
            }
            if (num1 > 0)
            {
                stream.Seek((long) num1, SeekOrigin.Current);
            }
        }


        public string Description
        {
            get
            {
                return this.m_Description;
            }
            set
            {
                this.m_Description = value;
                this.FirePropertyChanged("Description");
            }
        }

        public byte[] EncapsulatedObject
        {
            get
            {
                if (this.m_EncapsulatedObject == null)
                {
                    return null;
                }
                return (byte[]) this.m_EncapsulatedObject.Clone();
            }
            set
            {
                if (value == null)
                {
                    this.m_EncapsulatedObject = null;
                }
                else
                {
                    this.m_EncapsulatedObject = (byte[]) value.Clone();
                }
                this.FirePropertyChanged("EncapsulatedObject");
            }
        }

        public string FileName
        {
            get
            {
                return this.m_FileName;
            }
            set
            {
                this.m_FileName = value;
                this.FirePropertyChanged("FileName");
            }
        }

        public IFrameHeader FrameHeader
        {
            get
            {
                return this.m_FrameHeader;
            }
        }

        public string MimeType
        {
            get
            {
                return this.m_MimeType;
            }
            set
            {
                this.m_MimeType = value;
                this.FirePropertyChanged("MimeType");
            }
        }

        public EncodingType TextEncoding
        {
            get
            {
                return this.m_TextEncoding;
            }
            set
            {
                this.m_TextEncoding = value;
                this.FirePropertyChanged("TextEncoding");
            }
        }


        private string m_Description;
        private byte[] m_EncapsulatedObject;
        private string m_FileName;
        private IdSharp.Tagging.ID3v2.FrameHeader m_FrameHeader;
        private string m_MimeType;
        private EncodingType m_TextEncoding;
    }
}

