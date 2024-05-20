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
using DivBuildApp.BonusControl;
using DivBuildApp.Data.CsvFormats;
//using DivBuildApp.Classes;
using DivBuildApp.Data.Tables;
using System.Windows;

namespace DivBuildApp
{
    internal static class ListOptions
    {
        public static void Initialize()
        {
            GearHandler.GearSet += HandleGearSet;
            WeaponHandler.WeaponSet += HandleWeaponSet;
            //Creates the eventHandlers
        }
        private static void HandleWeaponSet(object sender, WeaponEventArgs e)
        {
            SetOptionsWeaponStatBoxes(e);
            SetOptionsWeaponModBoxes(e);
        }
        private static void HandleGearSet(object sender, GridEventArgs e)
        {
            SetOptionsStatBoxes(e);
            Task.Run(() => Logger.LogEvent("ListOptions <= GearHandler.GearSet"));
        }

        public static void SetOptionsStatBoxes(GridEventArgs e)
        {
            ComboBox[] statBoxes = e.Grid.StatBoxes;

            if (statBoxes[0] != null && statBoxes[1] != null && statBoxes[2] != null && statBoxes[3] != null)
            {
                ComboBox itemBox = e.Grid.Box;
                if (itemBox.SelectedItem is ComboBoxBrandItem selectedItem) 
                {
                    StringItem stringItem = ItemHandler.ItemFromIdentity(selectedItem.Name, selectedItem.Slot);

                    SetItemsSource(new GridEventArgs(e.Grid, e.ItemType, 0), stringItem.CoreAttribute);
                    SetItemsSource(new GridEventArgs(e.Grid, e.ItemType, 1), stringItem.SideAttribute1);
                    SetItemsSource(new GridEventArgs(e.Grid, e.ItemType, 2), stringItem.SideAttribute2);
                    SetItemsSource(new GridEventArgs(e.Grid, e.ItemType, 3), stringItem.SideAttribute3);
                }
            }
        }
        private static void SetItemsSource(GridEventArgs e, string itemAttribute)
        {
            ComboBox statBox = e.Grid.StatBoxes[e.Index];
            int bonusIndex = statBox.SelectedIndex;
            int bonusItemCount = statBox.Items.Count;
            statBox.ItemsSource = GetBonusOptionsList(itemAttribute);
            SetBoxSelectedIndex(e, bonusIndex, bonusItemCount);
        }
        public static void SetOptionsWeaponModBoxes(WeaponEventArgs e)
        {
            ComboBox box = e.Grid.Box;
            if(box.SelectedItem is WeaponListFormat wlf)
            {
                e.Grid.OpticalRail.ItemsSource = WeaponMods.GetFilteredMods(wlf.ModSlot().OpticalRail);
                e.Grid.Magazine.ItemsSource = WeaponMods.GetFilteredMods(wlf.ModSlot().Magazine);
                e.Grid.Underbarrel.ItemsSource = WeaponMods.GetFilteredMods(wlf.ModSlot().Underbarrel);
                e.Grid.Muzzle.ItemsSource = WeaponMods.GetFilteredMods(wlf.ModSlot().Muzzle);
                SetComboBoxSelectedIndex(new ComboBox[] { e.Grid.OpticalRail, e.Grid.Magazine, e.Grid.Underbarrel, e.Grid.Muzzle });
            }
        }
        
        
        
        public static void SetOptionsWeaponStatBoxes(WeaponEventArgs e)
        {
            ComboBox[] statBoxes = e.Grid.StatBoxes;

            if (statBoxes[0] != null && statBoxes[1] != null && statBoxes[2] != null)
            {
                ComboBox box = e.Grid.Box;
                if (box.SelectedItem is WeaponListFormat wlf)
                {

                    WeaponAttributesFormat attributes = wlf.Attributes();
                    if(attributes == null)
                    {
                        Console.WriteLine();
                        Console.WriteLine(attributes.Core);
                    }

                    SetWeaponSource(e, 0, attributes.Core);
                    SetWeaponSource(e, 1, attributes.Main);
                    SetWeaponSource(e, 2, attributes.Side);
                    SetComboBoxSelectedIndex(statBoxes);
                }
            }
        }
        private static void SetWeaponSource(WeaponEventArgs e, int i, string itemAttribute)
        {
            ComboBox statBox = e.Grid.StatBoxes[i];
            //int bonusIndex = statBox.SelectedIndex;
            //int bonusItemCount = statBox.Items.Count;
            List<BonusDisplay> list = GetBonusOptionsList(itemAttribute);
            statBox.ItemsSource = list;

        }
        private static void SetWeaponSelectedIndex(WeaponEventArgs e)
        {
            ComboBox box = e.Grid.Box;

            if(box.Items.Count > 0)
            {
                box.SelectedIndex = 0;
                if (box.Items.Count == 1)
                {
                    box.IsEnabled = false;
                }
                else
                {
                    box.IsEnabled = true;

                }
            }
        }

