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
        List<Color> DataColors = new List<Color>();

        const int columnOffset = 0;
        const int columnVAR = 1;
        const int columnDATA = 2;
        const int columnSize = 3;
        public UInt16 ThisPacketID { get; set; }
        PacketLogTypes ThisPacketLogType { get; set; }
        List<string> RawParseData = new List<string>();
        public List<byte> ParsedBytes = new List<byte>();
        public PacketData PD;
        public List<string> SwitchBlocks = new List<string>();
        public string LastSwitchedBlock = "";
        private string[] CompasDirectionNames = new string[16] { "E", "ESE", "SE", "SSE", "S", "SSW", "SW", "WSW", "W", "WNW", "NW", "NNW", "N", "NNE", "NE", "ENE" };

        public PacketParser(UInt16 aPacketID,PacketLogTypes aPacketLogType)
        {
            DataColors.Add(Color.Black);
            DataColors.Add(Color.Red);
            DataColors.Add(Color.Green);
            DataColors.Add(Color.Blue);
            DataColors.Add(Color.Purple);
            DataColors.Add(Color.DarkGray);
            DataColors.Add(Color.Maroon);
            DataColors.Add(Color.Navy);

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
            for (int i = 0; i < PD.RawBytes.Count(); i++)
            {
                ParsedBytes.Add(0x00); // 0 = unparsed
            }
        }

