using DivBuildApp.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

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

            OnGearAttributeSet();
        }


        private static readonly SynchronizedGroupedTaskRunner<ItemType> GearAttibuteSetTaskRunner = new SynchronizedGroupedTaskRunner<ItemType>(TimeSpan.FromSeconds(0.1));
        public static async void SetGearStatAttributesAsync(GridEventArgs e)
        {
            await GearAttibuteSetTaskRunner.ExecuteTaskAsync(e.ItemType, async () =>
            {
                ComboBox[] statBoxes = e.Grid.StatBoxes;
                Label[] statValues = e.Grid.StatValues;
                List<Bonus> bonusList = new List<Bonus>();

                for (int i = 0; i < 4; i++)
                {
                    object selectedItem = null;
                    await Application.Current.Dispatcher.InvokeAsync(() =>
                    {
                        selectedItem = statBoxes[i].SelectedItem;
                    });
                    if (!(selectedItem is BonusDisplay))
                    {
                        continue;
                    }

                    object dataContext = null;
                    await Application.Current.Dispatcher.InvokeAsync(() =>
                    {
                        dataContext = statValues[i].DataContext;
                    });
                    if (!(dataContext is Bonus bonus))
                    {
                        _ = Logger.LogEvent($"{statValues[i].Name} is not a Bonus");
                        continue;
                    }

                    bonusList.Add(new Bonus(bonus.BonusType, bonus.BonusValue));
                }
                GearFromSlot(e.ItemType).StatAttributes = bonusList.ToArray();
            });
            
        }


        private static readonly SynchronizedGroupedTaskRunner<ItemType> GearSetTaskRunner = new SynchronizedGroupedTaskRunner<ItemType>(TimeSpan.FromSeconds(0.05));
        public static async void SetEquippedGearListAsync(GridEventArgs e)
        {
            await GearSetTaskRunner.ExecuteTaskAsync(e.ItemType, () =>
            {
                if (equippedItemDict.ContainsKey(e.ItemType))
                {
                    equippedItemDict[e.ItemType] = CreateGear(e.ItemType);
                }
                else
                {
                    equippedItemDict.Add(e.ItemType, CreateGear(e.ItemType));
                }
                OnGearSet(e); //Notify other parts of the program

                // Return a completed task because lambda must return a Task
                return Task.CompletedTask;
            });
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
    }
}
