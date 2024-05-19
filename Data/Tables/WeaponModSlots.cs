using DivBuildApp.CsvFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DivBuildApp.Data.Tables
{
    internal static class WeaponModSlots
    {
        public static List<WeaponModSlot> All;
        public static void Initialize()
        {
            All = CsvReader.WeaponModSlots();
        }

        
    }
}
