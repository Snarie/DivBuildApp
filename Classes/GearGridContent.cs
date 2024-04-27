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
        public Label ItemArmor { get; set; }
        public ComboBox ItemExpertiece { get; set; }
        public ComboBox[] StatBoxes { get; set; }
        public Label[] StatValues { get; set; }
        public Slider[] StatSliders { get; set; }
        public TextBlock[] BrandBonusTextBlocks { get; set; }
        public Image BrandImage { get; set; }
        public GearGridContent(ComboBox itemBox, Label itemName, Label itemArmor, ComboBox itemExpertiece, ComboBox[] statBoxes, Label[] statValues, Slider[] statSliders, TextBlock[] brandBonusTextBlocks, Image brandImage) 
        {
            ItemBox = itemBox;
            ItemName = itemName;
            ItemArmor = itemArmor;
            ItemExpertiece = itemExpertiece;
            StatBoxes = statBoxes;
            StatValues = statValues;
            StatSliders = statSliders;
            BrandBonusTextBlocks = brandBonusTextBlocks;
            BrandImage = brandImage;
        }

    }
}
