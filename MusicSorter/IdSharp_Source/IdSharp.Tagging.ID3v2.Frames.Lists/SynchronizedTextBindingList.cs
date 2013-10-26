namespace IdSharp.Tagging.ID3v2.Frames.Lists
{
    using IdSharp.Tagging.ID3v2.Frames;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    internal sealed class SynchronizedTextBindingList : BindingList<ISynchronizedText>
    {
        public SynchronizedTextBindingList()
        {
            base.AllowNew = true;
        }

        public SynchronizedTextBindingList(IList<ISynchronizedText> synchronizedTextList) : base(synchronizedTextList)
        {
            base.AllowNew = true;
        }

        protected override object AddNewCore()
        {
            ISynchronizedText text1 = new SynchronizedText();
            base.Add(text1);
            return text1;
        }

        protected override void InsertItem(int index, ISynchronizedText item)
        {
            base.InsertItem(index, item);
        }

    }
}

