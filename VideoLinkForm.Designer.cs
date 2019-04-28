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
            this.media.Location = new System.Drawing.Point(12, 12);
            this.media.Name = "media";
            this.media.Size = new System.Drawing.Size(560, 314);
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
            this.btnOpen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOpen.Location = new System.Drawing.Point(12, 332);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(75, 23);
            this.btnOpen.TabIndex = 1;
            this.btnOpen.Text = "Open";
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
            this.btnPlay.Location = new System.Drawing.Point(93, 332);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(75, 23);
            this.btnPlay.TabIndex = 2;
            this.btnPlay.Text = "Play  >";
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
            this.lVideoPosition.Location = new System.Drawing.Point(280, 337);
            this.lVideoPosition.Name = "lVideoPosition";
            this.lVideoPosition.Size = new System.Drawing.Size(292, 18);
            this.lVideoPosition.TabIndex = 5;
            this.lVideoPosition.Text = "Time";
            this.lVideoPosition.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // VideoLinkForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 411);
            this.Controls.Add(this.lVideoPosition);
            this.Controls.Add(this.tb);
            this.Controls.Add(this.btnPlay);
            this.Controls.Add(this.btnOpen);
            this.Controls.Add(this.media);
            this.Name = "VideoLinkForm";
            this.Text = "VideoLinkForm";
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
    }
}