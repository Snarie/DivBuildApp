using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DivBuildApp
{
    internal class GearHandler
    {
        public static List<Gear> equippedItemList = new List<Gear>();

        public static Gear CreateGear(ItemType itemType)
        {
            ComboBox itemBox = Lib.GetItemBox(itemType);
            Bonus coreBoxValue = BonusHandler.CoreBonus(itemType);
            Bonus[] sideBoxValues = BonusHandler.SideBonuses(itemType);
            if (itemBox.SelectedItem is ComboBoxBrandItem selectedItem)
            {
                StringItem stringItem = ItemHandler.ItemFromIdentity(selectedItem.Name, selectedItem.Slot);
                return new Gear(stringItem.Name, stringItem.BrandName, stringItem.Slot, stringItem.Rarity, coreBoxValue, sideBoxValues, stringItem.Talent);
            }
            return null;
        }

        public static Gear GearFromSlot(string slot)
        {
            return equippedItemList.FirstOrDefault(i => i.Slot == slot);
        }
        public static bool SetEquippedGearList()
        {
            List<ComboBox> boxes = new List<ComboBox>();
            foreach (ItemType itemType in Enum.GetValues(typeof(ItemType)))
            {
                boxes.Add(Lib.GetItemBox(itemType));
            }
            List<string> unselectedItems = new List<string>();
            foreach (ComboBox box in boxes)
            {
                if (box.SelectedValue == null)
                {
                    unselectedItems.Add(box.Name);
                }
            }
            //Check if all items are selected
            if (unselectedItems.Any())
            {
                //string errors = string.Join(", ", unselectedItems);
                //Console.WriteLine(errors);
                return false;
            }

            equippedItemList = new List<Gear>
            {
                CreateGear(ItemType.Mask),
                CreateGear(ItemType.Backpack),
                CreateGear(ItemType.Chest),
                CreateGear(ItemType.Gloves),
                CreateGear(ItemType.Holster),
                CreateGear(ItemType.Kneepads),
            };
            return true;
        }

    }
}
