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
using DivBuildApp.Data.Tables;


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
