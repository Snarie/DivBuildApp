using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using static DivBuildApp.BonusHandler;

namespace DivBuildApp.UI
{
    internal static class StatTableDisplay
    {

        public static async Task DisplayBonusesInBoxes(MainWindow mainWindow)
        {
            Dictionary<Label, string> values = new Dictionary<Label, string>
            {
                //Task to leave room for complex tasks and more responsive design
                //Headshot bonuses: AR=55 LMG=65 Shotgun=45 SMG=50 Rifle=60 MMR=0 Pistol=100
                [mainWindow.CHC_Value] = $"{activeBonusses[BonusType.Critical_Hit_Chance]}%",
                [mainWindow.CHD_Value] = $"{activeBonusses[BonusType.Critical_Hit_Damage]}%",
                [mainWindow.HeadshotDMG_Value] = $"{activeBonusses[BonusType.Headshot_Damage]}%",
                [mainWindow.DMGooC_Value] = $"{activeBonusses[BonusType.DMG_out_of_Cover]}%",
                [mainWindow.ArmorDMG_Value] = $"{activeBonusses[BonusType.Damage_to_Armor]}%",
                [mainWindow.HealthDMG_Value] = $"{activeBonusses[BonusType.Health_Damage]}%",
                [mainWindow.WeaponHandling_Value] = $"{activeBonusses[BonusType.Weapon_Handling]}%",
                [mainWindow.Accuracy_Value] = $"{activeBonusses[BonusType.Accuracy] + activeBonusses[BonusType.Weapon_Handling]}%",
                [mainWindow.Stability_Value] = $"{activeBonusses[BonusType.Stability] + activeBonusses[BonusType.Weapon_Handling]}%",
                [mainWindow.ReloadSpeed_Value] = $"{activeBonusses[BonusType.Reload_Speed] + activeBonusses[BonusType.Weapon_Handling]}%",
                [mainWindow.SwapSpeed_Value] = $"{activeBonusses[BonusType.Swap_Speed] + activeBonusses[BonusType.Swap_Speed]}%",

                [mainWindow.WeaponDMG_Value] = $"{activeBonusses[BonusType.Weapon_Damage]}%",
                [mainWindow.MMRDMG_Value] = $"{activeBonusses[BonusType.MMR_Damage] + activeBonusses[BonusType.Weapon_Damage]}%",
                [mainWindow.RifleDMG_Value] = $"{activeBonusses[BonusType.Rifle_Damage] + activeBonusses[BonusType.Weapon_Damage]}%",
                [mainWindow.SMGDMG_Value] = $"{activeBonusses[BonusType.SMG_Damage] + activeBonusses[BonusType.Weapon_Damage]}%",
                [mainWindow.LMGDMG_Value] = $"{activeBonusses[BonusType.LMG_Damage] + activeBonusses[BonusType.Weapon_Damage]}%",
                [mainWindow.ARDMG_Value] = $"{activeBonusses[BonusType.AR_Damage] + activeBonusses[BonusType.Weapon_Damage]}%",
                [mainWindow.ShotgunDMG_Value] = $"{activeBonusses[BonusType.Shotgun_Damage] + activeBonusses[BonusType.Weapon_Damage]}%",
                [mainWindow.PistolDMG_Value] = $"{activeBonusses[BonusType.Pistol_Damage] + activeBonusses[BonusType.Weapon_Damage]}%",
                [mainWindow.SignatureDMG_Value] = $"{activeBonusses[BonusType.Signature_Weapon_Damage] + activeBonusses[BonusType.Signature_Weapon_Damage]}%",
                [mainWindow.RoFBonus_Value] = $"{activeBonusses[BonusType.Rate_of_Fire]}%",
                [mainWindow.AmmoCapacity_Value] = $"{activeBonusses[BonusType.Ammo_Capacity]}%",

                [mainWindow.MaxArmor_Value] = $"{activeBonusses[BonusType.Armor]}",
                [mainWindow.TotalArmor_Value] = $"{activeBonusses[BonusType.Total_Armor]}%",
                [mainWindow.HealthOnKill_Value] = $"{activeBonusses[BonusType.Health_on_Kill]}%",
                [mainWindow.ArmorOnKill_Value] = $"{activeBonusses[BonusType.Armor_on_Kill]}%",
                [mainWindow.ArmorOnKillSet_Value] = $"{activeBonusses[BonusType.Armor_on_Kill_Stat]}",

                //values[mainWindow.MaxHealth_Value] = $"{activeBonusses[BonusType.???]}";
                [mainWindow.Health_Value] = $"{activeBonusses[BonusType.Health]}%",
                [mainWindow.HealthRegen_Value] = $"{activeBonusses[BonusType.Health_Regen]}",
                [mainWindow.ArmorRegen_Value] = $"{activeBonusses[BonusType.Armor_Regen]}%",
                [mainWindow.ArmorRegenSet_Value] = $"{activeBonusses[BonusType.Armor_Regen_HPs]}",

                [mainWindow.SkillTiers_Value] = $"{activeBonusses[BonusType.Skill_Tier]}",
                [mainWindow.SkillEfficiency_Value] = $"{activeBonusses[BonusType.Skill_Efficiency]}%",
                [mainWindow.SkillDMG_Value] = $"{activeBonusses[BonusType.Skill_Damage] + activeBonusses[BonusType.Skill_Efficiency]}%",
                [mainWindow.SkillHaste_Value] = $"{activeBonusses[BonusType.Skill_Haste] + activeBonusses[BonusType.Skill_Efficiency]}%",
                [mainWindow.SkillDuration_Value] = $"{activeBonusses[BonusType.Skill_Duration] + activeBonusses[BonusType.Skill_Efficiency]}%",

                [mainWindow.SkillHealth_Value] = $"{activeBonusses[BonusType.Skill_Health] + activeBonusses[BonusType.Skill_Efficiency]}%",
                [mainWindow.ShieldHealth_Value] = $"{activeBonusses[BonusType.Shield_Health] + activeBonusses[BonusType.Skill_Efficiency]}%",
                [mainWindow.SkillRepair_Value] = $"{activeBonusses[BonusType.Repair_Skills] + activeBonusses[BonusType.Skill_Efficiency]}%",
                [mainWindow.StatusEffects_Value] = $"{activeBonusses[BonusType.Status_Effects] + activeBonusses[BonusType.Skill_Efficiency]}%",
                [mainWindow.ExplosivesDMG_Value] = $"{activeBonusses[BonusType.Explosive_Damage]}%"
            };


            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                foreach(KeyValuePair<Label, string> value in values)
                {
                    value.Key.Content = value.Value;
                }

            });

        }
    }
}
