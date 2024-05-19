using DivBuildApp.CsvFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DivBuildApp.Data.Tables
{
    internal class WeaponMod
    {
        public string Name { get; set; }
        public string Slot { get; set; }
        public string Type { get; set; }
        public Bonus[] Bonuses { get; set; }

        public WeaponMod(string name, string slot, string type, Bonus[] bonuses)
        {
            Name = name;
            Slot = slot;
            Type = type;
            Bonuses = bonuses;
        }
    }
    internal static class WeaponMods
    {
        public static List<WeaponModFormat> mods;
        public static void Initialize()
        {
            mods = CsvReader.WeaponMods();

            Console.WriteLine(mods[400]);
        }
    }
}
