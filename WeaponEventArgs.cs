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
        public WeaponSlot Slot { get; }
        public int Index { get; }
        public WeaponType Type { get; }  // Not in use but might be usefull

        public WeaponEventArgs(string slot, int index)
        {
            Grid = Lib.GetWeaponGridContent(slot);
            _ = Enum.TryParse(slot, true, out WeaponSlot ws);
            Slot = ws;
            Index = index;
        }
    }
}
