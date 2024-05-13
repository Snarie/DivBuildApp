using DivBuildApp.Data.CsvFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DivBuildApp.Data.Tables
{
    internal static class WeaponStats
    {
        public static List<WeaponStatsFormat> Stats = new List<WeaponStatsFormat>();
        public static void Initialize()
        {
            Stats = CsvReader.WeaponStats();
        }

        public static WeaponStatsFormat GetWeaponStats(string weaponName)
        {
            return Stats.FirstOrDefault(b => b.Name == weaponName);
        }
    }
}
