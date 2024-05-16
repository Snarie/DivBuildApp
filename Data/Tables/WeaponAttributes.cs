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
            CreateWeaponAttributes(CsvReader.WeaponAttributes());
            //ConvertWeaponAttributes();
        }

        public static void CreateWeaponAttributes(List<WeaponAttributesFormat> overrides)
        {
            foreach(WeaponListFormat weapon in WeaponList.WeaponBases)
            {
                WeaponAttributesFormat attributes = overrides.FirstOrDefault(w => w.Name == weapon.Name);

                if(attributes == null)
                {
                    attributes = new WeaponAttributesFormat(weapon.Name, "", "", "", "");
                }

                Attributes.Add(ConvertWeaponAttribute(attributes));
            }
        }

        public static WeaponAttributesFormat ConvertWeaponAttribute(WeaponAttributesFormat attributes)
        {
            string core = string.IsNullOrEmpty(attributes.Core) ? "wcore:Weapon_Damage" : attributes.Core;
            string main = string.IsNullOrEmpty(attributes.Main) ? "none" : attributes.Main;
            if (main == "none")
            {
                switch (attributes.List().Type)
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
            string side = string.IsNullOrEmpty(attributes.Side) ? "list:wside" : attributes.Side;
            string talent = string.IsNullOrEmpty(attributes.Talent) ? "list:wtalent" : attributes.Talent;
            return new WeaponAttributesFormat(attributes.Name, core, main, side, talent);
        }

    }
}
