using DivBuildApp.Data.CsvFormats;
using DivBuildApp.Data.Tables;
using DivBuildApp.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DivBuildApp
{
    internal static class WeaponHandler
    {
        public static void Initialize()
        {
            StatValueLabelControl.WeaponValueSet += HandleWeaponValueSet;
            // Set
        }
        public static event EventHandler<WeaponEventArgs> WeaponSet;
        public static event EventHandler WeaponModsSet;
        public static event EventHandler WeaponAttributeSet;
        public static event EventHandler WeaponExpertieceSet;
        public static event EventHandler EquippedWeaponSet;
        private static void OnWeaponSet(WeaponEventArgs e)
        {
            if(e.Grid.Box.SelectedItem is WeaponListFormat wlf)
            {
                WeaponStatsFormat wsf = wlf.Stats();
                WeaponSet?.Invoke(null, e);
            }
        }
        private static void OnWeaponModsSet()
        {
            WeaponModsSet?.Invoke(null, EventArgs.Empty);
        }
        private static void OnWeaponAttributeSet()
        {
            WeaponAttributeSet?.Invoke(null, EventArgs.Empty);
        }
        private static void OnWeaponExpertieceSet()
        {
            WeaponExpertieceSet?.Invoke(null, EventArgs.Empty);
        }
        private static void OnEquippedWeaponSet()
        {
            EquippedWeaponSet?.Invoke(null, EventArgs.Empty);
        }
        private static void HandleWeaponValueSet(object sender,  WeaponEventArgs e)
        {
            SetWeaponStatAttributes(e);
        }

        private static readonly Dictionary<string, double> weaponVarientHeadshot = new Dictionary<string, double>
        {
            {"MG5", 85.0 },
            {"SPAS-12", 25.0},
            {"Vector", 50.0 }
        };
        private static readonly Dictionary<WeaponType, double> weaponTypeHeadshot = new Dictionary<WeaponType, double>
        {
            {WeaponType.AR, 55.0 },
            {WeaponType.LMG, 65.0 },
            {WeaponType.MMR, 0.0 },
            {WeaponType.Pistol, 100.0 },
            {WeaponType.Rifle, 60.0 },
            {WeaponType.Shotgun, 45.0 },
            {WeaponType.SMG, 50 }
        };

        public static WeaponSlot equippedWeaponSlot;
        public static Dictionary<WeaponSlot, Weapon> equippedWeapons = new Dictionary<WeaponSlot, Weapon>();


        public static Bonus GetWeaponHeadshotDefault(Weapon weapon)
        {
            if (weaponVarientHeadshot.ContainsKey(weapon.Varient))
            {
                return new Bonus(BonusType.Headshot_Damage, weaponVarientHeadshot[weapon.Varient]);
            }
            return new Bonus(BonusType.Headshot_Damage, weaponTypeHeadshot[weapon.Type]);
        }
        private static Weapon CreateWeapon(WeaponEventArgs e)
        {
            if (e.Grid.Box.SelectedItem is WeaponListFormat wlf)
            {
                return new Weapon(wlf.Name, wlf.Varient, wlf.Type, wlf.Rarity, new Bonus[] { }, new WeaponMod[] { }, "", 0);
            }
            return null;
        }
        public static void SetEquippedWepaonSlot(WeaponSlot slot)
        {
            equippedWeaponSlot = slot;
            OnEquippedWeaponSet();
        }
        public static Weapon GetEquippedWeapon()
        {
            return equippedWeapons[equippedWeaponSlot];
        }
        
        public static void SetWeaponExpertiece(WeaponEventArgs e)
        {
            ComboBox expertieceBox = e.Grid.Expertiece;
            equippedWeapons[e.Slot].Expertiece = expertieceBox.SelectedIndex;
            OnWeaponExpertieceSet();
        }

        public static void SetWeaponMods(WeaponEventArgs e)
        {
            ComboBox[] modBoxes = new ComboBox[] { e.Grid.OpticalRail, e.Grid.Magazine, e.Grid.Underbarrel, e.Grid.Muzzle };
            List<WeaponMod> mods = new List<WeaponMod>();
            for (int i = 0; i < 4; i++)
            {
                if (modBoxes[i].SelectedItem is WeaponMod wm)
                {
                    mods.Add(wm);
                }
            }
            if (equippedWeapons.ContainsKey(e.Slot))
            {
                equippedWeapons[e.Slot].WeaponMods = mods.ToArray();
            }
            OnWeaponModsSet();
            
        }
        public static void SetWeaponStatAttributes(WeaponEventArgs e)
        {
            ComboBox[] statBoxes = e.Grid.StatBoxes;
            Label[] statValues = e.Grid.StatValues;
            List<Bonus> bonusList = new List<Bonus>();
            for (int i = 0; i < 3; i++)
            {
                if (statBoxes[i].SelectedItem is BonusDisplay)
                {
                    if (statValues[i].DataContext is Bonus bonus)
                    {
                        bonusList.Add(new Bonus(bonus.BonusType, bonus.BonusValue));
                    }
                    else
                    {
                        Task.Run(() => Logger.LogEvent($"{statValues[i].Name} is not a Bonus"));
                    }
                }
            }
            if(equippedWeapons.ContainsKey(e.Slot))
            {
                equippedWeapons[e.Slot].StatAttributes = bonusList.ToArray();
            }

            OnWeaponAttributeSet();
        }
        

        private static readonly SynchronizedGroupedTaskRunner<WeaponSlot> WeaponSetTaskRunner = new SynchronizedGroupedTaskRunner<WeaponSlot>(TimeSpan.FromSeconds(0.05));
        public static async void SetEquippedWeaponListAsync(WeaponEventArgs e)
        {
            await WeaponSetTaskRunner.ExecuteTaskAsync(e.Slot, () =>
            {
                if (equippedWeapons.ContainsKey(e.Slot))
                {
                    equippedWeapons[e.Slot] = CreateWeapon(e);
                }
                else
                {
                    equippedWeapons.Add(e.Slot, CreateWeapon(e));
                }
                OnWeaponSet(e); //Notify other parts of the program

                // Return a completed task because lambda must return a Task
                return Task.CompletedTask;
            });
        }

    }
}
