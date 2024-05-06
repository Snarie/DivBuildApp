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
using DivBuildApp.BonusControl;
using DivBuildApp.Data.Tables;
//using static DivBuildApp.ItemHandler;
//using static DivBuildApp.BonusHandler;

namespace DivBuildApp
{
    
    public partial class MainWindow : Window
    {

        public static bool Initializing;
        public MainWindow()
        {
            Initializing = true;
            // Order is important !!!!
            InitializeComponent();
            AdjustWindowSizeToScreen();

            InitializeGearGridLinks();
            Initializer();


            Initializing = false;

        }
        private void InitializeGearGridLinks()
        {
            CreateGearGridLink(MaskGrid);
            CreateGearGridLink(BackpackGrid);
            CreateGearGridLink(ChestGrid);
            CreateGearGridLink(GlovesGrid);
            CreateGearGridLink(HolsterGrid);
            CreateGearGridLink(KneepadsGrid);
        }
        private void Initializer()
        {
            //Stays private
            InitializeOptionsExpertiece();

            GearHandler.Initialize();
            ItemArmorControl.Initialize();
            StatTableControl.Initialize(this);
            StatValueLabelControl.Initialize();
            StatSliderControl.Initialize();
            IconControl.Initialize();
            SHDWatch.Initialize();
            ListOptions.Initialize();
            DisplayControl.Initialize();
            BonusDisplayTypes.Initialize();

            ItemHandler.Initialize();
            BrandSets.Initialize();
            BonusCaps.Initialize();

            ListOptions.OptionsGearBox(ItemHandler.AllItemList);
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
        }
        private void ExpertieceBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox)
            {
                GridEventArgs ge = new GridEventArgs(GetGridContentFromElement(comboBox), -1);

                Task.Run(() =>ItemArmorControl.SetItemArmorAsync(ge));
                //ItemArmorControl.SetItemArmor(ge);
            }
        }


        private void StatSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e, int index)
        {
            if(sender is Slider slider)
            {
                GridEventArgs ge = new GridEventArgs(GetGridContentFromElement(slider), index);

                StatValueLabelControl.SetValue(ge);
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
                GridEventArgs ge = new GridEventArgs(GetGridContentFromElement(comboBox), index);

                StatSliderControl.SetRange(ge);
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

        
        private void GearComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(sender is ComboBox comboBox)
            {
                GridEventArgs ge = new GridEventArgs(GetGridContentFromElement(comboBox), -1);

                GearHandler.SetEquippedGearList(ge);
            }
        }
        

        public void WatchLevel_TextChanged(object sender, TextChangedEventArgs e)
        {
            SHDWatch.SetWatchBonuses(WatchLevel.Text);
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
