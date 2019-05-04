using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PacketViewerLogViewer.Packets;
using System.Diagnostics;
using System.IO;

namespace PacketViewerLogViewer
{
    public partial class ProjectInfoForm : Form
    {
        PacketTabPage tp ;
        int lastTagID = 0;

        public ProjectInfoForm()
        {
            InitializeComponent();
        }

        private void AddTag(string name)
        {
            if (name.Trim(' ') == string.Empty)
                return;
            lastTagID++;
            var L = new Label();
            L.Tag = lastTagID;
            L.BorderStyle = BorderStyle.Fixed3D;
            L.BackColor = System.Drawing.SystemColors.Highlight;
            L.ForeColor = System.Drawing.SystemColors.HighlightText;
            tagContainer.Controls.Add(L);
            L.Text = name;
            L.AutoSize = true;
            L.Cursor = Cursors.No;
            L.Click += new EventHandler(LTagLabel_Click);
        }

        private void CreateVisualTags(string tagString)
        {
            var tags = tagString.Split(',').ToList();
            foreach(string t in tags)
            {
                var s = t.Trim(' ');
                AddTag(s);
            }
        }

        private string VisualTagsToString()
        {
            string res = string.Empty;
            foreach(Control c in tagContainer.Controls)
            {
                if ( (c is Label) && (c.Tag != null) && ((int)c.Tag > 0) )
                {
                    if (res != string.Empty)
                        res += ",";
                    res += (c as Label).Text;
                }
            }
            return res;
        }

        public void LoadFromPacketTapPage(PacketTabPage sourceTP)
        {
            tp = sourceTP;

            if (tp != null)
            {
                CreateVisualTags(tp.ProjectTags);
                tTagBox.Text = "";
                tProjectFolder.Text = tp.ProjectFolder;
                tOpenedLog.Text = tp.LoadedLogFile;
                tSourceVideo.Text = tp.LinkVideoFileName;
                tYoutubeURL.Text = tp.LinkYoutubeURL;
                tPackedLogsURL.Text = tp.LinkPacketsDownloadURL;

                gbProjectInfo.Text = "Project Information: " + tp.ProjectFile;
            }
        }

        public void ApplyPacketTapPage()
        {
            if (tp != null)
            {
                tp.ProjectTags = VisualTagsToString();
                tp.ProjectFolder = tProjectFolder.Text;
                tp.LoadedLogFile = tOpenedLog.Text;
                tp.LinkVideoFileName = tSourceVideo.Text;
                tp.LinkYoutubeURL = tYoutubeURL.Text;
                tp.LinkPacketsDownloadURL = tPackedLogsURL.Text;
            }
        }

        private void ProjectInfoForm_Load(object sender, EventArgs e)
        {
            // Populate Autocomplete for tags
            tTagBox.AutoCompleteCustomSource.Clear();
            tTagBox.AutoCompleteCustomSource.AddRange(DataLookups.AllValues.ToArray());
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void BtnAddTag_Click(object sender, EventArgs e)
        {
            AddTag(tTagBox.Text);
            tTagBox.Text = "";
            tTagBox.Focus();
        }

        private void TTagBox_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Enter) || (e.KeyCode == Keys.Return))
            {
                BtnAddTag_Click(null,null);
            }
        }

        private void LTagLabel_Click(object sender, EventArgs e)
        {
            if ((sender is Label) && ((int)(sender as Label).Tag > 0))
            {
                tagContainer.Controls.Remove(sender as Label);
            }
        }

        private void ProjectInfoForm_Shown(object sender, EventArgs e)
        {
            tTagBox.Focus();
        }

        private void BtnDownloadYoutube_Click(object sender, EventArgs e)
        {
            if (tYoutubeURL.Text != string.Empty)
                Process.Start(tYoutubeURL.Text);
        }

        private void BtnDownloadSource_Click(object sender, EventArgs e)
        {
            if (tPackedLogsURL.Text != string.Empty)
                Process.Start(tPackedLogsURL.Text);
        }

        private void ProjectInfo_TextChanged(object sender, EventArgs e)
        {
            bool res = true;
            // Project Folder
            if (Directory.Exists(tProjectFolder.Text.TrimEnd(Path.DirectorySeparatorChar)))
            {
                lProjectFolderOK.Text = "\x81";
                lProjectFolderOK.ForeColor = Color.LimeGreen;
            }
            else
            {
                lProjectFolderOK.Text = "\xCE";
                lProjectFolderOK.ForeColor = Color.Red;
                res = false;
            }

            // Attached Log file
            if (File.Exists(tOpenedLog.Text))
            {
                lOpenedLogOK.Text = "\x81";
                lOpenedLogOK.ForeColor = Color.LimeGreen;
            }
            else
            {
                lOpenedLogOK.Text = "\xCE";
                lOpenedLogOK.ForeColor = Color.Red;
                res = false;
            }

            // Linked Local Video
            if ( (tSourceVideo.Text == string.Empty) || (File.Exists(tSourceVideo.Text)) )
            {
                lVideoSourceOK.Text = "\x81";
                lVideoSourceOK.ForeColor = Color.LimeGreen;
            }
            else
            {
                lVideoSourceOK.Text = "\xCE";
                lVideoSourceOK.ForeColor = Color.Red;
            }

            btnDownloadSource.Enabled = (tPackedLogsURL.Text != string.Empty) && ( (tPackedLogsURL.Text.ToLower().StartsWith("http://")) || (tPackedLogsURL.Text.ToLower().StartsWith("https://")) );
            btnDownloadYoutube.Enabled = (tYoutubeURL.Text != string.Empty) && ((tYoutubeURL.Text.ToLower().StartsWith("http://")) || (tYoutubeURL.Text.ToLower().StartsWith("https://"))); ;

            btnSave.Enabled = res;
        }
    }
}
