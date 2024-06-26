﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DivBuildApp
{
    internal class WeaponMod
    {
        public string Name { get; set; }
        public string Slot { get; set; }
        public string Type { get; set; }
        public Bonus[] Attributes { get; set; }

        public WeaponMod() { }
        public WeaponMod(string name, string slot, string type, Bonus[] attributes) 
        { 
            Name = name;
            Slot = slot;
            Type = type;
            Attributes = attributes;
        }
    }
}
