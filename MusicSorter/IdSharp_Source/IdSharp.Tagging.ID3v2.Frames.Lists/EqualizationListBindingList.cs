namespace IdSharp.Tagging.ID3v2.Frames.Lists
{
    using IdSharp.Tagging.ID3v2.Frames;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    internal sealed class EqualizationListBindingList : BindingList<IEqualizationList>
    {
        public EqualizationListBindingList()
        {
            base.AllowNew = true;
        }

        public EqualizationListBindingList(IList<IEqualizationList> equalizationListList) : base(equalizationListList)
        {
            base.AllowNew = true;
        }

        protected override object AddNewCore()
        {
            IEqualizationList list1 = new EqualizationList();
            base.Add(list1);
            return list1;
        }

        protected override void InsertItem(int index, IEqualizationList item)
        {
            base.InsertItem(index, item);
        }

    }
}

