using DivBuildApp.CsvFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DivBuildApp.Data.Tables
{
    internal static class BrandSets
    {

        private static Dictionary<string, List<EquipBonus>> SetBonuses = new Dictionary<string, List<EquipBonus>>();

        public static void Initialize()
        {
            SetBrandSets(CsvReader.BrandBonuses());
        }
        private static void SetBrandSets(List<BrandBonusesFormat> brandBonusesList)
        {
            SetBonuses.Clear();
            foreach (BrandBonusesFormat brandBonuses in brandBonusesList)
            {
                if (SetBonuses.ContainsKey(brandBonuses.Name))
                {
                    Task.Run(() => Logger.LogWarning($"Duplicate brand name '{brandBonuses.Name}', skipping duplicate"));
                    continue; //Skip if a brand with the same name already exists.
                }
                SetBonuses.Add(brandBonuses.Name, GetBrandBonuses(brandBonuses));
            }
        }
        private static List<EquipBonus> GetBrandBonuses(BrandBonusesFormat brandBonuses)
        {
            string[] slots = { brandBonuses.Slot1, brandBonuses.Slot2, brandBonuses.Slot3 };
            List<EquipBonus> equipBonuses = new List<EquipBonus>();

            for (int i = 0; i < 3; i++)
            {
                foreach (string bonusString in slots[i].Split('+'))
                {
                    string[] bonusParts = bonusString.Split('=');
                    if (bonusParts.Length != 2) continue;
                    Bonus bonus = new Bonus(bonusParts[0], bonusParts[1]);
                    if (bonus.BonusType == BonusType.NoBonus)
                    {
                        Task.Run(() => Logger.LogError($"BonusType doesn't exist '{bonusParts[0]}'='{bonusParts[1]}' {brandBonuses.Name}"));
                        continue;
                    }
                    equipBonuses.Add(new EquipBonus(i + 1, bonus));
                }
            }
            return equipBonuses;

        }
        public static List<Bonus> GetBrandBonus(string brandName, int pieceNumber)
        {
            bool keyExists = SetBonuses.TryGetValue(brandName, out List<EquipBonus> equipBonuses);
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

    }
}
