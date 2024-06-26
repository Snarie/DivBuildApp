﻿using DivBuildApp.Data.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace DivBuildApp
{
    public enum BonusType
    {
        NoBonus,

        Armor,
        Protection_from_Elites,

        Health,
        Headshot_Damage,
        Explosive_Damage,
        Total_Armor,
        Health_Regen,
        Armor_Regen,
        Armor_Regen_HPs,

        Extra_Rounds,
        Magazine_Size,
        Ammo_Capacity,
        Grenade_Capacity,
        Armor_Kit_Capacity,

        Grenade_Radius,
        Grenade_Damage,

        Critical_Hit_Damage,
        Critical_Hit_Chance,

        Skill_Tier,

        Skill_Efficiency,
        //Riviver_Hive_Efficiency

        Status_Effects,
        Stim_Efficiency,
        Tech_Efficiency,
        //Bleed_Efficiency etz.

        Repair_Skills,
        Reviver_Armor_Repair,
        Incoming_Repairs,

        Cone_Size,
        Booster_Buff_Amount,
        Skill_Stim_Charges,
        Skill_Repair_Charges,
        Skill_Stinger_Charges,

        Skill_Duration,
        Burn_Duration,
        Bleed_Duration,
        Blind_Duration,
        Booster_Buff_Duration,
        Cloud_Duration,
        Disorient_Duration,
        Pulse_Duration,
        Shock_Duration,
        Ensnare_Duration,
        Zone_Duration,
        EMP_Effect_Duration,

        Skill_Health,
        Ensnare_Health,
        Zone_Health,
        Shield_Health,

        Skill_Damage,
        Burn_Damage,
        Bleed_Damage,
        Cloud_Damage,
        EMP_Damage,

        Skill_Radius,
        Blast_Radius,
        Cload_Radius,
        Blind_Radius,
        Disorient_Radius,
        Pulse_Radius,
        Shock_Radius,
        Ensnare_Radius,
        EMP_Blast_Radius,
        EMP_Radius,

        Skill_Haste,
        Hive_Skill_Haste,
        Scanner_Pulse_Haste,
        

        Hazard_Protection,
        Electricity_Protection,
        Bleed_Resistance,
        BlindDeaf_Resistance,
        Disorient_Resistance,
        Ensnare_Resistance,
        Pulse_Resistance,
        Shock_Resistance,
        Disrupt_Resistance,
        Burn_Resistance,
        Explosive_Resistance,
        Blind_Deaf_Resistance,

        Melee_Damage,
        Shotgun_Damage,
        AR_Damage,
        Rifle_Damage,
        MMR_Damage,
        SMG_Damage,
        LMG_Damage,
        Pistol_Damage,
        Signature_Weapon_Damage,
        Weapon_Damage,

        Stability,
        Accuracy,
        Weapon_Handling,
        Reload_Speed,
        Rate_of_Fire,
        Optimal_Range,
        Swap_Speed,

        Armor_on_Kill_Stat,
        Armor_on_Kill,
        Health_on_Kill,

        Health_Damage,
        Damage_to_Armor,
        DMG_out_of_Cover,


    }


    public class EquipBonus
    {
        public int PieceNumber { get; set; }
        public Bonus Bonus { get ; set; }

        public EquipBonus(int pieceNumber, Bonus bonus)
        {
            PieceNumber = pieceNumber;
            Bonus = bonus;
        }
    }
    public class Bonus
    {
        public BonusType BonusType { get; set; }
        public double Value { get; set; }
        public string DisplayType { get; set; }

        public Bonus(BonusType bonusType, double value)
        {
            BonusType = bonusType;
            Value = value;
            DisplayType = BonusDisplayTypes.GetBonusDisplayType(BonusType);
        }
        public Bonus(BonusType bonusType, double value, string displayType)
        {
            BonusType = bonusType;
            Value = value;
            DisplayType = displayType;
        }
        public Bonus(string bonusType, string value)
        {
            BonusType = Enum.TryParse(bonusType, true, out BonusType b) ? b : BonusType.NoBonus;
            Value = double.TryParse(value, out double v) ? v : 0;
            DisplayType = BonusDisplayTypes.GetBonusDisplayType(b);
        }
        public Bonus(string format)
        {
            if (string.IsNullOrEmpty(format))
            {
                _ = Logger.LogWarning("Creating Bonus from empty string");
                BonusType = BonusType.NoBonus;
                return;
            }
            string[] parts = format.Split('=');
            BonusType = Enum.TryParse(parts[0], true, out BonusType b) ? b : BonusType.NoBonus;
            Value = double.TryParse(parts[1], out double v) ? v : 0;
            DisplayType = BonusDisplayTypes.GetBonusDisplayType(b);

        }
        public double BonusValue
        {
            get
            {
                if(DisplayType == "percentage")
                {
                    return Math.Round(Value, 1);
                }
                return Math.Round(Value, 0);
            }
        }

        public string DisplayBonusType
        {
            get
            {
                return BonusType.ToString().Replace('_', ' ');
            }
        }
        public string DisplayValue
        {
            get
            {
                return (Value > 0 ? "+" : "") + BonusValue + (DisplayType == "percentage" ? "%" : "");
                //return "" + DisplayType == "percentage" ? "%" : "";
                /*if (DisplayType == "percentage"
                {
                    return "+" + BonusValue + "%";
                }
                return "+" + BonusValue;*/
            }
        }
    }
    public class BonusDisplay
    {
        public Bonus Bonus { get; }
        public string IconType { get; }

        public BonusDisplay(Bonus bonus, string iconType)
        {
            Bonus = bonus;
            IconType = iconType;
        }
        public BonusDisplay(Bonus bonus)
        {
            Bonus = bonus;
            IconType = "Side-"+BonusDisplayHandler.GetIconType(bonus.BonusType);
        }

        

    }


}
