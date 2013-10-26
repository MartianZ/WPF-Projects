namespace IdSharp.Tagging.ID3v2.Frames
{
    using IdSharp.Tagging.ID3v2;
    using IdSharp.Tagging.ID3v2.Frames.Items;
    using IdSharp.Tagging.ID3v2.Frames.Lists;
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.CompilerServices;

    internal sealed class SynchronizedText : ISynchronizedText, IFrame, INotifyPropertyChanged, ITextEncoding
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public SynchronizedText()
        {
            this.m_FrameHeader = new IdSharp.Tagging.ID3v2.FrameHeader();
            this.m_SynchronizedTextItemBindingList = new SynchronizedTextItemBindingList();
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
            if (this.Items.Count == 0)
            {
                return new byte[0];
            }
            using (MemoryStream stream1 = new MemoryStream())
            {
                stream1.WriteByte((byte) this.TextEncoding);
                Utils.Write(stream1, Utils.ISO88591GetBytes(this.LanguageCode));
                stream1.WriteByte((byte) this.TimestampFormat);
                stream1.WriteByte((byte) this.ContentType);
                Utils.Write(stream1, Utils.GetStringBytes(tagVersion, this.TextEncoding, this.ContentDescriptor, true));
                foreach (ISynchronizedTextItem item1 in this.Items)
                {
                    Utils.Write(stream1, Utils.GetStringBytes(tagVersion, this.TextEncoding, item1.Text, true));
                    Utils.Write(stream1, Utils.Get4Bytes(item1.Timestamp));
                }
                return this.m_FrameHeader.GetBytes(stream1, tagVersion, this.GetFrameID(tagVersion));
            }
        }

        public string GetFrameID(ID3v2TagVersion tagVersion)
        {
            switch (tagVersion)
            {
                case ID3v2TagVersion.ID3v22:
                    return "SLT";

                case ID3v2TagVersion.ID3v23:
                case ID3v2TagVersion.ID3v24:
                    return "SYLT";
            }
            throw new ArgumentException("Unknown tag version");
        }

        public void Read(TagReadingInfo tagReadingInfo, Stream stream)
        {
            this.Items.Clear();
            this.m_FrameHeader.Read(tagReadingInfo, ref stream);
            int num1 = this.m_FrameHeader.FrameSizeExcludingAdditions;
            if (num1 >= 1)
            {
                this.TextEncoding = (EncodingType) Utils.ReadByte(stream, ref num1);
                if (num1 >= 3)
                {
                    this.LanguageCode = Utils.ReadString(EncodingType.ISO88591, stream, 3);
                    num1 -= 3;
                    if (num1 >= 2)
                    {
                        this.TimestampFormat = (IdSharp.Tagging.ID3v2.TimestampFormat) Utils.ReadByte(stream, ref num1);
                        this.ContentType = (TextContentType) Utils.ReadByte(stream, ref num1);
                        if (num1 > 0)
                        {
                            this.ContentDescriptor = Utils.ReadString(this.TextEncoding, stream, ref num1);
                            while (num1 > 0)
                            {
                                string text1 = Utils.ReadString(this.TextEncoding, stream, ref num1);
                                if (num1 >= 4)
                                {
                                    SynchronizedTextItem item1 = new SynchronizedTextItem();
                                    item1.Text = text1;
                                    item1.Timestamp = Utils.ReadInt32(stream);
                                    num1 -= 4;
                                    this.Items.Add(item1);
                                }
                            }
                        }
                        else
                        {
                            this.ContentDescriptor = "";
                        }
                    }
                    else
                    {
                        this.TimestampFormat = IdSharp.Tagging.ID3v2.TimestampFormat.Milliseconds;
                        this.ContentType = TextContentType.Other;
                        this.ContentDescriptor = "";
                    }
                }
                else
                {
                    this.LanguageCode = "eng";
                    this.TimestampFormat = IdSharp.Tagging.ID3v2.TimestampFormat.Milliseconds;
                    this.ContentType = TextContentType.Other;
                    this.ContentDescriptor = "";
                }
            }
            else
            {
                this.TextEncoding = EncodingType.ISO88591;
                this.LanguageCode = "eng";
                this.TimestampFormat = IdSharp.Tagging.ID3v2.TimestampFormat.Milliseconds;
                this.ContentType = TextContentType.Other;
                this.ContentDescriptor = "";
            }
            if (num1 > 0)
            {
                stream.Seek((long) num1, SeekOrigin.Current);
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

        public TextContentType ContentType
        {
            get
            {
                return this.m_ContentType;
            }
            set
            {
                this.m_ContentType = value;
                this.FirePropertyChanged("ContentType");
            }
        }

        public IFrameHeader FrameHeader
        {
            get
            {
                return this.m_FrameHeader;
            }
        }

        public BindingList<ISynchronizedTextItem> Items
        {
            get
            {
                return this.m_SynchronizedTextItemBindingList;
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

        public IdSharp.Tagging.ID3v2.TimestampFormat TimestampFormat
        {
            get
            {
                return this.m_TimestampFormat;
            }
            set
            {
                this.m_TimestampFormat = value;
                this.FirePropertyChanged("TimestampFormat");
            }
        }


        private string m_ContentDescriptor;
        private TextContentType m_ContentType;
        private IdSharp.Tagging.ID3v2.FrameHeader m_FrameHeader;
        private string m_LanguageCode;
        private SynchronizedTextItemBindingList m_SynchronizedTextItemBindingList;
        private EncodingType m_TextEncoding;
        private IdSharp.Tagging.ID3v2.TimestampFormat m_TimestampFormat;
    }
}

