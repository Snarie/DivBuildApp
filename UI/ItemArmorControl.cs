using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DivBuildApp.UI
{
    public static class ItemArmorControl
    {

        public static Dictionary<ItemType, int> defaultArmorValues = new Dictionary<ItemType, int>()
        {
            [ItemType.Mask] = 80297,
            [ItemType.Backpack] = 130844,
            [ItemType.Chest] = 157961,
            [ItemType.Gloves] = 80297,
            [ItemType.Holster] = 111889,
            [ItemType.Kneepads] = 98726
        };
        public static Dictionary<ItemType, int> expertieceArmorValues = new Dictionary<ItemType, int>()
        {
            [ItemType.Mask] = 80297,
            [ItemType.Backpack] = 130844,
            [ItemType.Chest] = 157961,
            [ItemType.Gloves] = 80297,
            [ItemType.Holster] = 111889,
            [ItemType.Kneepads] = 98726
        };

        public static int GetExpertieceArmorValue()
        {
            int total = 0;
            foreach(KeyValuePair<ItemType, int> kvp in expertieceArmorValues)
            {
                total += kvp.Value;
            }
            return total;
        }

        public static async Task SetItemArmorValues()
        {
            ItemType[] items = { ItemType.Mask, ItemType.Backpack, ItemType.Chest, ItemType.Gloves, ItemType.Holster, ItemType.Kneepads};
            Console.WriteLine("wewe");
            for(int i=0; i<items.Length; i++)
            {
                Label itemArmorLabel = Lib.GetItemArmorLabel(items[i]);
                ComboBox multiplierBox = Lib.GetExpertieceBox(items[i]);
                int armorValue = defaultArmorValues[items[i]];
                await Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    int multiplier = 100 + (int)multiplierBox.SelectedValue; //Number ranging between 100 and 125
                    int multipliedValue = (int)Math.Round(armorValue * multiplier / 100.0);
                    int roundedValue = (int)Math.Round(multipliedValue / 1000.0);
                    string text = roundedValue + "k";
                    expertieceArmorValues[items[i]] = multipliedValue;
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
