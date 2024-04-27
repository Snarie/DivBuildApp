using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;

namespace DivBuildApp
{
    public static class Lib
    {
        
        public static Dictionary<ItemType, GearGridContent> GearGridLinks = new Dictionary<ItemType, GearGridContent>();

        

        public static void CreateGearGridLink(Grid grid)
        {
            // foreach (var child in LogicalTreeHelper.GetChildren(grid).OfType<FrameworkElement>()) Console.WriteLine(child.Name);

            string baseName = grid.Name.Replace("Grid", ""); // Removes "Grid" to get the base name, e.g. "Mask"
            ComboBox itemBox = FindChildControl<ComboBox>(grid, $"{baseName}Box");

            // Find label to display item name
            Label itemName = FindChildControl<Label>(grid, $"{baseName}Name");

            Label itemArmor = FindChildControl<Label>(grid, $"{baseName}Armor");

            ComboBox itemExpertiece = FindChildControl<ComboBox>(grid, $"{baseName}Expertiece");

            // Find the brandBonusBoxes
            TextBlock bonus1 = FindChildControl<TextBlock>(grid, $"{baseName}Bonus1");
            TextBlock bonus2 = FindChildControl<TextBlock>(grid, $"{baseName}Bonus2");
            TextBlock bonus3 = FindChildControl<TextBlock>(grid, $"{baseName}Bonus3");

            // Find the Stats
            ComboBox stat1 = FindChildControl<ComboBox>(grid, $"{baseName}Stat1");
            ComboBox stat2 = FindChildControl<ComboBox>(grid, $"{baseName}Stat2");
            ComboBox stat3 = FindChildControl<ComboBox>(grid, $"{baseName}Stat3");
            ComboBox stat4 = FindChildControl<ComboBox>(grid, $"{baseName}Stat4");
            // Find the Stat_Values
            Label stat1_Value = FindChildControl<Label>(grid, $"{baseName}Stat1_Value");
            Label stat2_Value = FindChildControl<Label>(grid, $"{baseName}Stat2_Value");
            Label stat3_Value = FindChildControl<Label>(grid, $"{baseName}Stat3_Value");
            Label stat4_Value = FindChildControl<Label>(grid, $"{baseName}Stat4_Value");
            // Find the Stat_Sliders
            Slider stat1_Slider = FindChildControl<Slider>(grid, $"{baseName}Stat1_Slider");
            Slider stat2_Slider = FindChildControl<Slider>(grid, $"{baseName}Stat2_Slider");
            Slider stat3_Slider = FindChildControl<Slider>(grid, $"{baseName}Stat3_Slider");
            Slider stat4_Slider = FindChildControl<Slider>(grid, $"{baseName}Stat4_Slider");

            Image brandImage = FindChildControl<Image>(grid, $"{baseName}BrandImage");

            bool success = Enum.TryParse(baseName, out ItemType itemType);
            if (success)
            {
                GearGridLinks.Add(itemType, new GearGridContent(
                    itemBox, 
                    itemName, itemArmor, itemExpertiece,
                    new ComboBox[] { stat1, stat2, stat3, stat4},
                    new Label[] {stat1_Value, stat2_Value, stat3_Value, stat4_Value},
                    new Slider[] { stat1_Slider, stat2_Slider, stat3_Slider, stat4_Slider },
                    new TextBlock[] { bonus1, bonus2, bonus3 },
                    brandImage
                ));;
            }
        }

        public static bool GridExists(ItemType itemType, out GearGridContent grid)
        {
            bool exists = GearGridLinks.TryGetValue(itemType, out grid);
            return exists;
        }

        /// <summary>
        /// Get the ItemBox for holding selected Item
        /// </summary>
        /// <param name="itemType">Mask/Backpack/Chest/Gloves/Holster/Kneepads</param>
        /// <returns>the ComboBox of the selected Item, or null if grid link doesn't exist</returns>
        public static ComboBox GetItemBox(ItemType itemType)
        {
            return GridExists(itemType, out GearGridContent grid) ? grid.ItemBox : null;
        }
        public static ComboBox GetExpertieceBox(ItemType itemType)
        {
            return GridExists(itemType, out GearGridContent grid) ? grid.ItemExpertiece : null;
        }

        public static Label GetItemNameLabel(ItemType itemType)
        {
            return GridExists(itemType, out GearGridContent grid) ? grid.ItemName : null;
        }

        public static Label GetItemArmorLabel(ItemType itemType)
        {
            return GridExists(itemType, out GearGridContent grid) ? grid.ItemArmor : null;
        }

        /// <summary>
        /// Get all StatSliders containing it's StatBox's selected BonusDisplay Max and minimum Value
        /// </summary>
        /// <param name="itemType">Mask/Backpack/Chest/Gloves/Holster/Kneepads</param>
        /// <returns>The Slider collection of StatSliders, or null if grid link doesn't exist</returns>
        public static Slider[] GetStatSliders(ItemType itemType)
        {
            return GridExists(itemType, out GearGridContent grid) ? grid.StatSliders : null;
        }

