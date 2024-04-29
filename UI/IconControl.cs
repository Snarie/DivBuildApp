using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Resources;

namespace DivBuildApp.UI
{
    internal static class IconControl
    {
        private static bool ResourceExists(string resourcePath)
        {
            try
            {
                Uri resourceUri = new Uri(resourcePath, UriKind.Absolute);
                StreamResourceInfo streamInfo = Application.GetResourceStream(resourceUri);
                return streamInfo != null;
            }
            catch
            {
                return false; // Resource not found
            }
        }
        public static async Task SetBrandImage(Image imageControl, string brandName)
        {
            string resourcePath = $"pack://application:,,,/Images/Brand Icons/{brandName}.png";

            // Check if the resource exists
            bool exists = ResourceExists(resourcePath);
            if (!exists)
            {
                Console.WriteLine(resourcePath + " Not found");
                resourcePath = "pack://application:,,,/Images/Brand Icons/Improvised.png";
            }

            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                imageControl.Source = new BitmapImage(new Uri(resourcePath));
            });
        }
        
        public static async Task SetStatIcon(ItemType itemType, int index)
        {
            Image image = Lib.GetStatIcon(itemType, index);
            ComboBox comboBox = Lib.GetStatBox(itemType, index);
            object selected = null;
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                selected = comboBox.SelectedItem;
            });
            string resourcePath = "pack://application:,,,/Images/ItemType Icons/Undefined.png";
            if (selected is BonusDisplay bonusDisplay)
            {
                string newPath = $"pack://application:,,,/Images/ItemType Icons/{bonusDisplay.IconType}.png";
                if( ResourceExists(newPath))
                {
                    resourcePath = newPath;
                }
            }

            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                image.Source = new BitmapImage(new Uri(resourcePath));
            });
        }
        public static async Task SetStatIcon(Image imageControl, BonusDisplay bonusDisplay)
        {
            string resourcePath = $"pack://application:,,,/Images/ItemType Icons/{bonusDisplay.IconType}.png";

            bool exists = ResourceExists(resourcePath);
            if (!exists)
            {
                Console.WriteLine(resourcePath + " Not found");
                resourcePath = "pack://application:,,,/Images/ItemType Icons/Undefined.png";
            }

            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                imageControl.Source = new BitmapImage(new Uri(resourcePath));
            });

        }


        
    }
}
