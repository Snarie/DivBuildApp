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
        public static ItemType[] gearTypes = { ItemType.Mask, ItemType.Backpack, ItemType.Chest, ItemType.Gloves, ItemType.Kneepads, ItemType.Holster };

        public static List<Gear> equippedItemList = new List<Gear>();

        public static Gear CreateGear(ItemType itemType)
        {
            ComboBox itemBox = Lib.GetItemBox(itemType);
            Bonus[] statBoxValues = BonusHandler.StatBoxBonuses(itemType);
            if (itemBox.SelectedItem is ComboBoxBrandItem selectedItem)
            {
                StringItem stringItem = ItemHandler.ItemFromIdentity(selectedItem.Name, selectedItem.Slot);
                return new Gear(stringItem.Name, stringItem.BrandName, stringItem.Slot, stringItem.Rarity, statBoxValues, stringItem.Talent);
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
            if (unselectedItems.Any())
            {
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
