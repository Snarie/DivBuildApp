using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using DivBuildApp.Data.Tables;

namespace DivBuildApp.Data.CsvFormats
{
    internal class WeaponListFormat
    {
        public string Name { get; set; }
        public string Varient { get; set; }
        public WeaponType Type { get; set; }
        public string Rarity { get; set; }

        public WeaponListFormat(string name, string varient, WeaponType type, string rarity)
        {
            Name = name;
            Varient = varient;
            Type = type;
            Rarity = rarity;
        }

        public WeaponAttributesFormat Attributes()
        {
            return WeaponAttributes.Attributes.FirstOrDefault(a => a.Name == Name);
        }
        public WeaponStatsFormat Stats()
        {
            return WeaponStats.Stats.FirstOrDefault(a => a.Name == Name);
        }
        public WeaponModSlot ModSlot()
        {
            return WeaponModSlots.All.FirstOrDefault(a => a.Name == Name);
        }
    }
}
