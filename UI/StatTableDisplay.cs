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
    public static class StatTableDisplay
    {

        public static async Task DisplayBonusesInBoxes(MainWindow mainWindow)
        {
            Dictionary<Label, string> values = new Dictionary<Label, String>();
            //Task to leave room for complex tasks and more responsive design
            //Headshot bonuses: AR=55 LMG=65 Shotgun=45 SMG=50 Rifle=60 MMR=0 Pistol=100
            values[mainWindow.CHC_Value] = $"{activeBonusses[BonusType.Critical_Hit_Chance]}%";
            values[mainWindow.CHD_Value] = $"{activeBonusses[BonusType.Critical_Hit_Damage]}%";
            values[mainWindow.HeadshotDMG_Value] = $"{activeBonusses[BonusType.Headshot_Damage]}%";
            values[mainWindow.DMGooC_Value] = $"{activeBonusses[BonusType.DMG_out_of_Cover]}%";
            values[mainWindow.ArmorDMG_Value] = $"{activeBonusses[BonusType.Damage_to_Armor]}%";
            values[mainWindow.HealthDMG_Value] = $"{activeBonusses[BonusType.Health_Damage]}%";
            values[mainWindow.WeaponHandling_Value] = $"{activeBonusses[BonusType.Weapon_Handling]}%";
            values[mainWindow.Accuracy_Value] = $"{activeBonusses[BonusType.Accuracy] + activeBonusses[BonusType.Weapon_Handling]}%";
            values[mainWindow.Stability_Value] = $"{activeBonusses[BonusType.Stability] + activeBonusses[BonusType.Weapon_Handling]}%";
            values[mainWindow.ReloadSpeed_Value] = $"{activeBonusses[BonusType.Reload_Speed] + activeBonusses[BonusType.Weapon_Handling]}%";
            values[mainWindow.SwapSpeed_Value] = $"{activeBonusses[BonusType.Swap_Speed] + activeBonusses[BonusType.Swap_Speed]}%";

            values[mainWindow.WeaponDMG_Value] = $"{activeBonusses[BonusType.Weapon_Damage]}%";
            values[mainWindow.MMRDMG_Value] = $"{activeBonusses[BonusType.MMR_Damage] + activeBonusses[BonusType.Weapon_Damage]}%";
            values[mainWindow.RifleDMG_Value] = $"{activeBonusses[BonusType.Rifle_Damage] + activeBonusses[BonusType.Weapon_Damage]}%";
            values[mainWindow.SMGDMG_Value] = $"{activeBonusses[BonusType.SMG_Damage] + activeBonusses[BonusType.Weapon_Damage]}%";
            values[mainWindow.LMGDMG_Value] = $"{activeBonusses[BonusType.LMG_Damage] + activeBonusses[BonusType.Weapon_Damage]}%";
            values[mainWindow.ARDMG_Value] = $"{activeBonusses[BonusType.AR_Damage] + activeBonusses[BonusType.Weapon_Damage]}%";
            values[mainWindow.ShotgunDMG_Value] = $"{activeBonusses[BonusType.Shotgun_Damage] + activeBonusses[BonusType.Weapon_Damage]}%";
            values[mainWindow.PistolDMG_Value] = $"{activeBonusses[BonusType.Pistol_Damage] + activeBonusses[BonusType.Weapon_Damage]}%";
            values[mainWindow.SignatureDMG_Value] = $"{activeBonusses[BonusType.Signature_Weapon_Damage] + activeBonusses[BonusType.Signature_Weapon_Damage]}%";
            values[mainWindow.RoFBonus_Value] = $"{activeBonusses[BonusType.Rate_of_Fire]}%";
            values[mainWindow.AmmoCapacity_Value] = $"{activeBonusses[BonusType.Ammo_Capacity]}%";

            values[mainWindow.MaxArmor_Value] = $"{activeBonusses[BonusType.Armor]}";
            values[mainWindow.TotalArmor_Value] = $"{activeBonusses[BonusType.Total_Armor]}%";
            values[mainWindow.HealthOnKill_Value] = $"{activeBonusses[BonusType.Health_on_Kill]}%";
            values[mainWindow.ArmorOnKill_Value] = $"{activeBonusses[BonusType.Armor_on_Kill]}%";
            values[mainWindow.ArmorOnKillSet_Value] = $"{activeBonusses[BonusType.Armor_on_Kill_Stat]}";

            //values[mainWindow.MaxHealth_Value] = $"{activeBonusses[BonusType.???]}";
            values[mainWindow.Health_Value] = $"{activeBonusses[BonusType.Health]}%";
            values[mainWindow.HealthRegen_Value] = $"{activeBonusses[BonusType.Health_Regen]}";
            values[mainWindow.ArmorRegen_Value] = $"{activeBonusses[BonusType.Armor_Regen]}%";
            values[mainWindow.ArmorRegenSet_Value] = $"{activeBonusses[BonusType.Armor_Regen_HPs]}";

            values[mainWindow.SkillTiers_Value] = $"{activeBonusses[BonusType.Skill_Tier]}";
            values[mainWindow.SkillEfficiency_Value] = $"{activeBonusses[BonusType.Skill_Efficiency]}%";
            values[mainWindow.SkillDMG_Value] = $"{activeBonusses[BonusType.Skill_Damage] + activeBonusses[BonusType.Skill_Efficiency]}%";
            values[mainWindow.SkillHaste_Value] = $"{activeBonusses[BonusType.Skill_Haste] + activeBonusses[BonusType.Skill_Efficiency]}%";
            values[mainWindow.SkillDuration_Value] = $"{activeBonusses[BonusType.Skill_Duration] + activeBonusses[BonusType.Skill_Efficiency]}%";

            values[mainWindow.SkillHealth_Value] = $"{activeBonusses[BonusType.Skill_Health] + activeBonusses[BonusType.Skill_Efficiency]}%";
            values[mainWindow.ShieldHealth_Value] = $"{activeBonusses[BonusType.Shield_Health] + activeBonusses[BonusType.Skill_Efficiency]}%";
            values[mainWindow.SkillRepair_Value] = $"{activeBonusses[BonusType.Repair_Skills] + activeBonusses[BonusType.Skill_Efficiency]}%";
            values[mainWindow.StatusEffects_Value] = $"{activeBonusses[BonusType.Status_Effects] + activeBonusses[BonusType.Skill_Efficiency]}%";
            values[mainWindow.ExplosivesDMG_Value] = $"{activeBonusses[BonusType.Explosive_Damage]}%";


            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                foreach(KeyValuePair<Label, string> value in values)
                {
                    value.Key.Content = value.Value;
                }

            });

        }


        public static void DisplayBonus(Label box, string str)
        {
            box.Content = str;
        }
    }
}
