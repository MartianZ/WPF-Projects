namespace IdSharp.Tagging.ID3v2.Frames
{
    using IdSharp.Tagging.ID3v2;
    using IdSharp.Tagging.ID3v2.Frames.Lists;
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.CompilerServices;

    internal sealed class MusicianCreditsList : IMusicianCreditsList, IFrame, INotifyPropertyChanged, ITextEncoding
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public MusicianCreditsList()
        {
            this.m_FrameHeader = new IdSharp.Tagging.ID3v2.FrameHeader();
            this.m_MusicianCreditsItemBindingList = new MusicianCreditsItemBindingList();
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
                    return null;

                case ID3v2TagVersion.ID3v23:
                case ID3v2TagVersion.ID3v24:
                    return "TMCL";
            }
            throw new ArgumentException("Unknown tag version");
        }

        public void Read(TagReadingInfo tagReadingInfo, Stream stream)
        {
            this.m_MusicianCreditsItemBindingList.Clear();
            throw new NotImplementedException();
        }


        public IFrameHeader FrameHeader
        {
            get
            {
                return this.m_FrameHeader;
            }
        }

        public BindingList<IMusicianCreditsItem> Items
        {
            get
            {
                return this.m_MusicianCreditsItemBindingList;
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


        private IdSharp.Tagging.ID3v2.FrameHeader m_FrameHeader;
        private MusicianCreditsItemBindingList m_MusicianCreditsItemBindingList;
        private EncodingType m_TextEncoding;
    }
}

