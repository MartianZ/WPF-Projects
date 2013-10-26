namespace IdSharp.Tagging.ID3v2.Frames.Lists
{
    using IdSharp.Tagging.ID3v2.Frames.Items;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    internal sealed class InvolvedPersonBindingList : BindingList<IInvolvedPerson>
    {
        public InvolvedPersonBindingList()
        {
            base.AllowNew = true;
        }

        public InvolvedPersonBindingList(IList<IInvolvedPerson> involvedPersonList) : base(involvedPersonList)
        {
            base.AllowNew = true;
        }

        protected override object AddNewCore()
        {
            IInvolvedPerson person1 = new InvolvedPerson();
            base.Add(person1);
            return person1;
        }

        protected override void InsertItem(int index, IInvolvedPerson item)
        {
            base.InsertItem(index, item);
        }

    }
}

