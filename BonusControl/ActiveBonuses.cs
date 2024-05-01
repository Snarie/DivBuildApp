using DivBuildApp.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DivBuildApp.BonusControl
{
    internal class ActiveBonuses
    {
        static ActiveBonuses()
        {
            //StatValueLabelControl.ValueSet += HandleValueSet;
            GearHandler.GearSet += HandleGearSet;
            GearHandler.GearAttributeSet += HandleGearAttributeSet;
            SHDWatch.WatchSet += HandleWatchSet;
            ItemArmorControl.ItemArmorSet += HandleItemArmorSet;
        }
        private static void HandleGearSet(object sender, EventArgs e)
        {
            CalculateBrandBonues();
            Console.WriteLine("ActiveBonuses noticed GearHandler.GearSet");
        }
        private static void HandleGearAttributeSet(object sender, EventArgs e)
        {
            CalculateStatAttributes();
            Console.WriteLine("ActiveBonuses noticed GearHandler.GearAttributeSet");
        }
        private static void HandleWatchSet(object sender, EventArgs e)
        {
            CalculateWatchBonuses();
            Console.WriteLine("ActiveBonuses noticed SHDWatch.WatchSet");
        }
        private static void HandleItemArmorSet(object sender, EventArgs e)
        {
            CalculateExpertieceBonuses();
            Console.WriteLine("ActiveBonuses noticed ItemArmorControl.ItemArmorSet");
        }


        private static readonly List<BonusSource> brandSetBonuses = new List<BonusSource>();
        private static readonly List<BonusSource> expertieceBonuses = new List<BonusSource>();
        private static readonly List<BonusSource> statAttributeBonuses = new List<BonusSource>();
        private static readonly List<BonusSource> watchBonuses = new List<BonusSource>();

        public static Dictionary<BonusType, double> activeBonuses = new Dictionary<BonusType, double>();


        private static void CalculateBrandBonues()
        {
            brandSetBonuses.Clear();
            Dictionary<string, int> brandLevels = new Dictionary<string, int>();
            foreach (Gear equippedItem in GearHandler.equippedItemList)
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
                foreach (Bonus bonus in BonusHandler.GetBrandBonus(brandName, brandLevels[brandName]))
                {
                    brandSetBonuses.Add(new BonusSource("Brand Set", new Bonus(bonus.BonusType, bonus.Value)));
                    //activeBonusses[bonus.BonusType] += bonus.Value;
                }

            }

            if (GearHandler.GearFromSlot(ItemType.Backpack) != null)
            {
                if (GearHandler.GearFromSlot(ItemType.Backpack).Name == "NinjaBike Backpack")
                {
                    Console.WriteLine("Correct");
                    var keysToUpdate = new List<string>(brandLevels.Keys);
                    foreach (string key in keysToUpdate)
                    {
                        brandLevels[key]++;
                        foreach (Bonus bonus in BonusHandler.GetBrandBonus(key, brandLevels[key]))
                        {
                            brandSetBonuses.Add(new BonusSource("NinjaBike Backpack", new Bonus(bonus.BonusType, bonus.Value)));
                        }
                    }
                }
            }
        }

        private static void CalculateExpertieceBonuses()
        {
            expertieceBonuses.Clear();
            expertieceBonuses.Add(new BonusSource("Gear Expertiece", new Bonus(BonusType.Armor, ItemArmorControl.GetExpertieceArmorValue())));
        }

        private static void CalculateStatAttributes()
        {
            statAttributeBonuses.Clear();
            foreach (Gear gear in GearHandler.equippedItemList)
            {
                foreach (Bonus bonus in gear.StatAttributes)
                {
                    statAttributeBonuses.Add(new BonusSource(gear.Name, bonus));
                }
            }
        }

        private static void CalculateWatchBonuses()
        {
            watchBonuses.Clear();
            foreach (Bonus bonus in SHDWatch.WatchBonuses)
            {
                watchBonuses.Add(new BonusSource("Watch Bonus", new Bonus(bonus.BonusType, bonus.Value)));
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

        public static void CalculateBonuses()
        {
            ResetBonuses();

            foreach (Bonus bonus in brandSetBonuses.Select(b => b.Bonus))
            {
                activeBonuses[bonus.BonusType] += bonus.BonusValue;
            }

            foreach (Bonus bonus in statAttributeBonuses.Select(b => b.Bonus))
            {
                activeBonuses[bonus.BonusType] += bonus.BonusValue;
            }

            foreach (Bonus bonus in watchBonuses.Select(b => b.Bonus))
            {
                activeBonuses[bonus.BonusType] += bonus.BonusValue;
            }

            foreach (Bonus bonus in expertieceBonuses.Select(b => b.Bonus))
            {
                activeBonuses[bonus.BonusType] += bonus.BonusValue;
            }

            activeBonuses[BonusType.Armor] = Math.Floor(activeBonuses[BonusType.Armor] * (100 + activeBonuses[BonusType.Total_Armor]) / 100);

        }
    }
}
