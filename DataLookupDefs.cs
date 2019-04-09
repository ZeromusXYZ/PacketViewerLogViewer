using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;

namespace PacketViewerLogViewer.Packets
{
    public struct DataLookupEntry
    {
        public UInt64 ID ;
        public string Val ;
        public string Extra ;
    }

    public class DataLookupList
    {
        public Dictionary<UInt64, DataLookupEntry> data = new Dictionary<UInt64, DataLookupEntry>();

        public string GetValue(UInt64 ID)
        {
            DataLookupEntry res;
            if (data.TryGetValue(ID, out res))
                return res.Val;
            return "";
        }

        public string GetExtra(UInt64 ID)
        {
            DataLookupEntry res;
            if (data.TryGetValue(ID, out res))
                return res.Extra;
            return "";
        }
    }

    static public class DataLookups
    {
        public static string LU_PacketOut = "out";
        public static string LU_PacketIn = "in";
        public static string LU_Zones = "zones";
        public static string LU_EquipmentSlots = "equipslot";
        public static string LU_Container = "containers";
        public static string LU_Item = "items";
        public static string LU_ItemModel = "itemmodels";
        public static string LU_Music = "music";
        public static string LU_Job = "jobs";
        public static string LU_Weather = "weather";
        public static string LU_Merit = "merits";
        public static string LU_JobPoint = "jobpoints";
        public static string LU_Spell = "spells";
        public static string LU_WeaponSkill = "weaponskill";
        public static string LU_Ability = "ability";
        public static string LU_ARecast = "abilityrecast";
        public static string LU_PetCommand = "petcommand";
        public static string LU_Trait = "trait";
        public static string LU_Mounts = "mounts";
        public static string LU_RoE = "roe";

        public static DataLookupList NullList = new DataLookupList();
        public static DataLookupEntry NullEntry = new DataLookupEntry();

        // lookupname, id, lookupresult
        static public Dictionary<string, DataLookupList> LookupLists = new Dictionary<string, DataLookupList>();

        static DataLookups()
        {
            NullEntry.ID = 0;
            NullEntry.Val = "NULL";
            NullEntry.Extra = "";
        }

        private static bool TryFieldParse(string field, out UInt64 res)
        {
            bool result = false ;
            if (field.StartsWith("0x"))
            {
                try
                {
                    res = UInt64.Parse(field.Substring(2, field.Length - 2), NumberStyles.HexNumber);
                    result = true;
                }
                catch
                {
                    res = 0;
                }
            }
            else
            if (field.StartsWith("$"))
            {
                try
                {
                    res = UInt64.Parse(field.Substring(1, field.Length - 1), NumberStyles.HexNumber);
                    result = true;
                }
                catch
                {
                    res = 0;
                }
            }
            else
            {
                try
                {
                    res = UInt64.Parse(field);
                    result = true;
                }
                catch
                {
                    res = 0;
                }
            }
            return result;
        }
        


        static void LoadLookupFile(string fileName)
        {
            // Extract name
            var lookupname = Path.GetFileNameWithoutExtension(fileName).ToLower();
            // Create new list
            DataLookupList dll = new DataLookupList();
            // Add it
            LookupLists.Add(lookupname,dll);
            // Load file
            List<string> sl = File.ReadAllLines(fileName).ToList();
            // Parse File
            foreach(string line in sl)
            {
                string[] fields = line.Split(';');
                if (fields.Length > 1)
                {
                    if (TryFieldParse(fields[0], out UInt64 newID))
                    {
                        DataLookupEntry dle = new DataLookupEntry();
                        dle.ID = newID;
                        dle.Val = fields[1];
                        if (fields.Length > 2)
                            dle.Extra = fields[2];
                        dll.data.Add(newID,dle);
                    }
                }
            }
        }

        public static void LoadLookups()
        {
            LookupLists.Clear();
            var lookupPath = System.Windows.Forms.Application.StartupPath + Path.DirectorySeparatorChar + "lookup";
            DirectoryInfo DI = new DirectoryInfo(lookupPath);
            if (Directory.Exists(lookupPath))
            {
                foreach (var fi in DI.GetFiles())
                {
                    LoadLookupFile(fi.FullName);
                }
            }
        }

        public static DataLookupList NLU(string lookupName)
        {
            DataLookupList res;
            if (LookupLists.TryGetValue(lookupName, out res))
                return res;
            return NullList;
        }

        public static string PacketTypeToString(PacketLogTypes PacketLogType, UInt16 PacketID)
        {
            string res = "";
            if (PacketLogType == PacketLogTypes.Outgoing)
                res = NLU(LU_PacketOut).GetValue(PacketID);
            if (PacketLogType == PacketLogTypes.Incoming)
                res = NLU(LU_PacketIn).GetValue(PacketID);
            if (res == "")
                res = "??? unknown";
            return res;
        }

    }
}
