namespace zlib
{
    using System;
    using System.IO;

    [Serializable]
    internal sealed class ZStreamException : IOException
    {
        public ZStreamException()
        {
        }

        public ZStreamException(string s) : base(s)
        {
        }

    }
}

