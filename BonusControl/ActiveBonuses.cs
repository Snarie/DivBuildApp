using DivBuildApp.Data.Tables;
using DivBuildApp.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace DivBuildApp.BonusControl
{
    internal static class ActiveBonuses
    {
        static ActiveBonuses()
        {
            //StatValueLabelControl.ValueSet += HandleValueSet;
            GearHandler.GearSet += HandleGearSet;
            GearHandler.GearAttributeSet += HandleGearAttributeSet;
            SHDWatch.WatchSet += HandleWatchSet;
            ItemArmorControl.ItemArmorSet += HandleItemArmorSet;
        }

        public static event EventHandler CalculateBonusesSet;
        private static void OnCalculateBonuses()
        {
            CalculateBonusesSet?.Invoke(null, EventArgs.Empty);
        }

        private static async void HandleGearSet(object sender, GridEventArgs e)
        {
            await CalculateBrandBonuses();
        }
        private static async void HandleGearAttributeSet(object sender, EventArgs e)
        {
            await CalculateStatAttributes();
        }
        private static async void HandleWatchSet(object sender, EventArgs e)
        {
            await CalculateWatchBonuses();
        }
        private static async void HandleItemArmorSet(object sender, EventArgs e)
        {
            await CalculateExpertieceBonuses();
        }


        private static readonly List<BonusSource> brandSetBonuses = new List<BonusSource>();
        private static readonly List<BonusSource> expertieceBonuses = new List<BonusSource>();
        private static readonly List<BonusSource> statAttributeBonuses = new List<BonusSource>();
        private static readonly List<BonusSource> watchBonuses = new List<BonusSource>();

        public static List<BonusSource> activeBonusSources = new List<BonusSource>();
        public static Dictionary<BonusType, double> activeBonuses = new Dictionary<BonusType, double>();



        private static readonly SynchronizedTaskRunner BrandBonusesTaskRunner = new SynchronizedTaskRunner(TimeSpan.FromSeconds(0.1));
        private static async Task CalculateBrandBonuses()
        {
            await BrandBonusesTaskRunner.ExecuteAsync(() =>
            {
                brandSetBonuses.Clear();
                Dictionary<string, int> brandLevels = new Dictionary<string, int>();
                Dictionary<ItemType, Gear> localEquippedDict = GearHandler.equippedItemDict;

                // First pass: Collect brand levels
                foreach (Gear equippedItem in localEquippedDict.Values)
                {
                    string brandName = equippedItem.BrandName;
                    if (brandLevels.ContainsKey(brandName))
                    {
                        brandLevels[brandName]++;
                    }
                    else
                    {
                        brandLevels.Add(brandName, 1);
                    }
                }

                // Apply brand set bonuses
                foreach(KeyValuePair<string, int> kvp in brandLevels)
                {
                    foreach(Bonus bonus in BrandSets.GetBrandBonus(kvp.Key, kvp.Value))
                    {
                        brandSetBonuses.Add(new BonusSource("Brand Set", new Bonus(bonus.BonusType, bonus.Value)));
                    }
                }

                // Special case fore "Ninjabike Backpack"
                if (GearHandler.GearFromSlot(ItemType.Backpack)?.Name == "NinjaBike Backpack")
                {
                    var keysToUpdate = new List<string>(brandLevels.Keys);
                    foreach (string key in keysToUpdate)
                    {
                        brandLevels[key]++;
                        foreach (Bonus bonus in BrandSets.GetBrandBonus(key, brandLevels[key]))
                        {
                            brandSetBonuses.Add(new BonusSource("NinjaBike Backpack", new Bonus(bonus.BonusType, bonus.Value)));
                        }
                    }
                }
                _ = CalculateBonuses();

                // Return a completed task because lambda must return a Task
                return Task.CompletedTask;
            });

            
        }


        private static readonly SynchronizedTaskRunner ExpertieceBonusesTaskRunner = new SynchronizedTaskRunner(TimeSpan.FromSeconds(0.1));
        private static async Task CalculateExpertieceBonuses()
        {
            await ExpertieceBonusesTaskRunner.ExecuteAsync(async () =>
            {
                expertieceBonuses.Clear();
                expertieceBonuses.Add(new BonusSource("Gear Expertiecem", new Bonus(BonusType.Armor, ItemArmorControl.GetExpertieceArmorValue())));
                await CalculateBonuses();
            });
        }


        private static readonly SynchronizedTaskRunner StatAttributeBonusesTaskRunner = new SynchronizedTaskRunner(TimeSpan.FromSeconds(0.1));
        private static async Task CalculateStatAttributes()
        {
            await StatAttributeBonusesTaskRunner.ExecuteAsync(async () =>
            {
                statAttributeBonuses.Clear();
                foreach (Gear gear in GearHandler.equippedItemDict.Values)
                {
                    foreach (Bonus bonus in gear.StatAttributes)
                    {
                        statAttributeBonuses.Add(new BonusSource(gear.Name, bonus));
                    }
                }
                await CalculateBonuses();
            });
        }


        private static readonly SynchronizedTaskRunner WatchBonusesTaskRunner = new SynchronizedTaskRunner(TimeSpan.FromSeconds(0.1));
        private static async Task CalculateWatchBonuses()
        {
            await WatchBonusesTaskRunner.ExecuteAsync(async () =>
            {
                watchBonuses.Clear();
                foreach (Bonus bonus in SHDWatch.WatchBonuses)
                {
                    watchBonuses.Add(new BonusSource("Watch Bonus", new Bonus(bonus.BonusType, bonus.Value)));
                }
                await CalculateBonuses();
            });
        }


        private static void ResetBonuses()
        {
            //Set all bonusses back to being at 0
            activeBonuses = new Dictionary<BonusType, double>();
            foreach (BonusType bonusTypes in Enum.GetValues(typeof(BonusType)))
            {
                activeBonuses[bonusTypes] = 0;
            }
        }

        private static readonly SemaphoreSlim CalculateSemaphore = new SemaphoreSlim(1);

        private static async Task CalculateBonuses()
        {
            await CalculateSemaphore.WaitAsync();
            try
            {
                ResetBonuses();
                activeBonusSources.Clear();
                foreach (BonusSource bonusSource in brandSetBonuses)
                {
                    activeBonusSources.Add(bonusSource);
                    activeBonuses[bonusSource.Bonus.BonusType] += bonusSource.Bonus.BonusValue;
                }

                foreach (BonusSource bonusSource in statAttributeBonuses)
                {
                    activeBonusSources.Add(bonusSource);
                    activeBonuses[bonusSource.Bonus.BonusType] += bonusSource.Bonus.BonusValue;
                }

                foreach (BonusSource bonusSource in watchBonuses)
                {
                    activeBonusSources.Add(bonusSource);
                    activeBonuses[bonusSource.Bonus.BonusType] += bonusSource.Bonus.BonusValue;
                }

                foreach (BonusSource bonusSource in expertieceBonuses)
                {
                    activeBonusSources.Add(bonusSource);
                    activeBonuses[bonusSource.Bonus.BonusType] += bonusSource.Bonus.BonusValue;
                }

                activeBonuses[BonusType.Armor] = Math.Floor(activeBonuses[BonusType.Armor] * (100 + activeBonuses[BonusType.Total_Armor]) / 100);

                OnCalculateBonuses();
            }
            finally
            {
                CalculateSemaphore.Release();
            }
            
        }
    }
}
