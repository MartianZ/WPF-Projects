namespace IdSharp.Tagging.ID3v2.Frames.Lists
{
    using IdSharp.Tagging.ID3v2.Frames;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    internal sealed class AudioEncryptionBindingList : BindingList<IAudioEncryption>
    {
        public AudioEncryptionBindingList()
        {
            base.AllowNew = true;
        }

        public AudioEncryptionBindingList(IList<IAudioEncryption> audioEncryptionList) : base(audioEncryptionList)
        {
            base.AllowNew = true;
        }

        protected override object AddNewCore()
        {
            IAudioEncryption encryption1 = new AudioEncryption();
            base.Add(encryption1);
            return encryption1;
        }

        protected override void InsertItem(int index, IAudioEncryption item)
        {
            base.InsertItem(index, item);
        }

    }
}

