using DivBuildApp.Data.Tables;
using DivBuildApp.UI;
using System;
using System.Collections.Generic;
using System.Linq;
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

        private static void HandleGearSet(object sender, GridEventArgs e)
        {

            Task.Run(() => CalculateBrandBonuses());
            Task.Run(() => Logger.LogEvent("GearHandler.GearSet"));
        }
        private static void HandleGearAttributeSet(object sender, EventArgs e)
        {
            Task.Run(() => CalculateStatAttributes());
            Task.Run(() => Logger.LogEvent("GearHandler.GearAttributeSet"));
        }
        private static void HandleWatchSet(object sender, EventArgs e)
        {
            Task.Run(() => CalculateWatchBonuses());
            Task.Run(() => Logger.LogEvent("SHDWatch.WatchSet"));
        }
        private static void HandleItemArmorSet(object sender, EventArgs e)
        {
            Task.Run(() => CalculateExpertieceBonuses());
            Task.Run(()=> Logger.LogEvent("ItemArmorControl.ItemArmorSet"));
        }


        private static readonly List<BonusSource> brandSetBonuses = new List<BonusSource>();
        private static readonly List<BonusSource> expertieceBonuses = new List<BonusSource>();
        private static readonly List<BonusSource> statAttributeBonuses = new List<BonusSource>();
        private static readonly List<BonusSource> watchBonuses = new List<BonusSource>();

        public static Dictionary<BonusType, double> activeBonuses = new Dictionary<BonusType, double>();


        private static readonly SemaphoreSlim BrandBonusesSemaphore = new SemaphoreSlim(1);
        private static void CalculateBrandBonuses()
        {
            brandSetBonuses.Clear();
            Dictionary<string, int> brandLevels = new Dictionary<string, int>();
            foreach (Gear equippedItem in GearHandler.equippedItemDict.Values)
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

                //BrandSet Bonuses
                foreach (Bonus bonus in BrandSets.GetBrandBonus(brandName, brandLevels[brandName]))
                {
                    brandSetBonuses.Add(new BonusSource("Brand Set", new Bonus(bonus.BonusType, bonus.Value)));
                    //activeBonusses[bonus.BonusType] += bonus.Value;
                }

            }

            if (GearHandler.GearFromSlot(ItemType.Backpack) != null)
            {
                if (GearHandler.GearFromSlot(ItemType.Backpack).Name == "NinjaBike Backpack")
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
            }
            Task.Run(() => CalculateBonuses());
        }

        private static readonly SemaphoreSlim ExpertieceBonusesSemaphore = new SemaphoreSlim(1);
        private static async void CalculateExpertieceBonuses()
        {
            await ExpertieceBonusesSemaphore.WaitAsync();
            try
            {
                expertieceBonuses.Clear();
                expertieceBonuses.Add(new BonusSource("Gear Expertiece", new Bonus(BonusType.Armor, ItemArmorControl.GetExpertieceArmorValue())));
                await CalculateBonuses();
            }
            finally
            {
                ExpertieceBonusesSemaphore.Release();
            }
        }

        private static readonly SemaphoreSlim StatAttributesSemaphore = new SemaphoreSlim(1);
        private static async void CalculateStatAttributes()
        {
            await StatAttributesSemaphore.WaitAsync();
            try
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
            }
            finally
            {
                StatAttributesSemaphore.Release();
            }
        }

        private static readonly SemaphoreSlim WatchBonusesSemaphore = new SemaphoreSlim(1);
        private static async void CalculateWatchBonuses()
        {
            await WatchBonusesSemaphore.WaitAsync();
            try
            {
                watchBonuses.Clear();
                foreach (Bonus bonus in SHDWatch.WatchBonuses)
                {
                    watchBonuses.Add(new BonusSource("Watch Bonus", new Bonus(bonus.BonusType, bonus.Value)));
                }
                await CalculateBonuses();
            }
            finally
            {
                WatchBonusesSemaphore.Release();
            }
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
            Console.WriteLine("Before Semaphore");
            await CalculateSemaphore.WaitAsync();
            try
            {
                Console.WriteLine("During Semaphore");
                ResetBonuses();
                Console.WriteLine("Armor(Reset) = " + activeBonuses[BonusType.Armor]);

                foreach (Bonus bonus in brandSetBonuses.Select(b => b.Bonus))
                {
                    activeBonuses[bonus.BonusType] += bonus.BonusValue;
                }
                Console.WriteLine("Armor(BrandSet) = " + activeBonuses[BonusType.Armor]);

                foreach (Bonus bonus in statAttributeBonuses.Select(b => b.Bonus))
                {
                    activeBonuses[bonus.BonusType] += bonus.BonusValue;
                }
                Console.WriteLine("Armor(Attribute) = " + activeBonuses[BonusType.Armor]);

                foreach (Bonus bonus in watchBonuses.Select(b => b.Bonus))
                {
                    activeBonuses[bonus.BonusType] += bonus.BonusValue;
                }
                Console.WriteLine("Armor(Watch) = " + activeBonuses[BonusType.Armor]);

                foreach (Bonus bonus in expertieceBonuses.Select(b => b.Bonus))
                {
                    Console.WriteLine("  added = " + bonus.BonusValue);
                    activeBonuses[bonus.BonusType] += bonus.BonusValue;
                }
                Console.WriteLine("Armor(Expertiece) = " + activeBonuses[BonusType.Armor]);
                activeBonuses[BonusType.Armor] = Math.Floor(activeBonuses[BonusType.Armor] * (100 + activeBonuses[BonusType.Total_Armor]) / 100);

                Console.WriteLine("Armor(Mult) = " + activeBonuses[BonusType.Armor]);
                OnCalculateBonuses();
            }
            finally
            {
                CalculateSemaphore.Release();
            }
            
        }
    }
}
