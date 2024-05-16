using System;
//using System.Reflection.Emit;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static DivBuildApp.Lib;
using System.Text.RegularExpressions;
using DivBuildApp.UI;
using DivBuildApp.BonusControl;
using DivBuildApp.Data.Tables;
using DivBuildApp.Data.CsvFormats;
using System.Text;
//using DivBuildApp.Classes;
//using static DivBuildApp.ItemHandler;
//using static DivBuildApp.BonusHandler;

namespace DivBuildApp
{

    public partial class MainWindow : Window
    {

        public static bool Initializing;
        public static bool InitializeComp;
        public MainWindow()
        {
            Initializing = true;
            InitializeComp = true;

            // Order is important !!!!
            InitializeComponent();
            AdjustWindowSizeToScreen();

            InitializeGearGridLinks();
            InitializeWeaponGridLinks();
            InitializeComp = false;

            Initializer();


            Initializing = false;

        }
        private void InitializeWeaponGridLinks()
        {
            WeaponGridLinks.Add("PrimaryWeapon", new WeaponGridContent(
                PrimaryWeaponBox, PrimaryWeaponName, PrimaryWeaponArmor,
                PrimaryWeaponExpertiece, PrimaryWeaponType,
                PrimaryWeaponOpticalRail, PrimaryWeaponMagazine, PrimaryWeaponUnderbarrel, PrimaryWeaponMuzzle,
                PrimaryWeaponDamage, PrimaryWeaponRPM, PrimaryWeaponMagazineSize,
                new ComboBox[] { PrimaryWeaponStat1, PrimaryWeaponStat2, PrimaryWeaponStat3 },
                new Label[] { PrimaryWeaponStat1_Value, PrimaryWeaponStat2_Value, PrimaryWeaponStat3_Value },
                new Slider[] { PrimaryWeaponStat1_Slider, PrimaryWeaponStat2_Slider, PrimaryWeaponStat3_Slider },
                new Image[] { PrimaryWeaponStat1_Icon, PrimaryWeaponStat2_Icon, PrimaryWeaponStat3_Icon },
                PrimaryWeaponImage
            )); WeaponGridLinks.Add("SecondaryWeapon", new WeaponGridContent(
                SecondaryWeaponBox, SecondaryWeaponName, SecondaryWeaponArmor,
                SecondaryWeaponExpertiece, SecondaryWeaponType,
                SecondaryWeaponOpticalRail, SecondaryWeaponMagazine, SecondaryWeaponUnderbarrel, SecondaryWeaponMuzzle,
                SecondaryWeaponDamage, SecondaryWeaponRPM, SecondaryWeaponMagazineSize,
                new ComboBox[] { SecondaryWeaponStat1, SecondaryWeaponStat2, SecondaryWeaponStat3 },
                new Label[] { SecondaryWeaponStat1_Value, SecondaryWeaponStat2_Value, SecondaryWeaponStat3_Value },
                new Slider[] { SecondaryWeaponStat1_Slider, SecondaryWeaponStat2_Slider, SecondaryWeaponStat3_Slider },
                new Image[] { SecondaryWeaponStat1_Icon, SecondaryWeaponStat2_Icon, SecondaryWeaponStat3_Icon },
                SecondaryWeaponImage
            )); WeaponGridLinks.Add("SideArm", new WeaponGridContent(
                SideArmBox, SideArmName, SideArmArmor,
                SideArmExpertiece, SideArmType,
                SideArmOpticalRail, SideArmMagazine, SideArmUnderbarrel, SideArmMuzzle,
                SideArmDamage, SideArmRPM, SideArmMagazineSize,
                new ComboBox[] { SideArmStat1, SideArmStat2, SideArmStat3 },
                new Label[] { SideArmStat1_Value, SideArmStat2_Value, SideArmStat3_Value },
                new Slider[] { SideArmStat1_Slider, SideArmStat2_Slider, SideArmStat3_Slider },
                new Image[] { SideArmStat1_Icon, SideArmStat2_Icon, SideArmStat3_Icon },
                SideArmImage
            ));
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
            ActiveBonuses.Initialize();
            //Stays private
            ItemArmorControl.Initialize();
            //Lib.Initialize(this);
            InitializeOptionsExpertiece();
            InitializeOptionsWeaponSlot();

            WeaponHandler.Initialize();
            GearHandler.Initialize();
            StatTableControl.Initialize(this);
            StatValueLabelControl.Initialize();
            StatSliderControl.Initialize();
            WeaponStatsControl.Initialize();
            IconControl.Initialize();
            SHDWatch.Initialize();
            ListOptions.Initialize();
            DisplayControl.Initialize();
            BonusDisplayTypes.Initialize();

            ItemHandler.Initialize();
            BrandSets.Initialize();
            BonusCaps.Initialize();
            WeaponList.Initialize();
            WeaponStats.Initialize();
            //attributes requires stats and list
            WeaponAttributes.Initialize();


            InitializeWeaponTypeBox();

            //ListOptions.OptionsWeaponBox(WeaponType.SMG, PrimaryWeaponBox, SecondaryWeaponBox);
            ListOptions.OptionsGearBox(ItemHandler.AllItemList);
        }
        private void InitializeOptionsWeaponSlot()
        {
            foreach(WeaponSlot slot in Enum.GetValues(typeof(WeaponSlot)))
            {
                EquippedWeaponSlot.Items.Add(slot);
            }
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
        private void InitializeWeaponTypeBox()
        {
            foreach (WeaponType weaponType in Enum.GetValues(typeof(WeaponType)))
            {
                PrimaryWeaponType.Items.Add(weaponType);
                SecondaryWeaponType.Items.Add(weaponType);
            }
            SideArmType.Items.Add(WeaponType.Pistol);
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





        private void WeaponType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox)
            {
                WeaponEventArgs we = new WeaponEventArgs(GetGridBaseNameFromChild(comboBox), -1);
                ListOptions.OptionsWeaponBox(we);
            }
        }
        private void WeaponBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox)
            {
                WeaponEventArgs we = new WeaponEventArgs(GetGridBaseNameFromChild(comboBox), -1);
                WeaponHandler.SetEquippedWeaponListAsync(we);
            }
        }
        private void WeaponStatSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e, int index)
        {
            if(sender is Slider slider)
            {
                WeaponEventArgs we = new WeaponEventArgs(GetGridBaseNameFromChild(slider), index);
                StatValueLabelControl.SetWeaponValueAsync(we);
            }
        }
        private void WeaponStat1Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (Initializing) return;
            WeaponStatSlider_ValueChanged(sender, e, 0);
        }
        private void WeaponStat2Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (Initializing) return;
            WeaponStatSlider_ValueChanged(sender, e, 1);
        }
        private void WeaponStat3Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (Initializing) return;
            WeaponStatSlider_ValueChanged(sender, e, 2);
        }

        private void WeaponStatBox_SelectionChanged(object sender, SelectionChangedEventArgs e, int index)
        {
            if(sender is ComboBox comboBox)
            {
                WeaponEventArgs we = new WeaponEventArgs(GetGridBaseNameFromChild(comboBox), index);
                StatSliderControl.SetWeaponRangeAsync(we);
            }
        }
        private void WeaponStat1Box_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            WeaponStatBox_SelectionChanged(sender, e, 0);
        }
        private void WeaponStat2Box_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            WeaponStatBox_SelectionChanged(sender, e, 1);
        }
        private void WeaponStat3Box_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            WeaponStatBox_SelectionChanged(sender, e, 2);
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
                Task.Run(() => ItemArmorControl.SetItemArmorAsync(ge));
            }
        }


        private void StatSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e, int index)
        {
            if(sender is Slider slider)
            {
                GridEventArgs ge = new GridEventArgs(GetGridContentFromElement(slider), index);
                StatValueLabelControl.SetValueAsync(ge);
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
                StatSliderControl.SetRangeAsync(ge);
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
                GearHandler.SetEquippedGearListAsync(ge);
            }
        }
        

        public void WatchLevel_TextChanged(object sender, TextChangedEventArgs e)
        {
            string level = WatchLevel.Text;
            // GridEventArgs ge = new GridEventArgs(GetGridContentFromElement(WatchLevel), -1);
            Task.Run(() => SHDWatch.SetWatchBonusesAsync(level));
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

        private void EquippedWeaponSlot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(sender is ComboBox comboBox)
            {
                if(comboBox.SelectedItem is WeaponSlot slot)
                {
                    WeaponHandler.SetEquippedWepaonSlot(slot);
                }
            }
        }
    }
}
