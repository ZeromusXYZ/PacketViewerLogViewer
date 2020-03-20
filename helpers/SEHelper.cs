using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Linq;
using System.Xml.Linq;
using System.Globalization;
using Microsoft.Win32;

namespace PacketViewerLogViewer.SEUtils
{

    public class FFXI_Item : IComparable
    {
        public uint Id { get; set; }
        public uint Flags { get; set; }
        public uint Type { get; set; }
        public uint StackSize { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string NameSingle { get; set; }
        public string NameMultiple { get; set; }

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;
            return Id.CompareTo((obj as FFXI_Item).Id);
        }
    }

    /*
    public struct FFXI_VTable_Item
    {
        public byte RomFolder;
    }
    */

    public struct FFXI_FTable_Item
    {
        public uint FileId; // Just for reference
        public byte RomFolder;
        public uint SubFolder;
        public uint DatFile;
        public string FullFileName;
    }

    static public class SEHelper
    {
        static public string FFXI_InstallationPath { get; private set; }
        static public string POL_InstallationPath { get; private set; }
        static public string TetraMaster_InstallationPath { get; private set; }

        static public Dictionary<uint,FFXI_FTable_Item> FFXI_FTable = new Dictionary<uint, FFXI_FTable_Item>();
        static public int FFXI_FTableCount = 0;

        private const string GameID_POL = "1000";
        private const string GameID_FFXI = "0001";
        private const string GameID_TetraMaster = "0002";
        // public const string GameID_FFXITC = "0015";

        [Flags]
        private enum Regions
        {
            None = 0x00,
            JP = 0x01,
            NA = 0x02,
            EU = 0x04,
        }

        static public List<FFXI_Item> ReadItemListFromXML(string ItemXmlFile)
        {
            List<FFXI_Item> res = new List<FFXI_Item>();

            XmlDocument D = new XmlDocument();
            D.Load(ItemXmlFile);

            foreach (XmlNode itemNode in D.DocumentElement.ChildNodes)
            {
                if (itemNode.Name == "thing")
                {
                    var newItem = new FFXI_Item();
                    foreach (XmlNode itemElement in itemNode.ChildNodes)
                    {
                        if (itemElement.Name == "field")
                        {
                            var fieldName = itemElement.Attributes["name"]?.Value.ToLower();
                            var fieldValueText = itemElement?.InnerText;
                            uint.TryParse(fieldValueText, out uint fieldValueUInt);
                            uint.TryParse(fieldValueText, System.Globalization.NumberStyles.HexNumber, CultureInfo.InvariantCulture , out uint fieldValueUIntHex);

                            switch (fieldName)
                            {
                                case "id":
                                    newItem.Id = fieldValueUInt;
                                    break;
                                case "flags":
                                    newItem.Flags = fieldValueUIntHex;
                                    break;
                                case "stack-size":
                                    newItem.StackSize = fieldValueUInt;
                                    break;
                                case "type":
                                    newItem.Type = fieldValueUIntHex;
                                    break;
                                case "name":
                                    newItem.Name = fieldValueText;
                                    break;
                                case "description":
                                    newItem.Description = fieldValueText;
                                    break;
                                case "log-name-singular":
                                    newItem.NameSingle = fieldValueText;
                                    break;
                                case "log-name-plural":
                                    newItem.NameMultiple = fieldValueText;
                                    break;

                            }

                        }

                    }

                    // Only add a item if it has some usefull info in it
                    if ((newItem.Id > 0) && (newItem.Name != null) && (newItem.Name != string.Empty) && (newItem.Name != "."))
                    {
                        res.Add(newItem);
                    }
                }
            }

            return res;
        }


        static private RegistryKey OpenRegistryKey(Regions Region, string KeyName)
        {
            string SubKey;
            {
                string POLKeyName = "PlayOnline";
                switch (Region)
                {
                    case Regions.EU:
                        POLKeyName += "EU";
                        break;
                    case Regions.JP:
                        break;
                    case Regions.NA:
                        POLKeyName += "US";
                        break;
                    default:
                        return null;
                }
                SubKey = Path.Combine(POLKeyName, KeyName);
            }
            try
            {
                using (RegistryKey Win64Root = Registry.LocalMachine.OpenSubKey(@"Software\WOW6432Node"))
                {
                    if (Win64Root != null)
                    {
                        RegistryKey Win64Key = Win64Root.OpenSubKey(SubKey, false);
                        if (Win64Key != null)
                        {
                            return Win64Key;
                        }
                    }
                }
            }
            catch { }
            try
            {
                return Registry.LocalMachine.OpenSubKey(Path.Combine("Software", SubKey), false);
            }
            catch
            {
                return null;
            }
        }

        static private string GetInstalledGamePath(string GameID, Regions Region)
        {
            RegistryKey POLKey = OpenRegistryKey(Region, "InstallFolder");
            if (POLKey == null)
            {
                return string.Empty;
            }
            string InstallPath = POLKey.GetValue(GameID, string.Empty) as string;
            POLKey.Close();
            if ((InstallPath != string.Empty) && (!InstallPath.EndsWith(Path.DirectorySeparatorChar.ToString())))
                InstallPath += Path.DirectorySeparatorChar;
            return InstallPath;
        }

