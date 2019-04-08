using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketViewerLogViewer.Packets
{
    public enum PacketLogTypes { Unknown, OutGoing, InComming }

    public static class String6BitEncodeKeys
    {
        static char[] Item = new char[0x40] {
        //   0    1    2    3    4    5    6    7    8    9    A    B    C    D    E    F
           '\0', '0', '1', '2', '3', '4', '5', '6', '7', '9', '8', 'A', 'B', 'C', 'D', 'E', // 0x00
            'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', // 0x10
            'V', 'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', // 0x20
            'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', '\0' // 0x30
        };
        static char[] Linkshell = new char[0x40] {
        //   0    1    2    3    4    5    6    7    8    9    A    B    C    D    E    F
           '\0', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', // 0x00
            'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', // 0x10
            'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', // 0x20
            'V', 'W', 'X', 'Y', 'Z', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '\0' // 0x30
        };
    }

    public class PacketData
    {
        public List<String> RawText { get; protected set; }
        public string HeaderText { get; protected set; }
        public string OriginalHeaderText { get; protected set; }
        public List<byte> RawBytes { get; protected set; }
        public PacketLogTypes PacketLogType { get; protected set; }
        public UInt16 PacketID { get; protected set; }
        public UInt16 PacketDataSize { get; protected set; }
        public UInt16 PacketSync { get; protected set; }
        public DateTime VirtualTimeStamp { get; protected set; }
        public string OriginalTimeString { get; protected set; }

        PacketData()
        {
            RawText = new List<string>();
            HeaderText = "Unknown Header";
            OriginalHeaderText = "";
            RawBytes = new List<byte>();
            PacketLogType = PacketLogTypes.Unknown;
            PacketID = 0x000 ;
            PacketDataSize = 0x0000 ;
            PacketSync = 0x0000 ;
            VirtualTimeStamp = new DateTime(0);
            OriginalTimeString = "";
        }

        ~PacketData()
        {
            RawText.Clear();
            RawBytes = null ;
        }

        public int AddRawLineAsBytes(string s)
        {
            /* Example:
            //        1         2         3         4         5         6         7         8         9
            //34567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890

            [2018-05-16 18:11:35] Outgoing packet 0x015:
                    |  0  1  2  3  4  5  6  7  8  9  A  B  C  D  E  F      | 0123456789ABCDEF
                -----------------------------------------------------  ----------------------
                  0 | 15 10 9E 00 CF 50 A0 C3 04 0E 1C C2 46 BF 33 43    0 | .....P......F.3C
                  1 | 00 00 02 00 5D 00 00 00 49 97 B8 69 00 00 00 00    1 | ....]...I..i....

            // 1         2         3         4         5         6         7         8         9
            // 123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890
            // 5 | 00 00 00 00 -- -- -- -- -- -- -- -- -- -- -- --    5 | ....------------
            */

            // if (s.Length < 81)
            if (s.Length < 57)
            {
                // Doesn't look like a correct format
                return 0;
            }

            int c = 0;
            for(int i = 0 ; i <= 0xf ; i++)
            {
                var h = s.Substring(11 + (i * 3), 2);
                if (h != "--")
                {
                    if (!byte.TryParse("0x" + h, out byte b))
                        break;
                    RawBytes.Add(b);
                    c++;
                }
            }
            return c;
        }

        public int AddRawPacketeerLineAsBytes(string s)
        {
            /* Example:
            // 1         2         3         4         5         6         7         8         9
            // 123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890
            // [C->S] Id: 001A | Size: 28
            // 1A 0E ED 24 D5 10 10 01 D5 00 00 00 00 00 00 00  ..í$Õ...Õ.......
            */

            if (s.Length < 51)
            {
                // Doesn't look like a correct format
                return 0;
            }

            int c = 0;
            for (int i = 0; i <= 0xf; i++)
            {
                var h = s.Substring(11 + (i * 3), 2);
                // If this fails, we're probably at the end of the packet
                // Unlike windower, Packeteer doesn't add dashes for the blanks
                if ((h != "--") && (h != "  ") && (h != " "))
                {
                    if (!byte.TryParse("0x" + h, out byte b))
                        break;
                    RawBytes.Add(b);
                    c++;
                }
            }
            return c;
        }

        public string PrintRawBytesAsHex()
        {
            const int ValuesPerRow = 16;
            string res = "";
            res += "   |  0  1  2  3   4  5  6  7   8  9  A  B   C  D  E  F\r\n";
            res += "---+----------------------------------------------------\r\n";
            int lineNumber = 0;
            for(int i = 0; i < RawBytes.Count; i++)
            {
                if ((i % ValuesPerRow) == 0)
                    res += lineNumber.ToString("X2") + " | " ;

                res += RawBytes[i].ToString("X2");

                if ((i % ValuesPerRow) == (ValuesPerRow - 1))
                {
                    res += "\r\n" ;
                    lineNumber++;
                }
                else
                {
                    res += " " ;
                    if ((i % 4) == 3)
                        res += " " ; // Extra space every 4 bytes
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

        public bool GetBitAtPos(int pos, int bit)
        {
            if ((pos > (RawBytes.Count - 1)) || ((bit < 0) || (bit > 7)))
                return false;
            byte b = RawBytes[pos];
            byte bitmask = (byte)(0x01 << bit);
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
            string res = "";
            int i = 0;
            while ((i < RawBytes.Count) && (RawBytes[i] != 0) && ((maxSize == -1) || (res.Length < maxSize)))
            {
                res += (char)RawBytes[i];
                i++;
            }
            return res;
        }

        public string GetDataAtPos(int pos, int size)
        {
            string res = "";
            int i = 0;
            while ( ((i+pos) < RawBytes.Count) && (i < size) && (i < 256) )
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
            for(uint BitShiftCount = 0; BitShiftCount < 32; BitShiftCount++)
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

        public Int64 GetBitsAtPos(int BitOffset, int BitsSize)
        {
            return GetBitsAtPos(BitOffset / 8, BitOffset % 8);
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
                var encodedChar = GetBitsAtPos(pos + (Offset / 8), (Offset % 8), 6);
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
            for(int i = 0; i < (RawBytes.Count - 2); i++)
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



        /*
        Function TPacketData.CompileData: Boolean;
        VAR
          S, TS: String;
          P1, P2: Integer;
        begin
          Result := False;
          If Length(fRawBytes) < 4 then
          Begin
            fPacketID := $FFFF; // invalid data
            fPacketDataSize := 0;
            fHeaderText := 'Invalid Packet Size < 4';
            exit;
          End;
          fPacketID := fRawBytes[$0] + ((fRawBytes[$1] AND $01) * $100);
          fPacketDataSize := (fRawBytes[$1] AND $FE) * 2;
          // basically, all packets are always multiples of 4 bytes
          fPacketSync := fRawBytes[$2] + (fRawBytes[$3] * $100); // packet order number

          If (Pos('[c->s]', LowerCase(fOriginalTimeString)) > 0) or
            (Pos('[s->c]', LowerCase(fOriginalTimeString)) > 0) Then
          Begin
            // Packeteer doesn't have time info (yet)
            TS := '';
            fTimeStamp := 0;
            fVirtualTimeStamp := 0;
            fOriginalTimeString := '0000-00-00 00:00';
          End
          Else
          Begin
            // Try to determine timestamp from header
            fOriginalTimeString := '';
            P1 := Pos('[', fOriginalHeaderText);
            P2 := Pos(']', fOriginalHeaderText);
            If (P1 > 0) and (P2 > 0) and (P2 > P1) Then
            Begin
              fOriginalTimeString := Copy(fOriginalHeaderText, P1 + 1, P2 - P1 - 1);
              If (Length(fOriginalTimeString) > 0) Then
                Try
                  fTimeStamp := VarToDateTime(fOriginalTimeString);
                  fVirtualTimeStamp := fTimeStamp;
                  // <-- seems to work better than anything I'd like to try
                  // fTimeStamp := StrToDateTime(fOriginalTimeString);
                  DateTimeToString(TS, 'hh:nn:ss', TimeStamp);
                Except
                  TS := '';
                  fTimeStamp := 0;
                  fVirtualTimeStamp := 0;
                  fOriginalTimeString := '0000-00-00 00:00';
                End;
            End;
          End;

          If (fTimeStamp = 0) Then
            TS := '';
          // If (fTimeStamp = 0) Then TS := '??:??:??' ;

          Case PacketLogType Of
            1:
              S := 'OUT ';
            2:
              S := 'IN  ';
          Else
            S := '??? ';
          End;
          S := TS + ' : ' + S + '0x' + IntToHex(PacketID, 3) + ' - ';

          fHeaderText := S + PacketTypeToString(PacketLogType, PacketID);
          Result := true;
        end;

        */


    }

}
