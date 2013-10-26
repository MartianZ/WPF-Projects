namespace IdSharp.Tagging.ID3v2.Frames.Lists
{
    using IdSharp.Tagging.ID3v2.Frames;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    internal sealed class UserDefinedUrlBindingList : BindingList<IWXXXFrame>
    {
        public UserDefinedUrlBindingList()
        {
            base.AllowNew = true;
        }

        public UserDefinedUrlBindingList(IList<IWXXXFrame> userDefineUrlList) : base(userDefineUrlList)
        {
            base.AllowNew = true;
        }

        protected override object AddNewCore()
        {
            IWXXXFrame frame1 = new WXXXFrame();
            base.Add(frame1);
            return frame1;
        }

        protected override void InsertItem(int index, IWXXXFrame item)
        {
            base.InsertItem(index, item);
        }

    }
}

