using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DivBuildApp
{
    internal class WeaponModSlot
    {
        public string Name { get; set; }
        public string OpticalRail { get; set; }
        public string Magazine { get; set; }
        public string Underbarrel { get; set; }
        public string Muzzle { get; set; }

        public WeaponModSlot() { }
        public WeaponModSlot(string name, string opticalRail, string magazine, string underbarrel, string muzzle)
        {
            Name = name;
            OpticalRail = opticalRail;
            Magazine = magazine;
            Underbarrel = underbarrel;
            Muzzle = muzzle;
        }
    }
}
