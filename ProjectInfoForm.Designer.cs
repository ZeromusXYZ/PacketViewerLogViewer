namespace PacketViewerLogViewer
{
    partial class ProjectInfoForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tProjectFolder = new System.Windows.Forms.TextBox();
            this.tOpenedLog = new System.Windows.Forms.TextBox();
            this.tSourceVideo = new System.Windows.Forms.TextBox();
            this.tYoutubeURL = new System.Windows.Forms.TextBox();
            this.tPackedLogsURL = new System.Windows.Forms.TextBox();
            this.saveProjectDlg = new System.Windows.Forms.SaveFileDialog();
            this.btnDownloadSource = new System.Windows.Forms.Button();
            this.btnDownloadYoutube = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tTagBox = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.tagContainer = new System.Windows.Forms.FlowLayoutPanel();
            this.btnAddTag = new System.Windows.Forms.Button();
            this.lTagLabel = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tagContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Opened Log file";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Project Folder";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 94);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Source Video";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Youtube URL";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 55);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(80, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Download URL";
            // 
            // tProjectFolder
            // 
            this.tProjectFolder.Location = new System.Drawing.Point(20, 32);
            this.tProjectFolder.Name = "tProjectFolder";
            this.tProjectFolder.Size = new System.Drawing.Size(389, 20);
            this.tProjectFolder.TabIndex = 2;
            // 
            // tOpenedLog
            // 
            this.tOpenedLog.Location = new System.Drawing.Point(20, 71);
            this.tOpenedLog.Name = "tOpenedLog";
            this.tOpenedLog.Size = new System.Drawing.Size(389, 20);
            this.tOpenedLog.TabIndex = 3;
            // 
            // tSourceVideo
            // 
            this.tSourceVideo.Location = new System.Drawing.Point(20, 110);
            this.tSourceVideo.Name = "tSourceVideo";
            this.tSourceVideo.Size = new System.Drawing.Size(389, 20);
            this.tSourceVideo.TabIndex = 4;
            // 
            // tYoutubeURL
            // 
            this.tYoutubeURL.Location = new System.Drawing.Point(20, 32);
            this.tYoutubeURL.Name = "tYoutubeURL";
            this.tYoutubeURL.Size = new System.Drawing.Size(389, 20);
            this.tYoutubeURL.TabIndex = 5;
            // 
            // tPackedLogsURL
            // 
            this.tPackedLogsURL.Location = new System.Drawing.Point(20, 71);
            this.tPackedLogsURL.Name = "tPackedLogsURL";
            this.tPackedLogsURL.Size = new System.Drawing.Size(389, 20);
            this.tPackedLogsURL.TabIndex = 6;
            // 
            // saveProjectDlg
            // 
            this.saveProjectDlg.DefaultExt = "pvlv";
            this.saveProjectDlg.Filter = "PVLV Project Files (*.pvlv)|*.pvlv|All files|*.*";
            // 
            // btnDownloadSource
            // 
            this.btnDownloadSource.Location = new System.Drawing.Point(415, 71);
            this.btnDownloadSource.Name = "btnDownloadSource";
            this.btnDownloadSource.Size = new System.Drawing.Size(75, 23);
            this.btnDownloadSource.TabIndex = 11;
            this.btnDownloadSource.Text = "Download";
            this.btnDownloadSource.UseVisualStyleBackColor = true;
            this.btnDownloadSource.Click += new System.EventHandler(this.BtnDownloadSource_Click);
            // 
            // btnDownloadYoutube
            // 
            this.btnDownloadYoutube.Location = new System.Drawing.Point(415, 32);
            this.btnDownloadYoutube.Name = "btnDownloadYoutube";
            this.btnDownloadYoutube.Size = new System.Drawing.Size(75, 23);
            this.btnDownloadYoutube.TabIndex = 12;
            this.btnDownloadYoutube.Text = "Download";
            this.btnDownloadYoutube.UseVisualStyleBackColor = true;
            this.btnDownloadYoutube.Click += new System.EventHandler(this.BtnDownloadYoutube_Click);
            // 
            // button2
            // 
            this.button2.Enabled = false;
            this.button2.Location = new System.Drawing.Point(415, 108);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(144, 23);
            this.button2.TabIndex = 14;
            this.button2.Text = "Upload to Youtube";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.tProjectFolder);
            this.groupBox1.Controls.Add(this.tOpenedLog);
            this.groupBox1.Controls.Add(this.tSourceVideo);
            this.groupBox1.Location = new System.Drawing.Point(12, 129);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(565, 141);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Local Files";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.tYoutubeURL);
            this.groupBox2.Controls.Add(this.btnDownloadSource);
            this.groupBox2.Controls.Add(this.btnDownloadYoutube);
            this.groupBox2.Controls.Add(this.tPackedLogsURL);
            this.groupBox2.Location = new System.Drawing.Point(12, 276);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(565, 110);
            this.groupBox2.TabIndex = 16;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Online Files";
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox3.Controls.Add(this.tTagBox);
            this.groupBox3.Controls.Add(this.btnAddTag);
            this.groupBox3.Controls.Add(this.tagContainer);
            this.groupBox3.Location = new System.Drawing.Point(12, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(565, 111);
            this.groupBox3.TabIndex = 17;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Project Info";
            // 
            // tTagBox
            // 
            this.tTagBox.AutoCompleteCustomSource.AddRange(new string[] {
            "San d\'Oria",
            "Bastok",
            "Windurst",
            "Jeuno",
            "Mhaura",
            "Selbina"});
            this.tTagBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.tTagBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.tTagBox.Location = new System.Drawing.Point(20, 19);
            this.tTagBox.Name = "tTagBox";
            this.tTagBox.Size = new System.Drawing.Size(127, 20);
            this.tTagBox.TabIndex = 0;
            this.tTagBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TTagBox_KeyDown);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(12, 396);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(88, 23);
            this.btnSave.TabIndex = 7;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // tagContainer
            // 
            this.tagContainer.AutoSize = true;
            this.tagContainer.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tagContainer.Controls.Add(this.lTagLabel);
            this.tagContainer.Location = new System.Drawing.Point(6, 47);
            this.tagContainer.Name = "tagContainer";
            this.tagContainer.Size = new System.Drawing.Size(40, 13);
            this.tagContainer.TabIndex = 2;
            // 
            // btnAddTag
            // 
            this.btnAddTag.Location = new System.Drawing.Point(153, 19);
            this.btnAddTag.Name = "btnAddTag";
            this.btnAddTag.Size = new System.Drawing.Size(71, 23);
            this.btnAddTag.TabIndex = 1;
            this.btnAddTag.Text = "Add Tag";
            this.btnAddTag.UseVisualStyleBackColor = true;
            this.btnAddTag.Click += new System.EventHandler(this.BtnAddTag_Click);
            // 
            // lTagLabel
            // 
            this.lTagLabel.AutoSize = true;
            this.lTagLabel.Location = new System.Drawing.Point(3, 0);
            this.lTagLabel.Name = "lTagLabel";
            this.lTagLabel.Size = new System.Drawing.Size(34, 13);
            this.lTagLabel.TabIndex = 0;
            this.lTagLabel.Text = "Tags:";
            this.lTagLabel.Click += new System.EventHandler(this.LTagLabel_Click);
            // 
            // ProjectInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(590, 431);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "ProjectInfoForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Project Information";
            this.Load += new System.EventHandler(this.ProjectInfoForm_Load);
            this.Shown += new System.EventHandler(this.ProjectInfoForm_Shown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.tagContainer.ResumeLayout(false);
            this.tagContainer.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tProjectFolder;
        private System.Windows.Forms.TextBox tOpenedLog;
        private System.Windows.Forms.TextBox tSourceVideo;
        private System.Windows.Forms.TextBox tYoutubeURL;
        private System.Windows.Forms.TextBox tPackedLogsURL;
        private System.Windows.Forms.SaveFileDialog saveProjectDlg;
        private System.Windows.Forms.Button btnDownloadSource;
        private System.Windows.Forms.Button btnDownloadYoutube;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox tTagBox;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.FlowLayoutPanel tagContainer;
        private System.Windows.Forms.Button btnAddTag;
        private System.Windows.Forms.Label lTagLabel;
    }
}