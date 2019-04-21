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

        public PacketListFilter Filter;

        public FilterForm()
        {
            InitializeComponent();
            Filter = new PacketListFilter();
            saveFileDlg.InitialDirectory = Application.StartupPath + System.IO.Path.DirectorySeparatorChar + "filter" + System.IO.Path.DirectorySeparatorChar;
            loadFileDlg.InitialDirectory = Application.StartupPath + System.IO.Path.DirectorySeparatorChar + "filter" + System.IO.Path.DirectorySeparatorChar;
            ClearFilters();
        }

        private void FilterForm_Load(object sender, EventArgs e)
        {
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

        public void LoadLocalFromFilter()
        {
            rbOutOff.Checked = (Filter.FilterOutType == FilterType.Off);
            rbOutHide.Checked = (Filter.FilterOutType == FilterType.HidePackets);
            rbOutShow.Checked = (Filter.FilterOutType == FilterType.ShowPackets);
            rbOutNone.Checked = (Filter.FilterOutType == FilterType.AllowNone);
            lbOut.Items.Clear();
            foreach(UInt16 n in Filter.FilterOutList)
                lbOut.Items.Add("0x" + n.ToString("X3") + " - " + DataLookups.NLU(DataLookups.LU_PacketOut).GetValue((UInt64)n));

            rbInOff.Checked = (Filter.FilterInType == FilterType.Off);
            rbInHide.Checked = (Filter.FilterInType == FilterType.HidePackets);
            rbInShow.Checked = (Filter.FilterInType == FilterType.ShowPackets);
            rbInNone.Checked = (Filter.FilterInType == FilterType.AllowNone);
            lbIn.Items.Clear();
            foreach (UInt16 n in Filter.FilterInList)
                lbIn.Items.Add("0x" + n.ToString("X3") + " - " + DataLookups.NLU(DataLookups.LU_PacketIn).GetValue((UInt64)n));
        }

        public void SaveLocalToFilter()
        {
            if (rbOutOff.Checked)
                Filter.FilterOutType = FilterType.Off;
            if (rbOutHide.Checked)
                Filter.FilterOutType = FilterType.HidePackets;
            if (rbOutShow.Checked)
                Filter.FilterOutType = FilterType.ShowPackets;
            if (rbOutNone.Checked)
                Filter.FilterOutType = FilterType.AllowNone;
            Filter.FilterOutList.Clear();
            foreach (string line in lbOut.Items)
            {
                int n = ValForID(line);
                Filter.AddOutFilterValueToList((UInt16)n);
            }

            if (rbInOff.Checked)
                Filter.FilterInType = FilterType.Off;
            if (rbInHide.Checked)
                Filter.FilterInType = FilterType.HidePackets;
            if (rbInShow.Checked)
                Filter.FilterInType = FilterType.ShowPackets;
            if (rbInNone.Checked)
                Filter.FilterInType = FilterType.AllowNone;
            Filter.FilterInList.Clear();
            foreach (string line in lbIn.Items)
            {
                int n = ValForID(line);
                Filter.AddInFilterValueToList((UInt16)n);
            }
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
            if ((rbOutOff.Checked) && (lbOut.Items.Count == 0))
                rbOutShow.Checked = true;
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
            if ((rbInOff.Checked) && (lbIn.Items.Count == 0))
                rbInShow.Checked = true;
            lbIn.Items.Add("0x" + n.ToString("X3") + " - " + DataLookups.NLU(DataLookups.LU_PacketIn).GetValue((UInt64)n));
        }

        private void BtnRemoveIn_Click(object sender, EventArgs e)
        {
            if (lbIn.SelectedIndex >= 0)
                lbIn.Items.Remove(lbIn.SelectedItem);
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (saveFileDlg.ShowDialog() == DialogResult.OK)
            {
                SaveLocalToFilter();
                Filter.SaveToFile(saveFileDlg.FileName);
            }
        }

        private void BtnLoad_Click(object sender, EventArgs e)
        {
            if (loadFileDlg.ShowDialog() == DialogResult.OK)
            {
                if (!Filter.LoadFromFile(loadFileDlg.FileName))
                    Filter.Clear();
                LoadLocalFromFilter();
            }

        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            Filter.Clear();
            LoadLocalFromFilter();
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
