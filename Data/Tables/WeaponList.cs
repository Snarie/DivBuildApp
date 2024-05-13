//using DivBuildApp.Classes;
using DivBuildApp.CsvFormats;
using DivBuildApp.Data.CsvFormats;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DivBuildApp.Data.Tables
{
    internal static class WeaponList
    {
        public static List<WeaponListFormat> WeaponBases = new List<WeaponListFormat>();
        public static Dictionary<WeaponType, List<WeaponListFormat>> TypeList = new Dictionary<WeaponType, List<WeaponListFormat>>();
        public static void Initialize()
        {
            WeaponBases = CsvReader.WeaponList();

            TypeList.Clear();
            foreach (WeaponType weaponType in Enum.GetValues(typeof(WeaponType)))
            {
                TypeList[weaponType] = new List<WeaponListFormat>();
            }
            foreach (WeaponListFormat weaponFormat in  WeaponBases)
            {
                bool success = Enum.TryParse(weaponFormat.Type, true, out WeaponType weaponType);
                if (!success)
                {
                    continue;
                }
                TypeList[weaponType].Add(weaponFormat);

            }
        }
        
    }
}
