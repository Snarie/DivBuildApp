using DivBuildApp.BonusControl;
using DivBuildApp.Data.CsvFormats;
using DivBuildApp.Data.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DivBuildApp.UI
{
    internal static class WeaponStatsControl
    {
        public static void Initialize()
        {
            ActiveBonuses.CalculateBonusesSet += HandleBonusesSet;
            WeaponHandler.WeaponSet += HandleWeaponSet;
            // Set
        }
        public static void HandleWeaponSet(object sender, WeaponEventArgs e)
        {
            Task.Run(() => SetWeaponStats(e.Grid));
        }
        public static void HandleBonusesSet(object sender, EventArgs e)
        {
            Task.Run(() => SetWeaponStats(Lib.WeaponGridLinks["PrimaryWeapon"]));
            Task.Run(() => SetWeaponStats(Lib.WeaponGridLinks["SecondaryWeapon"]));
            Task.Run(() => SetWeaponStats(Lib.WeaponGridLinks["SideArm"]));
        }

        public static async Task SetWeaponStats(WeaponGridContent grid)
        {
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                if (grid.Box.SelectedItem is WeaponListFormat wlf)
                {
                    WeaponStatsFormat wsf = WeaponStats.GetWeaponStats(wlf.Name);
                    bool success = Enum.TryParse(wlf.Type + "_Damage", out BonusType damageType);
                    if (success)
                    {
                        double dmgMult = (100 + ActiveBonuses.activeBonuses[damageType] + ActiveBonuses.activeBonuses[BonusType.Weapon_Damage]) / 100; 
                        grid.Damage.Content = Math.Floor(double.Parse(wsf.Damage) * dmgMult);
                        double rpmMult = (100 + ActiveBonuses.activeBonuses[BonusType.Rate_of_Fire]) / 100;
                        grid.RPM.Content = Math.Floor(double.Parse(wsf.RPM) * rpmMult);
                        double magMult = (100 + ActiveBonuses.activeBonuses[BonusType.Magazine_Size]) / 100;
                        grid.MagazineSize.Content = Math.Floor(double.Parse(wsf.MagazineSize) * magMult);
                    }
                }
            });
        }
    }
}
