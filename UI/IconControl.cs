using DivBuildApp.Data.CsvFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Imaging;
using System.Windows.Resources;

namespace DivBuildApp.UI
{
    internal static class IconControl
    {
        public static void Initialize()
        {
            GearHandler.GearSet += HandleGearSet;
            StatValueLabelControl.ValueSet += HandleValueSet;
            WeaponHandler.WeaponSet += HandleWeaponSet;
            //Creates the eventHandlers
        }
        private static void HandleGearSet(object sender, GridEventArgs e)
        {
            Task.Run(() => SetBrandImage(e));
        }
        private static void HandleValueSet(object sender, GridEventArgs e)
        {
            Task.Run(() => SetStatIcon(e));
        }
        private static void HandleWeaponSet(object sender, WeaponEventArgs e)
        {
            Task.Run(() => SetWeaponImage(e));
        }
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
                Task.Run(() => Logger.LogError($"{resourcePath} is not a recognized ResourceStream"));
                return false; // Resource not found
            }
        }
        public static async Task SetWeaponImage(WeaponEventArgs e)
        {
            Image imageControl = e.Grid.Image;

            object selectedItem = null;
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                selectedItem = e.Grid.Box.SelectedItem;
            });
            string resourcePath = $"pack://application:,,,/Images/BrandDefault.png";
            if(selectedItem is WeaponListFormat weapon)
            {
                string imageName = weapon.Rarity+"_"+weapon.Type;

                string newPath = $"pack://application:,,,/Images/Weapon Type Icons/{imageName}.png";
                if (ResourceExists(newPath))
                {
                    resourcePath = newPath;
                }
            }

            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                imageControl.Source = new BitmapImage(new Uri(resourcePath));
            });
        }
        public static async Task SetBrandImage(GridEventArgs e)
        {
            Image imageControl = e.Grid.BrandImage;

            object selectedItem = null;
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                selectedItem = e.Grid.Box.SelectedItem;
            });
            string resourcePath = $"pack://application:,,,/Images/Brand Icons/Improvised.png";
            if (selectedItem is ComboBoxBrandItem item)
            {
                string brandName = ItemHandler.BrandFromName(item.Name);
                string newPath = $"pack://application:,,,/Images/Brand Icons/{brandName}.png";
                if (ResourceExists(newPath))
                {
                    resourcePath = newPath;
                }
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
