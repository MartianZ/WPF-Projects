namespace IdSharp.Tagging.ID3v2.Frames.Lists
{
    using IdSharp.Tagging.ID3v2.Frames;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    internal sealed class PopularimeterBindingList : BindingList<IPopularimeter>
    {
        public PopularimeterBindingList()
        {
            base.AllowNew = true;
        }

        public PopularimeterBindingList(IList<IPopularimeter> popularimeterList) : base(popularimeterList)
        {
            base.AllowNew = true;
        }

        protected override object AddNewCore()
        {
            IPopularimeter popularimeter1 = new Popularimeter();
            base.Add(popularimeter1);
            return popularimeter1;
        }

        protected override void InsertItem(int index, IPopularimeter item)
        {
            base.InsertItem(index, item);
        }

    }
}

