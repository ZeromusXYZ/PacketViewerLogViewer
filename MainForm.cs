using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using PacketViewerLogViewer.Packets;
using System.IO;
using PacketViewerLogViewer.ClipboardHelper;
using PacketViewerLogViewer.PVLVHelper;

namespace PacketViewerLogViewer
{

    public partial class MainForm : Form
    {
        public static MainForm thisMainForm;

        string defaultTitle = "";
        const string urlGitHub = "https://github.com/ZeromusXYZ/PVLV";
        const string urlVideoLAN = "https://www.videolan.org/";

        //PacketList PLLoaded; // File Loaded
        //PacketList PL; // Filtered File Data Displayed
        public PacketParser PP;
        // private UInt16 CurrentSync;
        SearchParameters searchParameters;

        const string InfoGridHeader = "     |  0  1  2  3   4  5  6  7   8  9  A  B   C  D  E  F    | 0123456789ABCDEF\n" +
                "-----+----------------------------------------------------  -+------------------\n";

        public MainForm()
        {
            InitializeComponent();
            thisMainForm = this;
            searchParameters = new SearchParameters();
            searchParameters.Clear();
        }

        private void mmFileExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void mmAboutGithub_Click(object sender, EventArgs e)
        {
            Process.Start(urlGitHub);
        }

        private void mmAboutVideoLAN_Click(object sender, EventArgs e)
        {
            Process.Start(urlVideoLAN);
        }

