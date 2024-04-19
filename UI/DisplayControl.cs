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
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Linq;

namespace DivBuildApp
{
    public static class DisplayControl
    {

        public static async Task SetItemNameLabelAsync(Label itemLabel, ComboBoxBrandItem selectedItem)
        {
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
        public static async Task SetBrandImageAsync(Image imageControl, string brandName)
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string imagePath = Path.Combine(baseDirectory, "Images", "Brand Icons", brandName + ".png");

            bool exists = await Task.Run(() => File.Exists(imagePath));
            if (!exists)
            {
                Console.WriteLine(imagePath + " Not found");
                imagePath = Path.Combine(baseDirectory, "Images", "Brand Icons", "Improvised.png");
            }

            var uri = new Uri(imagePath, UriKind.Absolute);
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                imageControl.Source = new BitmapImage(uri);
            });
        }

        public static async Task DisplayBrandBonusesAsync(ItemType itemType, string brandName)
        {
            TextBlock[] bonusBoxes = Lib.GetBrandBonusTextBlocks(itemType);
            if (bonusBoxes[0] == null || bonusBoxes[1] == null || bonusBoxes[2] == null)
            {
                Console.WriteLine("One or more text boxes are null.");
                return;
            }
            // Assuming GetBrandBonus is an I/O operation or might benefit from being run asynchronously
            var bonusesTask1 = Task.Run(() => BonusHandler.GetBrandBonus(brandName, 1));
            var bonusesTask2 = Task.Run(() => BonusHandler.GetBrandBonus(brandName, 2));
            var bonusesTask3 = Task.Run(() => BonusHandler.GetBrandBonus(brandName, 3));

            List<Bonus>[] bonuses = await Task.WhenAll(bonusesTask1, bonusesTask2, bonusesTask3);

            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                DisplayBrandBonus(bonusBoxes[0], bonuses[0]);
                DisplayBrandBonus(bonusBoxes[1], bonuses[1]);
                DisplayBrandBonus(bonusBoxes[2], bonuses[2]);
            });
        }
        //Place Dispatcher to correct space, does alot on the UI thread now
        public static void DisplayBrandBonus(TextBlock textBlock, List<Bonus> Bonuses)
        {
            textBlock.Inlines.Clear();

            foreach(Bonus bonus in Bonuses)
            {
                var (names, values) = BonusToString(bonus);
                Run run1 = new Run(values)
                {
                    Foreground = Brushes.Goldenrod
                };
                Run run2 = new Run(" " + names)
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

        public static void DisplayBonus(Label label, Bonus bonus)
        {

            string name = bonus.BonusType.ToString().Replace('_', ' ');
            label.Content = $"{name} = {bonus.Value}";
        }
        public static void DisplayBonusses(TextBlock textBlock)
        {
            string msg = string.Empty;
            foreach (KeyValuePair<BonusType, double> bonus in BonusHandler.activeBonusses)
            {
                msg += $"{bonus.Key} = {bonus.Value}\n";
            }
            textBlock.Text = msg;
        }


        
        public static string ConvertBonusValue(Bonus bonus)
        {
            switch (bonus.BonusType)
            {
                case BonusType.Skill_Tier:
                case BonusType.Grenade_Capacity:
                case BonusType.Armor_Kit_Capacity:
                    return $"{bonus.Value}";
                default:
                    return $"{bonus.Value}%";
            }
            
        }
        public static string ConvertBonusType(Bonus bonus)
        {
            return bonus.BonusType.ToString().Replace('_', ' ');
        }


        public static string[] BonusToStringJoined(List<Bonus> bonuses, string separator, bool flipped = false)
        {
            var (left, right) = BonusesToString(bonuses);
            if(flipped) (left, right) = (right, left);
            if (left.Length != right.Length)
            {
                throw new ArgumentException("Names and values arrays must have the same length.");
            }
            string[] result = new string[left.Length];

            for (int i = 0; i < left.Length; i++)
            {
                result[i] = $"{left[i]}{separator}{right[i]}";
            }
            
            
            return result;
        }


        /// <summary>
        /// Converts the values of the bonusses to their string variants.
        /// </summary>
        /// <param name="bonusses"></param>
        /// <returns>The string representations of the name and value of the Bonuses inside a Dictionary</returns>
        public static (string[] names, string[] values) BonusesToString(IEnumerable<Bonus> bonuses)
        {
            var names = new List<string>();
            var values = new List<string>();

            foreach (var bonus in bonuses)
            {
                names.Add(ConvertBonusType(bonus));
                values.Add(ConvertBonusValue(bonus));
            }

            return (names.ToArray(), values.ToArray());
        }

        /// <summary>
        /// Converts the bonus to its string varient
        /// </summary>
        /// <param name="bonus"></param>
        /// <returns>the string representation of the name and value of the BOnus</returns>
        public static (string, string) BonusToString(Bonus bonus)
        {
            // If you only need the values:
            //var (_, values) = BonusToString(bonuses);
            return (ConvertBonusType(bonus), ConvertBonusValue(bonus));
        }
    }
}
