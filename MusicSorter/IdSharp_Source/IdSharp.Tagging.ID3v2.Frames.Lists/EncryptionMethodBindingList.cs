namespace IdSharp.Tagging.ID3v2.Frames.Lists
{
    using IdSharp.Tagging.ID3v2.Frames;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    internal sealed class EncryptionMethodBindingList : BindingList<IEncryptionMethod>
    {
        public EncryptionMethodBindingList()
        {
            base.AllowNew = true;
        }

        public EncryptionMethodBindingList(IList<IEncryptionMethod> encryptionMethodList) : base(encryptionMethodList)
        {
            base.AllowNew = true;
        }

        protected override object AddNewCore()
        {
            IEncryptionMethod method1 = new EncryptionMethod();
            base.Add(method1);
            return method1;
        }

        protected override void InsertItem(int index, IEncryptionMethod item)
        {
            base.InsertItem(index, item);
        }

    }
}

