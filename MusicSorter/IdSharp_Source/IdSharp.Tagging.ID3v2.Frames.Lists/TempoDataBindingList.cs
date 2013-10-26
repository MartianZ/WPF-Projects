namespace IdSharp.Tagging.ID3v2.Frames.Lists
{
    using IdSharp.Tagging.ID3v2.Frames.Items;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    internal sealed class TempoDataBindingList : BindingList<ITempoData>
    {
        public TempoDataBindingList()
        {
            base.AllowNew = true;
        }

        public TempoDataBindingList(IList<ITempoData> tempoDataList) : base(tempoDataList)
        {
            base.AllowNew = true;
        }

        protected override object AddNewCore()
        {
            ITempoData data1 = new TempoData();
            base.Add(data1);
            return data1;
        }

        protected override void InsertItem(int index, ITempoData item)
        {
            base.InsertItem(index, item);
        }

    }
}

