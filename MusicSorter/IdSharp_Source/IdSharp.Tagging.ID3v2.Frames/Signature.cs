namespace IdSharp.Tagging.ID3v2.Frames
{
    using IdSharp.Tagging.ID3v2;
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.CompilerServices;

    internal sealed class Signature : ISignature, IFrame, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Signature()
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
            throw new NotImplementedException();
        }

        public string GetFrameID(ID3v2TagVersion tagVersion)
        {
            switch (tagVersion)
            {
                case ID3v2TagVersion.ID3v22:
                    return null;

                case ID3v2TagVersion.ID3v23:
                case ID3v2TagVersion.ID3v24:
                    return "SIGN";
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

        public byte GroupSymbol
        {
            get
            {
                return this.m_GroupSymbol;
            }
            set
            {
                this.m_GroupSymbol = value;
                this.FirePropertyChanged("GroupSymbol");
            }
        }

        public byte[] SignatureData
        {
            get
            {
                if (this.m_SignatureData == null)
                {
                    return null;
                }
                return (byte[]) this.m_SignatureData.Clone();
            }
            set
            {
                if (value == null)
                {
                    this.m_SignatureData = null;
                }
                else
                {
                    this.m_SignatureData = (byte[]) value.Clone();
                }
                this.FirePropertyChanged("SignatureData");
            }
        }


        private IdSharp.Tagging.ID3v2.FrameHeader m_FrameHeader;
        private byte m_GroupSymbol;
        private byte[] m_SignatureData;
    }
}

