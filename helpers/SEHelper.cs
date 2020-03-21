using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Linq;
using System.Xml.Linq;
using System.Globalization;
using Microsoft.Win32;
using PlayOnline.FFXI;

namespace PacketViewerLogViewer.SEUtils
{
    public enum FFXI_ItemFlags : ushort
    {
        None = 0x0000,
        // Simple Flags - mostly assumed meanings
        WallHanging = 0x0001, // Used by furnishing like paintings.
        Flag01 = 0x0002,
        MysteryBox = 0x0004,  // Can be gained from Gobbie Mystery Box
        MogGarden = 0x0008,   // Can use in Mog Garden
        CanSendPOL = 0x0010,
        Inscribable = 0x0020,
        NoAuction = 0x0040,
        Scroll = 0x0080,
        Linkshell = 0x0100,
        CanUse = 0x0200,
        CanTradeNPC = 0x0400,
        CanEquip = 0x0800,
        NoSale = 0x1000,
        NoDelivery = 0x2000,
        NoTradePC = 0x4000,
        Rare = 0x8000,
        // Combined Flags
        Ex = 0x6040, // NoAuction + NoDelivery + NoTrade
    }

    public enum FFXI_ItemType : ushort
    {
        Nothing = 0x0000,
        Item = 0x0001,
        QuestItem = 0x0002,
        Fish = 0x0003,
        Weapon = 0x0004,
        Armor = 0x0005,
        Linkshell = 0x0006,
        UsableItem = 0x0007,
        Crystal = 0x0008,
        Currency = 0x0009,
        Furnishing = 0x000A,
        Plant = 0x000B,
        Flowerpot = 0x000C,
        PuppetItem = 0x000D,
        Mannequin = 0x000E,
        Book = 0x000F,
        RacingForm = 0x0010,
        BettingSlip = 0x0011,
        SoulPlate = 0x0012,
        Reflector = 0x0013,
        ItemType20 = 0x0014,
        LotteryTicket = 0x0015,
        MazeTabula_M = 0x0016,
        MazeTabula_R = 0x0017,
        MazeVoucher = 0x0018,
        MazeRune = 0x0019,
        ItemType_26 = 0x001A,
        StorageSlip = 0x001B,
        LegionPass = 0x001C,
        MeebleBurrows = 0x001D,
        Instincts = 0x001E,
        CraftingKit = 0x001F,
        Max = CraftingKit,
    }

    public class FFXI_Item : IComparable
    {
        public uint Id { get; set; }
        public FFXI_ItemFlags Flags { get; set; }
        public FFXI_ItemType Type { get; set; }
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

        public enum FFXI_ItemDatFileTypes
        {
            Unknown,
            Armor,
            Currency,
            Item,
            PuppetItem,
            UsableItem,
            Weapon,
            ItemSlip,
            Instinct,
            Monipulator
        };

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
                                    newItem.Flags = (FFXI_ItemFlags)fieldValueUIntHex;
                                    break;
                                case "stack-size":
                                    newItem.StackSize = fieldValueUInt;
                                    break;
                                case "type":
                                    newItem.Type = (FFXI_ItemType)fieldValueUIntHex;
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

        static public void FFXI_LoadItemsFromDats(ref Dictionary<uint, FFXI_Item> itemList)
        {
            itemList.Clear();

            itemList.Concat(ReadItemListFromDatFile(73));     // General
            itemList.Concat(ReadItemListFromDatFile(55671));  // General 2
            itemList.Concat(ReadItemListFromDatFile(74));     // Useable
            itemList.Concat(ReadItemListFromDatFile(75));     // Weapons
            itemList.Concat(ReadItemListFromDatFile(76));     // Armor
            itemList.Concat(ReadItemListFromDatFile(55668));  // Armor 2
            itemList.Concat(ReadItemListFromDatFile(77));     // Puppet
            itemList.Concat(ReadItemListFromDatFile(91));     // Currency
            itemList.Concat(ReadItemListFromDatFile(55667));  // Vouchers
            //itemList.Concat(ReadItemListFromDatFile(55669));  // Monipulator
            itemList.Concat(ReadItemListFromDatFile(55670));  // Instincts
        }

