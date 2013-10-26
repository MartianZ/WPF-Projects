namespace IdSharp.Tagging.ID3v2.Frames
{
    using IdSharp.Tagging.ID3v2;
    using IdSharp.Tagging.ID3v2.Frames.Lists;
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.CompilerServices;

    internal sealed class MpegLookupTable : IMpegLookupTable, IFrame, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public MpegLookupTable()
        {
            this.m_FrameHeader = new IdSharp.Tagging.ID3v2.FrameHeader();
            this.m_MpegLookupTableItemBindingList = new MpegLookupTableItemBindingList();
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
            if (((this.FramesBetweenReference != 0) && (this.BytesBetweenReference != 0)) && ((this.MillisecondsBetweenReference != 0) && (this.Items.Count != 0)))
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
                    return "MLL";

                case ID3v2TagVersion.ID3v23:
                case ID3v2TagVersion.ID3v24:
                    return "MLLT";
            }
            throw new ArgumentException("Unknown tag version");
        }

        public void Read(TagReadingInfo tagReadingInfo, Stream stream)
        {
            this.m_MpegLookupTableItemBindingList.Clear();
            throw new NotImplementedException();
        }


        public int BytesBetweenReference
        {
            get
            {
                return this.m_BytesBetweenReference;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("Value cannot be less than 0");
                }
                if (value > 0xffffff)
                {
                    throw new ArgumentOutOfRangeException("Value cannot be greater than 0xFFFFFF");
                }
                this.m_BytesBetweenReference = value;
                this.FirePropertyChanged("BytesBetweenReference");
            }
        }

        public IFrameHeader FrameHeader
        {
            get
            {
                return this.m_FrameHeader;
            }
        }

        public int FramesBetweenReference
        {
            get
            {
                return this.m_FramesBetweenReference;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("Value cannot be less than 0");
                }
                if (value > 0xffff)
                {
                    throw new ArgumentOutOfRangeException("Value cannot be greater than 0xFFFF");
                }
                this.m_FramesBetweenReference = value;
                this.FirePropertyChanged("FramesBetweenReference");
            }
        }

        public BindingList<IMpegLookupTableItem> Items
        {
            get
            {
                return this.m_MpegLookupTableItemBindingList;
            }
        }

        public int MillisecondsBetweenReference
        {
            get
            {
                return this.m_MillisecondsBetweenReference;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("Value cannot be less than 0");
                }
                if (value > 0xffffff)
                {
                    throw new ArgumentOutOfRangeException("Value cannot be greater than 0xFFFFFF");
                }
                this.m_MillisecondsBetweenReference = value;
                this.FirePropertyChanged("MillisecondsBetweenReference");
            }
        }


        private int m_BytesBetweenReference;
        private IdSharp.Tagging.ID3v2.FrameHeader m_FrameHeader;
        private int m_FramesBetweenReference;
        private int m_MillisecondsBetweenReference;
        private MpegLookupTableItemBindingList m_MpegLookupTableItemBindingList;
    }
}

