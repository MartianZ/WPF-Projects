namespace IdSharp.Tagging.ID3v2.Frames
{
    using IdSharp.Tagging.ID3v2;
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.CompilerServices;

    internal sealed class Comments : IComments, IFrame, INotifyPropertyChanged, ITextEncoding
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Comments()
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
            if (string.IsNullOrEmpty(this.Value))
            {
                return new byte[0];
            }
            if ((this.LanguageCode == null) || (this.LanguageCode.Length != 3))
            {
                this.LanguageCode = "eng";
            }
            byte[] buffer1 = new byte[] { this.TextEncoding };
            byte[] buffer2 = Utils.ISO88591GetBytes(this.LanguageCode);
            byte[] buffer3 = Utils.GetStringBytes(tagVersion, this.TextEncoding, this.Description, true);
            byte[] buffer4 = Utils.GetStringBytes(tagVersion, this.TextEncoding, this.Value, false);
            using (MemoryStream stream1 = new MemoryStream())
            {
                stream1.Write(buffer1, 0, buffer1.Length);
                stream1.Write(buffer2, 0, buffer2.Length);
                stream1.Write(buffer3, 0, buffer3.Length);
                stream1.Write(buffer4, 0, buffer4.Length);
                return this.m_FrameHeader.GetBytes(stream1, tagVersion, this.GetFrameID(tagVersion));
            }
        }

        public string GetFrameID(ID3v2TagVersion tagVersion)
        {
            switch (tagVersion)
            {
                case ID3v2TagVersion.ID3v22:
                    return "COM";

                case ID3v2TagVersion.ID3v23:
                case ID3v2TagVersion.ID3v24:
                    return "COMM";
            }
            throw new ArgumentException("Unknown tag version");
        }

        public void Read(TagReadingInfo tagReadingInfo, Stream stream)
        {
            this.m_FrameHeader.Read(tagReadingInfo, ref stream);
            if (this.m_FrameHeader.FrameSizeExcludingAdditions >= 1)
            {
                this.TextEncoding = (EncodingType) Utils.ReadByte(stream);
                if (this.m_FrameHeader.FrameSizeExcludingAdditions >= 4)
                {
                    string text1 = Utils.ReadString(EncodingType.ISO88591, stream, 3);
                    int num1 = (this.m_FrameHeader.FrameSizeExcludingAdditions - 1) - 3;
                    string text2 = Utils.ReadString(this.TextEncoding, stream, ref num1);
                    bool flag1 = false;
                    if (!LanguageHelper.Languages.ContainsKey(text1.ToLower()) && (text1.ToLower() != "xxx"))
                    {
                        if (text1.StartsWith("en"))
                        {
                            text1 = "";
                        }
                        flag1 = true;
                        if (num1 == 0)
                        {
                            this.Description = "";
                        }
                        else
                        {
                            this.Description = text1 + text2;
                        }
                        this.LanguageCode = "eng";
                    }
                    else
                    {
                        this.LanguageCode = text1;
                        this.Description = text2;
                    }
                    if (num1 > 0)
                    {
                        this.Value = Utils.ReadString(this.TextEncoding, stream, num1);
                    }
                    else if (flag1)
                    {
                        if (text1.Contains("\0"))
                        {
                            this.Value = "";
                        }
                        else
                        {
                            this.Value = text1 + text2;
                        }
                    }
                    else
                    {
                        this.Value = "";
                    }
                }
                else
                {
                    string text3 = string.Format("Under-sized ({0} bytes) COMM frame at position {1}", this.m_FrameHeader.FrameSizeExcludingAdditions, stream.Position);
                    Trace.WriteLine(text3);
                    this.LanguageCode = "eng";
                    this.Value = "";
                }
            }
            else
            {
                string text4 = string.Format("Under-sized ({0} bytes) COMM frame at position {1}", this.m_FrameHeader.FrameSizeExcludingAdditions, stream.Position);
                Trace.WriteLine(text4);
                this.LanguageCode = "eng";
                this.Value = "";
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
                if (string.IsNullOrEmpty(value))
                {
                    this.m_LanguageCode = "eng";
                }
                else
                {
                    this.m_LanguageCode = value.ToLower().Trim();
                    if (this.m_LanguageCode.Length != 3)
                    {
                        string text1 = string.Format("Invalid language code '{0}' in COMM frame", value);
                        Trace.WriteLine(text1);
                        if (this.m_LanguageCode.Length > 3)
                        {
                            this.m_LanguageCode = this.m_LanguageCode.Substring(0, 3);
                        }
                        else
                        {
                            this.m_LanguageCode = this.m_LanguageCode.PadRight(3, ' ');
                        }
                    }
                }
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


        private string m_Description;
        private IdSharp.Tagging.ID3v2.FrameHeader m_FrameHeader;
        private string m_LanguageCode;
        private EncodingType m_TextEncoding;
        private string m_Value;
    }
}