        private void mmAboutAbout_Click(object sender, EventArgs e)
        {
            using (AboutBoxForm ab = new AboutBoxForm())
            {
                ab.ShowDialog();
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            defaultTitle = Text;
            Application.UseWaitCursor = true;
            try
            {
                DataLookups.LoadLookups();
            }
            catch (Exception x)
            {
                MessageBox.Show("Exception: " + x.Message, "Loading Lookup Data", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                Close();
            }
            tcPackets.TabPages.Clear();
            Application.UseWaitCursor = false;
        }



        private void mmFileOpen_Click(object sender, EventArgs e)
        {
            openLogFileDialog.Title = "Open log file";
            if (openLogFileDialog.ShowDialog() != DialogResult.OK)
                return;
            TryOpenLogFile(openLogFileDialog.FileName);
        }

        private void TryOpenLogFile(string logFile)
        {
            PacketTabPage tp = CreateNewPacketsTabPage();
            tp.LoadProjectFile(logFile);
            //tp.ProjectFolder = Helper.MakeProjectDirectoryFromLogFileName(logFile);
            tp.Text = Helper.MakeTabName(logFile);

            tp.PLLoaded.Clear();
            tp.PLLoaded.Filter.Clear();
            if (!tp.PLLoaded.LoadFromFile(logFile))
            {
                MessageBox.Show("Error loading file: " + logFile, "File Open Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tp.PLLoaded.Clear();
                return;
            }
            Text = defaultTitle + " - " + logFile;
            tp.LoadedLogFile = logFile;
            tp.PL.CopyFrom(tp.PLLoaded);
            tp.FillListBox();
            UpdateStatusBarAndTitle(tp);
            if ( Properties.Settings.Default.AutoOpenVideoForm && (File.Exists(Path.ChangeExtension(tp.LoadedLogFile,".pvlvvl"))) )
            {
                MmVideoOpenLink_Click(null, null);
            }
        }

        public void lbPackets_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListBox lb = (sender as ListBox);
            if (!(lb.Parent is PacketTabPage))
                return;
            PacketTabPage tp = (lb.Parent as PacketTabPage);
            if ((lb.SelectedIndex < 0) || (lb.SelectedIndex >= tp.PL.Count()))
            {
                rtInfo.SelectionColor = rtInfo.ForeColor;
                rtInfo.SelectionBackColor = rtInfo.BackColor;
                rtInfo.Text = "Please select a valid item from the list";
                return;
            }
            PacketData pd = tp.PL.GetPacket(lb.SelectedIndex);
            cbShowBlock.Enabled = false;
            UpdatePacketDetails(tp,pd, "-");
            cbShowBlock.Enabled = true;
            lb.Invalidate();
            if ((tp.videoLink != null) && (tp.videoLink.cbFollowPacketList.Checked))
            {
                tp.videoLink.MoveToDateTime(pd.VirtualTimeStamp);
            }
        }


        private void cbOriginalData_CheckedChanged(object sender, EventArgs e)
        {
            PacketTabPage tp = GetCurrentPacketTabPage();
            if (tp == null)
            {
                rtInfo.SelectionColor = rtInfo.ForeColor;
                rtInfo.SelectionBackColor = rtInfo.BackColor;
                rtInfo.Text = "Please select open a list first";
                return;
            }

            PacketData pd = tp.GetSelectedPacket();
            if (pd == null)
            {
                rtInfo.SelectionColor = rtInfo.ForeColor;
                rtInfo.SelectionBackColor = rtInfo.BackColor;
                rtInfo.Text = "Please select a valid item from the list";
                return;
            }

            UpdatePacketDetails(tp,pd, "-");
        }

        private void mmFileClose_Click(object sender, EventArgs e)
        {
            if ((tcPackets.SelectedIndex >= 0) && (tcPackets.SelectedIndex < tcPackets.TabCount))
            {
                tcPackets.TabPages.RemoveAt(tcPackets.SelectedIndex);
            }
            /*
            PLLoaded.Clear();
            PLLoaded.ClearFilters();
            PL.Clear();
            PL.ClearFilters();
            FillListBox(lbPackets,PL);
            */
        }

        private void mmFileAppend_Click(object sender, EventArgs e)
        {
            openLogFileDialog.Title = "Append log file";
            if (openLogFileDialog.ShowDialog() != DialogResult.OK)
                return;

            PacketTabPage tp = GetCurrentOrNewPacketTabPage();
            tp.Text = "Multi";
            tp.LoadedLogFile = "?Multiple Sources";
            tp.ProjectFolder = string.Empty;

            if (!tp.PLLoaded.LoadFromFile(openLogFileDialog.FileName))
            {
                MessageBox.Show("Error loading file: " + openLogFileDialog.FileName, "File Append Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tp.PLLoaded.Clear();
                return;
            }
            Text = defaultTitle + " - " + tp.LoadedLogFile;
            tp.PL.CopyFrom(tp.PLLoaded);
            tp.FillListBox();
            UpdateStatusBarAndTitle(tp);
        }

        private void RawDataToRichText(PacketParser pp, RichTextBox rt)
        {
            void SetColorBasic(byte n)
            {
                rtInfo.SelectionFont = rtInfo.Font;
                rtInfo.SelectionColor = Color.Black;
                rtInfo.SelectionBackColor = Color.White;
            }

            void SetColorGrid()
            {
                rtInfo.SelectionFont = rtInfo.Font;
                rtInfo.SelectionColor = Color.DarkGray;
                rtInfo.SelectionBackColor = Color.White;
            }

            void SetColorSelect(byte n,bool forchars)
            {
                if (!forchars)
                {
                    rtInfo.SelectionFont = new Font(rtInfo.Font, FontStyle.Italic);
                }
                else
                {
                    rtInfo.SelectionFont = rtInfo.Font;
                }
                rtInfo.SelectionColor = Color.Yellow;
                rtInfo.SelectionBackColor = Color.DarkBlue;
            }

            void SetColorNotSelect(byte n, bool forchars)
            {
                rtInfo.SelectionFont = rtInfo.Font;
                if ((pp.SelectedFields.Count > 0) || forchars)
                {
                    rtInfo.SelectionColor = pp.GetDataColor(n);
                    rtInfo.SelectionBackColor = Color.White;
                }
                else
                {
                    rtInfo.SelectionColor = Color.White;
                    rtInfo.SelectionBackColor = pp.GetDataColor(n);
                }
            }


            void AddChars(int startIndex)
            {
                SetColorGrid();
                rtInfo.AppendText("  | ");
                for (int c = 0; (c < 0x10) && ((startIndex + c) < pp.ParsedBytes.Count); c++)
                {
                    var n = pp.ParsedBytes[startIndex + c];
                    if (pp.SelectedFields.IndexOf(n) >= 0)
                    {
                        SetColorSelect(n,true);
                    }
                    else
                    {
                        SetColorNotSelect(n,true);
                    }
                    char ch = (char)pp.PD.GetByteAtPos(startIndex + c);
                    if ((ch < 32) || (ch >= 128))
                        ch = '.';
                    rtInfo.AppendText(ch.ToString());
                }
            }

            rtInfo.Enabled = false;
            rtInfo.Clear();
            SetColorGrid();
            rtInfo.AppendText(InfoGridHeader);
            int addCharCount = 0;
            byte lastFieldIndex = 0;
            for (int i = 0; i < pp.PD.RawBytes.Count; i += 0x10)
            {
                SetColorGrid();
                rtInfo.AppendText(i.ToString("X").PadLeft(4,' ') + " | ");
                for (int i2 = 0; i2 < 0x10; i2++)
                {
                    if ((i + i2) < pp.ParsedBytes.Count)
                    {
                        var n = pp.ParsedBytes[i+i2];
                        lastFieldIndex = n;
                        if (pp.SelectedFields.Count > 0)
                        {
                            if (pp.SelectedFields.IndexOf(n) >= 0)
                            {
                                // Is selected field
                                SetColorSelect(n, false);
                            }
                            else
                            {
                                // we have non-selected field
                                SetColorNotSelect(n, false);
                            }
                        }
                        else
                        {
                            // No fields selected
                            SetColorNotSelect(n, false);
                        }
                        rtInfo.AppendText(pp.PD.GetByteAtPos(i+i2).ToString("X2"));
                        addCharCount++;
                    }
                    else
                    {
                        SetColorGrid();
                        rtInfo.AppendText("  ");
                    }

                    if ((i + i2 + 1) < pp.ParsedBytes.Count)
                    {
                        var n = pp.ParsedBytes[i + i2 + 1];
                        if (n != lastFieldIndex)
                        {
                            SetColorBasic(n);
                        }
                    }
                    else
                    {
                        SetColorGrid();
                    }

                    rtInfo.AppendText(" ");
                    if ((i2 % 0x4) == 0x3)
                        rtInfo.AppendText(" ");
                }
                if (addCharCount > 0)
                {
                    AddChars(i);
                    addCharCount = 0;
                }
                rtInfo.AppendText("\r\n");
            }
            rtInfo.ReadOnly = true;
            rtInfo.Enabled = true;
        }

        public void UpdatePacketDetails(PacketTabPage tp, PacketData pd, string SwitchBlockName, bool dontReloadParser = false)
        {
            if ((tp == null) || (pd == null))
                return;
            tp.CurrentSync = pd.PacketSync;
            lInfo.Text = pd.OriginalHeaderText;
            rtInfo.Clear();

            if (dontReloadParser == false)
            {
                PP = new PacketParser(pd.PacketID, pd.PacketLogType);
            }
            if (PP == null)
                return;
            PP.AssignPacket(pd);
            PP.ParseToDataGridView(dGV,SwitchBlockName);
            cbShowBlock.Enabled = false;
            if (PP.SwitchBlocks.Count > 0)
            {
                cbShowBlock.Items.Clear();
                cbShowBlock.Items.Add("-");
                cbShowBlock.Items.AddRange(PP.SwitchBlocks.ToArray());
                cbShowBlock.Show();
            }
            else
            {
                cbShowBlock.Items.Clear();
                cbShowBlock.Hide();
            }
            for(int i = 0; i < cbShowBlock.Items.Count;i++)
            {
                if ((SwitchBlockName == "-") && (cbShowBlock.Items[i].ToString() == PP.LastSwitchedBlock))
                {
                    if (cbShowBlock.SelectedIndex != i)
                        cbShowBlock.SelectedIndex = i;
                    //break;
                }
                else
                if (cbShowBlock.Items[i].ToString() == SwitchBlockName)
                {
                    if (cbShowBlock.SelectedIndex != i)
                        cbShowBlock.SelectedIndex = i;
                    //break;
                }
            }
            cbShowBlock.Enabled = true;

            if (cbOriginalData.Checked)
            {
                rtInfo.SelectionColor = rtInfo.ForeColor;
                rtInfo.SelectionBackColor = rtInfo.BackColor;
                rtInfo.Text = "Source:\r\n" + string.Join("\r\n", pd.RawText.ToArray());
            }
            else
            {
                RawDataToRichText(PP, rtInfo);
            }

        }

        private void mmFileSettings_Click(object sender, EventArgs e)
        {
            using (SettingsForm settingsDialog = new SettingsForm())
            {
                if (settingsDialog.ShowDialog() == DialogResult.OK)
                {
                    Properties.Settings.Default.Save();
                    //MessageBox.Show("Settings saved");
                }
                settingsDialog.Dispose();
            }
        }

        private void CbShowBlock_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!cbShowBlock.Enabled)
                return;

            if (!(tcPackets.SelectedTab is PacketTabPage))
                return;
            PacketTabPage tp = (tcPackets.SelectedTab as PacketTabPage);

            cbShowBlock.Enabled = false;
            if ((tp.lbPackets.SelectedIndex < 0) || (tp.lbPackets.SelectedIndex >= tp.PL.Count()))
            {
                rtInfo.SelectionColor = rtInfo.ForeColor;
                rtInfo.SelectionBackColor = rtInfo.BackColor;
                rtInfo.Text = "Please select a valid item from the list";
                return;
            }
            PacketData pd = tp.PL.GetPacket(tp.lbPackets.SelectedIndex);
            var sw = cbShowBlock.SelectedIndex;
            if (sw >= 0)
            {
                UpdatePacketDetails(tp,pd, cbShowBlock.Items[sw].ToString(),true);
            }
            else
            {
                UpdatePacketDetails(tp,pd, "-",true);
            }
            cbShowBlock.Enabled = true;
            tp.lbPackets.Invalidate();
        }

        private void dGV_SelectionChanged(object sender, EventArgs e)
        {
            if ((PP == null) || (PP.PD == null))
                return;
            if (dGV.Tag != null)
                return;
            PP.SelectedFields.Clear();
            for (int i = 0; i < dGV.RowCount;i++)
            {
                if ((dGV.Rows[i].Selected) && (i < PP.ParsedView.Count))
                {
                    var f = PP.ParsedView[i].FieldIndex;
                    //if (f != 0xFF)
                        PP.SelectedFields.Add(f);
                }
            }
            PP.ToGridView(dGV);
            RawDataToRichText(PP, rtInfo);
        }


        public void UpdateStatusBarAndTitle(PacketTabPage tp)
        {
            if (tp == null)
            {
                sbProjectInfo.Text = "Not a project";
                sbExtraInfo.Text = "";
                return;
            }

            var t = tp.LoadedLogFile;
            if (t.StartsWith("?"))
                t = t.TrimStart('?');
            Text = defaultTitle + " - " + t;
            if (tp.ProjectFolder != string.Empty)
                sbProjectInfo.Text = "Project Folder: " + tp.ProjectFolder;
            else
                sbProjectInfo.Text = "Not a project";

            if (File.Exists(tp.LinkVideoFileName))
                sbExtraInfo.Text = "Local Video Linked";
            else
            if (tp.LinkYoutubeURL != string.Empty)
                sbExtraInfo.Text = "Youtube Linked";
            else
                sbExtraInfo.Text = "";
        }

        private void TcPackets_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabControl tc = (sender as TabControl);
            if (!(tc.SelectedTab is PacketTabPage))
            {
                UpdateStatusBarAndTitle(null);
                return;
            }
            PacketTabPage tp = (tc.SelectedTab as PacketTabPage);
            UpdateStatusBarAndTitle(tp);
            PacketData pd = tp.PL.GetPacket(tp.lbPackets.SelectedIndex);
            cbShowBlock.Enabled = false;
            UpdatePacketDetails(tp, pd, "-");
            cbShowBlock.Enabled = true;
        }

