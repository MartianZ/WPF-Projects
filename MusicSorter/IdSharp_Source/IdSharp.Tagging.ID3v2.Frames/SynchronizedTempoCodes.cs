namespace IdSharp.Tagging.ID3v2.Frames
{
    using IdSharp.Tagging.ID3v2;
    using IdSharp.Tagging.ID3v2.Frames.Lists;
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.CompilerServices;

    internal sealed class SynchronizedTempoCodes : ISynchronizedTempoCodes, IFrame, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public SynchronizedTempoCodes()
        {
            this.m_FrameHeader = new IdSharp.Tagging.ID3v2.FrameHeader();
            this.m_TempoDataBindingList = new TempoDataBindingList();
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
                    return "STC";

                case ID3v2TagVersion.ID3v23:
                case ID3v2TagVersion.ID3v24:
                    return "SYTC";
            }
            throw new ArgumentException("Unknown tag version");
        }

        public void Read(TagReadingInfo tagReadingInfo, Stream stream)
        {
            this.m_TempoDataBindingList.Clear();
            throw new NotImplementedException();
        }


        public IFrameHeader FrameHeader
        {
            get
            {
                return this.m_FrameHeader;
            }
        }

        public BindingList<ITempoData> Items
        {
            get
            {
                return this.m_TempoDataBindingList;
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


        private IdSharp.Tagging.ID3v2.FrameHeader m_FrameHeader;
        private TempoDataBindingList m_TempoDataBindingList;
        private IdSharp.Tagging.ID3v2.TimestampFormat m_TimestampFormat;
    }
}

