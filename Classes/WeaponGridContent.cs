using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DivBuildApp
{
    internal class WeaponGridContent
    {
        public ComboBox Box { get; set; }
        public Label Name { get; set; }
        public Label Armor { get; set; }
        public ComboBox Expertiece { get; set; }
        public ComboBox WeaponType { get; set; }
        public ComboBox OpticalRail { get; set; }
        public ComboBox Magazine { get; set; }
        public ComboBox Underbarrel { get; set; }
        public ComboBox Muzzle { get; set; }
        public ComboBox[] StatBoxes { get; set; }
        public Label[] StatValues { get; set; }
        public Slider[] StatSliders { get; set; }
        public Image[] StatIcons { get; set; }
        public Image BrandImage { get; set; }

        public WeaponGridContent(ComboBox box, Label name, Label armor, ComboBox expertiece, ComboBox weaponType, ComboBox opticalRail, ComboBox magazine, ComboBox underbarrel, ComboBox muzzle, ComboBox[] statBoxes, Label[] statValues, Slider[] statSliders, Image[] statIcons, Image brandImage)
        {
            Box = box;
            Name = name;
            Armor = armor;
            Expertiece = expertiece;
            WeaponType = weaponType;
            OpticalRail = opticalRail;
            Magazine = magazine;
            Underbarrel = underbarrel;
            Muzzle = muzzle;
            StatBoxes = statBoxes;
            StatValues = statValues;
            StatSliders = statSliders;
            StatIcons = statIcons;
            BrandImage = brandImage;
        }
    }
}
