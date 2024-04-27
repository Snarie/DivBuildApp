using DivBuildApp.CsvFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Linq;

namespace DivBuildApp
{
    public static class ListOptions
    {

        private static List<BonusDisplay> gearCoreAttributes = new List<BonusDisplay>();
        private static List<BonusDisplay> gearSideAttributes = new List<BonusDisplay>();
        private static List<BonusDisplay> gearModAttributes = new List<BonusDisplay>();
        private static List<BonusDisplay> weaponCoreAttributes = new List<BonusDisplay>();
        private static List<BonusDisplay> weaponPrimaryAttributes = new List<BonusDisplay>();
        private static List<BonusDisplay> weaponSecondaryAttributes = new List<BonusDisplay>();
        
        public static List<BonusCapsFormat> bonusCapsList = new List<BonusCapsFormat>();
        public static void CreateBonusCapsFromData(List<BonusCapsFormat> bonusCapsFormat)
        {
            bonusCapsList = bonusCapsFormat;
            foreach(BonusCapsFormat bonusCaps in bonusCapsFormat)
            {

                BonusType name = BonusHandler.StringToBonusType(bonusCaps.Name);

                if (name == BonusType.NoBonus) continue;
                BonusDisplayHandler.bonusIconType.Add(name, bonusCaps.IconType);
                TryCreateBonusCap(name, bonusCaps.GearCore, "Core-"+bonusCaps.IconType, gearCoreAttributes);
                TryCreateBonusCap(name, bonusCaps.GearSide, "Side-"+bonusCaps.IconType, gearSideAttributes);
                TryCreateBonusCap(name, bonusCaps.Mod, "Mod-"+bonusCaps.IconType, gearModAttributes);
                TryCreateBonusCap(name, bonusCaps.WeaponCore, bonusCaps.IconType, weaponCoreAttributes);
                TryCreateBonusCap(name, bonusCaps.WeaponPrimary, bonusCaps.IconType, weaponPrimaryAttributes);
                TryCreateBonusCap(name, bonusCaps.WeaponSide, bonusCaps.IconType, weaponSecondaryAttributes);

            }
        }
        public static void TryCreateBonusCap(BonusType name, string stringValue, string iconType, List<BonusDisplay> holder)
        {
            if (string.IsNullOrEmpty(stringValue)) return;
            bool canParse = double.TryParse(stringValue, out double value);
            if (!canParse) return;
            BonusDisplay bonus = new BonusDisplay(new Bonus(name, value), iconType);
            holder.Add(bonus);
            
        }
        
        
        public static void SetItemsSource(ComboBox statBox, string itemAttribute)
        {
            int bonusIndex = statBox.SelectedIndex;
            int bonusItemCount = statBox.Items.Count;
            statBox.ItemsSource = GetBonusOptionsList(itemAttribute);
            SetBoxSelectedIndex(statBox, bonusIndex, bonusItemCount);
        }


        public static void DisplayOptionStatBoxIcons(ItemType itemType)
        {

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
                SetItemsSource(statBox, stringItem.CoreAttribute);
            }
        }

        public static void OptionsSideStats(ItemType itemType)
        {
            ComboBox itemBox = Lib.GetItemBox(itemType);
            if (itemBox.SelectedItem is ComboBoxBrandItem selectedItem)
            {
                ComboBox[] statBoxes = Lib.GetSideStatBoxes(itemType);
                StringItem stringItem = ItemHandler.ItemFromIdentity(selectedItem.Name, selectedItem.Slot);

                SetItemsSource(statBoxes[0], stringItem.SideAttribute1);
                SetItemsSource(statBoxes[1], stringItem.SideAttribute2);
                SetItemsSource(statBoxes[2], stringItem.SideAttribute3);
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
                if(bonusBox.SelectedIndex == -1)
                {
                    Slider slider = Lib.FindSiblingControl<Slider>(bonusBox, bonusBox.Name + "_Slider");
                    slider.Visibility = System.Windows.Visibility.Hidden;
                }
            }
            else
            {
                Image image = Lib.FindSiblingControl<Image>(bonusBox, bonusBox.Name + "_Icon");
                image.Source = new BitmapImage(new Uri("pack://application:,,,/Images/Empty.png"));
                Slider slider = Lib.FindSiblingControl<Slider>(bonusBox, bonusBox.Name + "_Slider");
                slider.Visibility = System.Windows.Visibility.Hidden;
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
        public static List<BonusDisplay> GetBonusOptionsList(string search)
        {
            string[] parts = search.Split(':');
            if(parts.Length == 1)
            {
                if (search == "none")
                {
                    return new List<BonusDisplay>();
                }
                else
                {
                    Console.WriteLine($"ListOptions.GetBonusOptionsList \"{search}\" invalid format :(");
                    return new List<BonusDisplay>();
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
                            return new List<BonusDisplay>();
                    }
                case "locked":
                    return new List<BonusDisplay>() { BonusDisplayHandler.BonusDisplayFromString(value) };
                case "core":
                    return new List<BonusDisplay>() { BonusDisplayHandler.BonusDisplayFromList(gearCoreAttributes, BonusHandler.StringToBonusType(value)) };
                case "side":
                    return new List<BonusDisplay>() { BonusDisplayHandler.BonusDisplayFromList(gearSideAttributes, BonusHandler.StringToBonusType(value)) };
                case "mod":
                    return new List<BonusDisplay>() { BonusDisplayHandler.BonusDisplayFromList(gearModAttributes, BonusHandler.StringToBonusType(value)) };
                default:
                    Console.WriteLine($"ListOptions.GetBonusOptionsList: \"{search}\" is not a recognized preset");
                    return new List<BonusDisplay>();
            }
        }

        public static void OptionsGearBox()
        {
            foreach(StringItem item in ItemHandler.AllItemList)
            {
                bool success = Enum.TryParse(item.Slot, true, out ItemType itemType);
                if (!success) 
                {
                    Console.WriteLine($"Slot: \"{item.Slot}\" cant be converted to itemType from {item.Name}"); continue; 
                }
                ComboBox bonker = Lib.GetItemBox(itemType);
                bonker.Items.Add(new ComboBoxBrandItem { Name = item.Name, Preset = $"[{item.Rarity}]", Slot = item.Slot });
            }
            
        }
    }
}
