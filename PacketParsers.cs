using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using PacketViewerLogViewer.Packets;

namespace PacketViewerLogViewer
{
    class PacketParser
    {
        const int columnOffset = 0 ;
        const int columnVAR = 1 ;
        const int columnDATA = 2 ;
        const int columnSize = 3 ;
        public UInt16 ThisPacketID { get; set; }
        PacketLogTypes ThisPacketLogType { get ; set ; }
        List<string> RawParseData = new List<string>();
        List<byte> ParsedBytes = new List<byte>();
        private PacketData PD;

        public PacketParser(UInt16 aPacketID,PacketLogTypes aPacketLogType)
        {
            var filename = Application.StartupPath + Path.DirectorySeparatorChar + "parse" + Path.DirectorySeparatorChar ;
            switch (aPacketLogType)
            {
                case PacketLogTypes.Incoming:
                    filename += "in-";
                    break;
                case PacketLogTypes.Outgoing:
                    filename += "out-";
                    break;
                default:
                    filename += "unk-"; // Really shouldn't be used, but added for completion
                    break;
            }
            filename += "0x" + aPacketID.ToString("X3").ToLower() + ".txt";
            if (File.Exists(filename))
            {
                PD = null;
                ThisPacketID = aPacketID;
                ThisPacketLogType = aPacketLogType;
                RawParseData = File.ReadLines(filename).ToList();
            }
        }

        public void AssignPacket(PacketData PacketData)
        {
            PD = PacketData;
            ParsedBytes.Clear();
            for (int i = 0; i < PD.RawBytes.Count(); i++)
            {
                ParsedBytes.Add(0x00); // 0 = unparsed
            }
        }

        private void AddParseLineToView(DataGridView DGV,string POSString, Color POSColor, string VARName, string DATAString)
        {
            int thisRow = DGV.Rows.Add();
            DGV.Rows[thisRow].Cells[columnOffset].Value = POSString;
            DGV.Rows[thisRow].Cells[columnOffset].Style.ForeColor = POSColor;
            DGV.Rows[thisRow].Cells[columnVAR].Value = VARName;
            DGV.Rows[thisRow].Cells[columnVAR].Style.ForeColor = POSColor;
            DGV.Rows[thisRow].Cells[columnDATA].Value = DATAString;
        }

        public void ParseToDataGridView(DataGridView DGV)
        {
            // Header
            DGV.Rows.Clear();
            DGV.ColumnCount = 3;

            DGV.Columns[columnOffset].HeaderText = "POS";
            DGV.Columns[columnOffset].Width = 48;

            DGV.Columns[columnVAR].HeaderText = "VAR";
            DGV.Columns[columnVAR].Width = 128;

            DGV.Columns[columnDATA].HeaderText = "Data";
            DGV.Columns[columnDATA].Width = DGV.Width - DGV.Columns[columnOffset].Width - DGV.Columns[columnVAR].Width - 20;

            //DGV.Columns[columnSize].HeaderText = "Size";
            //DGV.Columns[columnSize].Width = 32;

            // Fixed Header Info, Always 4 bytes, always listed
            switch (PD.PacketLogType)
            {
                case PacketLogTypes.Outgoing:
                    AddParseLineToView(DGV, 
                        "0x00", Color.Black, 
                        "PacketID", 
                        "OUT 0x" + PD.PacketID.ToString("X3") + " - " + DataLookups.NLU(DataLookups.LU_PacketOut).GetValue(PD.PacketID));
                    break;
                case PacketLogTypes.Incoming:
                    AddParseLineToView(DGV,
                        "0x00", Color.Black,
                        "PacketID",
                        "IN 0x" + PD.PacketID.ToString("X3") + " - " + DataLookups.NLU(DataLookups.LU_PacketIn).GetValue(PD.PacketID));
                    break;
                default:
                    AddParseLineToView(DGV,
                        "0x00", Color.Black,
                        "PacketID",
                        "??? 0x" + PD.PacketID.ToString("X3"));
                    break;
            }
            AddParseLineToView(DGV,
                "0x00", Color.Black,
                "Size",
                PD.PacketDataSize.ToString() + " (0x" + PD.PacketDataSize.ToString("X2")+")");
            AddParseLineToView(DGV,
                "0x02", Color.Black,
                "Sync",
                PD.PacketSync.ToString() + " (0x" + PD.PacketSync.ToString("X4") + ")");
            // Marked as FF for fixed format
            ParsedBytes[0] = 0xFF;
            ParsedBytes[1] = 0xFF;
            ParsedBytes[2] = 0xFF;
            ParsedBytes[3] = 0xFF;



            // List unparsed bytes
            for (int i = 4; i < PD.RawBytes.Count(); i++)
            {
                if ((i <= (PD.RawBytes.Count()-4)) && (ParsedBytes[i] == 0) && (ParsedBytes[i+1] == 0) && (ParsedBytes[i+2] == 0) && (ParsedBytes[i+3] == 0))
                {
                    AddParseLineToView(DGV,
                        "0x" + i.ToString("X2"),
                        Color.DarkGray,
                        "??_UInt32 (@" + i.ToString() + ")",
                        "0x" + PD.GetUInt32AtPos(i).ToString("X8") + " (" + PD.GetUInt32AtPos(i).ToString() + ")");
                    i += 3; // move forward a extra 3 bytes
                }
                else
                if (ParsedBytes[i] == 0)
                {
                    AddParseLineToView(DGV, 
                        "0x" + i.ToString("X2"), 
                        Color.DarkGray, 
                        "??_Byte (@" + i.ToString() + ")", 
                        "0x" + PD.GetByteAtPos(i).ToString("X2") + " (" + PD.GetByteAtPos(i).ToString() + ")");
                }
            }
        }

    }
}
