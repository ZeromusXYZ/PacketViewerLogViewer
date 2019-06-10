using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.IO;
using System.Windows.Forms ;
using System.Data.Sql;
using System.Data.Common;
using Microsoft.Data.Sqlite;
using PacketViewerLogViewer;
using System.ComponentModel;
using System.Drawing;
using System.Diagnostics;
using PacketViewerLogViewer.PVLVHelper;
using PacketViewerLogViewer.PacketSpecial;

namespace PacketViewerLogViewer.Packets
{

    public static class PacketColors
    {
        static public Color ColBackIN;
        static public Color ColBackOUT;
        static public Color ColBackUNK;
        static public Color ColBarIN;
        static public Color ColBarOUT;
        static public Color ColBarUNK;
        static public Color ColFontIN;
        static public Color ColFontOUT;
        static public Color ColFontUNK;
        static public Color ColSelectedFontIN;
        static public Color ColSelectedFontOUT;
        static public Color ColSelectedFontUNK;
        static public Color ColSelectIN;
        static public Color ColSelectOUT;
        static public Color ColSelectUNK;
        static public Color ColSyncIN;
        static public Color ColSyncOUT;
        static public Color ColSyncUNK;
        static public int PacketListStyle;
        public static List<Color> DataColors = new List<Color>();


        public static void UpdateColorsFromSettings()
        {
            ColBackIN = Properties.Settings.Default.ColBackIN;
            ColBackOUT = Properties.Settings.Default.ColBackOUT;
            ColBackUNK = Properties.Settings.Default.ColBackUNK;
            ColBarIN = Properties.Settings.Default.ColBarIN;
            ColBarOUT = Properties.Settings.Default.ColBarOUT;
            ColBarUNK = Properties.Settings.Default.ColBarUNK;
            ColFontIN = Properties.Settings.Default.ColFontIN;
            ColFontOUT = Properties.Settings.Default.ColFontOUT;
            ColFontUNK = Properties.Settings.Default.ColFontUNK;
            ColSelectedFontIN = Properties.Settings.Default.ColSelectedFontIN;
            ColSelectedFontOUT = Properties.Settings.Default.ColSelectedFontOUT;
            ColSelectedFontUNK = Properties.Settings.Default.ColSelectedFontUNK;
            ColSelectIN = Properties.Settings.Default.ColSelectIN;
            ColSelectOUT = Properties.Settings.Default.ColSelectOUT;
            ColSelectUNK = Properties.Settings.Default.ColSelectUNK;
            ColSyncIN = Properties.Settings.Default.ColSyncIN;
            ColSyncOUT = Properties.Settings.Default.ColSyncOUT;
            ColSyncUNK = Properties.Settings.Default.ColSyncUNK;
            PacketListStyle = Properties.Settings.Default.PacketListStyle;

            // Default Field Colors
            var n = Properties.Settings.Default.ColFieldCount;
            DataColors.Clear();
            DataColors.Add(SystemColors.WindowText);
            if (n >= 2)
                DataColors.Add(Properties.Settings.Default.ColField1);

            if (n >= 3)
                DataColors.Add(Properties.Settings.Default.ColField2);

            if (n >= 4)
                DataColors.Add(Properties.Settings.Default.ColField3);

            if (n >= 5)
                DataColors.Add(Properties.Settings.Default.ColField4);

            if (n >= 6)
                DataColors.Add(Properties.Settings.Default.ColField5);

            if (n >= 7)
                DataColors.Add(Properties.Settings.Default.ColField6);

            if (n >= 8)
                DataColors.Add(Properties.Settings.Default.ColField7);

            if (n >= 9)
                DataColors.Add(Properties.Settings.Default.ColField8);

            if (n >= 10)
                DataColors.Add(Properties.Settings.Default.ColField9);

            if (n >= 11)
                DataColors.Add(Properties.Settings.Default.ColField10);

            if (n >= 12)
                DataColors.Add(Properties.Settings.Default.ColField11);

            if (n >= 13)
                DataColors.Add(Properties.Settings.Default.ColField12);

            if (n >= 14)
                DataColors.Add(Properties.Settings.Default.ColField13);

            if (n >= 15)
                DataColors.Add(Properties.Settings.Default.ColField14);

            if (n >= 16)
                DataColors.Add(Properties.Settings.Default.ColField15);
            /*
            DataColors.Add(Color.Chocolate);
            DataColors.Add(Color.MediumSeaGreen);
            DataColors.Add(Color.CornflowerBlue);
            DataColors.Add(Color.DarkSalmon);
            DataColors.Add(Color.DarkGray);
            DataColors.Add(Color.Brown);
            DataColors.Add(Color.MidnightBlue);
            */

        }
    }

    public enum PacketLogTypes { Unknown, Outgoing, Incoming }
    public enum FilterType { Off, HidePackets, ShowPackets, AllowNone };
    public enum PacketLogFileFormats { Unknown = 0, WindowerPacketViewer = 1, AshitaPacketeer = 2, PacketDB = 3 }

    public static class String6BitEncodeKeys
    {
        public static char[] Item = new char[0x40] {
        //   0    1    2    3    4    5    6    7    8    9    A    B    C    D    E    F
           '\0', '0', '1', '2', '3', '4', '5', '6', '7', '9', '8', 'A', 'B', 'C', 'D', 'E', // 0x00
            'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', // 0x10
            'V', 'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', // 0x20
            'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', '\0' // 0x30
        };
        public static char[] Linkshell = new char[0x40] {
        //   0    1    2    3    4    5    6    7    8    9    A    B    C    D    E    F
           '\0', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', // 0x00
            'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', // 0x10
            'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', // 0x20
            'V', 'W', 'X', 'Y', 'Z', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '\0' // 0x30
        };
    }

    public class PacketData
    {
        public List<String> RawText { get; set; }
        public string HeaderText { get; set; }
        public string OriginalHeaderText { get; set; }
        public List<byte> RawBytes { get; set; }
        public PacketLogTypes PacketLogType { get; set; }
        public UInt16 PacketID { get; set; }
        public UInt16 PacketDataSize { get; set; }
        public UInt16 PacketSync { get; set; }
        public DateTime TimeStamp { get; set; }
        public DateTime VirtualTimeStamp { get; set; }
        public string OriginalTimeString { get; set; }
        public int capturedZoneId { get; set; }

        public PacketParser PP;

        // Source: https://docs.microsoft.com/en-us/dotnet/api/system.datetime.parse?view=netframework-4.7.2#System_DateTime_Parse_System_String_System_IFormatProvider_System_Globalization_DateTimeStyles_
        // Assume a date and time string formatted for the fr-FR culture is the local 
        // time and convert it to UTC.
        // dateString = "2008-03-01 10:00";
        private CultureInfo cultureForDateTimeParse = CultureInfo.CreateSpecificCulture("fr-FR"); // French seems to best match for what we need here
        private DateTimeStyles stylesForDateTimeParse = DateTimeStyles.AssumeLocal;

        public PacketData()
        {
            RawText = new List<string>();
            HeaderText = "Unknown Header";
            OriginalHeaderText = "";
            RawBytes = new List<byte>();
            PacketLogType = PacketLogTypes.Unknown;
            PacketID = 0x000;
            PacketDataSize = 0x0000;
            PacketSync = 0x0000;
            TimeStamp = new DateTime(0);
            VirtualTimeStamp = new DateTime(0);
            OriginalTimeString = "";
            capturedZoneId = 0;
            PP = null;
        }

        ~PacketData()
        {
            RawText.Clear();
            RawBytes = null;
        }

        public int AddRawLineAsBytes(string s)
        {

            var simpleLine = s.Replace(" ","").Replace("\t","");

            var dataStartPos = simpleLine.IndexOf("|")+1;
            if (simpleLine.Length < dataStartPos+32)
            {
                // Data seems too short
                return 0;
            }
            var dataString = simpleLine.Substring(dataStartPos, 32); // max 32 hex digits expect

            int c = 0;
            for (int i = 0; i <= 0xf; i++)
            {
                var h = dataString.Substring(i * 2, 2);
                if (h != "--")
                {
                    try
                    {
                        byte b = byte.Parse(h, System.Globalization.NumberStyles.HexNumber);
                        RawBytes.Add(b);
                    }
                    catch { }
                    c++;
                }
            }
            return c;


            /* Example:
            //        1         2         3         4         5         6         7         8         9
            01234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890

            [2018-05-16 18:11:35] Outgoing packet 0x015:
                    |  0  1  2  3  4  5  6  7  8  9  A  B  C  D  E  F      | 0123456789ABCDEF
                -----------------------------------------------------  ----------------------
                  0 | 15 10 9E 00 CF 50 A0 C3 04 0E 1C C2 46 BF 33 43    0 | .....P......F.3C
                  1 | 00 00 02 00 5D 00 00 00 49 97 B8 69 00 00 00 00    1 | ....]...I..i....
            ...
                  5 | 00 00 00 00 -- -- -- -- -- -- -- -- -- -- -- --    5 | ....------------
            */

            /*
            // if (s.Length < 81)
            if (s.Length < 57)
            {
                // Doesn't look like a correct format
                return 0;
            }

            int c = 0;
            for (int i = 0; i <= 0xf; i++)
            {
                var h = s.Substring(10 + (i * 3), 2);
                if (h != "--")
                {
                    try
                    {
                        byte b = byte.Parse(h, System.Globalization.NumberStyles.HexNumber);
                        RawBytes.Add(b);
                    }
                    catch { }
                    c++;
                }
            }
            return c;
            */
        }

