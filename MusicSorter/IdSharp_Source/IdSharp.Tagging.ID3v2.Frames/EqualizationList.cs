namespace IdSharp.Tagging.ID3v2.Frames
{
    using IdSharp.Tagging.ID3v2;
    using IdSharp.Tagging.ID3v2.Frames.Lists;
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.CompilerServices;

    internal sealed class EqualizationList : IEqualizationList, IFrame, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public EqualizationList()
        {
            this.m_FrameHeader = new IdSharp.Tagging.ID3v2.FrameHeader();
            this.m_Items = new EqualizationItemBindingList();
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
                    return "EQU";

                case ID3v2TagVersion.ID3v23:
                    return "EQUA";

                case ID3v2TagVersion.ID3v24:
                    return "EQU2";
            }
            throw new ArgumentException("Unknown tag version");
        }

        public void Read(TagReadingInfo tagReadingInfo, Stream stream)
        {
            this.m_FrameHeader.Read(tagReadingInfo, ref stream);
            int num1 = this.m_FrameHeader.FrameSizeExcludingAdditions;
            if (num1 != 0)
            {
                stream.Seek((long) num1, SeekOrigin.Current);
            }
            throw new NotImplementedException();
        }


        public IFrameHeader FrameHeader
        {
            get
            {
                return this.m_FrameHeader;
            }
        }

        public string Identification
        {
            get
            {
                return this.m_Identification;
            }
            set
            {
                this.m_Identification = value;
                this.FirePropertyChanged("Identification");
            }
        }

        public IdSharp.Tagging.ID3v2.InterpolationMethod InterpolationMethod
        {
            get
            {
                return this.m_InterpolationMethod;
            }
            set
            {
                this.m_InterpolationMethod = value;
                this.FirePropertyChanged("InterpolationMethod");
            }
        }

        public BindingList<IEqualizationItem> Items
        {
            get
            {
                return this.m_Items;
            }
        }


        private IdSharp.Tagging.ID3v2.FrameHeader m_FrameHeader;
        private string m_Identification;
        private IdSharp.Tagging.ID3v2.InterpolationMethod m_InterpolationMethod;
        private BindingList<IEqualizationItem> m_Items;
    }
}

