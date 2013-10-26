namespace IdSharp.Tagging.ID3v2.Frames
{
    using IdSharp.Tagging.ID3v2;
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.CompilerServices;

    internal sealed class UnsynchronizedText : IUnsynchronizedText, IFrame, INotifyPropertyChanged, ITextEncoding
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public UnsynchronizedText()
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
            if (string.IsNullOrEmpty(this.Text))
            {
                return new byte[0];
            }
            using (MemoryStream stream1 = new MemoryStream())
            {
                stream1.WriteByte((byte) this.TextEncoding);
                Utils.Write(stream1, Utils.ISO88591GetBytes(this.LanguageCode));
                Utils.Write(stream1, Utils.GetStringBytes(tagVersion, this.TextEncoding, this.ContentDescriptor, true));
                Utils.Write(stream1, Utils.GetStringBytes(tagVersion, this.TextEncoding, this.Text, false));
                return this.m_FrameHeader.GetBytes(stream1, tagVersion, this.GetFrameID(tagVersion));
            }
        }

        public string GetFrameID(ID3v2TagVersion tagVersion)
        {
            switch (tagVersion)
            {
                case ID3v2TagVersion.ID3v22:
                    return "ULT";

                case ID3v2TagVersion.ID3v23:
                case ID3v2TagVersion.ID3v24:
                    return "USLT";
            }
            throw new ArgumentException("Unknown tag version");
        }

        public void Read(TagReadingInfo tagReadingInfo, Stream stream)
        {
            this.m_FrameHeader.Read(tagReadingInfo, ref stream);
            if (this.m_FrameHeader.FrameSizeExcludingAdditions >= 4)
            {
                this.TextEncoding = (EncodingType) Utils.ReadByte(stream);
                this.LanguageCode = Utils.ReadString(EncodingType.ISO88591, stream, 3);
                int num1 = (this.m_FrameHeader.FrameSizeExcludingAdditions - 1) - 3;
                this.ContentDescriptor = Utils.ReadString(this.TextEncoding, stream, ref num1);
                this.Text = Utils.ReadString(this.m_TextEncoding, stream, num1);
            }
            else
            {
                string text1 = string.Format("Under-sized ({0} bytes) unsynchronized text frame at position {1}", this.m_FrameHeader.FrameSizeExcludingAdditions, stream.Position);
                Trace.WriteLine(text1);
                this.LanguageCode = "eng";
                this.ContentDescriptor = "";
                this.Text = "";
            }
        }


        public string ContentDescriptor
        {
            get
            {
                return this.m_ContentDescriptor;
            }
            set
            {
                this.m_ContentDescriptor = value;
                this.FirePropertyChanged("ContentDescriptor");
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

        public string Text
        {
            get
            {
                return this.m_Text;
            }
            set
            {
                this.m_Text = value;
                this.FirePropertyChanged("Text");
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


        private string m_ContentDescriptor;
        private IdSharp.Tagging.ID3v2.FrameHeader m_FrameHeader;
        private string m_LanguageCode;
        private string m_Text;
        private EncodingType m_TextEncoding;
    }
}

