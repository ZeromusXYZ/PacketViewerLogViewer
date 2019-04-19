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
    public partial class FilterForm : Form
    {
        public FilterForm()
        {
            InitializeComponent();
        }

        private void FilterForm_Load(object sender, EventArgs e)
        {
            saveFileDlg.InitialDirectory = Application.StartupPath + "filters" + System.IO.Path.DirectorySeparatorChar; 
            ClearFilters();
            cbOutIDs.Items.Clear();
            foreach(var key in DataLookups.NLU(DataLookups.LU_PacketOut).data.Keys)
            {
                cbOutIDs.Items.Add("0x" + key.ToString("X3") + " - " + DataLookups.NLU(DataLookups.LU_PacketOut).GetValue(key));
            }
            cbInIDs.Items.Clear();
            foreach (var key in DataLookups.NLU(DataLookups.LU_PacketIn).data.Keys)
            {
                cbInIDs.Items.Add("0x" + key.ToString("X3") + " - " + DataLookups.NLU(DataLookups.LU_PacketIn).GetValue(key));
            }
        }

        private void ClearFilters()
        {
            rbOutOff.Checked = true;
            rbOutHide.Checked = false;
            rbOutShow.Checked = false;
            rbOutNone.Checked = false;
            lbOut.Items.Clear();

            rbInOff.Checked = true;
            rbInHide.Checked = false;
            rbInShow.Checked = false;
            rbInNone.Checked = false;
            lbIn.Items.Clear();
        }

        private int ValForID(string s)
        {
            if (s == string.Empty)
                return 0;
            int res = 0;
            char[] splitchars = new char[1] { '-' };
            var fields = s.Split(splitchars,2);
            if (fields.Length >= 1)
            {
                if (DataLookups.TryFieldParse(fields[0].Trim(' '),out int n))
                    res = n;
            }
            return res;
        }

        private void BtnOutAdd_Click(object sender, EventArgs e)
        {
            var s = cbOutIDs.Text ;
            var n = ValForID(s);
            lbOut.Items.Add("0x" + n.ToString("X3") + " - " + DataLookups.NLU(DataLookups.LU_PacketOut).GetValue((UInt64)n));
        }

        private void BtnRemoveOut_Click(object sender, EventArgs e)
        {
            if (lbOut.SelectedIndex >= 0)
                lbOut.Items.Remove(lbOut.SelectedItem);
        }

        private void BtnInAdd_Click(object sender, EventArgs e)
        {
            var s = cbInIDs.Text;
            var n = ValForID(s);
            lbIn.Items.Add("0x" + n.ToString("X3") + " - " + DataLookups.NLU(DataLookups.LU_PacketIn).GetValue((UInt64)n));
        }

        private void BtnRemoveIn_Click(object sender, EventArgs e)
        {
            if (lbIn.SelectedIndex >= 0)
                lbIn.Items.Remove(lbIn.SelectedItem);
        }
    }
}
