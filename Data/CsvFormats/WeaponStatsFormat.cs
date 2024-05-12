using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DivBuildApp.Data.CsvFormats
{
    internal class WeaponStatsFormat
    {
        public string Name { get; set; }
        public string WeaponDamage { get; set; }
        public string CoreAttribute { get; set; }
        public string SideAttribute { get; set; }
        public string Talent { get; set; }

        public WeaponStatsFormat(string name, string weaponDamage, string coreAttribute, string sideAttribute, string talent)
        {
            Name = name;
            WeaponDamage = weaponDamage;
            CoreAttribute = coreAttribute;
            SideAttribute = sideAttribute;
            Talent = talent;
        }
    }
}
