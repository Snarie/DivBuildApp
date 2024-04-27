using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DivBuildApp.CsvFormats
{
    internal class BrandBonusesFormat
    {
        public string Name { get; set; }
        public string Slot1 {  get; set; }
        public string Slot2 { get; set; }
        public string Slot3 { get; set; }

        public BrandBonusesFormat(string name, string slot1, string slot2, string slot3)
        {
            Name = name;
            Slot1 = slot1;
            Slot2 = slot2;
            Slot3 = slot3;
        }
    }
}
