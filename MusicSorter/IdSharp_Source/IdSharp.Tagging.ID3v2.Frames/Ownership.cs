namespace IdSharp.Tagging.ID3v2.Frames
{
    using IdSharp.Tagging.ID3v2;
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.CompilerServices;

    internal sealed class Ownership : IOwnership, IFrame, INotifyPropertyChanged, ITextEncoding
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Ownership()
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
            if (((this.PricePaid != 0) || !string.IsNullOrEmpty(this.CurrencyCode)) || ((this.DateOfPurchase != DateTime.MinValue) || !string.IsNullOrEmpty(this.Seller)))
            {
                throw new NotImplementedException();
            }
            return new byte[0];
        }

        public string GetFrameID(ID3v2TagVersion tagVersion)
        {
            switch (tagVersion)
            {
                case ID3v2TagVersion.ID3v22:
                    return null;

                case ID3v2TagVersion.ID3v23:
                case ID3v2TagVersion.ID3v24:
                    return "OWNE";
            }
            throw new ArgumentException("Unknown tag version");
        }

        public void Read(TagReadingInfo tagReadingInfo, Stream stream)
        {
            throw new NotImplementedException();
        }


        public string CurrencyCode
        {
            get
            {
                return this.m_CurrencyCode;
            }
            set
            {
                this.m_CurrencyCode = value;
                this.FirePropertyChanged("CurrencyCode");
            }
        }

        public DateTime DateOfPurchase
        {
            get
            {
                return this.m_DateOfPurchase.Date;
            }
            set
            {
                this.m_DateOfPurchase = value.Date;
                this.FirePropertyChanged("DateOfPurchase");
            }
        }

        public IFrameHeader FrameHeader
        {
            get
            {
                return this.m_FrameHeader;
            }
        }

        public double PricePaid
        {
            get
            {
                return this.m_PricePaid;
            }
            set
            {
                this.m_PricePaid = value;
                this.FirePropertyChanged("PricePaid");
            }
        }

        public string Seller
        {
            get
            {
                return this.m_Seller;
            }
            set
            {
                this.m_Seller = value;
                this.FirePropertyChanged("Seller");
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


        private string m_CurrencyCode;
        private DateTime m_DateOfPurchase;
        private IdSharp.Tagging.ID3v2.FrameHeader m_FrameHeader;
        private double m_PricePaid;
        private string m_Seller;
        private EncodingType m_TextEncoding;
    }
}

