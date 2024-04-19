using DivBuildApp.CsvReadFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml.Linq;

namespace DivBuildApp
{
    public static class ListOptions
    {

        private static List<Bonus> gearCoreAttributes = new List<Bonus>();
        private static List<Bonus> gearSideAttributes = new List<Bonus>();
        private static List<Bonus> gearModAttributes = new List<Bonus>();
        private static List<Bonus> weaponCoreAttributes = new List<Bonus>();
        private static List<Bonus> weaponPrimaryAttributes = new List<Bonus>();
        private static List<Bonus> weaponSecondaryAttributes = new List<Bonus>();
        
        public static void CreateBonusCapsFromData(List<BonusCapsFormat> bonusCapsFormat)
        {
            foreach(BonusCapsFormat bonusCaps in bonusCapsFormat)
            {
                BonusType name = BonusHandler.StringToBonusType(bonusCaps.Name);
                if (name == BonusType.NoBonus) continue;
                TryCreateBonusCap(name, bonusCaps.GearCore, gearCoreAttributes);
                TryCreateBonusCap(name, bonusCaps.GearSide, gearSideAttributes);
                TryCreateBonusCap(name, bonusCaps.Mod, gearModAttributes);
                TryCreateBonusCap(name, bonusCaps.WeaponCore, weaponCoreAttributes);
                TryCreateBonusCap(name, bonusCaps.WeaponPrimary, weaponPrimaryAttributes);
                TryCreateBonusCap(name, bonusCaps.WeaponSide, weaponSecondaryAttributes);
            }
        }
        public static void TryCreateBonusCap(BonusType name, string stringValue, List<Bonus> holder)
        {
            if (string.IsNullOrEmpty(stringValue)) return;
            bool canParse = double.TryParse(stringValue, out double value);
            if (!canParse) return;
            Bonus bonus = new Bonus(name, value);
            holder.Add(bonus);
            
        }
        public static void CreateAttributesFromDictionary(Dictionary<BonusType, Dictionary<string, double>> attributes)
        {
            foreach(var attribute in attributes)
            {
                foreach(var capType in attribute.Value)
                {
                    CreateAttributeio(capType.Key, new Bonus(attribute.Key, capType.Value));
                }
            }
        }
        public static void CreateAttributeio(string type, Bonus bonus)
        {
            switch (type)
            {
                case "GC":
                    gearCoreAttributes.Add(bonus);
                    break;
                case "GP":
                    gearSideAttributes.Add(bonus);
                    break;
                case "M":
                    gearModAttributes.Add(bonus);
                    break;
                case "WC":
                    weaponCoreAttributes.Add(bonus);
                    break;
                case "WP":
                    weaponPrimaryAttributes.Add(bonus);
                    break;
                case "WS":
                    weaponSecondaryAttributes.Add(bonus);
                    break;
                default:
                    Console.WriteLine($"Warn: {type} isn't a valid prefix");
                    break;
            }
        }
        public static void SetItemsSource(ComboBox statBox, string itemAttribute, List<Bonus> defaultList)
        {
            int bonusIndex = statBox.SelectedIndex;
            int bonusItemCount = statBox.Items.Count;
            statBox.ItemsSource = GetBonusOptionsList(itemAttribute, defaultList);
            SetBoxSelectedIndex(statBox, bonusIndex, bonusItemCount);
        }

        public static void SetOptionsStatBoxes(ItemType itemType)
        {
            ComboBox[] statBoxes = Lib.GetSideStatBoxes(itemType);

            if (statBoxes[0] != null && statBoxes[1] != null && statBoxes[2] != null)
            {
                OptionsCoreStat(itemType);
                OptionsSideStats(itemType);
            }
        }
        public static void OptionsCoreStat(ItemType itemType)
        {
            ComboBox itemBox = Lib.GetItemBox(itemType);
            if (itemBox.SelectedItem is ComboBoxBrandItem selectedItem)
            {
                ComboBox statBox = Lib.GetCoreStatBox(itemType);
                StringItem stringItem = ItemHandler.ItemFromIdentity(selectedItem.Name, selectedItem.Slot);
                SetItemsSource(statBox, stringItem.CoreAttribute, gearCoreAttributes);
            }
        }

