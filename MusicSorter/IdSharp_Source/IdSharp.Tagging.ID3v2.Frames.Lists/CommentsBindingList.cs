namespace IdSharp.Tagging.ID3v2.Frames.Lists
{
    using IdSharp.Tagging.ID3v2.Frames;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    internal sealed class CommentsBindingList : BindingList<IComments>
    {
        public CommentsBindingList()
        {
            base.AllowNew = true;
        }

        public CommentsBindingList(IList<IComments> commentList) : base(commentList)
        {
            base.AllowNew = true;
        }

        protected override object AddNewCore()
        {
            IComments comments1 = new Comments();
            base.Add(comments1);
            return comments1;
        }

        protected override void InsertItem(int index, IComments item)
        {
            using (IEnumerator<IComments> enumerator1 = base.Items.GetEnumerator())
            {
                while (enumerator1.MoveNext())
                {
                    IComments local1 = enumerator1.Current;
                }
            }
            base.InsertItem(index, item);
        }

    }
}

