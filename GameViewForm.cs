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
            foreach(var dl in DataLookups.LookupLists)
            {
                //if (dl.Key.StartsWith("@") && (dl.Key != "@math"))
                {
                    lbLookupGroups.Items.Add(dl.Key);
                }
            }
        }

        private void LbLookupGroups_SelectedIndexChanged(object sender, EventArgs e)
        {
            var item = lbLookupGroups.SelectedItem;
            if (item == null)
                return;
            lbLookupValues.Items.Clear();
            foreach(var d in DataLookups.NLU((string)item).data)
            {
                lbLookupValues.Items.Add("0x" + d.Value.ID.ToString("x8") + " => " + d.Value.Val);
            }
        }
    }
}
