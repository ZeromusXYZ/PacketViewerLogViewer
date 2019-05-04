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
using PacketViewerLogViewer.PVLVHelper;
using Microsoft.Win32;

namespace PacketViewerLogViewer
{
    public partial class VideoLinkForm : Form
    {
        public PacketTabPage sourceTP { get; set; }
        private bool blockPositionUpdates = false;
        private bool closeOnStop = false;
        // TODO: check if the offset actually works and a way to set it
        // TODO: add something to set offset
        // TODO: add mute options

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
            if (LoadVideoFromLocalFile(openVideoDlg.FileName))
            {
                if (sourceTP != null)
                    sourceTP.LinkVideoFileName = openVideoDlg.FileName;
                media.VlcMediaPlayer.Play();
                media.VlcMediaPlayer.Pause();
                media.VlcMediaPlayer.NextFrame();
            }
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
            if (sourceTP == null)
            {
                Text = "Video not attached to a packet list";
                btnSetOffset.Enabled = false;
                cbFollowPacketList.Checked = false;
                cbFollowPacketList.Enabled = false;
                return;
            }
            if (!File.Exists(sourceTP.LoadedLogFile))
            {
                MessageBox.Show("Can only link video to complete log files", "Video Link", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Text = "Video not attached to a packet list";
                sourceTP = null;
                return;
            }
            Text = "Video - " + sourceTP.LoadedLogFile;
            sourceTP.videoLink = this;
            LoadVideoFromProjectFile();
        }

        public bool LoadVideoFromLocalFile(string filename)
        {
            media.SetMedia(new Uri("file://" + filename));
            return true;
        }

