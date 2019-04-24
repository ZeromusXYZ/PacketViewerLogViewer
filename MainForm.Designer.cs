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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.MM = new System.Windows.Forms.MenuStrip();
            this.mmFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mmFileOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.mmFileAppend = new System.Windows.Forms.ToolStripMenuItem();
            this.mmAddFromClipboard = new System.Windows.Forms.ToolStripMenuItem();
            this.mmFilePasteNew = new System.Windows.Forms.ToolStripMenuItem();
            this.mmFileS1 = new System.Windows.Forms.ToolStripSeparator();
            this.mmFileSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.mmFileS2 = new System.Windows.Forms.ToolStripSeparator();
            this.mmFileClose = new System.Windows.Forms.ToolStripMenuItem();
            this.mmFileS3 = new System.Windows.Forms.ToolStripSeparator();
            this.mmFileExit = new System.Windows.Forms.ToolStripMenuItem();
            this.mmSearch = new System.Windows.Forms.ToolStripMenuItem();
            this.mmSearchSearch = new System.Windows.Forms.ToolStripMenuItem();
            this.mmSearchNext = new System.Windows.Forms.ToolStripMenuItem();
            this.mmFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.mmFilterApply = new System.Windows.Forms.ToolStripMenuItem();
            this.MMFilterApplyItem = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.mmFilterEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.mmFilterReset = new System.Windows.Forms.ToolStripMenuItem();
            this.mmVideo = new System.Windows.Forms.ToolStripMenuItem();
            this.mmVideoOpenLink = new System.Windows.Forms.ToolStripMenuItem();
            this.mmVideoSaveLinkData = new System.Windows.Forms.ToolStripMenuItem();
            this.mmAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.mmAboutGithub = new System.Windows.Forms.ToolStripMenuItem();
            this.mmAboutVideoLAN = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.mmAboutAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tcPackets = new System.Windows.Forms.TabControl();
            this.tpPackets1 = new System.Windows.Forms.TabPage();
            this.lbPackets = new System.Windows.Forms.ListBox();
            this.pmPacketList = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.pmPLShowPacketName = new System.Windows.Forms.ToolStripMenuItem();
            this.pmPLS1 = new System.Windows.Forms.ToolStripSeparator();
            this.pmPLShowOnly = new System.Windows.Forms.ToolStripMenuItem();
            this.pmPLHideThis = new System.Windows.Forms.ToolStripMenuItem();
            this.pmPLS2 = new System.Windows.Forms.ToolStripSeparator();
            this.pmPLShowOutOnly = new System.Windows.Forms.ToolStripMenuItem();
            this.plPLShowInOnly = new System.Windows.Forms.ToolStripMenuItem();
            this.pmPLS3 = new System.Windows.Forms.ToolStripSeparator();
            this.pmPLResetFilters = new System.Windows.Forms.ToolStripMenuItem();
            this.pmPLS4 = new System.Windows.Forms.ToolStripSeparator();
            this.pmPLEditParser = new System.Windows.Forms.ToolStripMenuItem();
            this.pmPLExportPacket = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.dGV = new System.Windows.Forms.DataGridView();
            this.cbShowBlock = new System.Windows.Forms.ComboBox();
            this.lInfo = new System.Windows.Forms.Label();
            this.btnCopyRawSource = new System.Windows.Forms.Button();
            this.rtInfo = new System.Windows.Forms.RichTextBox();
            this.cbOriginalData = new System.Windows.Forms.CheckBox();
            this.openLogFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.MM.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tcPackets.SuspendLayout();
            this.tpPackets1.SuspendLayout();
            this.pmPacketList.SuspendLayout();
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
            this.MM.Size = new System.Drawing.Size(1084, 24);
            this.MM.TabIndex = 0;
            this.MM.Text = "Main Menu";
            // 
            // mmFile
            // 
            this.mmFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mmFileOpen,
            this.mmFileAppend,
            this.mmAddFromClipboard,
            this.mmFilePasteNew,
            this.mmFileS1,
            this.mmFileSettings,
            this.mmFileS2,
            this.mmFileClose,
            this.mmFileS3,
            this.mmFileExit});
            this.mmFile.Name = "mmFile";
            this.mmFile.Size = new System.Drawing.Size(37, 20);
            this.mmFile.Text = "&File";
            // 
            // mmFileOpen
            // 
            this.mmFileOpen.Name = "mmFileOpen";
            this.mmFileOpen.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.O)));
            this.mmFileOpen.Size = new System.Drawing.Size(247, 22);
            this.mmFileOpen.Text = "Open ...";
            this.mmFileOpen.Click += new System.EventHandler(this.mmFileOpen_Click);
            // 
            // mmFileAppend
            // 
            this.mmFileAppend.Name = "mmFileAppend";
            this.mmFileAppend.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.O)));
            this.mmFileAppend.Size = new System.Drawing.Size(247, 22);
            this.mmFileAppend.Text = "Append ...";
            this.mmFileAppend.Click += new System.EventHandler(this.mmFileAppend_Click);
            // 
            // mmAddFromClipboard
            // 
            this.mmAddFromClipboard.Name = "mmAddFromClipboard";
            this.mmAddFromClipboard.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.V)));
            this.mmAddFromClipboard.Size = new System.Drawing.Size(247, 22);
            this.mmAddFromClipboard.Text = "Add from clipboard";
            this.mmAddFromClipboard.Click += new System.EventHandler(this.MmAddFromClipboard_Click);
            // 
            // mmFilePasteNew
            // 
            this.mmFilePasteNew.Name = "mmFilePasteNew";
            this.mmFilePasteNew.Size = new System.Drawing.Size(247, 22);
            this.mmFilePasteNew.Text = "New Tab from Clipboard";
            this.mmFilePasteNew.Click += new System.EventHandler(this.MmFilePasteNew_Click);
            // 
            // mmFileS1
            // 
            this.mmFileS1.Name = "mmFileS1";
            this.mmFileS1.Size = new System.Drawing.Size(244, 6);
            // 
            // mmFileSettings
            // 
            this.mmFileSettings.Name = "mmFileSettings";
            this.mmFileSettings.Size = new System.Drawing.Size(247, 22);
            this.mmFileSettings.Text = "Settings ...";
            this.mmFileSettings.Click += new System.EventHandler(this.mmFileSettings_Click);
            // 
            // mmFileS2
            // 
            this.mmFileS2.Name = "mmFileS2";
            this.mmFileS2.Size = new System.Drawing.Size(244, 6);
            // 
            // mmFileClose
            // 
            this.mmFileClose.Name = "mmFileClose";
            this.mmFileClose.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W)));
            this.mmFileClose.Size = new System.Drawing.Size(247, 22);
            this.mmFileClose.Text = "Close";
            this.mmFileClose.Click += new System.EventHandler(this.mmFileClose_Click);
            // 
            // mmFileS3
            // 
            this.mmFileS3.Name = "mmFileS3";
            this.mmFileS3.Size = new System.Drawing.Size(244, 6);
            // 
            // mmFileExit
            // 
            this.mmFileExit.Name = "mmFileExit";
            this.mmFileExit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.mmFileExit.Size = new System.Drawing.Size(247, 22);
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
            this.mmSearchSearch.Click += new System.EventHandler(this.MmSearchSearch_Click);
            // 
            // mmSearchNext
            // 
            this.mmSearchNext.Name = "mmSearchNext";
            this.mmSearchNext.ShortcutKeys = System.Windows.Forms.Keys.F3;
            this.mmSearchNext.Size = new System.Drawing.Size(161, 22);
            this.mmSearchNext.Text = "Search next";
            this.mmSearchNext.Click += new System.EventHandler(this.MmSearchNext_Click);
            // 
            // mmFilter
            // 
            this.mmFilter.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mmFilterApply,
            this.toolStripMenuItem3,
            this.mmFilterEdit,
            this.mmFilterReset});
            this.mmFilter.Name = "mmFilter";
            this.mmFilter.Size = new System.Drawing.Size(45, 20);
            this.mmFilter.Text = "Fi&lter";
            // 
            // mmFilterApply
            // 
            this.mmFilterApply.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MMFilterApplyItem});
            this.mmFilterApply.Name = "mmFilterApply";
            this.mmFilterApply.Size = new System.Drawing.Size(106, 22);
            this.mmFilterApply.Text = "Apply";
            this.mmFilterApply.DropDownOpening += new System.EventHandler(this.MmFilterApply_DropDownOpening);
            // 
            // MMFilterApplyItem
            // 
            this.MMFilterApplyItem.Name = "MMFilterApplyItem";
            this.MMFilterApplyItem.Size = new System.Drawing.Size(57, 6);
            this.MMFilterApplyItem.Click += new System.EventHandler(this.MMFilterApplyItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(103, 6);
            // 
            // mmFilterEdit
            // 
            this.mmFilterEdit.Name = "mmFilterEdit";
            this.mmFilterEdit.Size = new System.Drawing.Size(106, 22);
            this.mmFilterEdit.Text = "Edit ...";
            this.mmFilterEdit.Click += new System.EventHandler(this.MmFilterEdit_Click);
            // 
            // mmFilterReset
            // 
            this.mmFilterReset.Name = "mmFilterReset";
            this.mmFilterReset.Size = new System.Drawing.Size(106, 22);
            this.mmFilterReset.Text = "Reset";
            this.mmFilterReset.Click += new System.EventHandler(this.MmFilterReset_Click);
            // 
            // mmVideo
            // 
            this.mmVideo.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mmVideoOpenLink,
            this.mmVideoSaveLinkData});
            this.mmVideo.Enabled = false;
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
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tcPackets);
            this.splitContainer1.Panel1MinSize = 350;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(1084, 487);
            this.splitContainer1.SplitterDistance = 350;
            this.splitContainer1.TabIndex = 3;
            // 
            // tcPackets
            // 
            this.tcPackets.Alignment = System.Windows.Forms.TabAlignment.Left;
            this.tcPackets.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tcPackets.Controls.Add(this.tpPackets1);
            this.tcPackets.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.tcPackets.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.tcPackets.HotTrack = true;
            this.tcPackets.Location = new System.Drawing.Point(0, 0);
            this.tcPackets.Multiline = true;
            this.tcPackets.Name = "tcPackets";
            this.tcPackets.SelectedIndex = 0;
            this.tcPackets.Size = new System.Drawing.Size(347, 484);
            this.tcPackets.TabIndex = 1;
            this.tcPackets.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.TcPackets_DrawItem);
            this.tcPackets.SelectedIndexChanged += new System.EventHandler(this.TcPackets_SelectedIndexChanged);
            this.tcPackets.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TcPackets_MouseDown);
            // 
            // tpPackets1
            // 
            this.tpPackets1.Controls.Add(this.lbPackets);
            this.tpPackets1.Location = new System.Drawing.Point(24, 4);
            this.tpPackets1.Name = "tpPackets1";
            this.tpPackets1.Padding = new System.Windows.Forms.Padding(3);
            this.tpPackets1.Size = new System.Drawing.Size(319, 476);
            this.tpPackets1.TabIndex = 0;
            this.tpPackets1.Text = "Packets";
            this.tpPackets1.UseVisualStyleBackColor = true;
            // 
            // lbPackets
            // 
            this.lbPackets.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbPackets.ContextMenuStrip = this.pmPacketList;
            this.lbPackets.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lbPackets.FormattingEnabled = true;
            this.lbPackets.ItemHeight = 14;
            this.lbPackets.Location = new System.Drawing.Point(0, 0);
            this.lbPackets.Name = "lbPackets";
            this.lbPackets.Size = new System.Drawing.Size(319, 480);
            this.lbPackets.TabIndex = 0;
            this.lbPackets.SelectedIndexChanged += new System.EventHandler(this.lbPackets_SelectedIndexChanged);
            // 
            // pmPacketList
            // 
            this.pmPacketList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pmPLShowPacketName,
            this.pmPLS1,
            this.pmPLShowOnly,
            this.pmPLHideThis,
            this.pmPLS2,
            this.pmPLShowOutOnly,
            this.plPLShowInOnly,
            this.pmPLS3,
            this.pmPLResetFilters,
            this.pmPLS4,
            this.pmPLEditParser,
            this.pmPLExportPacket});
            this.pmPacketList.Name = "pmPacketList";
            this.pmPacketList.Size = new System.Drawing.Size(208, 204);
            // 
            // pmPLShowPacketName
            // 
            this.pmPLShowPacketName.Name = "pmPLShowPacketName";
            this.pmPLShowPacketName.Size = new System.Drawing.Size(207, 22);
            this.pmPLShowPacketName.Text = "This packet type is";
            // 
            // pmPLS1
            // 
            this.pmPLS1.Name = "pmPLS1";
            this.pmPLS1.Size = new System.Drawing.Size(204, 6);
            // 
            // pmPLShowOnly
            // 
            this.pmPLShowOnly.Name = "pmPLShowOnly";
            this.pmPLShowOnly.Size = new System.Drawing.Size(207, 22);
            this.pmPLShowOnly.Text = "Show only this type";
            // 
            // pmPLHideThis
            // 
            this.pmPLHideThis.Name = "pmPLHideThis";
            this.pmPLHideThis.Size = new System.Drawing.Size(207, 22);
            this.pmPLHideThis.Text = "Hide this type";
            // 
            // pmPLS2
            // 
            this.pmPLS2.Name = "pmPLS2";
            this.pmPLS2.Size = new System.Drawing.Size(204, 6);
            // 
            // pmPLShowOutOnly
            // 
            this.pmPLShowOutOnly.Name = "pmPLShowOutOnly";
            this.pmPLShowOutOnly.Size = new System.Drawing.Size(207, 22);
            this.pmPLShowOutOnly.Text = "Show only Outgoing";
            // 
            // plPLShowInOnly
            // 
            this.plPLShowInOnly.Name = "plPLShowInOnly";
            this.plPLShowInOnly.Size = new System.Drawing.Size(207, 22);
            this.plPLShowInOnly.Text = "Show only Incoming";
            // 
            // pmPLS3
            // 
            this.pmPLS3.Name = "pmPLS3";
            this.pmPLS3.Size = new System.Drawing.Size(204, 6);
            // 
            // pmPLResetFilters
            // 
            this.pmPLResetFilters.Name = "pmPLResetFilters";
            this.pmPLResetFilters.Size = new System.Drawing.Size(207, 22);
            this.pmPLResetFilters.Text = "Reset all filters";
            // 
            // pmPLS4
            // 
            this.pmPLS4.Name = "pmPLS4";
            this.pmPLS4.Size = new System.Drawing.Size(204, 6);
            // 
            // pmPLEditParser
            // 
            this.pmPLEditParser.Name = "pmPLEditParser";
            this.pmPLEditParser.Size = new System.Drawing.Size(207, 22);
            this.pmPLEditParser.Text = "Edit parser for this packet";
            // 
            // pmPLExportPacket
            // 
            this.pmPLExportPacket.Name = "pmPLExportPacket";
            this.pmPLExportPacket.Size = new System.Drawing.Size(207, 22);
            this.pmPLExportPacket.Text = "Export packet";
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
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
            this.splitContainer2.Panel2.Controls.Add(this.btnCopyRawSource);
            this.splitContainer2.Panel2.Controls.Add(this.rtInfo);
            this.splitContainer2.Panel2.Controls.Add(this.cbOriginalData);
            this.splitContainer2.Size = new System.Drawing.Size(730, 487);
            this.splitContainer2.SplitterDistance = 328;
            this.splitContainer2.TabIndex = 0;
            // 
            // dGV
            // 
            this.dGV.AllowUserToAddRows = false;
            this.dGV.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.dGV.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dGV.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dGV.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.dGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dGV.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnF2;
            this.dGV.Location = new System.Drawing.Point(3, 32);
            this.dGV.Name = "dGV";
            this.dGV.RowHeadersVisible = false;
            this.dGV.RowHeadersWidth = 8;
            this.dGV.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dGV.Size = new System.Drawing.Size(712, 293);
            this.dGV.TabIndex = 2;
            this.dGV.SelectionChanged += new System.EventHandler(this.dGV_SelectionChanged);
            // 
            // cbShowBlock
            // 
            this.cbShowBlock.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbShowBlock.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbShowBlock.FormattingEnabled = true;
            this.cbShowBlock.Location = new System.Drawing.Point(560, 6);
            this.cbShowBlock.Name = "cbShowBlock";
            this.cbShowBlock.Size = new System.Drawing.Size(158, 22);
            this.cbShowBlock.TabIndex = 1;
            this.cbShowBlock.Visible = false;
            this.cbShowBlock.SelectedIndexChanged += new System.EventHandler(this.CbShowBlock_SelectedIndexChanged);
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
            // btnCopyRawSource
            // 
            this.btnCopyRawSource.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCopyRawSource.Location = new System.Drawing.Point(598, 132);
            this.btnCopyRawSource.Name = "btnCopyRawSource";
            this.btnCopyRawSource.Size = new System.Drawing.Size(120, 21);
            this.btnCopyRawSource.TabIndex = 3;
            this.btnCopyRawSource.Text = "Copy Source";
            this.btnCopyRawSource.UseVisualStyleBackColor = true;
            this.btnCopyRawSource.Click += new System.EventHandler(this.BtnCopyRawSource_Click);
            // 
            // rtInfo
            // 
            this.rtInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtInfo.Location = new System.Drawing.Point(6, 3);
            this.rtInfo.Name = "rtInfo";
            this.rtInfo.Size = new System.Drawing.Size(712, 125);
            this.rtInfo.TabIndex = 2;
            this.rtInfo.Text = resources.GetString("rtInfo.Text");
            this.rtInfo.SelectionChanged += new System.EventHandler(this.RtInfo_SelectionChanged);
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
            this.cbOriginalData.CheckedChanged += new System.EventHandler(this.cbOriginalData_CheckedChanged);
            // 
            // openLogFileDialog
            // 
            this.openLogFileDialog.DefaultExt = "log";
            this.openLogFileDialog.Filter = "Log files|*.log;*.txt;*.sqlite|Packet Viewer Log Files|*.log|Packeteer Log Files|" +
    "*.txt|PacketDB Files|*.sqlite|All Files|*.*";
            this.openLogFileDialog.SupportMultiDottedExtensions = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1084, 511);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.MM);
            this.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.MM;
            this.MinimumSize = new System.Drawing.Size(400, 300);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Packet Viewer Log Viewer";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.MM.ResumeLayout(false);
            this.MM.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tcPackets.ResumeLayout(false);
            this.tpPackets1.ResumeLayout(false);
            this.pmPacketList.ResumeLayout(false);
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
        private System.Windows.Forms.ToolStripSeparator mmFileS2;
        private System.Windows.Forms.ToolStripMenuItem mmFileExit;
        private System.Windows.Forms.ToolStripMenuItem mmSearch;
        private System.Windows.Forms.ToolStripMenuItem mmSearchSearch;
        private System.Windows.Forms.ToolStripMenuItem mmSearchNext;
        private System.Windows.Forms.ToolStripMenuItem mmFilter;
        private System.Windows.Forms.ToolStripMenuItem mmFilterEdit;
        private System.Windows.Forms.ToolStripMenuItem mmFilterReset;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem mmFilterApply;
        private System.Windows.Forms.ToolStripSeparator MMFilterApplyItem;
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
        private System.Windows.Forms.CheckBox cbOriginalData;
        private System.Windows.Forms.OpenFileDialog openLogFileDialog;
        private System.Windows.Forms.ToolStripMenuItem mmFileClose;
        private System.Windows.Forms.RichTextBox rtInfo;
        private System.Windows.Forms.TabControl tcPackets;
        private System.Windows.Forms.TabPage tpPackets1;
        private System.Windows.Forms.ToolStripSeparator mmFileS3;
        private System.Windows.Forms.ToolStripMenuItem mmFilePasteNew;
        private System.Windows.Forms.ContextMenuStrip pmPacketList;
        private System.Windows.Forms.ToolStripMenuItem pmPLShowPacketName;
        private System.Windows.Forms.ToolStripSeparator pmPLS1;
        private System.Windows.Forms.ToolStripMenuItem pmPLShowOnly;
        private System.Windows.Forms.ToolStripMenuItem pmPLHideThis;
        private System.Windows.Forms.ToolStripSeparator pmPLS2;
        private System.Windows.Forms.ToolStripMenuItem pmPLShowOutOnly;
        private System.Windows.Forms.ToolStripMenuItem plPLShowInOnly;
        private System.Windows.Forms.ToolStripSeparator pmPLS3;
        private System.Windows.Forms.ToolStripMenuItem pmPLResetFilters;
        private System.Windows.Forms.ToolStripSeparator pmPLS4;
        private System.Windows.Forms.ToolStripMenuItem pmPLEditParser;
        private System.Windows.Forms.ToolStripMenuItem pmPLExportPacket;
        private System.Windows.Forms.Button btnCopyRawSource;
        public System.Windows.Forms.DataGridView dGV;
    }
}

