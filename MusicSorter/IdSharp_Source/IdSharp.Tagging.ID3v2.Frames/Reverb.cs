namespace IdSharp.Tagging.ID3v2.Frames
{
    using IdSharp.Tagging.ID3v2;
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.CompilerServices;

    internal sealed class Reverb : IReverb, IFrame, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Reverb()
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
            if ((this.ReverbLeftMilliseconds != 0) || (this.ReverbRightMilliseconds != 0))
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
                    return "REV";

                case ID3v2TagVersion.ID3v23:
                case ID3v2TagVersion.ID3v24:
                    return "RVRB";
            }
            throw new ArgumentException("Unknown tag version");
        }

        public void Read(TagReadingInfo tagReadingInfo, Stream stream)
        {
            throw new NotImplementedException();
        }


        public IFrameHeader FrameHeader
        {
            get
            {
                return this.m_FrameHeader;
            }
        }

        public byte PremixLeftToRight
        {
            get
            {
                return this.m_PremixLeftToRight;
            }
            set
            {
                this.m_PremixLeftToRight = value;
                this.FirePropertyChanged("PremixLeftToRight");
            }
        }

        public byte PremixRightToLeft
        {
            get
            {
                return this.m_PremixRightToLeft;
            }
            set
            {
                this.m_PremixRightToLeft = value;
                this.FirePropertyChanged("PremixRightToLeft");
            }
        }

        public byte ReverbBouncesLeft
        {
            get
            {
                return this.m_ReverbBouncesLeft;
            }
            set
            {
                this.m_ReverbBouncesLeft = value;
                this.FirePropertyChanged("ReverbBouncesLeft");
            }
        }

        public byte ReverbBouncesRight
        {
            get
            {
                return this.m_ReverbBouncesRight;
            }
            set
            {
                this.m_ReverbBouncesRight = value;
                this.FirePropertyChanged("ReverbBouncesRight");
            }
        }

        public byte ReverbFeedbackLeftToLeft
        {
            get
            {
                return this.m_ReverbFeedbackLeftToLeft;
            }
            set
            {
                this.m_ReverbFeedbackLeftToLeft = value;
                this.FirePropertyChanged("ReverbFeedbackLeftToLeft");
            }
        }

        public byte ReverbFeedbackLeftToRight
        {
            get
            {
                return this.m_ReverbFeedbackLeftToRight;
            }
            set
            {
                this.m_ReverbFeedbackLeftToRight = value;
                this.FirePropertyChanged("ReverbFeedbackLeftToRight");
            }
        }

        public byte ReverbFeedbackRightToLeft
        {
            get
            {
                return this.m_ReverbFeedbackRightToLeft;
            }
            set
            {
                this.m_ReverbFeedbackRightToLeft = value;
                this.FirePropertyChanged("ReverbFeedbackRightToLeft");
            }
        }

        public byte ReverbFeedbackRightToRight
        {
            get
            {
                return this.m_ReverbFeedbackRightToRight;
            }
            set
            {
                this.m_ReverbFeedbackRightToRight = value;
                this.FirePropertyChanged("ReverbFeedbackRightToRight");
            }
        }

        public short ReverbLeftMilliseconds
        {
            get
            {
                return this.m_ReverbLeftMilliseconds;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("Value cannot be less than 0");
                }
                this.m_ReverbLeftMilliseconds = value;
                this.FirePropertyChanged("ReverbLeftMilliseconds");
            }
        }

        public short ReverbRightMilliseconds
        {
            get
            {
                return this.m_ReverbRightMilliseconds;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("Value cannot be less than 0");
                }
                this.m_ReverbRightMilliseconds = value;
                this.FirePropertyChanged("ReverbRightMilliseconds");
            }
        }


        private IdSharp.Tagging.ID3v2.FrameHeader m_FrameHeader;
        private byte m_PremixLeftToRight;
        private byte m_PremixRightToLeft;
        private byte m_ReverbBouncesLeft;
        private byte m_ReverbBouncesRight;
        private byte m_ReverbFeedbackLeftToLeft;
        private byte m_ReverbFeedbackLeftToRight;
        private byte m_ReverbFeedbackRightToLeft;
        private byte m_ReverbFeedbackRightToRight;
        private short m_ReverbLeftMilliseconds;
        private short m_ReverbRightMilliseconds;
    }
}