        public bool LoadVideoFromYoutube(string URL)
        {
            try
            {
                // Experimental youtube support
                var videos = YoutubeHelper.GetVideoURLs(URL);
                /*
                cbVideoStreams.Items.Clear();
                foreach (YoutubeHelperVideoLink l in videos)
                {
                    cbVideoStreams.Items.Add(l.QualityName);
                }
                */
                if (videos.Count <= 0)
                    return false;
                media.SetMedia(new Uri(videos[0].VideoURL));
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool LoadVideoFromProjectFile()
        {
            if (sourceTP == null)
                return false;

            try
            { 

                if (File.Exists(sourceTP.LinkVideoFileName))
                {
                    if (!LoadVideoFromLocalFile(sourceTP.LinkVideoFileName))
                        sourceTP.LinkVideoFileName = string.Empty;
                }
                else
                if ((sourceTP.LinkYoutubeURL.ToLower().StartsWith("http://")) || (sourceTP.LinkYoutubeURL.ToLower().StartsWith("https://")))
                {
                    if (!LoadVideoFromYoutube(sourceTP.LinkYoutubeURL))
                        sourceTP.LinkYoutubeURL = string.Empty;
                }
                else
                {
                    sourceTP.LinkVideoFileName = string.Empty;
                    sourceTP.LinkYoutubeURL = string.Empty;
                }

                if ((sourceTP.LinkVideoFileName != string.Empty) || (sourceTP.LinkYoutubeURL != string.Empty))
                {

                    media.VlcMediaPlayer.Play();
                    media.VlcMediaPlayer.Pause();
                    media.VlcMediaPlayer.NextFrame();
                }
            }
            catch
            {
                sourceTP.LinkVideoFileName = string.Empty;
                sourceTP.LinkYoutubeURL = string.Empty;
            }

            eYoutubeURL.Text = sourceTP.LinkYoutubeURL;
            if (sourceTP.LinkYoutubeURL != string.Empty)
                eYoutubeURL.ReadOnly = true;

            return true;
        }

        private void VideoLinkForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (sourceTP != null)
            {
                sourceTP.videoLink = null;
                sourceTP = null;
            }

            if (media.IsPlaying)
            try
            {
                e.Cancel = true;
                closeOnStop = true;
                closeFixTimer.Enabled = true;
                media.ResetMedia();
            }
            catch
            {

            }
        }

        private void Media_LengthChanged(object sender, Vlc.DotNet.Core.VlcMediaPlayerLengthChangedEventArgs e)
        {
            if (closeOnStop)
                return;
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

            var p = pos;
            if (p < 0)
                p = 0;
            if (p > max)
                p = max;
            tb.Maximum = (int)max;
            tb.Value = (int)p;

            if ((sourceTP != null) && (updatePacketList))
            {
                var start = sourceTP.PL.firstPacketTime;
                var videopos = TimeSpan.FromMilliseconds(pos);
                var off = start.Add(videopos).Add(sourceTP.LinkVideoOffset);
                sourceTP.lbPackets.SelectedIndex = sourceTP.PL.FindPacketIndexByDateTime(off, sourceTP.lbPackets.SelectedIndex);
                sourceTP.CenterListBox();
            }
        }

        private void Media_PositionChanged(object sender, Vlc.DotNet.Core.VlcMediaPlayerPositionChangedEventArgs e)
        {
            if (closeOnStop)
                return;
            if (blockPositionUpdates)
                return;
            blockPositionUpdates = true;
            try
            {
                Invoke((MethodInvoker)delegate
                {
                    lWarningLabel.Visible = false;
                    UpdateTimeLabelAndList((long)(e.NewPosition * media.Length), media.Length, cbFollowPacketList.Checked);
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
            UpdateTimeLabelAndList(tb.Value, tb.Maximum, cbFollowPacketList.Checked);

            blockPositionUpdates = false;
        }

        public void MoveToDateTime(DateTime findTime)
        {
            if (blockPositionUpdates)
                return;
            blockPositionUpdates = true;

            TimeSpan off = findTime - sourceTP.PL.firstPacketTime ;
            off = off.Subtract(sourceTP.LinkVideoOffset);
            UpdateTimeLabelAndList((int)off.TotalMilliseconds, tb.Maximum, false);
            media.Position = (float)(off.TotalMilliseconds / media.Length);
            if (off.TotalMilliseconds < 0)
            {
                lWarningLabel.Text = "Negative Offset";
                lWarningLabel.Visible = true;
                media.Visible = false;
            }
            else
            if (off.TotalMilliseconds > tb.Maximum)
            {
                lWarningLabel.Text = "Out of video range";
                lWarningLabel.Visible = true;
                media.Visible = false;
            }
            else
            {
                lWarningLabel.Visible = false;
                media.Visible = true;
            }

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
            if (closeOnStop)
                return;
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
            if (closeOnStop)
                return;
            try
            {
                Invoke((MethodInvoker)delegate {
                    btnPlay.Text = "Pause  | |";
                });
            }
            catch { }
        }

        private void BtnTestYT_Click(object sender, EventArgs e)
        {
            Application.UseWaitCursor = true;
            var s = eYoutubeURL.Text;
            if (LoadVideoFromYoutube(s))
            {
                if (sourceTP != null)
                    sourceTP.LinkYoutubeURL= s;
            }
            else
            {
                MessageBox.Show("Link does not seem valid, or could not access the page.", "Test Youtube Link", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            Application.UseWaitCursor = false;
        }

        private void CbStayOnTop_CheckedChanged(object sender, EventArgs e)
        {
            TopMost = cbStayOnTop.Checked;
        }

        private void BtnSetOffset_Click(object sender, EventArgs e)
        {
            if (sourceTP == null)
            {
                MessageBox.Show("Not linked to a packet log", "Set Offset", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            // Pause if we're still playing
            if (media.State == Vlc.DotNet.Core.Interops.Signatures.MediaStates.Playing)
                media.Pause();

            var thisPacket = sourceTP.GetSelectedPacket();
            if (thisPacket == null)
            {
                MessageBox.Show("No packet selected to offset to", "Set Offset", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            TimeSpan videoTime = TimeSpan.FromMilliseconds(media.Position * media.Length);
            TimeSpan packetTime = thisPacket.VirtualTimeStamp - sourceTP.PL.firstPacketTime ;
            var off = packetTime - videoTime;
            var currentvloff = sourceTP.LinkVideoOffset;

            if (MessageBox.Show("Set Link ?\r\n\r\n"+
                "Current Offset: " + currentvloff.ToString() + "\r\n\r\n" +
                "Packet Time: " + packetTime.ToString() + "\r\n" +
                "Video Time: " +videoTime.ToString() + "\r\n\r\n" +
                "Difference: " + off.ToString(),
                "Set Offset", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                sourceTP.LinkVideoOffset = off;
                cbFollowPacketList.Checked = true;
            }

        }

        private void BtnNextFrame_Click(object sender, EventArgs e)
        {
            if (media.State == Vlc.DotNet.Core.Interops.Signatures.MediaStates.Playing)
                media.Pause();
            media.VlcMediaPlayer.NextFrame();
            long newPos = (long)(media.Position * media.Length);
            UpdateTimeLabelAndList(newPos, media.Length, cbFollowPacketList.Checked);
        }

        private void BtnPrevFrame_Click(object sender, EventArgs e)
        {
            if (media.State == Vlc.DotNet.Core.Interops.Signatures.MediaStates.Playing)
                media.Pause();
            long newPos = (long)(media.Position * media.Length);
            newPos -= 1000;
            if (newPos < 0)
                newPos = 0;
            media.VlcMediaPlayer.Position = ((float)newPos / (float)media.Length);
            UpdateTimeLabelAndList(newPos, media.Length, cbFollowPacketList.Checked);
        }

        private void Media_MediaChanged(object sender, Vlc.DotNet.Core.VlcMediaPlayerMediaChangedEventArgs e)
        {
            lWarningLabel.Visible = false;
            if (closeOnStop)
                Close();
        }

        private void BtnMute_Click(object sender, EventArgs e)
        {
            media.VlcMediaPlayer.Audio.IsMute = !media.VlcMediaPlayer.Audio.IsMute;
            if (media.VlcMediaPlayer.Audio.IsMute)
            {
                btnMute.Text = "X ";
            }
            else
            {
                btnMute.Text = "X¯";
            }

        }

        private void Media_Stopped(object sender, Vlc.DotNet.Core.VlcMediaPlayerStoppedEventArgs e)
        {
            if (!closeOnStop)
                return;
            try
            {
                Invoke((MethodInvoker)delegate {
                    Close();
                });
            }
            catch { }
        }

        private void CloseFixTimer_Tick(object sender, EventArgs e)
        {
            if (closeOnStop)
                Close();
        }
    }
}
