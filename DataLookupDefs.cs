using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace PacketViewerLogViewer.Packets
{
    public struct DataLookupEntry
    {
        Int64 ID ;
        string Val ;
        string Extra ;
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

        // lookupname, id, lookupresult
        static public Dictionary<string,Dictionary<UInt64, DataLookupEntry>> NLU = new Dictionary<string,Dictionary<UInt64, DataLookupEntry>>();
        
        static DataLookups()
        {
            //
        }

        static void LoadLookupFile(string fileName)
        {
            var lookupname = Path.GetFileNameWithoutExtension(fileName).ToLower();
            List<string> sl = File.ReadAllLines(fileName).ToList();

            // TODO: parse file

        }

        public static void LoadLookups()
        {
            NLU.Clear();
            DirectoryInfo DI = new DirectoryInfo(System.Windows.Forms.Application.StartupPath + Path.DirectorySeparatorChar + "lookup");
            foreach(var fi in DI.GetFiles())
            {
                LoadLookupFile(fi.FullName);
            }
        }


    }
}
