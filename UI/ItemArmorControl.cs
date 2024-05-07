﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DivBuildApp.UI
{
    internal static class ItemArmorControl
    {
        public static void Initialize()
        {
            // Set
            foreach(ItemType itemType in ItemHandler.GearTypes)
            {
                runners[itemType] = new SynchronizedTaskRunner(TimeSpan.FromSeconds(0.3));
            }
        }
        public static event EventHandler ItemArmorSet;
        private static void OnSetItemArmor()
        {
            ItemArmorSet?.Invoke(null, EventArgs.Empty);
        }
        private static readonly Dictionary<ItemType, double> defaultArmorValues = new Dictionary<ItemType, double>()
        {
            [ItemType.Mask] = 80297,
            [ItemType.Backpack] = 130844,
            [ItemType.Chest] = 157961,
            [ItemType.Gloves] = 80297,
            [ItemType.Holster] = 111889,
            [ItemType.Kneepads] = 98726
        };
        private static readonly Dictionary<ItemType, double> expertieceArmorValues = new Dictionary<ItemType, double>()
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
            //Console.WriteLine($"{expertieceArmorValues[ItemType.Mask]}/{expertieceArmorValues[ItemType.Backpack]}/{expertieceArmorValues[ItemType.Chest]}/{expertieceArmorValues[ItemType.Gloves]}/{expertieceArmorValues[ItemType.Holster]}/{expertieceArmorValues[ItemType.Kneepads]}");
            return expertieceArmorValues.Values.Sum();
        }
        public static void SetItemArmor(GridEventArgs e)
        {
            double armorValue = defaultArmorValues[e.ItemType];
            ComboBox multiplierBox = e.Grid.ItemExpertiece;
            if (!multiplierBox.HasItems)
            {
                Task.Run(() => Logger.LogError($"{multiplierBox.Name} has no items"));
                return; 
            }
            double multiplier = 100 + (int)multiplierBox.SelectedValue;
            double multipliedValue = armorValue * multiplier / 100.0;
            expertieceArmorValues[e.ItemType] = multipliedValue;
            OnSetItemArmor();
            Task.Run(() => DisplayItemArmorValue(e));
        }

        private static async Task DisplayItemArmorValue(GridEventArgs e)
        {
            Label itemArmorLabel = e.Grid.ItemArmor;
            double value = expertieceArmorValues[e.ItemType];

            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                int roundedValue = (int)Math.Round(value / 1000.0);
                string text = roundedValue + "k";
                itemArmorLabel.Content = text;
            });
        }

        private static readonly Dictionary<ItemType, SynchronizedTaskRunner> runners = new Dictionary<ItemType, SynchronizedTaskRunner>();
        private static readonly SemaphoreSlim globalSemaphore = new SemaphoreSlim(1, 1);
        public static async Task SetItemArmorAsync(GridEventArgs e)
        {
            SynchronizedTaskRunner runner = runners[e.ItemType];
            if(!await runner.TryEnterAsync())
            {
                Console.WriteLine("Exiting early due to queue for " + e.ItemType);
                return;
            }
            try
            {
                runner.ResetLastStartTime();
                await globalSemaphore.WaitAsync();
                try
                {
                    // Fetch default armor values and UI controls
                    double armorValue = defaultArmorValues[e.ItemType];
                    ComboBox multiplierBox = e.Grid.ItemExpertiece;
                    double multiplier = 100;

                    // UI thread work to fetch selected value from combo box
                    await Application.Current.Dispatcher.InvokeAsync(() =>
                    {
                        if (!multiplierBox.HasItems)
                        {
                            _ = Logger.LogError($"{multiplierBox.Name} has no items");
                            return; // Exit if the combo box is empty
                        }
                        // Adjust multiplier based on selected value
                        multiplier += (int)multiplierBox.SelectedValue;
                    });

                    double multipliedValue = armorValue * multiplier / 100.0; // Calculate the new armor value
                    expertieceArmorValues[e.ItemType] = multipliedValue; // Store calculated value
                    OnSetItemArmor(); // Notify other parts of the program
                    await DisplayItemArmorValue(e); // Update UI with new value
                }
                finally
                {
                    globalSemaphore.Release();
                }

                 
            }
            finally
            {
                runner.Release();
            }



        }

    }
}
