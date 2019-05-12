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
    public partial class SearchForm : Form
    {
        public SearchParameters searchParameters ;
        bool isValidating = false;

        public SearchForm()
        {
            InitializeComponent();
            searchParameters = new SearchParameters();
            searchParameters.Clear();
        }

        private void FilterForm_Load(object sender, EventArgs e)
        {
            // temporary disable validate
            isValidating = true;
            rbAny.Checked = ((searchParameters.SearchIncoming && searchParameters.SearchOutgoing) || (!searchParameters.SearchIncoming && !searchParameters.SearchOutgoing));
            rbIncoming.Checked = (searchParameters.SearchIncoming && !searchParameters.SearchOutgoing);
            rbOutgoing.Checked = (!searchParameters.SearchIncoming && searchParameters.SearchOutgoing);

            if (searchParameters.SearchByPacketID)
                ePacketID.Text = "0x"+searchParameters.SearchPacketID.ToString("X");
            else
                ePacketID.Text = "";

            if (searchParameters.SearchBySync)
                eSync.Text = "0x" + searchParameters.SearchSync.ToString("X");
            else
                eSync.Text = "";

            if (searchParameters.SearchByByte)
            {
                eValue.Text = "0x" + searchParameters.SearchByte.ToString("X");
                rbByte.Checked = true;
            }
            else
            if (searchParameters.SearchByUInt16)
            {
                eValue.Text = "0x" + searchParameters.SearchUInt16.ToString("X");
                rbUInt16.Checked = true;
            }
            else
            if (searchParameters.SearchByUInt32)
            {
                eValue.Text = "0x" + searchParameters.SearchUInt32.ToString("X");
                rbUInt32.Checked = true;
            }
            else
            {
                eValue.Text = "";
            }

            cbFieldNames.Items.Clear();
            cbFieldNames.Items.AddRange(PacketParser.AllFieldNames.ToArray());

            if (searchParameters.SearchByParsedData)
            {
                cbFieldNames.Text = searchParameters.SearchParsedFieldName;
                eFieldValue.Text = searchParameters.SearchParsedFieldValue;
            }

            isValidating = false;
            ValidateFields();
        }

        private void BtnFindNext_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void BtnAsNewTab_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Retry;
        }

        private void SearchFieldsChanged(object sender, EventArgs e)
        {
            ValidateFields();
        }
        private void ValidateFields()
        {
            bool isValid = true;
            bool hasData = false;
            if (isValidating)
                return;
            isValidating = true;

            // Packet directions
            searchParameters.SearchIncoming = (rbAny.Checked || rbIncoming.Checked);
            searchParameters.SearchOutgoing = (rbAny.Checked || rbOutgoing.Checked);

            // PacketID
            if (DataLookups.TryFieldParse(ePacketID.Text,out int nPacketID))
            {
                if ((nPacketID > 0) && (nPacketID < 0x1FF))
                {
                    hasData = true;
                    searchParameters.SearchByPacketID = true;
                    searchParameters.SearchPacketID = (UInt16)nPacketID;
                    ePacketID.ForeColor = Color.Blue;
                }
                else
                    ePacketID.ForeColor = Color.Red;
            }
            else
                ePacketID.ForeColor = Color.DarkGray;

            // Sync
            if (DataLookups.TryFieldParse(eSync.Text, out int nSync))
            {
                if ((nSync > 0) && (nSync < 0xFFFF))
                {
                    hasData = true;
                    searchParameters.SearchBySync = true;
                    searchParameters.SearchSync = (UInt16)nSync;
                    eSync.ForeColor = Color.Blue;
                }
                else
                    eSync.ForeColor = Color.Red;
            }
            else
                eSync.ForeColor = Color.DarkGray;

            // Value
            if (DataLookups.TryFieldParseUInt64(eValue.Text, out UInt64 nValue))
            {
                // Check the correct type
                if ((nValue > 0xFFFF) && (rbByte.Checked || rbUInt16.Checked))
                    rbUInt32.Checked = true;
                else
                if ((nValue > 0xFF) && (rbByte.Checked))
                    rbUInt16.Checked = true;

                if ((nValue >= 0) && (nValue <= 0xFF) && (rbByte.Checked))
                {
                    hasData = true;
                    searchParameters.SearchByByte = true;
                    searchParameters.SearchByUInt16 = false;
                    searchParameters.SearchByUInt32 = false;
                    searchParameters.SearchByte = (byte)nValue;
                    eValue.ForeColor = Color.Navy;
                }
                else
                if ((nValue >= 0) && (nValue <= 0xFFFF) && (rbUInt16.Checked))
                {
                    hasData = true;
                    searchParameters.SearchByByte = false;
                    searchParameters.SearchByUInt16 = true;
                    searchParameters.SearchByUInt32 = false;
                    searchParameters.SearchUInt16 = (UInt16)nValue;
                    eValue.ForeColor = Color.RoyalBlue;
                }
                else
                if ((nValue >= 0) && (nValue <= 0xFFFFFFFF) && (rbUInt32.Checked))
                {
                    hasData = true;
                    searchParameters.SearchByByte = false;
                    searchParameters.SearchByUInt16 = false;
                    searchParameters.SearchByUInt32 = true;
                    searchParameters.SearchUInt32 = (UInt32)nValue;
                    eValue.ForeColor = Color.Blue;
                }
                else
                {
                    rbByte.Checked = true;
                    eValue.ForeColor = Color.Red;
                }

            }
            else
            {
                rbByte.Checked = true;
                eValue.ForeColor = Color.DarkGray;
            }

            if (eFieldValue.Text != string.Empty)
            {
                hasData = true;
                searchParameters.SearchByParsedData = true;
                searchParameters.SearchParsedFieldName = cbFieldNames.Text.ToLower();
                searchParameters.SearchParsedFieldValue = eFieldValue.Text.ToLower();
            }

            if ((!isValid) || (!hasData))
                searchParameters.Clear();
            btnFindNext.Enabled = isValid && hasData;
            btnAsNewTab.Enabled = isValid && hasData;
            isValidating = false;
        }
    }

}
