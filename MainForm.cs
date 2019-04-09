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
        const string versionString = "0.0.1";
        const string urlGitHub = "https://github.com/ZeromusXYZ/PacketViewerLogViewer";
        const string urlVideoLAN = "https://www.videolan.org/";

        PacketList PLLoaded; // File Loaded
        PacketList PL; // Filtered File Data Displayed

        public MainForm()
        {
            InitializeComponent();
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
            MessageBox.Show("Made by ZeromusXYZ\r\nVersion " + versionString, "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
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

            if (!PLLoaded.LoadFromFile(openLogFileDialog.FileName))
            {
                MessageBox.Show("Error loading file: " + openLogFileDialog.FileName, "File Open Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                PLLoaded.Clear();
                return;
            }

            PL.CopyFrom(PLLoaded);
            FillListBox();
        }

        private void FillListBox()
        {
            Application.UseWaitCursor = true;
            lbPackets.Items.Clear();
            for(int i = 0; i < PL.Count(); i++)
            {
                PacketData pd = PL.GetPacket(i);
                switch(pd.PacketLogType)
                {
                    case PacketLogTypes.Outgoing:
                        lbPackets.Items.Add("=> " + pd.HeaderText);
                        break;
                    case PacketLogTypes.Incoming:
                        lbPackets.Items.Add("<= " + pd.HeaderText);
                        break;
                    default:
                        lbPackets.Items.Add("?? " + pd.HeaderText);
                        break;
                }
            }
            Application.UseWaitCursor = false;
        }

    }
}
