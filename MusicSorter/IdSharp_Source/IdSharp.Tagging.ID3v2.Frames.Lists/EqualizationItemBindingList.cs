namespace IdSharp.Tagging.ID3v2.Frames.Lists
{
    using IdSharp.Tagging.ID3v2.Frames.Items;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    internal sealed class EqualizationItemBindingList : BindingList<IEqualizationItem>
    {
        public EqualizationItemBindingList()
        {
            base.AllowNew = true;
        }

        public EqualizationItemBindingList(IList<IEqualizationItem> equalizationItemList) : base(equalizationItemList)
        {
            base.AllowNew = true;
        }

        protected override object AddNewCore()
        {
            IEqualizationItem item1 = new EqualizationItem();
            base.Add(item1);
            return item1;
        }

        protected override void InsertItem(int index, IEqualizationItem item)
        {
            base.InsertItem(index, item);
        }

    }
}

