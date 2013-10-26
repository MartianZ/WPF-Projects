namespace IdSharp.Tagging.ID3v2.Frames.Lists
{
    using IdSharp.Tagging.ID3v2.Frames.Items;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    internal sealed class SynchronizedTextItemBindingList : BindingList<ISynchronizedTextItem>
    {
        public SynchronizedTextItemBindingList()
        {
            base.AllowNew = true;
        }

        public SynchronizedTextItemBindingList(IList<ISynchronizedTextItem> synchronizedTextItemList) : base(synchronizedTextItemList)
        {
            base.AllowNew = true;
        }

        protected override object AddNewCore()
        {
            ISynchronizedTextItem item1 = new SynchronizedTextItem();
            base.Add(item1);
            return item1;
        }

        protected override void InsertItem(int index, ISynchronizedTextItem item)
        {
            base.InsertItem(index, item);
        }

    }
}

