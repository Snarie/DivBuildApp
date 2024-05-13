using DivBuildApp.Data.CsvFormats;
using DivBuildApp.Data.Tables;
using DivBuildApp.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public static void SetEquippedWeapon(WeaponEventArgs e)
        {
            OnWeaponSet(e);
        }
    }
}
