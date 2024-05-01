using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DivBuildApp.UI
{
    internal static class ItemArmorControl
    {
        public static event EventHandler ItemArmorSet;
        private static void OnSetItemArmor()
        {
            ItemArmorSet?.Invoke(null, EventArgs.Empty);
        }
        public static Dictionary<ItemType, double> defaultArmorValues = new Dictionary<ItemType, double>()
        {
            [ItemType.Mask] = 80297,
            [ItemType.Backpack] = 130844,
            [ItemType.Chest] = 157961,
            [ItemType.Gloves] = 80297,
            [ItemType.Holster] = 111889,
            [ItemType.Kneepads] = 98726
        };
        public static Dictionary<ItemType, double> expertieceArmorValues = new Dictionary<ItemType, double>()
        {
            [ItemType.Mask] = 0,
            [ItemType.Backpack] = 0,
            [ItemType.Chest] = 0,
            [ItemType.Gloves] = 0,
            [ItemType.Holster] = 0,
            [ItemType.Kneepads] = 0
        };

        public static double GetExpertieceArmorValue()
        {
            double total = 0;
            foreach(KeyValuePair<ItemType, double> kvp in expertieceArmorValues)
            {
                total += kvp.Value;
            }
            return total;
        }
        public static void SetItemArmor(ItemType itemType)
        {
            double armorValue = defaultArmorValues[itemType];
            ComboBox multiplierBox = Lib.GetExpertieceBox(itemType);
            if (!multiplierBox.HasItems) 
            { 
                Console.WriteLine($"{multiplierBox.Name} has no items."); 
                return; 
            }
            double multiplier = 100 + (int)multiplierBox.SelectedValue;
            double multipliedValue = armorValue * multiplier / 100.0;
            expertieceArmorValues[itemType] = multipliedValue;
            OnSetItemArmor();
        }
        
        public static async Task DisplayItemArmorValues()
        {
            ItemType[] items = { ItemType.Mask, ItemType.Backpack, ItemType.Chest, ItemType.Gloves, ItemType.Holster, ItemType.Kneepads};
            


            for(int i=0; i<items.Length; i++)
            {
                Label itemArmorLabel = Lib.GetItemArmorLabel(items[i]);
                double value = expertieceArmorValues[items[i]];
                await Application.Current.Dispatcher.InvokeAsync(() =>
                {

                    int roundedValue = (int)Math.Round(value / 1000.0);
                    string text = roundedValue + "k";
                    SetItemArmorValue(itemArmorLabel, text);
                });
            }
        }
        public static void SetItemArmorValue(Label label, string text)
        {
            label.Content = text;
        }
    }
}