        static public bool FFXI_LoadFileTables()
        {
            // Do we have a installation ?
            if ((FFXI_InstallationPath == null) || (FFXI_InstallationPath == string.Empty) || (!Directory.Exists(FFXI_InstallationPath)))
                return false;
            
            // Did we already load this ?
            if (FFXI_FTable.Count > 0)
                return true;

            try
            {
                byte[] FFXI_VTable = new byte[0];
                FFXI_FTable.Clear();

                for (byte RomDir = 1; RomDir < 0xF;RomDir++)
                {
                    string RomDirPath = FFXI_InstallationPath;
                    if (RomDir != 1)
                        RomDirPath = Path.Combine(FFXI_InstallationPath, "Rom" + RomDir.ToString());
                    else
                        RomDirPath = Path.Combine(FFXI_InstallationPath, "Rom");

                    string VTableFile = FFXI_InstallationPath;
                    if (RomDir != 1)
                        VTableFile = Path.Combine(VTableFile, "Rom" + RomDir.ToString(), "VTABLE" + RomDir.ToString() + ".DAT");
                    else
                        VTableFile = Path.Combine(VTableFile, "VTABLE.DAT");

                    string FTableFile = FFXI_InstallationPath;
                    if (RomDir != 1)
                        FTableFile = Path.Combine(FTableFile, "Rom" + RomDir.ToString(), "FTABLE" + RomDir.ToString() + ".DAT");
                    else
                        FTableFile = Path.Combine(FTableFile, "FTABLE.DAT");

                    if ((!File.Exists(VTableFile)) || (!File.Exists(FTableFile)))
                        continue;

                    // Open VTable and FTable files for this ROM Folder
                    FileStream VTableFS = new FileStream(VTableFile, FileMode.Open, FileAccess.Read);
                    FileStream FTableFS = new FileStream(FTableFile, FileMode.Open, FileAccess.Read);

                    // File Size check
                    if (FTableFS.Length != (VTableFS.Length * 2))
                        throw new Exception("File table size mismatch for Rom Folder " + RomDir.ToString());

                    // Change VTable size if needed
                    if (FFXI_VTable.Length < VTableFS.Length)
                        Array.Resize(ref FFXI_VTable, (int)VTableFS.Length);

                    BinaryReader VTableBR = new BinaryReader(VTableFS);
                    BinaryReader FTableBR = new BinaryReader(FTableFS);
                    for (uint i = 0; i < VTableBR.BaseStream.Length;i++)
                    {
                        byte thisRomFolder = VTableBR.ReadByte();
                        ushort thisRomFileLocation = FTableBR.ReadUInt16();

                        if (thisRomFolder > 0)
                        {
                            // Sanity Check
                            if (FFXI_VTable[i] != 0)
                                throw new Exception("File ID " + i.ToString() + " in " + VTableFile + " already has a ROM folder set");
                            FFXI_VTable[i] = thisRomFolder;

                            ushort SubDirLocation = (ushort)(thisRomFileLocation / 0x80);
                            byte FileLocation = (byte)(thisRomFileLocation % 0x80);

                            var FTableEntry = new FFXI_FTable_Item();
                            FTableEntry.FileId = i;
                            FTableEntry.RomFolder = RomDir;
                            FTableEntry.SubFolder = SubDirLocation;
                            FTableEntry.DatFile = FileLocation;
                            FTableEntry.FullFileName = Path.Combine(RomDirPath, SubDirLocation.ToString(),FileLocation.ToString()+".dat");
                            FFXI_FTable.Add(i, FTableEntry);
                        }
                    }
                    
                }
                FFXI_FTableCount = FFXI_VTable.Length;
            }
            catch 
            {
                return false;
            }
            return true;
        }

        static public void FindPaths()
        {
            // PlayOnline
            if (POL_InstallationPath == null)
                POL_InstallationPath = string.Empty;

            if (POL_InstallationPath == string.Empty)
                POL_InstallationPath = GetInstalledGamePath(GameID_POL, Regions.JP);

            if (POL_InstallationPath == string.Empty)
                POL_InstallationPath = GetInstalledGamePath(GameID_POL, Regions.NA);

            if (POL_InstallationPath == string.Empty)
                POL_InstallationPath = GetInstalledGamePath(GameID_POL, Regions.EU);

            // Final Fantasy XI
            if (FFXI_InstallationPath == null)
                FFXI_InstallationPath = string.Empty;

            if (FFXI_InstallationPath == string.Empty)
                FFXI_InstallationPath = GetInstalledGamePath(GameID_FFXI, Regions.JP);

            if (FFXI_InstallationPath == string.Empty)
                FFXI_InstallationPath = GetInstalledGamePath(GameID_FFXI, Regions.NA);

            if (FFXI_InstallationPath == string.Empty)
                FFXI_InstallationPath = GetInstalledGamePath(GameID_FFXI, Regions.EU);

            // Tetra Master
            if (TetraMaster_InstallationPath == null)
                TetraMaster_InstallationPath = string.Empty;

            if (TetraMaster_InstallationPath == string.Empty)
                TetraMaster_InstallationPath = GetInstalledGamePath(GameID_TetraMaster, Regions.JP);

            if (TetraMaster_InstallationPath == string.Empty)
                TetraMaster_InstallationPath = GetInstalledGamePath(GameID_TetraMaster, Regions.NA);

            if (TetraMaster_InstallationPath == string.Empty)
                TetraMaster_InstallationPath = GetInstalledGamePath(GameID_TetraMaster, Regions.EU);

            FFXI_LoadFileTables();
        }

    }
}