        public static void FFXI_DeduceItemType(BinaryReader BR, out FFXI_ItemDatFileTypes ItemType)
        {
            ItemType = FFXI_ItemDatFileTypes.Unknown;
            byte[] FirstItem = null;
            long Position = BR.BaseStream.Position;
            BR.BaseStream.Position = 0;
            try
            {
                while (BR.BaseStream.Position != BR.BaseStream.Length)
                {
                    FirstItem = BR.ReadBytes(0x4);
                    BR.BaseStream.Position += (0xc00 - 0x4);
                    FFXIEncryption.Rotate(FirstItem, 5);
                    {
                        // Type -> Based on ID
                        uint ID = 0;
                        for (int i = 0; i < 4; ++i)
                        {
                            ID <<= 8;
                            ID += FirstItem[3 - i];
                        }
                        if (ID == 0xffff)
                        {
                            ItemType = FFXI_ItemDatFileTypes.Currency;
                        }
                        else if (ID < 0x1000)
                        {
                            ItemType = FFXI_ItemDatFileTypes.Item;
                        }
                        else if (ID < 0x2000)
                        {
                            ItemType = FFXI_ItemDatFileTypes.UsableItem;
                        }
                        else if (ID < 0x2200)
                        {
                            ItemType = FFXI_ItemDatFileTypes.PuppetItem;
                        }
                        else if (ID < 0x2800)
                        {
                            ItemType = FFXI_ItemDatFileTypes.Item;
                        }
                        else if (ID < 0x4000)
                        {
                            ItemType = FFXI_ItemDatFileTypes.Armor;
                        }
                        else if (ID < 0x5A00)
                        {
                            ItemType = FFXI_ItemDatFileTypes.Weapon;
                        }
                        else if (ID < 0x7000)
                        {
                            ItemType = FFXI_ItemDatFileTypes.Armor;
                        }
                        else if (ID < 0x7400)
                        {
                            ItemType = FFXI_ItemDatFileTypes.ItemSlip;
                        }
                        else if (ID < 0x7800)
                        {
                            ItemType = FFXI_ItemDatFileTypes.Instinct;
                        }
                        else if (ID < 0xF200)
                        {
                            ItemType = FFXI_ItemDatFileTypes.Monipulator;
                        }
                    }
                    if (ItemType != FFXI_ItemDatFileTypes.Unknown)
                    {
                        break;
                    }
                }
            }
            catch { }
            BR.BaseStream.Position = Position;
        }

