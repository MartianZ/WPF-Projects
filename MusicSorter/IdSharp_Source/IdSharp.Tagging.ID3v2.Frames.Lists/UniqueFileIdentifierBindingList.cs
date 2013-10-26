namespace IdSharp.Tagging.ID3v2.Frames.Lists
{
    using IdSharp.Tagging.ID3v2.Frames;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    internal sealed class UniqueFileIdentifierBindingList : BindingList<IUniqueFileIdentifier>
    {
        public UniqueFileIdentifierBindingList()
        {
            base.AllowNew = true;
        }

        public UniqueFileIdentifierBindingList(IList<IUniqueFileIdentifier> uniqueFileIdentifierList) : base(uniqueFileIdentifierList)
        {
            base.AllowNew = true;
        }

        protected override object AddNewCore()
        {
            IUniqueFileIdentifier identifier1 = new UniqueFileIdentifier();
            base.Add(identifier1);
            return identifier1;
        }

        protected override void InsertItem(int index, IUniqueFileIdentifier item)
        {
            base.InsertItem(index, item);
        }

    }
}

