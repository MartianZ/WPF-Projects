namespace IdSharp.Tagging.ID3v2.Frames.Lists
{
    using IdSharp.Tagging.ID3v2.Frames;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    internal sealed class EncryptedMetaFrameBindingList : BindingList<IEncryptedMetaFrame>
    {
        public EncryptedMetaFrameBindingList()
        {
            base.AllowNew = true;
        }

        public EncryptedMetaFrameBindingList(IList<IEncryptedMetaFrame> encryptedMetaFrameList) : base(encryptedMetaFrameList)
        {
            base.AllowNew = true;
        }

        protected override object AddNewCore()
        {
            IEncryptedMetaFrame frame1 = new EncryptedMetaFrame();
            base.Add(frame1);
            return frame1;
        }

        protected override void InsertItem(int index, IEncryptedMetaFrame item)
        {
            base.InsertItem(index, item);
        }

    }
}

