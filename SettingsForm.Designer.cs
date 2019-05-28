namespace PacketViewerLogViewer
{
    partial class SettingsForm
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
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.colorDlg = new System.Windows.Forms.ColorDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label10 = new System.Windows.Forms.Label();
            this.btnFontIN = new System.Windows.Forms.Button();
            this.btnBackIN = new System.Windows.Forms.Button();
            this.btnSyncIN = new System.Windows.Forms.Button();
            this.btnSelectIN = new System.Windows.Forms.Button();
            this.btnFontOUT = new System.Windows.Forms.Button();
            this.btnBackOUT = new System.Windows.Forms.Button();
            this.btnSyncOUT = new System.Windows.Forms.Button();
            this.btnSelectOUT = new System.Windows.Forms.Button();
            this.btnFontUNK = new System.Windows.Forms.Button();
            this.btnBackUNK = new System.Windows.Forms.Button();
            this.btnSyncUNK = new System.Windows.Forms.Button();
            this.btnSelectUNK = new System.Windows.Forms.Button();
            this.btnBarIN = new System.Windows.Forms.Button();
            this.btnBarOUT = new System.Windows.Forms.Button();
            this.btnBarUNK = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.btnSelectedFontIN = new System.Windows.Forms.Button();
            this.btnSelectedFontOUT = new System.Windows.Forms.Button();
            this.btnSelectedFontUNK = new System.Windows.Forms.Button();
            this.btnDefault = new System.Windows.Forms.Button();
            this.cbUseExternalEditor = new System.Windows.Forms.CheckBox();
            this.gbAutoLoadVideo = new System.Windows.Forms.GroupBox();
            this.cbAutoOpenVideoForm = new System.Windows.Forms.CheckBox();
            this.rbAutoLoadVideoYoutube = new System.Windows.Forms.RadioButton();
            this.rbAutoLoadVideoLocalOnly = new System.Windows.Forms.RadioButton();
            this.rbAutoLoadVideoNever = new System.Windows.Forms.RadioButton();
            this.gbListStyle = new System.Windows.Forms.GroupBox();
            this.label12 = new System.Windows.Forms.Label();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.rbListStyleTransparent = new System.Windows.Forms.RadioButton();
            this.rbListStyleSolid = new System.Windows.Forms.RadioButton();
            this.rbListStyleText = new System.Windows.Forms.RadioButton();
            this.cbPreParseData = new System.Windows.Forms.CheckBox();
            this.btnSetDarkMode = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.gbAutoLoadVideo.SuspendLayout();
            this.gbListStyle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOK.Location = new System.Drawing.Point(12, 359);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(93, 359);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // colorDlg
            // 
            this.colorDlg.AnyColor = true;
            this.colorDlg.FullOpen = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "IN packet";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 78);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "OUT Packet";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 116);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "?? Packet";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(155, 2);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Background";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(230, 2);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(43, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Synced";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(305, 2);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 26);
            this.label6.TabIndex = 7;
            this.label6.Text = "Selected Background";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(455, 2);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(50, 26);
            this.label7.TabIndex = 8;
            this.label7.Text = "SyncBar color";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(80, 2);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(28, 13);
            this.label8.TabIndex = 9;
            this.label8.Text = "Font";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Inset;
            this.tableLayoutPanel1.ColumnCount = 7;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.Controls.Add(this.label10, 5, 0);
            this.tableLayoutPanel1.Controls.Add(this.label8, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label6, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label5, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label4, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnFontIN, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnBackIN, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnSyncIN, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnSelectIN, 4, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnFontOUT, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnBackOUT, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnSyncOUT, 3, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnSelectOUT, 4, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnFontUNK, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.btnBackUNK, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.btnSyncUNK, 3, 3);
            this.tableLayoutPanel1.Controls.Add(this.btnSelectUNK, 4, 3);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label7, 6, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnBarIN, 6, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnBarOUT, 6, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnBarUNK, 6, 3);
            this.tableLayoutPanel1.Controls.Add(this.label9, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnSelectedFontIN, 5, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnSelectedFontOUT, 5, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnSelectedFontUNK, 5, 3);
            this.tableLayoutPanel1.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(6, 19);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(532, 155);
            this.tableLayoutPanel1.TabIndex = 10;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(380, 2);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(52, 26);
            this.label10.TabIndex = 29;
            this.label10.Text = "Selected Text";
            // 
            // btnFontIN
            // 
            this.btnFontIN.AutoSize = true;
            this.btnFontIN.BackColor = System.Drawing.Color.Lime;
            this.btnFontIN.Location = new System.Drawing.Point(80, 43);
            this.btnFontIN.Name = "btnFontIN";
            this.btnFontIN.Size = new System.Drawing.Size(63, 28);
            this.btnFontIN.TabIndex = 10;
            this.btnFontIN.Tag = "1";
            this.btnFontIN.UseVisualStyleBackColor = false;
            this.btnFontIN.Click += new System.EventHandler(this.btnColorButton_Click);
            // 
            // btnBackIN
            // 
            this.btnBackIN.AutoSize = true;
            this.btnBackIN.BackColor = System.Drawing.Color.Lime;
            this.btnBackIN.Location = new System.Drawing.Point(155, 43);
            this.btnBackIN.Name = "btnBackIN";
            this.btnBackIN.Size = new System.Drawing.Size(63, 28);
            this.btnBackIN.TabIndex = 11;
            this.btnBackIN.Tag = "1";
            this.btnBackIN.UseVisualStyleBackColor = false;
            this.btnBackIN.Click += new System.EventHandler(this.btnColorButton_Click);
            // 
            // btnSyncIN
            // 
            this.btnSyncIN.AutoSize = true;
            this.btnSyncIN.BackColor = System.Drawing.Color.Lime;
            this.btnSyncIN.Location = new System.Drawing.Point(230, 43);
            this.btnSyncIN.Name = "btnSyncIN";
            this.btnSyncIN.Size = new System.Drawing.Size(63, 28);
            this.btnSyncIN.TabIndex = 12;
            this.btnSyncIN.Tag = "1";
            this.btnSyncIN.UseVisualStyleBackColor = false;
            this.btnSyncIN.Click += new System.EventHandler(this.btnColorButton_Click);
            // 
            // btnSelectIN
            // 
            this.btnSelectIN.AutoSize = true;
            this.btnSelectIN.BackColor = System.Drawing.Color.Lime;
            this.btnSelectIN.Location = new System.Drawing.Point(305, 43);
            this.btnSelectIN.Name = "btnSelectIN";
            this.btnSelectIN.Size = new System.Drawing.Size(63, 28);
            this.btnSelectIN.TabIndex = 13;
            this.btnSelectIN.Tag = "1";
            this.btnSelectIN.UseVisualStyleBackColor = false;
            this.btnSelectIN.Click += new System.EventHandler(this.btnColorButton_Click);
            // 
            // btnFontOUT
            // 
            this.btnFontOUT.AutoSize = true;
            this.btnFontOUT.BackColor = System.Drawing.Color.Blue;
            this.btnFontOUT.Location = new System.Drawing.Point(80, 81);
            this.btnFontOUT.Name = "btnFontOUT";
            this.btnFontOUT.Size = new System.Drawing.Size(63, 28);
            this.btnFontOUT.TabIndex = 15;
            this.btnFontOUT.Tag = "1";
            this.btnFontOUT.UseVisualStyleBackColor = false;
            this.btnFontOUT.Click += new System.EventHandler(this.btnColorButton_Click);
            // 
            // btnBackOUT
            // 
            this.btnBackOUT.AutoSize = true;
            this.btnBackOUT.BackColor = System.Drawing.Color.Blue;
            this.btnBackOUT.Location = new System.Drawing.Point(155, 81);
            this.btnBackOUT.Name = "btnBackOUT";
            this.btnBackOUT.Size = new System.Drawing.Size(63, 28);
            this.btnBackOUT.TabIndex = 16;
            this.btnBackOUT.Tag = "1";
            this.btnBackOUT.UseVisualStyleBackColor = false;
            this.btnBackOUT.Click += new System.EventHandler(this.btnColorButton_Click);
            // 
            // btnSyncOUT
            // 
            this.btnSyncOUT.AutoSize = true;
            this.btnSyncOUT.BackColor = System.Drawing.Color.Blue;
            this.btnSyncOUT.Location = new System.Drawing.Point(230, 81);
            this.btnSyncOUT.Name = "btnSyncOUT";
            this.btnSyncOUT.Size = new System.Drawing.Size(63, 28);
            this.btnSyncOUT.TabIndex = 17;
            this.btnSyncOUT.Tag = "1";
            this.btnSyncOUT.UseVisualStyleBackColor = false;
            this.btnSyncOUT.Click += new System.EventHandler(this.btnColorButton_Click);
            // 
            // btnSelectOUT
            // 
            this.btnSelectOUT.AutoSize = true;
            this.btnSelectOUT.BackColor = System.Drawing.Color.Blue;
            this.btnSelectOUT.Location = new System.Drawing.Point(305, 81);
            this.btnSelectOUT.Name = "btnSelectOUT";
            this.btnSelectOUT.Size = new System.Drawing.Size(63, 28);
            this.btnSelectOUT.TabIndex = 18;
            this.btnSelectOUT.Tag = "1";
            this.btnSelectOUT.UseVisualStyleBackColor = false;
            this.btnSelectOUT.Click += new System.EventHandler(this.btnColorButton_Click);
            // 
            // btnFontUNK
            // 
            this.btnFontUNK.AutoSize = true;
            this.btnFontUNK.BackColor = System.Drawing.Color.Black;
            this.btnFontUNK.Location = new System.Drawing.Point(80, 119);
            this.btnFontUNK.Name = "btnFontUNK";
            this.btnFontUNK.Size = new System.Drawing.Size(63, 28);
            this.btnFontUNK.TabIndex = 20;
            this.btnFontUNK.Tag = "1";
            this.btnFontUNK.UseVisualStyleBackColor = false;
            this.btnFontUNK.Click += new System.EventHandler(this.btnColorButton_Click);
            // 
            // btnBackUNK
            // 
            this.btnBackUNK.AutoSize = true;
            this.btnBackUNK.BackColor = System.Drawing.Color.White;
            this.btnBackUNK.Location = new System.Drawing.Point(155, 119);
            this.btnBackUNK.Name = "btnBackUNK";
            this.btnBackUNK.Size = new System.Drawing.Size(63, 28);
            this.btnBackUNK.TabIndex = 21;
            this.btnBackUNK.Tag = "1";
            this.btnBackUNK.UseVisualStyleBackColor = false;
            this.btnBackUNK.Click += new System.EventHandler(this.btnColorButton_Click);
            // 
            // btnSyncUNK
            // 
            this.btnSyncUNK.AutoSize = true;
            this.btnSyncUNK.BackColor = System.Drawing.Color.White;
            this.btnSyncUNK.Location = new System.Drawing.Point(230, 119);
            this.btnSyncUNK.Name = "btnSyncUNK";
            this.btnSyncUNK.Size = new System.Drawing.Size(63, 28);
            this.btnSyncUNK.TabIndex = 22;
            this.btnSyncUNK.Tag = "1";
            this.btnSyncUNK.UseVisualStyleBackColor = false;
            this.btnSyncUNK.Click += new System.EventHandler(this.btnColorButton_Click);
            // 
            // btnSelectUNK
            // 
            this.btnSelectUNK.AutoSize = true;
            this.btnSelectUNK.BackColor = System.Drawing.Color.White;
            this.btnSelectUNK.Location = new System.Drawing.Point(305, 119);
            this.btnSelectUNK.Name = "btnSelectUNK";
            this.btnSelectUNK.Size = new System.Drawing.Size(63, 28);
            this.btnSelectUNK.TabIndex = 23;
            this.btnSelectUNK.Tag = "1";
            this.btnSelectUNK.UseVisualStyleBackColor = false;
            this.btnSelectUNK.Click += new System.EventHandler(this.btnColorButton_Click);
            // 
            // btnBarIN
            // 
            this.btnBarIN.AutoSize = true;
            this.btnBarIN.BackColor = System.Drawing.Color.Lime;
            this.btnBarIN.Location = new System.Drawing.Point(455, 43);
            this.btnBarIN.Name = "btnBarIN";
            this.btnBarIN.Size = new System.Drawing.Size(63, 28);
            this.btnBarIN.TabIndex = 14;
            this.btnBarIN.Tag = "1";
            this.btnBarIN.UseVisualStyleBackColor = false;
            this.btnBarIN.Click += new System.EventHandler(this.btnColorButton_Click);
            // 
            // btnBarOUT
            // 
            this.btnBarOUT.AutoSize = true;
            this.btnBarOUT.BackColor = System.Drawing.Color.Blue;
            this.btnBarOUT.Location = new System.Drawing.Point(455, 81);
            this.btnBarOUT.Name = "btnBarOUT";
            this.btnBarOUT.Size = new System.Drawing.Size(63, 28);
            this.btnBarOUT.TabIndex = 19;
            this.btnBarOUT.Tag = "1";
            this.btnBarOUT.UseVisualStyleBackColor = false;
            this.btnBarOUT.Click += new System.EventHandler(this.btnColorButton_Click);
            // 
            // btnBarUNK
            // 
            this.btnBarUNK.AutoSize = true;
            this.btnBarUNK.BackColor = System.Drawing.Color.White;
            this.btnBarUNK.Location = new System.Drawing.Point(455, 119);
            this.btnBarUNK.Name = "btnBarUNK";
            this.btnBarUNK.Size = new System.Drawing.Size(63, 28);
            this.btnBarUNK.TabIndex = 24;
            this.btnBarUNK.Tag = "1";
            this.btnBarUNK.UseVisualStyleBackColor = false;
            this.btnBarUNK.Click += new System.EventHandler(this.btnColorButton_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(5, 2);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(36, 13);
            this.label9.TabIndex = 25;
            this.label9.Text = "Colors";
            // 
            // btnSelectedFontIN
            // 
            this.btnSelectedFontIN.AutoSize = true;
            this.btnSelectedFontIN.BackColor = System.Drawing.Color.Lime;
            this.btnSelectedFontIN.Location = new System.Drawing.Point(380, 43);
            this.btnSelectedFontIN.Name = "btnSelectedFontIN";
            this.btnSelectedFontIN.Size = new System.Drawing.Size(63, 28);
            this.btnSelectedFontIN.TabIndex = 26;
            this.btnSelectedFontIN.Tag = "1";
            this.btnSelectedFontIN.UseVisualStyleBackColor = false;
            this.btnSelectedFontIN.Click += new System.EventHandler(this.btnColorButton_Click);
            // 
            // btnSelectedFontOUT
            // 
            this.btnSelectedFontOUT.AutoSize = true;
            this.btnSelectedFontOUT.BackColor = System.Drawing.Color.Blue;
            this.btnSelectedFontOUT.Location = new System.Drawing.Point(380, 81);
            this.btnSelectedFontOUT.Name = "btnSelectedFontOUT";
            this.btnSelectedFontOUT.Size = new System.Drawing.Size(63, 28);
            this.btnSelectedFontOUT.TabIndex = 27;
            this.btnSelectedFontOUT.Tag = "1";
            this.btnSelectedFontOUT.UseVisualStyleBackColor = false;
            this.btnSelectedFontOUT.Click += new System.EventHandler(this.btnColorButton_Click);
            // 
            // btnSelectedFontUNK
            // 
            this.btnSelectedFontUNK.AutoSize = true;
            this.btnSelectedFontUNK.BackColor = System.Drawing.Color.White;
            this.btnSelectedFontUNK.Location = new System.Drawing.Point(380, 119);
            this.btnSelectedFontUNK.Name = "btnSelectedFontUNK";
            this.btnSelectedFontUNK.Size = new System.Drawing.Size(63, 28);
            this.btnSelectedFontUNK.TabIndex = 28;
            this.btnSelectedFontUNK.Tag = "1";
            this.btnSelectedFontUNK.UseVisualStyleBackColor = false;
            this.btnSelectedFontUNK.Click += new System.EventHandler(this.btnColorButton_Click);
            // 
            // btnDefault
            // 
            this.btnDefault.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDefault.Location = new System.Drawing.Point(190, 359);
            this.btnDefault.Name = "btnDefault";
            this.btnDefault.Size = new System.Drawing.Size(75, 23);
            this.btnDefault.TabIndex = 11;
            this.btnDefault.Text = "Default";
            this.btnDefault.UseVisualStyleBackColor = true;
            this.btnDefault.Click += new System.EventHandler(this.btnDefault_Click);
            // 
            // cbUseExternalEditor
            // 
            this.cbUseExternalEditor.AutoSize = true;
            this.cbUseExternalEditor.Location = new System.Drawing.Point(18, 301);
            this.cbUseExternalEditor.Name = "cbUseExternalEditor";
            this.cbUseExternalEditor.Size = new System.Drawing.Size(239, 17);
            this.cbUseExternalEditor.TabIndex = 12;
            this.cbUseExternalEditor.Text = "Use External Text Editor for editing parse files";
            this.cbUseExternalEditor.UseVisualStyleBackColor = true;
            // 
            // gbAutoLoadVideo
            // 
            this.gbAutoLoadVideo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbAutoLoadVideo.Controls.Add(this.cbAutoOpenVideoForm);
            this.gbAutoLoadVideo.Controls.Add(this.rbAutoLoadVideoYoutube);
            this.gbAutoLoadVideo.Controls.Add(this.rbAutoLoadVideoLocalOnly);
            this.gbAutoLoadVideo.Controls.Add(this.rbAutoLoadVideoNever);
            this.gbAutoLoadVideo.Location = new System.Drawing.Point(12, 246);
            this.gbAutoLoadVideo.Name = "gbAutoLoadVideo";
            this.gbAutoLoadVideo.Size = new System.Drawing.Size(544, 49);
            this.gbAutoLoadVideo.TabIndex = 13;
            this.gbAutoLoadVideo.TabStop = false;
            this.gbAutoLoadVideo.Text = "Auto-Load Video";
            // 
            // cbAutoOpenVideoForm
            // 
            this.cbAutoOpenVideoForm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbAutoOpenVideoForm.AutoSize = true;
            this.cbAutoOpenVideoForm.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbAutoOpenVideoForm.Location = new System.Drawing.Point(357, 20);
            this.cbAutoOpenVideoForm.Name = "cbAutoOpenVideoForm";
            this.cbAutoOpenVideoForm.Size = new System.Drawing.Size(181, 17);
            this.cbAutoOpenVideoForm.TabIndex = 3;
            this.cbAutoOpenVideoForm.Text = "Auto-open video window on load";
            this.cbAutoOpenVideoForm.UseVisualStyleBackColor = true;
            // 
            // rbAutoLoadVideoYoutube
            // 
            this.rbAutoLoadVideoYoutube.AutoSize = true;
            this.rbAutoLoadVideoYoutube.Location = new System.Drawing.Point(184, 19);
            this.rbAutoLoadVideoYoutube.Name = "rbAutoLoadVideoYoutube";
            this.rbAutoLoadVideoYoutube.Size = new System.Drawing.Size(126, 17);
            this.rbAutoLoadVideoYoutube.TabIndex = 2;
            this.rbAutoLoadVideoYoutube.Text = "also for Youtube links";
            this.rbAutoLoadVideoYoutube.UseVisualStyleBackColor = true;
            // 
            // rbAutoLoadVideoLocalOnly
            // 
            this.rbAutoLoadVideoLocalOnly.AutoSize = true;
            this.rbAutoLoadVideoLocalOnly.Checked = true;
            this.rbAutoLoadVideoLocalOnly.Location = new System.Drawing.Point(66, 20);
            this.rbAutoLoadVideoLocalOnly.Name = "rbAutoLoadVideoLocalOnly";
            this.rbAutoLoadVideoLocalOnly.Size = new System.Drawing.Size(112, 17);
            this.rbAutoLoadVideoLocalOnly.TabIndex = 1;
            this.rbAutoLoadVideoLocalOnly.TabStop = true;
            this.rbAutoLoadVideoLocalOnly.Text = "for Local Files only";
            this.rbAutoLoadVideoLocalOnly.UseVisualStyleBackColor = true;
            // 
            // rbAutoLoadVideoNever
            // 
            this.rbAutoLoadVideoNever.AutoSize = true;
            this.rbAutoLoadVideoNever.Location = new System.Drawing.Point(6, 19);
            this.rbAutoLoadVideoNever.Name = "rbAutoLoadVideoNever";
            this.rbAutoLoadVideoNever.Size = new System.Drawing.Size(54, 17);
            this.rbAutoLoadVideoNever.TabIndex = 0;
            this.rbAutoLoadVideoNever.Text = "Never";
            this.rbAutoLoadVideoNever.UseVisualStyleBackColor = true;
            this.rbAutoLoadVideoNever.CheckedChanged += new System.EventHandler(this.RbAutoLoadVideoNever_CheckedChanged);
            // 
            // gbListStyle
            // 
            this.gbListStyle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbListStyle.Controls.Add(this.label12);
            this.gbListStyle.Controls.Add(this.pictureBox3);
            this.gbListStyle.Controls.Add(this.pictureBox4);
            this.gbListStyle.Controls.Add(this.pictureBox2);
            this.gbListStyle.Controls.Add(this.pictureBox1);
            this.gbListStyle.Controls.Add(this.tableLayoutPanel1);
            this.gbListStyle.Controls.Add(this.rbListStyleTransparent);
            this.gbListStyle.Controls.Add(this.rbListStyleSolid);
            this.gbListStyle.Controls.Add(this.rbListStyleText);
            this.gbListStyle.Location = new System.Drawing.Point(12, 12);
            this.gbListStyle.Name = "gbListStyle";
            this.gbListStyle.Size = new System.Drawing.Size(544, 228);
            this.gbListStyle.TabIndex = 14;
            this.gbListStyle.TabStop = false;
            this.gbListStyle.Text = "Packet List Style";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(3, 187);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(60, 13);
            this.label12.TabIndex = 12;
            this.label12.Text = "Arrow Style";
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = global::PacketViewerLogViewer.Properties.Resources.mini_out_ticon;
            this.pictureBox3.Location = new System.Drawing.Point(458, 208);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(12, 12);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox3.TabIndex = 6;
            this.pictureBox3.TabStop = false;
            // 
            // pictureBox4
            // 
            this.pictureBox4.Image = global::PacketViewerLogViewer.Properties.Resources.mini_in_ticon;
            this.pictureBox4.Location = new System.Drawing.Point(440, 208);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(12, 12);
            this.pictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox4.TabIndex = 5;
            this.pictureBox4.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::PacketViewerLogViewer.Properties.Resources.mini_out_icon;
            this.pictureBox2.Location = new System.Drawing.Point(270, 208);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(12, 12);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox2.TabIndex = 4;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::PacketViewerLogViewer.Properties.Resources.mini_in_icon;
            this.pictureBox1.Location = new System.Drawing.Point(252, 208);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(12, 12);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // rbListStyleTransparent
            // 
            this.rbListStyleTransparent.AutoSize = true;
            this.rbListStyleTransparent.Location = new System.Drawing.Point(317, 203);
            this.rbListStyleTransparent.Name = "rbListStyleTransparent";
            this.rbListStyleTransparent.Size = new System.Drawing.Size(117, 17);
            this.rbListStyleTransparent.TabIndex = 2;
            this.rbListStyleTransparent.Text = "Transparent Arrows";
            this.rbListStyleTransparent.UseVisualStyleBackColor = true;
            // 
            // rbListStyleSolid
            // 
            this.rbListStyleSolid.AutoSize = true;
            this.rbListStyleSolid.Checked = true;
            this.rbListStyleSolid.Location = new System.Drawing.Point(163, 203);
            this.rbListStyleSolid.Name = "rbListStyleSolid";
            this.rbListStyleSolid.Size = new System.Drawing.Size(83, 17);
            this.rbListStyleSolid.TabIndex = 1;
            this.rbListStyleSolid.TabStop = true;
            this.rbListStyleSolid.Text = "Solid Arrows";
            this.rbListStyleSolid.UseVisualStyleBackColor = true;
            // 
            // rbListStyleText
            // 
            this.rbListStyleText.AutoSize = true;
            this.rbListStyleText.Location = new System.Drawing.Point(6, 203);
            this.rbListStyleText.Name = "rbListStyleText";
            this.rbListStyleText.Size = new System.Drawing.Size(117, 17);
            this.rbListStyleText.TabIndex = 0;
            this.rbListStyleText.Text = "Text Arrows  <=  =>";
            this.rbListStyleText.UseVisualStyleBackColor = true;
            // 
            // cbPreParseData
            // 
            this.cbPreParseData.AutoSize = true;
            this.cbPreParseData.Location = new System.Drawing.Point(18, 324);
            this.cbPreParseData.Name = "cbPreParseData";
            this.cbPreParseData.Size = new System.Drawing.Size(404, 17);
            this.cbPreParseData.TabIndex = 15;
            this.cbPreParseData.Text = "Pre-Parse loaded packet data (loads slower but allows additional search options)";
            this.cbPreParseData.UseVisualStyleBackColor = true;
            // 
            // btnSetDarkMode
            // 
            this.btnSetDarkMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSetDarkMode.Location = new System.Drawing.Point(326, 359);
            this.btnSetDarkMode.Name = "btnSetDarkMode";
            this.btnSetDarkMode.Size = new System.Drawing.Size(224, 23);
            this.btnSetDarkMode.TabIndex = 16;
            this.btnSetDarkMode.Text = "Set Dark Mode Packet Colors";
            this.btnSetDarkMode.UseVisualStyleBackColor = true;
            this.btnSetDarkMode.Click += new System.EventHandler(this.BtnSetDarkMode_Click);
            // 
            // SettingsForm
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(568, 394);
            this.ControlBox = false;
            this.Controls.Add(this.btnSetDarkMode);
            this.Controls.Add(this.cbPreParseData);
            this.Controls.Add(this.gbListStyle);
            this.Controls.Add(this.gbAutoLoadVideo);
            this.Controls.Add(this.cbUseExternalEditor);
            this.Controls.Add(this.btnDefault);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.SettingsForm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.gbAutoLoadVideo.ResumeLayout(false);
            this.gbAutoLoadVideo.PerformLayout();
            this.gbListStyle.ResumeLayout(false);
            this.gbListStyle.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ColorDialog colorDlg;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnFontIN;
        private System.Windows.Forms.Button btnBackIN;
        private System.Windows.Forms.Button btnSyncIN;
        private System.Windows.Forms.Button btnSelectIN;
        private System.Windows.Forms.Button btnBarIN;
        private System.Windows.Forms.Button btnFontOUT;
        private System.Windows.Forms.Button btnBackOUT;
        private System.Windows.Forms.Button btnSyncOUT;
        private System.Windows.Forms.Button btnSelectOUT;
        private System.Windows.Forms.Button btnBarOUT;
        private System.Windows.Forms.Button btnFontUNK;
        private System.Windows.Forms.Button btnBackUNK;
        private System.Windows.Forms.Button btnSyncUNK;
        private System.Windows.Forms.Button btnSelectUNK;
        private System.Windows.Forms.Button btnBarUNK;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnSelectedFontIN;
        private System.Windows.Forms.Button btnSelectedFontOUT;
        private System.Windows.Forms.Button btnSelectedFontUNK;
        private System.Windows.Forms.Button btnDefault;
        private System.Windows.Forms.CheckBox cbUseExternalEditor;
        private System.Windows.Forms.GroupBox gbAutoLoadVideo;
        private System.Windows.Forms.CheckBox cbAutoOpenVideoForm;
        private System.Windows.Forms.RadioButton rbAutoLoadVideoYoutube;
        private System.Windows.Forms.RadioButton rbAutoLoadVideoLocalOnly;
        private System.Windows.Forms.RadioButton rbAutoLoadVideoNever;
        private System.Windows.Forms.GroupBox gbListStyle;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.RadioButton rbListStyleTransparent;
        private System.Windows.Forms.RadioButton rbListStyleSolid;
        private System.Windows.Forms.RadioButton rbListStyleText;
        private System.Windows.Forms.CheckBox cbPreParseData;
        private System.Windows.Forms.Button btnSetDarkMode;
    }
}