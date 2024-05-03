using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Xml.Linq;
using DivBuildApp.Data.Tables;

namespace DivBuildApp.UI
{
    internal static class DisplayControl
    {

        public static async Task SetItemNameLabelAsync(GridEventArgs e, ComboBoxBrandItem selectedItem)
        {
            Label itemLabel = e.Grid.ItemName;
            Brush brush = Brushes.Red;
            switch (selectedItem.Preset)
            {
                case "[High-End]":
                    brush = Brushes.White;
                    break;
                case "[Named]":
                    brush = Brushes.Goldenrod;
                    break;
                case "[Exotic]":
                    brush = Brushes.OrangeRed;
                    break;
                case "[Gear Set]":
                    brush = Brushes.SpringGreen;
                    break;
                default:
                    // Should never happen
                    brush = Brushes.Red;
                    break;
            }
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                itemLabel.Content = selectedItem.Name;
                itemLabel.Foreground = brush;
            });
        }
        
        
        

        public static async Task DisplayBrandBonusesAsync(GridEventArgs e, string brandName)
        {
            TextBlock[] bonusBoxes = e.Grid.BrandBonusTextBlocks;
            //TextBlock[] bonusBoxes = Lib.GetBrandBonusTextBlocks(itemType);
            if (bonusBoxes[0] == null || bonusBoxes[1] == null || bonusBoxes[2] == null)
            {
#pragma warning disable CS4014
                Task.Run(() => Logger.LogError($"One or more text boxes are null"));
#pragma warning restore CS4014
                return;
            }
            var bonusesTask1 = Task.Run(() => BrandSets.GetBrandBonus(brandName, 1));
            var bonusesTask2 = Task.Run(() => BrandSets.GetBrandBonus(brandName, 2));
            var bonusesTask3 = Task.Run(() => BrandSets.GetBrandBonus(brandName, 3));

            List<Bonus>[] bonuses = await Task.WhenAll(bonusesTask1, bonusesTask2, bonusesTask3);

            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                DisplayBrandBonus(bonusBoxes[0], bonuses[0]);
                DisplayBrandBonus(bonusBoxes[1], bonuses[1]);
                DisplayBrandBonus(bonusBoxes[2], bonuses[2]);
            });
        }
        
        private static void DisplayBrandBonus(TextBlock textBlock, List<Bonus> Bonuses)
        {
            textBlock.Inlines.Clear();

            foreach(Bonus bonus in Bonuses)
            {
                Run run1 = new Run(bonus.DisplayValue)
                {
                    Foreground = Brushes.Goldenrod
                };
                Run run2 = new Run(" " + bonus.DisplayBonusType)
                {
                    Foreground = Brushes.White
                };
                textBlock.Inlines.Add(run1);
                textBlock.Inlines.Add(run2);

                if(bonus != Bonuses.Last())
                {
                    textBlock.Inlines.Add(new LineBreak());
                }
            }
  
        }
    }
}
