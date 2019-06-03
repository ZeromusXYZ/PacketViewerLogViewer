using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PacketViewerLogViewer.Packets;

namespace PacketViewerLogViewer
{
    public partial class GameViewForm : Form
    {
        public static GameViewForm GV = null;

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
                return;
            lbLookupValues.Items.Clear();
            lbLookupValues.BeginUpdate();
            foreach (var d in DataLookups.NLU((string)item).data)
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
    }
}
