namespace IdSharp.Utils
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public interface IBindableControl
    {
        event EventHandler Validated;

        System.Windows.Forms.Control Control { get; }

        bool Enabled { get; set; }

        string Name { get; }

        object Value { get; set; }

    }
}

