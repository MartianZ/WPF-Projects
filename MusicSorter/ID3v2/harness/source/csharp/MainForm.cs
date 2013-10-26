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
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using IdSharp.Tagging.ID3v1;
using IdSharp.Tagging.ID3v2;

namespace IdSharpHarness
{
    public partial class MainForm : Form
    {
        #region <<< Private Fields >>>

        private Boolean m_IsScanning;
        private Boolean m_CancelScanning;
        private String m_Filename;

        #endregion <<< Private Fields >>>

        #region <<< Constructor >>>

        public MainForm()
        {
            InitializeComponent();

            AssemblyName assemblyName = AssemblyName.GetAssemblyName("IdSharp.dll");
            lblVersion.Text = String.Format("IdSharp library version: {0}   ALPHA RELEASE   PLEASE TEST ON BACKUPS ONLY", assemblyName.Version);
        }

        #endregion <<< Constructor >>>

        #region <<< Event Handlers >>>

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnChooseDirectory_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                txtDirectory.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void btnScan_Click(object sender, EventArgs e)
        {
            if (m_IsScanning)
            {
                m_CancelScanning = true;
                return;
            }

            if (Directory.Exists(txtDirectory.Text))
                StartRecursiveScan(txtDirectory.Text);
            else
                MessageBox.Show("Directory does not exist");
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            if (audioOpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                LoadFile(audioOpenFileDialog.FileName);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveFile(m_Filename);
        }

        private void btnRemoveID3v2_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(m_Filename))
            {
                MessageBox.Show("No file loaded", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                DialogResult result = MessageBox.Show(String.Format("Remove ID3v2 tag from '{0}'?", Path.GetFileName(m_Filename)), "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    Boolean success = ID3v2Helper.RemoveTag(m_Filename);
                    if (success)
                        MessageBox.Show("ID3v2 tag successfully removed", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                        MessageBox.Show("ID3v2 tag not found", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            btnRemoveID3v2.Enabled = ID3v2Helper.DoesTagExist(m_Filename);
            ucID3v2.LoadFile(m_Filename);
        }

        private void btnRemoveID3v1_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(m_Filename))
            {
                MessageBox.Show("No file loaded", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                DialogResult result = MessageBox.Show(String.Format("Remove ID3v1 tag from '{0}'?", Path.GetFileName(m_Filename)), "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    Boolean success = ID3v1Helper.RemoveTag(m_Filename);
                    if (success)
                        MessageBox.Show("ID3v1 tag successfully removed", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                        MessageBox.Show("ID3v1 tag not found", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            btnRemoveID3v1.Enabled = ID3v1Helper.DoesTagExist(m_Filename);
            ucID3v1.LoadFile(m_Filename);
        }

        #endregion <<< Event Handlers >>>

        #region <<< Private Methods >>>

        private void StartRecursiveScan(String basePath)
        {
            m_IsScanning = true;
            m_CancelScanning = false;
            lblDirectory.Visible = false;
            txtDirectory.Visible = false;
            btnChooseDirectory.Visible = false;
            btnScan.Text = "&Cancel";
            btnScan.Enabled = false;
            prgScanFiles.Value = 0;
            prgScanFiles.Visible = true;
            ThreadPool.QueueUserWorkItem(new WaitCallback(this.ScanDirectory), basePath);
        }

        private void ScanDirectory(Object basePathObject)
        {
            Int32 totalFiles = 0;
            BindingList<Track> trackList = new BindingList<Track>();
            try
            {
                String basePath = (String)basePathObject;

                DirectoryInfo di = new DirectoryInfo(basePath);
                FileInfo[] fileList = di.GetFiles("*.mp3", SearchOption.AllDirectories);

                EnableCancelButton();

                totalFiles = fileList.Length;

                for (Int32 i = 0; i < totalFiles; i++)
                {
                    if (m_CancelScanning)
                    {
                        totalFiles = i;
                        break;
                    }

                    IID3v2 id3 = ID3v2Helper.CreateID3v2(fileList[i].FullName);

                    trackList.Add(new Track(id3.Artist, id3.Title, id3.Album, id3.Year, id3.Genre, fileList[i].Name));

                    if ((i % 100) == 0)
                    {
                        UpdateProgress(i * 100 / totalFiles);
                    }
                }
            }
            finally
            {
                EndRecursiveScanning(totalFiles, trackList);
            }
        }

        private void EnableCancelButton()
        {
            if (!this.InvokeRequired)
                this.btnScan.Enabled = true;
            else
                this.Invoke(new MethodInvoker(this.EnableCancelButton));
        }

        private delegate void UpdateProgressDelegate(Int32 progressValue);
        private void UpdateProgress(Int32 progressValue)
        {
            if (!this.InvokeRequired)
                this.prgScanFiles.Value = progressValue;
            else
                this.Invoke(new UpdateProgressDelegate(this.UpdateProgress), progressValue);
        }

        private delegate void EndRecursiveScanningDelegate(Int32 totalFiles, BindingList<Track> trackList);
        private void EndRecursiveScanning(Int32 totalFiles, BindingList<Track> trackList)
        {
            if (!this.InvokeRequired)
            {
                m_IsScanning = false;
                prgScanFiles.Visible = false;
                lblDirectory.Visible = true;
                txtDirectory.Visible = true;
                btnChooseDirectory.Visible = true;
                btnScan.Text = "&Scan";
                btnScan.Enabled = true;

                this.dataGridView1.DataSource = trackList;
            }
            else
            {
                this.Invoke(new EndRecursiveScanningDelegate(this.EndRecursiveScanning), totalFiles, trackList);
            }
        }

        private void LoadFile(String path)
        {
            m_Filename = path;

            ucID3v2.LoadFile(m_Filename);
            ucID3v1.LoadFile(m_Filename);

            btnSave.Enabled = true;
            btnRemoveID3v2.Enabled = ID3v2Helper.DoesTagExist(m_Filename);
            btnRemoveID3v1.Enabled = ID3v1Helper.DoesTagExist(m_Filename);
        }

        private void SaveFile(String path)
        {
            ucID3v2.SaveFile(path);
            ucID3v1.SaveFile(path);
            LoadFile(path);
        }

        #endregion <<< Private Methods >>>
    }
}
