namespace IdSharp.Tagging.ID3v2.Frames
{
    using IdSharp.Tagging.ID3v2;
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.CompilerServices;

    internal sealed class TextFrame : ITextFrame, IFrame, INotifyPropertyChanged, ITextEncoding
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public TextFrame(string ID3v24FrameID, string ID3v23FrameID, string ID3v22FrameID)
        {
            this.m_FrameHeader = new IdSharp.Tagging.ID3v2.FrameHeader();
            this.m_ID3v24FrameID = ID3v24FrameID;
            this.m_ID3v23FrameID = ID3v23FrameID;
            this.m_ID3v22FrameID = ID3v22FrameID;
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
            if (string.IsNullOrEmpty(this.Value))
            {
                return new byte[0];
            }
            using (MemoryStream stream1 = new MemoryStream())
            {
                stream1.WriteByte((byte) this.m_TextEncoding);
                Utils.Write(stream1, Utils.GetStringBytes(tagVersion, this.TextEncoding, this.Value, false));
                return this.m_FrameHeader.GetBytes(stream1, tagVersion, this.GetFrameID(tagVersion));
            }
        }

        public string GetFrameID(ID3v2TagVersion tagVersion)
        {
            switch (tagVersion)
            {
                case ID3v2TagVersion.ID3v22:
                    return this.m_ID3v22FrameID;

                case ID3v2TagVersion.ID3v23:
                    return this.m_ID3v23FrameID;

                case ID3v2TagVersion.ID3v24:
                    return this.m_ID3v24FrameID;
            }
            throw new ArgumentException("Unknown tag version");
        }

        public void Read(TagReadingInfo tagReadingInfo, Stream stream)
        {
            this.m_FrameHeader.Read(tagReadingInfo, ref stream);
            if (this.m_FrameHeader.FrameSizeExcludingAdditions >= 1)
            {
                this.TextEncoding = (EncodingType) Utils.ReadByte(stream);
                this.Value = Utils.ReadString(this.m_TextEncoding, stream, this.m_FrameHeader.FrameSizeExcludingAdditions - 1);
            }
            else
            {
                this.TextEncoding = EncodingType.ISO88591;
                this.Value = "";
            }
        }


        public IFrameHeader FrameHeader
        {
            get
            {
                return this.m_FrameHeader;
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
                if (string.IsNullOrEmpty(value))
                {
                    this.m_Value = value;
                }
                else
                {
                    this.m_Value = value.Trim();
                }
                this.FirePropertyChanged("Value");
            }
        }


        private IdSharp.Tagging.ID3v2.FrameHeader m_FrameHeader;
        private string m_ID3v22FrameID;
        private string m_ID3v23FrameID;
        private string m_ID3v24FrameID;
        private EncodingType m_TextEncoding;
        private string m_Value;
    }
}

