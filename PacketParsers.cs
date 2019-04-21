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
    public struct ParsedViewLine
    {
        public string Pos ;
        public string Var ;
        public string Data ;
        public byte FieldIndex;
        public Color FieldColor;
    }

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

        private string VanaTimeToString(UInt32 v)
        {
            // const UInt64 VTIME_BASEDATE = 1009810800;
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

        public void ToGridView(DataGridView DGV)
        {
            if (DGV.Tag != null)
                return;
            DGV.Tag = 1;
            // Header
            //DGV.Rows.Clear();
            DGV.ColumnCount = 3;

            DGV.Columns[columnOffset].HeaderText = "Pos";
            DGV.Columns[columnOffset].Width = 72;

            DGV.Columns[columnVAR].HeaderText = "Name";
            DGV.Columns[columnVAR].Width = 128;

            DGV.Columns[columnDATA].HeaderText = "Data";
            DGV.Columns[columnDATA].Width = DGV.Width - DGV.Columns[columnOffset].Width - DGV.Columns[columnVAR].Width - 20 ;

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

        private int Parse_Packet_In_0x028(PacketData PD,ref byte DataFieldIndex)
        {
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
            AddParseLineToView(DataFieldIndex, "0x0A", GetDataColor(DataFieldIndex), "Action Category", pActionCategory.ToString() + " => ");
            MarkParsed(0x0A, 1, DataFieldIndex);

            // TODO: add the rest

            /*
              AddSGRow(SG, $A, 'Action Cat', IntToStr(pActionCategory) + ' - ' +
                ActionCategoryToStr(pActionCategory), 1);
              AddSGRow(SG, $A, 'Action ID', IntToStr(pActionID), 2);
              AddSGRow(SG, $C, 'Unknown1', IntToStr(pUnknown1), 2);
              AddSGRow(SG, $E, 'Recast', IntToStr(pRecast), 4);

              FirstTargetOffset ;= 150; // $12;6
              LastBit ;= PD.RawSize * 8;

              Offset ;= FirstTargetOffset;
              pTargetCountLoopCounter ;= 0;

              While (Offset < LastBit) and (pTargetCountLoopCounter < pTargetCount) Do
              Begin
                pTargetCountLoopCounter ;= pTargetCountLoopCounter + 1;

                pActionTargetID ;= PD.GetBitsAtPos(Offset, 32);
                AddSGRow(SG, (Offset div 8), '#' + IntToStr(pTargetCountLoopCounter) +
                  ' ; Target ID', '0x' + IntToHex(pActionTargetID, 8) + ' - ' +
                  IntToStr(pActionTargetID), 4);
                Offset ;= Offset + 32;

                pActionTargetIDSize ;= PD.GetBitsAtPos(Offset, 4);
                AddSGRow(SG, (Offset div 8), '#' + IntToStr(pTargetCountLoopCounter) +
                  ' ; Count', IntToStr(pActionTargetIDSize), 1);
                Offset ;= Offset + 4;

                tTargetEffectLoopCounter ;= 0;
                While (Offset < LastBit) and
                  (tTargetEffectLoopCounter < pActionTargetIDSize) Do
                Begin
                  tTargetEffectLoopCounter ;= tTargetEffectLoopCounter + 1;

                  tReaction ;= PD.GetBitsAtPos(Offset, 5);
                  AddSGRow(SG, (Offset div 8), ' #' + IntToStr(pTargetCountLoopCounter) +
                    ' ' + IntToStr(tTargetEffectLoopCounter) + '/' +
                    IntToStr(pActionTargetIDSize) + ' ; Reaction',
                    IntToStr(tReaction) + ' - ' + ActionReactionToStr(tReaction), 1);
                  Offset ;= Offset + 5;

                  tAnimation ;= PD.GetBitsAtPos(Offset, 12);
                  AddSGRow(SG, (Offset div 8), ' #' + IntToStr(pTargetCountLoopCounter) +
                    ' ' + IntToStr(tTargetEffectLoopCounter) + '/' +
                    IntToStr(pActionTargetIDSize) + ' ; Animation',
                    '0x' + IntToHex(tAnimation, 4) + ' - ' + IntToStr(tAnimation), 2);
                  Offset ;= Offset + 12;

                  tSpecialEffect ;= PD.GetBitsAtPos(Offset, 7);
                  AddSGRow(SG, (Offset div 8), ' #' + IntToStr(pTargetCountLoopCounter) +
                    ' ' + IntToStr(tTargetEffectLoopCounter) + '/' +
                    IntToStr(pActionTargetIDSize) + ' ; SpecialEffect',
                    '0x' + IntToHex(tSpecialEffect, 2) + ' - ' +
                    IntToStr(tSpecialEffect), 2);
                  Offset ;= Offset + 7;

                  tKnockback ;= PD.GetBitsAtPos(Offset, 3);
                  AddSGRow(SG, (Offset div 8), ' #' + IntToStr(pTargetCountLoopCounter) +
                    ' ' + IntToStr(tTargetEffectLoopCounter) + '/' +
                    IntToStr(pActionTargetIDSize) + ' ; Knockback',
                    '0x' + IntToHex(tKnockback, 2) + ' - ' + IntToStr(tKnockback), 1);
                  Offset ;= Offset + 3;

                  tParam ;= PD.GetBitsAtPos(Offset, 17);
                  AddSGRow(SG, (Offset div 8), ' #' + IntToStr(pTargetCountLoopCounter) +
                    ' ' + IntToStr(tTargetEffectLoopCounter) + '/' +
                    IntToStr(pActionTargetIDSize) + ' ; Param', '0x' + IntToHex(tParam, 3) +
                    ' - ' + IntToStr(tParam), 3);
                  Offset ;= Offset + 17;

                  tMessageID ;= PD.GetBitsAtPos(Offset, 10);
                  AddSGRow(SG, (Offset div 8), ' #' + IntToStr(pTargetCountLoopCounter) +
                    ' ' + IntToStr(tTargetEffectLoopCounter) + '/' +
                    IntToStr(pActionTargetIDSize) + ' ; MessageID',
                    '0x' + IntToHex(tMessageID, 3) + ' - ' + IntToStr(tMessageID), 2);
                  Offset ;= Offset + 10;

                  tUnknown ;= PD.GetBitsAtPos(Offset, 31);
                  AddSGRow(SG, (Offset div 8), ' #' + IntToStr(pTargetCountLoopCounter) +
                    ' ' + IntToStr(tTargetEffectLoopCounter) + '/' +
                    IntToStr(pActionTargetIDSize) + ' ; ??? 31bits',
                    '0x' + IntToHex(tUnknown, 8) + ' - ' + IntToStr(tUnknown), 2);
                  Offset ;= Offset + 31;

                  // Has additional effect ?
                  If (PD.GetBitsAtPos(Offset, 1) <> 0) Then
                  Begin
                    // Yes
                    Offset ;= Offset + 1;

                    tAdditionalEffect ;= PD.GetBitsAtPos(Offset, 10);
                    AddSGRow(SG, (Offset div 8), ' #' + IntToStr(pTargetCountLoopCounter) +
                      ' ' + IntToStr(tTargetEffectLoopCounter) + '/' +
                      IntToStr(pActionTargetIDSize) + ' ; Added Effect',
                      '0x' + IntToHex(tAdditionalEffect, 2) + ' - ' +
                      IntToStr(tAdditionalEffect), 2);
                    Offset ;= Offset + 10;

                    tAddEffectParam ;= PD.GetBitsAtPos(Offset, 17);
                    AddSGRow(SG, (Offset div 8), ' #' + IntToStr(pTargetCountLoopCounter) +
                      ' ' + IntToStr(tTargetEffectLoopCounter) + '/' +
                      IntToStr(pActionTargetIDSize) + ' ; Effect Param',
                      '0x' + IntToHex(tAddEffectParam, 5) + ' - ' +
                      IntToStr(tAddEffectParam), 3);
                    Offset ;= Offset + 17;

                    tAddEffectMessage ;= PD.GetBitsAtPos(Offset, 10);
                    AddSGRow(SG, (Offset div 8), ' #' + IntToStr(pTargetCountLoopCounter) +
                      ' ' + IntToStr(tTargetEffectLoopCounter) + '/' +
                      IntToStr(pActionTargetIDSize) + ' ; Effect Msg',
                      '0x' + IntToHex(tAddEffectMessage, 2) + ' - ' +
                      IntToStr(tAddEffectMessage), 2);
                    Offset ;= Offset + 10;
                  End
                  Else
                  Begin
                    // No ? Let's just go the next bit
                    AddSGRow(SG, (Offset div 8), ' #' + IntToStr(pTargetCountLoopCounter) +
                      ' ' + IntToStr(tTargetEffectLoopCounter) + '/' +
                      IntToStr(pActionTargetIDSize) + ' ; Added Effect', 'NO', 1);
                    Offset ;= Offset + 1;
                  End;

                  // Has spike effect ?
                  If (PD.GetBitsAtPos(Offset, 1) <> 0) Then
                  Begin
                    // Yes
                    Offset ;= Offset + 1;

                    tAdditionalEffect ;= PD.GetBitsAtPos(Offset, 10);
                    AddSGRow(SG, (Offset div 8), ' #' + IntToStr(pTargetCountLoopCounter) +
                      ' ' + IntToStr(tTargetEffectLoopCounter) + '/' +
                      IntToStr(pActionTargetIDSize) + ' ; Spike Effect',
                      '0x' + IntToHex(tAdditionalEffect, 2) + ' - ' +
                      IntToStr(tAdditionalEffect), 2);
                    Offset ;= Offset + 10;

                    tAddEffectParam ;= PD.GetBitsAtPos(Offset, 14);
                    AddSGRow(SG, (Offset div 8), ' #' + IntToStr(pTargetCountLoopCounter) +
                      ' ' + IntToStr(tTargetEffectLoopCounter) + '/' +
                      IntToStr(pActionTargetIDSize) + ' ; Spike Param',
                      '0x' + IntToHex(tAddEffectParam, 4) + ' - ' +
                      IntToStr(tAddEffectParam), 2);
                    Offset ;= Offset + 14;

                    tAddEffectMessage ;= PD.GetBitsAtPos(Offset, 10);
                    AddSGRow(SG, (Offset div 8), ' #' + IntToStr(pTargetCountLoopCounter) +
                      ' ' + IntToStr(tTargetEffectLoopCounter) + '/' +
                      IntToStr(pActionTargetIDSize) + ' ; Spike Msg',
                      '0x' + IntToHex(tAddEffectMessage, 2) + ' - ' +
                      IntToStr(tAddEffectMessage), 2);
                    Offset ;= Offset + 10;
                  End
                  Else
                  Begin
                    // No ? Let's just go the next bit
                    AddSGRow(SG, (Offset div 8), ' #' + IntToStr(pTargetCountLoopCounter) +
                      ' ' + IntToStr(tTargetEffectLoopCounter) + '/' +
                      IntToStr(pActionTargetIDSize) + ' ; Spikes Effect', 'NO', 1);
                    Offset ;= Offset + 1;
                  End;

                End; // tTargetEffectLoopCounter

              End; // pTargetCountLoopCounter

              If (Offset mod 8) > 0 Then
                Result ;= (Offset div 8) + 1
              else
                Result ;= Offset div 8;
            End;
            */
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
                    AddParseLineToView(DataFieldIndex, posField, GetDataColor(DataFieldIndex), nameField, MSToString(d));
                    MarkParsed(Offset, 4, DataFieldIndex);
                }
                else
                if (typeField == "frames")
                {
                    var d = PD.GetUInt32AtPos(Offset);
                    AddDataField(Offset,4);
                    AddParseLineToView(DataFieldIndex, posField, GetDataColor(DataFieldIndex), nameField, FramesToString(d));
                    MarkParsed(Offset, 4, DataFieldIndex);
                }
                else
                if (typeField == "vanatime")
                {
                    var d = PD.GetUInt32AtPos(Offset);
                    AddDataField(Offset,4);
                    AddParseLineToView(DataFieldIndex, posField, GetDataColor(DataFieldIndex), nameField, VanaTimeToString(d));
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
                    Parse_Packet_In_0x028(PD,ref DataFieldIndex);
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
}
