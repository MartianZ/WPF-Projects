namespace IdSharp.Tagging.ID3v2.Frames.Lists
{
    using IdSharp.Tagging.ID3v2.Frames;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    internal sealed class AudioTextBindingList : BindingList<IAudioText>
    {
        public AudioTextBindingList()
        {
            base.AllowNew = true;
        }

        public AudioTextBindingList(IList<IAudioText> audioTextList) : base(audioTextList)
        {
            base.AllowNew = true;
        }

        protected override object AddNewCore()
        {
            IAudioText text1 = new AudioText();
            base.Add(text1);
            return text1;
        }

        protected override void InsertItem(int index, IAudioText item)
        {
            base.InsertItem(index, item);
        }

    }
}

