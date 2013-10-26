namespace IdSharp.Tagging.ID3v2.Frames.Items
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    internal sealed class SynchronizedTextItem : ISynchronizedTextItem, INotifyPropertyChanged
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


        private string m_Text;
        private int m_Timestamp;
    }
}

