using DivBuildApp.CsvFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DivBuildApp.Data.Tables
{
    internal static class WeaponMods
    {
        public static List<WeaponMod> mods;
        public static void Initialize()
        {
            mods = CsvReader.WeaponMods();
        }

        public static List<WeaponMod> GetFilteredMods(string name)
        {
            string[] groups = name.Split('/');
            return mods.Where(m => groups.Contains(m.Type)).ToList();
        }
    }
}
