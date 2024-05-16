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
        public static event EventHandler WeaponAttributeSet;
        public static event EventHandler EquippedWeaponSet;
        private static void OnWeaponSet(WeaponEventArgs e)
        {
            if(e.Grid.Box.SelectedItem is WeaponListFormat wlf)
            {
                WeaponStatsFormat wsf = wlf.Stats();
                e.Grid.Damage.Content = wsf.Damage;
                e.Grid.RPM.Content = wsf.RPM;
                e.Grid.MagazineSize.Content = wsf.MagazineSize;
            }
            WeaponSet?.Invoke(null, e);
        }
        private static void OnWeaponAttributeSet()
        {
            WeaponAttributeSet?.Invoke(null, EventArgs.Empty);
        }
        private static void OnEquippedWeaponSet()
        {
            EquippedWeaponSet?.Invoke(null, EventArgs.Empty);
        }

        private static void HandleWeaponValueSet(object sender,  WeaponEventArgs e)
        {
            SetWeaponStatAttributes(e);
        }

        public static WeaponSlot equippedWeaponSlot;
        public static Dictionary<WeaponSlot, Weapon> equippedWeapons = new Dictionary<WeaponSlot, Weapon>();

        private static Weapon CreateWeapon(WeaponEventArgs e)
        {
            if (e.Grid.Box.SelectedItem is WeaponListFormat wlf)
            {
                return new Weapon(wlf.Name, wlf.Varient, wlf.Type, wlf.Rarity, new Bonus[] { }, new WeaponMod[] { }, "");
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
