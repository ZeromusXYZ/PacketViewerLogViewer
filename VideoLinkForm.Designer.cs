namespace PacketViewerLogViewer
{
    partial class VideoLinkForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.media = new Vlc.DotNet.Forms.VlcControl();
            this.btnOpen = new System.Windows.Forms.Button();
            this.openVideoDlg = new System.Windows.Forms.OpenFileDialog();
            this.btnPlay = new System.Windows.Forms.Button();
            this.tb = new System.Windows.Forms.TrackBar();
            this.lVideoPosition = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.eYoutubeURL = new System.Windows.Forms.TextBox();
            this.btnTestYT = new System.Windows.Forms.Button();
            this.cbStayOnTop = new System.Windows.Forms.CheckBox();
            this.cbFollowPacketList = new System.Windows.Forms.CheckBox();
            this.btnSetOffset = new System.Windows.Forms.Button();
            this.lWarningLabel = new System.Windows.Forms.Label();
            this.btnNextFrame = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.media)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tb)).BeginInit();
            this.SuspendLayout();
            // 
            // media
            // 
            this.media.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.media.BackColor = System.Drawing.Color.DimGray;
            this.media.ForeColor = System.Drawing.Color.Black;
            this.media.Location = new System.Drawing.Point(12, 71);
            this.media.Name = "media";
            this.media.Size = new System.Drawing.Size(560, 255);
            this.media.Spu = -1;
            this.media.TabIndex = 0;
            this.media.Text = "Video";
            this.media.VlcLibDirectory = null;
            this.media.VlcMediaplayerOptions = null;
            this.media.VlcLibDirectoryNeeded += new System.EventHandler<Vlc.DotNet.Forms.VlcLibDirectoryNeededEventArgs>(this.Media_VlcLibDirectoryNeeded);
            this.media.LengthChanged += new System.EventHandler<Vlc.DotNet.Core.VlcMediaPlayerLengthChangedEventArgs>(this.Media_LengthChanged);
            this.media.Paused += new System.EventHandler<Vlc.DotNet.Core.VlcMediaPlayerPausedEventArgs>(this.Media_Paused);
            this.media.Playing += new System.EventHandler<Vlc.DotNet.Core.VlcMediaPlayerPlayingEventArgs>(this.Media_Playing);
            this.media.PositionChanged += new System.EventHandler<Vlc.DotNet.Core.VlcMediaPlayerPositionChangedEventArgs>(this.Media_PositionChanged);
            // 
            // btnOpen
            // 
            this.btnOpen.Location = new System.Drawing.Point(12, 10);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(75, 23);
            this.btnOpen.TabIndex = 1;
            this.btnOpen.Text = "Open Local";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.BtnOpen_Click);
            // 
            // openVideoDlg
            // 
            this.openVideoDlg.Filter = "Video files|*.avi;*.mp4;*.mpg;*.mpeg;*.ts;*.mkv;*.mov|All files|*.*";
            this.openVideoDlg.Title = "Open Video File";
            // 
            // btnPlay
            // 
            this.btnPlay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPlay.Location = new System.Drawing.Point(12, 332);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(75, 23);
            this.btnPlay.TabIndex = 2;
            this.btnPlay.Text = "Play  |>";
            this.btnPlay.UseVisualStyleBackColor = true;
            this.btnPlay.Click += new System.EventHandler(this.BtnPlay_Click);
            // 
            // tb
            // 
            this.tb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tb.LargeChange = 60000;
            this.tb.Location = new System.Drawing.Point(12, 361);
            this.tb.Name = "tb";
            this.tb.Size = new System.Drawing.Size(560, 45);
            this.tb.SmallChange = 5000;
            this.tb.TabIndex = 4;
            this.tb.TickFrequency = 10000;
            this.tb.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.tb.Scroll += new System.EventHandler(this.Tb_Scroll);
            // 
            // lVideoPosition
            // 
            this.lVideoPosition.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lVideoPosition.Location = new System.Drawing.Point(351, 337);
            this.lVideoPosition.Name = "lVideoPosition";
            this.lVideoPosition.Size = new System.Drawing.Size(221, 18);
            this.lVideoPosition.TabIndex = 5;
            this.lVideoPosition.Text = "Time";
            this.lVideoPosition.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(320, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Youtube URL:";
            // 
            // eYoutubeURL
            // 
            this.eYoutubeURL.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.eYoutubeURL.Location = new System.Drawing.Point(401, 12);
            this.eYoutubeURL.Name = "eYoutubeURL";
            this.eYoutubeURL.Size = new System.Drawing.Size(171, 20);
            this.eYoutubeURL.TabIndex = 8;
            // 
            // btnTestYT
            // 
            this.btnTestYT.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTestYT.Location = new System.Drawing.Point(438, 42);
            this.btnTestYT.Name = "btnTestYT";
            this.btnTestYT.Size = new System.Drawing.Size(134, 23);
            this.btnTestYT.TabIndex = 9;
            this.btnTestYT.Text = "Stream from Youtube";
            this.btnTestYT.UseVisualStyleBackColor = true;
            this.btnTestYT.Click += new System.EventHandler(this.BtnTestYT_Click);
            // 
            // cbStayOnTop
            // 
            this.cbStayOnTop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbStayOnTop.AutoSize = true;
            this.cbStayOnTop.Checked = true;
            this.cbStayOnTop.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbStayOnTop.Location = new System.Drawing.Point(93, 12);
            this.cbStayOnTop.Name = "cbStayOnTop";
            this.cbStayOnTop.Size = new System.Drawing.Size(80, 17);
            this.cbStayOnTop.TabIndex = 10;
            this.cbStayOnTop.Text = "Stay on top";
            this.cbStayOnTop.UseVisualStyleBackColor = true;
            this.cbStayOnTop.CheckedChanged += new System.EventHandler(this.CbStayOnTop_CheckedChanged);
            // 
            // cbFollowPacketList
            // 
            this.cbFollowPacketList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbFollowPacketList.AutoSize = true;
            this.cbFollowPacketList.Checked = true;
            this.cbFollowPacketList.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbFollowPacketList.Location = new System.Drawing.Point(174, 336);
            this.cbFollowPacketList.Name = "cbFollowPacketList";
            this.cbFollowPacketList.Size = new System.Drawing.Size(98, 17);
            this.cbFollowPacketList.TabIndex = 11;
            this.cbFollowPacketList.Text = "Follow Packets";
            this.cbFollowPacketList.UseVisualStyleBackColor = true;
            // 
            // btnSetOffset
            // 
            this.btnSetOffset.Location = new System.Drawing.Point(12, 42);
            this.btnSetOffset.Name = "btnSetOffset";
            this.btnSetOffset.Size = new System.Drawing.Size(75, 23);
            this.btnSetOffset.TabIndex = 12;
            this.btnSetOffset.Text = "Set Offset";
            this.btnSetOffset.UseVisualStyleBackColor = true;
            this.btnSetOffset.Click += new System.EventHandler(this.BtnSetOffset_Click);
            // 
            // lWarningLabel
            // 
            this.lWarningLabel.AutoSize = true;
            this.lWarningLabel.BackColor = System.Drawing.Color.Maroon;
            this.lWarningLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lWarningLabel.ForeColor = System.Drawing.Color.Yellow;
            this.lWarningLabel.Location = new System.Drawing.Point(19, 80);
            this.lWarningLabel.Name = "lWarningLabel";
            this.lWarningLabel.Size = new System.Drawing.Size(68, 20);
            this.lWarningLabel.TabIndex = 13;
            this.lWarningLabel.Text = "Warning";
            this.lWarningLabel.Visible = false;
            // 
            // btnNextFrame
            // 
            this.btnNextFrame.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnNextFrame.Location = new System.Drawing.Point(93, 332);
            this.btnNextFrame.Name = "btnNextFrame";
            this.btnNextFrame.Size = new System.Drawing.Size(75, 23);
            this.btnNextFrame.TabIndex = 14;
            this.btnNextFrame.Text = "||>>";
            this.btnNextFrame.UseVisualStyleBackColor = true;
            this.btnNextFrame.Click += new System.EventHandler(this.BtnNextFrame_Click);
            // 
            // VideoLinkForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 411);
            this.Controls.Add(this.btnNextFrame);
            this.Controls.Add(this.lWarningLabel);
            this.Controls.Add(this.btnSetOffset);
            this.Controls.Add(this.cbFollowPacketList);
            this.Controls.Add(this.cbStayOnTop);
            this.Controls.Add(this.btnTestYT);
            this.Controls.Add(this.eYoutubeURL);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lVideoPosition);
            this.Controls.Add(this.tb);
            this.Controls.Add(this.btnPlay);
            this.Controls.Add(this.btnOpen);
            this.Controls.Add(this.media);
            this.Name = "VideoLinkForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "VideoLinkForm";
            this.TopMost = true;
            this.Deactivate += new System.EventHandler(this.VideoLinkForm_Deactivate);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.VideoLinkForm_FormClosing);
            this.Load += new System.EventHandler(this.VideoLinkForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.media)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tb)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.OpenFileDialog openVideoDlg;
        private System.Windows.Forms.Button btnPlay;
        private System.Windows.Forms.TrackBar tb;
        private System.Windows.Forms.Label lVideoPosition;
        public Vlc.DotNet.Forms.VlcControl media;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox eYoutubeURL;
        private System.Windows.Forms.Button btnTestYT;
        private System.Windows.Forms.CheckBox cbStayOnTop;
        public System.Windows.Forms.CheckBox cbFollowPacketList;
        private System.Windows.Forms.Button btnSetOffset;
        private System.Windows.Forms.Label lWarningLabel;
        private System.Windows.Forms.Button btnNextFrame;
    }
}