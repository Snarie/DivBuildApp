﻿using DivBuildApp.Data.Tables;
using DivBuildApp.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;


namespace DivBuildApp.BonusControl
{
    internal static class ActiveBonuses
    {
        public static void Initialize()
        {
            ResetBonuses();
        }
        static ActiveBonuses()
        {
            //StatValueLabelControl.ValueSet += HandleValueSet;
            GearHandler.GearSet += HandleGearSet;
            GearHandler.GearAttributeSet += HandleGearAttributeSet;
            WeaponHandler.WeaponSet += HandleWeaponSet;
            WeaponHandler.WeaponModsSet += HandleWeaponModsSet;
            WeaponHandler.WeaponAttributeSet += HandleWeaponAttributeSet;
            WeaponHandler.WeaponExpertieceSet += HandleWeaponExpertieceSet;
            WeaponHandler.EquippedWeaponSet += HandleEquippedWeaponSet;
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
        private static async void HandleWeaponSet(object sender, WeaponEventArgs e)
        {
            await CalculateWeaponDefaultBonuses();
        }
        private static async void HandleWeaponModsSet(object sender, EventArgs e)
        {
            await CalculateWeaponModsBonuses();
        }
        private static async void HandleWeaponAttributeSet(object sender, EventArgs e)
        {
            await CalculateWeaponAttributes();
        }
        private static async void HandleWeaponExpertieceSet(object sender, EventArgs e)
        {
            await CalculateExpertieceBonuses();
        }
        private static async void HandleEquippedWeaponSet(object sender, EventArgs e)
        {
            await CalculateWeaponDefaultBonuses();
            await CalculateWeaponAttributes();
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
        private static readonly List<BonusSource> weaponModsBonuses = new List<BonusSource>();
        private static readonly List<BonusSource> weaponAttributeBonuses = new List<BonusSource>();
        private static readonly List<BonusSource> watchBonuses = new List<BonusSource>();
        private static readonly List<BonusSource> weaponDefaultBonuses = new List<BonusSource>();

        public static List<BonusSource> activeBonusSources = new List<BonusSource>();
        public static Dictionary<BonusType, double> activeBonuses = new Dictionary<BonusType, double>();



        private static readonly SynchronizedTaskRunner WeaponDefaultTaskRunner = new SynchronizedTaskRunner(TimeSpan.FromSeconds(0.1));
        private static async Task CalculateWeaponDefaultBonuses()
        {
            await WeaponDefaultTaskRunner.ExecuteAsync(async () =>
            {
                weaponDefaultBonuses.Clear();
                Weapon weapon = WeaponHandler.GetEquippedWeapon();
                weaponDefaultBonuses.Add(new BonusSource("Weapon Base Headshot", WeaponHandler.GetWeaponHeadshotDefault(weapon)));
                await CalculateBonuses();
            });
        }

        private static readonly SynchronizedTaskRunner BrandBonusesTaskRunner = new SynchronizedTaskRunner(TimeSpan.FromSeconds(0.1));
        private static async Task CalculateBrandBonuses()
        {
            await BrandBonusesTaskRunner.ExecuteAsync( async () =>
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
                Console.WriteLine(localEquippedDict.Values);

                // Apply brand set bonuses
                foreach(KeyValuePair<string, int> kvp in brandLevels)
                {
                    List<Bonus> bonuses = new List<Bonus>();
                    for(int i = 1; i <= kvp.Value; i++)
                    {
                        List<Bonus> brandBonus = BrandSets.GetBrandBonus(kvp.Key, i);
                        foreach(Bonus bonus in brandBonus)
                        {
                            bonuses.Add(bonus);
                        }
                    }
                    foreach(Bonus bonus in bonuses)
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
                await CalculateBonuses();
            });
        }

        private static readonly SynchronizedTaskRunner WeaponModsTaskRunner = new SynchronizedTaskRunner(TimeSpan.FromSeconds(0.1));
        private static async Task CalculateWeaponModsBonuses()
        {
            await WeaponModsTaskRunner.ExecuteAsync(async () =>
            {
                weaponModsBonuses.Clear();
                Weapon weapon = WeaponHandler.GetEquippedWeapon();
                foreach(WeaponMod weaponMod in weapon.WeaponMods)
                {
                    foreach(Bonus bonus in weaponMod.Attributes)
                    {
                        weaponModsBonuses.Add(new BonusSource(weapon.Name + "=>" + weaponMod.Name, bonus));
                    }
                }
                await CalculateBonuses();
            });
        }

        private static readonly SynchronizedTaskRunner WeaponAttributesTaskRunner = new SynchronizedTaskRunner(TimeSpan.FromSeconds(0.1));
        private static async Task CalculateWeaponAttributes()
        {
            await WeaponAttributesTaskRunner.ExecuteAsync(async () =>
            {
                weaponAttributeBonuses.Clear();
                Weapon weapon = WeaponHandler.GetEquippedWeapon();
                foreach(Bonus bonus in weapon.StatAttributes)
                {
                    weaponAttributeBonuses.Add(new BonusSource(weapon.Name, bonus));
                }
                await CalculateBonuses();
            });
        }


        private static readonly SynchronizedTaskRunner ExpertieceBonusesTaskRunner = new SynchronizedTaskRunner(TimeSpan.FromSeconds(0.1));
        private static async Task CalculateExpertieceBonuses()
        {
            await ExpertieceBonusesTaskRunner.ExecuteAsync(async () =>
            {
                expertieceBonuses.Clear();
                expertieceBonuses.Add(new BonusSource("Gear Expertiecem", new Bonus(BonusType.Armor, ItemArmorControl.GetExpertieceArmorValue())));
                expertieceBonuses.Add(new BonusSource("Weapon Expertiece", new Bonus(BonusType.Weapon_Damage, WeaponHandler.GetEquippedWeapon().Expertiece)));
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

                // Specialization 15% on 3 weapon types (so all 3 equipped weapons for now)
                BonusSource specializationBonus = new BonusSource("Specialization", new Bonus(BonusType.Weapon_Damage, 15.0));
                activeBonusSources.Add(specializationBonus);
                activeBonuses[specializationBonus.Bonus.BonusType] += specializationBonus.Bonus.BonusValue;

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
                foreach(BonusSource bonusSource in weaponAttributeBonuses)
                {
                    activeBonusSources.Add(bonusSource);
                    activeBonuses[bonusSource.Bonus.BonusType] += bonusSource.Bonus.BonusValue;
                }
                foreach(BonusSource bonusSource in weaponModsBonuses)
                {
                    activeBonusSources.Add(bonusSource);
                    activeBonuses[bonusSource.Bonus.BonusType] += bonusSource.Bonus.BonusValue;
                }
                foreach(BonusSource bonusSource in weaponDefaultBonuses)
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