        private void MmAddFromClipboard_Click(object sender, EventArgs e)
        {
            if ((!Clipboard.ContainsText()) || (Clipboard.GetText() == string.Empty))
            {
                MessageBox.Show("Nothing to paste", "Paste from Clipboard", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            try
            {
                PacketTabPage tp = GetCurrentOrNewPacketTabPage();
                var cText = Clipboard.GetText().Replace("\r", "");
                List<string> clipText = new List<string>();
                clipText.AddRange(cText.Split((char)10).ToList());

                tp.Text = "Clipboard";
                tp.LoadedLogFile = "?Paste from Clipboard";
                tp.ProjectFolder = string.Empty;

                if (!tp.PLLoaded.LoadFromStringList(clipText, PacketLogFileFormats.Unknown, PacketLogTypes.Unknown))
                {
                    MessageBox.Show("Error loading data from clipboard", "Clipboard Paste Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    tp.PLLoaded.Clear();
                    return;
                }
                Text = defaultTitle + " - " + tp.LoadedLogFile;
                tp.PL.CopyFrom(tp.PLLoaded);
                tp.FillListBox();
                UpdateStatusBarAndTitle(tp);
            }
            catch (Exception x)
            {
                MessageBox.Show("Paste Failed, Exception: "+x.Message, "Paste from Clipboard", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private PacketTabPage CreateNewPacketsTabPage()
        {
            PacketTabPage tp = new PacketTabPage(this);
            tp.lbPackets.SelectedIndexChanged += lbPackets_SelectedIndexChanged;
            tcPackets.TabPages.Add(tp);
            tcPackets.SelectedTab = tp;
            tp.lbPackets.Focus();
            return tp;
        }

        private PacketTabPage GetCurrentOrNewPacketTabPage()
        {
            PacketTabPage tp = GetCurrentPacketTabPage();
            if (tp == null)
            {
                tp = CreateNewPacketsTabPage();
            }
            return tp;
        }

        public PacketTabPage GetCurrentPacketTabPage()
        {
            if (!(tcPackets.SelectedTab is PacketTabPage))
            {
                return null;
            }
            else
            {
                return (tcPackets.SelectedTab as PacketTabPage);
            }
        }

        private void MmFilterEdit_Click(object sender, EventArgs e)
        {
            var tp = GetCurrentPacketTabPage();
            using (var filterDlg = new FilterForm())
            {
                filterDlg.btnOK.Enabled = (tp != null);
                if (tp != null)
                {
                    filterDlg.Filter.CopyFrom(tp.PL.Filter);
                    filterDlg.LoadLocalFromFilter();
                }
                if (filterDlg.ShowDialog(this) == DialogResult.OK)
                {
                    filterDlg.SaveLocalToFilter();
                    UInt16 lastSync = tp.CurrentSync;
                    tp.PL.Filter.CopyFrom(filterDlg.Filter);
                    tp.PL.FilterFrom(tp.PLLoaded);
                    tp.FillListBox(lastSync);
                    tp.CenterListBox();

                }
            }
        }

        private void MmFilterReset_Click(object sender, EventArgs e)
        {
            var tp = GetCurrentPacketTabPage();
            if (tp != null)
            {
                UInt16 lastSync = tp.CurrentSync;
                tp.PL.Filter.Clear();
                tp.PL.CopyFrom(tp.PLLoaded);
                tp.FillListBox(lastSync);
                tp.CenterListBox();
            }

        }

        private void MmFilterApply_Click(object sender, EventArgs e)
        {
        }

        private void MMFilterApplyItem_Click(object sender, EventArgs e)
        {
            var tp = GetCurrentPacketTabPage();
            if (tp == null)
                return;

            if (sender is ToolStripMenuItem)
            {
                var mITem = (sender as ToolStripMenuItem);
                // apply filter
                UInt16 lastSync = tp.CurrentSync;
                tp.PL.Filter.LoadFromFile(Application.StartupPath + Path.DirectorySeparatorChar + "filter" + Path.DirectorySeparatorChar + mITem.Text + ".pfl");
                tp.PL.FilterFrom(tp.PLLoaded);
                tp.FillListBox(lastSync);
                tp.CenterListBox();
            }
        }

        private void MmFilterApply_DropDownOpening(object sender, EventArgs e)
        {
            // generate menu
            // GetFiles
            try
            {
                mmFilterApply.DropDownItems.Clear();
                var di = new DirectoryInfo(Application.StartupPath + Path.DirectorySeparatorChar + "filter");
                var files = di.GetFiles("*.pfl");
                foreach (var fi in files)
                {
                    ToolStripMenuItem mi = new ToolStripMenuItem(Path.GetFileNameWithoutExtension(fi.Name));
                    mi.Click += MMFilterApplyItem_Click;
                    mmFilterApply.DropDownItems.Add(mi);
                }
                if (files.Length <= 0)
                {
                    ToolStripMenuItem mi = new ToolStripMenuItem("no filters found");
                    mi.Enabled = false;
                    mmFilterApply.DropDownItems.Add(mi);
                }
            }
            catch
            {
                // Do nothing
            }
        }

        private void MmSearchSearch_Click(object sender, EventArgs e)
        {
            var tp = GetCurrentPacketTabPage();
            if (tp == null)
                return;
            using (SearchForm SearchDlg = new SearchForm())
            {
                SearchDlg.searchParameters.CopyFrom(this.searchParameters);
                var res = SearchDlg.ShowDialog();
                if ((res == DialogResult.OK) || (res == DialogResult.Retry))
                {
                    searchParameters.CopyFrom(SearchDlg.searchParameters);
                    if (res == DialogResult.OK)
                        FindNext();
                    else
                    if (res == DialogResult.Retry)
                        FindAsNewTab();
                }
            }
        }

        private void MmSearchNext_Click(object sender, EventArgs e)
        {
            var tp = GetCurrentPacketTabPage();
            if (tp == null)
                return;
            if ((searchParameters.SearchIncoming == false) && (searchParameters.SearchOutgoing == false))
            {
                MmSearchSearch_Click(null, null);
                return;
            }
            else
                FindNext();
        }

        private void FindNext()
        {
            var tp = GetCurrentPacketTabPage();

            if ((tp == null) || (tp.lbPackets.Items.Count <= 0))
            {
                MessageBox.Show("Nothing to search in !", "Search", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var startIndex = tp.lbPackets.SelectedIndex;
            if ((startIndex < 0) && (startIndex >= tp.lbPackets.Items.Count))
                startIndex = -1;
            int i = startIndex + 1 ;
            for(int c = 0;c < tp.lbPackets.Items.Count-1;c++)
            {
                var pd = tp.PL.GetPacket(i);
                if (pd.MatchesSearch(searchParameters))
                {
                    // Select index
                    tp.lbPackets.SelectedIndex = i;
                    // Move to center
                    var iHeight = tp.lbPackets.ItemHeight;
                    if (iHeight <= 0)
                        iHeight = 8;
                    var iCount = tp.lbPackets.Size.Height / iHeight;
                    var tPos = i - (iCount / 2);
                    if (tPos < 0)
                        tPos = 0;
                    tp.lbPackets.TopIndex = tPos;
                    tp.lbPackets.Focus();
                    // We're done
                    return;
                }
                i++;
                if (i >= tp.lbPackets.Items.Count)
                    i = 0;
            }
            MessageBox.Show("No matches found !", "Search", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void FindAsNewTab()
        {
            var tp = GetCurrentPacketTabPage();

            if ((tp == null) || (tp.lbPackets.Items.Count <= 0))
            {
                MessageBox.Show("Nothing to search in !", "Search as New Tab", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            PacketTabPage newtp = CreateNewPacketsTabPage();
            newtp.Text = "*" + tp.Text;
            newtp.LoadedLogFile = "Search Result";

            var count = newtp.PLLoaded.SearchFrom(tp.PL, searchParameters);

            if (count <= 0)
            {
                MessageBox.Show("No matches found !", "Search as New Tab", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                newtp.PL.CopyFrom(newtp.PLLoaded);
                newtp.FillListBox();
            }
            UpdateStatusBarAndTitle(newtp);
        }

        private void MmFilePasteNew_Click(object sender, EventArgs e)
        {

            if ((!Clipboard.ContainsText()) || (Clipboard.GetText() == string.Empty))
            {
                MessageBox.Show("Nothing to paste", "Paste from Clipboard", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            try
            {
                PacketTabPage tp = CreateNewPacketsTabPage();
                tp.Text = "Clipboard";
                tp.LoadedLogFile = "?Paste from Clipboard";
                tp.ProjectFolder = string.Empty;
                tcPackets.SelectedTab = tp;

                var cText = Clipboard.GetText().Replace("\r", "");
                List<string> clipText = new List<string>();
                clipText.AddRange(cText.Split((char)10).ToList());

                if (!tp.PLLoaded.LoadFromStringList(clipText, PacketLogFileFormats.Unknown, PacketLogTypes.Unknown))
                {
                    MessageBox.Show("Error loading data from clipboard", "Clipboard Paste Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    tp.PLLoaded.Clear();
                    return;
                }
                Text = defaultTitle + " - " + tp.LoadedLogFile;
                tp.PL.CopyFrom(tp.PLLoaded);
                tp.FillListBox();
                UpdateStatusBarAndTitle(tp);
            }
            catch (Exception x)
            {
                MessageBox.Show("Paste Failed, Exception: " + x.Message, "Paste from Clipboard", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void BtnCopyRawSource_Click(object sender, EventArgs e)
        {
            PacketTabPage tp = GetCurrentPacketTabPage();
            if (tp == null)
            {
                MessageBox.Show("No Packet List selected", "Copy", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            PacketData pd = tp.GetSelectedPacket();
            if (pd == null)
            {
                MessageBox.Show("No Packet selected", "Copy", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string cliptext = "";
            foreach(string s in pd.RawText)
            {
                // re-add the linefeeds
                if (cliptext != string.Empty)
                    cliptext += "\n";
                cliptext += s;
            }
            try
            {
                // Because nothing is ever as simple as >.>
                // Clipboard.SetText(s);
                // Helper will (try to) prevent errors when copying to clipboard because of threading issues
                var cliphelp = new SetClipboardHelper(DataFormats.Text, cliptext);
                cliphelp.DontRetryWorkOnFailed = false;
                cliphelp.Go();
            }
            catch
            {
            }
        }

        private void TcPackets_DrawItem(object sender, DrawItemEventArgs e)
        {
            // Source: https://social.technet.microsoft.com/wiki/contents/articles/50957.c-winform-tabcontrol-with-add-and-close-button.aspx
            // Adapted to using resources and without the add button
            try
            {
                TabControl tabControl = (sender as TabControl);
                var tabPage = tabControl.TabPages[e.Index];
                var tabRect = tabControl.GetTabRect(e.Index);
                tabRect.Inflate(-2, -2);
                var closeImage = Properties.Resources.close_icon;
                if ((tabControl.Alignment == TabAlignment.Top) || (tabControl.Alignment == TabAlignment.Bottom))
                {
                    // for tabs at the top/bottom
                    e.Graphics.DrawImage(closeImage,
                        (tabRect.Right - closeImage.Width),
                        tabRect.Top + (tabRect.Height - closeImage.Height) / 2);
                    TextRenderer.DrawText(e.Graphics, tabPage.Text, tabPage.Font,
                        tabRect, tabPage.ForeColor, TextFormatFlags.Left);
                }
                else 
                if (tabControl.Alignment == TabAlignment.Left)
                {
                    // for tabs to the left
                    e.Graphics.DrawImage(closeImage,
                        tabRect.Left + (tabRect.Width - closeImage.Width) / 2,
                        tabRect.Top);
                    var tSize = e.Graphics.MeasureString(tabPage.Text, tabPage.Font);
                    e.Graphics.TranslateTransform(tabRect.Width, tabRect.Bottom);
                    e.Graphics.RotateTransform(-90);
                    e.Graphics.DrawString(tabPage.Text, tabPage.Font, Brushes.Black, 0, -tabRect.Width - (tSize.Height / -4), StringFormat.GenericDefault);
                }
                else
                {
                    // If you want it on the right as well, you code it >.>
                }
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        private void TcPackets_MouseDown(object sender, MouseEventArgs e)
        {
            // Process MouseDown event only till (tabControl.TabPages.Count - 1) excluding the last TabPage
            TabControl tabControl = (sender as TabControl);
            for (var i = 0; i < tabControl.TabPages.Count; i++)
            {
                var tabRect = tabControl.GetTabRect(i);
                tabRect.Inflate(-2, -2);
                var closeImage = Properties.Resources.close_icon;
                Rectangle imageRect;
                if ((tabControl.Alignment == TabAlignment.Top) || (tabControl.Alignment == TabAlignment.Bottom))
                {
                    imageRect = new Rectangle(
                        (tabRect.Right - closeImage.Width),
                        tabRect.Top + (tabRect.Height - closeImage.Height) / 2,
                        closeImage.Width,
                        closeImage.Height);
                }
                else
                {
                    imageRect = new Rectangle(
                        tabRect.Left + (tabRect.Width - closeImage.Width) / 2,
                        tabRect.Top,
                        closeImage.Width,
                        closeImage.Height);
                }
                if (imageRect.Contains(e.Location))
                {
                    tabControl.TabPages.RemoveAt(i);
                    break;
                }
            }
        }

        public void OpenParseEditor(string parseFileName)
        {
            string editFile = Application.StartupPath + Path.DirectorySeparatorChar + parseFileName;
            if (!File.Exists(editFile))
            {
                if (MessageBox.Show("Parser \"" + parseFileName + "\" doesn't exists, create one ?", "Edit Parse File", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;

                var s = "file;" + Path.GetFileNameWithoutExtension(parseFileName) + ";unnamed package";
                s += "\r\n\r\n";
                s += "rem;insert your parser fields here";
                try
                {
                    File.WriteAllText(editFile, s);
                }
                catch
                {
                    MessageBox.Show("Failed to create new parser file", "Edit Parse File", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            if (Properties.Settings.Default.ExternalParseEditor)
            {
                Process.Start(editFile);
            }
            else
            {
                // Open in-app editor
                var editDlg = new ParseEditorForm();
                editDlg.LoadFromFile(editFile);
                editDlg.Show();
                //MessageBox.Show("Internal editor not implemented yet");
            }
        }

        /*
        private int InfoByteByPos(int pos,PacketParser pp)
        {
            string s = InfoGridHeader;
            int addCharCount = 0;
            for (int i = 0; i < pp.PD.RawBytes.Count; i += 0x10)
            {
                s += i.ToString("X").PadLeft(4, ' ') + " | " ;

                for (int i2 = 0; i2 < 0x10; i2++)
                {
                    var thisByteStartPos = s.Length;

                    if ((i + i2) < pp.ParsedBytes.Count)
                    {
                        s += "XX" ;
                        addCharCount++;
                    }
                    else
                    {
                        s += "  " ;
                    }

                    s += " " ;
                    if ((i2 % 0x4) == 0x3)
                        s += " " ;

                    var thisByteEndPos = s.Length;
                    if ((pos >= thisByteStartPos) && (pos <= thisByteEndPos))
                        return (i + i2);
                }

                if (addCharCount > 0)
                {
                    s += "  | " ;
                    for (int c = 0; (c < 0x10) && ((i + c) < pp.ParsedBytes.Count); c++)
                    {
                        s += "Y" ;
                    }
                    addCharCount = 0;
                }
                s += "\n" ;
            }

            return -1 ;
        }
        */

        private void RtInfo_SelectionChanged(object sender, EventArgs e)
        {
            /*
            if ((rtInfo.Enabled == false) || (dGV.Enabled == false))
                return;
            // Doesn't work on original data as we can't predict it's layout
            if (cbOriginalData.Checked == true)
                return;
            if (PP == null)
                return;
            // Get selection
            var firstPos = rtInfo.SelectionStart;
            var lastPos = rtInfo.SelectionStart + rtInfo.SelectionLength;
            if ((firstPos < 0) || (lastPos < firstPos))
                return;

            rtInfo.Enabled = false;
            dGV.Enabled = false;
            try
            {
                var firstSelected = InfoByteByPos(firstPos, PP);
                var lastSelected = InfoByteByPos(lastPos, PP);
                if ((firstSelected >= 0) && (lastSelected >= 0))
                {
                    List<int> selected = new List<int>();
                    selected.Clear();
                    for (int i = 0; i < PP.ParsedBytes.Count; i++)
                    {
                        if ((i >= firstSelected) && (i <= lastSelected))
                            selected.Add(PP.ParsedBytes[i]);
                    }

                    for (int i = 0; i < PP.ParsedView.Count; i++)
                    {
                        var b = (selected.IndexOf(PP.ParsedView[i].FieldIndex) >= 0);
                        dGV.Rows[i].Selected = b;
                    }
                }
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message);
            }
            finally
            {
                dGV.Enabled = true;
                rtInfo.Enabled = true;
            }
            */
        }

        private void MmVideoOpenLink_Click(object sender, EventArgs e)
        {
            if (VideoLinkForm.GetVLCLibPath() == string.Empty)
            {
                MessageBox.Show("VideoLAN VLC needs to be installed on your PC to use the video linking feature", "libvlc not found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            VideoLinkForm videoLink = null ;
            try
            {
                PacketTabPage thisTP = GetCurrentPacketTabPage();
                if ((thisTP != null) && (thisTP.videoLink != null))
                {
                    // MessageBox.Show("You already have a video link open for this packet", "Video Link Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    thisTP.videoLink.BringToFront();
                    return;
                }
                // Create our virtualtime stamps now
                if (thisTP != null)
                    thisTP.PL.BuildVirtualTimeStamps();
                videoLink = new VideoLinkForm();
                videoLink.sourceTP = thisTP;
                videoLink.Show();
                videoLink.BringToFront();
                UpdateStatusBarAndTitle(thisTP);
            }
            catch (Exception x)
            {
                if (videoLink != null)
                    videoLink.Dispose();
                MessageBox.Show("Could not create video link, likely libvlc not correcty installed !\r\n"+x.Message, "Video Link Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void MmVideoBuildProject_Click(object sender, EventArgs e)
        {
            using (var projectDlg = new ProjectInfoForm())
            {
                projectDlg.LoadFromPacketTapPage(GetCurrentPacketTabPage());
                projectDlg.ShowDialog();
            }
        }

        private void TcPackets_ControlRemoved(object sender, ControlEventArgs e)
        {
            if (e.Control is PacketTabPage)
            {
                PacketTabPage tp = (e.Control as PacketTabPage);
                tp.SaveProjectFile();
                if (tcPackets.TabCount <= 1)
                    Text = defaultTitle;
            }
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            // Properly draw everything before we start
            this.Refresh();
            // Handle arguments
            var args = Environment.GetCommandLineArgs().ToList();
            args.RemoveAt(0);
            foreach (string arg in args)
            {
                if (File.Exists(arg))
                {
                    // open log
                    TryOpenLogFile(arg);
                }
            }
        }
    }
}
