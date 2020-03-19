using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Linq;
using System.Xml.Linq;

namespace PacketViewerLogViewer.POLUtils
{
/*
<?xml version="1.0" encoding="utf-8"?>
<thing-list>
    <thing type="Item">
    <field name="id">0</field>
    <field name="flags">F040</field>
    <field name="stack-size">1</field>
    <field name="type">0001</field>
    <field name="resource-id">65000</field>
    <field name="valid-targets">0000</field>
    <field name="name">.</field>
    <field name="description">.</field>
    <field name="log-name-singular">.</field>
    <field name="log-name-plural">.</field>
    <field name="icon">
        <thing type="Graphic">
        <field name="format">8-bit Bitmap</field>
        <field name="flag">145</field>
        <field name="category">icon</field>
        <field name="id">noimage</field>
        <field name="width">32</field>
        <field name="height">32</field>
        <field name="planes">1</field>
        <field name="bits">8</field>
        <field name="compression">0</field>
        <field name="size">0</field>
        <field name="horizontal-resolution">0</field>
        <field name="vertical-resolution">0</field>
        <field name="used-colors">0</field>
        <field name="important-colors">32</field>
        <field name="image" format="image/png" encoding="base64">...</field>
        </thing>
    </field>
    <field name="unknown-2">65535</field>
    <field name="unknown-3">0</field>
    </thing>
    <thing type="Item">
        <field name="id">1</field>
        <field name="flags">6054</field>
        <field name="stack-size">1</field>
        <field name="type">000A</field>
        <field name="resource-id">65000</field>
        <field name="valid-targets">0000</field>
        <field name="name">Chocobo Bedding</field>
        <field name="description">Furnishing:
        Extremely popular among San d'Orian
        children, this hay is so soft that it is
        used in mattresses across the kingdom.</field>
        <field name="log-name-singular">pile of chocobo bedding</field>
        <field name="log-name-plural">piles of chocobo bedding</field>
        <field name="element">07</field>
        <field name="storage-slots">1</field>
        <field name="icon">
            <thing type="Graphic">
            <field name="format">8-bit Bitmap</field>
            <field name="flag">145</field>
            <field name="category">recepi</field>
            <field name="id">0001</field>
            <field name="width">32</field>
            <field name="height">32</field>
            <field name="planes">1</field>
            <field name="bits">8</field>
            <field name="compression">0</field>
            <field name="size">0</field>
            <field name="horizontal-resolution">0</field>
            <field name="vertical-resolution">0</field>
            <field name="used-colors">0</field>
            <field name="important-colors">32</field>
            <field name="image" format="image/png" encoding="base64">...</field>
            </thing>
        </field>
        <field name="unknown-3">0</field>
        </thing>

*/
    public class Item : IComparable
    {
        public int Id { get; set; }
        //public int Flags { get; set; }
        public int StackSize { get; set; }
        public string Name { get; set; }
        //public string Type { get; set; }

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;
            return Id.CompareTo((obj as Item).Id);
        }
    }

    public class POLUtilsHelper
	{
        static public List<Item> ReadItemListFromXML(string ItemXmlFile)
        {
            List<Item> res = new List<Item>();

            XmlDocument D = new XmlDocument();
            D.Load(ItemXmlFile);

            foreach (XmlNode itemNode in D.DocumentElement.ChildNodes)
            {
                if (itemNode.Name == "thing")
                {
                    var newItem = new Item();
                    foreach (XmlNode itemElement in itemNode.ChildNodes)
                    {
                        if ((itemElement.Name == "field") && (itemElement.Attributes["name"].Value == "id"))
                        {
                            newItem.Id = int.Parse(itemElement.InnerText);
                        }
                        else
                        if ((itemElement.Name == "field") && (itemElement.Attributes["name"].Value == "name"))
                        {
                            newItem.Name = itemElement.InnerText;
                        }
                        else
                        if ((itemElement.Name == "field") && (itemElement.Attributes["name"].Value == "stack-size"))
                        {
                            newItem.StackSize = int.Parse(itemElement.InnerText);
                        }
                    }
                    // List all the childs

                    if (newItem.Id > 0)
                    {
                        res.Add(newItem);
                    }
                }
            }

            return res;
        }

        public POLUtilsHelper()
		{
		}
	}
}
