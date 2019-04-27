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
            editBox.SelectionLength = 0;
            editBox.SelectionStart = editBox.Text.Length;

        }

        private void ParseEditorForm_Load(object sender, EventArgs e)
        {
            cbLookup.Items.Clear();
            cbLookup.Items.Add("");
            foreach (var item in DataLookups.LookupLists)
            {
                cbLookup.Items.Add(item.Key);
            }
            cbFieldType.Sorted = true;
            cbFieldType.Text = "uint16";
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

        private void BtnInsert_Click(object sender, EventArgs e)
        {
            string s = "";
            s += cbFieldType.Text;
            if (cbLookup.Text != string.Empty)
                s += ":" + cbLookup.Text;
            s += ";" + tPos.Text;
            s += ";" + tName.Text;
            if (tComment.Text != string.Empty)
                s += ";" + tComment.Text;
            s += "\r\n";
            var p = editBox.SelectionStart;
            editBox.SelectedText = s;
            editBox.SelectionStart = p;
            editBox.SelectionLength = s.Length;
        }
    }
}
