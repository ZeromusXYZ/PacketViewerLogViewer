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

namespace PacketViewerLogViewer
{

    public partial class MainForm : Form
    {
        public static Form thisMainForm ;

        const string versionString = "0.0.1";
        string defaultTitle = "";
        const string urlGitHub = "https://github.com/ZeromusXYZ/PacketViewerLogViewer";
        const string urlVideoLAN = "https://www.videolan.org/";

        PacketList PLLoaded; // File Loaded
        PacketList PL; // Filtered File Data Displayed
        PacketParser PP;
        private UInt16 CurrentSync;

        public MainForm()
        {
            InitializeComponent();
            thisMainForm = this;
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
            PLLoaded = new PacketList();
            PL = new PacketList();
            Application.UseWaitCursor = false;
        }

        private void mmFileOpen_Click(object sender, EventArgs e)
        {
            openLogFileDialog.Title = "Open log file";
            if (openLogFileDialog.ShowDialog() != DialogResult.OK)
                return;

            PacketTabPage tp = new PacketTabPage();
            tcPackets.TabPages.Add(tp);
            tp.Text = System.IO.Path.GetFileNameWithoutExtension(openLogFileDialog.FileName);

            tp.PLLoaded.Clear();
            tp.PLLoaded.ClearFilters();
            if (!tp.PLLoaded.LoadFromFile(openLogFileDialog.FileName))
            {
                MessageBox.Show("Error loading file: " + openLogFileDialog.FileName, "File Open Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tp.PLLoaded.Clear();
                return;
            }
            Text = defaultTitle + " - " + openLogFileDialog.FileName;
            tp.PL.CopyFrom(tp.PLLoaded);
            FillListBox(tp.lbPackets, tp.PL);
        }

        private void FillListBox(ListBox lb, PacketList pList)
        {
            Application.UseWaitCursor = true;
            using (LoadingForm loadform = new LoadingForm(this))
            {
                try
                {
                    Random rand = new Random();
                    if (rand.Next(100) >= 95)
                    {
                        switch (rand.Next(5))
                        {
                            case 0:
                                loadform.Text = "That's a lot of data, please wait ...";
                                break;
                            case 1:
                                loadform.Text = "Burning circles, please wait ...";
                                break;
                            case 2:
                                loadform.Text = "I'm bored, please wait ...";
                                break;
                            case 3:
                                loadform.Text = "Camping NM, please wait ...";
                                break;
                            default:
                                loadform.Text = "Sacrificing Taru-Taru's, please wait ...";
                                break;
                        }
                    }
                    else
                    {
                        loadform.Text = "Populating Listbox, please wait ...";
                    }
                    loadform.Show();
                    loadform.pb.Minimum = 0;
                    loadform.pb.Maximum = pList.Count();
                    lb.Items.Clear();
                    for (int i = 0; i < pList.Count(); i++)
                    {
                        PacketData pd = pList.GetPacket(i);
                        switch (pd.PacketLogType)
                        {
                            case PacketLogTypes.Outgoing:
                                lb.Items.Add("=> " + pd.HeaderText);
                                break;
                            case PacketLogTypes.Incoming:
                                lb.Items.Add("<= " + pd.HeaderText);
                                break;
                            default:
                                lb.Items.Add("?? " + pd.HeaderText);
                                break;
                        }
                        loadform.pb.Value = i;
                        if ((i % 50) == 0)
                            loadform.pb.Refresh();
                    }
                    loadform.Hide();
                }
                catch
                {

                }
            }
            Application.UseWaitCursor = false;
        }

        private void lbPackets_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListBox lb = (sender as ListBox);
            if ((lb.SelectedIndex < 0) || (lb.SelectedIndex >= PL.Count()))
            {
                rtInfo.SelectionColor = rtInfo.ForeColor;
                rtInfo.SelectionBackColor = rtInfo.BackColor;
                rtInfo.Text = "Please select a valid item from the list";
                return;
            }
            PacketData pd = PL.GetPacket(lb.SelectedIndex);
            cbShowBlock.Enabled = false;
            UpdatePacketDetails(pd, "-");
            cbShowBlock.Enabled = true;
            lb.Invalidate();
        }


        private void cbOriginalData_CheckedChanged(object sender, EventArgs e)
        {
            ListBox lb = lbPackets ;
            if ((lb.SelectedIndex < 0) || (lb.SelectedIndex >= PL.Count()))
            {
                rtInfo.SelectionColor = rtInfo.ForeColor;
                rtInfo.SelectionBackColor = rtInfo.BackColor;
                rtInfo.Text = "Please select a valid item from the list";
                return;
            }
            PacketData pd = PL.GetPacket(lb.SelectedIndex);
            UpdatePacketDetails(pd, "-");
        }

        private void mmFileClose_Click(object sender, EventArgs e)
        {
            Text = defaultTitle;
            PLLoaded.Clear();
            PLLoaded.ClearFilters();
            PL.Clear();
            PL.ClearFilters();
            FillListBox(lbPackets,PL);
        }

