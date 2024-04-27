using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;
using System.Windows;
using System.Security.Policy;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System.Runtime.CompilerServices;
using DivBuildApp.CsvFormats;

namespace DivBuildApp
{
    internal static class FileInfo
    {
        public static Dictionary<string , List<EquipBonus>> CsvBrandBonus()
        {
            List<BrandBonusesFormat> brandBonusesList = CsvReader.BrandBonuses();

            Dictionary<string, List<EquipBonus>> brandSets = new Dictionary<string, List<EquipBonus>>();

            foreach (BrandBonusesFormat brandBonuses in brandBonusesList)
            {
                if (brandSets.ContainsKey(brandBonuses.Name))
                {
                    Console.WriteLine($"Error: Duplicate brand name '{brandBonuses.Name}' found. Skipping duplicate entry.");
                    continue; //Skip if a brand with the same name already exists.
                }
                brandSets.Add(brandBonuses.Name, GetBrandBonuses(brandBonuses));
            }
            return brandSets;
        }
        public static List<EquipBonus> GetBrandBonuses(BrandBonusesFormat brandBonuses)
        {
            string[] slots = { brandBonuses.Slot1, brandBonuses.Slot2, brandBonuses.Slot3 };
            List<EquipBonus> equipBonuses = new List<EquipBonus>();

            for (int i = 0; i < 3; i++)
            {
                foreach (string bonusString in slots[i].Split('+'))
                {
                    string[] bonusParts = bonusString.Split('=');
                    if (bonusParts.Length != 2) continue;
                    bool success = BonusHandler.TryCreateBonus(bonusParts[0], bonusParts[1], out Bonus bonus);
                    if (!success) continue;
                    equipBonuses.Add(new EquipBonus(i + 1, bonus));
                }
            }
            return equipBonuses;

        }
        
        public static List<StringItem> CsvItemDefaults()
        {
            List<StringItem> expandedItems = new List<StringItem>();

            List<StringItem> tempItems = CsvReader.ItemDefault();

            foreach (var item in tempItems)
            {
                if (item.Slot.Equals("all", StringComparison.OrdinalIgnoreCase))
                {
                    var slots = new List<string> { "mask", "backpack", "chest", "gloves", "kneepads", "holster" };
                    foreach (var slot in slots)
                    {
                        var newItem = new StringItem
                        (
                            item.Name,
                            item.BrandName,
                            slot,
                            item.Rarity,
                            item.CoreAttribute,
                            item.SideAttribute1,
                            item.SideAttribute2,
                            item.SideAttribute3,
                            item.Talent
                        );
                        expandedItems.Add(newItem);
                    }
                }
                else if(item.Slot.Equals("nontalent", StringComparison.OrdinalIgnoreCase))
                {
                    var slots = new List<string> { "mask", "gloves", "kneepads", "holster" };
                    foreach(var slot in slots)
                    {
                        var newItem = new StringItem
                        (
                            item.Name,
                            item.BrandName,
                            slot,
                            item.Rarity,
                            item.CoreAttribute,
                            item.SideAttribute1,
                            item.SideAttribute2,
                            item.SideAttribute3,
                            item.Talent
                        );
                        expandedItems.Add(newItem);
                    }
                }
                else
                {
                    expandedItems.Add(item);
                }
            }
            return expandedItems;
        }

        public static List<StringItem> ConvertItem(List<StringItem> items)
        {
            List<StringItem> formattedItems = new List<StringItem>();
            foreach(StringItem item in items)
            {
                string core = string.IsNullOrEmpty(item.CoreAttribute) ? "list:core" : item.CoreAttribute;
                string side1 = string.IsNullOrEmpty(item.SideAttribute1) ? "list:side" : item.SideAttribute1;
                string side2 = string.IsNullOrEmpty(item.SideAttribute2) ? "list:side" : item.SideAttribute2;
                string side3 = string.IsNullOrEmpty(item.SideAttribute3) ? "none" : item.SideAttribute3;
                if(side3 == "none" && (item.Slot == "mask" || item.Slot == "backpack" || item.Slot == "chest"))
                {
                    side3 = "list:mod";
                }

                string talent = string.IsNullOrEmpty(item.Talent) ? "none" : item.Talent;
                if (talent == "none" && (item.Slot == "backpack" || item.Slot == "chest"))
                {
                    if(item.Slot == "backpack")
                    {
                        talent = "list:backpack_talent";
                    }
                    if(item.Slot == "chest")
                    {
                        talent = "list:chest_talent";
                    }
                }
                
                formattedItems.Add(new StringItem(item.Name, item.BrandName, item.Slot, item.Rarity, core, side1, side2, side3, talent));
            }
            return formattedItems;
        }


    }
}