        public int AddRawPacketeerLineAsBytes(string s)
        {
            /* Example:
            //           1         2         3         4         5         6         7         8         9
            // 0123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890
            // [C->S] Id: 001A | Size: 28
            //     1A 0E ED 24 D5 10 10 01 D5 00 00 00 00 00 00 00  ..í$Õ...Õ.......
            */

            if (s.Length < 51)
            {
                // Doesn't look like a correct format
                return 0;
            }

            int c = 0;
            for (int i = 0; i <= 0xf; i++)
            {
                var h = s.Substring(4 + (i * 3), 2);
                // If this fails, we're probably at the end of the packet
                // Unlike windower, Packeteer doesn't add dashes for the blanks
                if ((h != "--") && (h != "  ") && (h != " "))
                {
                    byte b = byte.Parse(h, System.Globalization.NumberStyles.HexNumber);
                    //if (!byte.TryParse("0x" + h, out byte b))
                    //    break;
                    RawBytes.Add(b);
                    c++;
                }
            }
            return c;
        }

        public int AddRawHexStringDataAsBytes(string hexData)
        {
            int res = 0;
            try
            {
                RawBytes.Clear();
                string dataLine = hexData.Replace(" ", "").Replace("\r", "").Replace("\n", "").Replace("\t", "");
                for (int i = 0; i < (dataLine.Length - 1); i += 2)
                {
                    string num = dataLine.Substring(i, 2);
                    byte b = byte.Parse(num, System.Globalization.NumberStyles.HexNumber);
                    RawBytes.Add(b);
                    res++;
                }
                /*
                string[] nums = hexData.Split(' ');
                foreach(string num in nums)
                {
                    byte b = byte.Parse(num, System.Globalization.NumberStyles.HexNumber);
                    RawBytes.Add(b);
                    res++;
                }
                */
            }
            catch
            {
                //
            }
            return res;
        }

        public string PrintRawBytesAsHex()
        {
            const int ValuesPerRow = 16;
            string res = "";
            res += "   |  0  1  2  3   4  5  6  7   8  9  A  B   C  D  E  F\r\n";
            res += "---+----------------------------------------------------\r\n";
            int lineNumber = 0;
            for (int i = 0; i < RawBytes.Count; i++)
            {
                if ((i % ValuesPerRow) == 0)
                    res += lineNumber.ToString("X2") + " | ";

                res += RawBytes[i].ToString("X2");

                if ((i % ValuesPerRow) == (ValuesPerRow - 1))
                {
                    res += "\r\n";
                    lineNumber++;
                }
                else
                {
                    res += " ";
                    if ((i % 4) == 3)
                        res += " "; // Extra space every 4 bytes
                }
            }
            return res;
        }

        public byte GetByteAtPos(int pos)
        {
            if (pos > (RawBytes.Count - 1))
                return 0;
            return RawBytes[pos];
        }

        public sbyte GetSByteAtPos(int pos)
        {
            if (pos > (RawBytes.Count - 1))
                return 0;
            return unchecked( (sbyte)RawBytes[pos]);
        }


        public bool GetBitAtPos(int pos, int bit)
        {
            if ((pos > (RawBytes.Count - 1)) || ((bit < 0) || (bit > 7)))
                return false;
            byte b = RawBytes[(int)pos];
            byte bitmask = (byte)(0x01 << (int)bit);
            return ((b & bitmask) != 0);
        }

        public UInt16 GetUInt16AtPos(int pos)
        {
            if (pos > (RawBytes.Count - 2))
                return 0;
            return BitConverter.ToUInt16(RawBytes.GetRange(pos, 2).ToArray(), 0);
        }

        public Int16 GetInt16AtPos(int pos)
        {
            if (pos > (RawBytes.Count - 2))
                return 0;
            return BitConverter.ToInt16(RawBytes.GetRange(pos, 2).ToArray(), 0);
        }

        public UInt32 GetUInt32AtPos(int pos)
        {
            if (pos > (RawBytes.Count - 4))
                return 0;
            return BitConverter.ToUInt32(RawBytes.GetRange(pos, 4).ToArray(), 0);
        }

        public Int32 GetInt32AtPos(int pos)
        {
            if (pos > (RawBytes.Count - 4))
                return 0;
            return BitConverter.ToInt32(RawBytes.GetRange(pos, 4).ToArray(), 0);
        }

        public UInt64 GetUInt64AtPos(int pos)
        {
            if (pos > (RawBytes.Count - 8))
                return 0;
            return BitConverter.ToUInt64(RawBytes.GetRange(pos, 8).ToArray(), 0);
        }

        public Int64 GetInt64AtPos(int pos)
        {
            if (pos > (RawBytes.Count - 8))
                return 0;
            return BitConverter.ToInt64(RawBytes.GetRange(pos, 8).ToArray(), 0);
        }


        public string GetTimeStampAtPos(int pos)
        {
            string res = "???";
            if (pos > (RawBytes.Count - 4))
                return res;

            try
            {
                UInt32 DT = GetUInt32AtPos(pos);
                // Unix timestamp is seconds past epoch
                System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                dtDateTime = dtDateTime.AddSeconds(DT).ToLocalTime();
                res = dtDateTime.ToLongTimeString();
            }
            catch
            {
                res = "ERROR";
            }
            return res;
        }

        public string GetStringAtPos(int pos, int maxSize = -1)
        {
            string res = string.Empty;
            int i = 0;
            while (((i + pos) < RawBytes.Count) && (RawBytes[pos + i] != 0) && ((maxSize == -1) || (res.Length < maxSize)))
            {
                res += (char)RawBytes[pos + i];
                i++;
            }
            return res;
        }

        public string GetDataAtPos(int pos, int size)
        {
            string res = "";
            int i = 0;
            while (((i + pos) < RawBytes.Count) && (i < size) && (i < 256))
            {
                res += RawBytes[i + pos].ToString("X2") + " ";
                i++;
            }
            return res;
        }

        public string GetIP4AtPos(int pos)
        {
            if (pos > RawBytes.Count - 4)
                return "";
            return RawBytes[pos + 0].ToString() + "." + RawBytes[pos + 1].ToString() + "." + RawBytes[pos + 2].ToString() + "." + RawBytes[pos + 3].ToString();
        }

        public string GetJobFlagsAtPos(int pos)
        {
            string res = "";
            if (pos > (RawBytes.Count - 4))
                return res;
            UInt32 Flags = GetUInt32AtPos(pos);
            for (uint BitShiftCount = 0; BitShiftCount < 32; BitShiftCount++)
            {
                if ((Flags & 0x00000001) == 1)
                {
                    if (res != "")
                        res += " ";

                    if (BitShiftCount == 0)
                    {
                        res += "SubJob";
                    }
                    else
                    {
                        var JobName = DataLookups.NLU(DataLookups.LU_Job).GetValue(BitShiftCount);
                        if (JobName == "")
                            JobName = "[Bit" + BitShiftCount.ToString() + "]";
                        res += JobName;
                    }

                }
                Flags = Flags >> 1;
            }

            return res;
        }

        public Int64 GetBitsAtPos(int pos, int BitOffset, int BitsSize)
        {
            Int64 res = 0;
            int P = pos;
            int B = BitOffset;
            int RestBits = BitsSize;
            // Minimum 1 bit
            if (RestBits < 1)
                RestBits = 1;
            Int64 Mask = 1;
            while (RestBits > 0)
            {
                while (B >= 8)
                {
                    B -= 8;
                    P++;
                }
                // Add Mask Value if Bit is set
                if (GetBitAtPos(P, B))
                    res += Mask;
                RestBits--;
                Mask = Mask << 1;
                B++;
            }
            return res;
        }

        public Int64 GetBitsAtBitPos(int BitOffset, int BitsSize)
        {
            return GetBitsAtPos(BitOffset / 8, BitOffset % 8, BitsSize);
        }

        public string GetPackedString16AtPos(int pos, char[] Encoded6BitKey)
        {
            string res = "";
            // Hex: B8 81 68 24  72 14 4F 10  54 0C 8F 00  00 00 00 00
            // Bit: 101110 00
            // 1000 0001
            // 01 101000
            // 001001 00
            // 0111 0010
            // 00 010100
            // 010011 11
            // 0001 0000
            // 01 010100
            // 000011 00
            // 1000 1111
            // 00 000000

            // PackedString: TheNightsWatch (with no spaces)
            // PackedNum: 2E 08 05 ...
            // 101110  T
            // 001000  h
            // 000101  e
            //

            // A_  6F F0    011011 11-1111 0000  =>  1B 3F 00  =>  A
            // B_  73 F0    011100 11-1111 0000  =>  1C 3F 00  =>  B
            // F_  83 F0    100000 11-1111 0000  =>  20 3F 00  =>  F

            // EncodeLSStr : Array [0..63] of Char = (
            // #0 ,'a','b','c','d','e','f','g',  'h','i','j','k','l','m','n','o', // $00
            // 'p','q','r','s','t','u','v','w',  'x','y','z','A','B','C','D','E', // $10
            // 'F','G','H','I','J','K','L','M',  'N','O','P','Q','R','S','T','U', // $20
            // 'V','W','X','Y','Z',' ',' ',' ',  ' ',' ',' ',' ',' ',' ',' ', #0  // $30
            //  0   1   2   3   4   5   6   7     8   9   A   B   C   D   E   F
            // );
            int Offset = 0;
            while ((Offset / 8) < 15)
            {
                byte encodedChar = 0;
                byte bitMask = 0b00100000;
                for (int bit = 0; bit < 6; bit++)
                {
                    bool isSet = GetBitAtPos(pos + ((Offset + bit) / 8), 7 - ((Offset + bit) % 8));
                    if (isSet)
                        encodedChar += bitMask;
                    bitMask >>= 1;
                }
                // GetBitsAtPos(pos + (Offset / 8), (Offset % 8), 6);
                if ((encodedChar >= Encoded6BitKey.Length) || (encodedChar < 0))
                    break;
                var c = Encoded6BitKey[encodedChar];
                if (c == 0)
                    break;
                res += c;
                Offset += 6;
            }
            return res;
        }

