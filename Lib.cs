using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;

namespace DivBuildApp
{
    internal static class Lib
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

            Image stat1_Icon = FindChildControl<Image>(grid, $"{baseName}Stat1_Icon");
            Image stat2_Icon = FindChildControl<Image>(grid, $"{baseName}Stat2_Icon");
            Image stat3_Icon = FindChildControl<Image>(grid, $"{baseName}Stat3_Icon");
            Image stat4_Icon = FindChildControl<Image>(grid, $"{baseName}Stat4_Icon");

            Image brandImage = FindChildControl<Image>(grid, $"{baseName}BrandImage");

            bool success = Enum.TryParse(baseName, out ItemType itemType);
            if (success)
            {
                GearGridLinks.Add(itemType, new GearGridContent(
                    itemBox, 
                    itemName, itemArmor, itemExpertiece,
                    new TextBlock[] { bonus1, bonus2, bonus3 },
                    new ComboBox[] { stat1, stat2, stat3, stat4},
                    new Label[] { stat1_Value, stat2_Value, stat3_Value, stat4_Value},
                    new Slider[] {stat1_Slider, stat2_Slider, stat3_Slider, stat4_Slider},
                    new Image[] { stat1_Icon, stat2_Icon, stat3_Icon, stat4_Icon },
                    brandImage
                ));;
            }
        }


        /// <summary>
        /// Gets the <see cref="GearGridContent"/> associated with the <paramref name="itemType"/>
        /// </summary>
        /// <param name="itemType">The <see cref="ItemType"/> key</param>
        /// <returns><see langword="true"/> <see cref="GearGridLinks"/> contains the specified <paramref name="itemType"/>; otherwise, <see langword="false"/></returns>
        public static bool GridExists(ItemType itemType, out GearGridContent grid)
        {
            bool exists = GearGridLinks.TryGetValue(itemType, out grid);
            return exists;
        }


        public static GearGridContent GetGridContent(ItemType itemType)
        {
            return GridExists(itemType, out GearGridContent grid) ? grid : null;
        }

        /// <summary>
        /// Gets the <see cref="ComboBox"/> that represents the ItemBox
        /// </summary>
        /// <param name="itemType">The <see cref="ItemType"/> whose grid is searched</param>
        /// <returns><see cref="ComboBox"/> if <paramref name="itemType"/>'s <see cref="GearGridContent"/> exists; otherwise, <see langword="null"/></returns>
        public static ComboBox GetItemBox(ItemType itemType)
        {
            return GridExists(itemType, out GearGridContent grid) ? grid.ItemBox : null;
        }

        /// <summary>
        /// Gets the <see cref="Label"/> that represents the ItemName
        /// </summary>
        /// <param name="itemType">The <see cref="ItemType"/> whose grid is searched</param>
        /// <returns>ItemName <see cref="Label"/> if <paramref name="itemType"/>'s <see cref="GearGridContent"/> exists; otherwise, <see langword="null"/></returns>
        public static Label GetItemNameLabel(ItemType itemType)
        {
            return GridExists(itemType, out GearGridContent grid) ? grid.ItemName : null;
        }


        /// <summary>
        /// Gets the <see cref="Label"/> that represents the ItemArmor
        /// </summary>
        /// <param name="itemType">The <see cref="ItemType"/> whose grid is searched</param>
        /// <returns>ItemArmor <see cref="Label"/> if <paramref name="itemType"/>'s <see cref="GearGridContent"/> exists; otherwise, <see langword="null"/></returns>
        public static Label GetItemArmorLabel(ItemType itemType)
        {
            return GridExists(itemType, out GearGridContent grid) ? grid.ItemArmor : null;
        }


        /// <summary>
        /// Gets the <see cref="ComboBox"/> that represents the ExpertieceBox
        /// </summary>
        /// <param name="itemType">The <see cref="ItemType"/> whose grid is searched</param>
        /// <returns>ExpertieceBox <see cref="ComboBox"/> if <paramref name="itemType"/>'s <see cref="GearGridContent"/> exists; otherwise, <see langword="null"/></returns>
        public static ComboBox GetExpertieceBox(ItemType itemType)
        {
            return GridExists(itemType, out GearGridContent grid) ? grid.ItemExpertiece : null;
        }


        /// <summary>
        /// Gets the <see cref="TextBlock"/> array that represents the BrandBonusText
        /// </summary>
        /// <param name="itemType">The <see cref="ItemType"/> whose grid is searched</param>
        /// <returns>BrandBonusText <see cref="TextBlock"/>[] if <paramref name="itemType"/>'s <see cref="GearGridContent"/> exists; otherwise, <see langword="null"/></returns>
        public static TextBlock[] GetBrandBonusTextBlocks(ItemType itemType)
        {
            if (GridExists(itemType, out GearGridContent grid))
            {
                return new TextBlock[] { grid.BrandBonusTextBlocks[0], grid.BrandBonusTextBlocks[1], grid.BrandBonusTextBlocks[2] };
            }
            return null;
        }
        /// <summary>
        /// Gets the <see cref="TextBlock"/> with the specific <paramref name="index"/> that represents the BrandBonusText
        /// </summary>
        /// <param name="itemType">The <see cref="ItemType"/> whose grid is searched</param>
        /// <param name="index">The index 0-3</param>
        /// <returns>BrandBonusText <see cref="TextBlock"/> if <paramref name="itemType"/>'s <see cref="GearGridContent"/> exists; otherwise, <see langword="null"/></returns>
        public static TextBlock GetBrandBonusTextBlock(ItemType itemType, int index)
        {
            TextBlock[] textBlocks = GetBrandBonusTextBlocks(itemType);
            return textBlocks?[index];
        }


        /// <summary>
        /// Gets the <see cref="ComboBox"/> array that represents the StatBox
        /// </summary>
        /// <param name="itemType">The <see cref="ItemType"/> whose grid is searched</param>
        /// <returns>StatBox <see cref="ComboBox"/>[] if <paramref name="itemType"/>'s <see cref="GearGridContent"/> exists; otherwise, <see langword="null"/></returns>
        public static ComboBox[] GetStatBoxes(ItemType itemType)
        {
            return GridExists(itemType, out GearGridContent grid) ? grid.StatBoxes : null;
        }
        /// <summary>
        /// Gets the <see cref="ComboBox"/> with the specific <paramref name="index"/> that represents the StatBox
        /// </summary>
        /// <param name="itemType">The <see cref="ItemType"/> whose grid is searched</param>
        /// <param name="index">The index 0-3</param>
        /// <returns>StatBox <see cref="ComboBox"/> if <paramref name="itemType"/>'s <see cref="GearGridContent"/> exists; otherwise, <see langword="null"/></returns>
        public static ComboBox GetStatBox(ItemType itemType, int index)
        {
            ComboBox[] comboBoxes = GetStatBoxes(itemType);
            return comboBoxes?[index];
        }
        /*
        /// <summary>
        /// Gets the <see cref="ComboBox"/> defined as Core Stat that represents the StatBox
        /// </summary>
        /// <param name="itemType">The <see cref="ItemType"/> whose grid is searched</param>
        /// <returns>StatBox <see cref="ComboBox"/>[] if <paramref name="itemType"/>'s <see cref="GearGridContent"/> exists; otherwise, <see langword="null"/></returns>
        public static ComboBox GetCoreStatBox(ItemType itemType)
        {
            return GridExists(itemType, out GearGridContent grid) ? grid.StatBoxes[0] : null;
        }
        /// <summary>
        /// Gets the <see cref="ComboBox"/> array defined as Side Stat that represents the StatBox
        /// </summary>
        /// <param name="itemType">The <see cref="ItemType"/> whose grid is searched</param>
        /// <returns>StatBox <see cref="ComboBox"/>[] if <paramref name="itemType"/>'s <see cref="GearGridContent"/> exists; otherwise, <see langword="null"/></returns>
        public static ComboBox[] GetSideStatBoxes(ItemType itemType)
        {
            return GridExists(itemType, out GearGridContent grid) ? new ComboBox[] { grid.StatBoxes[1], grid.StatBoxes[2], grid.StatBoxes[3] } : null;
        }*/


        /// <summary>
        /// Gets the <see cref="Label"/> array that represents the StatValue
        /// </summary>
        /// <param name="itemType">The <see cref="ItemType"/> whose grid is searched</param>
        /// <returns>StatValue <see cref="Label"/>[] if <paramref name="itemType"/>'s <see cref="GearGridContent"/> exists; otherwise, <see langword="null"/></returns>
        public static Label[] GetStatValues(ItemType itemType)
        {
            return GridExists(itemType, out GearGridContent grid) ? grid.StatValues : null;
        }
        /// <summary>
        /// Gets the <see cref="Label"/> with the specific <paramref name="index"/> that represents the StatValue
        /// </summary>
        /// <param name="itemType">The <see cref="ItemType"/> whose grid is searched</param>
        /// <param name="index">The index 0-3</param>
        /// <returns>StatValue <see cref="Label"/> if <paramref name="itemType"/>'s <see cref="GearGridContent"/> exists; otherwise, <see langword="null"/></returns>
        public static Label GetStatValue(ItemType itemType, int index)
        {
            Label[] labels = GetStatValues(itemType);
            return labels?[index];
        }


        /// <summary>
        /// Gets the <see cref="Slider"/> array that represents the StatSlider
        /// </summary>
        /// <param name="itemType">The <see cref="ItemType"/> whose grid is searched</param>
        /// <returns>StatValue <see cref="Slider"/>[] if <paramref name="itemType"/>'s <see cref="GearGridContent"/> exists; otherwise, <see langword="null"/></returns>
        public static Slider[] GetStatSliders(ItemType itemType)
        {
            return GridExists(itemType, out GearGridContent grid) ? grid.StatSliders : null;
        }
        /// <summary>
        /// Gets the <see cref="Slider"/> with the specific <paramref name="index"/> that represents the StatSlider
        /// </summary>
        /// <param name="itemType">The <see cref="ItemType"/> whose grid is searched</param>
        /// <param name="index">The index 0-3</param>
        /// <returns>StatSlider <see cref="Slider"/> if <paramref name="itemType"/>'s <see cref="GearGridContent"/> exists; otherwise, <see langword="null"/></returns>
        public static Slider GetStatSlider(ItemType itemType, int index)
        {
            Slider[] sliders = GetStatSliders(itemType);
            return sliders?[index];
        }


        /// <summary>
        /// Gets the <see cref="Image"/> array that represents the StatIcon
        /// </summary>
        /// <param name="itemType">The <see cref="ItemType"/> whose grid is searched</param>
        /// <returns>StatIcon <see cref="Image"/>[] if <paramref name="itemType"/>'s <see cref="GearGridContent"/> exists; otherwise, <see langword="null"/></returns>
        public static Image[] GetStatIcons(ItemType itemType)
        {
            return GridExists(itemType, out GearGridContent grid) ? grid.StatIcons : null;
        }
        /// <summary>
        /// Gets the <see cref="Image"/> with the specific <paramref name="index"/> that represents the StatIcon
        /// </summary>
        /// <param name="itemType">The <see cref="ItemType"/> whose grid is searched</param>
        /// <param name="index">The index 0-3</param>
        /// <returns>StatIcon <see cref="Image"/> if <paramref name="itemType"/>'s <see cref="GearGridContent"/> exists; otherwise, <see langword="null"/></returns>
        public static Image GetStatIcon(ItemType itemType, int index)
        {
            Image[] images = GetStatIcons(itemType);
            return images?[index];
        }


        /// <summary>
        /// Gets the <see cref="Image"/> that represents the BrandImage
        /// </summary>
        /// <param name="itemType">The <see cref="ItemType"/> whose grid is searched</param>
        /// <returns>BrandImage <see cref="Image"/> if <paramref name="itemType"/>'s <see cref="GearGridContent"/> exists; otherwise, <see langword="null"/></returns>
        public static Image GetBrandImage(ItemType itemType)
        {
            if (GridExists(itemType, out GearGridContent grid))
            {
                return grid.BrandImage;
            }
            return null;
        }


        /// <summary>
        /// Gets the (<see cref="string"/>)prefix that (<see cref="FrameworkElement"/>)<paramref name="element"/> starts with
        /// </summary>
        /// <param name="element">The <see cref="FrameworkElement"/> whose Prefix is searched</param>
        /// <returns><see cref="string"/> if <paramref name="element"/> starts with a correct prefix; otherwise, <see langword="default"/></returns>
        public static string GetPrefixFromElement(FrameworkElement element)
        {
            string elementName = element.Name;
            string[] prefixes = { "Mask", "Backpack", "Chest", "Gloves", "Holster", "Kneepads" };
            return prefixes.FirstOrDefault(prefix => elementName.StartsWith(prefix));

        }

        /// <summary>
        /// Gets the <see cref="ItemType"/> prefix that (<see cref="FrameworkElement"/>)<paramref name="element"/> starts with
        /// </summary>
        /// <param name="element">The <see cref="FrameworkElement"/> whose Prefix is searched</param>
        /// <returns><see cref="ItemType"/> if <paramref name="element"/> starts with a correct prefix; otherwise, <see langword="default"/></returns>
        public static ItemType GetItemTypeFromElement(FrameworkElement element)
        {
            return (ItemType)Enum.Parse(typeof(ItemType), GetPrefixFromElement(element));
        }


        /// <summary>
        /// Returns the first immediate <see cref="FrameworkElement"/> child of specified <paramref name="startElement"/> where the name equals <paramref name="targetName"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="startElement">the <see cref="FrameworkElement"/> whose child is returned</param>
        /// <param name="targetName">the name of the child ojbect</param>
        /// <returns>The child object or <see langword="null"/> if no child doesn't exist</returns>
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
        /// Returns the first <see cref="FrameworkElement"/> sibling of specified <paramref name="startElement"/> where the name equals <paramref name="targetName"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="startElement">the <see cref="FrameworkElement"/> whose sibling is returned</param>
        /// <param name="targetName">the name of the sibling ojbect</param>
        /// <returns>The sibling object or <see langword="null"/> if no sibling doesn't exist</returns>
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

        /// <summary>
        /// Returns the parent <see cref="FrameworkElement"/> of specified <paramref name="startElement"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="startElement">the <see cref="FrameworkElement"/> whose parent is returned</param>
        /// <returns>The parent object or <see langword="null"/> if parent doesn't exist</returns>
        public static T FindParentControl<T>(FrameworkElement startElement) where T : FrameworkElement
        {
            var parent = VisualTreeHelper.GetParent(startElement);
            if (parent is FrameworkElement parentContainer)
            {
                return parentContainer as T;
            }
            return null;
        }


    }
}
