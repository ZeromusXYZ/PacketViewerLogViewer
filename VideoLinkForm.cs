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
using PacketViewerLogViewer.Packets;
using PacketViewerLogViewer.YoutubeVideoHelper;
using Microsoft.Win32;

namespace PacketViewerLogViewer
{
    public partial class VideoLinkForm : Form
    {
        public PacketTabPage sourceTP { get; set; }
        private string LinkFileName;
        private string LinkVideoFileName;
        private bool blockPositionUpdates = false;
        private TimeSpan videoOffset = TimeSpan.Zero;
        // TODO: check if the offset actually works and a way to set it
        // TODO: add something to set offset
        // TODO: add mute and auto-load options
        // TODO: add "follow packets" checkbox

        public VideoLinkForm()
        {
            InitializeComponent();
        }

        static public string GetVLCLibPath()
        {
            // First try to get from registry
            string res ;
            try
            {
                res = Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\VideoLAN\\VLC", "InstallDir", "").ToString();
            }
            catch
            {
                res = "";
            }
            if (File.Exists(res + Path.DirectorySeparatorChar + "libvlc.dll"))
                return res;

            // Try default location
            res = Path.Combine(Environment.GetFolderPath(Environment.Is64BitProcess ? Environment.SpecialFolder.ProgramFiles : Environment.SpecialFolder.ProgramFilesX86), "VideoLAN\\VLC");
            if (File.Exists(res + Path.DirectorySeparatorChar + "libvlc.dll"))
                return res;

            res = "";
            return res;
        }

        private void Media_VlcLibDirectoryNeeded(object sender, Vlc.DotNet.Forms.VlcLibDirectoryNeededEventArgs e)
        {
            var dir = GetVLCLibPath();
            if (dir == string.Empty)
                e.VlcLibDirectory = null; // this will throw a exception
            else
                e.VlcLibDirectory = new DirectoryInfo(dir);
        }

        private void BtnOpen_Click(object sender, EventArgs e)
        {
            if (openVideoDlg.ShowDialog() != DialogResult.OK)
                return;
            media.SetMedia(new Uri("file://" + openVideoDlg.FileName));
            LinkVideoFileName = openVideoDlg.FileName;
            media.VlcMediaPlayer.Play();
            media.VlcMediaPlayer.Pause();
            media.VlcMediaPlayer.NextFrame();
        }

        private void BtnPlay_Click(object sender, EventArgs e)
        {
            if (media.State == Vlc.DotNet.Core.Interops.Signatures.MediaStates.Playing)
                media.Pause();
            else
                media.Play();
        }