/*
        CONST
            CompasDirectionNames: Array[0..15] of String = ('E', 'ESE', 'SE', 'SSE',
                'S', 'SSW', 'SW', 'WSW', 'W', 'WNW', 'NW', 'NNW', 'N', 'NNE', 'NE', 'ENE');

        Function ByteToRotation(B: Byte): String;
        VAR
          I: Integer;
        Begin
          I := B* 360;
          I := I div 256;
                Result := CompasDirectionNames[(B div 16) mod 16] + ' (0x' + IntToHex(B, 2) +
            ' ≈ ' + IntToStr(I) + '°)';
        End;
*/

        private string ByteToRotation(byte b)
        {
            int i = (b * 360) / 256;
            return CompasDirectionNames[(i / 16) % 16] + " (0x"+b.ToString("X2")+ " ≈ " + i.ToString() + "°)";
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

        private string VanaTimeToString(UInt32 v)
        {
            const UInt64 VTIME_BASEDATE = 1009810800;
            // unix epoch - 1009810800 = se epoch (in earth seconds)
            const UInt64 VTIME_YEAR = 518400; // 360 * GameDay
            const UInt64 VTIME_MONTH = 43200; // 30 * GameDay
            const UInt64 VTIME_WEEK = 11520; // 8 * GameDay
            const UInt64 VTIME_DAY = 1440; // 24 hours * GameHour
            const UInt64 VTIME_HOUR = 60; // 60 minutes
            const UInt64 VTIME_FIRSTYEAR = 886;

            var VanaDate = v;
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

        private void AddParseLineToView(DataGridView DGV,string POSString, Color POSColor, string VARName, string DATAString)
        {
            int thisRow = DGV.Rows.Add();
            DGV.Rows[thisRow].Cells[columnOffset].Value = POSString;
            DGV.Rows[thisRow].Cells[columnOffset].Style.ForeColor = POSColor;
            DGV.Rows[thisRow].Cells[columnVAR].Value = VARName;
            DGV.Rows[thisRow].Cells[columnVAR].Style.ForeColor = POSColor;
            DGV.Rows[thisRow].Cells[columnDATA].Value = DATAString;
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

        public void ParseToDataGridView(DataGridView DGV,string ActiveSwitchBlock)
        {
            byte DataFieldIndex = 0; // header is considered 0

            void AddDataField(int StartPos, int FieldByteSize)
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

            // Header
            DGV.Rows.Clear();
            DGV.ColumnCount = 3;

            DGV.Columns[columnOffset].HeaderText = "Pos";
            DGV.Columns[columnOffset].Width = 72;

            DGV.Columns[columnVAR].HeaderText = "Name";
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
                        "0x00", Color.DarkSlateGray, 
                        "PacketID", 
                        "OUT 0x" + PD.PacketID.ToString("X3") + " - " + DataLookups.NLU(DataLookups.LU_PacketOut).GetValue(PD.PacketID));
                    break;
                case PacketLogTypes.Incoming:
                    AddParseLineToView(DGV,
                        "0x00", Color.DarkGray,
                        "PacketID",
                        "IN 0x" + PD.PacketID.ToString("X3") + " - " + DataLookups.NLU(DataLookups.LU_PacketIn).GetValue(PD.PacketID));
                    break;
                default:
                    AddParseLineToView(DGV,
                        "0x00", Color.DarkSlateGray,
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
                    AddParseLineToView(DGV, "L " + parseLineNumber.ToString(), Color.Red, "Parse Error", "Need at least 2 fields");
                    continue;
                }
                string typeField = fields[0].ToLower().Trim(' ');
                string posField = fields[1].ToLower().Trim(' ');
                string nameField = "";
                string descriptionField = "";
                string lookupField = "";
                if (fields.Length > 2)
                {
                    nameField = fields[2].Trim(' ');
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
                    lookupField = typeField.Substring(lookupFieldSplitPos + 1, typeField.Length - lookupFieldSplitPos - 1);
                    typeField = typeField.Substring(0,lookupFieldSplitPos);
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
                        AddParseLineToView(DGV, "L " + parseLineNumber.ToString(), Color.Red, "Parse Error", "Invalid Offset Value in: " + posField);
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
                            AddParseLineToView(DGV, "L " + parseLineNumber.ToString(), Color.Red, "Parse Error", "Invalid SubOffset Value in: " + posField);
                            continue;
                        }

                        if (!DataLookups.TryFieldParse(subOffsetRangeStr, out SubOffsetRange))
                        {
                            Offset = 0;
                            AddParseLineToView(DGV, "L " + parseLineNumber.ToString(), Color.Red, "Parse Error", "Invalid SubOffsetRange Value in: " + posField);
                            continue;
                        }
                    }
                    else
                    {
                        // doesn't have a range
                        if (!DataLookups.TryFieldParse(subOffsetStr, out SubOffset))
                        {
                            Offset = 0;
                            AddParseLineToView(DGV, "L " + parseLineNumber.ToString(), Color.Red, "Parse Error", "Invalid SubOffset Value in: " + posField);
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
                        AddParseLineToView(DGV, "L " + parseLineNumber.ToString(), Color.Red, "Parse Error", "Invalid Offset Value in: "+posField);
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
                        // Compares BYTE value at checkpos, if checkval matches, activate blockname as current block

                        int switchTargetValue = 0;
                        if (DataLookups.TryFieldParse(descriptionField, out switchTargetValue))
                        {
                            int posVal = 0;
                            posVal = (int)PD.GetBitsAtPos(Offset,SubOffset,SubOffsetRange);

                            // Switchval seems valid, next compare it
                            if (posVal == switchTargetValue)
                            {
                                ActiveSwitchBlock = nameField;
                                AllowAutoSwitchBlock = false;
                                // Debug Info
                                AddParseLineToView(DGV, "L " + parseLineNumber.ToString(), Color.Red, "Switch", "Activate Block: " + ActiveSwitchBlock);
                                LastSwitchedBlock = ActiveSwitchBlock;
                                continue;
                            }
                        }

                    }
                }
                else
                if (typeField == "info")
                {
                    // Info line for the view
                    AddDataField(Offset,0);
                    AddParseLineToView(DGV, posField, GetDataColor(DataFieldIndex), nameField, descriptionField);
                }
                else
                if (typeField == "byte")
                {
                    var d = PD.GetByteAtPos(Offset);
                    var l = Lookup(lookupField,(UInt64)d);
                    AddDataField(Offset,1);
                    AddParseLineToView(DGV, posField, GetDataColor(DataFieldIndex), nameField, l + d.ToString() + " - 0x" + d.ToString("X2") + " - " + ByteToBits(d) + " - '" + (char)d + "'");
                    MarkParsed(Offset, 1, DataFieldIndex);
                }
                else
                if ((typeField == "bit") || (typeField == "bool"))
                {
                    var d = PD.GetBitAtPos(Offset,SubOffset);
                    AddDataField(Offset,1);
                    AddParseLineToView(DGV, posField, GetDataColor(DataFieldIndex), nameField, d.ToString());
                    MarkParsed(Offset, 1, DataFieldIndex);
                }
                else
                if (typeField == "bits")
                {
                    var d = PD.GetBitsAtPos(Offset, SubOffset, SubOffsetRange);
                    var l = Lookup(lookupField, (UInt64)d);
                    AddDataField(Offset,1);
                    AddParseLineToView(DGV, posField, GetDataColor(DataFieldIndex), nameField, l + "0x"+d.ToString("X") + " - " + d.ToString(""));
                    MarkParsed(Offset, (SubOffsetRange / 8), DataFieldIndex);
                }
                else
                if (typeField == "uint16")
                {
                    var d = PD.GetUInt16AtPos(Offset);
                    var l = Lookup(lookupField, (UInt64)d);
                    AddDataField(Offset,2);
                    AddParseLineToView(DGV, posField, GetDataColor(DataFieldIndex), nameField, l + d.ToString() + " (0x" + d.ToString("X4") + ")");
                    MarkParsed(Offset, 2, DataFieldIndex);
                }
                else
                if (typeField == "int16")
                {
                    var d = PD.GetInt16AtPos(Offset);
                    var l = Lookup(lookupField, (UInt64)d);
                    AddDataField(Offset,2);
                    AddParseLineToView(DGV, posField, GetDataColor(DataFieldIndex), nameField, l + d.ToString() + " (0x" + d.ToString("X4") + ")");
                    MarkParsed(Offset, 2, DataFieldIndex);
                }
                else
                if (typeField == "uint32")
                {
                    var d = PD.GetUInt32AtPos(Offset);
                    var l = Lookup(lookupField, (UInt64)d);
                    AddDataField(Offset,4);
                    AddParseLineToView(DGV, posField, GetDataColor(DataFieldIndex), nameField, l + d.ToString() + " (0x" + d.ToString("X8") + ")");
                    MarkParsed(Offset, 4, DataFieldIndex);
                }
                else
                if (typeField == "int32")
                {
                    var d = PD.GetInt32AtPos(Offset);
                    var l = Lookup(lookupField, (UInt64)d);
                    AddDataField(Offset,4);
                    AddParseLineToView(DGV, posField, GetDataColor(DataFieldIndex), nameField, l + d.ToString() + " (0x" + d.ToString("X8") + ")");
                    MarkParsed(Offset, 4, DataFieldIndex);
                }
                else
                if (typeField == "float")
                {
                    var d = PD.GetFloatAtPos(Offset);
                    AddDataField(Offset,4);
                    AddParseLineToView(DGV, posField, GetDataColor(DataFieldIndex), nameField, d.ToString());
                    MarkParsed(Offset, 4, DataFieldIndex);
                }
                else
                if (typeField == "pos")
                {
                    var x = PD.GetFloatAtPos(Offset);
                    var y = PD.GetFloatAtPos(Offset+4);
                    var z = PD.GetFloatAtPos(Offset+8);
                    AddDataField(Offset,12);
                    AddParseLineToView(DGV, posField, GetDataColor(DataFieldIndex), nameField, "X:"+x.ToString("F") + "  Y:" + y.ToString("F") + "  Z:" + z.ToString("F"));
                    MarkParsed(Offset, 12, DataFieldIndex);
                }
                else
                if (typeField == "dir")
                {
                    var d = PD.GetByteAtPos(Offset);
                    var dir = ByteToRotation(d);
                    AddDataField(Offset,1);
                    AddParseLineToView(DGV, posField, GetDataColor(DataFieldIndex), nameField, dir);
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
                    AddParseLineToView(DGV, posField, GetDataColor(DataFieldIndex), nameField, d + " ("+dHex+")");
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
                    AddParseLineToView(DGV, posField, GetDataColor(DataFieldIndex), nameField, d);
                    MarkParsed(Offset, size, DataFieldIndex);
                }
                else
                if (typeField == "ms")
                {
                    var d = PD.GetUInt32AtPos(Offset);
                    AddDataField(Offset,4);
                    AddParseLineToView(DGV, posField, GetDataColor(DataFieldIndex), nameField, MSToString(d));
                    MarkParsed(Offset, 4, DataFieldIndex);
                }
                else
                if (typeField == "frames")
                {
                    var d = PD.GetUInt32AtPos(Offset);
                    AddDataField(Offset,4);
                    AddParseLineToView(DGV, posField, GetDataColor(DataFieldIndex), nameField, FramesToString(d));
                    MarkParsed(Offset, 4, DataFieldIndex);
                }
                else
                if (typeField == "vanatime")
                {
                    var d = PD.GetUInt32AtPos(Offset);
                    AddDataField(Offset,4);
                    AddParseLineToView(DGV, posField, GetDataColor(DataFieldIndex), nameField, VanaTimeToString(d));
                    MarkParsed(Offset, 4, DataFieldIndex);
                }
                else
                {
                    // Unknown field type
                    AddDataField(Offset,0);
                    AddParseLineToView(DGV, "L " + parseLineNumber.ToString(), GetDataColor(DataFieldIndex), "Parse Error", "Unknown Field Type: " + typeField);
                }

            }

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
