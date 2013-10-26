namespace IdSharp.ComInterop.Utils
{
    using System;
    using System.Drawing;
    using System.IO;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    [ComVisible(true), Guid("8698B0BF-63DB-4314-9E5E-6E7B0A7D03CF"), ClassInterface(ClassInterfaceType.None)]
    public class PictureUtils : IPictureUtils
    {
        public object GetIPictureDispFromByteArray(byte[] image)
        {
            return typeof(AxHost).GetMethod("GetIPictureDispFromPicture", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { Image.FromStream(new MemoryStream(image)) });
        }

        public object GetIPictureDispFromImage(Image image)
        {
            return typeof(AxHost).GetMethod("GetIPictureDispFromPicture", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { image });
        }

    }
}

