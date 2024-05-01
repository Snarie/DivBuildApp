﻿using CsvHelper;
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
using DivBuildApp.BonusControl;
//using static DivBuildApp.ItemHandler;
//using static DivBuildApp.BonusHandler;

namespace DivBuildApp
{
    
    public partial class MainWindow : Window
    {


        //public static Dictionary<string, List<EquipBonus>> brandSets = new Dictionary<string, List<EquipBonus>>();

        public static bool Initializing;
        public MainWindow()
        {
            Initializing = true;
            // Order is important !!!!
            InitializeComponent();
            AdjustWindowSizeToScreen();

            InitializeGearGridLinks();

            InitializeOptionsExpertiece();

            // These don't conflict with eachother
            InitializeItems();
            InitializeBrands();
            InitializeBonusDisplayTypes();
            InitializeBonusCaps();

            //No order but must be done last
            InitializeGearOptions();
            InitializeOptionsStatAttributes();

            InitializeItemArmor();

            Initializing = false;

        }

        private void InitializeOptionsExpertiece()
        {
            for (int i = 0; i <= 26; i++)
            {
                MaskExpertiece.Items.Add(i);
                BackpackExpertiece.Items.Add(i);
                ChestExpertiece.Items.Add(i);
                GlovesExpertiece.Items.Add(i);
                HolsterExpertiece.Items.Add(i);
                KneepadsExpertiece.Items.Add(i);
                GlobalExpertiece.Items.Add(i);
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
            //Values are already defined by InitializeOptionsExpertiece selectionchanged triggers
            //ItemArmorControl.SetItemArmor();
            Task.Run(() => ItemArmorControl.DisplayItemArmorValues());
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
            BonusHandler.brandSets = FileInfo.CsvBrandBonus();
        }

        

        private void InitializeBonusDisplayTypes()
        {
            BonusHandler.SetBonusDisplayTypes(CsvReader.BonusDisplayType());
        }
        /// <summary>
        /// Create default bonus caps for items types
        /// </summary>
        private void InitializeBonusCaps()
        {
            //ListOptions.CreateAttributesFromDictionary(FileInfo.ReadBonusCaps());
            //ListOptions.bonusCapsList = FileInfo.CsvBonusCaps();
            BonusCaps.CreateBonusCapsFromData(CsvReader.BonusCaps());
        }


        /// <summary>
        /// Create all options for the GearBoxes
        /// </summary>
        private void InitializeGearOptions()
        {
            ListOptions.OptionsGearBox();
        }

        private void InitializeOptionsStatAttributes()
        {
            ListOptions.SetAllOptionsStatBoxes();
        }
        



        private void UpdateDisplay()
        {
            ActiveBonuses.CalculateBonuses();
            Task.Run(() => StatTableControl.DisplayBonusesInBoxes(this));
        }

        private void GlobalExpertieceBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox)
            {
                MaskExpertiece.SelectedIndex = comboBox.SelectedIndex;
                BackpackExpertiece.SelectedIndex = comboBox.SelectedIndex;
                ChestExpertiece.SelectedIndex = comboBox.SelectedIndex;
                GlovesExpertiece.SelectedIndex = comboBox.SelectedIndex;
                HolsterExpertiece.SelectedIndex = comboBox.SelectedIndex;
                KneepadsExpertiece.SelectedIndex = comboBox.SelectedIndex;

            }
            UpdateDisplay();
        }
        private void ExpertieceBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox)
            {
                string baseName = comboBox.Name.Replace("Expertiece", "");
                if(Enum.TryParse(baseName, out ItemType itemType))
                {
                    ItemArmorControl.SetItemArmor(itemType);
                }
            }
            Task.Run(() => ItemArmorControl.DisplayItemArmorValues());
            Console.WriteLine($"ExpertieceBox_SelectionChanged {sender}");
            UpdateDisplay();
        }


        private void StatSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e, int index)
        {
            if(sender is Slider slider)
            {
                ItemType itemType = GetItemTypeFromElement(slider);
                StatValueLabelControl.SetValue(itemType, index);
                UpdateDisplay();
            }
        }
        private void Stat1Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (Initializing) return;
            StatSlider_ValueChanged(sender, e, 0);
        }
        private void Stat2Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (Initializing) return;
            StatSlider_ValueChanged(sender, e, 1);
        }
        private void Stat3Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (Initializing) return;
            StatSlider_ValueChanged(sender, e, 2);
        }
        private void Stat4Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (Initializing) return;
            StatSlider_ValueChanged(sender, e, 3);
        }

        private void StatComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e, int index)
        {
            if (sender is ComboBox comboBox)
            {
                ItemType itemType = GetItemTypeFromElement(comboBox);
                StatValueLabelControl.SetValue(itemType, index);
                Task.Run(() => IconControl.SetStatIcon(itemType, index));

                Image image = FindSiblingControl<Image>(comboBox, comboBox.Name + "_Icon");
                Slider slider = FindSiblingControl<Slider>(comboBox, comboBox.Name + "_Slider");
                if (comboBox.SelectedItem is BonusDisplay bonusDisplay)
                {
                    StatSliderControl.SetRange(slider, bonusDisplay);
                    //Task.Run(() => IconControl.SetStatIcon(image, bonusDisplay));
                }
                else
                {
                    //image.Source = new BitmapImage(new Uri("pack://application:,,,/Images/ItemType Icons/Undefined.png"));
                    slider.Visibility = Visibility.Collapsed;
                }
                UpdateDisplay();
            }
        }
        private void Stat1ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            StatComboBox_SelectionChanged(sender, e, 0);
        }
        private void Stat2ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            StatComboBox_SelectionChanged(sender, e, 1);
        }
        private void Stat3ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            StatComboBox_SelectionChanged(sender, e, 2);
        }
        private void Stat4ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            StatComboBox_SelectionChanged(sender, e, 3);
        }

        private async Task ProcessGearComboBoxChange(ComboBoxBrandItem selectedItem, string baseName)
        {
            if (Enum.TryParse(baseName, out ItemType itemType))
            {
                string brandName = ItemHandler.BrandFromName(selectedItem.Name);

                // Async operations can be awaited normally
                await DisplayControl.SetItemNameLabelAsync(GetItemNameLabel(itemType), selectedItem);
                await IconControl.SetBrandImage(GetBrandImage(itemType), brandName);
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
            Console.WriteLine($"GearComboBox_SelectionChanged {sender}");
            if (GearHandler.SetEquippedGearList())
            {
                UpdateDisplay();
            }
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
