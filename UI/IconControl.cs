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
        public static async Task SetBrandImage(GridEventArgs e, string brandName)
        {
            Image imageControl = e.Grid.BrandImage;
            string resourcePath = $"pack://application:,,,/Images/Brand Icons/{brandName}.png";

            // Check if the resource exists
            bool exists = ResourceExists(resourcePath);
            if (!exists)
            {
#pragma warning disable CS4014
                Task.Run(() => Logger.LogError($"{resourcePath} Not found"));
#pragma warning restore CS4014
                resourcePath = "pack://application:,,,/Images/Brand Icons/Improvised.png";
            }

            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                imageControl.Source = new BitmapImage(new Uri(resourcePath));
            });
        }
        
        public static async Task SetStatIcon(GridEventArgs e)
        {
            Image image = e.Grid.StatIcons[e.Index];
            ComboBox comboBox = e.Grid.StatBoxes[e.Index];
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


        
    }
}
