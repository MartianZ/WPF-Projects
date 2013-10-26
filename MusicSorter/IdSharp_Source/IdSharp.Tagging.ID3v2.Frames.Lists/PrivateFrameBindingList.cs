namespace IdSharp.Tagging.ID3v2.Frames.Lists
{
    using IdSharp.Tagging.ID3v2.Frames;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    internal sealed class PrivateFrameBindingList : BindingList<IPrivateFrame>
    {
        public PrivateFrameBindingList()
        {
            base.AllowNew = true;
        }

        public PrivateFrameBindingList(IList<IPrivateFrame> privateFrameList) : base(privateFrameList)
        {
            base.AllowNew = true;
        }

        protected override object AddNewCore()
        {
            IPrivateFrame frame1 = new PrivateFrame();
            base.Add(frame1);
            return frame1;
        }

        protected override void InsertItem(int index, IPrivateFrame item)
        {
            base.InsertItem(index, item);
        }

    }
}

