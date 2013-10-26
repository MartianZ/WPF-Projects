namespace IdSharp.Tagging.ID3v2.Frames.Lists
{
    using IdSharp.Tagging.ID3v2.Frames.Items;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    internal sealed class MusicianCreditsItemBindingList : BindingList<IMusicianCreditsItem>
    {
        public MusicianCreditsItemBindingList()
        {
            base.AllowNew = true;
        }

        public MusicianCreditsItemBindingList(IList<IMusicianCreditsItem> musicianCreditsItemList) : base(musicianCreditsItemList)
        {
            base.AllowNew = true;
        }

        protected override object AddNewCore()
        {
            IMusicianCreditsItem item1 = new MusicianCreditsItem();
            base.Add(item1);
            return item1;
        }

        protected override void InsertItem(int index, IMusicianCreditsItem item)
        {
            base.InsertItem(index, item);
        }

    }
}

