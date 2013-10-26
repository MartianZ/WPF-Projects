namespace IdSharp.Utils
{
    using System;
    using System.Windows.Forms;

    public interface IErrorProvider
    {
        void ClearError(Control control);
        void SetError(Control control, string text, ErrorType errorType);
    }
}