        private void lbPackets_DrawItem(object sender, DrawItemEventArgs e)
        {
            PacketData pd = null;
            if ((e.Index >= 0) && (e.Index < PL.Count()))
            {
                pd = PL.GetPacket(e.Index);
            }
            else
            {
                // Draw the background of the ListBox control for each item.
                e.DrawBackground();
                return;
            }

            ListBox lb = (sender as ListBox);
            bool barOn = (CurrentSync == pd.PacketSync);
            bool isSelected = (e.Index == lb.SelectedIndex);
            Color textCol;
            Color backCol;
            Color barCol;

            // Determine the color of the brush to draw each item based 
            // on the index of the item to draw.
            switch (pd.PacketLogType)
            {
                case PacketLogTypes.Incoming:
                    textCol = Properties.Settings.Default.ColFontIN;
                    if (isSelected)
                    {
                        backCol = Properties.Settings.Default.ColSelectIN;
                        textCol = Properties.Settings.Default.ColSelectedFontIN;
                    }
                    else
                    if (barOn)
                        backCol = Properties.Settings.Default.ColSyncIN;
                    else
                        backCol = Properties.Settings.Default.ColBackIN;
                    barCol = Properties.Settings.Default.ColBarIN ;
                    break;
                case PacketLogTypes.Outgoing:
                    textCol = Properties.Settings.Default.ColFontOUT;
                    if (isSelected)
                    {
                        backCol = Properties.Settings.Default.ColSelectOUT;
                        textCol = Properties.Settings.Default.ColSelectedFontOUT;
                    }
                    else
                    if (barOn)
                        backCol = Properties.Settings.Default.ColSyncOUT;
                    else
                        backCol = Properties.Settings.Default.ColBackOUT;
                    barCol = Properties.Settings.Default.ColBarOUT;
                    break;
                default:
                    textCol = Properties.Settings.Default.ColFontUNK;
                    if (isSelected)
                    {
                        backCol = Properties.Settings.Default.ColSelectUNK;
                        textCol = Properties.Settings.Default.ColSelectedFontUNK;
                    }
                    else
                    if (barOn)
                        backCol = Properties.Settings.Default.ColSyncUNK;
                    else
                        backCol = Properties.Settings.Default.ColBackUNK;
                    barCol = Properties.Settings.Default.ColBarUNK;
                    break;
            }

            // Define the colors of our brushes.
            Brush textBrush = new SolidBrush(textCol);
            Brush backBrush = new SolidBrush(backCol);
            Brush barBrush = new SolidBrush(barCol);


            // Draw the background of the ListBox control for each item.
            e.Graphics.FillRectangle(backBrush, e.Bounds);

            // Draw the current item text based on the current Font 
            // and the custom brush settings.
            e.Graphics.DrawString(lb.Items[e.Index].ToString(),
                e.Font, textBrush, e.Bounds, StringFormat.GenericDefault);
            if (barOn)
            {
                var barSize = 8;
                if (isSelected)
                    barSize = 16;
                e.Graphics.FillRectangle(barBrush, new Rectangle(e.Bounds.Right - barSize, e.Bounds.Top, barSize, e.Bounds.Height));
            }
            // If the ListBox has focus, draw a focus rectangle around the selected item.
            e.DrawFocusRectangle();
        }

        private void mmFileAppend_Click(object sender, EventArgs e)
        {
            openLogFileDialog.Title = "Append log file";
            if (openLogFileDialog.ShowDialog() != DialogResult.OK)
                return;

            if (!PLLoaded.LoadFromFile(openLogFileDialog.FileName))
            {
                MessageBox.Show("Error loading file: " + openLogFileDialog.FileName, "File Append Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                PLLoaded.Clear();
                return;
            }
            Text = defaultTitle + " - Multiple sources";

            PL.CopyFrom(PLLoaded);
            FillListBox(lbPackets,PL);
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

            rtInfo.Clear();
            SetColorGrid();
            rtInfo.AppendText("     |  0  1  2  3   4  5  6  7   8  9  A  B   C  D  E  F    | 0123456789ABCDEF\r\n" + 
                "-----+----------------------------------------------------  -+------------------\r\n");
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
        }

        private void UpdatePacketDetails(PacketData pd, string SwitchBlockName)
        {
            if (pd == null)
                return;
            CurrentSync = pd.PacketSync;
            lInfo.Text = pd.OriginalHeaderText;
            rtInfo.Clear();

            PP = new PacketParser(pd.PacketID, pd.PacketLogType);
            PP.AssignPacket(pd);
            PP.ParseToDataGridView(dGV,SwitchBlockName);
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

            cbShowBlock.Enabled = false;
            if ((lbPackets.SelectedIndex < 0) || (lbPackets.SelectedIndex >= PL.Count()))
            {
                rtInfo.SelectionColor = rtInfo.ForeColor;
                rtInfo.SelectionBackColor = rtInfo.BackColor;
                rtInfo.Text = "Please select a valid item from the list";
                return;
            }
            PacketData pd = PL.GetPacket(lbPackets.SelectedIndex);
            var sw = cbShowBlock.SelectedIndex;
            if (sw >= 0)
            {
                UpdatePacketDetails(pd, cbShowBlock.Items[sw].ToString());
            }
            else
            {
                UpdatePacketDetails(pd, "-");
            }
            cbShowBlock.Enabled = true;
            lbPackets.Invalidate();
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
    }
}
