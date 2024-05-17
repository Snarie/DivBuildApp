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
        public static event EventHandler<WeaponEventArgs> WeaponSliderRangeSet;
        private static void OnSliderRangeSet(GridEventArgs e)
        {
            SliderRangeSet?.Invoke(null, e);
        }
        private static void OnWeaponSliderRangeSet(WeaponEventArgs e)
        {
            WeaponSliderRangeSet?.Invoke(null, e);
        }



        private static readonly SynchronizedIndexGroupedTaskRunner<WeaponSlot> WeaponRangeSetTaskRunner = new SynchronizedIndexGroupedTaskRunner<WeaponSlot>(TimeSpan.FromSeconds(1), 3);
        public static async void SetWeaponRangeAsync(WeaponEventArgs e)
        {
            await WeaponRangeSetTaskRunner.ExecuteTaskAsync(e.Slot, e.Index, () =>
            {
                Slider slider = e.Grid.StatSliders[e.Index];
                ComboBox statBox = e.Grid.StatBoxes[e.Index];

                if (!(statBox.SelectedItem is BonusDisplay bonusDisplay))
                {
                    slider.Visibility = Visibility.Collapsed;
                }
                else
                {
                    slider.Visibility = Visibility.Visible;

                    if (FillRectangleExists(slider, out Rectangle rect)) SetFillColorFromIconType(rect, bonusDisplay.IconType);

                    SetRange(slider, bonusDisplay);
                }

                OnWeaponSliderRangeSet(e);

                // Return a completed task because lambda must return a Task
                // Alternative is async await OnSliderRangeSet
                return Task.CompletedTask;
            });
        }
        
        private static readonly SynchronizedIndexGroupedTaskRunner<ItemType> RangeSetTaskRunner = new SynchronizedIndexGroupedTaskRunner<ItemType>(TimeSpan.FromSeconds(0.05), 4);
        public static async void SetRangeAsync(GridEventArgs e)
        {
            await RangeSetTaskRunner.ExecuteTaskAsync(e.ItemType, e.Index, () =>
            {
                Slider slider = e.Grid.StatSliders[e.Index];
                ComboBox statBox = e.Grid.StatBoxes[e.Index];

                if (!(statBox.SelectedItem is BonusDisplay bonusDisplay))
                {
                    slider.Visibility = Visibility.Collapsed;
                }
                else
                {
                    slider.Visibility = Visibility.Visible;

                    if (FillRectangleExists(slider, out Rectangle rect)) SetFillColorFromIconType(rect, bonusDisplay.IconType);

                    SetRange(slider, bonusDisplay);
                }

                OnSliderRangeSet(e);

                // Return a completed task because lambda must return a Task
                // Alternative is async await OnSliderRangeSet
                return Task.CompletedTask;
            });
            
        }
        private static void SetRange(Slider slider, BonusDisplay bonusDisplay)
        {
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
        }
        private static void SetFillColorFromIconType(Rectangle rect, string iconType)
        {
            Brush brush = Brushes.Pink;
            if (iconType.EndsWith("Red"))
            {
                brush = Brushes.Red;
            }
            else if (iconType.EndsWith("Blue"))
            {
                brush = Brushes.DeepSkyBlue;
            }
            else if (iconType.EndsWith("Yellow"))
            {
                brush = Brushes.Yellow;
            }
            rect.Fill = brush;
        }
        private static bool FillRectangleExists(Slider slider, out Rectangle rectangle)
        {
            rectangle = null;

            if (!(slider.Template.FindName("PART_Track", slider) is Track track))
            {
                string t = slider.Name;
                Task.Run(() => Logger.LogWarning($"track doesn't exist: {t}"));
                return false;
            }
            RepeatButton decreaseButton = track.DecreaseRepeatButton;
            //RepeatButton increaseButton = track.IncreaseRepeatButton;

            if (decreaseButton == null)
            {
                string t = slider.Name;
                Task.Run(() => Logger.LogError($"decreaseButton doesn't exist: {t}"));
                return false;
            }
            rectangle = decreaseButton.Template.FindName("decreaseRect", decreaseButton) as Rectangle;
            if (rectangle == null)
            {
                string t = decreaseButton.Name;
                Task.Run(() => Logger.LogError($"rectangle doesn't exist: {t}"));
                return false;
            }
            return true;
        }
    }
}
