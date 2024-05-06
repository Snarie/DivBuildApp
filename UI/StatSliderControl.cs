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
        public static void Initialize()
        {
            // Set eventHandlers
        }
        public static event EventHandler<GridEventArgs> SliderRangeSet;
        private static void OnSliderRangeSet(GridEventArgs e)
        {
            SliderRangeSet?.Invoke(null, e);
        }
        public static void SetRange(GridEventArgs e)
        {
            Slider slider = e.Grid.StatSliders[e.Index];
            ComboBox statBox = e.Grid.StatBoxes[e.Index];

            if (!(statBox.SelectedItem is BonusDisplay bonusDisplay))
            {
                slider.Visibility = Visibility.Collapsed;
                OnSliderRangeSet(e);
                return;
            }
            slider.Visibility = Visibility.Visible;

            bool success = FillRectangleExists(slider, out Rectangle rect);
            if(success) 
            {
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
                rect.Fill = brush;
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
            OnSliderRangeSet(e);
        }
        private static bool FillRectangleExists(Slider slider, out Rectangle rectangle)
        {
            rectangle = null;

            if (!(slider.Template.FindName("PART_Track", slider) is Track track))
            {
                Task.Run(() => Logger.LogWarning($"track doesn't exist: {slider.Name}"));
                return false;
            }
            RepeatButton decreaseButton = track.DecreaseRepeatButton;
            //RepeatButton increaseButton = track.IncreaseRepeatButton;

            if (decreaseButton == null)
            {
                Task.Run(() => Logger.LogError($"decreaseButton doesn't exist: {slider.Name}"));
                return false;
            }
            rectangle = decreaseButton.Template.FindName("decreaseRect", decreaseButton) as Rectangle;
            if (rectangle == null)
            {
                Task.Run(() => Logger.LogError($"rectangle doesn't exist: {decreaseButton.Name}"));
                return false;
            }
            return true;
        }
    }
}