        static public Dictionary<uint, FFXI_Item> ReadItemListFromDatFile(uint DatFileId)
        {
            Dictionary<uint, FFXI_Item> res = new Dictionary<uint, FFXI_Item>();

            if (!FFXI_FTable.TryGetValue(DatFileId, out var FileDatInfo))
                return res;

            if (!File.Exists(FileDatInfo.FullFileName))
                return res;

            var FS = new FileStream(FileDatInfo.FullFileName, FileMode.Open, FileAccess.Read);
            // needs to be in 3072 byte block size, and a minimum of 16 entries
            if (((FS.Length % 0x0C00) != 0) || (FS.Length < 0xC000))
                return res;
            var BR = new BinaryReader(FS);
            FFXI_DeduceItemType(BR, out var ExpectedItemType);

            var itemCount = BR.BaseStream.Length / 0x0C00;
            for (var i = 0; i < itemCount; i++)
            {
                BR.BaseStream.Seek(i * 0x0C00, SeekOrigin.Begin);
                byte[] ItemBytes = BR.ReadBytes(0xC00);
                FFXIEncryption.Rotate(ItemBytes, 5);
                var MemBR = new BinaryReader(new MemoryStream(ItemBytes, false));

                FFXI_Item item = new FFXI_Item();
                // Common Fields (14 bytes)
                item.Id = MemBR.ReadUInt32();
                item.Flags = (FFXI_ItemFlags)MemBR.ReadUInt16();
                item.StackSize = MemBR.ReadUInt16(); // 0xe0ff for Currency, which kinda suggests this is really 2 separate bytes
                item.Type = (FFXI_ItemType)MemBR.ReadUInt16();
                MemBR.ReadUInt16(); // item.ResourceID_ = MemBR.ReadUInt16();
                MemBR.ReadUInt16(); // item.ValidTargets_ = (ValidTarget)MemBR.ReadUInt16();
                // Extra Fields (22/30/10/6/2 bytes for Armor/Weapon/Puppet/Item/UsableItem)

                if (item.Type == FFXI_ItemType.Nothing)
                    continue;

                switch(ExpectedItemType)
                {
                    case FFXI_ItemDatFileTypes.Armor:
                    case FFXI_ItemDatFileTypes.Weapon:
                        MemBR.ReadUInt16(); // item.Level_ = MemBR.ReadUInt16();
                        MemBR.ReadUInt16(); // item.Slots_ = (EquipmentSlot)MemBR.ReadUInt16();
                        MemBR.ReadUInt16(); // item.Races_ = (Race)MemBR.ReadUInt16();
                        MemBR.ReadUInt32(); // item.Jobs_ = (Job)MemBR.ReadUInt32();
                        MemBR.ReadUInt16(); //item.SuperiorLevel_ = MemBR.ReadUInt16();
                        if (ExpectedItemType == FFXI_ItemDatFileTypes.Armor)
                        {
                            MemBR.ReadUInt16(); // item.ShieldSize_ = MemBR.ReadUInt16();
                        }
                        else
                        {
                            // Weapon
                            MemBR.ReadUInt16(); // item.Unknown4_ = MemBR.ReadUInt16();
                            MemBR.ReadUInt16(); // item.Damage_ = MemBR.ReadUInt16();
                            MemBR.ReadInt16(); // item.Delay_ = MemBR.ReadInt16();
                            MemBR.ReadUInt16(); // item.DPS_ = MemBR.ReadUInt16();
                            MemBR.ReadByte(); // item.Skill_ = (Skill)MemBR.ReadByte();
                            MemBR.ReadByte(); // item.JugSize_ = MemBR.ReadByte();
                            MemBR.ReadUInt32(); // item.Unknown1_ = MemBR.ReadUInt32();
                        }
                        MemBR.ReadByte(); // item.MaxCharges_ = MemBR.ReadByte();
                        MemBR.ReadByte(); // item.CastingTime_ = MemBR.ReadByte();
                        MemBR.ReadUInt16(); // item.UseDelay_ = MemBR.ReadUInt16();
                        MemBR.ReadUInt32(); // item.ReuseDelay_ = MemBR.ReadUInt32();
                        MemBR.ReadUInt16(); // item.Unknown2_ = MemBR.ReadUInt16();
                        MemBR.ReadUInt16(); // item.iLevel_ = MemBR.ReadUInt16();
                        MemBR.ReadUInt32(); // item.Unknown3_ = MemBR.ReadUInt32();
                        break;

                    case FFXI_ItemDatFileTypes.PuppetItem:
                        MemBR.ReadUInt16(); // item.PuppetSlot_ = (PuppetSlot)MemBR.ReadUInt16();
                        MemBR.ReadUInt32(); // item.ElementCharge_ = MemBR.ReadUInt32();
                        MemBR.ReadUInt32(); // item.Unknown3_ = MemBR.ReadUInt32();
                        break;

                    case FFXI_ItemDatFileTypes.Instinct:
                        MemBR.ReadUInt32();
                        MemBR.ReadUInt32();
                        MemBR.ReadUInt16();
                        MemBR.ReadUInt16(); // item.InstinctCost_ = MemBR.ReadUInt16();
                        MemBR.ReadUInt16();
                        MemBR.ReadUInt32();
                        MemBR.ReadUInt32();
                        MemBR.ReadUInt32();
                        break;

                    case FFXI_ItemDatFileTypes.Item:
                        switch (item.Type)
                        {
                            case FFXI_ItemType.Flowerpot:
                            case FFXI_ItemType.Furnishing:
                            case FFXI_ItemType.Mannequin:
                                MemBR.ReadUInt16(); // item.Element_ = (Element)MemBR.ReadUInt16();
                                MemBR.ReadInt32(); // item.StorageSlots_ = MemBR.ReadInt32();
                                MemBR.ReadUInt32(); // item.Unknown3_ = MemBR.ReadUInt32();
                                break;
                            default:
                                MemBR.ReadUInt16(); // item.Unknown2_ = MemBR.ReadUInt16();
                                MemBR.ReadUInt32(); // item.Unknown3_ = MemBR.ReadUInt32();
                                MemBR.ReadUInt32(); // item.Unknown3_ = MemBR.ReadUInt32();
                                break;
                        }
                        break;

                    case FFXI_ItemDatFileTypes.UsableItem:
                        MemBR.ReadUInt16(); // item.ActivationTime_ = MemBR.ReadUInt16();
                        MemBR.ReadUInt32(); // item.Unknown1_ = MemBR.ReadUInt32();
                        MemBR.ReadUInt32(); // item.Unknown3_ = MemBR.ReadUInt32();
                        MemBR.ReadUInt32(); // item.Unknown4_ = MemBR.ReadUInt32();
                        break;

                    case FFXI_ItemDatFileTypes.Currency:
                        MemBR.ReadUInt16(); // item.Unknown2_ = MemBR.ReadUInt16();
                        break;

                    case FFXI_ItemDatFileTypes.ItemSlip:
                        MemBR.ReadUInt16(); // item.Unknown1_ = MemBR.ReadUInt16();
                        for (int counter = 0; counter < 17; counter++)
                        {
                            MemBR.ReadUInt32();
                        }
                        break;
                    case FFXI_ItemDatFileTypes.Monipulator:
                        MemBR.ReadUInt16(); // item.Unknown1_ = MemBR.ReadUInt16();
                        for (int counter = 0; counter < 24; counter++)
                        {
                            MemBR.ReadInt32();
                        }
                        break;
                    default:
                        // If this is a unknown expected file type, ignore and exit
                        return res;
                }

                if (item.Type > FFXI_ItemType.Max)
                {
                    // Invalid item type ?
                    continue;
                }

                // Next Up: Strings (variable size)
                long StringBase = MemBR.BaseStream.Position;
                uint StringCount = MemBR.ReadUInt32();
                if (StringCount > 9)
                {
                    // Sanity check, for safety - 0 strings is fine for now
                    // item.Clear();
                    continue;
                }
                FFXIEncoding E = new FFXIEncoding();
                string[] Strings = new string[StringCount];
                for (byte iStrings = 0; iStrings < StringCount; iStrings++)
                {
                    long Offset = StringBase + MemBR.ReadUInt32();
                    uint Flag = MemBR.ReadUInt32();
                    if (Offset < 0 || Offset + 0x20 > 0x280 || (Flag != 0 && Flag != 1))
                    {
                        // item.Clear();
                        StringCount = 0;
                        break;
                    }
                    // Flag seems to be 1 if the offset is not actually an offset. Could just be padding to make StringCount unique per language, or it could be an indication
                    // of the pronoun to use (a/an/the/...). The latter makes sense because of the increased number of such flags for french and german.
                    if (Flag == 0)
                    {
                        MemBR.BaseStream.Position = Offset;
                        Strings[iStrings] = FFXI_ReadString(MemBR, E);
                        if (Strings[iStrings] == null)
                        {
                            // item.Clear();
                            StringCount = 0;
                            break;
                        }
                        BR.BaseStream.Position = StringBase + 4 + 8 * (i + 1);
                    }
                }
                // Assign the strings to the proper fields
                switch (StringCount)
                {
                    case 0:
                        // Temporary hack until I can figure out what the hell is wrong with reading the strings
                        if ((Strings.Length > 1) && (Strings[0] != null))
                            item.Name = Strings[0];
                        else
                            item.Name = "<no name found>";
                        // item.Name = item.Type.ToString() + " - " + item.Id.ToString();
                        break;
                    case 1:
                        item.Name = Strings[0];
                        break;
                    case 2: // Japanese
                        item.Name = Strings[0];
                        item.Description = Strings[1];
                        break;
                    case 5: // English
                        item.Name = Strings[0];
                        // unused:              Strings[1]
                        item.NameSingle = Strings[2];
                        item.NameMultiple = Strings[3];
                        item.Description = Strings[4];
                        break;
                    case 6: // French
                        item.Name = Strings[0];
                        // unused:              Strings[1]
                        // unused:              Strings[2]
                        item.NameSingle = Strings[3];
                        item.NameMultiple = Strings[4];
                        item.Description = Strings[5];
                        break;
                    case 9: // German
                        item.Name = Strings[0];
                        // unused:              Strings[1]
                        // unused:              Strings[2]
                        // unused:              Strings[3]
                        item.NameSingle = Strings[4];
                        // unused:              Strings[5]
                        // unused:              Strings[6]
                        item.NameMultiple = Strings[7];
                        item.Description = Strings[8];
                        break;
                }
                MemBR.Close();

                res.Add(item.Id, item);
            }
            return res;
        }