        /// <summary>
        /// Get all StatValues containing it's StatBox's selected BonusDisplay Value
        /// </summary>
        /// <param name="itemType">Mask/Backpack/Chest/Gloves/Holster/Kneepads</param>
        /// <returns>The Label collection of StatValues, or null if grid link doesn't exist</returns>
        public static Label[] GetStatValues(ItemType itemType)
        {
            return GridExists(itemType, out GearGridContent grid) ? grid.StatValues : null;
        }

        /// <summary>
        /// Get all StatBoxes for holding selected BonusDisplay
        /// </summary>
        /// <param name="itemType">Mask/Backpack/Chest/Gloves/Holster/Kneepads</param>
        /// <returns>The ComboBox collection of StatBoxes with a list of BonusDisplays, or null if grid link doesn't exist</returns>
        public static ComboBox[] GetStatBoxes(ItemType itemType)
        {
            return GridExists(itemType, out GearGridContent grid) ? grid.StatBoxes : null;
        }

        /// <summary>
        /// Get the CoreStatBox for holding selected core Stat
        /// </summary>
        /// <param name="itemType">Mask/Backpack/Chest/Gloves/Holster/Kneepads</param>
        /// <returns>The ComboBox of the coreStat, or null if grid link doens't exist</returns>
        public static ComboBox GetCoreStatBox(ItemType itemType)
        {
            return GridExists(itemType, out GearGridContent grid) ? grid.StatBoxes[0] : null;
        }
        
        /// <summary>
        /// Get all SideBonusBoxes for holding selected side Stats
        /// </summary>
        /// <param name="itemType">Mask/Backpack/Chest/Gloves/Holster/Kneepads</param>
        /// <returns>The ComboBox collection of sideStats, or null if grid link doesn't exist</returns>
        public static ComboBox[] GetSideStatBoxes(ItemType itemType)
        {
            return GridExists(itemType, out GearGridContent grid) ? new ComboBox[] { grid.StatBoxes[1], grid.StatBoxes[2], grid.StatBoxes[3] } : null;
        }



        /// <summary>
        /// Get all BrandBonusBoxes for displaying brandbonus
        /// </summary>
        /// <param name="itemType">Mask/Backpack/Chest/Gloves/Holster/Kneepads</param>
        /// <returns>The ContentControl Collection of Labels, or null if grid link doesn't exist </returns>
        public static TextBlock[] GetBrandBonusTextBlocks(ItemType itemType)
        {
            if (GridExists(itemType, out GearGridContent grid))
            {
                return new TextBlock[] { grid.BrandBonusTextBlocks[0], grid.BrandBonusTextBlocks[1], grid.BrandBonusTextBlocks[2] };
            }
            return null;
        }



        public static Image GetBrandImage(ItemType itemType)
        {
            if (GridExists(itemType, out GearGridContent grid))
            {
                return grid.BrandImage;
            }
            return null;
        }

        /// <summary>
        /// Returns the child with the specified targetName if it exists
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="startElement"></param>
        /// <param name="targetName"></param>
        /// <returns>FrameworkElement of first correct Child objecct</returns>
        public static T FindChildControl<T>(FrameworkElement startElement, string targetName) where T : FrameworkElement
        {
            // Directly search the children of the startElement
            foreach (var child in LogicalTreeHelper.GetChildren(startElement).OfType<FrameworkElement>())
            {
                // Check if the child matches the target type and name
                if (child.Name == targetName)
                {
                    //Console.WriteLine($"{child.Name} found");
                    return child as T;
                }
                // Recursively search in the current child's children
                /*var foundChild = FindChildControl<T>(child, targetName);
                if (foundChild != null)
                {
                    return foundChild;
                }/**/
            }
            Console.WriteLine(targetName + " Not found");
            return null;
        }



        /// <summary>
        /// Search for siblings with specified targetName inside a grid container
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="startElement"></param>
        /// <param name="targetName"></param>
        /// <returns>FrameworkElement of first correct Sibling object</returns>
        public static T FindSiblingControl<T>(FrameworkElement startElement, string targetName) where T : FrameworkElement
        {
            var parent = VisualTreeHelper.GetParent(startElement);
            // Assuming the common parent container is a Grid
            while (parent != null && !(parent is Grid))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }
            //parent isnt defined
            if (parent == null) { Console.WriteLine("Element isn't inside a grid"); return null; }

            if (!(parent is FrameworkElement parentContainer)) return null;

            foreach (var child in LogicalTreeHelper.GetChildren(parentContainer).OfType<FrameworkElement>())
            {
                if (child.Name == targetName)
                {
                    return child as T;
                }
            }
            Console.WriteLine($"{targetName} sibling not found");
            return null;
        }

    }
}
