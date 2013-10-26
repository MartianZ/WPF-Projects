namespace IdSharp.Utils
{
    using System;

    public sealed class CancelDataEventArgs<T> : DataEventArgs<T>
    {
        public CancelDataEventArgs(T data) : base(data)
        {
            this.m_Cancel = false;
        }


        public bool Cancel
        {
            get
            {
                return this.m_Cancel;
            }
            set
            {
                this.m_Cancel = value;
            }
        }

        public string CancelReason
        {
            get
            {
                return this.m_CancelReason;
            }
            set
            {
                this.m_CancelReason = value;
            }
        }


        private bool m_Cancel;
        private string m_CancelReason;
    }
}

