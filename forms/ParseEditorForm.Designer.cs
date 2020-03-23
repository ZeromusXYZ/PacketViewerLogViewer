namespace PacketViewerLogViewer
{
    partial class ParseEditorForm
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
            this.editBox = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnTest = new System.Windows.Forms.Button();
            this.btnInsert = new System.Windows.Forms.Button();
            this.cbFieldType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cbLookup = new System.Windows.Forms.ComboBox();
            this.tPos = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tComment = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // editBox
            // 
            this.editBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.editBox.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.editBox.HideSelection = false;
            this.editBox.Location = new System.Drawing.Point(14, 13);
            this.editBox.Multiline = true;
            this.editBox.Name = "editBox";
            this.editBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.editBox.Size = new System.Drawing.Size(759, 410);
            this.editBox.TabIndex = 0;
            this.editBox.WordWrap = false;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(502, 504);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(87, 25);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(595, 504);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(87, 25);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // btnTest
            // 
            this.btnTest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTest.Location = new System.Drawing.Point(688, 479);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(87, 50);
            this.btnTest.TabIndex = 3;
            this.btnTest.Text = "Test";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.BtnTest_Click);
            // 
            // btnInsert
            // 
            this.btnInsert.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnInsert.Location = new System.Drawing.Point(688, 429);
            this.btnInsert.Name = "btnInsert";
            this.btnInsert.Size = new System.Drawing.Size(87, 45);
            this.btnInsert.TabIndex = 4;
            this.btnInsert.Text = "Insert Line";
            this.btnInsert.UseVisualStyleBackColor = true;
            this.btnInsert.Click += new System.EventHandler(this.BtnInsert_Click);
            // 
            // cbFieldType
            // 
            this.cbFieldType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbFieldType.FormattingEnabled = true;
            this.cbFieldType.Items.AddRange(new object[] {
            "uint32",
            "int32",
            "uint16",
            "int16",
            "byte",
            "float",
            "pos",
            "dir",
            "switchblock",
            "showblock",
            "info",
            "bit",
            "bits",
            "string",
            "data",
            "ms",
            "frames",
            "vanatime",
            "ip4",
            "linkshellstring",
            "inscribestring",
            "bitflaglist",
            "bitflaglist2",
            "combatskill",
            "craftskill",
            "equipsetitem",
            "equipsetitemlist",
            "abilityrecastlist",
            "blacklistentry",
            "meritentries",
            "playercheckitems",
            "bufficons",
            "bufftimers",
            "buffs",
            "jobpointentries",
            "shopitems",
            "guildshopitems",
            "jobpoints",
            "roequest",
            "packet-in-0x028"});
            this.cbFieldType.Location = new System.Drawing.Point(11, 452);
            this.cbFieldType.Name = "cbFieldType";
            this.cbFieldType.Size = new System.Drawing.Size(115, 22);
            this.cbFieldType.TabIndex = 5;
            this.cbFieldType.Text = "byte";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 435);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 14);
            this.label1.TabIndex = 6;
            this.label1.Text = "Type";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 480);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 14);
            this.label2.TabIndex = 8;
            this.label2.Text = "Lookup";
            // 
            // cbLookup
            // 
            this.cbLookup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbLookup.FormattingEnabled = true;
            this.cbLookup.Location = new System.Drawing.Point(12, 497);
            this.cbLookup.Name = "cbLookup";
            this.cbLookup.Size = new System.Drawing.Size(115, 22);
            this.cbLookup.TabIndex = 7;
            // 
            // tPos
            // 
            this.tPos.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.tPos.Location = new System.Drawing.Point(135, 452);
            this.tPos.Name = "tPos";
            this.tPos.Size = new System.Drawing.Size(105, 22);
            this.tPos.TabIndex = 9;
            this.tPos.Text = "0x04";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(132, 435);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 14);
            this.label3.TabIndex = 10;
            this.label3.Text = "Position";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(243, 435);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(91, 14);
            this.label4.TabIndex = 12;
            this.label4.Text = "Display Name";
            // 
            // tName
            // 
            this.tName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.tName.Location = new System.Drawing.Point(246, 452);
            this.tName.Name = "tName";
            this.tName.Size = new System.Drawing.Size(130, 22);
            this.tName.TabIndex = 11;
            this.tName.Text = "MyField";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(379, 435);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(231, 14);
            this.label5.TabIndex = 14;
            this.label5.Text = "Comment (or Compare switchblock)";
            // 
            // tComment
            // 
            this.tComment.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tComment.Location = new System.Drawing.Point(382, 452);
            this.tComment.Name = "tComment";
            this.tComment.Size = new System.Drawing.Size(300, 22);
            this.tComment.TabIndex = 13;
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(132, 500);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(259, 14);
            this.label6.TabIndex = 15;
            this.label6.Text = "Leave Lookup empty for normal values";
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(132, 477);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(182, 14);
            this.label7.TabIndex = 16;
            this.label7.Text = "byte[:startbit[-bitsize]]";
            // 
            // ParseEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(787, 534);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tComment);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tPos);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbLookup);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbFieldType);
            this.Controls.Add(this.btnInsert);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.editBox);
            this.Font = new System.Drawing.Font("Consolas", 9F);
            this.Name = "ParseEditorForm";
            this.Text = "Parser Editor";
            this.Load += new System.EventHandler(this.ParseEditorForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox editBox;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Button btnInsert;
        private System.Windows.Forms.ComboBox cbFieldType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbLookup;
        private System.Windows.Forms.TextBox tPos;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tComment;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
    }
}