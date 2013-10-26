namespace IdSharp.Tagging.ID3v2.Frames
{
    using IdSharp.Tagging.ID3v2;
    using IdSharp.Tagging.ID3v2.Frames.Lists;
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.CompilerServices;

    internal sealed class EventTiming : IEventTiming, IFrame, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public EventTiming()
        {
            this.m_FrameHeader = new IdSharp.Tagging.ID3v2.FrameHeader();
            this.m_EventTimingItemBindingList = new EventTimingItemBindingList();
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
            if (this.Items.Count != 0)
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
                    return "ETC";

                case ID3v2TagVersion.ID3v23:
                case ID3v2TagVersion.ID3v24:
                    return "ETCO";
            }
            throw new ArgumentException("Unknown tag version");
        }

        public void Read(TagReadingInfo tagReadingInfo, Stream stream)
        {
            this.m_EventTimingItemBindingList.Clear();
            throw new NotImplementedException();
        }


        public IFrameHeader FrameHeader
        {
            get
            {
                return this.m_FrameHeader;
            }
        }

        public BindingList<IEventTimingItem> Items
        {
            get
            {
                return this.m_EventTimingItemBindingList;
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


        private EventTimingItemBindingList m_EventTimingItemBindingList;
        private IdSharp.Tagging.ID3v2.FrameHeader m_FrameHeader;
        private IdSharp.Tagging.ID3v2.TimestampFormat m_TimestampFormat;
    }
}

