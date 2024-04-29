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
    internal static class StatSliderControl
    {
        public static void SetRange(Slider slider, BonusDisplay bonusDisplay)
        {
            slider.Visibility = Visibility.Visible;

            bool success = FillRectangleExists(slider, out Rectangle rect);
            if(!success) 
            {
                return;
            }

            Brush brush = Brushes.Pink;
            if (bonusDisplay.IconType.EndsWith("Red"))
            {
                brush = Brushes.Red;
            }
            else if (bonusDisplay.IconType.EndsWith("Blue"))
            {
                brush = Brushes.DeepSkyBlue;
            }
            else if (bonusDisplay.IconType.EndsWith("Yellow"))
            {
                brush = Brushes.Yellow;
            }

            switch (bonusDisplay.Bonus.BonusType)
            {
                case BonusType.Skill_Tier:
                case BonusType.Armor_Kit_Capacity:
                case BonusType.Grenade_Capacity:
                case BonusType.Skill_Repair_Charges:
                case BonusType.Skill_Stim_Charges:
                case BonusType.Skill_Stinger_Charges:
                    slider.Minimum = 100;
                    slider.Value = 100;
                    break;
                default:
                    slider.Minimum = 1;
                    break;
            }
            rect.Fill = brush;
        }
        private static bool FillRectangleExists(Slider slider, out Rectangle rectangle)
        {
            rectangle = null;

            if (!(slider.Template.FindName("PART_Track", slider) is Track track))
            {
                Console.WriteLine($"track doesn't exist: {slider.Name}");
                return false;
            }
            RepeatButton decreaseButton = track.DecreaseRepeatButton;
            //RepeatButton increaseButton = track.IncreaseRepeatButton;

            if (decreaseButton == null)
            {
                Console.WriteLine($"decreaseButton doesn't exist: {slider.Name}");
                return false;
            }
            rectangle = decreaseButton.Template.FindName("decreaseRect", decreaseButton) as Rectangle;
            if (rectangle == null)
            {
                Console.WriteLine($"rect doesn't exist: {slider.Name}");
                return false;
            }
            return true;
        }
    }
}
