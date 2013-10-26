namespace IdSharp.Tagging.ID3v2.Frames.Lists
{
    using IdSharp.Tagging.ID3v2.Frames.Items;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    internal sealed class MpegLookupTableItemBindingList : BindingList<IMpegLookupTableItem>
    {
        public MpegLookupTableItemBindingList()
        {
            base.AllowNew = true;
        }

        public MpegLookupTableItemBindingList(IList<IMpegLookupTableItem> mpegLookupTableItemList) : base(mpegLookupTableItemList)
        {
            base.AllowNew = true;
        }

        protected override object AddNewCore()
        {
            IMpegLookupTableItem item1 = new MpegLookupTableItem();
            base.Add(item1);
            return item1;
        }

        protected override void InsertItem(int index, IMpegLookupTableItem item)
        {
            base.InsertItem(index, item);
        }

    }
}

