using DivBuildApp.UI;
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
        public static event EventHandler GearSet;
        public static event EventHandler GearAttributeSet;
        private static void OnGearSet()
        {
            GearSet?.Invoke(null, EventArgs.Empty);
        }
        private static void OnGearAttributeSet()
        {
            GearAttributeSet?.Invoke(null, EventArgs.Empty);
        }
        static GearHandler()
        {
            StatValueLabelControl.ValueSet += HandleValueSet;
        }
        private static void HandleValueSet(object sender, ValueSetEventArgs e)
        {
            SetGearStatAttributes(e.ItemType);
            Console.WriteLine($"GearHandler noticed StatValueLabelControl {e.ItemType}");
            OnGearAttributeSet();
        }

        public static ItemType[] gearTypes = { ItemType.Mask, ItemType.Backpack, ItemType.Chest, ItemType.Gloves, ItemType.Kneepads, ItemType.Holster };

        public static List<Gear> equippedItemList = new List<Gear>();

        public static Gear CreateGear(ItemType itemType)
        {
            ComboBox itemBox = Lib.GetItemBox(itemType);
            Bonus[] statBoxValues = BonusHandler.StatBoxBonuses(itemType);
            if (itemBox.SelectedItem is ComboBoxBrandItem selectedItem)
            {
                StringItem stringItem = ItemHandler.ItemFromIdentity(selectedItem.Name, selectedItem.Slot);
                ItemType slot = (ItemType)Enum.Parse(typeof(ItemType), stringItem.Slot, true);
                return new Gear(stringItem.Name, stringItem.BrandName, slot, stringItem.Rarity, statBoxValues, stringItem.Talent);
            }
            return null;
        }

        public static void SetGearStatAttributes(ItemType itemType)
        {
            ComboBox[] statBoxes = Lib.GetStatBoxes(itemType);
            Label[] statValues = Lib.GetStatValues(itemType);
            List<Bonus> bonusList = new List<Bonus>();
            for (int i = 0; i < 4; i++)
            {
                if (statBoxes[i].SelectedItem is BonusDisplay)
                {
                    if (statValues[i].DataContext is Bonus bonus)
                    {
                        bonusList.Add(new Bonus(bonus.BonusType, bonus.BonusValue));
                    }
                    else
                    {
                        Console.WriteLine($"{statValues[i].Name} is not a Bonus");
                    }
                }
            }
            GearFromSlot(itemType).StatAttributes = bonusList.ToArray();
        }

        public static Gear GearFromSlot(ItemType slot)
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
                OnGearSet();
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
            OnGearSet();
            return true;
        }

    }
}
