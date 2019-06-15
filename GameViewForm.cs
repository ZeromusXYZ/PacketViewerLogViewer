using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PacketViewerLogViewer.ClipboardHelper;
using PacketViewerLogViewer.Packets;

namespace PacketViewerLogViewer
{
    public partial class GameViewForm : Form
    {
        public static GameViewForm GV = null;
        private DataLookupList LastLookupList = null;

        public GameViewForm()
        {
            InitializeComponent();
            GameViewForm.GV = this;
        }

        private void GameViewForm_Load(object sender, EventArgs e)
        {
        }

        private void GameViewForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            GV = null;
        }

        private void BtnRefreshLookups_Click(object sender, EventArgs e)
        {
            lbLookupValues.Items.Clear();
            lbLookupGroups.Items.Clear();
            lbLookupGroups.Items.AddRange(DataLookups.LookupLists.Keys.ToArray());
            lbLookupGroups.Sorted = true;
        }

        private void LbLookupGroups_SelectedIndexChanged(object sender, EventArgs e)
        {
            var item = lbLookupGroups.SelectedItem;
            if (item == null)
            {
                LastLookupList = null;
                return;
            }
            lbLookupValues.Items.Clear();
            lbLookupValues.BeginUpdate();
            LastLookupList = DataLookups.NLU((string)item);
            foreach (var d in LastLookupList.data)
            {
                if (cbHexIndex.Checked)
                    lbLookupValues.Items.Add("0x" + d.Value.ID.ToString("X8") + " => " + d.Value.Val);
                else
                    lbLookupValues.Items.Add(d.Value.ID.ToString() + " => " + d.Value.Val);
            }
            lbLookupValues.EndUpdate();
        }

        private void GameViewForm_Shown(object sender, EventArgs e)
        {
            BtnRefreshLookups_Click(null, null);
        }

        private void SendToClipBoard(string cliptext)
        {
            try
            {
                // Because nothing is ever as simple as the next line >.>
                // Clipboard.SetText(s);
                // Helper will (try to) prevent errors when copying to clipboard because of threading issues
                var cliphelp = new SetClipboardHelper(DataFormats.Text, cliptext);
                cliphelp.DontRetryWorkOnFailed = false;
                cliphelp.Go();
            }
            catch
            {
            }
        }

        private void BtnCopyID_Click(object sender, EventArgs e)
        {
            var n = lbLookupValues.SelectedIndex;
            if (n >= LastLookupList.data.Count)
                return;
            string s;
            if (cbHexIndex.Checked)
            {
                s = "0x" + LastLookupList.data.ElementAt(n).Value.ID.ToString("X");
            }
            else
            {
                s = LastLookupList.data.ElementAt(n).Value.ID.ToString();
            }
            SendToClipBoard(s);
        }

        private void BtnCopyVal_Click(object sender, EventArgs e)
        {
            var n = lbLookupValues.SelectedIndex;
            if (n >= LastLookupList.data.Count)
                return;
            var s = LastLookupList.data.ElementAt(n).Value.Val;
            SendToClipBoard(s);
        }
    }
}
