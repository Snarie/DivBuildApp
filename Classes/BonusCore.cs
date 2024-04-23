using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public BonusType BonusType { get; }
        public double Value { get; }

        public Bonus(BonusType bonusType, double value)
        {
            BonusType = bonusType;
            Value = value;
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
    }


}
