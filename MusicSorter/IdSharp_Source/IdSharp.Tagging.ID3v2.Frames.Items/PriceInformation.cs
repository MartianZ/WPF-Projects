namespace IdSharp.Tagging.ID3v2.Frames.Items
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    internal sealed class PriceInformation : IPriceInformation, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void FirePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler1 = this.PropertyChanged;
            if (handler1 != null)
            {
                handler1(this, new PropertyChangedEventArgs(propertyName));
            }
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

        public double Price
        {
            get
            {
                return this.m_Price;
            }
            set
            {
                this.m_Price = value;
                this.FirePropertyChanged("Price");
            }
        }


        private string m_CurrencyCode;
        private double m_Price;
    }
}

