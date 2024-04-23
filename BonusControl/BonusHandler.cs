﻿//using DivBuildApp.DataContextClasses;
using CsvHelper.Configuration.Attributes;
using DivBuildApp.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using static DivBuildApp.Lib;

namespace DivBuildApp
{

    public static class BonusHandler
    {
       


        public static Dictionary<BonusType, double> activeBonusses = new Dictionary<BonusType, double>();




        
        /// <summary>
        /// Get bonus from its string equivalant.
        /// </summary>
        /// <param name="text">string in format "BonusType=Value"</param>
        /// <returns>Bonus with a BonusType and value, returns BonusType.NoBonus if bonus doesn't exist</returns>
        public static Bonus BonusFromString(string text)
        {
            string[] info = text.Split('=');
            //TODO: Add fail checks later
            return CreateBonus(info[0], info[1]);
            //BonusType name = StringToBonusType(info[0]);
            //bool successValue = double.TryParse(info[1], out double value);
            //if (!successValue) value = 0;
            //return new Bonus(name, value);
        }

        public static bool TryCreateBonus(string type, string value, out Bonus bonus)
        {
            bonus = null;   
            bool successName = Enum.TryParse(type, true, out BonusType bonusName);
            if (!successName)
            {
                return false;
            }
            bool successValue = double.TryParse(value, out double bonusValue);
            if (!successValue)
            {
                return false;
            }
            bonus = CreateBonus(bonusName, bonusValue);
            return true;
        }
        public static Bonus CreateBonus(string type, string value)
        {
            BonusType bonusType = StringToBonusType(type);
            bool successValue = double.TryParse(value, out double bonusValue);
            if (!successValue) bonusValue = 0;
            return CreateBonus(bonusType, bonusValue);
        }
        public static Bonus CreateBonus(BonusType bonusType, double bonusValue)
        {
            return new Bonus(bonusType, bonusValue);
        }
        public static BonusType StringToBonusType(string bonusName)
        {
            bool successName = Enum.TryParse(bonusName, out  BonusType name);
            if (!successName) name = BonusType.NoBonus;
            return name;
        }
        public static Bonus BonusFromList(List<Bonus> list, BonusType bonusType)
        {
            return list.FirstOrDefault(b => b.BonusType == bonusType);
        }

        public static List<Bonus> GetBrandBonus(string brandName, int pieceNumber)
        {
            bool keyExists = MainWindow.brandSets.TryGetValue(brandName, out List<EquipBonus> equipBonuses);
            if (!keyExists)
            {
                return new List<Bonus>();
            }
            //if(equipBonusses.KeyExist())
            //List<EquipBonus> equipBonusses = MainWindow.brandSets[brandName];
            List<Bonus> bonusses = new List<Bonus>();
            foreach (EquipBonus equipBonus in equipBonuses)
            {
                if (equipBonus.PieceNumber == pieceNumber)
                {
                    bonusses.Add(equipBonus.Bonus);
                }
            }
            return bonusses;
        }


        public static Bonus BonusFromBox(ComboBox box)
        {
            return BonusDisplayHandler.BonusDisplayFromBox(box).Bonus;
            //if (box.SelectedItem is BonusDisplay bonusDisplay) return bonusDisplay.Bonus;
            //else return new Bonus(BonusType.NoBonus, 1);
        }

        

        /// <summary>
        /// Gets all the side bonusses
        /// </summary>
        /// <param name="itemType">Mask/Backpack/Chest/Gloves/Holster/Kneepads</param>
        /// <returns>The Bonus collection containing all selected SideStats</returns>
        public static Bonus[] SideBonuses(ItemType itemType)
        {
            ComboBox[] comboBox = GetSideStatBoxes(itemType);
            return comboBox.Select(box => BonusFromBox(box)).ToArray();
            //return comboBox.Select(box => BonusFromBox(box)).ToArray();
        }



        /// <summary>
        /// Gets the core bonus
        /// </summary>
        /// <param name="itemType">Mask/Backpack/Chest/Gloves/Holster/Kneepads</param>
        /// <returns>The Bonus containing the selected CoreStat</returns>
        public static Bonus CoreBonus(ItemType itemType)
        {
            ComboBox comboBox = GetCoreStatBox(itemType);
            return BonusFromBox(comboBox);
        }


        public static void ResetBonuses()
        {
            //Set all bonusses back to being at 0
            activeBonusses = new Dictionary<BonusType, double>();
            foreach (BonusType bonusTypes in Enum.GetValues(typeof(BonusType)))
            {
                activeBonusses[bonusTypes] = 0;
            }
        }


        public static void CalculateBrandBonues()
        {

            Dictionary<string, int> brandLevels = new Dictionary<string, int>();
            foreach (Gear equippedItem in GearHandler.equippedItemList)
            {
                string brandName = equippedItem.BrandName;
                if (brandLevels.ContainsKey(brandName))
                {
                    brandLevels[brandName]++;
                }
                else
                {
                    brandLevels.Add(brandName, 1);
                }

                //BrandSet Bonuses
                foreach (Bonus bonus in GetBrandBonus(brandName, brandLevels[brandName]))
                {
                    activeBonusses[bonus.BonusType] += bonus.Value;
                }
                //Selected side bonuses
                foreach (Bonus bonus in equippedItem.SideAttributes)
                {
                    activeBonusses[bonus.BonusType] += bonus.Value;
                }

                //Selected core bonus
                Bonus coreBonus = equippedItem.CoreAttribute;
                activeBonusses[coreBonus.BonusType] += coreBonus.Value;

            }

            if (GearHandler.GearFromSlot("backpack") != null)
            {
                if (GearHandler.GearFromSlot("backpack").Name == "NinjaBike Backpack")
                {
                    Console.WriteLine("Correct");
                    var keysToUpdate = new List<string>(brandLevels.Keys);
                    foreach (string key in keysToUpdate)
                    {
                        brandLevels[key]++;
                        foreach (Bonus bonus in GetBrandBonus(key, brandLevels[key]))
                        {
                            activeBonusses[bonus.BonusType] += bonus.Value;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Wrong " + GearHandler.GearFromSlot("backpack").Name);
                }
            }
        }


        public static void CalculateBonuses()
        {
            Console.WriteLine("CalculateBonuses Called");
            //Set all bonusses back to 0 to not stack stats from earlier
            ResetBonuses();

            CalculateBrandBonues();
            //Watch Bonuses
            foreach (Bonus bonus in SHDWatch.WatchBonuses)
            {
                activeBonusses[bonus.BonusType] += bonus.Value;
            }


            activeBonusses[BonusType.Armor] += ItemArmorControl.GetExpertieceArmorValue();

            activeBonusses[BonusType.Armor] = Math.Floor(activeBonusses[BonusType.Armor] * (100 + activeBonusses[BonusType.Total_Armor])/100);

        }


    }


}