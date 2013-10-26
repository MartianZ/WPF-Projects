namespace IdSharp.Tagging.ID3v2.Frames
{
    using IdSharp.Tagging.ID3v2;
    using IdSharp.Tagging.ID3v2.Frames.Items;
    using IdSharp.Tagging.ID3v2.Frames.Lists;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.CompilerServices;

    internal sealed class LanguageFrame : ILanguageFrame, IFrame, INotifyPropertyChanged, ITextEncoding
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public LanguageFrame()
        {
            this.m_FrameHeader = new IdSharp.Tagging.ID3v2.FrameHeader();
            this.m_LanguageItems = new LanguageItemBindingList();
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
                bool flag1 = true;
                for (int num1 = 0; num1 < this.Items.Count; num1++)
                {
                    ILanguageItem item1 = this.Items[num1];
                    if (num1 == (this.Items.Count - 1))
                    {
                        flag1 = false;
                    }
                    Utils.Write(stream1, Utils.GetStringBytes(tagVersion, this.TextEncoding, item1.LanguageCode, flag1));
                }
                return this.m_FrameHeader.GetBytes(stream1, tagVersion, this.GetFrameID(tagVersion));
            }
        }

        public string GetFrameID(ID3v2TagVersion tagVersion)
        {
            switch (tagVersion)
            {
                case ID3v2TagVersion.ID3v22:
                    return "TLA";

                case ID3v2TagVersion.ID3v23:
                case ID3v2TagVersion.ID3v24:
                    return "TLAN";
            }
            throw new ArgumentException("Unknown tag version");
        }

        public void Read(TagReadingInfo tagReadingInfo, Stream stream)
        {
            this.m_LanguageItems.Clear();
            this.m_FrameHeader.Read(tagReadingInfo, ref stream);
            int num1 = this.m_FrameHeader.FrameSizeExcludingAdditions;
            if (num1 >= 4)
            {
                this.TextEncoding = (EncodingType) Utils.ReadByte(stream, ref num1);
                string text1 = Utils.ReadString(this.TextEncoding, stream, ref num1);
                if (text1.Length != 3)
                {
                    if ((text1.ToLower() == "english") || (text1.ToLower() == "en"))
                    {
                        this.Items.AddNew().LanguageCode = "eng";
                    }
                    else
                    {
                        foreach (KeyValuePair<string, string> pair1 in LanguageHelper.Languages)
                        {
                            if (pair1.Value.ToLower() == text1.ToLower())
                            {
                                this.Items.AddNew().LanguageCode = pair1.Key;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    this.Items.AddNew().LanguageCode = text1;
                }
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

        public BindingList<ILanguageItem> Items
        {
            get
            {
                return this.m_LanguageItems;
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


        private IdSharp.Tagging.ID3v2.FrameHeader m_FrameHeader;
        private LanguageItemBindingList m_LanguageItems;
        private EncodingType m_TextEncoding;
    }
}

