using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using PacketViewerLogViewer.Packets;

namespace PacketViewerLogViewer
{
    public partial class ParseEditorForm : Form
    {
        string LoadedFile = "";

        public ParseEditorForm()
        {
            InitializeComponent();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            BtnTest_Click(null, null);
            File.WriteAllText(LoadedFile, editBox.Text);
            Dispose();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        public void LoadFromFile(string filename)
        {
            editBox.Text = File.ReadAllText(filename);
            LoadedFile = filename;
            Text += " - " + LoadedFile;
        }

        private void ParseEditorForm_Load(object sender, EventArgs e)
        {

        }

        private void BtnTest_Click(object sender, EventArgs e)
        {
            PacketTabPage tp = MainForm.thisMainForm.GetCurrentPacketTabPage();
            if (tp == null)
            {
                return;
            }

            PacketData pd = tp.GetSelectedPacket();
            if (pd == null)
            {
                return;
            }

            MainForm.thisMainForm.PP.RawParseData.Clear();
            MainForm.thisMainForm.PP.RawParseData.AddRange(editBox.Lines);
            MainForm.thisMainForm.PP.ParsedView.Clear();
            MainForm.thisMainForm.UpdatePacketDetails(tp, pd, "-", true);
        }
    }
}
