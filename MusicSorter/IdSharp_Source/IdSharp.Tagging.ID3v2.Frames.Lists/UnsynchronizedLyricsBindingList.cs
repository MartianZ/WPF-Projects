namespace IdSharp.Tagging.ID3v2.Frames.Lists
{
    using IdSharp.Tagging.ID3v2.Frames;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    internal sealed class UnsynchronizedLyricsBindingList : BindingList<IUnsynchronizedText>
    {
        public UnsynchronizedLyricsBindingList()
        {
            base.AllowNew = true;
        }

        public UnsynchronizedLyricsBindingList(IList<IUnsynchronizedText> urlList) : base(urlList)
        {
            base.AllowNew = true;
        }

        protected override object AddNewCore()
        {
            IUnsynchronizedText text1 = new UnsynchronizedText();
            base.Add(text1);
            return text1;
        }

        protected override void InsertItem(int index, IUnsynchronizedText item)
        {
            base.InsertItem(index, item);
        }

    }
}

