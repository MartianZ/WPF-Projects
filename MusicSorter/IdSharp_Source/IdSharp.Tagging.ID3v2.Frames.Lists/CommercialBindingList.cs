namespace IdSharp.Tagging.ID3v2.Frames.Lists
{
    using IdSharp.Tagging.ID3v2.Frames;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    internal sealed class CommercialBindingList : BindingList<ICommercial>
    {
        public CommercialBindingList()
        {
            base.AllowNew = true;
        }

        public CommercialBindingList(IList<ICommercial> commercialInfoList) : base(commercialInfoList)
        {
            base.AllowNew = true;
        }

        protected override object AddNewCore()
        {
            ICommercial commercial1 = new Commercial();
            base.Add(commercial1);
            return commercial1;
        }

        protected override void InsertItem(int index, ICommercial item)
        {
            base.InsertItem(index, item);
        }

    }
}

