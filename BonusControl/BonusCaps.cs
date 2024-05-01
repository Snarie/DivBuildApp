using DivBuildApp.CsvFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DivBuildApp.BonusControl
{
    internal static class BonusCaps
    {

        public static List<BonusDisplay> GearCoreAttributes = new List<BonusDisplay>();
        public static List<BonusDisplay> GearSideAttributes = new List<BonusDisplay>();
        public static List<BonusDisplay> GearModAttributes = new List<BonusDisplay>();
        public static List<BonusDisplay> WeaponCoreAttributes = new List<BonusDisplay>();
        public static List<BonusDisplay> WeaponPrimaryAttributes = new List<BonusDisplay>();
        public static List<BonusDisplay> WeaponSecondaryAttributes = new List<BonusDisplay>();
        public static void CreateBonusCapsFromData(List<BonusCapsFormat> bonusCapsFormat)
        {
            foreach (BonusCapsFormat bonusCaps in bonusCapsFormat)
            {

                BonusType name = BonusHandler.StringToBonusType(bonusCaps.Name);

                if (name == BonusType.NoBonus) continue;
                BonusDisplayHandler.bonusIconType.Add(name, bonusCaps.IconType);
                TryCreateBonusCap(name, bonusCaps.GearCore, "Core-" + bonusCaps.IconType, GearCoreAttributes);
                TryCreateBonusCap(name, bonusCaps.GearSide, "Side-" + bonusCaps.IconType, GearSideAttributes);
                TryCreateBonusCap(name, bonusCaps.Mod, "Mod-" + bonusCaps.IconType, GearModAttributes);
                TryCreateBonusCap(name, bonusCaps.WeaponCore, bonusCaps.IconType, WeaponCoreAttributes);
                TryCreateBonusCap(name, bonusCaps.WeaponPrimary, bonusCaps.IconType, WeaponPrimaryAttributes);
                TryCreateBonusCap(name, bonusCaps.WeaponSide, bonusCaps.IconType, WeaponSecondaryAttributes);

            }
        }
        private static void TryCreateBonusCap(BonusType name, string stringValue, string iconType, List<BonusDisplay> holder)
        {
            if (string.IsNullOrEmpty(stringValue)) return;
            bool canParse = double.TryParse(stringValue, out double value);
            if (!canParse) return;
            BonusDisplay bonus = new BonusDisplay(new Bonus(name, value), iconType);
            holder.Add(bonus);

        }
    }
}
