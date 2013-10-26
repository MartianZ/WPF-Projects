/*
 *   IdSharp - A tagging library for .NET
 *   Copyright (C) 2007  Jud White
 *
 *   This program is free software; you can redistribute it and/or modify
 *   it under the terms of the GNU General Public License as published by
 *   the Free Software Foundation; either version 2 of the License, or
 *   (at your option) any later version.
 *
 *   This program is distributed in the hope that it will be useful,
 *   but WITHOUT ANY WARRANTY; without even the implied warranty of
 *   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *   GNU General Public License for more details.
 *
 *   You should have received a copy of the GNU General Public License along
 *   with this program; if not, write to the Free Software Foundation, Inc.,
 *   51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA.
 */
using System;

namespace IdSharpHarness
{
    internal class Track
    {
        #region <<< Private Fields >>>

        private String m_Filename;
        private String m_Artist;
        private String m_Title;
        private String m_Album;
        private String m_Year;
        private String m_Genre;

        #endregion <<< Private Fields >>>

        #region <<< Constructor >>>

        public Track(String artist, String title, String album, String year, String genre, String filename)
        {
            m_Artist = artist;
            m_Title = title;
            m_Album = album;
            m_Year = year;
            m_Genre = genre;
            m_Filename = filename;
        }

        #endregion <<< Constructor >>>

        #region <<< Public Properties >>>

        public String Filename
        {
            get { return m_Filename; }
            set { m_Filename = value; }
        }

        public String Artist
        {
            get { return m_Artist; }
            set { m_Artist = value; }
        }

        public String Title
        {
            get { return m_Title; }
            set { m_Title = value; }
        }

        public String Album
        {
            get { return m_Album; }
            set { m_Album = value; }
        }

        public String Year
        {
            get { return m_Year; }
            set { m_Year = value; }
        }

        public String Genre
        {
            get { return m_Genre; }
            set { m_Genre = value; }
        }

        #endregion <<< Public Properties >>>
    }
}
