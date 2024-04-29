using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;

namespace DivBuildApp.UI
{
    internal static class StatValueLabelControl
    {
        public static void SetValue(ItemType itemType, int index)
        {
            Label label = Lib.GetStatValue(itemType, index);
            ComboBox comboBox = Lib.GetStatBox(itemType, index);
            Slider slider = Lib.GetStatSlider(itemType, index);
            if(comboBox.SelectedItem is BonusDisplay bonusDisplay)
            {
                SetValue(label, bonusDisplay, slider.Value);
            }
            else
            {
                label.Content = "";
            }
        }
        public static void SetValue(Label label, BonusDisplay bonusDisplay, double multiplier)
        {
            Bonus bonus = new Bonus(bonusDisplay.Bonus.BonusType, bonusDisplay.Bonus.Value, bonusDisplay.Bonus.DisplayType);
            switch (bonus.BonusType)
            {
                case BonusType.Skill_Tier:
                case BonusType.Armor_Kit_Capacity:
                case BonusType.Grenade_Capacity:
                case BonusType.Skill_Repair_Charges:
                case BonusType.Skill_Stim_Charges:
                case BonusType.Skill_Stinger_Charges:
                    break;
                default:
                    bonus.Value *= multiplier / 100;
                    break;
            }
            label.DataContext = bonus;
            label.Content = bonus.DisplayValue;
            //Console.WriteLine(bonus.DisplayValue + " " + bonus.BonusType + " " + multiplier);
        }
    }

}
