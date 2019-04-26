using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using PacketViewerLogViewer.Packets;
using Vanadiel.Time;

namespace PacketViewerLogViewer
{
    public struct ParsedViewLine
    {
        public string Pos ;
        public string Var ;
        public string Data ;
        public byte FieldIndex;
        public Color FieldColor;
    }

    public class PacketParser
    {
        List<Color> DataColors = new List<Color>();

        const int columnOffset = 0;
        const int columnVAR = 1;
        const int columnDATA = 2;
        const int columnSize = 3;
        public UInt16 ThisPacketID { get; set; }
        PacketLogTypes ThisPacketLogType { get; set; }
        public List<string> RawParseData = new List<string>();
        public List<byte> ParsedBytes = new List<byte>();
        public List<ParsedViewLine> ParsedView = new List<ParsedViewLine>();
        public List<byte> SelectedFields = new List<byte>();
        public PacketData PD;
        public List<string> SwitchBlocks = new List<string>();
        public string LastSwitchedBlock = "";
        private string[] CompasDirectionNames = new string[16] { "E", "ESE", "SE", "SSE", "S", "SSW", "SW", "WSW", "W", "WNW", "NW", "NNW", "N", "NNE", "NE", "ENE" };

        public PacketParser(UInt16 aPacketID,PacketLogTypes aPacketLogType)
        {
            // Default Field Colors
            /*
            DataColors.Add(Color.Black);
            DataColors.Add(Color.Red);
            DataColors.Add(Color.Green);
            DataColors.Add(Color.Blue);
            DataColors.Add(Color.Purple);
            DataColors.Add(Color.DarkGray);
            DataColors.Add(Color.Maroon);
            DataColors.Add(Color.Navy);
            */
            DataColors.Add(Color.Black);
            DataColors.Add(Color.Chocolate);
            DataColors.Add(Color.MediumSeaGreen);
            DataColors.Add(Color.CornflowerBlue);
            DataColors.Add(Color.DarkSalmon);
            DataColors.Add(Color.DarkGray);
            DataColors.Add(Color.Brown);
            DataColors.Add(Color.MidnightBlue);

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

        public Color GetDataColor(int fieldIndex)
        {
            if (DataColors.Count > 0)
            {
                return DataColors[fieldIndex % DataColors.Count];
            }
            else
            {
                return Color.MediumPurple;
            }
        }

        public void AssignPacket(PacketData PacketData)
        {
            PD = PacketData;
            ParsedBytes.Clear();
            SelectedFields.Clear();
            for (int i = 0; i < PD.RawBytes.Count(); i++)
            {
                ParsedBytes.Add(0x00); // 0 = unparsed
            }
        }

        private string ByteToRotation(byte b)
        {
            int i = (b * 360) / 256;
            double rads = ((Math.PI * 2) / 360 * i);
            return CompasDirectionNames[(i / 16) % 16] + " (" + b.ToString()+" - 0x"+b.ToString("X2")+ " ≈ " + i.ToString() + "° = " + rads.ToString("F") + " rad)";
        }

        private string ByteToBits(byte b)
        {
            string res = "";
            for(int i = 1;i < 256;i <<= 1)
            {
                if (i == 16)
                    res = " " + res;

                if ((b & i) != 0)
                {
                    res = "1" + res ;
                }
                else
                {
                    res = "0" + res ;
                }
            }

            return res;
        }

        private string MSToString(UInt32 ms)
        {
            UInt32 r = ms % 1000;
            UInt32 v = ms / 1000;
            string res = r.ToString("0000")+"ms";
            if (v > 0)
            {
                r = v % 60;
                v = v / 60;
                res = r.ToString("00") + "s " + res ;
                if (v > 0)
                {
                    r = v % 60;
                    v = v / 60;
                    res = r.ToString("00") + "m " + res;
                    if (v > 0)
                    {
                        r = v % 24;
                        v = v / 24;
                        res = r.ToString("00") + "h " + res;
                        if (v > 0)
                        {
                            res = v.ToString() + "d " + res;
                        }
                    }
                }
            }
            return res;
        }

        private string FramesToString(UInt32 frames)
        {
            UInt32 r = frames % 60;
            UInt32 v = frames / 60;
            string res = r.ToString("00") + "f";
            if (v > 0)
            {
                r = v % 60;
                v = v / 60;
                res = r.ToString("00") + " / " + res;
                if (v > 0)
                {
                    r = v % 60;
                    v = v / 60;
                    res = r.ToString("00") + "." + res;
                    if (v > 0)
                    {
                        r = v % 24;
                        v = v / 24;
                        res = r.ToString("00") + ":" + res;
                        if (v > 0)
                        {
                            res = v.ToString() + "d " + res;
                        }
                    }
                }
            }
            return res;
        }

        private string VanaDoW(byte dow)
        {
            switch(dow)
            {
                case 0:
                    return "Firesday";
                case 1:
                    return "Earthsday";
                case 2:
                    return "Watersday";
                case 3:
                    return "Windsday";
                case 4:
                    return "Iceday";
                case 5:
                    return "Thundersday";
                case 6:
                    return "Lightsday";
                case 7:
                    return "Darksday";
                default:
                    return "??? DoW 0x" + dow.ToString("X");
            }
        }

        private DateTime UnixUInt32ToDateTime(UInt32 v)
        {
            const UInt64 VTIME_BASEDATE = 1009810800;
            DateTime res = new DateTime(1970, 1, 1);
            res = res.AddSeconds(VTIME_BASEDATE + v);
            return res;
        }

        private string UnixUInt32ToVanaTime(UInt32 v)
        {
            // const UInt64 VTIME_BASEDATE = 1009810800;
            // unix epoch - 1009810800 = se epoch (in earth seconds)
            const UInt64 VTIME_YEAR  = 518400; // 360 * GameDay
            const UInt64 VTIME_MONTH = 43200;  // 30 * GameDay
            const UInt64 VTIME_WEEK  = 11520;  // 8 * GameDay
            const UInt64 VTIME_DAY   = 1440;   // 24 hours * GameHour
            const UInt64 VTIME_HOUR  = 60;     // 60 minutes
            const UInt64 VTIME_FIRSTYEAR = 886;

            var VanaDate = v ;
            var vYear = VanaDate / VTIME_YEAR;
            var vMonth = ((VanaDate / VTIME_MONTH) % 12) + 1;
            var vDay = ((VanaDate / VTIME_DAY) % 30) + 1;
            var vDoW = (VanaDate % VTIME_WEEK) / VTIME_DAY ;
            var vHour = (VanaDate % VTIME_DAY) / VTIME_HOUR ;
            var vMinute = VanaDate % VTIME_HOUR ;

            return VanaDoW((byte)vDoW) + " - " + (vYear + VTIME_FIRSTYEAR).ToString() + "/" +
                vMonth.ToString("00") + "/" + vDay.ToString("00") + " - " + vHour.ToString("00") + ":" +
                vMinute.ToString("00") + "  (0x" + v.ToString("X8") + " - " + v.ToString() + ")";
        }

        private string Lookup(string lookupName,UInt64 value)
        {
            if (lookupName == string.Empty)
                return "";
            return DataLookups.NLU(lookupName).GetValue(value) + " <= " ;
        }

        public void ToGridView(DataGridView DGV)
        {
            if (DGV.Tag != null)
                return;
            DGV.Tag = 1;
            // Header
            //DGV.Rows.Clear();
            DGV.ColumnCount = 3;

            DGV.Columns[columnOffset].HeaderText = "Pos";
            DGV.Columns[columnOffset].Width = 88;

            DGV.Columns[columnVAR].HeaderText = "Name";
            DGV.Columns[columnVAR].Width = 192;

            DGV.Columns[columnDATA].HeaderText = "Data";
            var dataWidth = DGV.Width - DGV.Columns[columnOffset].Width - DGV.Columns[columnVAR].Width - 20;
            if (dataWidth < 128)
                dataWidth = 128;
            DGV.Columns[columnDATA].Width = dataWidth;

            //DGV.Columns[columnSize].HeaderText = "Size";
            //DGV.Columns[columnSize].Width = 32;
            for(int thisRow = 0;thisRow < ParsedView.Count;thisRow++)
            {
                if (DGV.RowCount <= thisRow)
                    DGV.Rows.Add();
                ParsedViewLine pvl = ParsedView[thisRow];
                DGV.Rows[thisRow].Cells[columnOffset].Value = pvl.Pos;
                DGV.Rows[thisRow].Cells[columnOffset].Style.ForeColor = pvl.FieldColor;
                DGV.Rows[thisRow].Cells[columnOffset].Value = pvl.Pos;
                DGV.Rows[thisRow].Cells[columnOffset].Style.ForeColor = pvl.FieldColor ;
                DGV.Rows[thisRow].Cells[columnVAR].Value = pvl.Var;
                DGV.Rows[thisRow].Cells[columnVAR].Style.ForeColor = pvl.FieldColor;
                DGV.Rows[thisRow].Cells[columnDATA].Value = pvl.Data ;
                // DGV.Rows[thisRow].Cells[3].Value = pvl.FieldIndex.ToString();
                if (SelectedFields.IndexOf(pvl.FieldIndex) >= 0)
                {
                    // this field is selected 
                    DGV.Rows[thisRow].Selected = true;
                }
                else
                {
                    DGV.Rows[thisRow].Selected = false;
                }
            }

            while (DGV.Rows.Count > ParsedView.Count)
                DGV.Rows.RemoveAt(DGV.Rows.Count - 1);

            DGV.Tag = null;
        }

        private void AddParseLineToView(byte FieldIndex,string POSString, Color POSColor, string VARName, string DATAString)
        {
            ParsedViewLine pvl = new ParsedViewLine();
            pvl.Pos = POSString;
            pvl.Var = VARName;
            pvl.Data = DATAString;
            pvl.FieldIndex = FieldIndex;
            pvl.FieldColor = POSColor;
            ParsedView.Add(pvl);
        }

        private void MarkParsed(int offset, int bytesize, byte fieldindex)
        {
            if (bytesize <= 0)
                bytesize = 1;

            for(int i = 0; i < bytesize;i++)
            {
                var p = offset + i;
                if ((p >= 0) && (p < ParsedBytes.Count))
                    ParsedBytes[p] = fieldindex;
            }
        }

        private bool ValueInStringList(int searchValue, string searchList)
        {
            if ((searchList == null) || (searchList == string.Empty))
                return false;
            var searchStrings = searchList.Split(',').ToList();
            foreach(string s in searchStrings)
            {
                if (DataLookups.TryFieldParse(s.Trim(' '), out int n))
                    if (n == searchValue)
                        return true;
            }
            return false;
        }

        private string BitFlagsToString(string lookupname,UInt64 value,string concatString)
        {
            string res = "";
            if (concatString == "")
                concatString = " ";
            UInt64 bitmask = 0x1;
            for(UInt64 i = 0; i < 64;i++)
            {
                if ((value & bitmask) != 0)
                {
                    string item = "";
                    if (lookupname != "")
                        item = DataLookups.NLU(lookupname).GetValue(i);
                    if (item == "")
                        item = "Bit" + i.ToString();
                    if (res != "")
                        res += concatString;
                    res += item;
                }
                bitmask <<= 1;
            }
            if (res == "")
                res = "No bits set";
            return res;
        }

        private int Parse_Packet_In_0x028(PacketData PD,ref byte DataFieldIndex)
        {
            int BitOffset = 0 ;

            void AddField(ref byte AddDataFieldIndex, int BitSize,string name, string data)
            {
                AddDataFieldEx((BitOffset / 8), (BitSize / 8), ref AddDataFieldIndex);
                var posStr = "0x" + (BitOffset / 8).ToString("X") + ":" + (BitOffset % 8).ToString();
                if (BitSize > 1)
                    posStr += "-" + BitSize.ToString();
                AddParseLineToView(AddDataFieldIndex, posStr, GetDataColor(AddDataFieldIndex), name, data);
                MarkParsed((BitOffset / 8), (BitSize / 8), AddDataFieldIndex);
                BitOffset += BitSize;
            }

            int bytesUsed = 0;

            if ((PD.PacketLogType != PacketLogTypes.Incoming) || (PD.PacketID != 0x028))
            {
                AddParseLineToView(0x00, "Error", Color.Red, "Invalid", "Can only use this field type on a Incoming 0x028 packet");
                return 0;
            }
            var pSize = PD.GetByteAtPos(0x04);
            var pActor = PD.GetUInt32AtPos(0x05);
            var pTargetCount = PD.GetBitsAtPos(0x09, 0, 10);

            // First group contains info about the size instead of actual data ?
            // The bit offset is a pain to work with however >.>

            var pActionCategory = PD.GetBitsAtPos(0x0A, 2, 4);
            var pActionID = PD.GetBitsAtPos(0x0A, 6, 16);
            var pUnknown1 = PD.GetBitsAtPos(0x0C, 6, 16);
            var pRecast = PD.GetBitsAtPos(0x0E, 6, 32);

            AddDataFieldEx(0x04, 1, ref DataFieldIndex);
            AddParseLineToView(DataFieldIndex, "0x04", GetDataColor(DataFieldIndex), "Info Size", pSize.ToString());
            MarkParsed(0x04, 1, DataFieldIndex);

            AddDataFieldEx(0x05, 4, ref DataFieldIndex);
            AddParseLineToView(DataFieldIndex, "0x05", GetDataColor(DataFieldIndex), "Actor", "0x" + pActor.ToString("X8") + " - " + pActor.ToString());
            MarkParsed(0x05, 4, DataFieldIndex);

            AddDataFieldEx(0x09, 1, ref DataFieldIndex);
            AddParseLineToView(DataFieldIndex, "0x09", GetDataColor(DataFieldIndex), "Target Count", pTargetCount.ToString());
            MarkParsed(0x09, 1, DataFieldIndex);

            AddDataFieldEx(0x0A, 1, ref DataFieldIndex);
            AddParseLineToView(DataFieldIndex, "0x0A:2-4", GetDataColor(DataFieldIndex), "Action Category", pActionCategory.ToString() + " => " + DataLookups.NLU(DataLookups.LU_ActionCategory).GetValue((UInt64)pActionCategory));
            MarkParsed(0x0A, 1, DataFieldIndex);

            AddDataFieldEx(0x0B, 1, ref DataFieldIndex);
            AddParseLineToView(DataFieldIndex, "0x0A:6-16", GetDataColor(DataFieldIndex), "Action ID", pActionID.ToString());
            MarkParsed(0x0B, 1, DataFieldIndex);

            AddDataFieldEx(0x0C, 2, ref DataFieldIndex);
            AddParseLineToView(DataFieldIndex, "0x0C:6-16", GetDataColor(DataFieldIndex), "_unknown1", pUnknown1.ToString());
            MarkParsed(0x0C, 2, DataFieldIndex);

            AddDataFieldEx(0x0E, 5, ref DataFieldIndex);
            AddParseLineToView(DataFieldIndex, "0x0E:6-32", GetDataColor(DataFieldIndex), "Recast", pRecast.ToString());
            MarkParsed(0x0E, 5, DataFieldIndex);

            int FirstTargetBitOffset = 150; // (0x012:6)
            int LastBit = PD.RawBytes.Count * 8;
            BitOffset = FirstTargetBitOffset;
            int pTargetCountLoopCounter = 0;

            while ((BitOffset < LastBit) && (pTargetCountLoopCounter < pTargetCount))
            {
                pTargetCountLoopCounter++;

                var pActionTargetID = PD.GetBitsAtBitPos(BitOffset, 32);
                AddField(ref DataFieldIndex, 32, "#" + pTargetCountLoopCounter + " : Target ID", "0x" + pActionTargetID.ToString("X8") + " - " + pActionTargetID.ToString());

                var pActionTargetIDSize = PD.GetBitsAtBitPos(BitOffset, 4);
                AddField(ref DataFieldIndex, 4, "#" + pTargetCountLoopCounter + " : Effect Count", pActionTargetIDSize.ToString());

                int tTargetEffectLoopCounter = 0;

                while ((BitOffset < LastBit) && (tTargetEffectLoopCounter < pActionTargetIDSize))
                {
                    tTargetEffectLoopCounter++;

                    var thisLoopName = "  #" + pTargetCountLoopCounter + " " + tTargetEffectLoopCounter + "/" + pActionTargetIDSize.ToString() + " ";

                    var tReaction = PD.GetBitsAtBitPos(BitOffset, 5);
                    AddField(ref DataFieldIndex, 5, thisLoopName + "Reaction", "0x"+tReaction.ToString("X") + " => " + BitFlagsToString(DataLookups.LU_ActionReaction,(UInt64)tReaction," + "));

                    var tAnimation = PD.GetBitsAtBitPos(BitOffset, 12);
                    AddField(ref DataFieldIndex, 12, thisLoopName + "Animation", "0x"+ tAnimation.ToString("X3") + " - " + tAnimation.ToString());

                    var tSpecialEffect = PD.GetBitsAtBitPos(BitOffset, 7);
                    AddField(ref DataFieldIndex, 7, thisLoopName + "SpecialEffect", "0x" + tSpecialEffect.ToString("X2") + " - " + tSpecialEffect.ToString());

                    var tKnockBack = PD.GetBitsAtBitPos(BitOffset, 3);
                    AddField(ref DataFieldIndex, 3, thisLoopName + "KnockBack", tKnockBack.ToString());

                    var tParam = PD.GetBitsAtBitPos(BitOffset, 17);
                    AddField(ref DataFieldIndex, 17, thisLoopName + "Param", "0x" + tParam.ToString("X5") + " - " + tParam.ToString());

                    var tMessageID = PD.GetBitsAtBitPos(BitOffset, 10);
                    AddField(ref DataFieldIndex, 10, thisLoopName + "MessageID", "0x" + tMessageID.ToString("X3") + " - " + tMessageID.ToString());

                    var tUnknown = PD.GetBitsAtBitPos(BitOffset, 31);
                    AddField(ref DataFieldIndex, 31, thisLoopName + "??? 31 Bits", "0x" + tUnknown.ToString("X8") + " - " + tUnknown.ToString());

                    var hasAdditionalEffect = PD.GetBitAtPos(BitOffset / 8, BitOffset % 8);
                    AddField(ref DataFieldIndex, 1, thisLoopName + "Has Additional Effect", hasAdditionalEffect.ToString());

                    if (hasAdditionalEffect)
                    {
                        var tAdditionalEffect = PD.GetBitsAtBitPos(BitOffset, 10);
                        AddField(ref DataFieldIndex, 10, "  " + thisLoopName + "Added Effect", "0x" + tAdditionalEffect.ToString("X3") + " - " + tAdditionalEffect.ToString());

                        var tAddEffectParam = PD.GetBitsAtBitPos(BitOffset, 17);
                        AddField(ref DataFieldIndex, 17, "  " + thisLoopName + "Effect Param", "0x" + tAddEffectParam.ToString("X5") + " - " + tAddEffectParam.ToString());

                        var tAddEffectMessage = PD.GetBitsAtBitPos(BitOffset, 10);
                        AddField(ref DataFieldIndex, 10, "  " + thisLoopName + "Effect MsgID", "0x" + tAddEffectMessage.ToString("X3") + " - " + tAddEffectMessage.ToString());
                    }

                    var hasSpikesEffect = PD.GetBitAtPos(BitOffset / 8, BitOffset % 8);
                    AddField(ref DataFieldIndex, 1, thisLoopName + "Has Spikes Effect", hasSpikesEffect.ToString());

                    if (hasSpikesEffect)
                    {
                        var tSpikesEffect = PD.GetBitsAtBitPos(BitOffset, 10);
                        AddField(ref DataFieldIndex, 10, "  " + thisLoopName + "Spike Effect", "0x" + tSpikesEffect.ToString("X3") + " - " + tSpikesEffect.ToString());

                        var tSpikesParam = PD.GetBitsAtBitPos(BitOffset, 14);
                        AddField(ref DataFieldIndex, 14, "  " + thisLoopName + "Spike Param", "0x" + tSpikesParam.ToString("X4") + " - " + tSpikesParam.ToString());

                        var tSpikesMessage = PD.GetBitsAtBitPos(BitOffset, 10);
                        AddField(ref DataFieldIndex, 10, "  " + thisLoopName + "Spike MsgID", "0x" + tSpikesMessage.ToString("X3") + " - " + tSpikesMessage.ToString());
                    }

                }
            }
            if ((BitOffset % 8) > 0)
                bytesUsed = (BitOffset / 8) + 1;
            else
                bytesUsed = (BitOffset / 8);

            return bytesUsed;
        }

        private void AddDataFieldEx(int StartPos, int FieldByteSize, ref byte DataFieldIndex)
        {
            if (FieldByteSize < 1)
                FieldByteSize = 1;
            if ((StartPos < 0) || (StartPos >= ParsedBytes.Count))
                return;
            // DataFieldIndex++;
            if (ParsedBytes[StartPos] == 0)
            {
                DataFieldIndex++;
                for (int i = StartPos; (i < ParsedBytes.Count) && (i < (StartPos + FieldByteSize)); i++)
                {
                    ParsedBytes[i] = DataFieldIndex;
                }
            }
        }


        public void ParseToDataGridView(DataGridView DGV,string ActiveSwitchBlock)
        {
            byte DataFieldIndex = 0; // header is considered 0

            void AddDataField(int StartPos, int FieldByteSize)
            {
                AddDataFieldEx(StartPos, FieldByteSize, ref DataFieldIndex);
            }

            // Fixed Header Info, Always 4 bytes, always listed
            switch (PD.PacketLogType)
            {
                case PacketLogTypes.Outgoing:
                    AddParseLineToView(0xff, 
                        "0x00", Color.DarkSlateGray, 
                        "PacketID", 
                        "OUT 0x" + PD.PacketID.ToString("X3") + " - " + DataLookups.NLU(DataLookups.LU_PacketOut).GetValue(PD.PacketID));
                    break;
                case PacketLogTypes.Incoming:
                    AddParseLineToView(0xff,
                        "0x00", Color.DarkGray,
                        "PacketID",
                        "IN 0x" + PD.PacketID.ToString("X3") + " - " + DataLookups.NLU(DataLookups.LU_PacketIn).GetValue(PD.PacketID));
                    break;
                default:
                    AddParseLineToView(0xff,
                        "0x00", Color.DarkSlateGray,
                        "PacketID",
                        "??? 0x" + PD.PacketID.ToString("X3"));
                    break;
            }
            AddParseLineToView(0xff,
                "0x00", Color.Black,
                "Size",
                PD.PacketDataSize.ToString() + " (0x" + PD.PacketDataSize.ToString("X2")+")");
            AddParseLineToView(0xff,
                "0x02", Color.Black,
                "Sync",
                PD.PacketSync.ToString() + " (0x" + PD.PacketSync.ToString("X4") + ")");
            // Marked as FF for fixed format
            ParsedBytes[0] = 0xFF;
            ParsedBytes[1] = 0xFF;
            ParsedBytes[2] = 0xFF;
            ParsedBytes[3] = 0xFF;
            int DataCursor = 0;
            int DataSubCursor = 0;

            string CurrentlyParsedSwitchBlock = "";
            SwitchBlocks.Clear();
            // Parse the parse file
            //foreach(string rawline in RawParseData)
            bool AllowAutoSwitchBlock = ((ActiveSwitchBlock == string.Empty) || (ActiveSwitchBlock == "-"));
            LastSwitchedBlock = "";
            for (int parseLineNumber = 1;parseLineNumber <= RawParseData.Count;parseLineNumber++)
            {
                // We want a  type;offset[;name[;description]] format

                string line = RawParseData[parseLineNumber-1];
                // Cut out comments
                if (line.IndexOf("//") >= 0)
                    line = line.Substring(0, line.IndexOf("//"));
                line = line.Trim(' ');

                // Skip blank lines
                if (line == string.Empty)
                    continue;

                // Handle Switch Blocks
                if (line.StartsWith("[[") && line.EndsWith("]]"))
                {
                    // This is a switch block statement
                    var thisSwitchName = line.Substring(2, line.Length - 4).Trim(' ');
                    if (thisSwitchName != string.Empty)
                    {
                        SwitchBlocks.Add(thisSwitchName);
                    }
                    else
                    {
                        AllowAutoSwitchBlock = true;
                    }
                    CurrentlyParsedSwitchBlock = thisSwitchName;
                    continue;
                }
                if ((CurrentlyParsedSwitchBlock != "") && (CurrentlyParsedSwitchBlock.ToLower() != ActiveSwitchBlock.ToLower()))
                    continue;

                string[] fields = line.Split(';');
                if (fields.Count() < 2)
                {
                    // Need at least 2 fields to be a valid line
                    AddParseLineToView(0xff, "L " + parseLineNumber.ToString(), Color.Red, "Parse Error", "Need at least 2 fields");
                    continue;
                }
                string typeField = fields[0].ToLower().Trim(' ');
                string posField = fields[1].ToLower().Trim(' ');
                string nameField = "";
                string descriptionField = "";
                string lookupField = "";
                int lookupFieldOffset = 0;
                if (fields.Length > 2)
                {
                    nameField = fields[2].TrimEnd(' ');
                }
                else
                {
                    nameField = typeField + " (@" + posField+")" ;
                }
                if (fields.Length > 3)
                {
                    descriptionField = fields[3];
                }

                if (typeField == "file")
                    continue;
                if (typeField == "rem")
                    continue;
                if (typeField == "comment")
                    continue;
                if (typeField == string.Empty)
                    continue;

                // Parse type lookup (if present)
                var lookupFieldSplitPos = typeField.IndexOf(":");
                if (lookupFieldSplitPos > 0)
                {
                    var lookupFields = typeField.Split(':');
                    typeField = lookupFields[0];
                    lookupField = lookupFields[1];
                    if (lookupFields.Length > 2)
                    {
                        if (DataLookups.TryFieldParse(lookupFields[2], out int lookupoffsetres))
                            lookupFieldOffset = lookupoffsetres;
                    }
                }

                // Parse posField
                int Offset = 0;
                int SubOffset = 0;
                int SubOffsetRange = 0;
                bool hasSubOffset = false;
                bool hasSubOffsetRange = false;
                int posSplitPos = posField.IndexOf(":");
                if (posSplitPos > 0)
                {
                    // Has sub offset
                    var offsetStr = posField.Substring(0, posSplitPos);
                    var subOffsetStr = posField.Substring(posSplitPos+1, posField.Length - posSplitPos-1);
                    hasSubOffset = true ;

                    // Get normal offset for this
                    if (!DataLookups.TryFieldParse(offsetStr, out Offset))
                    {
                        Offset = 0;
                        AddParseLineToView(0xff, "L " + parseLineNumber.ToString(), Color.Red, "Parse Error", "Invalid Offset Value in: " + posField);
                        continue;
                    }

                    // Check for bit range
                    int rangeSplitPos = subOffsetStr.IndexOf("-");
                    if (rangeSplitPos > 0)
                    {
                        // We have a range
                        var subOffsetBitStr = subOffsetStr.Substring(0, rangeSplitPos);
                        var subOffsetRangeStr = subOffsetStr.Substring(rangeSplitPos + 1, subOffsetStr.Length - rangeSplitPos-1);
                        hasSubOffsetRange = true;

                        if (!DataLookups.TryFieldParse(subOffsetBitStr, out SubOffset))
                        {
                            Offset = 0;
                            AddParseLineToView(0xff, "L " + parseLineNumber.ToString(), Color.Red, "Parse Error", "Invalid SubOffset Value in: " + posField);
                            continue;
                        }

                        if (!DataLookups.TryFieldParse(subOffsetRangeStr, out SubOffsetRange))
                        {
                            Offset = 0;
                            AddParseLineToView(0xff, "L " + parseLineNumber.ToString(), Color.Red, "Parse Error", "Invalid SubOffsetRange Value in: " + posField);
                            continue;
                        }
                    }
                    else
                    {
                        // doesn't have a range
                        if (!DataLookups.TryFieldParse(subOffsetStr, out SubOffset))
                        {
                            Offset = 0;
                            AddParseLineToView(0xff, "L " + parseLineNumber.ToString(), Color.Red, "Parse Error", "Invalid SubOffset Value in: " + posField);
                            continue;
                        }
                    }
                }
                else
                {
                    // Normal single offset value
                    if (!DataLookups.TryFieldParse(posField, out Offset))
                    {
                        Offset = 0;
                        AddParseLineToView(0xff, "L " + parseLineNumber.ToString(), Color.Red, "Parse Error", "Invalid Offset Value in: "+posField);
                        continue;
                    }
                }

                DataCursor = Offset;
                DataSubCursor = SubOffset;
                // Rebuild Pos string
                posField = "0x" + Offset.ToString("X2");
                if (hasSubOffset)
                {
                    posField += ":" + SubOffset.ToString();
                    if (hasSubOffsetRange)
                        posField += "-" + SubOffsetRange.ToString();
                }

                // Begin parse
                if (typeField == "switchblock")
                {
                    if (AllowAutoSwitchBlock)
                    {
                        // switchblock;checkpos;checkval;blockname
                        // Compares value at Offset, if posVal matches a value in the description field, activate blockname as current block

                        int posVal = 0;
                        if ((SubOffset != 0) || (SubOffsetRange != 0))
                            posVal = (int)PD.GetBitsAtPos(Offset, SubOffset, SubOffsetRange);
                        else
                            posVal = (int)PD.GetByteAtPos(Offset);
                        // Switchval seems valid, next compare it
                        if (ValueInStringList(posVal, descriptionField))
                        {
                            ActiveSwitchBlock = nameField;
                            AllowAutoSwitchBlock = false;
                            // Debug Info
                            AddParseLineToView(0xff, "L " + parseLineNumber.ToString(), Color.Red, "Switch", "Activate Block: " + ActiveSwitchBlock);
                            LastSwitchedBlock = ActiveSwitchBlock;
                            continue;
                        }
                    }
                }
                else
                if (typeField == "info")
                {
                    // Info line for the view
                    if (Offset < 4)
                        AddParseLineToView(0xFF, "Info", GetDataColor(0xFF), nameField, descriptionField);
                    else
                    {
                        // Only mark it as parsed if a specific offset is provided
                        AddDataField(Offset, 1);
                        AddParseLineToView(DataFieldIndex, posField, GetDataColor(DataFieldIndex), nameField, descriptionField);
                        MarkParsed(Offset, 1, DataFieldIndex);
                    }
                }
                else
                if (typeField == "byte")
                {
                    var d = PD.GetByteAtPos(Offset);
                    var l = Lookup(lookupField,(UInt64)(d + lookupFieldOffset));
                    AddDataField(Offset,1);
                    AddParseLineToView(DataFieldIndex, posField, GetDataColor(DataFieldIndex), nameField, l + d.ToString() + " - 0x" + d.ToString("X2") + " - " + ByteToBits(d) + " - '" + (char)d + "'");
                    MarkParsed(Offset, 1, DataFieldIndex);
                }
                else
                if ((typeField == "bit") || (typeField == "bool"))
                {
                    var d = PD.GetBitAtPos(Offset,SubOffset);
                    AddDataField(Offset,1);
                    AddParseLineToView(DataFieldIndex, posField, GetDataColor(DataFieldIndex), nameField, d.ToString());
                    MarkParsed(Offset, 1, DataFieldIndex);
                }
                else
                if (typeField == "bits")
                {
                    var d = PD.GetBitsAtPos(Offset, SubOffset, SubOffsetRange);
                    var l = Lookup(lookupField, (UInt64)(d + lookupFieldOffset));
                    var firstbit = (Offset * 8) + SubOffset;
                    var lastbit = firstbit + SubOffsetRange;
                    var firstbyte = firstbit / 8;
                    var lastbyte = lastbit / 8;
                    var bytesize = lastbyte - firstbyte + 1;
                    AddDataField(firstbyte, bytesize);
                    AddParseLineToView(DataFieldIndex, posField, GetDataColor(DataFieldIndex), nameField, l + "0x"+d.ToString("X") + " - " + d.ToString(""));
                    MarkParsed(firstbyte, bytesize, DataFieldIndex);
                }
                else
                if (typeField == "uint16")
                {
                    var d = PD.GetUInt16AtPos(Offset);
                    var l = Lookup(lookupField, (UInt64)(d + lookupFieldOffset));
                    AddDataField(Offset,2);
                    AddParseLineToView(DataFieldIndex, posField, GetDataColor(DataFieldIndex), nameField, l + d.ToString() + " (0x" + d.ToString("X4") + ")");
                    MarkParsed(Offset, 2, DataFieldIndex);
                }
                else
                if (typeField == "int16")
                {
                    var d = PD.GetInt16AtPos(Offset);
                    var l = Lookup(lookupField, (UInt64)(d + lookupFieldOffset));
                    AddDataField(Offset,2);
                    AddParseLineToView(DataFieldIndex, posField, GetDataColor(DataFieldIndex), nameField, l + d.ToString() + " (0x" + d.ToString("X4") + ")");
                    MarkParsed(Offset, 2, DataFieldIndex);
                }
                else
                if (typeField == "uint32")
                {
                    var d = PD.GetUInt32AtPos(Offset);
                    var l = Lookup(lookupField, (UInt64)(d + lookupFieldOffset));
                    AddDataField(Offset,4);
                    AddParseLineToView(DataFieldIndex, posField, GetDataColor(DataFieldIndex), nameField, l + d.ToString() + " (0x" + d.ToString("X8") + ")");
                    MarkParsed(Offset, 4, DataFieldIndex);
                }
                else
                if (typeField == "int32")
                {
                    var d = PD.GetInt32AtPos(Offset);
                    var l = Lookup(lookupField, (UInt64)(d + lookupFieldOffset));
                    AddDataField(Offset,4);
                    AddParseLineToView(DataFieldIndex, posField, GetDataColor(DataFieldIndex), nameField, l + d.ToString() + " (0x" + d.ToString("X8") + ")");
                    MarkParsed(Offset, 4, DataFieldIndex);
                }
                else
                if (typeField == "float")
                {
                    var d = PD.GetFloatAtPos(Offset);
                    AddDataField(Offset,4);
                    AddParseLineToView(DataFieldIndex, posField, GetDataColor(DataFieldIndex), nameField, d.ToString());
                    MarkParsed(Offset, 4, DataFieldIndex);
                }
                else
                if (typeField == "pos")
                {
                    var x = PD.GetFloatAtPos(Offset);
                    var y = PD.GetFloatAtPos(Offset+4);
                    var z = PD.GetFloatAtPos(Offset+8);
                    AddDataField(Offset,12);
                    AddParseLineToView(DataFieldIndex, posField, GetDataColor(DataFieldIndex), nameField, "X:"+x.ToString("F") + "  Y:" + y.ToString("F") + "  Z:" + z.ToString("F"));
                    MarkParsed(Offset, 12, DataFieldIndex);
                }
                else
                if (typeField == "dir")
                {
                    var d = PD.GetByteAtPos(Offset);
                    var dir = ByteToRotation(d);
                    AddDataField(Offset,1);
                    AddParseLineToView(DataFieldIndex, posField, GetDataColor(DataFieldIndex), nameField, dir);
                    MarkParsed(Offset, 1, DataFieldIndex);
                }
                else
                if (typeField.StartsWith("string"))
                {
                    var sizeStr = typeField.Substring(6, typeField.Length - 6);
                    int size = -1;
                    if ((sizeStr != string.Empty) || (!int.TryParse(sizeStr,out size)))
                    {
                        size = -1;
                    }
                    var d = PD.GetStringAtPos(Offset, size);
                    string dHex = "";
                    foreach(char c in d)
                    {
                        if (dHex != string.Empty)
                            dHex += " ";
                        dHex += ((byte)c).ToString("X2");
                    }
                    if (d == string.Empty)
                        d = "null";

                    AddDataField(Offset,size);
                    AddParseLineToView(DataFieldIndex, posField, GetDataColor(DataFieldIndex), nameField, d + " ("+dHex+")");
                    if (size > 1)
                    {
                        MarkParsed(Offset, size, DataFieldIndex);
                    }
                    else
                    {
                        MarkParsed(Offset, d.Length , DataFieldIndex);
                    }
                }
                else
                if (typeField.StartsWith("data"))
                {
                    var sizeStr = typeField.Substring(4, typeField.Length - 4);
                    int size = 1;
                    if (!int.TryParse(sizeStr, out size))
                    {
                        size = 1;
                    }
                    var d = PD.GetDataAtPos(Offset, size);
                    AddDataField(Offset,size);
                    AddParseLineToView(DataFieldIndex, posField, GetDataColor(DataFieldIndex), nameField, d);
                    MarkParsed(Offset, size, DataFieldIndex);
                }
                else
                if (typeField == "ms")
                {
                    var d = PD.GetUInt32AtPos(Offset);
                    AddDataField(Offset,4);
                    AddParseLineToView(DataFieldIndex, posField, GetDataColor(DataFieldIndex), nameField, MSToString(d) + " ("+d.ToString()+")");
                    MarkParsed(Offset, 4, DataFieldIndex);
                }
                else
                if (typeField == "frames")
                {
                    var d = PD.GetUInt32AtPos(Offset);
                    AddDataField(Offset,4);
                    AddParseLineToView(DataFieldIndex, posField, GetDataColor(DataFieldIndex), nameField, FramesToString(d) + " (" + d.ToString() + ")");
                    MarkParsed(Offset, 4, DataFieldIndex);
                }
                else
                if (typeField == "vanatime")
                {
                    var d = PD.GetUInt32AtPos(Offset);
                    AddDataField(Offset,4);
                    var vt = new VanadielTime();
                    vt.FromVanadielIntTime((int)d);
                    AddParseLineToView(DataFieldIndex, posField, GetDataColor(DataFieldIndex), nameField, vt.LocalEarthTime.ToString("yyyy-MM-dd HH:mm:ss") + " ("+ d.ToString() +") => " + vt.ToString());
                    MarkParsed(Offset, 4, DataFieldIndex);
                }
                else
                if (typeField == "ip4")
                {
                    var d = PD.GetIP4AtPos(Offset);
                    AddDataField(Offset, 4);
                    AddParseLineToView(DataFieldIndex, posField, GetDataColor(DataFieldIndex), nameField, d);
                    MarkParsed(Offset, 4, DataFieldIndex);
                }
                else
                if (typeField == "linkshellstring")
                {
                    var d = PD.GetPackedString16AtPos(Offset,String6BitEncodeKeys.Linkshell);
                    AddDataField(Offset, 16);
                    AddParseLineToView(DataFieldIndex, posField, GetDataColor(DataFieldIndex), nameField, d);
                    MarkParsed(Offset, 16, DataFieldIndex);
                }
                else
                if (typeField == "inscribestring")
                {
                    var d = PD.GetPackedString16AtPos(Offset, String6BitEncodeKeys.Item);
                    AddDataField(Offset, 16);
                    AddParseLineToView(DataFieldIndex, posField, GetDataColor(DataFieldIndex), nameField, d);
                    MarkParsed(Offset, 16, DataFieldIndex);
                }
                else
                if (typeField == "bitflaglist")
                {
                    // Default to 64 bits if nothing provided
                    if (SubOffsetRange <= 0)
                        SubOffsetRange = 64;
                    AddDataField(Offset, SubOffsetRange / 8);
                    int foundCount = 0;
                    int posByte = Offset;
                    int posBit = SubOffset;
                    int bitNumber = 0;
                    while (posByte + (posBit / 8) < PD.RawBytes.Count)
                    {
                        bool thisBit = PD.GetBitAtPos(posByte, posBit);
                        if (thisBit)
                        {
                            foundCount++;
                            string d = "Bit" + bitNumber.ToString();
                            if (lookupField != string.Empty)
                            {
                                d += " => " + DataLookups.NLU(lookupField).GetValue((UInt64)(bitNumber + lookupFieldOffset));
                            }
                            AddParseLineToView(DataFieldIndex, posField, GetDataColor(DataFieldIndex), nameField + " - Bit"+bitNumber.ToString(), d);
                        }

                        bitNumber++;
                        posBit++;

                        if (bitNumber >= SubOffsetRange)
                            break;

                        while (posBit >= 8)
                        {
                            posBit -= 8;
                            posByte++;
                        }
                    }
                    if (foundCount <= 0)
                    {
                        AddParseLineToView(DataFieldIndex, posField, GetDataColor(DataFieldIndex), nameField, "No bits are set");
                    }
                    MarkParsed(Offset, SubOffsetRange / 8, DataFieldIndex);
                }
                else
                if (typeField == "bitflaglist2")
                {
                    // Default to 16 bits if nothing provided
                    if (SubOffsetRange <= 0)
                        SubOffsetRange = 16;
                    AddDataField(Offset, SubOffsetRange / 8);
                    int foundCount = 0;
                    int posByte = Offset;
                    int posBit = SubOffset;
                    int bitNumber = 0;
                    string d2 = string.Empty;
                    while (posByte + (posBit / 8) < PD.RawBytes.Count)
                    {
                        bool thisBit = PD.GetBitAtPos(posByte, posBit);
                        if (thisBit)
                        {
                            foundCount++;
                            string d = "Bit" + bitNumber.ToString();
                            if (lookupField != string.Empty)
                            {
                                d = DataLookups.NLU(lookupField).GetValue((UInt64)(bitNumber + lookupFieldOffset));
                            }
                            if (d.Trim(' ') == "")
                                d = "Bit" + bitNumber.ToString();
                            if (d2 != string.Empty)
                                d2 += ", ";
                            d2 += d;
                        }

                        bitNumber++;
                        posBit++;

                        if (bitNumber >= SubOffsetRange)
                            break;

                        while (posBit >= 8)
                        {
                            posBit -= 8;
                            posByte++;
                        }
                    }
                    if (foundCount <= 0)
                    {
                        AddParseLineToView(DataFieldIndex, posField, GetDataColor(DataFieldIndex), nameField, "No bits are set");
                    }
                    else
                    {
                        AddParseLineToView(DataFieldIndex, posField, GetDataColor(DataFieldIndex), nameField, d2);
                    }
                    MarkParsed(Offset, SubOffsetRange / 8, DataFieldIndex);
                }
                else
                if (typeField == "combatskill")
                {
                    var d = PD.GetUInt16AtPos(Offset);
                    var cappedString = "";
                    var skilllevel = (d & 0x7FFF);
                    AddDataField(Offset, 2);
                    if ((d & 0x8000) != 0)
                        cappedString = " (Capped)";
                    AddParseLineToView(DataFieldIndex, posField, GetDataColor(DataFieldIndex), nameField, skilllevel.ToString() + cappedString + " <= 0x"+d.ToString("X4"));
                    MarkParsed(Offset, 2, DataFieldIndex);
                }
                else
                if (typeField == "craftskill")
                {
                    var d = PD.GetUInt16AtPos(Offset);
                    var cappedString = "";
                    var craftlevel = ((d >> 5) & 0x03FF);
                    var craftrank = (d & 0x001F);
                    AddDataField(Offset, 2);
                    if ((d & 0x8000) != 0)
                        cappedString = " (Capped)";
                    AddParseLineToView(DataFieldIndex, posField, GetDataColor(DataFieldIndex), nameField, "Level: "+ craftlevel.ToString() + " Rank: " + craftrank.ToString() + " " + DataLookups.NLU(DataLookups.LU_CraftRanks).GetValue((UInt64)craftrank) + cappedString + " <= 0x" + d.ToString("X4"));
                    MarkParsed(Offset, 2, DataFieldIndex);
                }
                else
                if (typeField == "equipsetitem")
                {
                    AddDataField(Offset, 4);
                    string d = "";
                    if (PD.GetBitAtPos(Offset, 0))
                        d += "Active ";
                    if (PD.GetBitAtPos(Offset, 1))
                        d += "Bit1Set? ";
                    var bagid = PD.GetBitsAtPos(Offset, 2, 6);
                    var invindex = PD.GetByteAtPos(Offset + 1);
                    var item = PD.GetUInt32AtPos(Offset + 2);
                    d += DataLookups.NLU(DataLookups.LU_Container).GetValue((UInt64)bagid)+" ";
                    d += "InvIndex: " + invindex.ToString() + " " ;
                    d += "Item: " + DataLookups.NLU(DataLookups.LU_Item).GetValue((UInt64)item);
                    AddParseLineToView(DataFieldIndex, posField, GetDataColor(DataFieldIndex), nameField, "0x" + PD.GetUInt32AtPos(Offset).ToString("X4") + " => " + d);
                    MarkParsed(Offset, 4, DataFieldIndex);
                }
                else
                if (typeField == "equipsetitemlist")
                {
                    // Special field, SubOffset is used a position for the count byte
                    var counter = PD.GetByteAtPos(SubOffset);
                    // Otherwise works exactly the same as a loop of "equipsetitem"

                    AddDataField(Offset, counter * 4);
                    var c = 0;
                    for (int off = 0; (off < PD.RawBytes.Count - 4) && (c < counter); off += 4)
                    {
                        string d = "";
                        if (PD.GetBitAtPos(off + Offset, 0))
                            d += "Active ";
                        if (PD.GetBitAtPos(off + Offset, 1))
                            d += "Bit1Set? ";
                        var bagid = PD.GetBitsAtPos(off + Offset, 2, 6);
                        var invindex = PD.GetByteAtPos(off + Offset + 1);
                        var item = PD.GetUInt32AtPos(off + Offset + 2);
                        d += DataLookups.NLU(DataLookups.LU_Container).GetValue((UInt64)bagid) + " ";
                        d += "InvIndex: " + invindex.ToString() + " ";
                        d += "Item: " + DataLookups.NLU(DataLookups.LU_Item).GetValue((UInt64)item);
                        AddParseLineToView(DataFieldIndex, posField, GetDataColor(DataFieldIndex), nameField + " #"+c.ToString(), "0x" + PD.GetUInt32AtPos(off + Offset).ToString("X4") + " => " + d);
                        c++;
                    }
                    MarkParsed(Offset, counter * 4, DataFieldIndex);
                }
                else
                if (typeField == "abilityrecastlist")
                {
                    string d = "";
                    int off = Offset;
                    int counter = SubOffset;
                    for(int c = 0; c < counter; c++)
                    {
                        if (off > PD.RawBytes.Count - 8)
                            break;

                        var aID = PD.GetByteAtPos(off + 3);
                        var aDur = PD.GetUInt16AtPos(off);

                        d = "";
                        d += "ID: 0x" + aID.ToString("X2") + " (" + DataLookups.NLU(DataLookups.LU_ARecast).GetValue(aID) + ") ";
                        d += "Duration: " + aDur.ToString() + " - ";
                        d += "byte@2: 0x" + PD.GetByteAtPos(off + 2).ToString("X2") + " ";
                        d += "uint32@4: 0x" + PD.GetUInt32AtPos(off + 4).ToString("X8") + " ";

                        // Only show used recast timers
                        if ((aID != 0) || (c == 0))
                        {
                            AddDataField(off, 8);
                            AddParseLineToView(DataFieldIndex, "0x"+off.ToString("X2") , GetDataColor(DataFieldIndex), nameField + " #" + c.ToString(), d);
                            MarkParsed(off, 8, DataFieldIndex);
                        }
                        off += 8;
                    }
                }
                else
                if (typeField == "blacklistentry")
                {
                    var pID = PD.GetUInt32AtPos(Offset);
                    var pName = PD.GetStringAtPos(Offset + 4);
                    AddDataField(Offset, 20);
                    AddParseLineToView(DataFieldIndex, posField, GetDataColor(DataFieldIndex), nameField, "ID: 0x" + pID.ToString("X8") + " => " + pName);
                    MarkParsed(Offset, 20, DataFieldIndex);
                }
                else
                if (typeField == "meritentries")
                {
                    // Special field, SubOffset is used a position for the count byte
                    var counter = PD.GetByteAtPos(SubOffset);
                    // Otherwise works exactly the same as a loop of "equipsetitem"

                    AddDataField(Offset, counter * 4);
                    var c = 0;
                    for (int off = 0; (off < PD.RawBytes.Count - 4) && (c < counter); off += 4)
                    {
                        var meritID = PD.GetUInt16AtPos(off + Offset);
                        var meritNextCost = PD.GetByteAtPos(off + Offset + 2);
                        var meritValue = PD.GetUInt32AtPos(off + Offset + 3);
                        string d = "";
                        d += "ID: 0x" + meritID.ToString("X4") + " (" + DataLookups.NLU(DataLookups.LU_Merit).GetValue((UInt64)meritID) + ") - ";
                        d += "Next Cost: " + meritNextCost.ToString() + " - ";
                        d += "Value: " + meritValue.ToString();
                        AddParseLineToView(DataFieldIndex, posField, GetDataColor(DataFieldIndex), nameField + " #" + c.ToString(), d);
                        c++;
                    }
                    MarkParsed(Offset, counter * 4, DataFieldIndex);
                }
                else
                if( typeField == "playercheckitems")
                {
                    // SubOffset is the adress for the counter to use, defaults to previous byte
                    int counter ;
                    if (SubOffset < 1)
                        counter = PD.GetByteAtPos(Offset - 1);
                    else
                        counter = PD.GetByteAtPos(SubOffset);

                    var n = Offset;
                    var c = 0;
                    while ((n <= (PD.RawBytes.Count - 28)) && (c < counter))
                    {
                        c++;
                        UInt16 idVal = PD.GetUInt16AtPos(n);
                        UInt16 slotVal = PD.GetUInt16AtPos(n+2);
                        byte unkVal = PD.GetByteAtPos(n+3);
                        string idStr = "0x" + idVal.ToString("X4") + " => " + DataLookups.NLU(DataLookups.LU_Item).GetValue(idVal);
                        string slotStr = "0x" + slotVal.ToString("X4") + " => " + DataLookups.NLU(DataLookups.LU_EquipmentSlots).GetValue(slotVal);
                        string unkStr = "0x" + unkVal.ToString("X4") + " (" + unkVal.ToString()+")" ;
                        string extdataStr = PD.GetDataAtPos(n + 4, 24);

                        AddDataField(n, 28);
                        AddParseLineToView(DataFieldIndex, "0x" + n.ToString("X"), GetDataColor(DataFieldIndex), nameField + " #" + c.ToString()+" ID", idStr);
                        AddParseLineToView(DataFieldIndex, "0x" + n.ToString("X"), GetDataColor(DataFieldIndex), nameField + " #" + c.ToString() + " Slot", slotStr);
                        AddParseLineToView(DataFieldIndex, "0x" + n.ToString("X"), GetDataColor(DataFieldIndex), nameField + " #" + c.ToString() + " ???", unkStr);
                        AddParseLineToView(DataFieldIndex, "0x" + n.ToString("X"), GetDataColor(DataFieldIndex), nameField + " #" + c.ToString() + " ExtData", extdataStr);
                        MarkParsed(n, 28, DataFieldIndex);
                        n += 28;
                    }

                }
                else
                if (typeField == "bufficons")
                {
                    // SubOffset is counter to use, defaults to 1
                    int counter;
                    if (SubOffset < 1)
                        counter = 1;
                    else
                        counter = SubOffset ;

                    var n = Offset;
                    var c = 0;
                    while ((n <= PD.RawBytes.Count - 2) && (c < counter))
                    {
                        c++;
                        UInt16 iconVal = PD.GetUInt16AtPos(n);
                        string iconStr = "0x" + iconVal.ToString("X4") + " => " + DataLookups.NLU(DataLookups.LU_Buffs).GetValue(iconVal);

                        AddDataField(n, 2);
                        AddParseLineToView(DataFieldIndex, "0x" + n.ToString("X"), GetDataColor(DataFieldIndex), nameField + " #" + c.ToString(), iconStr);
                        MarkParsed(n, 2, DataFieldIndex);
                        n += 2;
                    }

                }
                else
                if (typeField == "bufftimers")
                {
                    // SubOffset is counter to use, defaults to 1
                    int counter;
                    if (SubOffset < 1)
                        counter = 1;
                    else
                        counter = SubOffset;

                    var n = Offset;
                    var c = 0;
                    while ((n <= PD.RawBytes.Count - 4) && (c < counter))
                    {
                        c++;
                        Int32 timerVal = PD.GetInt32AtPos(n);
                        string timerStr ;
                        if (timerVal == 0)
                            timerStr = "0x" + timerVal.ToString("X8") + " => Not defined";
                        else
                        if (timerVal == 0x7FFFFFFF)
                            timerStr = "0x" + timerVal.ToString("X8") + " => Always";
                        else
                            timerStr = "0x" + timerVal.ToString("X8") + " => " + MSToString((uint)timerVal);

                        AddDataField(n, 4);
                        AddParseLineToView(DataFieldIndex, "0x" + n.ToString("X"), GetDataColor(DataFieldIndex), nameField + " #" + c.ToString(), timerStr);
                        MarkParsed(n, 4, DataFieldIndex);
                        n += 4;
                    }

                }
                else
                if (typeField == "buffs")
                {
                    // SubOffset is counter to use, defaults to 1
                    // SubOffsetRange is the starting offset for the timers, defaults to right after the icons
                    int counter;
                    int timerOffset;
                    if (SubOffset < 1)
                        counter = 1;
                    else
                        counter = SubOffset;

                    if (SubOffsetRange < 1)
                        timerOffset = Offset + (counter * 2);
                    else
                        timerOffset = SubOffsetRange;

                    var n = Offset;
                    var t = timerOffset;
                    var c = 0;
                    while ((n <= PD.RawBytes.Count - 2) && (t <= PD.RawBytes.Count - 4) && (c < counter))
                    {
                        c++;

                        UInt16 iconVal = PD.GetUInt16AtPos(n);
                        string iconStr = "0x" + iconVal.ToString("X4") + " => " + DataLookups.NLU(DataLookups.LU_Buffs).GetValue(iconVal);

                        AddDataField(n, 2);
                        AddParseLineToView(DataFieldIndex, "0x" + n.ToString("X"), GetDataColor(DataFieldIndex), nameField + " #" + c.ToString()+" Icon", iconStr);
                        MarkParsed(n, 2, DataFieldIndex);

                        Int32 timerVal = PD.GetInt32AtPos(t);
                        string timerStr;
                        if (timerVal == 0)
                            timerStr = "0x" + timerVal.ToString("X8") + " => Not defined";
                        else
                        if (timerVal == 0x7FFFFFFF)
                            timerStr = "0x" + timerVal.ToString("X8") + " => Always";
                        else
                            timerStr = "0x" + timerVal.ToString("X8") + " => " + MSToString((uint)timerVal);

                        // AddDataField(t, 4);
                        AddParseLineToView(DataFieldIndex, "0x" + t.ToString("X"), GetDataColor(DataFieldIndex), nameField + " #" + c.ToString()+" Timer", timerStr);
                        MarkParsed(t, 4, DataFieldIndex);
                        n += 2;
                        t += 4;
                    }

                }
                else
                if (typeField == "jobpointentries")
                {
                    // SubOffset is counter to use, defaults to "until end of packet"
                    int counter;
                    if (SubOffset < 1)
                        counter = (PD.RawBytes.Count - Offset) / 4 ;
                    else
                        counter = SubOffset;

                    var n = Offset;
                    var c = 0;
                    while ((n <= PD.RawBytes.Count - 4) && (c < counter))
                    {
                        c++;
                        string d = "";
                        UInt16 jpVal = PD.GetUInt16AtPos(n);
                        UInt16 jp2Val = PD.GetUInt16AtPos(n+2);
                        var jpUnkVal = PD.GetBitsAtPos(n + 2, 0, 10);
                        var jpLevelVal = PD.GetBitsAtPos(n + 3, 2, 6);

                        d += "ID: 0x" + jpVal.ToString("X4") + " => " + DataLookups.NLU(DataLookups.LU_JobPoint).GetValue(jpVal) + " + ";
                        d += "Para: 0x" + jp2Val.ToString("X4") + " => ";
                        d += "Level: " + jpLevelVal.ToString() + " - ";
                        d += "Next?: " + jpUnkVal.ToString() +" (0x" + jpUnkVal.ToString("X")+")";

                        AddDataField(n, 4);
                        if ((jpVal != 0) && (jp2Val != 0))
                            AddParseLineToView(DataFieldIndex, "0x" + n.ToString("X"), GetDataColor(DataFieldIndex), nameField + " #" + c.ToString(), d);
                        MarkParsed(n, 4, DataFieldIndex);
                        n += 4;
                    }

                }
                else
                if (typeField == "shopitems")
                {
                    // SubOffset is counter to use, defaults to "until end of packet"
                    int counter;
                    if (SubOffset < 1)
                        counter = (PD.RawBytes.Count - Offset) / 12;
                    else
                        counter = SubOffset;

                    var n = Offset;
                    var c = 0;
                    while ((n <= PD.RawBytes.Count - 12) && (c < counter))
                    {
                        c++;
                        string d = "";
                        UInt32 gilVal = PD.GetUInt32AtPos(n);
                        UInt16 itemVal = PD.GetUInt16AtPos(n + 4);
                        byte slotVal = PD.GetByteAtPos(n + 6);
                        byte unkVal = PD.GetByteAtPos(n + 7);
                        UInt16 skillVal = PD.GetUInt16AtPos(n + 8);
                        UInt16 rankVal = PD.GetUInt16AtPos(n + 10);

                        d += "Item: 0x" + itemVal.ToString("X4") + "  ";
                        d += "Gil: " + gilVal.ToString().PadLeft(7) + "  ";
                        d += "Skill: 0x" + skillVal.ToString("X4") + "  ";
                        d += "Rank: 0x" + rankVal.ToString("X4") + "  ";
                        d += "Slot: " + slotVal.ToString().PadRight(2) + "  " ;
                        d += "???: 0x" + unkVal.ToString("X2");
                        d += " => " + DataLookups.NLU(DataLookups.LU_Item).GetValue(itemVal);

                        AddDataField(n, 12);
                        AddParseLineToView(DataFieldIndex, "0x" + n.ToString("X"), GetDataColor(DataFieldIndex), nameField + " #" + c.ToString(), d);
                        MarkParsed(n, 12, DataFieldIndex);
                        n += 12;
                    }

                }
                else
                if (typeField == "guildshopitems")
                {
                    int counter = 30 ;
                    var n = Offset;
                    var c = 0;
                    while ((n <= PD.RawBytes.Count - 8) && (c < counter))
                    {
                        c++;
                        string d = "";
                        UInt32 gilVal = PD.GetUInt32AtPos(n + 4);
                        UInt16 itemVal = PD.GetUInt16AtPos(n);
                        byte stockVal = PD.GetByteAtPos(n + 2);
                        byte stockmaxVal = PD.GetByteAtPos(n + 3);

                        d += "Item: 0x" + itemVal.ToString("X4") + "  ";
                        d += "Gil: " + gilVal.ToString().PadLeft(7) + "  ";
                        d += "Stock: " + stockVal.ToString() + " / " + stockmaxVal.ToString() ;
                        d += " => " + DataLookups.NLU(DataLookups.LU_Item).GetValue(itemVal);

                        AddDataField(n, 8);
                        AddParseLineToView(DataFieldIndex, "0x" + n.ToString("X"), GetDataColor(DataFieldIndex), nameField + " #" + c.ToString(), d);
                        MarkParsed(n, 8, DataFieldIndex);
                        n += 8;
                    }

                }
                else
                if (typeField == "jobpoints")
                {
                    var cpVal = PD.GetUInt16AtPos(Offset);
                    var jpVal = PD.GetUInt16AtPos(Offset + 2);
                    var spentVal = PD.GetUInt16AtPos(Offset + 4);
                    string d = "";
                    d += cpVal.ToString() + " CP  ";
                    d += jpVal.ToString() + " JP  ";
                    d += spentVal.ToString() + " Spent JP  ";

                    AddDataField(Offset, 6);
                    AddParseLineToView(DataFieldIndex, posField, GetDataColor(DataFieldIndex), nameField, d);
                    MarkParsed(Offset, 6, DataFieldIndex);
                }
                else
                if (typeField == "roequest")
                {
                    var idVal = PD.GetBitsAtPos(Offset,0,12);
                    var progressVal = PD.GetBitsAtPos(Offset+1, 4, 20);
                    var maxVal = DataLookups.NLU(DataLookups.LU_RoE).GetExtra((UInt64)idVal);
                    if (maxVal == "")
                        maxVal = "???";
                    string d = "";
                    d += "ID; 0x" + idVal.ToString("X3") + " => " + DataLookups.NLU(DataLookups.LU_RoE).GetValue((UInt64)idVal)+"  " ;
                    d += "Progress; " + progressVal.ToString() + " / " + maxVal ;

                    AddDataField(Offset, 4);
                    AddParseLineToView(DataFieldIndex, posField, GetDataColor(DataFieldIndex), nameField, d);
                    MarkParsed(Offset, 4, DataFieldIndex);
                }
                else
                if (typeField == "packet-in-0x028")
                {
                    // This packet is too complex to do the normal way (for now)
                    var bytesfor0x028 = Parse_Packet_In_0x028(PD,ref DataFieldIndex);
                    // Mark any byts that didn't get marked as parsed, as parsed
                    for (int i = 0; i < bytesfor0x028; i++)
                    {
                        if (ParsedBytes[i] == 0)
                            ParsedBytes[i] = 0xFF;
                    }
                }
                else
                {
                    // Unknown field type
                    AddDataField(Offset,0);
                    AddParseLineToView(0xFF, "L " + parseLineNumber.ToString(), GetDataColor(DataFieldIndex), "Parse Error", "Unknown Field Type: " + typeField);
                }

            }

            // List unparsed bytes
            for (int i = 4; i < PD.RawBytes.Count(); i++)
            {
                if ((i <= (PD.RawBytes.Count()-4)) && (ParsedBytes[i] == 0) && (ParsedBytes[i+1] == 0) && (ParsedBytes[i+2] == 0) && (ParsedBytes[i+3] == 0))
                {
                    AddDataField(i, 4);
                    AddParseLineToView(DataFieldIndex,
                        "0x" + i.ToString("X2"),
                        Color.DarkGray,
                        "??_UInt32 (@" + i.ToString() + ")",
                        "0x" + PD.GetUInt32AtPos(i).ToString("X8") + " (" + PD.GetUInt32AtPos(i).ToString() + ")");
                    MarkParsed(i, 4, DataFieldIndex);
                    i += 3; // move forward a extra 3 bytes
                }
                else
                if (ParsedBytes[i] == 0)
                {
                    AddDataField(i, 1);
                    AddParseLineToView(DataFieldIndex, 
                        "0x" + i.ToString("X2"), 
                        Color.DarkGray, 
                        "??_Byte (@" + i.ToString() + ")", 
                        "0x" + PD.GetByteAtPos(i).ToString("X2") + " (" + PD.GetByteAtPos(i).ToString() + ")");
                    MarkParsed(i, 1, DataFieldIndex);
                }
            }

            ToGridView(DGV);
        }

    }

