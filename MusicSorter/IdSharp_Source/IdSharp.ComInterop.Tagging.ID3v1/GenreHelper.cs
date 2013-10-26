namespace IdSharp.ComInterop.Tagging.ID3v1
{
    using IdSharp.Tagging.ID3v1;
    using System;
    using System.Runtime.InteropServices;

    [ClassInterface(ClassInterfaceType.None), ComVisible(true), Guid("E4868CA7-8A67-4f1c-81AD-C685806866A0")]
    public class GenreHelper : IGenreHelper
    {
        public int GetGenreIndex(string genre)
        {
            return IdSharp.Tagging.ID3v1.GenreHelper.GetGenreIndex(genre);
        }


        public string[] GenreByIndex
        {
            get
            {
                return IdSharp.Tagging.ID3v1.GenreHelper.GenreByIndex;
            }
        }

    }
}

