namespace IdSharp.Tagging.ID3v2.Frames.Items
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    internal sealed class TempoData : ITempoData, INotifyPropertyChanged
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


        public short TempoCode
        {
            get
            {
                return this.m_TempoCode;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("Value cannot be less than 0");
                }
                this.m_TempoCode = value;
                this.FirePropertyChanged("TempoCode");
            }
        }

        public int Timestamp
        {
            get
            {
                return this.m_Timestamp;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("Value cannot be less than 0");
                }
                this.m_Timestamp = value;
                this.FirePropertyChanged("Timestamp");
            }
        }


        private short m_TempoCode;
        private int m_Timestamp;
    }
}