        public static void OptionsSideStats(ItemType itemType)
        {
            ComboBox itemBox = Lib.GetItemBox(itemType);
            if (itemBox.SelectedItem is ComboBoxBrandItem selectedItem)
            {
                ComboBox[] statBoxes = Lib.GetSideStatBoxes(itemType);
                StringItem stringItem = ItemHandler.ItemFromIdentity(selectedItem.Name, selectedItem.Slot);

                SetItemsSource(statBoxes[0], stringItem.SideAttribute1, gearSideAttributes);
                SetItemsSource(statBoxes[1], stringItem.SideAttribute2, gearSideAttributes);
                SetItemsSource(statBoxes[2], stringItem.SideAttribute3, gearSideAttributes);
            }
        }

        private static void SetBoxSelectedIndex(ComboBox bonusBox, int oldIndex, int oldItemCount)
        {
            //TODO: add a saver if it switches away from a list (to go back to the saved index when you go back to lists)
            if (bonusBox.Items.Count > 0)
            {
                if (bonusBox.Items.Count == 1)
                {
                    bonusBox.Foreground = Brushes.Yellow;
                    bonusBox.IsEnabled = false;
                    bonusBox.SelectedIndex = 0;
                }
                else
                {
                    bonusBox.Foreground = Brushes.Cornsilk;
                    bonusBox.IsEnabled = true;
                    if (oldItemCount == 1)
                    {
                        bonusBox.SelectedIndex = -1;
                    }
                    else
                    {
                        bonusBox.SelectedIndex = oldIndex;
                    }
                }
                
            }
            else
            {
                bonusBox.IsEnabled = false;
            }
        }



        /// <summary>
        /// Returns the list of all Bonusses available to be selected
        /// </summary>
        /// <param name="item">Selected item</param>
        /// <param name="search">Paramater that is checked for presets</param>
        /// <param name="defaultList">List of bonusses when no overrides are done</param>
        /// <returns></returns>
        public static List<Bonus> GetBonusOptionsList(string search, List<Bonus> defaultList)
        {
            string[] parts = search.Split(':');
            if(parts.Length == 1)
            {
                if (search == "none")
                {
                    return new List<Bonus>();
                }
                else
                {
                    Console.WriteLine($"ListOptions.GetBonusOptionsList \"{search}\" invalid format :(");
                    return new List<Bonus>();
                }
            }
            //Omdat leesbaar :)
            (string group, string value) = (parts[0], parts[1]);
            switch (group)
            {
                case "list":
                    switch (value)
                    {
                        case "core":
                            return gearCoreAttributes;
                        case "side":
                            return gearSideAttributes;
                        case "mod":
                            return gearModAttributes;
                        default:
                            Console.WriteLine($"ListOptions.GetBonusOptionsList \"{value}\" is not a recognized list");
                            return new List<Bonus>();
                    }
                case "locked":
                    return new List<Bonus>() { BonusHandler.BonusFromString(value) };
                case "core":
                    return new List<Bonus>() { BonusHandler.BonusFromList(gearCoreAttributes, BonusHandler.StringToBonusType(value)) };
                case "side":
                    return new List<Bonus>() { BonusHandler.BonusFromList(gearSideAttributes, BonusHandler.StringToBonusType(value)) };
                case "mod":
                    return new List<Bonus>() { BonusHandler.BonusFromList(gearModAttributes, BonusHandler.StringToBonusType(value)) };
                default:
                    Console.WriteLine($"ListOptions.GetBonusOptionsList: \"{search}\" is not a recognized preset");
                    return new List<Bonus>();
            }
        }

        public static void OptionsGearBox()
        {
            foreach(StringItem item in ItemHandler.AllItemList)
            {
                bool success = Enum.TryParse(item.Slot, true, out ItemType itemType);
                if (!success) 
                {
                    Console.WriteLine($"{item.Slot} cant be converted to itemType"); continue; 
                }
                ComboBox bonker = Lib.GetItemBox(itemType);
                bonker.Items.Add(new ComboBoxBrandItem { Name = item.Name, Preset = $"[{item.Rarity}]", Slot = item.Slot });
            }
            
        }
    }
}
