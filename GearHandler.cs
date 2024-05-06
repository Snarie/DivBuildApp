using DivBuildApp.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DivBuildApp
{
    internal class GearHandler
    {
        public static void Initialize()
        {
            StatValueLabelControl.ValueSet += HandleValueSet;
            // Set eventHandlers
        }
        public static event EventHandler<GridEventArgs> GearSet;
        public static event EventHandler GearAttributeSet;
        private static void OnGearSet(GridEventArgs e)
        {
            GearSet?.Invoke(null, e);
        }
        private static void OnGearAttributeSet()
        {
            GearAttributeSet?.Invoke(null, EventArgs.Empty);
        }
        private static void HandleValueSet(object sender, GridEventArgs e)
        {
            SetGearStatAttributes(e);
            Task.Run(() => Logger.LogEvent($"StatValueLabelControl.ValueSet({e.ItemType})"));
            OnGearAttributeSet();
        }

        public static ItemType[] gearTypes = { ItemType.Mask, ItemType.Backpack, ItemType.Chest, ItemType.Gloves, ItemType.Kneepads, ItemType.Holster };

        public static Dictionary<ItemType, Gear> equippedItemDict = new Dictionary<ItemType, Gear>();
        

        private static Gear CreateGear(ItemType itemType)
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

        public static void SetGearStatAttributes(GridEventArgs e)
        {
            ComboBox[] statBoxes = e.Grid.StatBoxes;
            Label[] statValues = e.Grid.StatValues;
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
                        Task.Run(() => Logger.LogEvent($"{statValues[i].Name} is not a Bonus"));
                    }
                }
            }
            GearFromSlot(e.ItemType).StatAttributes = bonusList.ToArray();
        }

        public static Gear GearFromSlot(ItemType slot)
        {
            Dictionary<ItemType, Gear> eww = equippedItemDict;
            if (equippedItemDict.ContainsKey(slot))
            {
                return equippedItemDict[slot];
            }
            return null;
        }


        private static readonly SemaphoreSlim EquippedGearListSemaphore = new SemaphoreSlim(1);
        public static async void SetEquippedGearListAsync(GridEventArgs e)
        {
            await EquippedGearListSemaphore.WaitAsync();
            try
            {
                if (equippedItemDict.ContainsKey(e.ItemType))
                {
                    equippedItemDict[e.ItemType] = CreateGear(e.ItemType);
                }
                else
                {
                    equippedItemDict.Add(e.ItemType, CreateGear(e.ItemType));
                }

                /*bool unselected = false;
                await Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    foreach (ItemType itemType in Enum.GetValues(typeof(ItemType)))
                    {
                        ComboBox box = Lib.GetItemBox(itemType);
                        if (box.SelectedValue == null)
                        {
                            unselected = true;
                            return;
                        }
                    }
                });
                if (unselected) { };*/
                
                OnGearSet(e);
                return;
            }
            finally
            {
                EquippedGearListSemaphore.Release();
            }
        }
        public static void SetEquippedGearList(GridEventArgs e)
        {
            if (equippedItemDict.ContainsKey(e.ItemType))
            {
                equippedItemDict[e.ItemType] = CreateGear(e.ItemType);
            }
            else
            {
                equippedItemDict.Add(e.ItemType, CreateGear(e.ItemType));
            }

            /*foreach (ItemType itemType in Enum.GetValues(typeof(ItemType)))
            {
                ComboBox box = Lib.GetItemBox(itemType);
                if(box.SelectedValue == null)
                {
                    OnGearSet(e);
                    return;
                }
            }*/
            
            OnGearSet(e);
            return;
        }

    }
}
