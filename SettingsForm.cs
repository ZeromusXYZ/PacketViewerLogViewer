using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PacketViewerLogViewer
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            SaveButtonsIntoColorSettings();
            Properties.Settings.Default.ExternalParseEditor = cbUseExternalEditor.Checked ;
            Properties.Settings.Default.AutoOpenVideoForm = cbAutoOpenVideoForm.Checked;
            if (rbAutoLoadVideoNever.Checked)
                Properties.Settings.Default.AutoLoadVideo = 0;
            if (rbAutoLoadVideoLocalOnly.Checked)
                Properties.Settings.Default.AutoLoadVideo = 1;
            if (rbAutoLoadVideoYoutube.Checked)
                Properties.Settings.Default.AutoLoadVideo = 2;
            if (rbListStyleText.Checked)
                Properties.Settings.Default.PacketListStyle = 0;
            if (rbListStyleSolid.Checked)
                Properties.Settings.Default.PacketListStyle = 1;
            if (rbListStyleTransparent.Checked)
                Properties.Settings.Default.PacketListStyle = 2;
            Properties.Settings.Default.PreParseData = cbPreParseData.Checked;
            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            LoadColorSettingsIntoButtons();
            cbUseExternalEditor.Checked = Properties.Settings.Default.ExternalParseEditor;
            cbAutoOpenVideoForm.Checked = Properties.Settings.Default.AutoOpenVideoForm;
            rbAutoLoadVideoLocalOnly.Checked = (Properties.Settings.Default.AutoLoadVideo == 1);
            rbAutoLoadVideoYoutube.Checked = (Properties.Settings.Default.AutoLoadVideo == 2);
            rbAutoLoadVideoNever.Checked = (!rbAutoLoadVideoLocalOnly.Checked && !rbAutoLoadVideoYoutube.Checked);
            rbListStyleText.Checked = (Properties.Settings.Default.PacketListStyle == 0);
            rbListStyleSolid.Checked = (Properties.Settings.Default.PacketListStyle == 1);
            rbListStyleTransparent.Checked = (Properties.Settings.Default.PacketListStyle == 2);
            cbPreParseData.Checked = Properties.Settings.Default.PreParseData;
        }

        private void LoadColorSettingsIntoButtons()
        {
            // Manual loading
            btnBackIN.BackColor = Properties.Settings.Default.ColBackIN;
            btnBackOUT.BackColor = Properties.Settings.Default.ColBackOUT;
            btnBackUNK.BackColor = Properties.Settings.Default.ColBackUNK;
            btnBarIN.BackColor = Properties.Settings.Default.ColBarIN;
            btnBarOUT.BackColor = Properties.Settings.Default.ColBarOUT;
            btnBarUNK.BackColor = Properties.Settings.Default.ColBarUNK;
            btnFontIN.BackColor = Properties.Settings.Default.ColFontIN;
            btnFontOUT.BackColor = Properties.Settings.Default.ColFontOUT;
            btnFontUNK.BackColor = Properties.Settings.Default.ColFontUNK;
            btnSelectedFontIN.BackColor = Properties.Settings.Default.ColSelectedFontIN;
            btnSelectedFontOUT.BackColor = Properties.Settings.Default.ColSelectedFontOUT;
            btnSelectedFontUNK.BackColor = Properties.Settings.Default.ColSelectedFontUNK;
            btnSelectIN.BackColor = Properties.Settings.Default.ColSelectIN;
            btnSelectOUT.BackColor = Properties.Settings.Default.ColSelectOUT;
            btnSelectUNK.BackColor = Properties.Settings.Default.ColSelectUNK;
            btnSyncIN.BackColor = Properties.Settings.Default.ColSyncIN;
            btnSyncOUT.BackColor = Properties.Settings.Default.ColSyncOUT;
            btnSyncUNK.BackColor = Properties.Settings.Default.ColSyncUNK;
            
        }

        private void SaveButtonsIntoColorSettings()
        {

            // Manual loading
            Properties.Settings.Default.ColBackIN = btnBackIN.BackColor;
            Properties.Settings.Default.ColBackOUT = btnBackOUT.BackColor;
            Properties.Settings.Default.ColBackUNK = btnBackUNK.BackColor;
            Properties.Settings.Default.ColBarIN = btnBarIN.BackColor;
            Properties.Settings.Default.ColBarOUT = btnBarOUT.BackColor;
            Properties.Settings.Default.ColBarUNK = btnBarUNK.BackColor;
            Properties.Settings.Default.ColFontIN = btnFontIN.BackColor;
            Properties.Settings.Default.ColFontOUT = btnFontOUT.BackColor;
            Properties.Settings.Default.ColFontUNK = btnFontUNK.BackColor;
            Properties.Settings.Default.ColSelectedFontIN = btnSelectedFontIN.BackColor;
            Properties.Settings.Default.ColSelectedFontOUT = btnSelectedFontOUT.BackColor;
            Properties.Settings.Default.ColSelectedFontUNK = btnSelectedFontUNK.BackColor;
            Properties.Settings.Default.ColSelectIN = btnSelectIN.BackColor;
            Properties.Settings.Default.ColSelectOUT = btnSelectOUT.BackColor;
            Properties.Settings.Default.ColSelectUNK = btnSelectUNK.BackColor;
            Properties.Settings.Default.ColSyncIN = btnSyncIN.BackColor;
            Properties.Settings.Default.ColSyncOUT = btnSyncOUT.BackColor;
            Properties.Settings.Default.ColSyncUNK = btnSyncUNK.BackColor;
        }

        private void btnDefault_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Reset();
            LoadColorSettingsIntoButtons();
        }

        private void btnColorButton_Click(object sender, EventArgs e)
        {
            var btn = (sender as Button);
            colorDlg.Color = btn.BackColor;
            if (colorDlg.ShowDialog() == DialogResult.OK)
            {
                btn.BackColor = colorDlg.Color;
            }
        }

        private void RbAutoLoadVideoNever_CheckedChanged(object sender, EventArgs e)
        {
            if (rbAutoLoadVideoNever.Checked)
                cbAutoOpenVideoForm.Checked = false;
        }

        private void BtnSetDarkMode_Click(object sender, EventArgs e)
        {
            // Dark Mode suggested colors

            btnBackIN.BackColor = Color.FromArgb(0, 16, 0);
            btnBackOUT.BackColor = Color.FromArgb(0, 0, 16);
            btnBackUNK.BackColor = Color.FromArgb(16, 16, 16);
            btnBarIN.BackColor = Color.FromArgb(200, 255, 200);
            btnBarOUT.BackColor = Color.FromArgb(200, 200, 255);
            btnBarUNK.BackColor = Color.FromArgb(255, 200, 200);
            btnFontIN.BackColor = Color.FromArgb(128, 255, 128);
            btnFontOUT.BackColor = Color.FromArgb(64, 128, 255);
            btnFontUNK.BackColor = Color.FromArgb(192, 64, 64);
            btnSelectedFontIN.BackColor = Color.FromArgb(16, 200, 16);
            btnSelectedFontOUT.BackColor = Color.FromArgb(64, 128, 255);
            btnSelectedFontUNK.BackColor = Color.FromArgb(200, 16, 16);
            btnSelectIN.BackColor = Color.FromArgb(0, 64, 0);
            btnSelectOUT.BackColor = Color.FromArgb(8, 16, 64);
            btnSelectUNK.BackColor = Color.FromArgb(64, 0, 0);
            btnSyncIN.BackColor = Color.FromArgb(16, 92, 16);
            btnSyncOUT.BackColor = Color.FromArgb(16, 16, 92);
            btnSyncUNK.BackColor = Color.FromArgb(92, 16, 16);
        }
    }
}
