namespace IdSharp.Tagging.ID3v2.Frames
{
    using IdSharp.Tagging.ID3v2;
    using IdSharp.Tagging.ID3v2.Frames.Items;
    using IdSharp.Tagging.ID3v2.Frames.Lists;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.IO;
    using System.Runtime.CompilerServices;

    internal sealed class Commercial : ICommercial, IFrame, INotifyPropertyChanged, ITextEncoding
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Commercial()
        {
            this.m_FrameHeader = new IdSharp.Tagging.ID3v2.FrameHeader();
            this.m_PriceList = new PriceInformationBindingList();
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
            if (this.m_PriceList.Count == 0)
            {
                return new byte[0];
            }
            using (MemoryStream stream1 = new MemoryStream())
            {
                stream1.WriteByte((byte) this.m_TextEncoding);
                string text1 = "";
                using (IEnumerator<IPriceInformation> enumerator1 = this.m_PriceList.GetEnumerator())
                {
                    while (enumerator1.MoveNext())
                    {
                        IPriceInformation information1 = enumerator1.Current;
                        if ((information1.CurrencyCode != null) && (information1.CurrencyCode.Length == 3))
                        {
                            if (text1 != "")
                            {
                                text1 = text1 + "/";
                            }
                            text1 = text1 + string.Format("{0}{1:0.00}", information1.CurrencyCode, information1.Price);
                        }
                    }
                }
                if (text1 == "")
                {
                    return new byte[0];
                }
                Utils.Write(stream1, Utils.ISO88591GetBytes(text1));
                stream1.WriteByte(0);
                Utils.Write(stream1, Utils.ISO88591GetBytes(this.m_ValidUntil.ToString("yyyyMMdd")));
                Utils.Write(stream1, Utils.ISO88591GetBytes(this.m_ContactUrl));
                stream1.WriteByte(0);
                stream1.WriteByte((byte) this.m_ReceivedAs);
                Utils.Write(stream1, Utils.GetStringBytes(tagVersion, this.m_TextEncoding, this.m_NameOfSeller, true));
                Utils.Write(stream1, Utils.GetStringBytes(tagVersion, this.m_TextEncoding, this.m_Description, true));
                if ((this.m_SellerLogo != null) && (this.m_SellerLogo.Length != 0))
                {
                    Utils.Write(stream1, Utils.ISO88591GetBytes(this.m_SellerLogoMimeType));
                    stream1.WriteByte(0);
                    Utils.Write(stream1, this.m_SellerLogo);
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
                    return "COMR";
            }
            throw new ArgumentException("Unknown tag version");
        }

        public void Read(TagReadingInfo tagReadingInfo, Stream stream)
        {
            this.Reset();
            this.m_FrameHeader.Read(tagReadingInfo, ref stream);
            int num1 = this.m_FrameHeader.FrameSizeExcludingAdditions;
            if (num1 > 1)
            {
                this.TextEncoding = (EncodingType) Utils.ReadByte(stream, ref num1);
                string text1 = Utils.ReadString(EncodingType.ISO88591, stream, ref num1);
                if (!string.IsNullOrEmpty(text1))
                {
                    foreach (string text2 in text1.Split(new char[] { '/' }))
                    {
                        if (text2.Length > 3)
                        {
                            double num2;
                            string text3 = text2.Substring(3, text2.Length - 3);
                            if (double.TryParse(text3, NumberStyles.Any, NumberFormatInfo.InvariantInfo, out num2))
                            {
                                IPriceInformation information1 = new PriceInformation();
                                information1.CurrencyCode = text2.Substring(0, 3);
                                information1.Price = num2;
                                this.m_PriceList.Add(information1);
                            }
                        }
                    }
                }
                if (num1 > 0)
                {
                    string text4 = Utils.ReadString(EncodingType.ISO88591, stream, 8);
                    num1 -= 8;
                    if (text4.Length == 8)
                    {
                        text4 = string.Format("{0}-{1}-{2}", text4.Substring(0, 4), text4.Substring(4, 2), text4.Substring(6, 2));
                        DateTime.TryParse(text4, out this.m_ValidUntil);
                    }
                    if (num1 > 0)
                    {
                        this.ContactUrl = Utils.ReadString(EncodingType.ISO88591, stream, ref num1);
                        if (num1 > 0)
                        {
                            this.ReceivedAs = (IdSharp.Tagging.ID3v2.ReceivedAs) Utils.ReadByte(stream, ref num1);
                            this.NameOfSeller = Utils.ReadString(this.TextEncoding, stream, ref num1);
                            this.Description = Utils.ReadString(this.TextEncoding, stream, ref num1);
                            this.SellerLogoMimeType = Utils.ReadString(EncodingType.ISO88591, stream, ref num1);
                            if (num1 > 0)
                            {
                                this.SellerLogo = Utils.Read(stream, num1);
                                num1 = 0;
                            }
                        }
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
            this.TextEncoding = EncodingType.ISO88591;
            this.PriceList.Clear();
            this.ValidUntil = DateTime.MinValue;
            this.ContactUrl = null;
            this.ReceivedAs = IdSharp.Tagging.ID3v2.ReceivedAs.Other;
            this.NameOfSeller = null;
            this.Description = null;
            this.SellerLogoMimeType = null;
            this.SellerLogo = null;
        }


        public string ContactUrl
        {
            get
            {
                return this.m_ContactUrl;
            }
            set
            {
                this.m_ContactUrl = value;
                this.FirePropertyChanged("ContactUrl");
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

        public string NameOfSeller
        {
            get
            {
                return this.m_NameOfSeller;
            }
            set
            {
                this.m_NameOfSeller = value;
                this.FirePropertyChanged("NameOfSeller");
            }
        }

        public BindingList<IPriceInformation> PriceList
        {
            get
            {
                return this.m_PriceList;
            }
        }

        public IdSharp.Tagging.ID3v2.ReceivedAs ReceivedAs
        {
            get
            {
                return this.m_ReceivedAs;
            }
            set
            {
                this.m_ReceivedAs = value;
                this.FirePropertyChanged("ReceivedAs");
            }
        }

        public byte[] SellerLogo
        {
            get
            {
                if (this.m_SellerLogo == null)
                {
                    return null;
                }
                return (byte[]) this.m_SellerLogo.Clone();
            }
            set
            {
                if (value == null)
                {
                    this.m_SellerLogo = null;
                }
                else
                {
                    this.m_SellerLogo = (byte[]) value.Clone();
                }
                this.FirePropertyChanged("SellerLogo");
            }
        }

        public string SellerLogoMimeType
        {
            get
            {
                return this.m_SellerLogoMimeType;
            }
            set
            {
                this.m_SellerLogoMimeType = value;
                this.FirePropertyChanged("SellerLogoMimeType");
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

        public DateTime ValidUntil
        {
            get
            {
                return this.m_ValidUntil.Date;
            }
            set
            {
                this.m_ValidUntil = value.Date;
                this.FirePropertyChanged("ValidUntil");
            }
        }


        private string m_ContactUrl;
        private string m_Description;
        private IdSharp.Tagging.ID3v2.FrameHeader m_FrameHeader;
        private string m_NameOfSeller;
        private PriceInformationBindingList m_PriceList;
        private IdSharp.Tagging.ID3v2.ReceivedAs m_ReceivedAs;
        private byte[] m_SellerLogo;
        private string m_SellerLogoMimeType;
        private EncodingType m_TextEncoding;
        private DateTime m_ValidUntil;
    }
}

