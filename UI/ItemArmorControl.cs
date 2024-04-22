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
        public static async Task SetItemArmorValues()
        {
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                SetItemArmorValue(ItemType.Mask);
                SetItemArmorValue(ItemType.Backpack);
                SetItemArmorValue(ItemType.Chest);
                SetItemArmorValue(ItemType.Gloves);
                SetItemArmorValue(ItemType.Holster);
                SetItemArmorValue(ItemType.Kneepads);
            });
        }
        public static void SetItemArmorValue(ItemType itemType)
        {
            Label itemArmorLabel = Lib.GetItemArmorLabel(itemType);
            int multiplier = 100 + (int)Lib.GetExpertieceBox(itemType).SelectedValue; //Number ranging between 100 and 125
            int armorValue = defaultArmorValues[itemType];

            int roundedValue = (int)Math.Round(armorValue * multiplier / 100000.0);

            itemArmorLabel.Content = roundedValue+"k";
        }
    }
}
