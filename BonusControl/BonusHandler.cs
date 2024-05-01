//using DivBuildApp.DataContextClasses;
using CsvHelper.Configuration.Attributes;
using DivBuildApp.CsvFormats;
using DivBuildApp.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace DivBuildApp
{
    public class BonusSource
    {
        public string Source { get; set; }
        public Bonus Bonus { get; set; }

        public BonusSource(string source, Bonus bonus) 
        {
            Source = source;
            Bonus = bonus;
        }

    }
    internal static class BonusHandler
    {

        public static Dictionary<string, List<EquipBonus>> brandSets = new Dictionary<string, List<EquipBonus>>();

        private static readonly Dictionary<BonusType, string> bonusDisplayTypes = new Dictionary<BonusType, string>();

        public static void SetBonusDisplayTypes(List<BonusDisplayTypeFormat> formats)
        {
            foreach(BonusDisplayTypeFormat format in formats)
            {
                BonusType bonusType = StringToBonusType(format.Name);
                if(bonusType == BonusType.NoBonus)
                {
                    continue;
                }
                string displayType = !string.IsNullOrEmpty(format.DisplayType) ? format.DisplayType : "percentage";
                bonusDisplayTypes.Add(bonusType, displayType);
            }
        }

        public static string GetBonusDisplayType(BonusType bonusType)
        {
            if(bonusDisplayTypes.TryGetValue(bonusType, out string displayType))
            {
                return displayType;
            }
            //If bonusDisplayType was never created
            return "percentage";
        }

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
            bool keyExists = brandSets.TryGetValue(brandName, out List<EquipBonus> equipBonuses);
            if (!keyExists)
            {
                return new List<Bonus>();
            }
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
        /// Gets all the StatBox bonusses
        /// </summary>
        /// <param name="itemType">Mask/Backpack/Chest/Gloves/Holster/Kneepads</param>
        /// <returns>The Bonus collection containing all selected Stats</returns>
        public static Bonus[] StatBoxBonuses(ItemType itemType)
        {
            ComboBox[] comboBoxes = Lib.GetStatBoxes(itemType);
            return comboBoxes.Select(box => BonusFromBox(box)).ToArray();
        }


    }


}
