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
using System.IO;
using System.Windows.Forms;
using IdSharp.Tagging.ID3v1;

namespace IdSharpHarness
{
    public partial class ID3v1UserControl : UserControl
    {
        #region <<< Private Fields >>>

        private IID3v1 m_ID3v1;

        #endregion <<< Private Fields >>>

        #region <<< Constructor >>>

        public ID3v1UserControl()
        {
            InitializeComponent();

            cmbGenre.Sorted = true;
            cmbGenre.Items.AddRange(GenreHelper.GenreByIndex);
        }

        #endregion <<< Constructor >>>

        #region <<< Event Handlers >>>

        private void cmbID3v1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // ID3v1.0
            if (cmbID3v1.SelectedIndex == 0)
            {
                txtTrackNumber.Enabled = false;
                txtTrackNumber.Text = "";
                txtComment.MaxLength = 30;
            }
            // ID3v1.1
            else
            {
                txtTrackNumber.Enabled = true;
                txtComment.MaxLength = 28;
                // Resets to the appropriate length for the ID3v1 version.
                // Normally this would be handled by binding.  This will be updated in later versions of the harness.
                // TODO
                m_ID3v1.Comment = txtComment.Text;
                txtComment.Text = m_ID3v1.Comment;
            }
        }

        #endregion <<< Event Handlers >>>

        #region <<< Public Methods >>>

        public void LoadFile(String path)
        {
            m_ID3v1 = ID3v1Helper.CreateID3v1(path);

            txtFilename.Text = Path.GetFileName(path);
            txtArtist.Text = m_ID3v1.Artist;
            txtTitle.Text = m_ID3v1.Title;
            txtAlbum.Text = m_ID3v1.Album;
            cmbGenre.SelectedIndex = cmbGenre.Items.IndexOf(GenreHelper.GenreByIndex[m_ID3v1.GenreIndex]);
            txtYear.Text = m_ID3v1.Year;
            txtComment.Text = m_ID3v1.Comment;
            if (m_ID3v1.TrackNumber > 0)
                txtTrackNumber.Text = m_ID3v1.TrackNumber.ToString();
            else
                txtTrackNumber.Text = String.Empty;

            switch (m_ID3v1.TagVersion)
            {
                case ID3v1TagVersion.ID3v10:
                    cmbID3v1.SelectedIndex = cmbID3v1.Items.IndexOf("ID3v1.0");
                    break;
                case ID3v1TagVersion.ID3v11:
                    cmbID3v1.SelectedIndex = cmbID3v1.Items.IndexOf("ID3v1.1");
                    break;
            }
        }

        public void SaveFile(String path)
        {
            if (m_ID3v1 == null)
            {
                MessageBox.Show("Nothing to save!");
                return;
            }

            m_ID3v1.TagVersion = (cmbID3v1.SelectedIndex == 0 ? ID3v1TagVersion.ID3v10 : ID3v1TagVersion.ID3v11);
            m_ID3v1.Artist = txtArtist.Text;
            m_ID3v1.Title = txtTitle.Text;
            m_ID3v1.Album = txtAlbum.Text;
            m_ID3v1.GenreIndex = GenreHelper.GetGenreIndex(cmbGenre.Text);
            m_ID3v1.Year = txtYear.Text;
            m_ID3v1.Comment = txtComment.Text;
            Int32 trackNumber;
            Int32.TryParse(txtTrackNumber.Text, out trackNumber);
            m_ID3v1.TrackNumber = trackNumber;

            m_ID3v1.Save(path);
        }

        #endregion <<< Public Methods >>>
    }
}
