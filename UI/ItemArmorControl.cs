﻿using System;
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
            [ItemType.Mask] = 80297,
            [ItemType.Backpack] = 130844,
            [ItemType.Chest] = 157961,
            [ItemType.Gloves] = 80297,
            [ItemType.Holster] = 111889,
            [ItemType.Kneepads] = 98726
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
            Console.WriteLine(multiplierBox.Name + " " + multiplierBox.HasItems);
            if (!multiplierBox.HasItems) return;
            double multiplier = 100 + (int)multiplierBox.SelectedValue;
            double multipliedValue = armorValue * multiplier / 100.0;
            expertieceArmorValues[itemType] = multipliedValue;
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
