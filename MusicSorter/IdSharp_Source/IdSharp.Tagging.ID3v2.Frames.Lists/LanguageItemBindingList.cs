namespace IdSharp.Tagging.ID3v2.Frames.Lists
{
    using IdSharp.Tagging.ID3v2.Frames.Items;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    internal sealed class LanguageItemBindingList : BindingList<ILanguageItem>
    {
        public LanguageItemBindingList()
        {
            base.AllowNew = true;
        }

        public LanguageItemBindingList(IList<ILanguageItem> languageItemList) : base(languageItemList)
        {
            base.AllowNew = true;
        }

        protected override object AddNewCore()
        {
            ILanguageItem item1 = new LanguageItem();
            base.Add(item1);
            return item1;
        }

        protected override void InsertItem(int index, ILanguageItem item)
        {
            base.InsertItem(index, item);
        }

    }
}

