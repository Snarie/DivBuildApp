using DivBuildApp.Data.CsvFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace DivBuildApp.Data.Tables
{
    internal class WeaponAttributes
    {
        public static List<WeaponAttributesFormat> Attributes = new List<WeaponAttributesFormat>();
        public static void Initialize()
        {
            Attributes = CsvReader.WeaponAttributes();
            //ConvertWeaponAttributes();
        }

        public static WeaponAttributesFormat ConvertWeaponAttribute(WeaponAttributesFormat attribute)
        {
            string core = string.IsNullOrEmpty(attribute.Core) ? "wcore:Weapon_Damage" : attribute.Core;
            string main = string.IsNullOrEmpty(attribute.Main) ? "none" : attribute.Main;
            if (main == "none")
            {
                switch (attribute.List().Type)
                {
                    case WeaponType.AR:
                        main = "wmain:Health_Damage";
                        break;
                    case WeaponType.LMG:
                        main = "wmain:DMG_out_of_Cover";
                        break;
                    case WeaponType.MMR:
                        main = "wmain:Headshot_Damagee";
                        break;
                    case WeaponType.Rifle:
                        main = "wmain:Critical_hit_Damage";
                        break;
                    case WeaponType.Shotgun:
                        main = "wmain:Armor_Damage";
                        break;
                    case WeaponType.SMG:
                        main = "wmain:Critical_hit_Chance";
                        break;
                    case WeaponType.Pistol:
                        break;
                }
            }
            string side = string.IsNullOrEmpty(attribute.Side) ? "list:wside" : attribute.Side;
            string talent = string.IsNullOrEmpty(attribute.Talent) ? "list:wtalent" : attribute.Talent;
            return new WeaponAttributesFormat(attribute.Name, core, main, side, talent);
        }

    }
}
