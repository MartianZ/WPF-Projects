namespace IdSharp.Tagging.ID3v2.Frames.Lists
{
    using IdSharp.Tagging.ID3v2.Frames;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    internal sealed class SignatureBindingList : BindingList<ISignature>
    {
        public SignatureBindingList()
        {
            base.AllowNew = true;
        }

        public SignatureBindingList(IList<ISignature> signatureList) : base(signatureList)
        {
            base.AllowNew = true;
        }

        protected override object AddNewCore()
        {
            ISignature signature1 = new IdSharp.Tagging.ID3v2.Frames.Signature();
            base.Add(signature1);
            return signature1;
        }

        protected override void InsertItem(int index, ISignature item)
        {
            base.InsertItem(index, item);
        }

    }
}

