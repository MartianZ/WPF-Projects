namespace IdSharp.Utils
{
    using System;

    public sealed class InvalidDataEventArgs : EventArgs
    {
        public InvalidDataEventArgs(string propertyName, string message)
        {
            this.m_Property = propertyName;
            this.m_Message = message;
        }


        public string Message
        {
            get
            {
                return this.m_Message;
            }
        }

        public string Property
        {
            get
            {
                return this.m_Property;
            }
        }


        private string m_Message;
        private string m_Property;
    }
}

