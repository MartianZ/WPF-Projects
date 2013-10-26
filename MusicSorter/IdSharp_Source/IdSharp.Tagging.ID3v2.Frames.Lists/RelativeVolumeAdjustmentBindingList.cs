namespace IdSharp.Tagging.ID3v2.Frames.Lists
{
    using IdSharp.Tagging.ID3v2.Frames;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    internal sealed class RelativeVolumeAdjustmentBindingList : BindingList<IRelativeVolumeAdjustment>
    {
        public RelativeVolumeAdjustmentBindingList()
        {
            base.AllowNew = true;
        }

        public RelativeVolumeAdjustmentBindingList(IList<IRelativeVolumeAdjustment> relativeVolumeAdjustmentList) : base(relativeVolumeAdjustmentList)
        {
            base.AllowNew = true;
        }

        protected override object AddNewCore()
        {
            IRelativeVolumeAdjustment adjustment1 = new RelativeVolumeAdjustment();
            base.Add(adjustment1);
            return adjustment1;
        }

        protected override void InsertItem(int index, IRelativeVolumeAdjustment item)
        {
            base.InsertItem(index, item);
        }

    }
}

