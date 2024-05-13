using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DivBuildApp
{
    internal class WeaponEventArgs
    {
        public WeaponGridContent Grid { get; }
        public WeaponType Type { get; }
        public string Slot { get; }

        public WeaponEventArgs(string slot)
        {
            Grid = Lib.GetWeaponGridContent(slot);
            Slot = slot;

        }
    }
}
