using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DivBuildApp
{
    public class GearGridContent
    {
        public ComboBox Box { get; set; }
        public Label Name { get; set; }
        public Label Armor { get; set; }
        public ComboBox Expertiece { get; set; }
        
        public TextBlock[] BrandBonusTextBlocks { get; set; }
        public ComboBox[] StatBoxes { get; set; }
        public Label[] StatValues { get; set; }
        public Slider[] StatSliders { get; set; }
        public Image[] StatIcons { get; set; }
        public Image BrandImage { get; set; }
        public GearGridContent(ComboBox box, Label name, Label armor, ComboBox expertiece, TextBlock[] brandBonusTextBlocks, ComboBox[] statBoxes, Label[] statValues, Slider[] statSliders, Image[] statIcons, Image brandImage) 
        {
            Box = box;
            Name = name;
            Armor = armor;
            Expertiece = expertiece;
            BrandBonusTextBlocks = brandBonusTextBlocks;
            StatBoxes = statBoxes;
            StatValues = statValues;
            StatSliders = statSliders;
            StatIcons = statIcons;
            BrandImage = brandImage;
        }

    }
}
