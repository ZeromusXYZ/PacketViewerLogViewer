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
        public string LinkFileName;
        public string LinkVideoFileName;
        public string LinkYoutubeURL;
        private bool blockPositionUpdates = false;
        public TimeSpan videoOffset = TimeSpan.Zero;
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
            if (LoadVideoFromLocalFile(openVideoDlg.FileName))
            {
                LinkVideoFileName = openVideoDlg.FileName;
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
            LinkFileName = string.Empty;
            if (sourceTP == null)
            {
                Text = "Video not attached to a packet list";
                return;
            }
            if (!File.Exists(sourceTP.LoadedLogFile))
            {
                MessageBox.Show("Can only link video to complete log files", "Video Link", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Text = "Video not attached to a packet list";
                sourceTP = null;
                return;
            }
            LinkFileName = Path.ChangeExtension(sourceTP.LoadedLogFile, ".pvlvvl"); // Packet Viewer Log Viewer Video Link
            Text = "Video - " + sourceTP.LoadedLogFile;
            sourceTP.videoLink = this;
            LoadVideoLinkFile();
            
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

        public bool LoadVideoLinkFile()
        {
            if (LinkFileName == string.Empty)
                return false;

            try
            {
                string vFile = string.Empty;
                string vYT = string.Empty;
                long vOffset = 0;

                string[] sl;
                if (File.Exists(LinkFileName))
                {
                    sl = File.ReadAllLines(LinkFileName);
                }
                else
                {
                    sl = new string[0];
                }
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
                    if (fields[0].ToLower() == "youtube")
                    {
                        vYT = fields[1];
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

                // If a file is provided, try to expand it to it's full path
                if (vFile != string.Empty)
                {
                    if (!File.Exists(vFile))
                    {
                        var s = Path.GetFullPath(vFile);
                        if (File.Exists(s))
                        {
                            vFile = s;
                        }
                        else
                        {
                            s = Path.GetFullPath(Path.GetDirectoryName(LinkFileName) + Path.DirectorySeparatorChar + vFile);
                            if (File.Exists(s))
                            {
                                vFile = s;
                            }
                        }
                    }
                }

                    

                if (File.Exists(vFile))
                {
                    if (!LoadVideoFromLocalFile(vFile))
                        vFile = string.Empty;
                }
                else
                if ((vYT.ToLower().StartsWith("http://")) || (vYT.ToLower().StartsWith("https://")))
                {
                    if (!LoadVideoFromYoutube(vYT))
                        vYT = string.Empty;
                }
                else
                {
                    vFile = "";
                    vYT = "";
                }
                LinkVideoFileName = vFile;
                LinkYoutubeURL = vYT;
                if ((vFile != string.Empty) || (vYT != string.Empty))
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
                LinkYoutubeURL = string.Empty;
            }

            eYoutubeURL.Text = LinkYoutubeURL;
            if (LinkYoutubeURL != string.Empty)
                eYoutubeURL.ReadOnly = true;


            if (sourceTP != null)
            {
                sourceTP.LinkVideoFileName = LinkVideoFileName;
                sourceTP.LinkYoutubeURL = LinkYoutubeURL;
            }

            return true;
        }

        public bool SaveVideoLinkFile()
        {
            //if ((LinkFileName == string.Empty) || (LinkVideoFileName == string.Empty))
            //    return false;

            string relVideo = string.Empty ;
            if ((LinkFileName != string.Empty) && (LinkVideoFileName != string.Empty))
                relVideo = Helper.MakeRelative(Path.GetDirectoryName(LinkFileName), LinkVideoFileName);
            try
            {
                List<string> sl = new List<string>();
                sl.Add("rem;PacketViewerLogViewer Video Link File");
                sl.Add("video;" + relVideo);
                sl.Add("youtube;" + LinkYoutubeURL);
                sl.Add("offset;" + videoOffset.TotalMilliseconds.ToString());
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
            try
            {
                media.Stop();
            }
            catch
            {

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

            var p = pos;
            if (p < 0)
                p = 0;
            if (p > max)
                p = max;
            tb.Maximum = (int)max;
            tb.Value = (int)p;

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

        private void BtnTestYT_Click(object sender, EventArgs e)
        {
            Application.UseWaitCursor = true;
            var s = eYoutubeURL.Text;
            if (LoadVideoFromYoutube(s))
            {
                LinkYoutubeURL = s;
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
    }
}
