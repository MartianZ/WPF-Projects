namespace IdSharp.Utils
{
    using System;

    public class DataEventArgs<T> : EventArgs
    {
        public DataEventArgs(T data)
        {
            this.m_Data = data;
        }


        public T Data
        {
            get
            {
                return this.m_Data;
            }
        }


        private T m_Data;
    }
}

