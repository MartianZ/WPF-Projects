namespace IdSharp.Tagging.ID3v2.Frames.Lists
{
    using IdSharp.Tagging.ID3v2.Frames.Items;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    internal sealed class EventTimingItemBindingList : BindingList<IEventTimingItem>
    {
        public EventTimingItemBindingList()
        {
            base.AllowNew = true;
        }

        public EventTimingItemBindingList(IList<IEventTimingItem> eventTimingItemList) : base(eventTimingItemList)
        {
            base.AllowNew = true;
        }

        protected override object AddNewCore()
        {
            IEventTimingItem item1 = new EventTimingItem();
            base.Add(item1);
            return item1;
        }

        protected override void InsertItem(int index, IEventTimingItem item)
        {
            base.InsertItem(index, item);
        }

    }
}

