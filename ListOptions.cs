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

namespace DivBuildApp
{
    internal static class ListOptions
    {
        
        private static void SetItemsSource(ComboBox statBox, string itemAttribute)
        {
            int bonusIndex = statBox.SelectedIndex;
            int bonusItemCount = statBox.Items.Count;
            statBox.ItemsSource = GetBonusOptionsList(itemAttribute);
            SetBoxSelectedIndex(statBox, bonusIndex, bonusItemCount);
        }


        public static void SetOptionsStatBoxes(GridEventArgs e)
        {
            ComboBox itemBox = e.Grid.ItemBox;
            ComboBox[] statBoxes = e.Grid.StatBoxes;

            if (statBoxes[0] != null && statBoxes[1] != null && statBoxes[2] != null && statBoxes[3] != null)
            {
                OptionsStatAttributes(itemBox, statBoxes);
            }
        }
        
        private static void OptionsStatAttributes(ComboBox itemBox, ComboBox[] statBoxes)
        {
            if(itemBox.SelectedItem is ComboBoxBrandItem selectedItem)
            {
                StringItem stringItem = ItemHandler.ItemFromIdentity(selectedItem.Name, selectedItem.Slot);

                SetItemsSource(statBoxes[0], stringItem.CoreAttribute);
                SetItemsSource(statBoxes[1], stringItem.SideAttribute1);
                SetItemsSource(statBoxes[2], stringItem.SideAttribute2);
                SetItemsSource(statBoxes[3], stringItem.SideAttribute3);
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
    }
}
