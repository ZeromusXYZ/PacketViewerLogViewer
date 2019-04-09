namespace PacketViewerLogViewer
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.MM = new System.Windows.Forms.MenuStrip();
            this.mmFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mmFileOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.mmFileAppend = new System.Windows.Forms.ToolStripMenuItem();
            this.mmAddFromClipboard = new System.Windows.Forms.ToolStripMenuItem();
            this.mmFileS1 = new System.Windows.Forms.ToolStripSeparator();
            this.mmFileSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.mmFileExit = new System.Windows.Forms.ToolStripMenuItem();
            this.mmSearch = new System.Windows.Forms.ToolStripMenuItem();
            this.mmSearchSearch = new System.Windows.Forms.ToolStripMenuItem();
            this.mmSearchNext = new System.Windows.Forms.ToolStripMenuItem();
            this.mmFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.mmFilterEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.mmFilterReset = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.mmFilterApply = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.mmVideo = new System.Windows.Forms.ToolStripMenuItem();
            this.mmVideoOpenLink = new System.Windows.Forms.ToolStripMenuItem();
            this.mmVideoSaveLinkData = new System.Windows.Forms.ToolStripMenuItem();
            this.mmAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.mmAboutGithub = new System.Windows.Forms.ToolStripMenuItem();
            this.mmAboutVideoLAN = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.mmAboutAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.lbPackets = new System.Windows.Forms.ListBox();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.dGV = new System.Windows.Forms.DataGridView();
            this.cbShowBlock = new System.Windows.Forms.ComboBox();
            this.lInfo = new System.Windows.Forms.Label();
            this.cbOriginalData = new System.Windows.Forms.CheckBox();
            this.mInfo = new System.Windows.Forms.RichTextBox();
            this.openLogFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.MM.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dGV)).BeginInit();
            this.SuspendLayout();
            // 
            // MM
            // 
            this.MM.ImageScalingSize = new System.Drawing.Size(18, 18);
            this.MM.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mmFile,
            this.mmSearch,
            this.mmFilter,
            this.mmVideo,
            this.mmAbout});
            this.MM.Location = new System.Drawing.Point(0, 0);
            this.MM.Name = "MM";
            this.MM.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            this.MM.Size = new System.Drawing.Size(984, 24);
            this.MM.TabIndex = 0;
            this.MM.Text = "Main Menu";
            // 
            // mmFile
            // 
            this.mmFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mmFileOpen,
            this.mmFileAppend,
            this.mmAddFromClipboard,
            this.mmFileS1,
            this.mmFileSettings,
            this.toolStripMenuItem2,
            this.mmFileExit});
            this.mmFile.Name = "mmFile";
            this.mmFile.Size = new System.Drawing.Size(37, 20);
            this.mmFile.Text = "&File";
            // 
            // mmFileOpen
            // 
            this.mmFileOpen.Name = "mmFileOpen";
            this.mmFileOpen.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.O)));
            this.mmFileOpen.Size = new System.Drawing.Size(199, 22);
            this.mmFileOpen.Text = "Open ...";
            this.mmFileOpen.Click += new System.EventHandler(this.mmFileOpen_Click);
            // 
            // mmFileAppend
            // 
            this.mmFileAppend.Name = "mmFileAppend";
            this.mmFileAppend.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.O)));
            this.mmFileAppend.Size = new System.Drawing.Size(199, 22);
            this.mmFileAppend.Text = "Append ...";
            // 
            // mmAddFromClipboard
            // 
            this.mmAddFromClipboard.Name = "mmAddFromClipboard";
            this.mmAddFromClipboard.Size = new System.Drawing.Size(199, 22);
            this.mmAddFromClipboard.Text = "Add from clipboard";
            // 
            // mmFileS1
            // 
            this.mmFileS1.Name = "mmFileS1";
            this.mmFileS1.Size = new System.Drawing.Size(196, 6);
            // 
            // mmFileSettings
            // 
            this.mmFileSettings.Name = "mmFileSettings";
            this.mmFileSettings.Size = new System.Drawing.Size(199, 22);
            this.mmFileSettings.Text = "Settings ...";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(196, 6);
            // 
            // mmFileExit
            // 
            this.mmFileExit.Name = "mmFileExit";
            this.mmFileExit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W)));
            this.mmFileExit.Size = new System.Drawing.Size(199, 22);
            this.mmFileExit.Text = "E&xit";
            this.mmFileExit.Click += new System.EventHandler(this.mmFileExit_Click);
            // 
            // mmSearch
            // 
            this.mmSearch.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mmSearchSearch,
            this.mmSearchNext});
            this.mmSearch.Name = "mmSearch";
            this.mmSearch.Size = new System.Drawing.Size(54, 20);
            this.mmSearch.Text = "&Search";
            // 
            // mmSearchSearch
            // 
            this.mmSearchSearch.Name = "mmSearchSearch";
            this.mmSearchSearch.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.mmSearchSearch.Size = new System.Drawing.Size(161, 22);
            this.mmSearchSearch.Text = "Search ...";
            // 
            // mmSearchNext
            // 
            this.mmSearchNext.Name = "mmSearchNext";
            this.mmSearchNext.ShortcutKeys = System.Windows.Forms.Keys.F3;
            this.mmSearchNext.Size = new System.Drawing.Size(161, 22);
            this.mmSearchNext.Text = "Search next";
            // 
            // mmFilter
            // 
            this.mmFilter.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mmFilterEdit,
            this.mmFilterReset,
            this.toolStripMenuItem3,
            this.mmFilterApply});
            this.mmFilter.Name = "mmFilter";
            this.mmFilter.Size = new System.Drawing.Size(45, 20);
            this.mmFilter.Text = "Fi&lter";
            // 
            // mmFilterEdit
            // 
            this.mmFilterEdit.Name = "mmFilterEdit";
            this.mmFilterEdit.Size = new System.Drawing.Size(106, 22);
            this.mmFilterEdit.Text = "Edit ...";
            // 
            // mmFilterReset
            // 
            this.mmFilterReset.Name = "mmFilterReset";
            this.mmFilterReset.Size = new System.Drawing.Size(106, 22);
            this.mmFilterReset.Text = "Reset";
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(103, 6);
            // 
            // mmFilterApply
            // 
            this.mmFilterApply.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem4});
            this.mmFilterApply.Name = "mmFilterApply";
            this.mmFilterApply.Size = new System.Drawing.Size(106, 22);
            this.mmFilterApply.Text = "Apply";
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(57, 6);
            // 
            // mmVideo
            // 
            this.mmVideo.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mmVideoOpenLink,
            this.mmVideoSaveLinkData});
            this.mmVideo.Name = "mmVideo";
            this.mmVideo.Size = new System.Drawing.Size(49, 20);
            this.mmVideo.Text = "Video";
            // 
            // mmVideoOpenLink
            // 
            this.mmVideoOpenLink.Name = "mmVideoOpenLink";
            this.mmVideoOpenLink.Size = new System.Drawing.Size(183, 22);
            this.mmVideoOpenLink.Text = "Open video link ...";
            // 
            // mmVideoSaveLinkData
            // 
            this.mmVideoSaveLinkData.Name = "mmVideoSaveLinkData";
            this.mmVideoSaveLinkData.Size = new System.Drawing.Size(183, 22);
            this.mmVideoSaveLinkData.Text = "Save Video Link Data";
            // 
            // mmAbout
            // 
            this.mmAbout.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mmAboutGithub,
            this.mmAboutVideoLAN,
            this.toolStripMenuItem5,
            this.mmAboutAbout});
            this.mmAbout.Name = "mmAbout";
            this.mmAbout.Size = new System.Drawing.Size(52, 20);
            this.mmAbout.Text = "&About";
            // 
            // mmAboutGithub
            // 
            this.mmAboutGithub.Name = "mmAboutGithub";
            this.mmAboutGithub.Size = new System.Drawing.Size(197, 22);
            this.mmAboutGithub.Text = "Open source on Github";
            this.mmAboutGithub.Click += new System.EventHandler(this.mmAboutGithub_Click);
            // 
            // mmAboutVideoLAN
            // 
            this.mmAboutVideoLAN.Name = "mmAboutVideoLAN";
            this.mmAboutVideoLAN.Size = new System.Drawing.Size(197, 22);
            this.mmAboutVideoLAN.Text = "Visit VideoLAN VLC site";
            this.mmAboutVideoLAN.Click += new System.EventHandler(this.mmAboutVideoLAN_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(194, 6);
            // 
            // mmAboutAbout
            // 
            this.mmAboutAbout.Name = "mmAboutAbout";
            this.mmAboutAbout.Size = new System.Drawing.Size(197, 22);
            this.mmAboutAbout.Text = "About ...";
            this.mmAboutAbout.Click += new System.EventHandler(this.mmAboutAbout_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lbPackets);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(984, 435);
            this.splitContainer1.SplitterDistance = 328;
            this.splitContainer1.TabIndex = 3;
            // 
            // lbPackets
            // 
            this.lbPackets.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbPackets.FormattingEnabled = true;
            this.lbPackets.ItemHeight = 14;
            this.lbPackets.Location = new System.Drawing.Point(12, 12);
            this.lbPackets.Name = "lbPackets";
            this.lbPackets.Size = new System.Drawing.Size(301, 396);
            this.lbPackets.TabIndex = 0;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.dGV);
            this.splitContainer2.Panel1.Controls.Add(this.cbShowBlock);
            this.splitContainer2.Panel1.Controls.Add(this.lInfo);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.cbOriginalData);
            this.splitContainer2.Panel2.Controls.Add(this.mInfo);
            this.splitContainer2.Size = new System.Drawing.Size(652, 435);
            this.splitContainer2.SplitterDistance = 276;
            this.splitContainer2.TabIndex = 0;
            // 
            // dGV
            // 
            this.dGV.AllowUserToAddRows = false;
            this.dGV.AllowUserToDeleteRows = false;
            this.dGV.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dGV.Location = new System.Drawing.Point(6, 32);
            this.dGV.Name = "dGV";
            this.dGV.ReadOnly = true;
            this.dGV.Size = new System.Drawing.Size(634, 241);
            this.dGV.TabIndex = 2;
            // 
            // cbShowBlock
            // 
            this.cbShowBlock.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbShowBlock.FormattingEnabled = true;
            this.cbShowBlock.Location = new System.Drawing.Point(482, 6);
            this.cbShowBlock.Name = "cbShowBlock";
            this.cbShowBlock.Size = new System.Drawing.Size(158, 22);
            this.cbShowBlock.TabIndex = 1;
            // 
            // lInfo
            // 
            this.lInfo.AutoSize = true;
            this.lInfo.Location = new System.Drawing.Point(3, 12);
            this.lInfo.Name = "lInfo";
            this.lInfo.Size = new System.Drawing.Size(35, 14);
            this.lInfo.TabIndex = 0;
            this.lInfo.Text = "Info";
            // 
            // cbOriginalData
            // 
            this.cbOriginalData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbOriginalData.AutoSize = true;
            this.cbOriginalData.Location = new System.Drawing.Point(6, 134);
            this.cbOriginalData.Name = "cbOriginalData";
            this.cbOriginalData.Size = new System.Drawing.Size(152, 18);
            this.cbOriginalData.TabIndex = 1;
            this.cbOriginalData.Text = "Show original data";
            this.cbOriginalData.UseVisualStyleBackColor = true;
            // 
            // mInfo
            // 
            this.mInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mInfo.Location = new System.Drawing.Point(6, 3);
            this.mInfo.Name = "mInfo";
            this.mInfo.ReadOnly = true;
            this.mInfo.Size = new System.Drawing.Size(634, 122);
            this.mInfo.TabIndex = 0;
            this.mInfo.Text = resources.GetString("mInfo.Text");
            // 
            // openLogFileDialog
            // 
            this.openLogFileDialog.DefaultExt = "log";
            this.openLogFileDialog.Filter = "Log files|*.log;*.txt|Packet Viewer Log Files|*.log|Packeteer Log Files|*.txt|All" +
    " Files|*.*";
            this.openLogFileDialog.SupportMultiDottedExtensions = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 459);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.MM);
            this.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.MM;
            this.MinimumSize = new System.Drawing.Size(400, 300);
            this.Name = "MainForm";
            this.Text = "Packet Viewer Log Viewer";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.MM.ResumeLayout(false);
            this.MM.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dGV)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip MM;
        private System.Windows.Forms.ToolStripMenuItem mmFile;
        private System.Windows.Forms.ToolStripMenuItem mmFileOpen;
        private System.Windows.Forms.ToolStripMenuItem mmFileAppend;
        private System.Windows.Forms.ToolStripMenuItem mmAddFromClipboard;
        private System.Windows.Forms.ToolStripSeparator mmFileS1;
        private System.Windows.Forms.ToolStripMenuItem mmFileSettings;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem mmFileExit;
        private System.Windows.Forms.ToolStripMenuItem mmSearch;
        private System.Windows.Forms.ToolStripMenuItem mmSearchSearch;
        private System.Windows.Forms.ToolStripMenuItem mmSearchNext;
        private System.Windows.Forms.ToolStripMenuItem mmFilter;
        private System.Windows.Forms.ToolStripMenuItem mmFilterEdit;
        private System.Windows.Forms.ToolStripMenuItem mmFilterReset;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem mmFilterApply;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem mmVideo;
        private System.Windows.Forms.ToolStripMenuItem mmVideoOpenLink;
        private System.Windows.Forms.ToolStripMenuItem mmVideoSaveLinkData;
        private System.Windows.Forms.ToolStripMenuItem mmAbout;
        private System.Windows.Forms.ToolStripMenuItem mmAboutGithub;
        private System.Windows.Forms.ToolStripMenuItem mmAboutVideoLAN;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem mmAboutAbout;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListBox lbPackets;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.ComboBox cbShowBlock;
        private System.Windows.Forms.Label lInfo;
        private System.Windows.Forms.DataGridView dGV;
        private System.Windows.Forms.CheckBox cbOriginalData;
        private System.Windows.Forms.RichTextBox mInfo;
        private System.Windows.Forms.OpenFileDialog openLogFileDialog;
    }
}

