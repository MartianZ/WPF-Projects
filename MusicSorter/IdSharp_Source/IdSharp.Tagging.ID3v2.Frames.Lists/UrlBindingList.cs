namespace IdSharp.Tagging.ID3v2.Frames.Lists
{
    using IdSharp.Tagging.ID3v2.Frames;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    internal sealed class UrlBindingList : BindingList<IUrlFrame>
    {
        public UrlBindingList()
        {
            throw new NotSupportedException("Use constructor with ID3v2 FrameID's");
        }

        public UrlBindingList(IList<IUrlFrame> urlList) : base(urlList)
        {
            throw new NotSupportedException("Use constructor with ID3v2 FrameID's");
        }

        public UrlBindingList(string ID3v24FrameID, string ID3v23FrameID, string ID3v22FrameID)
        {
            base.AllowNew = true;
            this.m_ID3v24FrameID = ID3v24FrameID;
            this.m_ID3v23FrameID = ID3v23FrameID;
            this.m_ID3v22FrameID = ID3v22FrameID;
        }

        public UrlBindingList(string ID3v24FrameID, string ID3v23FrameID, string ID3v22FrameID, IList<IUrlFrame> urlList) : base(urlList)
        {
            base.AllowNew = true;
            this.m_ID3v24FrameID = ID3v24FrameID;
            this.m_ID3v23FrameID = ID3v23FrameID;
            this.m_ID3v22FrameID = ID3v22FrameID;
        }

        protected override object AddNewCore()
        {
            IUrlFrame frame1 = new UrlFrame(this.m_ID3v24FrameID, this.m_ID3v23FrameID, this.m_ID3v22FrameID);
            base.Add(frame1);
            return frame1;
        }

        protected override void InsertItem(int index, IUrlFrame item)
        {
            base.InsertItem(index, item);
        }


        private string m_ID3v22FrameID;
        private string m_ID3v23FrameID;
        private string m_ID3v24FrameID;
    }
}

