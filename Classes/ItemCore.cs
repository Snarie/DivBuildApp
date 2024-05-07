using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DivBuildApp
{
    public enum ItemType
    {
        Mask,
        Backpack,
        Chest,
        Gloves,
        Holster,
        Kneepads
    }

    public class StringItem
    {
        public string Name { get; set; }
        public string BrandName { get; set; }
        public string Slot { get; set; }
        public string Rarity { get; set; }
        public string CoreAttribute { get; set; }
        public string SideAttribute1 { get; set; }
        public string SideAttribute2 { get;set; }
        public string SideAttribute3 { get; set; }
        public string Talent {  get; set; }

        public StringItem(string name, string brandName, string slot, string rarity, string coreAttribute, string sideAttribute1, string sideAttribute2, string sideAttribute3, string talent) 
        {
            Name = name; 
            BrandName = brandName; 
            Slot = slot; 
            Rarity = rarity; 
            CoreAttribute = coreAttribute;
            SideAttribute1 = sideAttribute1;
            SideAttribute2 = sideAttribute2;
            SideAttribute3 = sideAttribute3;
            Talent = talent;
        }
    }

    public class Gear
    {
        public string Name { get; set; }
        public string BrandName { get; set; }
        public ItemType Slot { get; set; }
        public string Rarity { get; set; }
        public Bonus[] StatAttributes { get; set; }
        public string Talent { get; set; }

        public Gear(string name, string brandName, ItemType slot, string rarity, Bonus[] statAttributes, string talent)
        {
            Name = name;
            BrandName = brandName;
            Slot = slot;
            Rarity = rarity;
            StatAttributes = statAttributes;
            Talent = talent;
        }
    }

}
