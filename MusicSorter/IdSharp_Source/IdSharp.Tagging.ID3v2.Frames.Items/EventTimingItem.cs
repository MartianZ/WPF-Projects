namespace IdSharp.Tagging.ID3v2.Frames.Items
{
    using IdSharp.Tagging.ID3v2;
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    internal sealed class EventTimingItem : IEventTimingItem, INotifyPropertyChanged
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


        public MusicEvent EventType
        {
            get
            {
                return this.m_EventType;
            }
            set
            {
                this.m_EventType = value;
                this.FirePropertyChanged("EventType");
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


        private MusicEvent m_EventType;
        private int m_Timestamp;
    }
}

