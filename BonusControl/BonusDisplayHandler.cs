using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;

namespace DivBuildApp
{
    internal static class BonusDisplayHandler
    {

        public static Dictionary<BonusType, string> bonusIconType = new Dictionary<BonusType, string>();

        public static string GetIconType(BonusType bonusType)
        {
            if (bonusIconType.TryGetValue(bonusType, out string iconType))
            {
                return iconType;
            }
            return "undifined";
        }
        public static BonusDisplay BonusDisplayFromString(string str, string iconGroup = "Side-")
        {
            Bonus bonus = BonusHandler.BonusFromString(str);
            return new BonusDisplay(bonus, "Side-"+GetIconType(bonus.BonusType));
            
        }
        public static BonusDisplay BonusDisplayFromList(List<BonusDisplay> list, BonusType bonusType)
        {
            return list.FirstOrDefault(b => b.Bonus.BonusType == bonusType);
        }

        public static BonusDisplay BonusDisplayFromBox(ComboBox box)
        {
            if (box.SelectedItem is BonusDisplay bonusDisplay) return bonusDisplay;
            else return new BonusDisplay(new Bonus(BonusType.NoBonus, 1), "undifined");
        }

    }
}
