namespace IdSharp.Tagging.ID3v2.Frames
{
    using IdSharp.Tagging.ID3v2;
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.CompilerServices;

    internal sealed class UnknownFrame : IFrame, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public UnknownFrame(string frameID, TagReadingInfo tagReadingInfo, Stream stream)
        {
            this.m_FrameID = frameID;
            this.m_FrameHeader = new IdSharp.Tagging.ID3v2.FrameHeader();
            this.Read(tagReadingInfo, stream);
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
            if ((this.m_FrameData == null) || (this.m_FrameData.Length == 0))
            {
                return new byte[0];
            }
            using (MemoryStream stream1 = new MemoryStream(this.m_FrameData))
            {
                return this.m_FrameHeader.GetBytes(stream1, tagVersion, this.GetFrameID(tagVersion));
            }
        }

        public string GetFrameID(ID3v2TagVersion tagVersion)
        {
            switch (tagVersion)
            {
                case ID3v2TagVersion.ID3v22:
                    if (this.m_FrameID.Length == 3)
                    {
                        return this.m_FrameID;
                    }
                    return null;

                case ID3v2TagVersion.ID3v23:
                case ID3v2TagVersion.ID3v24:
                    if (this.m_FrameID.Length == 4)
                    {
                        return this.m_FrameID;
                    }
                    return null;
            }
            throw new ArgumentException("Unknown tag version");
        }

        public void Read(TagReadingInfo tagReadingInfo, Stream stream)
        {
            this.m_FrameHeader.Read(tagReadingInfo, ref stream);
            this.m_FrameData = Utils.Read(stream, this.m_FrameHeader.FrameSizeExcludingAdditions);
        }


        public IFrameHeader FrameHeader
        {
            get
            {
                return this.m_FrameHeader;
            }
        }


        private byte[] m_FrameData;
        private IdSharp.Tagging.ID3v2.FrameHeader m_FrameHeader;
        private string m_FrameID;
    }
}

