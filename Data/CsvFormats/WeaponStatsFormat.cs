using DivBuildApp.Data.Tables;
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
        public double Damage { get; set; }
        public double RPM { get; set; }
        public double MagazineSize { get; set; }
        public double ReloadSpeed { get; set; }
        public double OptimalRange { get; set; }

        public WeaponStatsFormat(string name, double damage, double rpm, double magazineSize, double reloadSpeed, double optimalRange)
        {
            Name = name;
            Damage = damage;
            RPM = rpm;
            MagazineSize = magazineSize;
            ReloadSpeed = reloadSpeed;
            OptimalRange = optimalRange;
        }

        public WeaponListFormat List()
        {
            return WeaponList.WeaponBases.FirstOrDefault(a => a.Name == Name);
        }
        public WeaponAttributesFormat Attributes()
        {
            return WeaponAttributes.Attributes.FirstOrDefault(a => a.Name == Name);
        }
    }
}
