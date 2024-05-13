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
        public string Damage { get; set; }
        public string RPM { get; set; }
        public string MagazineSize { get; set; }
        public string ReloadSpeed { get; set; }
        public string OptimalRange { get; set; }

        public WeaponStatsFormat(string name, string damage, string rpm, string magazineSize, string reloadSpeed, string optimalRange)
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
