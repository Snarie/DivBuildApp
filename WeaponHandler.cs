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
            WeaponSet?.Invoke(null, e);
        }
        public static void SetEquippedWeapon(WeaponEventArgs e)
        {
            OnWeaponSet(e);
        }
    }
}