        private void VideoLinkForm_Load(object sender, EventArgs e)
        {
            LinkFileName = string.Empty;
            if (sourceTP == null)
            {
                Text = "Video not attached to a packet list";
                return;
            }
            if (!File.Exists(sourceTP.LoadedFileTitle))
            {
                MessageBox.Show("Can only link video to complete log files", "Video Link", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Text = "Video not attached to a packet list";
                sourceTP = null;
                return;
            }
            LinkFileName = Path.ChangeExtension(sourceTP.LoadedFileTitle, ".pvlvvl"); // Packet Viewer Log Viewer Video Link
            Text = "Video - " + sourceTP.LoadedFileTitle;
            sourceTP.videoLink = this;
            LoadVideoLinkFile();

            
        }

        public bool LoadVideoLinkFile()
        {
            if (LinkFileName == string.Empty)
                return false;

            try
            {
                string vFile = string.Empty;
                long vOffset = 0;

                string[] sl = File.ReadAllLines(LinkFileName);
                foreach (string s in sl)
                {
                    var fields = s.Split(';');
                    if (fields.Length < 2)
                        continue;
                    if (fields[0].ToLower() == "video")
                    {
                        vFile = fields[1];
                    }
                    else
                    if (fields[0].ToLower() == "offset")
                    {
                        if (DataLookups.TryFieldParse(fields[1], out int n))
                            vOffset = n;
                    }
                    else
                    {
                        continue;
                    }

                }

                videoOffset = TimeSpan.FromMilliseconds(vOffset);

                if (File.Exists(vFile))
                {
                    media.SetMedia(new Uri("file://" + vFile));
                }
                else
                if ( (vFile.ToLower().StartsWith("http")) && (vFile.ToLower().IndexOf("youtube.com") >= 0) )
                {
                    // Experimental youtube support
                    var videos = YoutubeHelper.GetVideoURLs(vFile);
                    if (videos.Count > 0)
                        media.SetMedia(new Uri(videos[0]));
                    else
                        vFile = "";
                }
                else
                {
                    vFile = "";
                }
                LinkVideoFileName = vFile;
                if (vFile != string.Empty)
                {

                    media.VlcMediaPlayer.Play();
                    media.VlcMediaPlayer.Pause();
                    media.VlcMediaPlayer.NextFrame();
                }
            }
            catch
            {
                LinkFileName = string.Empty;
                LinkVideoFileName = string.Empty;
            }



            return true;
        }

        public bool SaveVideoLinkFile()
        {
            if ((LinkFileName == string.Empty) || (LinkVideoFileName == string.Empty))
                return false;

            if (!File.Exists(LinkVideoFileName))
                return false;

            try
            {
                List<string> sl = new List<string>();
                sl.Add("rem;PacketViewerLogViewer Video Link File");
                sl.Add("video;" + LinkVideoFileName);
                sl.Add("offset;" + videoOffset.ToString());
                File.WriteAllLines(LinkFileName, sl);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void VideoLinkForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveVideoLinkFile();
            if (sourceTP != null)
            {
                sourceTP.videoLink = null;
                sourceTP = null;
            }
        }

        private void Media_LengthChanged(object sender, Vlc.DotNet.Core.VlcMediaPlayerLengthChangedEventArgs e)
        {
            try
            {
                Invoke((MethodInvoker)delegate { 
                    tb.Maximum = (int)e.NewLength;
                    tb.Minimum = 0;
                });
            }
            catch { }
        }

        private void UpdateTimeLabelAndList(long pos, long max, bool updatePacketList)
        {
            lVideoPosition.Text = "Time: " + MediaTimeToString(pos) + " / " + MediaTimeToString(max);

            tb.Value = (int)pos;

            if ((sourceTP != null) && (updatePacketList))
            {
                var start = sourceTP.PL.GetPacket(0).VirtualTimeStamp;
                var videopos = TimeSpan.FromMilliseconds(pos);
                var off = start.Add(videopos).Add(videoOffset);
                sourceTP.lbPackets.SelectedIndex = sourceTP.PL.FindPacketIndexByDateTime(off, sourceTP.lbPackets.SelectedIndex);
                sourceTP.CenterListBox();
            }
        }

        private void Media_PositionChanged(object sender, Vlc.DotNet.Core.VlcMediaPlayerPositionChangedEventArgs e)
        {
            if (blockPositionUpdates)
                return;
            blockPositionUpdates = true;
            try
            {
                Invoke((MethodInvoker)delegate
                {
                    UpdateTimeLabelAndList((long)(e.NewPosition * media.Length), media.Length, true);
                });
            }
            catch { }
            blockPositionUpdates = false;
        }

        private string MediaTimeToString(long position)
        {
            var h = (position / 3600000);
            var m = ((position / 60000) % 60);
            var s = ((position / 1000) % 60);
            string res = "";
            if (h > 0)
                res += h.ToString("00") + ":";
            res += m.ToString("00") + "." + s.ToString("00");
            return res;
        }

        private void Tb_Scroll(object sender, EventArgs e)
        {
            if (blockPositionUpdates)
                return;
            blockPositionUpdates = true;

            media.Position = ((float)tb.Value / (float)tb.Maximum);
            UpdateTimeLabelAndList(tb.Value, tb.Maximum, true);

            blockPositionUpdates = false;
        }

        public void MoveToDateTime(DateTime findTime)
        {
            if (blockPositionUpdates)
                return;
            blockPositionUpdates = true;

            TimeSpan off = findTime - sourceTP.PL.firstPacketTime ;
            off = off.Subtract(videoOffset);
            UpdateTimeLabelAndList((int)off.TotalMilliseconds, tb.Maximum, false);
            media.Position = (float)(off.TotalMilliseconds / media.Length);

            blockPositionUpdates = false;
        }

        private void VideoLinkForm_Deactivate(object sender, EventArgs e)
        {
            // pause if playing
            if (media.State == Vlc.DotNet.Core.Interops.Signatures.MediaStates.Playing)
                media.Pause();
        }

        private void Media_Paused(object sender, Vlc.DotNet.Core.VlcMediaPlayerPausedEventArgs e)
        {
            try
            {
                Invoke((MethodInvoker)delegate {
                    btnPlay.Text = "Play  |>";
                });
            }
            catch { }
        }

        private void Media_Playing(object sender, Vlc.DotNet.Core.VlcMediaPlayerPlayingEventArgs e)
        {
            try
            {
                Invoke((MethodInvoker)delegate {
                    btnPlay.Text = "Pause  | |";
                });
            }
            catch { }
        }
    }
}
