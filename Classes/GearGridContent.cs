using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DivBuildApp
{
    public class GearGridContent
    {
        public ComboBox ItemBox { get; set; }
        public Label ItemName { get; set; }
        public ComboBox CoreStatBox { get; set; }
        public ComboBox[] SideStatBoxes { get; set; }
        public TextBlock[] BrandBonusTextBlocks { get; set; }
        public Image BrandImage { get; set; }
        public GearGridContent(ComboBox itemBox, Label itemName, ComboBox coreStatBox, ComboBox[] sideStatBoxes, TextBlock[] brandBonusTextBlocks, Image brandImage) 
        {
            ItemBox = itemBox;
            ItemName = itemName;
            CoreStatBox = coreStatBox;
            SideStatBoxes = sideStatBoxes;
            BrandBonusTextBlocks = brandBonusTextBlocks;
            BrandImage = brandImage;
        }

    }
}
