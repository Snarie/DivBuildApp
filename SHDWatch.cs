using DivBuildApp.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DivBuildApp
{
    internal static class SHDWatch
    {
        public static void Initialize()
        {
            //Set the eventHandlers
        }
        public static event EventHandler WatchSet;
        private static void OnWatchSet()
        {
            WatchSet?.Invoke(null, EventArgs.Empty);
        }
        public static int WatchLevel { get;set; }

        public static List<Bonus> WatchBonuses = new List<Bonus>();

        public static void SetWatchBonuses(string level)
        {
            WatchLevel = int.Parse((string.IsNullOrEmpty(level)) ? "0" : level);
            double watchLevelPerc = (double)WatchLevel / 1000;
            if (watchLevelPerc > 1) watchLevelPerc = 1;

            WatchBonuses.Clear();

            WatchBonuses.Add(new Bonus(BonusType.Critical_Hit_Chance, 10 * watchLevelPerc));
            WatchBonuses.Add(new Bonus(BonusType.Critical_Hit_Damage, 10 * watchLevelPerc));
            WatchBonuses.Add(new Bonus(BonusType.Headshot_Damage, 10 * watchLevelPerc));
            WatchBonuses.Add(new Bonus(BonusType.Weapon_Damage, 10 * watchLevelPerc));

            WatchBonuses.Add(new Bonus(BonusType.Ammo_Capacity, 20 * watchLevelPerc));
            WatchBonuses.Add(new Bonus(BonusType.Accuracy, 10 * watchLevelPerc));
            WatchBonuses.Add(new Bonus(BonusType.Stability, 10 * watchLevelPerc));
            WatchBonuses.Add(new Bonus(BonusType.Reload_Speed, 10 * watchLevelPerc));

            WatchBonuses.Add(new Bonus(BonusType.Total_Armor, 10 * watchLevelPerc));
            WatchBonuses.Add(new Bonus(BonusType.Health, 10 * watchLevelPerc));
            WatchBonuses.Add(new Bonus(BonusType.Hazard_Protection, 10 * watchLevelPerc));
            WatchBonuses.Add(new Bonus(BonusType.Explosive_Resistance, 10 * watchLevelPerc));
            WatchBonuses.Add(new Bonus(BonusType.Skill_Haste, 10 * watchLevelPerc));
            WatchBonuses.Add(new Bonus(BonusType.Skill_Damage, 10 * watchLevelPerc));
            WatchBonuses.Add(new Bonus(BonusType.Repair_Skills, 10 * watchLevelPerc));
            WatchBonuses.Add(new Bonus(BonusType.Skill_Duration, 10 * watchLevelPerc));

            OnWatchSet();
        }
        
        public static async void SetWatchBonusesAsync(string level)
        {
            WatchLevel = int.Parse((string.IsNullOrEmpty(level)) ? "0" : level);
            double watchLevelPerc = (double)WatchLevel / 1000;
            if (watchLevelPerc > 1) watchLevelPerc = 1;

            WatchBonuses.Clear();

            WatchBonuses.Add(new Bonus(BonusType.Critical_Hit_Chance, 10 * watchLevelPerc));
            WatchBonuses.Add(new Bonus(BonusType.Critical_Hit_Damage, 10 * watchLevelPerc));
            WatchBonuses.Add(new Bonus(BonusType.Headshot_Damage, 10 * watchLevelPerc));
            WatchBonuses.Add(new Bonus(BonusType.Weapon_Damage, 10 * watchLevelPerc));

            WatchBonuses.Add(new Bonus(BonusType.Ammo_Capacity, 20 * watchLevelPerc));
            WatchBonuses.Add(new Bonus(BonusType.Accuracy, 10 * watchLevelPerc));
            WatchBonuses.Add(new Bonus(BonusType.Stability, 10 * watchLevelPerc));
            WatchBonuses.Add(new Bonus(BonusType.Reload_Speed, 10 * watchLevelPerc));

            WatchBonuses.Add(new Bonus(BonusType.Total_Armor, 10 * watchLevelPerc));
            WatchBonuses.Add(new Bonus(BonusType.Health, 10 * watchLevelPerc));
            WatchBonuses.Add(new Bonus(BonusType.Hazard_Protection, 10 * watchLevelPerc));
            WatchBonuses.Add(new Bonus(BonusType.Explosive_Resistance, 10 * watchLevelPerc));
            WatchBonuses.Add(new Bonus(BonusType.Skill_Haste, 10 * watchLevelPerc));
            WatchBonuses.Add(new Bonus(BonusType.Skill_Damage, 10 * watchLevelPerc));
            WatchBonuses.Add(new Bonus(BonusType.Repair_Skills, 10 * watchLevelPerc));
            WatchBonuses.Add(new Bonus(BonusType.Skill_Duration, 10 * watchLevelPerc));

            OnWatchSet();
        }
    }
}
