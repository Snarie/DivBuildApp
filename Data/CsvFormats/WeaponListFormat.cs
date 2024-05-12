using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DivBuildApp.Data.CsvFormats
{
    internal class WeaponListFormat
    {
        public string Name { get; set; }
        public string Varient { get; set; }
        public string Type { get; set; }
        public string Rarity { get; set; }

        public WeaponListFormat(string name, string varient, string type, string rarity)
        {
            Name = name;
            Varient = varient;
            Type = type;
            Rarity = rarity;
        }
    }
}
