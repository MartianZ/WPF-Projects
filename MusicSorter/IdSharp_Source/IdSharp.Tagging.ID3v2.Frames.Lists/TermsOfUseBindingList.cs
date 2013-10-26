namespace IdSharp.Tagging.ID3v2.Frames.Lists
{
    using IdSharp.Tagging.ID3v2.Frames;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    internal sealed class TermsOfUseBindingList : BindingList<ITermsOfUse>
    {
        public TermsOfUseBindingList()
        {
            base.AllowNew = true;
        }

        public TermsOfUseBindingList(IList<ITermsOfUse> urlList) : base(urlList)
        {
            base.AllowNew = true;
        }

        protected override object AddNewCore()
        {
            ITermsOfUse use1 = new TermsOfUse();
            base.Add(use1);
            return use1;
        }

        protected override void InsertItem(int index, ITermsOfUse item)
        {
            base.InsertItem(index, item);
        }

    }
}

