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

            // These don't conflict with eachother
            InitializeItems();
            InitializeBrands();
            InitializeActiveBonusses();
            InitializeGearBonusCaps();

            //No order but must be done last
            InitializeGearOptions();
            InitializeOptionsCoreStat();
            InitializeOptionsSideStats();

            
        }
        private void AdjustWindowSizeToScreen()
        {
            double screenHeight = SystemParameters.WorkArea.Height;
            this.Height = screenHeight * 0.8;
            this.Width = this.Height * (16.0 / 9.0);


            // Optionally, center the window on the screen
            this.Left = (SystemParameters.WorkArea.Width - this.Width) / 2;
            this.Top = (SystemParameters.WorkArea.Height - this.Height) / 2;
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
            brandSets = FileInfo.ReadBrandSets();
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
            ListOptions.CreateBonusCapsFromData(ReadCsv.BonusCaps());
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

        private void StatComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GearHandler.SetEquippedGearList();
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
            UpdateDisplay();
        }

        public void WatchLevel_TextChanged(object sender, TextChangedEventArgs e)
        {
            SHDWatch.SetWatchBonuses(WatchLevel.Text);
            UpdateDisplay();
        }
        public void DisplayBonusesInBoxes()
        {

            //Headshot bonuses: AR=55 LMG=65 Shotgun=45 SMG=50 Rifle=60 MMR=0 Pistol=100
            //Explosive_Resistance=10 Hazard_Protection=10
            DisplayBonus(CHC_Value, BonusHandler.activeBonusses[BonusType.Critical_Hit_Chance]);
            DisplayBonus(CHD_Value, BonusHandler.activeBonusses[BonusType.Critical_Hit_Damage]);
            DisplayBonus(HeadshotDMG_Value, BonusHandler.activeBonusses[BonusType.Headshot_Damage]);
            DisplayBonus(DMGooC_Value, BonusHandler.activeBonusses[BonusType.DMG_out_of_Cover]);
            DisplayBonus(ArmorDMG_Value, BonusHandler.activeBonusses[BonusType.Damage_to_Armor]);
            DisplayBonus(HealthDMG_Value, BonusHandler.activeBonusses[BonusType.Health_Damage]);
            DisplayBonus(WeaponHandling_Value, BonusHandler.activeBonusses[BonusType.Weapon_Handling]);
            DisplayBonus(Accuracy_Value, BonusHandler.activeBonusses[BonusType.Accuracy] + BonusHandler.activeBonusses[BonusType.Weapon_Handling]);
            DisplayBonus(Stability_Value, BonusHandler.activeBonusses[BonusType.Stability] + BonusHandler.activeBonusses[BonusType.Weapon_Handling]);
            DisplayBonus(ReloadSpeed_Value, BonusHandler.activeBonusses[BonusType.Reload_Speed] + BonusHandler.activeBonusses[BonusType.Weapon_Handling]);
            DisplayBonus(SwapSpeed_Value, BonusHandler.activeBonusses[BonusType.Swap_Speed] + BonusHandler.activeBonusses[BonusType.Weapon_Handling]);

            DisplayBonus(WeaponDMG_Value, BonusHandler.activeBonusses[BonusType.Weapon_Damage]);
            DisplayBonus(MMRDMG_Value, BonusHandler.activeBonusses[BonusType.MMR_Damage]);
            DisplayBonus(RifleDMG_Value, BonusHandler.activeBonusses[BonusType.Rifle_Damage]);
            DisplayBonus(SMGDMG_Value, BonusHandler.activeBonusses[BonusType.SMG_Damage]);
            DisplayBonus(LMGDMG_Value, BonusHandler.activeBonusses[BonusType.LMG_Damage]);
            DisplayBonus(ARDMG_Value, BonusHandler.activeBonusses[BonusType.AR_Damage]);
            DisplayBonus(ShotgunDMG_Value, BonusHandler.activeBonusses[BonusType.Shotgun_Damage]);
            DisplayBonus(PistolDMG_Value, BonusHandler.activeBonusses[BonusType.Pistol_Damage]);
            DisplayBonus(SignatureDMG_Value, BonusHandler.activeBonusses[BonusType.Signature_Weapon_Damage]);
            DisplayBonus(RoFBonus_Value, BonusHandler.activeBonusses[BonusType.Rate_of_Fire]);
            DisplayBonus(AmmoCapacity_Value, BonusHandler.activeBonusses[BonusType.Ammo_Capacity]);
            
            DisplayBonus(TotalArmor_Value, BonusHandler.activeBonusses[BonusType.Total_Armor]);
            DisplayBonus(HealthOnKill_Value, BonusHandler.activeBonusses[BonusType.Health_on_Kill]);
            DisplayBonus(ArmorOnKill_Value, BonusHandler.activeBonusses[BonusType.Armor_on_Kill]);
            DisplayBonus(ArmorOnKillSet_Value, BonusHandler.activeBonusses[BonusType.Armor_on_Kill_Stat]);

            DisplayBonus(Health_Value, BonusHandler.activeBonusses[BonusType.Health]);
            DisplayBonus(HealthRegen_Value, BonusHandler.activeBonusses[BonusType.Health_Regen]);
            DisplayBonus(ArmorRegen_Value, BonusHandler.activeBonusses[BonusType.Armor_Regen]);
            DisplayBonus(ArmorRegenSet_Value, BonusHandler.activeBonusses[BonusType.Armor_Regen_HPs]);

            DisplayBonus(SkillTiers_Value, BonusHandler.activeBonusses[BonusType.Skill_Tier]);
            DisplayBonus(SkillEfficiency_Value, BonusHandler.activeBonusses[BonusType.Skill_Efficiency]);
            DisplayBonus(SkillDMG_Value, BonusHandler.activeBonusses[BonusType.Skill_Damage] + BonusHandler.activeBonusses[BonusType.Skill_Efficiency]);
            DisplayBonus(SkillHaste_Value, BonusHandler.activeBonusses[BonusType.Skill_Haste] + BonusHandler.activeBonusses[BonusType.Skill_Efficiency]);
            DisplayBonus(SkillDuration_Value, BonusHandler.activeBonusses[BonusType.Skill_Duration] + BonusHandler.activeBonusses[BonusType.Skill_Efficiency]);

            DisplayBonus(SkillHealth_Value, BonusHandler.activeBonusses[BonusType.Skill_Health] + BonusHandler.activeBonusses[BonusType.Skill_Efficiency]);
            DisplayBonus(ShieldHealth_Value, BonusHandler.activeBonusses[BonusType.Shield_Health] + BonusHandler.activeBonusses[BonusType.Skill_Health] + BonusHandler.activeBonusses[BonusType.Skill_Efficiency]);
            DisplayBonus(SkillRepair_Value, BonusHandler.activeBonusses[BonusType.Repair_Skills] + BonusHandler.activeBonusses[BonusType.Skill_Efficiency]);
            DisplayBonus(StatusEffects_Value, BonusHandler.activeBonusses[BonusType.Status_Effects] + BonusHandler.activeBonusses[BonusType.Status_Effects]);
            DisplayBonus(ExplosivesDMG_Value, BonusHandler.activeBonusses[BonusType.Explosive_Damage]);

        }
        public void DisplayBonus(Label box, double value)
        {
            box.Content = "+"+value+"%";
        }
        public void DisplayBonuses()
        {
            string msg = string.Empty;
            foreach (KeyValuePair<BonusType, double> bonus in BonusHandler.activeBonusses)
            {
                msg += $"{bonus.Key} = {bonus.Value}\n";
            }
            DumpBlock.Text = msg;
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
