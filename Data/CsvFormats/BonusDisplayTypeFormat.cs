using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DivBuildApp.CsvFormats
{
    public class BonusDisplayTypeFormat
    {
        public string Name { get; set; }
        public string DisplayType { get; set; }

        public BonusDisplayTypeFormat(string name, string displayType)
        {
            Name = name;
            DisplayType = displayType;
        }
    }
}
