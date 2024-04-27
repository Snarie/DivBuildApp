using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DivBuildApp.CsvFormats
{
    public class BonusCapsFormat
    {
        public string Name { get; set; }
        public string IconType {  get; set; }
        public string GearCore { get; set; }
        public string GearSide { get; set; }
        public string Mod { get; set; }
        public string WeaponCore { get; set; }
        public string WeaponPrimary { get; set; }
        public string WeaponSide { get; set; }
        public string DisplayType { get; set; }

        public BonusCapsFormat(string name, string iconType, string gearCore, string gearSide, string mod, string weaponCore, string weaponPrimary, string weaponSide, string displayType)
        {
            Name = name;
            IconType = iconType;
            GearCore = gearCore;
            GearSide = gearSide;
            Mod = mod;
            WeaponCore = weaponCore;
            WeaponPrimary = weaponPrimary;
            WeaponSide = weaponSide;
            DisplayType = displayType;

        }
    }
}
