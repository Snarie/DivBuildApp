using DivBuildApp.Data.CsvFormats;
using DivBuildApp.Data.Tables;
using DivBuildApp.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DivBuildApp
{
    internal static class WeaponHandler
    {
        public static void Initialize()
        {
            // Set
        }

        public static event EventHandler<WeaponEventArgs> WeaponSet;
        private static void OnWeaponSet(WeaponEventArgs e)
        {
            if(e.Grid.Box.SelectedItem is WeaponListFormat wlf)
            {
                WeaponStatsFormat wsf = WeaponStats.GetWeaponStats(wlf.Name);
                e.Grid.Damage.Content = wsf.Damage;
                e.Grid.RPM.Content = wsf.RPM;
                e.Grid.MagazineSize.Content = wsf.MagazineSize;
            }
            WeaponSet?.Invoke(null, e);
        }

        private static Dictionary<string, Weapon> equippedWeapons = new Dictionary<string, Weapon>();
        private static async Task CreateGear(WeaponEventArgs e)
        {
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                if (e.Grid.Box.SelectedItem is WeaponListFormat wlf) 
                {
                    new Weapon(wlf.Name, wlf.Varient, wlf.Type, wlf.Rarity, new Bonus[] { }, new WeaponMod[] { }, "");
                }
            });
        }
        public static async void SetEquippedWeapon(WeaponEventArgs e)
        {
            await CreateGear(e);
            OnWeaponSet(e);
        }
    }
}