        public Single GetFloatAtPos(int pos)
        {
            if (pos > (RawBytes.Count - 4))
                return 0;
            return BitConverter.ToSingle(RawBytes.GetRange(pos, 4).ToArray(), 0);
        }

        public int FindByte(byte aByte)
        {
            return RawBytes.IndexOf(aByte);
        }

        public int FindUInt16(UInt16 aUInt16)
        {
            for (int i = 0; i < (RawBytes.Count - 2); i++)
            {
                if (GetUInt16AtPos(i) == aUInt16)
                    return i;
            }
            return -1;
        }

        public int FindUInt32(UInt32 aUInt32)
        {
            for (int i = 0; i < (RawBytes.Count - 4); i++)
            {
                if (GetUInt32AtPos(i) == aUInt32)
                    return i;
            }
            return -1;
        }

        public int FindUInt64(UInt64 aUInt64)
        {
            for (int i = 0; i < (RawBytes.Count - 8); i++)
            {
                if (GetUInt64AtPos(i) == aUInt64)
                    return i;
            }
            return -1;
        }

        public bool DateTimeParse(string s, out DateTime res)
        {
            res = new DateTime(0);
            if (s.Length != 19)
                return false;
            try
            {
                // 0         1
                // 01234567890123456789
                // 2018-05-16 18:11:35
                var yyyy = int.Parse(s.Substring(0, 4));
                var mm = int.Parse(s.Substring(5, 2));
                var dd = int.Parse(s.Substring(8, 2));
                var hh = int.Parse(s.Substring(11, 2));
                var nn = int.Parse(s.Substring(14, 2));
                var ss = int.Parse(s.Substring(17, 2));
                res = new DateTime(yyyy, mm, dd, hh, nn, ss);
                return true;
            }
            catch
            {
            }
            return false;
        }

        public bool CompileData(PacketLogFileFormats plff)
        {
            if (RawBytes.Count < 4)
            {
                PacketID = 0xFFFF;
                PacketDataSize = 0;
                HeaderText = "Invalid Packet Size < 4";
                return false;
            }
            PacketID = (UInt16)(GetByteAtPos(0) + ((GetByteAtPos(1) & 0x01) * 0x100));
            PacketDataSize = (UInt16)((GetByteAtPos(1) & 0xFE) * 2);
            PacketSync = (UInt16)(GetByteAtPos(2) + (GetByteAtPos(3) * 0x100));
            string TS = "";
            if (TimeStamp.Ticks > 0)
                TS = TimeStamp.ToString("HH:mm:ss");
            if (plff == PacketLogFileFormats.AshitaPacketeer)
            {
                // Packeteer doesn't have time info (yet)
                TimeStamp = new DateTime(0);
                OriginalTimeString = "0000-00-00 00:00";
            }
            if (plff == PacketLogFileFormats.WindowerPacketViewer)
            {
                // Try to determine timestamp from header
                var P1 = OriginalHeaderText.IndexOf('[');
                var P2 = OriginalHeaderText.IndexOf(']');
                if ((P1 >= 0) && (P2 >= 0) && (P2 > P1))
                {
                    OriginalTimeString = OriginalHeaderText.Substring(P1 + 1, P2 - P1 - 1);
                    if (OriginalTimeString.Length > 0)
                    {
                        try
                        {
                            // try quick-parse first
                            DateTime dt;
                            if (DateTimeParse(OriginalTimeString, out dt))
                            {
                                TimeStamp = dt;
                            }
                            else
                            {
                                TimeStamp = DateTime.Parse(OriginalTimeString, cultureForDateTimeParse, stylesForDateTimeParse);
                            }
                            TS = TimeStamp.ToString("HH:mm:ss");
                        }
                        catch (FormatException)
                        {
                            TimeStamp = new DateTime(0);
                            TS = "";
                            OriginalTimeString = "0000-00-00 00:00";
                        }
                    }
                }
            }
            VirtualTimeStamp = TimeStamp;
            if (TimeStamp.Ticks == 0)
                TS = "";

            string S = "";
            switch (PacketLogType)
            {
                case PacketLogTypes.Outgoing:
                    S = "OUT ";
                    break;
                case PacketLogTypes.Incoming:
                    S = "IN  ";
                    break;
                default:
                    S = "??? ";
                    break;
            }
            S = TS + " : " + S + "0x" + PacketID.ToString("X3") + " - ";
            HeaderText = S + DataLookups.PacketTypeToString(PacketLogType, PacketID);
            return true;
        }

        public bool MatchesSearch(SearchParameters p)
        {
            if ((PacketLogType == PacketLogTypes.Incoming) && (!p.SearchIncoming))
                return false;
            if ((PacketLogType == PacketLogTypes.Outgoing) && (!p.SearchOutgoing))
                return false;

            bool res = true;

            if ((res) && (p.SearchByPacketID))
            {
                res = false;
                if (PacketID == p.SearchPacketID)
                    res = true;
            }

            if ((res) && (p.SearchBySync))
            {
                res = false;
                if (PacketSync == p.SearchSync)
                    res = true;
            }

            if ((res) && (p.SearchByByte))
            {
                res = false;
                if (RawBytes.IndexOf(p.SearchByte) >= 0)
                    res = true;
            }

            if ((res) && (p.SearchByUInt16))
            {
                res = false;
                for (int i = 0; i < RawBytes.Count - 2; i++)
                {
                    var n = GetUInt16AtPos(i);
                    if (n == p.SearchUInt16)
                    {
                        res = true;
                        break;
                    }
                }
            }

            if ((res) && (p.SearchByUInt32))
            {
                res = false;
                for (int i = 0; i < RawBytes.Count - 4; i++)
                {
                    var n = GetUInt32AtPos(i);
                    if (n == p.SearchUInt32)
                    {
                        res = true;
                        break;
                    }
                }
            }

            if ((res) && (PP != null) && (p.SearchByParsedData) && (p.SearchParsedFieldValue != string.Empty))
            {
                res = false;
                foreach (var f in PP.ParsedView)
                {
                    if (p.SearchParsedFieldName != string.Empty)
                    {
                        // Field Name Specified
                        res = ((f.Var.ToLower().IndexOf(p.SearchParsedFieldName) >= 0) && (f.Data.ToLower().IndexOf(p.SearchParsedFieldValue) >= 0));
                    }
                    else
                    {
                        // No field name defined
                        res = (f.Data.ToLower().IndexOf(p.SearchParsedFieldValue) >= 0);
                    }
                    if (res)
                        break;
                }
            }

            return res;
        }

        public void CompileSpecial(PacketList pl)
        {
            try
            {
                if (PacketLogType == PacketLogTypes.Incoming)
                {
                    switch (PacketID)
                    {
                        case 0x00a: CompileSpecialized.In0x00a(this,pl);
                            break;
                        default:
                            break;
                    }
                }
                else
                if (PacketLogType == PacketLogTypes.Outgoing)
                {
                    switch (PacketID)
                    {
                        default:
                            break;
                    }
                }
            }
            catch { }
        }

    }

    public class PacketListFilter
    {
        public FilterType FilterOutType { get; set; }
        public List<UInt16> FilterOutList { get; set; }
        public FilterType FilterInType { get; set; }
        public List<UInt16> FilterInList { get; set; }

        public PacketListFilter()
        {
            FilterOutList = new List<UInt16>();
            FilterInList = new List<UInt16>();
            Clear();
        }

        public void Clear()
        {
            FilterOutType = FilterType.Off;
            FilterOutList.Clear();
            FilterInType = FilterType.Off;
            FilterInList.Clear();
        }

        public void CopyFrom(PacketListFilter aFilter)
        {
            FilterOutType = aFilter.FilterOutType;
            FilterInType = aFilter.FilterInType;
            FilterOutList.Clear();
            FilterOutList.AddRange(aFilter.FilterOutList);
            FilterInList.Clear();
            FilterInList.AddRange(aFilter.FilterInList);
        }

        public void AddOutFilterValueToList(UInt16 value)
        {
            if ((value > 0) && (FilterOutList.IndexOf(value) < 0))
                FilterOutList.Add(value);
        }

        public void AddInFilterValueToList(UInt16 value)
        {
            if ((value > 0) && (FilterInList.IndexOf(value) < 0))
                FilterInList.Add(value);
        }

