using DivBuildApp.CsvFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DivBuildApp.Data.Tables
{
    internal static class BonusDisplayTypes
    {
        private static readonly Dictionary<BonusType, string> bonusDisplayTypes = new Dictionary<BonusType, string>();
        
        public static void Initialize()
        {
            SetBonusDisplayTypes(CsvReader.BonusDisplayType());
        }
        public static void SetBonusDisplayTypes(List<BonusDisplayTypeFormat> formats)
        {
            foreach (BonusDisplayTypeFormat format in formats)
            {
                BonusType bonusType = BonusHandler.StringToBonusType(format.Name);
                if (bonusType == BonusType.NoBonus)
                {
                    continue;
                }
                string displayType = !string.IsNullOrEmpty(format.DisplayType) ? format.DisplayType : "percentage";
                bonusDisplayTypes.Add(bonusType, displayType);
            }
        }

        public static string GetBonusDisplayType(BonusType bonusType)
        {
            if (bonusDisplayTypes.TryGetValue(bonusType, out string displayType))
            {
                return displayType;
            }
            //If bonusDisplayType was never created
            return "percentage";
        }
    }
}