    public class SearchParameters
    {
        public bool SearchIncoming { get; set; }
        public bool SearchOutgoing { get; set; }
        public bool SearchByPacketID { get; set; }
        public UInt16 SearchPacketID { get; set; }
        public bool SearchBySync { get; set; }
        public UInt16 SearchSync { get; set; }
        public bool SearchByByte { get; set; }
        public byte SearchByte { get; set; }
        public bool SearchByUInt16 { get; set; }
        public UInt16 SearchUInt16 { get; set; }
        public bool SearchByUInt32 { get; set; }
        public UInt32 SearchUInt32 { get; set; }

        public void Clear()
        {
            SearchIncoming = false;
            SearchOutgoing = false;
            SearchByPacketID = false;
            SearchBySync = false;
            SearchByByte = false;
            SearchByUInt16 = false;
            SearchByUInt32 = false;
        }

        public void CopyFrom(SearchParameters p)
        {
            SearchIncoming = p.SearchIncoming;
            SearchOutgoing = p.SearchOutgoing;
            SearchByPacketID = p.SearchByPacketID;
            SearchPacketID = p.SearchPacketID;
            SearchBySync = p.SearchBySync;
            SearchSync = p.SearchSync;
            SearchByByte = p.SearchByByte;
            SearchByte = p.SearchByte;
            SearchByUInt16 = p.SearchByUInt16;
            SearchUInt16 = p.SearchUInt16;
            SearchByUInt32 = p.SearchByUInt32;
            SearchUInt32 = p.SearchUInt32;
        }
    }

}