        public bool LoadFromFile(string filename)
        {
            try
            {
                List<string> sl = File.ReadAllLines(filename).ToList();

                Clear();
                foreach(string line in sl)
                {
                    var fields = line.Split(';');
                    if (fields.Length <= 1)
                        continue;

                    var f0 = fields[0].ToLower();
                    var f1 = fields[1].ToLower();

                    switch (f0)
                    {
                        case "outtype":
                            switch(f1)
                            {
                                case "off":
                                    FilterOutType = FilterType.Off;
                                    break;
                                case "show":
                                    FilterOutType = FilterType.ShowPackets;
                                    break;
                                case "hide":
                                    FilterOutType = FilterType.HidePackets;
                                    break;
                                case "none":
                                    FilterOutType = FilterType.AllowNone;
                                    break;
                            }
                            break;
                        case "intype":
                            switch (f1)
                            {
                                case "off":
                                    FilterInType = FilterType.Off;
                                    break;
                                case "show":
                                    FilterInType = FilterType.ShowPackets;
                                    break;
                                case "hide":
                                    FilterInType = FilterType.HidePackets;
                                    break;
                                case "none":
                                    FilterInType = FilterType.AllowNone;
                                    break;
                            }
                            break;
                        case "out":
                            if (DataLookups.TryFieldParse(f1, out long nout))
                                AddOutFilterValueToList((UInt16)nout);
                            break;
                        case "in":
                            if (DataLookups.TryFieldParse(f1, out long nin))
                                AddInFilterValueToList((UInt16)nin);
                            break;
                    }


                }

            }
            catch (Exception x)
            {
                MessageBox.Show("Failed to load " + filename + "\r\nException: " + x.Message, "Load Filter Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        public bool SaveToFile(string filename)
        {
            List<string> sl = new List<string>();
            sl.Add("rem;original-file;" + Path.GetFileName(filename));
            switch(FilterOutType)
            {
                case FilterType.Off:
                    sl.Add("outtype;off");
                    break;
                case FilterType.ShowPackets:
                    sl.Add("outtype;show");
                    break;
                case FilterType.HidePackets:
                    sl.Add("outtype;hide");
                    break;
                case FilterType.AllowNone:
                    sl.Add("outtype;none");
                    break;
            }
            foreach(UInt16 i in FilterOutList)
            {
                sl.Add("out;0x"+ i.ToString("X3") + ";" + DataLookups.NLU(DataLookups.LU_PacketOut).GetValue(i));
            }

            switch (FilterInType)
            {
                case FilterType.Off:
                    sl.Add("intype;off");
                    break;
                case FilterType.ShowPackets:
                    sl.Add("intype;show");
                    break;
                case FilterType.HidePackets:
                    sl.Add("intype;hide");
                    break;
                case FilterType.AllowNone:
                    sl.Add("intype;none");
                    break;
            }
            foreach (UInt16 i in FilterInList)
            {
                sl.Add("in;0x" + i.ToString("X3") + ";" + DataLookups.NLU(DataLookups.LU_PacketIn).GetValue(i));
            }

            try
            {
                File.WriteAllLines(filename, sl);
            }
            catch (Exception x)
            {
                MessageBox.Show("Failed to save " + filename + "\r\nException: " + x.Message, "Save Filter Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }
    }

    public class PacketList
    {
        protected List<PacketData> PacketDataList { get; set; }
        public List<UInt16> ContainsPacketsIn { get; set; }
        public List<UInt16> ContainsPacketsOut { get; set; }

        public PacketListFilter Filter ;
        public DateTime firstPacketTime;
        public bool IsPreParsed = false;
        public UInt16 currentParseZone = 0;
        public UInt32 currentParsePlayerID = 0;
        public string currentParsePlayerName = "";

        public PacketList()
        {
            PacketDataList = new List<PacketData>();
            ContainsPacketsIn = new List<UInt16>();
            ContainsPacketsOut = new List<UInt16>();
            Filter = new PacketListFilter();
            firstPacketTime = new DateTime(0);
        }

        ~PacketList()
        {
            Filter.Clear();
            Clear();
        }

        public void Clear()
        {
            PacketDataList.Clear();
            firstPacketTime = new DateTime(0);
        }

        public bool LoadFromFile(string fileName)
        {
            if (!File.Exists(fileName))
                return false;

            PacketLogTypes expectedPacketType = PacketLogTypes.Unknown;
            PacketLogFileFormats expectedLogType = PacketLogFileFormats.Unknown;
            var fn = fileName.ToLower();

            // Take the filename and 2 parent directories, and use those names to try and guess the packet direction
            var toCheckList = fn.Split(Path.DirectorySeparatorChar).ToList();
            while (toCheckList.Count > 3)
            {
                toCheckList.RemoveAt(0);
            }
            toCheckList.Reverse();
            foreach(var checkName in toCheckList)
            {
                if ((expectedPacketType == PacketLogTypes.Unknown) && (checkName.IndexOf("outgoing") >= 0))
                    expectedPacketType = PacketLogTypes.Outgoing;
                if ((expectedPacketType == PacketLogTypes.Unknown) && (checkName.IndexOf("incoming") >= 0))
                    expectedPacketType = PacketLogTypes.Incoming;
                if ((expectedPacketType == PacketLogTypes.Unknown) && (checkName.IndexOf("out-") >= 0))
                    expectedPacketType = PacketLogTypes.Outgoing;
                if ((expectedPacketType == PacketLogTypes.Unknown) && (checkName.IndexOf("in-") >= 0))
                    expectedPacketType = PacketLogTypes.Incoming;
            }

            // Try file type depending on it's extension
            if ((expectedLogType == PacketLogFileFormats.Unknown) && (Path.GetExtension(fn) == ".log"))
                expectedLogType = PacketLogFileFormats.WindowerPacketViewer;
            if ((expectedLogType == PacketLogFileFormats.Unknown) && (Path.GetExtension(fn) == ".txt"))
                expectedLogType = PacketLogFileFormats.AshitaPacketeer;
            if ((expectedLogType == PacketLogFileFormats.Unknown) && (Path.GetExtension(fn) == ".sqlite"))
                expectedLogType = PacketLogFileFormats.PacketDB;

            if ((expectedLogType == PacketLogFileFormats.WindowerPacketViewer) || (expectedLogType == PacketLogFileFormats.AshitaPacketeer))
            {
                try
                {
                    List<string> sl = File.ReadAllLines(fileName).ToList();
                    return LoadFromStringList(sl, expectedLogType, expectedPacketType);
                }
                catch (Exception x)
                {
                    if (x is PathTooLongException)
                    {
                        MessageBox.Show("This program does not support file paths that are longer than MAX_PATH (260 characters by default)\r\nPlease consider shortening your directory or file names, and try again.", "Name too long", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                    else
                    {
                        MessageBox.Show("Exception:\r\n"+x.Message, "LoadFromFile()", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                    return false;
                }
            }
            else
            if (expectedLogType == PacketLogFileFormats.PacketDB)
            {
                return LoadFromSQLite3(fileName);
            }
            else
            {
                return false;
            }
        }

        public bool LoadFromStringList(List<string> FileData,PacketLogFileFormats logFileType , PacketLogTypes preferedType)
        {
            IsPreParsed = Properties.Settings.Default.PreParseData;
            // Add dummy blank lines to fix a bug of ignoring last packet if isn't finished by a blank line
            FileData.Add("");

            Application.UseWaitCursor = true;

            using (LoadingForm loadform = new LoadingForm(MainForm.thisMainForm))
            {
                try
                {
                    loadform.Text = "Loading text log file";
                    loadform.Show();
                    loadform.pb.Minimum = 0;
                    loadform.pb.Maximum = FileData.Count ;
                    loadform.pb.Step = 1000;

                    PacketData pd = null;
                    bool IsUndefinedPacketType = true;
                    bool AskForPacketType = true;
                    bool hasHadDataHeader = false;
                    bool pastStartOfDataMarker = false;

                    int c = 0;
                    foreach(string s in FileData)
                    {
                        string sLower = s.ToLower().Trim(' ');
                        if ((sLower != string.Empty) && (pd == null))
                        {
                            hasHadDataHeader = true;
                            pastStartOfDataMarker = false;
                            // Begin building a new packet
                            pd = new PacketData();
                            if (sLower.IndexOf("incoming") >= 0)
                            {
                                pd.PacketLogType = PacketLogTypes.Incoming;
                                IsUndefinedPacketType = false;
                                logFileType = PacketLogFileFormats.WindowerPacketViewer;
                            }
                            else
                            if (sLower.IndexOf("outgoing") >= 0)
                            {
                                pd.PacketLogType = PacketLogTypes.Outgoing;
                                IsUndefinedPacketType = false;
                                logFileType = PacketLogFileFormats.WindowerPacketViewer;
                            }
                            else
                            if (sLower.IndexOf("[s->c]") >= 0)
                            {
                                pd.PacketLogType = PacketLogTypes.Incoming;
                                IsUndefinedPacketType = false;
                                logFileType = PacketLogFileFormats.AshitaPacketeer;
                            }
                            else
                            if (sLower.IndexOf("[c->s]") >= 0)
                            {
                                pd.PacketLogType = PacketLogTypes.Outgoing;
                                IsUndefinedPacketType = false;
                                logFileType = PacketLogFileFormats.AshitaPacketeer;
                            }
                            else
                            if (sLower.IndexOf("npc id:") >= 0)
                            {
                                // This is likely a npc logger log file, assume it's a incomming packet
                                pd.PacketLogType = PacketLogTypes.Incoming;
                                IsUndefinedPacketType = false;
                                logFileType = PacketLogFileFormats.WindowerPacketViewer;
                            }
                            else
                            {
                                pd.PacketLogType = preferedType;
                            }

                            if (
                                // Not a comment or empty line
                                ((s != "") && (!s.StartsWith("--"))) &&
                                // Unknown packet and we need to know ?
                                (IsUndefinedPacketType && AskForPacketType && (pd.PacketLogType == PacketLogTypes.Unknown))
                               )
                            {
                                AskForPacketType = false;
                                // Ask for type
                                var askDlgRes = DialogResult.Cancel;
                                using (PacketTypeSelectForm askDlg = new PacketTypeSelectForm())
                                {
                                    askDlg.lHeaderData.Text = s;
                                    askDlgRes = askDlg.ShowDialog();
                                }

                                if (askDlgRes == DialogResult.Yes)
                                {
                                    preferedType = PacketLogTypes.Incoming ;
                                    IsUndefinedPacketType = false;
                                    pd.PacketLogType = preferedType ;
                                }
                                else
                                if (askDlgRes == DialogResult.No)
                                {
                                    preferedType = PacketLogTypes.Outgoing;
                                    IsUndefinedPacketType = false;
                                    pd.PacketLogType = preferedType;
                                }
                            }

                            pd.RawText.Add(s);
                            if (logFileType == PacketLogFileFormats.Unknown)
                            {
                                // We couldn't identify what type of packet this might be, and we didn't provide a type
                                // Assume the pasted data is just raw hex bytes (as string)
                                pd.HeaderText = "Clipboard";
                                pd.OriginalHeaderText = "Clipboard Data";
                                pd.AddRawHexStringDataAsBytes(s);
                            }
                            else
                            {
                                // Looks like a normal text packet, initialize the header
                                pd.HeaderText = s;
                                pd.OriginalHeaderText = s;

                                // Packeteer doesn't have any "in between" lines, so mark our header as complete
                                if (logFileType == PacketLogFileFormats.AshitaPacketeer)
                                    pastStartOfDataMarker = true;
                            }

                        } // end start new packet
                        else
                        if (hasHadDataHeader && (sLower != string.Empty) && (pd != null))
                        {
                            // Add line of data
                            pd.RawText.Add(s);
                            // Actual packet data starts at the 3rd line after the header
                            if ((logFileType != PacketLogFileFormats.AshitaPacketeer) && (pastStartOfDataMarker))
                            {
                                pd.AddRawLineAsBytes(s);
                            }
                            else
                            if ((logFileType != PacketLogFileFormats.AshitaPacketeer) && (!pastStartOfDataMarker))
                            {
                                // a resonable amount of dashes line (32 chars) to mark the start of the data
                                if (sLower.IndexOf("--------------------------------") >= 0)
                                    pastStartOfDataMarker = true;
                            }
                            else
                            if ((logFileType == PacketLogFileFormats.AshitaPacketeer) && (pd.RawText.Count > 1))
                            {
                                pd.AddRawPacketeerLineAsBytes(s);
                            }
                            else
                            if (logFileType == PacketLogFileFormats.Unknown)
                            {
                                // Assume the pasted data is just raw hex bytes (as string)
                                pd.AddRawHexStringDataAsBytes(s);
                            }
                        }
                        else
                        if (hasHadDataHeader && (sLower == string.Empty) && (pd != null))
                        {
                            // Close this packet and add it to list
                            if (pd.CompileData(logFileType))
                            {
                                pd.CompileSpecial(this);
                                // Set zone after CompileSpecial, this is only needed if not captured by PacketDB
                                pd.capturedZoneId = currentParseZone;

                                if (IsPreParsed)
                                {
                                    pd.PP = new PacketParser(pd.PacketID, pd.PacketLogType);
                                    pd.PP.AssignPacket(pd);
                                    pd.PP.ParseData("-");
                                }

                                PacketDataList.Add(pd);
                                if (pd.PacketLogType == PacketLogTypes.Outgoing)
                                {
                                    if (ContainsPacketsOut.IndexOf(pd.PacketID) < 0)
                                        ContainsPacketsOut.Add(pd.PacketID);
                                }
                                else
                                if (pd.PacketLogType == PacketLogTypes.Incoming)
                                {
                                    if (ContainsPacketsIn.IndexOf(pd.PacketID) < 0)
                                        ContainsPacketsIn.Add(pd.PacketID);
                                }
                            }
                            else
                            {
                                // Invalid data
                            }
                            // reset our packet holder
                            pd = null;
                        }
                        else
                        if ((sLower == "") && (pd == null))
                        {
                            // Blank line
                        }
                        else
                        if (sLower.StartsWith("--") && (pd != null))
                        {
                            // Comment
                        }
                        else
                        {
                            // ERROR, this should not be possible in a valid file, but just let's ignore it anyway, just in case
                        }

                        c++;
                        if ((c % 1000) == 0)
                        {
                            loadform.pb.PerformStep();
                            loadform.pb.Refresh();
                        }
                    } // end foreach datafile line

                }
                catch
                {
                    Application.UseWaitCursor = false;
                    return false;
                }
            }
            if (PacketDataList.Count > 0)
                firstPacketTime = PacketDataList[0].TimeStamp;
            Application.UseWaitCursor = false;
            return true;
        }

        public bool LoadFromSQLite3(string sqliteFileName)
        {
            IsPreParsed = Properties.Settings.Default.PreParseData;
            int c = 0;
            using (LoadingForm loadform = new LoadingForm(MainForm.thisMainForm))
            {
                try
                {
                    loadform.Text = "Loading sqlite log file";
                    loadform.Show();
                    loadform.pb.Minimum = 0;
                    loadform.pb.Maximum = 100000;
                    loadform.pb.Step = 100;

                    using (SqliteConnection sqlConnection = new SqliteConnection("Data Source=" + sqliteFileName))
                    {
                        sqlConnection.Open();

                        string sql = "SELECT * FROM `packets` ORDER by PACKET_ID ASC";

                        SqliteCommand command = new SqliteCommand(sql, sqlConnection);
                        SqliteDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            PacketData pd = new PacketData();
                            pd.TimeStamp = reader.GetDateTime(reader.GetOrdinal("RECEIVED_DT"));
                            pd.VirtualTimeStamp = pd.TimeStamp;
                            var dir = reader.GetInt16(reader.GetOrdinal("DIRECTION")); // 0 = in ; 1 = out
                            switch (dir)
                            {
                                case 0:
                                    pd.PacketLogType = PacketLogTypes.Incoming;
                                    break;
                                case 1:
                                    pd.PacketLogType = PacketLogTypes.Outgoing;
                                    break;
                                default:
                                    pd.PacketLogType = PacketLogTypes.Unknown;
                                    break;
                            }
                            pd.PacketID = (UInt16)reader.GetInt32(reader.GetOrdinal("PACKET_TYPE"));
                            var pData = reader.GetString(reader.GetOrdinal("PACKET_DATA"));
                            pd.RawText.Add(pData);
                            pd.AddRawHexStringDataAsBytes(pData);
                            pd.capturedZoneId = reader.GetInt16(reader.GetOrdinal("ZONE_ID"));

                            pd.OriginalHeaderText = "PACKET_ID " + reader.GetInt64(reader.GetOrdinal("PACKET_ID")) + " , DIR " + dir.ToString() + " , TYPE " + pd.PacketID.ToString();
                            pd.OriginalTimeString = "";

                            if (pd.CompileData(PacketLogFileFormats.PacketDB))
                            {
                                pd.CompileSpecial(this);

                                if (IsPreParsed)
                                {
                                    pd.PP = new PacketParser(pd.PacketID, pd.PacketLogType);
                                    pd.PP.AssignPacket(pd);
                                    pd.PP.ParseData("-");
                                }
                                PacketDataList.Add(pd);

                                if (pd.PacketLogType == PacketLogTypes.Outgoing)
                                {
                                    if (ContainsPacketsOut.IndexOf(pd.PacketID) < 0)
                                        ContainsPacketsOut.Add(pd.PacketID);
                                }
                                else
                                if (pd.PacketLogType == PacketLogTypes.Incoming)
                                {
                                    if (ContainsPacketsIn.IndexOf(pd.PacketID) < 0)
                                        ContainsPacketsIn.Add(pd.PacketID);
                                }
                            }

                            c++;
                            if ((c % 100) == 0)
                            {
                                loadform.pb.PerformStep();
                                if (loadform.pb.Value >= loadform.pb.Maximum)
                                    loadform.pb.Value = loadform.pb.Minimum;
                                loadform.pb.Refresh();
                            }
                        }

                    }
                }
                catch (Exception x)
                {
                    MessageBox.Show("Exception: " + x.Message, "Exception loading SQLite file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            if (PacketDataList.Count > 0)
                firstPacketTime = PacketDataList[0].TimeStamp;
            return true;
        }

        public int Count()
        {
            return PacketDataList.Count;
        }

        public PacketData GetPacket(int index)
        {
            if ((index >= 0) && (index < PacketDataList.Count))
                return PacketDataList[index];
            if (PacketDataList.Count > 0)
                return PacketDataList[0];
            else
                return null;
        }

        public int CopyFrom(PacketList Original)
        {
            int c = 0;
            Clear();
            IsPreParsed = Original.IsPreParsed;
            foreach(PacketData pd in Original.PacketDataList)
            {
                PacketDataList.Add(pd);
                c++;
            }
            if (PacketDataList.Count > 0)
                firstPacketTime = PacketDataList[0].TimeStamp;
            return c;
        }

        protected bool DoIShowThis(UInt16 PacketID, FilterType FT, List<UInt16> FL)
        {
            switch (FT)
            { 
                case FilterType.AllowNone:
                    return false;
                case FilterType.ShowPackets:
                    return FL.Contains(PacketID);
                case FilterType.HidePackets:
                    return !FL.Contains(PacketID);
                case FilterType.Off:
                default:
                    return true;
            }
        }

        protected bool DoIShowThis(PacketData PD)
        {
            if ((PD.PacketLogType == PacketLogTypes.Incoming) && (Filter.FilterInType != FilterType.Off))
                return DoIShowThis(PD.PacketID, Filter.FilterInType, Filter.FilterInList);
            if ((PD.PacketLogType == PacketLogTypes.Outgoing) && (Filter.FilterOutType != FilterType.Off))
                return DoIShowThis(PD.PacketID, Filter.FilterOutType, Filter.FilterOutList);
            return true;
        }

        public int FilterFrom(PacketList Original)
        {
            int c = 0;
            Clear();
            IsPreParsed = Original.IsPreParsed;
            foreach (PacketData pd in Original.PacketDataList)
            {
                if (DoIShowThis(pd))
                {
                    PacketDataList.Add(pd);
                    c++;
                }
            }
            if (PacketDataList.Count > 0)
                firstPacketTime = PacketDataList[0].TimeStamp;
            return c;
        }

        public int SearchFrom(PacketList Original, SearchParameters p)
        {
            int c = 0;
            Clear();
            foreach (PacketData pd in Original.PacketDataList)
            {
                if (pd.MatchesSearch(p))
                {
                    PacketDataList.Add(pd);
                    c++;
                }
            }
            if (PacketDataList.Count > 0)
                firstPacketTime = PacketDataList[0].TimeStamp;
            return c;
        }

        public int FindPacketIndexByDateTime(DateTime dt,int searchStartLocation = 0)
        {
            if (PacketDataList.Count <= 0)
                return -1;
            int i = searchStartLocation ;
            if ((i < 0) || (i >= PacketDataList.Count))
                i = 0;
            DateTime lastCheckTime ;
            for(int c = 0;c < PacketDataList.Count; c++)
            {
                // Next
                lastCheckTime = PacketDataList[i].VirtualTimeStamp;
                i++;
                if (i >= PacketDataList.Count)
                    i = 0;

                if ((lastCheckTime <= dt) && (dt < PacketDataList[i].VirtualTimeStamp))
                   return i;

            }
            return -1;
        }

        public void BuildVirtualTimeStamps()
        {
            // Need a minimum of 3 packets to be able to have effect
            if (PacketDataList.Count <= 1)
                return;

            int i = 0;
            int divider = 0 ;
            DateTime FirstOfGroupTime = GetPacket(0).TimeStamp;
            int FirstOfGroupIndex = 0;
            DateTime LastTimeStamp = FirstOfGroupTime;

            while (i < PacketDataList.Count)
            {
                PacketData thisPacket = GetPacket(i);
                thisPacket.VirtualTimeStamp = thisPacket.TimeStamp;
                if (thisPacket.TimeStamp == LastTimeStamp)
                {
                    // Same packet Group
                    divider++;
                }
                if ( ((thisPacket.TimeStamp != LastTimeStamp) || (i >= PacketDataList.Count)) )
                {
                    // Last packet of the group
                    TimeSpan oneStepTime = TimeSpan.Zero;
                    if (divider > 0)
                        oneStepTime = TimeSpan.FromMilliseconds(1000 / divider);
                    TimeSpan stepTime = TimeSpan.Zero;
                    for (int n = 0; n <= divider; n++)
                    {
                        GetPacket(FirstOfGroupIndex + n).VirtualTimeStamp = FirstOfGroupTime + stepTime;
                        stepTime += oneStepTime ;
                    }

                    if (i < (PacketDataList.Count - 1))
                    {
                        // If not the last one
                        FirstOfGroupIndex = i + 1;
                        FirstOfGroupTime = GetPacket(i + 1).TimeStamp;
                        divider = 0;
                    }
                }
                LastTimeStamp = thisPacket.TimeStamp;
                i++;
            }

        }

    } // End PacketList

    // source: http://yacsharpblog.blogspot.com/2008/07/listbox-flicker.html
    public class FlickerFreeListBox : System.Windows.Forms.ListBox
    {
        public FlickerFreeListBox()
        {
            this.SetStyle(
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.UserPaint,
                true);
            this.DrawMode = DrawMode.OwnerDrawFixed;
        }
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if (this.Items.Count > 0)
            {
                e.DrawBackground();
                e.Graphics.DrawString(this.Items[e.Index].ToString(), e.Font, new SolidBrush(e.ForeColor), new PointF(e.Bounds.X, e.Bounds.Y));
            }
            base.OnDrawItem(e);
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            Region iRegion = new Region(e.ClipRectangle);
            e.Graphics.FillRegion(new SolidBrush(this.BackColor), iRegion);
            if (this.Items.Count > 0)
            {
                for (int i = 0; i < this.Items.Count; ++i)
                {
                    System.Drawing.Rectangle irect = this.GetItemRectangle(i);
                    if (e.ClipRectangle.IntersectsWith(irect))
                    {
                        if ((this.SelectionMode == SelectionMode.One && this.SelectedIndex == i)
                        || (this.SelectionMode == SelectionMode.MultiSimple && this.SelectedIndices.Contains(i))
                        || (this.SelectionMode == SelectionMode.MultiExtended && this.SelectedIndices.Contains(i)))
                        {
                            OnDrawItem(new DrawItemEventArgs(e.Graphics, this.Font,
                                irect, i,
                                DrawItemState.Selected, this.ForeColor,
                                this.BackColor));
                        }
                        else
                        {
                            OnDrawItem(new DrawItemEventArgs(e.Graphics, this.Font,
                                irect, i,
                                DrawItemState.Default, this.ForeColor,
                                this.BackColor));
                        }
                        iRegion.Complement(irect);
                    }
                }
            }
            base.OnPaint(e);
        }
    }

    public class PacketTabPage: System.Windows.Forms.TabPage
    {
        private MainForm ownerMainForm;
        public PacketList PLLoaded; // File Loaded
        public PacketList PL; // Filtered File Data Displayed
        // public PacketParser PP;
        public UInt16 CurrentSync;
        public string LoadedLogFile ;
        public VideoLinkForm videoLink ;
        public string ProjectFolder;
        public string ProjectFile;
        public string ProjectTags;
        public string LinkVideoFileName;
        public string LinkYoutubeURL;
        public string LinkPacketsDownloadURL;
        public TimeSpan LinkVideoOffset;

        public FlickerFreeListBox lbPackets;
        // Popup Menu Controls
        public ContextMenuStrip pmPL;
        public ToolStripMenuItem pmPLShowPacketName;
        public ToolStripSeparator pmPLS1;
        public ToolStripMenuItem pmPLShowOnly;
        public ToolStripMenuItem pmPLHideThis;
        public ToolStripSeparator pmPLS2;
        public ToolStripMenuItem pmPLShowOutOnly;
        public ToolStripMenuItem pmPLShowInOnly;
        public ToolStripSeparator pmPLS3;
        public ToolStripMenuItem pmPLResetFilters;
        public ToolStripSeparator pmPLS4;
        public ToolStripMenuItem pmPLEditParser;
        public ToolStripMenuItem pmPLExportPacket;

        public PacketTabPage(MainForm mainForm)
        {
            ownerMainForm = mainForm;

            // Create base controls
            PLLoaded = new PacketList();
            PL = new PacketList();
            lbPackets = new FlickerFreeListBox();
            videoLink = null;
            ProjectFolder = string.Empty ;
            LinkVideoFileName = string.Empty;
            LinkYoutubeURL = string.Empty;
            LinkPacketsDownloadURL = string.Empty;

            // Set ListBox Position
            lbPackets.Parent = this;
            lbPackets.Location = new System.Drawing.Point(0, 0);
            lbPackets.Size = new System.Drawing.Size(this.Width, this.Height);
            lbPackets.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lbPackets.Font = new Font("Consolas", 9); // Add fixedsized font (to override the tab page itself)
            lbPackets.DrawMode = DrawMode.OwnerDrawFixed;
            lbPackets.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lbPackets_DrawItem);
            // Add the SelectedIndexChanged for this from MainForm/process creating it, as it's too complex to handle internally
            // lbPackets.SelectedIndexChanged += new System.EventHandler(this.lbPackets_SelectedIndexChanged); 

            // Title to use on main program as "Filename"
            LoadedLogFile = "?Packets";
            ProjectFolder = string.Empty;
            ProjectTags = string.Empty;

            // Create Popup Menu
            pmPL = new ContextMenuStrip();
            pmPL.Opening += new CancelEventHandler(PmPL_Opening);

            lbPackets.ContextMenuStrip = pmPL;

            pmPLShowPacketName = new ToolStripMenuItem("Show packet name");
            pmPL.Items.Add(pmPLShowPacketName);

            pmPLS1 = new ToolStripSeparator();
            pmPL.Items.Add(pmPLS1);

            pmPLShowOnly = new ToolStripMenuItem("Show this type only");
            pmPLShowOnly.Click += new EventHandler(PmPLShowOnly_Click);
            pmPL.Items.Add(pmPLShowOnly);

            pmPLHideThis = new ToolStripMenuItem("Hide this type");
            pmPLHideThis.Click += new EventHandler(PmPLHideThis_Click);
            pmPL.Items.Add(pmPLHideThis);

            pmPLS2 = new ToolStripSeparator();
            pmPL.Items.Add(pmPLS2);

            pmPLShowOutOnly = new ToolStripMenuItem("Show only Outgoing");
            pmPLShowOutOnly.Click += new EventHandler(PmPLShowOutgoingOnly_Click);
            pmPL.Items.Add(pmPLShowOutOnly);

            pmPLShowInOnly = new ToolStripMenuItem("Show only Incoming");
            pmPLShowInOnly.Click += new EventHandler(PmPLShowIncomingOnly_Click);
            pmPL.Items.Add(pmPLShowInOnly);

            pmPLS3 = new ToolStripSeparator();
            pmPL.Items.Add(pmPLS3);

            pmPLResetFilters = new ToolStripMenuItem("Reset all filters");
            pmPLResetFilters.Click += new EventHandler(PmPLResetFilter_Click);
            pmPL.Items.Add(pmPLResetFilters);

            pmPLS4 = new ToolStripSeparator();
            pmPL.Items.Add(pmPLS4);

            pmPLEditParser = new ToolStripMenuItem("Edit this parser");
            pmPLEditParser.Click += new EventHandler(PmPLEditParser_Click);
            pmPL.Items.Add(pmPLEditParser);

            pmPLExportPacket = new ToolStripMenuItem("Export Packet");
            pmPLExportPacket.Click += new EventHandler(PmPLExport_Click);
            pmPL.Items.Add(pmPLExportPacket);

            // Init misc stuff
            CurrentSync = 0xFFFF;
        }

        public PacketData GetSelectedPacket()
        {
            if ((lbPackets.SelectedIndex < 0) || (lbPackets.SelectedIndex >= PL.Count()))
                return null;
            return PL.GetPacket(lbPackets.SelectedIndex);
        }

        public void lbPackets_DrawItem(object sender, DrawItemEventArgs e)
        {
            ListBox lb = (sender as ListBox);
            if (!(lb.Parent is PacketTabPage))
                return;
            PacketTabPage tp = (lb.Parent as PacketTabPage);
            PacketData pd = null;
            if ((e.Index >= 0) && (e.Index < tp.PL.Count()))
            {
                pd = tp.PL.GetPacket(e.Index);
            }
            else
            {
                // Draw the background of the ListBox control for each item.
                e.DrawBackground();
                return;
            }

            bool barOn = (tp.CurrentSync == pd.PacketSync);
            bool isSelected = (e.Index == lb.SelectedIndex);
            Color textCol;
            Color backCol;
            Color barCol;

            // Determine the color of the brush to draw each item based 
            // on the index of the item to draw.
            switch (pd.PacketLogType)
            {
                case PacketLogTypes.Incoming:
                    textCol = PacketColors.ColFontIN;
                    if (isSelected)
                    {
                        backCol = PacketColors.ColSelectIN;
                        textCol = PacketColors.ColSelectedFontIN;
                    }
                    else
                    if (barOn)
                        backCol = PacketColors.ColSyncIN;
                    else
                        backCol = PacketColors.ColBackIN;
                    barCol = PacketColors.ColBarIN;
                    break;
                case PacketLogTypes.Outgoing:
                    textCol = PacketColors.ColFontOUT;
                    if (isSelected)
                    {
                        backCol = PacketColors.ColSelectOUT;
                        textCol = PacketColors.ColSelectedFontOUT;
                    }
                    else
                    if (barOn)
                        backCol = PacketColors.ColSyncOUT;
                    else
                        backCol = PacketColors.ColBackOUT;
                    barCol = PacketColors.ColBarOUT;
                    break;
                default:
                    textCol = PacketColors.ColFontUNK;
                    if (isSelected)
                    {
                        backCol = PacketColors.ColSelectUNK;
                        textCol = PacketColors.ColSelectedFontUNK;
                    }
                    else
                    if (barOn)
                        backCol = PacketColors.ColSyncUNK;
                    else
                        backCol = PacketColors.ColBackUNK;
                    barCol = PacketColors.ColBarUNK;
                    break;
            }

            // Define the colors of our brushes.
            Brush textBrush = new SolidBrush(textCol);
            Brush backBrush = new SolidBrush(backCol);
            Brush barBrush = new SolidBrush(barCol);

            // Draw the background of the ListBox control for each item.
            e.Graphics.FillRectangle(backBrush, e.Bounds);
            // header text
            var s = lb.Items[e.Index].ToString();
            //s = pd.VirtualTimeStamp.ToString() + "." + pd.VirtualTimeStamp.Millisecond.ToString("0000");

            Rectangle icon1 = new Rectangle(e.Bounds.Left, e.Bounds.Top + ((e.Bounds.Height - Properties.Resources.mini_unk_icon.Height) / 2), Properties.Resources.mini_unk_icon.Width, Properties.Resources.mini_unk_icon.Height);
            Rectangle icon2 = new Rectangle(icon1.Left + icon1.Width, icon1.Top, icon1.Width, icon1.Height);

            Rectangle textBounds;
            if ((tp.videoLink != null) && (tp.videoLink.IsInTimeRange(pd.VirtualTimeStamp)))
            {
                e.Graphics.DrawImage(Properties.Resources.mini_video_icon, icon2);
            }

            if ((tp.LinkVideoFileName != string.Empty) || (tp.LinkYoutubeURL != string.Empty))
            { 
                textBounds = new Rectangle(e.Bounds.Left + (icon1.Width * 2), e.Bounds.Top, e.Bounds.Width - (icon1.Width * 2), e.Bounds.Height);
            }
            else
            {
                textBounds = new Rectangle(e.Bounds.Left + (icon1.Width), e.Bounds.Top, e.Bounds.Width - (icon1.Width), e.Bounds.Height);
            }

            switch (PacketColors.PacketListStyle)
            {
                case 1:
                    // Colored arrows
                    if (pd.PacketLogType == PacketLogTypes.Incoming)
                    {
                        e.Graphics.DrawImage(Properties.Resources.mini_in_icon, icon1);
                    }
                    else
                    if (pd.PacketLogType == PacketLogTypes.Outgoing)
                    {
                        e.Graphics.DrawImage(Properties.Resources.mini_out_icon, icon1);
                    }
                    else
                    {
                        e.Graphics.DrawImage(Properties.Resources.mini_unk_icon, icon1);
                    }
                    break;
                case 2:
                    // transparent arrows
                    if (pd.PacketLogType == PacketLogTypes.Incoming)
                    {
                        e.Graphics.DrawImage(Properties.Resources.mini_in_ticon, icon1);
                    }
                    else
                    if (pd.PacketLogType == PacketLogTypes.Outgoing)
                    {
                        e.Graphics.DrawImage(Properties.Resources.mini_out_ticon, icon1);
                    }
                    else
                    {
                        e.Graphics.DrawImage(Properties.Resources.mini_unk_ticon, icon1);
                    }
                    break;
                case 0:
                default:
                    textBounds = e.Bounds ;
                    // No icons, just text
                    if (pd.PacketLogType == PacketLogTypes.Incoming)
                    {
                        s = "<= " + s;
                    }
                    else
                    if (pd.PacketLogType == PacketLogTypes.Outgoing)
                    {
                        s = "=> " + s;
                    }
                    else
                    {
                        s = "?? " + s;
                    }
                    break;
            }

            // Draw the current item text based on the current Font 
            // and the custom brush settings.
            e.Graphics.DrawString(s,
                e.Font, textBrush, textBounds, StringFormat.GenericDefault);

            if (barOn)
            {
                var barSize = 8;
                if (isSelected)
                    barSize = 16;
                e.Graphics.FillRectangle(barBrush, new Rectangle(e.Bounds.Right - barSize, e.Bounds.Top, barSize, e.Bounds.Height));
            }
            // If the ListBox has focus, draw a focus rectangle around the selected item.
            e.DrawFocusRectangle();
        }

        public void CenterListBox()
        {
            // Move to center
            var iHeight = lbPackets.ItemHeight;
            if (iHeight <= 0)
                iHeight = 8;
            var iCount = lbPackets.Size.Height / iHeight;
            var tPos = lbPackets.SelectedIndex - (iCount / 2);
            if (tPos < 0)
                tPos = 0;
            lbPackets.TopIndex = tPos;
        }

        public void FillListBox(UInt16 GotTolastSync = 0)
        {
            int GotoIndex = -1;
            Application.UseWaitCursor = true;
            using (LoadingForm loadform = new LoadingForm(ownerMainForm))
            {
                try
                {
                    Random rand = new Random();
                    switch (rand.Next(100))
                    {
                        case 0:
                            loadform.BackColor = Color.DarkRed;
                            loadform.Text = "Sacrificing Taru-Taru's ...";
                            break;
                        case 1:
                            loadform.Text = "That's a lot of data ...";
                            break;
                        case 2:
                            loadform.Text = "Burning circles, please wait ...";
                            break;
                        case 3:
                            loadform.Text = "I'm bored ...";
                            break;
                        case 4:
                            loadform.Text = "Camping Shikigami Weapon, come back tomorrow ...";
                            break;
                        default:
                            loadform.Text = "Populating Listbox, please wait ...";
                            break;
                    }
                    loadform.Show();
                    loadform.pb.Minimum = 0;
                    loadform.pb.Maximum = PL.Count();
                    lbPackets.BeginUpdate();
                    lbPackets.Items.Clear();
                    for (int i = 0; i < PL.Count(); i++)
                    {
                        PacketData pd = PL.GetPacket(i);
                        /*
                        switch (pd.PacketLogType)
                        {
                            case PacketLogTypes.Outgoing:
                                lbPackets.Items.Add("=> " + pd.HeaderText);
                                break;
                            case PacketLogTypes.Incoming:
                                lbPackets.Items.Add("<= " + pd.HeaderText);
                                break;
                            default:
                                lbPackets.Items.Add("?? " + pd.HeaderText);
                                break;
                        }
                        */
                        lbPackets.Items.Add(pd.HeaderText);
                        if ((GotoIndex < 0) && (GotTolastSync > 0) && (pd.PacketSync >= GotTolastSync))
                        {
                            GotoIndex = lbPackets.Items.Count - 1 ;
                        }
                        loadform.pb.Value = i;
                        if ((i % 50) == 0)
                            loadform.pb.Refresh();
                    }
                    lbPackets.EndUpdate();
                    if (GotoIndex >= 0)
                    {
                        lbPackets.SelectedIndex = GotoIndex;
                    }
                    loadform.Hide();
                }
                catch
                {

                }
            }
            Application.UseWaitCursor = false;
        }


        private void PmPL_Opening(object sender, CancelEventArgs e)
        {
            var PD = GetSelectedPacket();
            if (PD == null)
            {
                e.Cancel = true;
                return;
            }
            pmPLShowPacketName.Text = PD.PacketLogType.ToString() + " - 0x" + PD.PacketID.ToString("X3");
            string ParserFileName ;
            switch (PD.PacketLogType)
            {
                case PacketLogTypes.Incoming:
                    ParserFileName = Path.Combine("data","parse","in-0x" + PD.PacketID.ToString("X3") + ".txt");
                    pmPLEditParser.Text = "Edit " + ParserFileName;
                    pmPLEditParser.Visible = true;
                    break;
                case PacketLogTypes.Outgoing:
                    ParserFileName = Path.Combine("data", "parse", "out-0x" + PD.PacketID.ToString("X3") + ".txt");
                    pmPLEditParser.Text = "Edit " + ParserFileName ;
                    pmPLEditParser.Visible = true;
                    break;
                default:
                    ParserFileName = "" ;
                    pmPLEditParser.Text = "";
                    pmPLEditParser.Visible = false;
                    break;
            }
            pmPLEditParser.Tag = ParserFileName;
            pmPLShowOnly.Enabled = (PD.PacketLogType != PacketLogTypes.Unknown);
            pmPLHideThis.Enabled = (PD.PacketLogType != PacketLogTypes.Unknown);


        }

        private void PmPLEditParser_Click(object sender, EventArgs e)
        {
            if ((pmPLEditParser.Tag == null) || ((string)pmPLEditParser.Tag == string.Empty))
                return;
            ownerMainForm.OpenParseEditor((string)pmPLEditParser.Tag);
        }

        private void PmPLShowOnly_Click(object sender, EventArgs e)
        {
            var PD = GetSelectedPacket();
            if (PD == null)
                return;

            switch (PD.PacketLogType)
            {
                case PacketLogTypes.Incoming:
                    PL.Filter.Clear();
                    PL.Filter.FilterInType = FilterType.ShowPackets;
                    PL.Filter.FilterInList.Add(PD.PacketID);
                    PL.Filter.FilterOutType = FilterType.AllowNone;
                    break;
                case PacketLogTypes.Outgoing:
                    PL.Filter.Clear();
                    PL.Filter.FilterOutType = FilterType.ShowPackets;
                    PL.Filter.FilterOutList.Add(PD.PacketID);
                    PL.Filter.FilterInType = FilterType.AllowNone;
                    break;
                default:
                    return;
            }
            var lastSync = CurrentSync;
            PL.FilterFrom(PLLoaded);
            FillListBox(lastSync);
            CenterListBox();
        }

        private void PmPLHideThis_Click(object sender, EventArgs e)
        {
            var PD = GetSelectedPacket();
            if (PD == null)
                return;

            switch (PD.PacketLogType)
            {
                case PacketLogTypes.Incoming:
                    if (PL.Filter.FilterInType != FilterType.HidePackets)
                    {
                        PL.Filter.FilterInType = FilterType.HidePackets;
                        PL.Filter.FilterInList.Clear();
                    }
                    PL.Filter.FilterInList.Add(PD.PacketID);
                    break;
                case PacketLogTypes.Outgoing:
                    if (PL.Filter.FilterOutType != FilterType.HidePackets)
                    {
                        PL.Filter.FilterOutType = FilterType.HidePackets;
                        PL.Filter.FilterOutList.Clear();
                    }
                    PL.Filter.FilterOutList.Add(PD.PacketID);
                    break;
                default:
                    return;
            }
            var lastSync = CurrentSync;
            PL.FilterFrom(PLLoaded);
            FillListBox(lastSync);
            CenterListBox();
        }

        private void PmPLShowIncomingOnly_Click(object sender, EventArgs e)
        {
            var PD = GetSelectedPacket();
            if (PD == null)
                return;

            if ((PL.Filter.FilterInType == FilterType.AllowNone) || (PL.Filter.FilterInType == FilterType.HidePackets))
            {
                PL.Filter.FilterInType = FilterType.Off;
            }
            PL.Filter.FilterOutType = FilterType.AllowNone;

            var lastSync = CurrentSync;
            PL.FilterFrom(PLLoaded);
            FillListBox(lastSync);
            CenterListBox();
        }

        private void PmPLShowOutgoingOnly_Click(object sender, EventArgs e)
        {
            var PD = GetSelectedPacket();
            if (PD == null)
                return;

            if ((PL.Filter.FilterOutType == FilterType.AllowNone) || (PL.Filter.FilterOutType == FilterType.HidePackets))
            {
                PL.Filter.FilterOutType = FilterType.Off;
            }
            PL.Filter.FilterInType = FilterType.AllowNone;

            var lastSync = CurrentSync;
            PL.FilterFrom(PLLoaded);
            FillListBox(lastSync);
            CenterListBox();
        }

        private void PmPLResetFilter_Click(object sender, EventArgs e)
        {
            var PD = GetSelectedPacket();
            if (PD == null)
                return;

            PL.Filter.Clear();
            var lastSync = CurrentSync;
            PL.FilterFrom(PLLoaded);
            FillListBox(lastSync);
            CenterListBox();
        }

        private void PmPLExport_Click(object sender, EventArgs e)
        {
            var PD = GetSelectedPacket();
            if (PD == null)
                return;

            string exportName = "";
            switch(PD.PacketLogType)
            {
                case PacketLogTypes.Incoming:
                    exportName += "i";
                    break;
                case PacketLogTypes.Outgoing:
                    exportName += "o";
                    break;
                default:
                    exportName += "u";
                    break;
            }
            exportName += PD.PacketID.ToString("X3");

            using (var saveDlg = new SaveFileDialog())
            {
                saveDlg.FileName = exportName;
                saveDlg.CheckPathExists = true;
                if (saveDlg.ShowDialog() != DialogResult.OK)
                    return;
                exportName = saveDlg.FileName;
            }

            try
            {
                File.WriteAllBytes(exportName,PD.RawBytes.ToArray());
            }
            catch (Exception x)
            {
                MessageBox.Show("Error saving raw packet " + exportName + "\r\nException: " + x.Message);
            }

        }

        public bool LoadProjectFile(string aProjectFile)
        {
            LoadedLogFile = string.Empty;
            LinkVideoFileName = string.Empty;
            LinkVideoOffset = TimeSpan.Zero;
            LinkYoutubeURL = string.Empty;
            LinkPacketsDownloadURL = string.Empty;
            ProjectFolder = Path.GetDirectoryName(aProjectFile).TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar;
            try
            {
                string[] sl;
                if (File.Exists(aProjectFile))
                {
                    sl = File.ReadAllLines(aProjectFile);
                }
                else
                {
                    sl = new string[0];
                }
                foreach (string s in sl)
                {
                    var fields = s.Split(';');
                    if (fields.Length < 2)
                        continue;
                    if (fields[0].ToLower() == "packetlog")
                    {
                        LoadedLogFile = Helper.TryMakeFullPath(ProjectFolder, fields[1]);
                    }
                    else
                    if (fields[0].ToLower() == "video")
                    {
                        LinkVideoFileName = Helper.TryMakeFullPath(ProjectFolder, fields[1]);
                    }
                    else
                    if (fields[0].ToLower() == "youtube")
                    {
                        LinkYoutubeURL = fields[1];
                    }
                    else
                    if (fields[0].ToLower() == "offset")
                    {
                        if (DataLookups.TryFieldParse(fields[1], out int n))
                            LinkVideoOffset = TimeSpan.FromMilliseconds(n);
                    }
                    else
                    if (fields[0].ToLower() == "packedsource")
                    {
                        LinkPacketsDownloadURL = fields[1];
                    }
                    else
                    if (fields[0].ToLower() == "tags")
                    {
                        ProjectTags = fields[1];
                    }
                    else
                    if (fields[0].ToLower() == "pin")
                    {
                        // not used
                    }
                    else
                    if (fields[0].ToLower() == "pout")
                    {
                        // not used
                    }
                    else
                    {
                        continue;
                    }

                }
                // Set on success
                // if (sl.Length >= 1)
                    ProjectFile = aProjectFile;
            }
            catch
            {
                LoadedLogFile = string.Empty;
                LinkVideoFileName = string.Empty;
                LinkVideoOffset = TimeSpan.Zero;
                LinkYoutubeURL = string.Empty;
                LinkPacketsDownloadURL = string.Empty;
            }

            return true;
        }

        public bool LoadProjectFileFromLogFile(string fromLogFile)
        {
            if ((fromLogFile != null) && (fromLogFile != string.Empty))
                ProjectFolder = Helper.MakeProjectDirectoryFromLogFileName(fromLogFile);

            var aProjectFile = ProjectFolder + Path.GetFileName(ProjectFolder.TrimEnd('\\')) + ".pvlv";

            return LoadProjectFile(aProjectFile);
        }

        public bool SaveProjectFile()
        {
            if ((ProjectFolder == null) || (ProjectFolder == string.Empty))
                return false;

            // Generate Filename if needed
            if ((ProjectFile == null) || (ProjectFile == string.Empty))
            {
                var partialProjectFileName = Path.GetFileName(ProjectFolder.TrimEnd('\\'));
                // don't create in a drive root directory
                if (partialProjectFileName == string.Empty)
                    return false;
                ProjectFile = ProjectFolder + partialProjectFileName + ".pvlv";
            }

            if ( (Properties.Settings.Default.AskCreateNewProjectFile == true) && (!File.Exists(ProjectFile)) )
            {
                if (MessageBox.Show("Do you want to save project settings as a new project file ?\r\n" + ProjectFile, "Create Project File", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                {
                    ProjectFile = string.Empty;
                    return false;
                }
            }


            string relVideo = string.Empty;
            if ((LinkVideoFileName != null) && (LinkVideoFileName != string.Empty))
                relVideo = Helper.MakeRelative(ProjectFolder, LinkVideoFileName);

            string relLogFile = string.Empty;
            if ((LoadedLogFile != null) && (LoadedLogFile != string.Empty))
                relLogFile = Helper.MakeRelative(ProjectFolder, LoadedLogFile);

            try
            {
                string pin = string.Empty;
                foreach(UInt16 n in PLLoaded.ContainsPacketsIn)
                {
                    if (pin != string.Empty)
                        pin += ",";
                    pin += n.ToString("X3");
                }
                string pout = string.Empty;
                foreach (UInt16 n in PLLoaded.ContainsPacketsOut)
                {
                    if (pout != string.Empty)
                        pout += ",";
                    pout += n.ToString("X3");
                }

                List<string> sl = new List<string>();
                sl.Add("rem;PacketViewerLogViewer Project File");
                sl.Add("packetlog;" + relLogFile);
                sl.Add("tags;" + ProjectTags);
                sl.Add("video;" + relVideo);
                sl.Add("packedsource;" + LinkPacketsDownloadURL);
                sl.Add("youtube;" + LinkYoutubeURL);
                sl.Add("offset;" + LinkVideoOffset.TotalMilliseconds.ToString());
                sl.Add("pin;" + pin);
                sl.Add("pout;" + pout);
                File.WriteAllLines(ProjectFile, sl);
                return true;
            }
            catch
            {
                return false;
            }
        }

    }

}
