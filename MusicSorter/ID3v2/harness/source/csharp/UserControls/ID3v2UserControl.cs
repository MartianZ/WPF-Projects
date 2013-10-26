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
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using IdSharp.Tagging.ID3v1;
using IdSharp.Tagging.ID3v2;
using IdSharp.Tagging.ID3v2.Frames;

namespace IdSharpHarness
{
    public partial class ID3v2UserControl : UserControl
    {
        #region <<< Private Fields >>>

        private IID3v2 m_ID3v2;

        #endregion <<< Private Fields >>>

        #region <<< Constructor >>>

        public ID3v2UserControl()
        {
            InitializeComponent();

            cmbGenre.Sorted = true;
            cmbGenre.Items.AddRange(GenreHelper.GenreByIndex);

            cmbImageType.Items.AddRange(PictureTypeHelper.PictureTypes);
        }

        #endregion <<< Constructor >>>

        #region <<< Event Handlers >>>

        private void bindingSource_CurrentChanged(object sender, EventArgs e)
        {
            IAttachedPicture attachedPicture = GetCurrentPictureFrame();
            if (attachedPicture != null)
                LoadImageData(attachedPicture);
            else
                ClearImageData();
        }

        private void imageContextMenu_Opening(object sender, CancelEventArgs e)
        {
            miSaveImage.Enabled = (this.pictureBox1.Image != null);
            miLoadImage.Enabled = (GetCurrentPictureFrame() != null);
        }

        private void miSaveImage_Click(object sender, EventArgs e)
        {
            IAttachedPicture attachedPicture = GetCurrentPictureFrame();
            SaveImageToFile(attachedPicture);
        }

        private void miLoadImage_Click(object sender, EventArgs e)
        {
            IAttachedPicture attachedPicture = GetCurrentPictureFrame();
            LoadImageFromFile(attachedPicture);
        }

        private void bindingNavigatorAddNewItem_Click(object sender, EventArgs e)
        {
            IAttachedPicture attachedPicture = GetCurrentPictureFrame();
            LoadImageFromFile(attachedPicture);
        }

        private void cmbImageType_SelectedIndexChanged(object sender, EventArgs e)
        {
            IAttachedPicture attachedPicture = GetCurrentPictureFrame();
            if (attachedPicture != null)
                attachedPicture.PictureType = PictureTypeHelper.GetPictureTypeFromString(cmbImageType.Text);
        }

        private void txtImageDescription_Validated(object sender, EventArgs e)
        {
            IAttachedPicture attachedPicture = GetCurrentPictureFrame();
            if (attachedPicture != null)
                attachedPicture.Description = txtImageDescription.Text;
        }

        #endregion <<< Event Handlers >>>

        #region <<< Private Methods >>>

        private void LoadImageData(IAttachedPicture attachedPicture)
        {
            pictureBox1.Image = attachedPicture.Picture;

            txtImageDescription.Text = attachedPicture.Description;
            cmbImageType.SelectedIndex = cmbImageType.Items.IndexOf(PictureTypeHelper.GetStringFromPictureType(attachedPicture.PictureType));

            txtImageDescription.Enabled = true;
            cmbImageType.Enabled = true;
        }

        private void ClearImageData()
        {
            pictureBox1.Image = null;
            txtImageDescription.Text = "";
            cmbImageType.SelectedIndex = -1;

            txtImageDescription.Enabled = false;
            cmbImageType.Enabled = false;
        }

        private void SaveImageToFile(IAttachedPicture attachedPicture)
        {
            String extension = attachedPicture.PictureExtension;

            imageSaveFileDialog.FileName = "image." + extension;

            DialogResult dialogResult = imageSaveFileDialog.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                using (FileStream fs = File.Open(imageSaveFileDialog.FileName, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    fs.Write(attachedPicture.PictureData, 0, attachedPicture.PictureData.Length);
                }
            }
        }

        private void LoadImageFromFile(IAttachedPicture attachedPicture)
        {
            DialogResult dialogResult = imageOpenFileDialog.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                attachedPicture.Picture = Image.FromFile(imageOpenFileDialog.FileName);
                pictureBox1.Image = attachedPicture.Picture;
            }
        }

        private IAttachedPicture GetCurrentPictureFrame()
        {
            if (imageBindingNavigator.BindingSource == null)
                return null;
            return imageBindingNavigator.BindingSource.Current as IAttachedPicture;
        }

        #endregion <<< Private Methods >>>

        #region <<< Public Methods >>>

        public void LoadFile(String path)
        {
            ClearImageData();

            m_ID3v2 = ID3v2Helper.CreateID3v2(path);

            txtFilename.Text = Path.GetFileName(path);
            txtArtist.Text = m_ID3v2.Artist;
            txtTitle.Text = m_ID3v2.Title;
            txtAlbum.Text = m_ID3v2.Album;
            cmbGenre.Text = m_ID3v2.Genre;
            txtYear.Text = m_ID3v2.Year;
            txtTrackNumber.Text = m_ID3v2.TrackNumber;

            pictureBox1.Image = m_ID3v2.PictureList[0].Picture;
            /*
            BindingSource bindingSource = new BindingSource();
            imageBindingNavigator.BindingSource = bindingSource;
            bindingSource.CurrentChanged += new EventHandler(bindingSource_CurrentChanged);
            bindingSource.DataSource = m_ID3v2.PictureList; */

            switch (m_ID3v2.Header.TagVersion)
            {
                case ID3v2TagVersion.ID3v22:
                    cmbID3v2.SelectedIndex = cmbID3v2.Items.IndexOf("ID3v2.2");
                    break;
                case ID3v2TagVersion.ID3v23:
                    cmbID3v2.SelectedIndex = cmbID3v2.Items.IndexOf("ID3v2.3");
                    break;
                case ID3v2TagVersion.ID3v24:
                    cmbID3v2.SelectedIndex = cmbID3v2.Items.IndexOf("ID3v2.4");
                    break;
            }
        }

        public void SaveFile(String path)
        {
            if (m_ID3v2 == null)
            {
                MessageBox.Show("Nothing to save!");
                return;
            }

            if (cmbID3v2.SelectedIndex == cmbID3v2.Items.IndexOf("ID3v2.2"))
                m_ID3v2.Header.TagVersion = ID3v2TagVersion.ID3v22;
            else if (cmbID3v2.SelectedIndex == cmbID3v2.Items.IndexOf("ID3v2.3"))
                m_ID3v2.Header.TagVersion = ID3v2TagVersion.ID3v23;
            else if (cmbID3v2.SelectedIndex == cmbID3v2.Items.IndexOf("ID3v2.4"))
                m_ID3v2.Header.TagVersion = ID3v2TagVersion.ID3v24;
            else
                throw new Exception("Unknown tag version");

            m_ID3v2.Artist = txtArtist.Text;
            m_ID3v2.Title = txtTitle.Text;
            m_ID3v2.Album = txtAlbum.Text;
            m_ID3v2.Genre = cmbGenre.Text;
            m_ID3v2.Year = txtYear.Text;
            m_ID3v2.TrackNumber = txtTrackNumber.Text;

            m_ID3v2.Save(path);
        }

        #endregion <<< Public Methods >>>

        private void bindingNavigatorDeleteItem_Click(object sender, EventArgs e)
        {
            Console.WriteLine(m_ID3v2.PictureList.Count);
        }
    }
}
