using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using static DivBuildApp.BonusControl.ActiveBonuses;
//using static DivBuildApp.MainWindow;

namespace DivBuildApp.UI
{
    internal static class StatTableControl
    {
        public static void Initialize(MainWindow main)
        {
            mainWindow = main;
            CalculateBonusesSet += HandleCalculateBonusesSet;

        }
        
        private static void HandleCalculateBonusesSet(object sender, EventArgs e)
        {
            displayCancellationSource.Cancel(); // cancel previous if still running
            displayCancellationSource = new CancellationTokenSource(); // create new token
            Task.Run(() => DisplayBonusesInBoxes(), displayCancellationSource.Token);
            Task.Run(() => Logger.LogEvent("StatTableControl <= ActiveBonuses.CalculateBonuses"));
        }


        private static MainWindow mainWindow;


        private static readonly SemaphoreSlim DisplaySemaphore = new SemaphoreSlim(1); 
        private static CancellationTokenSource displayCancellationSource = new CancellationTokenSource();

        public static async Task DisplayBonusesInBoxes()
        {
            CancellationToken cancellationToken = displayCancellationSource.Token; // Get current token

            await DisplaySemaphore.WaitAsync();
            try
            {
                cancellationToken.ThrowIfCancellationRequested(); // Check for cancellation
                Dictionary<Label, string> values = new Dictionary<Label, string>
                {

                    [mainWindow.WeaponDMG_Value] = $"{activeBonuses[BonusType.Weapon_Damage]}%",
                    [mainWindow.PistolDMG_Value] = $"{activeBonuses[BonusType.Pistol_Damage] + activeBonuses[BonusType.Weapon_Damage]}%",
                    [mainWindow.SignatureDMG_Value] = $"{activeBonuses[BonusType.Signature_Weapon_Damage] + activeBonuses[BonusType.Signature_Weapon_Damage]}%",

                    [mainWindow.ARDMG_Value] = $"{activeBonuses[BonusType.AR_Damage] + activeBonuses[BonusType.Weapon_Damage]}%",
                    [mainWindow.LMGDMG_Value] = $"{activeBonuses[BonusType.LMG_Damage] + activeBonuses[BonusType.Weapon_Damage]}%",
                    [mainWindow.MMRDMG_Value] = $"{activeBonuses[BonusType.MMR_Damage] + activeBonuses[BonusType.Weapon_Damage]}%",

                    [mainWindow.RifleDMG_Value] = $"{activeBonuses[BonusType.Rifle_Damage] + activeBonuses[BonusType.Weapon_Damage]}%",
                    [mainWindow.ShotgunDMG_Value] = $"{activeBonuses[BonusType.Shotgun_Damage] + activeBonuses[BonusType.Weapon_Damage]}%",
                    [mainWindow.SMGDMG_Value] = $"{activeBonuses[BonusType.SMG_Damage] + activeBonuses[BonusType.Weapon_Damage]}%",

                    [mainWindow.CHC_Value] = $"{activeBonuses[BonusType.Critical_Hit_Chance]}%",
                    [mainWindow.CHD_Value] = $"{activeBonuses[BonusType.Critical_Hit_Damage]}%",
                    [mainWindow.HeadshotDMG_Value] = $"{activeBonuses[BonusType.Headshot_Damage]}%",

                    [mainWindow.DMGooC_Value] = $"{activeBonuses[BonusType.DMG_out_of_Cover]}%",
                    [mainWindow.ArmorDMG_Value] = $"{activeBonuses[BonusType.Damage_to_Armor]}%",
                    [mainWindow.HealthDMG_Value] = $"{activeBonuses[BonusType.Health_Damage]}%",

                    [mainWindow.WeaponHandling_Value] = $"{activeBonuses[BonusType.Weapon_Handling]}%",
                    [mainWindow.Accuracy_Value] = $"{activeBonuses[BonusType.Accuracy] + activeBonuses[BonusType.Weapon_Handling]}%",
                    [mainWindow.Stability_Value] = $"{activeBonuses[BonusType.Stability] + activeBonuses[BonusType.Weapon_Handling]}%",

                    [mainWindow.ReloadSpeed_Value] = $"{activeBonuses[BonusType.Reload_Speed] + activeBonuses[BonusType.Weapon_Handling]}%",
                    [mainWindow.SwapSpeed_Value] = $"{activeBonuses[BonusType.Swap_Speed] + activeBonuses[BonusType.Swap_Speed]}%",
                    [mainWindow.RoFBonus_Value] = $"{activeBonuses[BonusType.Rate_of_Fire]}%",

                    [mainWindow.AmmoCapacity_Value] = $"{activeBonuses[BonusType.Ammo_Capacity]}%",
                    [mainWindow.MagaineSize_Value] = $"{activeBonuses[BonusType.Magazine_Size]}%",


                    [mainWindow.MaxArmor_Value] = $"{activeBonuses[BonusType.Armor]}",
                    //values[mainWindow.MaxHealth_Value] = $"{activeBonusses[BonusType.???]}";

                    [mainWindow.TotalArmor_Value] = $"{activeBonuses[BonusType.Total_Armor]}%",
                    [mainWindow.Health_Value] = $"{activeBonuses[BonusType.Health]}%",

                    [mainWindow.HealthOnKill_Value] = $"{activeBonuses[BonusType.Health_on_Kill]}%",
                    [mainWindow.ArmorOnKill_Value] = $"{activeBonuses[BonusType.Armor_on_Kill]}%",
                    [mainWindow.ArmorOnKillSet_Value] = $"{activeBonuses[BonusType.Armor_on_Kill_Stat]}",

                    [mainWindow.HealthRegen_Value] = $"{activeBonuses[BonusType.Health_Regen]}",
                    [mainWindow.ArmorRegen_Value] = $"{activeBonuses[BonusType.Armor_Regen]}%",
                    [mainWindow.ArmorRegenSet_Value] = $"{activeBonuses[BonusType.Armor_Regen_HPs]}",


                    [mainWindow.SkillTiers_Value] = $"{activeBonuses[BonusType.Skill_Tier]}",
                    [mainWindow.SkillEfficiency_Value] = $"{activeBonuses[BonusType.Skill_Efficiency]}%",
                    [mainWindow.SkillDMG_Value] = $"{activeBonuses[BonusType.Skill_Damage] + activeBonuses[BonusType.Skill_Efficiency]}%",

                    [mainWindow.SkillHaste_Value] = $"{activeBonuses[BonusType.Skill_Haste] + activeBonuses[BonusType.Skill_Efficiency]}%",
                    [mainWindow.SkillDuration_Value] = $"{activeBonuses[BonusType.Skill_Duration] + activeBonuses[BonusType.Skill_Efficiency]}%",
                    [mainWindow.SkillRepair_Value] = $"{activeBonuses[BonusType.Repair_Skills] + activeBonuses[BonusType.Skill_Efficiency]}%",

                    [mainWindow.SkillHealth_Value] = $"{activeBonuses[BonusType.Skill_Health] + activeBonuses[BonusType.Skill_Efficiency]}%",
                    [mainWindow.ShieldHealth_Value] = $"{activeBonuses[BonusType.Shield_Health] + activeBonuses[BonusType.Skill_Efficiency]}%",
                    [mainWindow.StatusEffects_Value] = $"{activeBonuses[BonusType.Status_Effects] + activeBonuses[BonusType.Skill_Efficiency]}%",

                    [mainWindow.ExplosivesDMG_Value] = $"{activeBonuses[BonusType.Explosive_Damage]}%"
                };


                await Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    foreach (KeyValuePair<Label, string> value in values)
                    {
                        value.Key.Content = value.Value;
                    }

                }, DispatcherPriority.Normal, cancellationToken); // Use cancellationToken in the InvokeAsync
            }
            catch(OperationCanceledException)
            {
                // Handle exceptions somehow??
            }
            finally
            {
                DisplaySemaphore.Release();
            }

        }
    }
}