        static private string FFXI_ReadString(BinaryReader ItemMemoryBR, FFXIEncoding E)
        {
            // Read past "padding"
            if (ItemMemoryBR.ReadUInt32() != 1)
            {
                return null;
            }
            for (byte i = 0; i < 6; ++i)
            {
                if (ItemMemoryBR.ReadUInt32() != 0)
                {
                    return null;
                }
            }
            List<byte> TextBytes = new List<byte>();
            while (ItemMemoryBR.BaseStream.Position < 0x280)
            {
                byte[] Next4 = ItemMemoryBR.ReadBytes(4);
                byte UsableBytes = (byte)Next4.Length;
                while (UsableBytes > 0 && Next4[UsableBytes - 1] == 0)
                {
                    --UsableBytes;
                }
                if (UsableBytes != 4)
                {
                    byte i = 0;
                    while (UsableBytes-- > 0)
                    {
                        TextBytes.Add(Next4[i++]);
                    }
                    // return E.GetString(TextBytes.ToArray()).Replace("\n", Environment.NewLine);
                    return FFXIEncoding.UTF8.GetString(TextBytes.ToArray()).Replace("\n", Environment.NewLine);
                }
                else
                {
                    TextBytes.AddRange(Next4);
                }
            }
            return null;
        }

    }
}
