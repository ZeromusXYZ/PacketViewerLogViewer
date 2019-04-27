using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace PacketViewerLogViewer
{
    public partial class VideoLinkForm : Form
    {
        public VideoLinkForm()
        {
            InitializeComponent();
        }

        private void Media_VlcLibDirectoryNeeded(object sender, Vlc.DotNet.Forms.VlcLibDirectoryNeededEventArgs e)
        {
            e.VlcLibDirectory = new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.Is64BitProcess ? Environment.SpecialFolder.ProgramFiles : Environment.SpecialFolder.ProgramFilesX86), "VideoLAN\\VLC"));
        }

        private void BtnOpen_Click(object sender, EventArgs e)
        {
            if (openVideoDlg.ShowDialog() != DialogResult.OK)
                return;
            media.SetMedia(new Uri("file://" + openVideoDlg.FileName));
        }

        private void BtnPlay_Click(object sender, EventArgs e)
        {
            media.Play();
        }

        private void BtnPause_Click(object sender, EventArgs e)
        {
            media.Pause();
        }
    }
}
