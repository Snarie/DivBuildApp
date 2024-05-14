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
    }
}
