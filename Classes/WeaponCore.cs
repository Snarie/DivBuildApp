﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DivBuildApp
{
    public enum WeaponType
    {
         AR,
         LMG,
         Shotgun,
         SMG,
         Rifle,
         MMR,
         Pistol
    }
    
    internal class Weapon
    {
        public string Name { get; set; }
        public string Varient { get; set; }
        public WeaponType Type { get; set; }
        public string Rarity { get; set; }
        public Bonus[] StatAttributes { get; set; }
        public WeaponMod[] WeaponMods { get; set; }
        public string Talent { get; set; }

        public Weapon(string name, string varient, WeaponType type, string rarity, Bonus[] statAttributes, WeaponMod[] weaponMods, string talent)
        {
            Name = name;
            Varient = varient;
            Type = type;
            Rarity = rarity;
            StatAttributes = statAttributes;
            WeaponMods = weaponMods;
            Talent = talent;
        }
    }

    internal class WeaponMod
    {
        public string Name { get; set; }
        public Bonus[] Bonuses { get; set; }
        public WeaponMod(string name, Bonus[] bonuses)
        {
            Name = name;
            Bonuses = bonuses;
        }
    }
}
