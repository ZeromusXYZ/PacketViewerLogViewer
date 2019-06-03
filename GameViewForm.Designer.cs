namespace PacketViewerLogViewer
{
    partial class GameViewForm
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
            this.gbPlayer = new System.Windows.Forms.GroupBox();
            this.lPlayerName = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnRefreshLookups = new System.Windows.Forms.Button();
            this.lbLookupValues = new System.Windows.Forms.ListBox();
            this.lbLookupGroups = new System.Windows.Forms.ListBox();
            this.cbHexIndex = new System.Windows.Forms.CheckBox();
            this.gbPlayer.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbPlayer
            // 
            this.gbPlayer.Controls.Add(this.lPlayerName);
            this.gbPlayer.Location = new System.Drawing.Point(12, 12);
            this.gbPlayer.Name = "gbPlayer";
            this.gbPlayer.Size = new System.Drawing.Size(129, 46);
            this.gbPlayer.TabIndex = 0;
            this.gbPlayer.TabStop = false;
            this.gbPlayer.Text = "Player 0x00000000";
            // 
            // lPlayerName
            // 
            this.lPlayerName.AutoSize = true;
            this.lPlayerName.Location = new System.Drawing.Point(6, 16);
            this.lPlayerName.Name = "lPlayerName";
            this.lPlayerName.Size = new System.Drawing.Size(64, 13);
            this.lPlayerName.TabIndex = 0;
            this.lPlayerName.Text = "PlayerName";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.cbHexIndex);
            this.groupBox1.Controls.Add(this.btnRefreshLookups);
            this.groupBox1.Controls.Add(this.lbLookupValues);
            this.groupBox1.Controls.Add(this.lbLookupGroups);
            this.groupBox1.Location = new System.Drawing.Point(12, 64);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(484, 304);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Saved Values";
            // 
            // btnRefreshLookups
            // 
            this.btnRefreshLookups.Location = new System.Drawing.Point(9, 20);
            this.btnRefreshLookups.Name = "btnRefreshLookups";
            this.btnRefreshLookups.Size = new System.Drawing.Size(120, 23);
            this.btnRefreshLookups.TabIndex = 2;
            this.btnRefreshLookups.Text = "Refresh Lookups";
            this.btnRefreshLookups.UseVisualStyleBackColor = true;
            this.btnRefreshLookups.Click += new System.EventHandler(this.BtnRefreshLookups_Click);
            // 
            // lbLookupValues
            // 
            this.lbLookupValues.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbLookupValues.FormattingEnabled = true;
            this.lbLookupValues.Location = new System.Drawing.Point(135, 46);
            this.lbLookupValues.Name = "lbLookupValues";
            this.lbLookupValues.Size = new System.Drawing.Size(343, 251);
            this.lbLookupValues.TabIndex = 1;
            // 
            // lbLookupGroups
            // 
            this.lbLookupGroups.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lbLookupGroups.FormattingEnabled = true;
            this.lbLookupGroups.Location = new System.Drawing.Point(9, 45);
            this.lbLookupGroups.Name = "lbLookupGroups";
            this.lbLookupGroups.Size = new System.Drawing.Size(120, 251);
            this.lbLookupGroups.TabIndex = 0;
            this.lbLookupGroups.SelectedIndexChanged += new System.EventHandler(this.LbLookupGroups_SelectedIndexChanged);
            // 
            // cbHexIndex
            // 
            this.cbHexIndex.AutoSize = true;
            this.cbHexIndex.Checked = true;
            this.cbHexIndex.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbHexIndex.Location = new System.Drawing.Point(135, 23);
            this.cbHexIndex.Name = "cbHexIndex";
            this.cbHexIndex.Size = new System.Drawing.Size(118, 17);
            this.cbHexIndex.TabIndex = 3;
            this.cbHexIndex.Text = "Show Index as Hex";
            this.cbHexIndex.UseVisualStyleBackColor = true;
            // 
            // GameViewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(508, 380);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.gbPlayer);
            this.Name = "GameViewForm";
            this.Text = "Game View";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.GameViewForm_FormClosed);
            this.Load += new System.EventHandler(this.GameViewForm_Load);
            this.Shown += new System.EventHandler(this.GameViewForm_Shown);
            this.gbPlayer.ResumeLayout(false);
            this.gbPlayer.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.GroupBox gbPlayer;
        public System.Windows.Forms.Label lPlayerName;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox lbLookupValues;
        private System.Windows.Forms.ListBox lbLookupGroups;
        private System.Windows.Forms.Button btnRefreshLookups;
        private System.Windows.Forms.CheckBox cbHexIndex;
    }
}