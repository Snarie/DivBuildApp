using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DivBuildApp.UI
{
    
    internal static class StatValueLabelControl
    {
        public static void Initialize()
        {
            StatSliderControl.SliderRangeSet += HandleSliderRangeSet;
            //Creates the eventHandlers
        }
        private static void HandleSliderRangeSet(object sender, GridEventArgs e)
        {
            SetValueAsync(e);
            Task.Run(() => Logger.LogEvent("StatValueLabelControl <= StatSliderControl.SliderRangeSet"));
        }

        public static event EventHandler<GridEventArgs> ValueSet;
        private static void OnValueSet(GridEventArgs e)
        {
            ValueSet?.Invoke(null, e);
        }


        public static void SetValue(GridEventArgs e)
        {
            
            Label statValueLabel = e.Grid.StatValues[e.Index];
            ComboBox statBox = e.Grid.StatBoxes[e.Index];
            Slider statSlider = e.Grid.StatSliders[e.Index];
            if (statBox.SelectedItem is BonusDisplay bonusDisplay)
            {
                SetValueRouted(statValueLabel, bonusDisplay, statSlider.Value);
            }
            else
            {
                statValueLabel.Content = "";
            }
            OnValueSet(e);
        }

        private static readonly SynchronizedIndexGroupedTaskRunner ValueSetTaskRunner = new SynchronizedIndexGroupedTaskRunner(TimeSpan.FromSeconds(0));
        public static async void SetValueAsync(GridEventArgs e)
        {
            await ValueSetTaskRunner.ExecuteTaskAsync(e.ItemType, e.Index, () =>
            {
                Label statValueLabel = e.Grid.StatValues[e.Index];
                ComboBox statBox = e.Grid.StatBoxes[e.Index];
                Slider statSlider = e.Grid.StatSliders[e.Index];
                if (statBox.SelectedItem is BonusDisplay bonusDisplay)
                {
                    SetValueRouted(statValueLabel, bonusDisplay, statSlider.Value);
                }
                else
                {
                    statValueLabel.Content = "";
                }
                OnValueSet(e);

                // Return a completed task because lambda must return a Task
                return Task.CompletedTask;
            });
        }
        private static void SetValueRouted(Label label, BonusDisplay bonusDisplay, double multiplier)
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
        }
    }

}
