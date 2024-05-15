using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using DivBuildApp.Data.Tables;

namespace DivBuildApp.Data.CsvFormats
{
    internal class WeaponAttributesFormat
    {
        public string Name { get; set; }
        public string Core { get; set; }
        public string Main { get; set; }
        public string Side { get; set; }
        public string Talent { get; set; }

        public WeaponAttributesFormat(string name, string core, string main, string side, string talent)
        {
            Name = name;
            Core = core;
            Main = main;
            Side = side;
            Talent = talent;
        }

        public WeaponListFormat List()
        {
            WeaponListFormat wlf = WeaponList.WeaponBases.FirstOrDefault(a => a.Name == Name);
            return wlf;
        }
        public WeaponStatsFormat Stats()
        {
            return WeaponStats.Stats.FirstOrDefault(a => a.Name == Name);
        }
    }
}
