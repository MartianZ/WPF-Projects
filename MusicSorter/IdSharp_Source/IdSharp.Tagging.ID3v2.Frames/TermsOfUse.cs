namespace IdSharp.Tagging.ID3v2.Frames
{
    using IdSharp.Tagging.ID3v2;
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.CompilerServices;

    internal sealed class TermsOfUse : ITermsOfUse, IFrame, INotifyPropertyChanged, ITextEncoding
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public TermsOfUse()
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
            if (string.IsNullOrEmpty(this.m_Value))
            {
                return new byte[0];
            }
            using (MemoryStream stream1 = new MemoryStream())
            {
                stream1.WriteByte((byte) this.m_TextEncoding);
                Utils.Write(stream1, Utils.ISO88591GetBytes(this.m_LanguageCode));
                Utils.Write(stream1, Utils.GetStringBytes(tagVersion, this.m_TextEncoding, this.m_Value, false));
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
                    return "USER";
            }
            throw new ArgumentException("Unknown tag version");
        }

        public void Read(TagReadingInfo tagReadingInfo, Stream stream)
        {
            this.m_FrameHeader.Read(tagReadingInfo, ref stream);
            int num1 = this.m_FrameHeader.FrameSizeExcludingAdditions;
            if (num1 >= 1)
            {
                this.TextEncoding = (EncodingType) Utils.ReadByte(stream, ref num1);
                if (num1 >= 3)
                {
                    this.LanguageCode = Utils.ReadString(EncodingType.ISO88591, stream, 3);
                    num1 -= 3;
                    if (num1 > 0)
                    {
                        this.Value = Utils.ReadString(this.TextEncoding, stream, num1);
                        num1 = 0;
                    }
                }
                else
                {
                    this.LanguageCode = "eng";
                }
            }
            else
            {
                this.TextEncoding = EncodingType.ISO88591;
                this.LanguageCode = "eng";
            }
            if (num1 > 0)
            {
                stream.Seek((long) num1, SeekOrigin.Current);
            }
        }


        public IFrameHeader FrameHeader
        {
            get
            {
                return this.m_FrameHeader;
            }
        }

        public string LanguageCode
        {
            get
            {
                return this.m_LanguageCode;
            }
            set
            {
                this.m_LanguageCode = value;
                this.FirePropertyChanged("LanguageCode");
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

        public string Value
        {
            get
            {
                return this.m_Value;
            }
            set
            {
                this.m_Value = value;
                this.FirePropertyChanged("Value");
            }
        }


        private IdSharp.Tagging.ID3v2.FrameHeader m_FrameHeader;
        private string m_LanguageCode;
        private EncodingType m_TextEncoding;
        private string m_Value;
    }
}

