using CsvHelper;
using System;
using System.Data;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
//using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using static DivBuildApp.Lib;
using System.Text.RegularExpressions;
using DivBuildApp.UI;
//using static DivBuildApp.ItemHandler;
//using static DivBuildApp.BonusHandler;

namespace DivBuildApp
{
    
    public partial class MainWindow : Window
    {


        public static Dictionary<string, List<EquipBonus>> brandSets = new Dictionary<string, List<EquipBonus>>();

        public MainWindow()
        {
            // Order is important !!!!
            InitializeComponent();
            AdjustWindowSizeToScreen();

            InitializeGearGridLinks();

            InitializeOptionsExpertiece();

            // These don't conflict with eachother
            InitializeItems();
            InitializeBrands();
            InitializeActiveBonusses();
            InitializeGearBonusCaps();

            //No order but must be done last
            InitializeGearOptions();
            InitializeOptionsCoreStat();
            InitializeOptionsSideStats();

            InitializeItemArmor();
            

            
        }

        private void InitializeOptionsExpertiece()
        {
            for (int i = 0; i <= 25; i++)
            {
                MaskExpertiece.Items.Add(i);
                BackpackExpertiece.Items.Add(i);
                ChestExpertiece.Items.Add(i);
                GlovesExpertiece.Items.Add(i);
                HolsterExpertiece.Items.Add(i);
                KneepadsExpertiece.Items.Add(i);
            }
        }
        private void AdjustWindowSizeToScreen()
        {
            double screenHeight = SystemParameters.WorkArea.Height;
            Height = screenHeight * 0.8;
            Width = Height * (16.0 / 9.0);


            // Optionally, center the window on the screen
            Left = (SystemParameters.WorkArea.Width - Width) / 2;
            Top = (SystemParameters.WorkArea.Height - Height) / 2;
        }
        
        public void InitializeItemArmor()
        {
            Task.Run(() => ItemArmorControl.SetItemArmorValues());
        }


        /// <summary>
        /// Creates all GridLinks so they can be referenced by their ItemType name e.g. 'Mask'
        /// </summary>
        private void InitializeGearGridLinks()
        {
            CreateGearGridLink(MaskGrid);
            CreateGearGridLink(BackpackGrid);
            CreateGearGridLink(ChestGrid);
            CreateGearGridLink(GlovesGrid);
            CreateGearGridLink(HolsterGrid);
            CreateGearGridLink(KneepadsGrid);

        }
        /// <summary>
        /// Fills the AllItemList Dictionary with all StringItems in ItemDefault.txt
        /// </summary>
        private void InitializeItems()
        {
            //ItemHandler.AllItemList = FileInfo.ReadItems();
            ItemHandler.AllItemList = FileInfo.ConvertItem(FileInfo.CsvItemDefaults());
        }

        /// <summary>
        /// Fills the brandSets Dictionary with all brand sets in BrandBonusses.txt
        /// </summary>
        private void InitializeBrands()
        {
            brandSets = FileInfo.CsvBrandBonus();
        }

        /// <summary>
        /// Create all the bonusses and set them to 0
        /// </summary>
        private void InitializeActiveBonusses()
        {
            BonusHandler.ResetBonuses();
        }

        /// <summary>
        /// Create default bonus caps for items types
        /// </summary>
        private void InitializeGearBonusCaps()
        {
            //ListOptions.CreateAttributesFromDictionary(FileInfo.ReadBonusCaps());
            ListOptions.CreateBonusCapsFromData(CsvReader.BonusCaps());
        }


        /// <summary>
        /// Create all options for the GearBoxes
        /// </summary>
        private void InitializeGearOptions()
        {
            ListOptions.OptionsGearBox();
        }

        /// <summary>
        /// Create all options for the CoreStatBoxes
        /// </summary>
        private void InitializeOptionsCoreStat()
        {
            ListOptions.OptionsCoreStat(ItemType.Mask);
            ListOptions.OptionsCoreStat(ItemType.Backpack);
            ListOptions.OptionsCoreStat(ItemType.Chest);
            ListOptions.OptionsCoreStat(ItemType.Gloves);
            ListOptions.OptionsCoreStat(ItemType.Holster);
            ListOptions.OptionsCoreStat(ItemType.Kneepads);
        }

        /// <summary>
        /// Create all options for the SideStatBoxes
        /// </summary>
        private void InitializeOptionsSideStats()
        {
            ListOptions.OptionsSideStats(ItemType.Mask);
            ListOptions.OptionsSideStats(ItemType.Backpack);
            ListOptions.OptionsSideStats(ItemType.Chest);
            ListOptions.OptionsSideStats(ItemType.Gloves);
            ListOptions.OptionsSideStats(ItemType.Gloves);
            ListOptions.OptionsSideStats(ItemType.Kneepads);
        }
        



        private void UpdateDisplay()
        {
            BonusHandler.CalculateBonuses();
            Task.Run(() => StatTableDisplay.DisplayBonusesInBoxes(this));

            //DisplayBonusesInBoxes();
        }

        private void ExpertieceBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Task.Run(() => ItemArmorControl.SetItemArmorValues());
            Console.WriteLine($"ExpertieceBox_SelectionChanged {sender}");
            UpdateDisplay();
        }
        private void StatComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GearHandler.SetEquippedGearList();
            Console.WriteLine($"StatComboBox_SelectionChanged {sender}");
            UpdateDisplay();
        }

        private async Task ProcessGearComboBoxChange(ComboBoxBrandItem selectedItem, string baseName)
        {
            if (Enum.TryParse(baseName, out ItemType itemType))
            {
                string brandName = ItemHandler.BrandFromName(selectedItem.Name);

                // Async operations can be awaited normally
                await DisplayControl.SetItemNameLabelAsync(GetItemNameLabel(itemType), selectedItem);
                await DisplayControl.SetBrandImageAsync(GetBrandImage(itemType), brandName);
                await DisplayControl.DisplayBrandBonusesAsync(itemType, brandName);

                // Dispatch other UI updates or CPU-bound work to the UI thread
                Dispatcher.Invoke(() =>
                {
                    ListOptions.SetOptionsStatBoxes(itemType);
                });
            }
        }
        private void GearComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if(sender is ComboBox comboBox && comboBox.SelectedItem is ComboBoxBrandItem selectedItem)
            {
                string baseName = comboBox.Name.Replace("Box", ""); // Removes "Box" to get the base name
                Task.Run(() => ProcessGearComboBoxChange(selectedItem, baseName));
            }
            GearHandler.SetEquippedGearList();
            Console.WriteLine($"GearComboBox_SelectionChanged {sender}");
            UpdateDisplay();
        }

        public void WatchLevel_TextChanged(object sender, TextChangedEventArgs e)
        {
            SHDWatch.SetWatchBonuses(WatchLevel.Text);
            Console.WriteLine($"WatchLevel {sender}");
            UpdateDisplay();
        }
        


        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Regex to match numeric input only
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void TextBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(String)))
            {
                string text = (string)e.DataObject.GetData(typeof(String));
                // Check if pasted text is numeric
                if (!IsTextNumeric(text))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }

        private bool IsTextNumeric(string text)
        {
            Regex regex = new Regex("^[0-9]+$"); // Ensure the string is entirely numeric
            return regex.IsMatch(text);
        }


    }
}
