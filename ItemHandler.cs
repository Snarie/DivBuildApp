using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DivBuildApp
{
    internal static class ItemHandler
    {

        public static List<StringItem> AllItemList = new List<StringItem>();

        public static void Initialize()
        {
            SetAllItemList(CsvReader.ItemDefault());
        }

        public static ItemType[] GearTypes = { ItemType.Mask, ItemType.Backpack, ItemType.Chest, ItemType.Gloves, ItemType.Holster, ItemType.Kneepads};
        public static void SetAllItemList(List<StringItem> tempItems)
        {
            List<StringItem> expandedItems = new List<StringItem>();

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
                else if (item.Slot.Equals("nontalent", StringComparison.OrdinalIgnoreCase))
                {
                    var slots = new List<string> { "mask", "gloves", "kneepads", "holster" };
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
                else
                {
                    expandedItems.Add(item);
                }
            }
            AllItemList = ConvertItems(expandedItems);
            
        }

        private static List<StringItem> ConvertItems(List<StringItem> items)
        {
            List<StringItem> formattedItems = new List<StringItem>();
            foreach (StringItem item in items)
            {
                string core = string.IsNullOrEmpty(item.CoreAttribute) ? "list:core" : item.CoreAttribute;
                string side1 = string.IsNullOrEmpty(item.SideAttribute1) ? "list:side" : item.SideAttribute1;
                string side2 = string.IsNullOrEmpty(item.SideAttribute2) ? "list:side" : item.SideAttribute2;
                string side3 = string.IsNullOrEmpty(item.SideAttribute3) ? "none" : item.SideAttribute3;
                if (side3 == "none" && (item.Slot == "mask" || item.Slot == "backpack" || item.Slot == "chest"))
                {
                    side3 = "list:mod";
                }

                string talent = string.IsNullOrEmpty(item.Talent) ? "none" : item.Talent;
                if (talent == "none" && (item.Slot == "backpack" || item.Slot == "chest"))
                {
                    if (item.Slot == "backpack")
                    {
                        talent = "list:backpack_talent";
                    }
                    if (item.Slot == "chest")
                    {
                        talent = "list:chest_talent";
                    }
                }

                formattedItems.Add(new StringItem(item.Name, item.BrandName, item.Slot, item.Rarity, core, side1, side2, side3, talent));
            }
            return formattedItems;
        }

        public static StringItem ItemFromIdentity(string name, string slot)
        {
            return AllItemList.FirstOrDefault(i => i.Name == name && i.Slot == slot);
        }
        public static StringItem ItemFromName(string name)
        {
            return AllItemList.FirstOrDefault(i => i.Name == name);
        }
        public static string BrandFromName(string name)
        {
            return AllItemList.FirstOrDefault(i => i.Name == name).BrandName;
        }
    }
}