        private static void SetComboBoxSelectedIndex(ComboBox[] comboBoxes)
        {
            foreach(ComboBox comboBox in comboBoxes)
            {
                if (comboBox.Items.Count > 0)
                {
                    if (comboBox.Items.Count == 1)
                    {
                        comboBox.SelectedIndex = 0;
                        comboBox.IsEnabled = false;
                    }
                    else
                    {
                        comboBox.SelectedIndex = 0;
                        comboBox.IsEnabled = true;
                    }
                }
                else
                {
                    comboBox.IsEnabled = false;
                }
            }
        }
        
        
        private static void SetBoxSelectedIndex(GridEventArgs e, int oldIndex, int oldItemCount)
        {
            ComboBox bonusBox = e.Grid.StatBoxes[e.Index];
            // TODO: add a saver if it switches away from a list (to go back to the saved index when you go back to lists)
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
                    // TODO: integrate image into IconControl
                    Slider slider = e.Grid.StatSliders[e.Index];
                    slider.Visibility = System.Windows.Visibility.Hidden;
                }
            }
            else
            {
                // TODO: integrate image and slider into IconControl and SliderControl respectively
                Image image = e.Grid.StatIcons[e.Index];
                image.Source = new BitmapImage(new Uri("pack://application:,,,/Images/Empty.png"));
                Slider slider = e.Grid.StatSliders[e.Index];
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

                    Task.Run(() => Logger.LogError($"\"{search}\" invalid format"));
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
                        case "wside":
                            return BonusCaps.WeaponSideAttributes;
                        case "core":
                            return BonusCaps.GearCoreAttributes;
                        case "side":
                            return BonusCaps.GearSideAttributes;
                        case "mod":
                            return BonusCaps.GearModAttributes;
                        default:

                            Task.Run(() => Logger.LogError($"\"{value}\" is not a recognized list"));
                            return new List<BonusDisplay>();
                    }
                case "locked":
                    return new List<BonusDisplay>() { new BonusDisplay(new Bonus(value)) };
                case "wcore":
                    return new List<BonusDisplay>() { BonusDisplayHandler.BonusDisplayFromList(BonusCaps.WeaponCoreAttributes, BonusHandler.StringToBonusType(value)) };
                case "wmain":
                    return new List<BonusDisplay>() { BonusDisplayHandler.BonusDisplayFromList(BonusCaps.WeaponMainAttributes, BonusHandler.StringToBonusType(value)) };
                case "wside":
                    return new List<BonusDisplay>() { BonusDisplayHandler.BonusDisplayFromList(BonusCaps.WeaponSideAttributes, BonusHandler.StringToBonusType(value)) };
                case "core":
                    return new List<BonusDisplay>() { BonusDisplayHandler.BonusDisplayFromList(BonusCaps.GearCoreAttributes, BonusHandler.StringToBonusType(value)) };
                case "side":
                    return new List<BonusDisplay>() { BonusDisplayHandler.BonusDisplayFromList(BonusCaps.GearSideAttributes, BonusHandler.StringToBonusType(value)) };
                case "mod":
                    return new List<BonusDisplay>() { BonusDisplayHandler.BonusDisplayFromList(BonusCaps.GearModAttributes, BonusHandler.StringToBonusType(value)) };
                default:
                    Task.Run(() => Logger.LogError($"\"{search}\" is not a recognized preset"));
                    return new List<BonusDisplay>();
            }
        }

        public static void OptionsGearBox(List<StringItem> items)
        {
            foreach(StringItem item in items)
            {
                bool success = Enum.TryParse(item.Slot, true, out ItemType itemType);
                if (!success) 
                {
                    Task.Run(() => Logger.LogError($"\"{item.Slot}\" can't be converted to ItemType from {item.Name}"));
                    continue; 
                }
                ComboBox bonker = Lib.GetItemBox(itemType);
                bonker.Items.Add(new ComboBoxBrandItem { Name = item.Name, Preset = $"[{item.Rarity}]", Slot = item.Slot });
            }
            
        }

        public static void OptionsWeaponBox(WeaponEventArgs e)
        {
            if(e.Grid.WeaponType.SelectedItem is WeaponType weaponType)
            {
                List<WeaponListFormat> weapons = WeaponList.TypeList[weaponType];
                e.Grid.Box.Items.Clear();
                foreach (WeaponListFormat weapon in weapons)
                {
                    e.Grid.Box.Items.Add(weapon);
                }
                SetWeaponSelectedIndex(e);
            }
        }
    }
}
