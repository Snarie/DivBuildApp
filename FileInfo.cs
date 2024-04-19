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
using DivBuildApp.CsvReadFormats;

namespace DivBuildApp
{
    public static class FileInfo
    {
        public static Dictionary<string, List<EquipBonus>> ReadBrandSets()
        {
            string filePath = "BrandBonusses.txt";
            string[] brandSetSheet = File.ReadAllLines(filePath);

            Dictionary<string, List<EquipBonus>> brandSets = new Dictionary<string, List<EquipBonus>>();

            foreach (string brandSet in brandSetSheet)
            {
                string[] brandInfo = brandSet.Split(':'); //[0]=Name, [1]=Values
                if (brandInfo.Length != 2)
                {
                    if (brandInfo.Length > 2) Console.WriteLine($"Error: Invalid brand info format, contains to many ':' in line '{brandInfo[0]}'");
                    else Console.WriteLine($"Error: Invalid brand info format, contains to little ':' in line '{brandInfo[0]}'");
                    continue; //Skip brand if format is wrong 
                }
                if (string.IsNullOrEmpty(brandInfo[1]))
                {
                    Console.WriteLine($"Note: Field was left empty for '{brandInfo[0]}'");
                    continue; //No characters were added after the ':', probably on purpose.
                }
                string[] bonusses = brandInfo[1].Split(',');

                List<EquipBonus> bonusValues = new List<EquipBonus>();
                foreach (string bonus in bonusses)
                {
                    string[] bonusParts = bonus.Split('=');
                    if (bonusParts.Length != 3)
                    {
                        Console.WriteLine($"Error: Incorrect bonus format (x=y=z) in {bonus} from '{brandInfo[0]}'");
                        continue; //Skip bonus if not enough '=' are present to split.
                    }

                    bool successIndex = int.TryParse(bonusParts[0], out int bonusIndex);
                    if (!successIndex)
                    {
                        Console.WriteLine($"Error: couldn't parse {bonusParts[0]} to int in {bonus} from '{brandInfo[0]}'");
                        continue; //Skip bonus if parsing bonusIndex failed.
                    }

                    bool successValue = double.TryParse(bonusParts[2], out double bonusValue);
                    if (!successValue)
                    {
                        Console.WriteLine($"Error: couldn't parse {bonusParts[1]} to double in {bonus} from '{brandInfo[0]}");
                        continue; //Skip bonus if parsing bonusValue failed.
                    }
                    bool successName = Enum.TryParse(bonusParts[1], true, out BonusType bonusName);
                    if (!successName)
                    {
                        Console.WriteLine($"Error: {bonusParts[1]} isn't a valid bonus type from {brandInfo[0]}");
                    }
                    bonusValues.Add(new EquipBonus(bonusIndex, new Bonus(bonusName, bonusValue)));
                }
                if (bonusValues.Count < 1)
                {
                    Console.WriteLine($"Warn: No valid bonusses found for '{brandInfo[0]}'");
                    //Most likely a mistake, but doesn't cause problems.
                }
                if (brandSets.ContainsKey(brandInfo[0]))
                {
                    Console.WriteLine($"Error: Duplicate brand name '{brandInfo[0]}' found. Skipping duplicate entry.");
                    continue; //Skip if a brand with the same name already exists.
                }
                brandSets.Add(brandInfo[0], bonusValues);
            }
            return brandSets;
        }

        
        

        public static List<StringItem> CsvItemDefaults()
        {
            string filePath = "data/ItemDefault.csv";
            List<StringItem> expandedItems = new List<StringItem>();

            List<StringItem> tempItems = ReadCsv.ReadCsvFile<StringItem>(filePath, ReadCsv.Config());

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
