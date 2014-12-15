// <copyright file="Converter.cs" company="Secondnorth.com">
//     Secondnorth.com. All rights reserved.
// </copyright>
// <author>Me</author>

namespace WoWGuildOrganizer
{
    #region Includes

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    #endregion

    /// <summary>
    /// Converter Class
    /// </summary>
    public static class Converter
    {
        /// <summary>
        /// Enumeration for the item slot values
        /// </summary>
        public enum ItemSlot
        {
            /// <summary>
            /// Head Slot
            /// </summary>
            head = 0,

            /// <summary>
            /// Neck Slot
            /// </summary>
            neck = 1,

            /// <summary>
            /// Shoulder Slot
            /// </summary>
            shoulder = 2,

            /// <summary>
            /// Back Slot
            /// </summary>
            back = 3,

            /// <summary>
            /// Chest Slot
            /// </summary>
            chest = 4,

            /// <summary>
            /// Tabard Slot
            /// </summary>
            tabard = 5,

            /// <summary>
            /// Shirt Slot
            /// </summary>
            shirt = 6,

            /// <summary>
            /// Wrist Slot
            /// </summary>
            wrist = 7,

            /// <summary>
            /// Hands Slot
            /// </summary>
            hands = 8,

            /// <summary>
            /// Waist Slot
            /// </summary>
            waist = 9,

            /// <summary>
            /// Legs Slot
            /// </summary>
            legs = 10,

            /// <summary>
            /// Feet Slot
            /// </summary>
            feet = 11,

            /// <summary>
            /// Finger Slot
            /// </summary>
            finger1 = 12,

            /// <summary>
            /// Finger Slot
            /// </summary>
            finger2 = 13,

            /// <summary>
            /// Trinket Slot
            /// </summary>
            trinket1 = 14,

            /// <summary>
            /// Trinket Slot
            /// </summary>
            trinket2 = 15,

            /// <summary>
            /// Main Hand Slot
            /// </summary>
            mainhand = 16,

            /// <summary>
            /// Off Hand Slot
            /// </summary>
            offhand = 17
        }

        /// <summary>
        /// A list of all the Classes, with the proper spelling
        /// </summary>
        public enum WoWClass
        {
            /// <summary>
            /// Death Knight Class
            /// </summary>
            DeathKnight,

            /// <summary>
            /// Druid Class
            /// </summary>
            Druid,

            /// <summary>
            /// Hunter Class
            /// </summary>
            Hunter,

            /// <summary>
            /// Mage Class
            /// </summary>
            Mage,

            /// <summary>
            /// Monk Class
            /// </summary>
            Monk,

            /// <summary>
            /// Paladin Class
            /// </summary>
            Paladin,

            /// <summary>
            /// Priest Class
            /// </summary>
            Priest,

            /// <summary>
            /// Rogue Class
            /// </summary>
            Rogue,

            /// <summary>
            /// Shaman Class
            /// </summary>
            Shaman,

            /// <summary>
            /// Warlock Class
            /// </summary>
            Warlock,

            /// <summary>
            /// Warrior Class
            /// </summary>
            Warrior
        }

        /// <summary>
        /// A list of all the specializations for every class in the game
        /// </summary>
        public enum WoWSpecs
        {
            /// <summary>
            /// Survival Specialization
            /// </summary>
            Survival,

            /// <summary>
            /// Marksmanship Specialization
            /// </summary>
            Marksmanship,

            /// <summary>
            /// Beast Mastery Specialization
            /// </summary>
            BeastMastery,

            /// <summary>
            /// Arms Specialization
            /// </summary>
            Arms,

            /// <summary>
            /// Fury Specialization
            /// </summary>
            Fury,

            /// <summary>
            /// Protection Specialization
            /// </summary>
            Protection
        }

        /// <summary>
        /// A list of all 3 roles in the game
        /// </summary>
        public enum WoWRole
        {
            /// <summary>
            /// Damage Per Second
            /// </summary>
            DPS,

            /// <summary>
            /// Healing Role
            /// </summary>
            HEALING,

            /// <summary>
            /// Tanking Role
            /// </summary>
            TANK
        }

        /// <summary>
        /// Converts a number from Blizzard's web site into a class type
        /// </summary>
        /// <param name="i">the numeric value of a wow class</param>
        /// <returns>string value of the number passed in</returns>
        public static string ConvertClass(string i)
        {
            string converted = i;

            switch (i)
            {
                case "1":
                    converted = "Warrior";
                    break;
                case "2":
                    converted = "Paladin";
                    break;
                case "3":
                    converted = "Hunter";
                    break;
                case "4":
                    converted = "Rogue";
                    break;
                case "5":
                    converted = "Priest";
                    break;
                case "6":
                    converted = "Death Knight";
                    break;
                case "7":
                    converted = "Shaman";
                    break;
                case "8":
                    converted = "Mage";
                    break;
                case "9":
                    converted = "Warlock";
                    break;
                case "10":
                    converted = "Monk";
                    break;
                case "11":
                    converted = "Druid";
                    break;                
                default:
                    converted = "error: " + i;
                    break;
            }

            return converted;
        }

        /// <summary>
        /// Converts a number from Blizzard's web site into a Race type
        /// </summary>
        /// <param name="i">The numeric value of a race passed in</param>
        /// <returns>string value of the number passed in</returns>
        public static string ConvertRace(string i)
        {
            string converted = "error";

            switch (i)
            {
                case "1":
                    converted = "Human";
                    break;
                case "2":
                    converted = "Orc";
                    break;
                case "3":
                    converted = "Dwarf";
                    break;
                case "4":
                    converted = "Night Elf";
                    break;
                case "5":
                    converted = "Undead";
                    break;
                case "6":
                    converted = "Tauren";
                    break;
                case "7":
                    converted = "Gnome";
                    break;
                case "8":
                    converted = "Troll";
                    break;
                case "9":
                    converted = "Goblin";
                    break;
                case "10":
                    converted = "Blood Elf";
                    break;
                case "11":
                    converted = "Draenei";
                    break;
                case "22":
                    converted = "Worgen";
                    break;
                case "26":
                    converted = "Pandaren";
                    break;
                default:
                    converted = "error";
                    break;
            }

            return converted;
        }

        /// <summary>
        /// Converts a number from Blizzard's web site into a Item Class type
        /// </summary>
        /// <param name="i">numeric value of an item class</param>
        /// <returns>string conversion of that passed in number</returns>
        public static string ConvertItemClass(int i)
        {
            string converted = "error";

            switch (i)
            {
                case 0:
                    converted = "Consumable";
                    break;
                case 1:
                    converted = "Container";
                    break;
                case 2:
                    converted = "Weapon";
                    break;
                case 4:
                    converted = "Armor";
                    break;
                case 5:
                    converted = "Reagent";
                    break;
                case 6:
                    converted = "Projectile";
                    break;
                case 7:
                    converted = "Trade Goods";
                    break;
                case 9:
                    converted = "Recipe";
                    break;
                case 11:
                    converted = "Quiver";
                    break;
                case 12:
                    converted = "Quest";
                    break;
                case 13:
                    converted = "Key";
                    break;
                case 15:
                    converted = "Miscellaneous";
                    break;
            }

            return converted;
        }

        /// <summary>
        /// Converts a number from Blizzard's web site into a Item Sub Class type
        /// </summary>
        /// <param name="wowClass">numeric value of an class</param>
        /// <param name="i">numeric value</param>
        /// <returns>string conversion of that passed in number</returns>
        public static string ConvertItemSubClass(int wowClass, int i)
        {
            string converted = "error";

            if (wowClass == 2)
            {
                // Weapon
                switch (i)
                {
                    case 0:
                        converted = "1 Axe"; // 1 Handed
                        break;
                    case 1:
                        converted = "2 Axe"; // 2 Handed
                        break;
                    case 2:
                        converted = "Bow";
                        break;
                    case 3:
                        converted = "Rifle";
                        break;
                    case 4:
                        converted = "1 Mace"; // 1 handed
                        break;
                    case 5:
                        converted = "2 Mace"; // 2 handed
                        break;
                    case 6:
                        converted = "Polearm";
                        break;
                    case 7:
                        converted = "1 Sword"; // 1 handed
                        break;
                    case 8:
                        converted = "2 Sword"; // 2 handed
                        break;
                    case 10:
                        converted = "Staff";
                        break;
                    case 11:
                        converted = "1 Exotic"; // 1 handed
                        break;
                    case 12:
                        converted = "2 Exotic"; // 2 handed
                        break;
                    case 13:
                        converted = "Fist Weapon";
                        break;
                    case 14:
                        converted = "Miscellaneous Weapon"; // Blacksmith Hammer, Mining Pick, etc.
                        break;
                    case 15:
                        converted = "Dagger";
                        break;
                    case 16:
                        converted = "Thrown";
                        break;
                    case 17:
                        converted = "Spear";
                        break;
                    case 18:
                        converted = "Crossbow";
                        break;
                    case 19:
                        converted = "Wand";
                        break;
                    case 20:
                        converted = "Fishing Pole";
                        break;
                }
            }
            else if (wowClass == 4)
            {
                // Armor           
                switch (i)
                {
                    case 0:
                        converted = "Micellaneous";
                        break;
                    case 1:
                        converted = "Cloth";
                        break;
                    case 2:
                        converted = "Leather";
                        break;
                    case 3:
                        converted = "Mail";
                        break;
                    case 4:
                        converted = "Plate";
                        break;
                    case 6:
                        converted = "Shield";
                        break;
                }
            }

            return converted;
        }

        /// <summary>
        /// Converts a number from Blizzard's web site into an Inventory type
        /// </summary>
        /// <param name="i">numeric value</param>
        /// <returns>string conversion of that passed in number</returns>
        public static string ConvertInventoryType(int i)
        {
            string converted = "error";

            switch (i)
            {
                case 1:
                    converted = "head";
                    break;
                case 2:
                    converted = "neck";
                    break;
                case 3:
                    converted = "shoulder";
                    break;
                case 4:
                    converted = "shirt";
                    break;
                case 5:
                    converted = "chest";
                    break;
                case 6:
                    converted = "waist"; // Belt
                    break;
                case 7:
                    converted = "legs"; // Pants
                    break;
                case 8:
                    converted = "feet"; // Boots
                    break;
                case 9:
                    converted = "wrist"; // Bracers
                    break;
                case 10:
                    converted = "hands"; // Gloves
                    break;
                case 11:
                    converted = "finger"; // ring
                    break;
                case 12:
                    converted = "trinket";
                    break;
                case 13:
                    converted = "one hand"; // dagger
                    converted = "mainHand";
                    break;
                case 14:
                    converted = "off hand";
                    converted = "offHand";
                    break;
                case 15:
                    converted = "bow";
                    converted = "mainHand";
                    break;
                case 16:
                    converted = "back";
                    break;
                case 17:
                    converted = "two hand";
                    converted = "mainHand";
                    break;
                case 18:
                    converted = "bag"; // incl. quivers
                    break;
                case 19:
                    converted = "tabard";
                    break;
                case 20:
                    converted = "robe";
                    converted = "chest";
                    break;
                case 21:
                    converted = "main hand";
                    break;
                case 22:
                    converted = "off hand"; // misc items
                    break;
                case 23:
                    converted = "tome";
                    converted = "offHand";
                    break;
                case 24:
                    converted = "ammunition";
                    break;
                case 25:
                    converted = "thrown";
                    break;
                case 26:
                    converted = "gun";
                    converted = "mainHand";
                    break;
            }

            /*
            switch (In)
            {
                case 1:
                    Converted = "head";
                    break;
                case 2:
                    Converted = "neck";
                    break;
                case 3:
                    Converted = "shoulder";
                    break;
                case 4:
                    Converted = "shirt";
                    break;
                case 5:
                    Converted = "chest";
                    break;
                case 6:
                    Converted = "waist"; // Belt
                    break;
                case 7:
                    Converted = "legs"; // Pants
                    break;
                case 8:
                    Converted = "feet"; // Boots
                    break;
                case 9:
                    Converted = "wrists"; // Bracers
                    break;
                case 10:
                    Converted = "hands"; // Gloves
                    break;
                case 11:
                    Converted = "finger"; // ring
                    break;
                case 12:
                    Converted = "trinket";
                    break;
                case 13:
                    Converted = "one hand";
                    break;
                case 14:
                    Converted = "off hand";
                    break;
                case 15:
                    Converted = "bow";
                    break;
                case 16:
                    Converted = "back";
                    break;
                case 17:
                    Converted = "two hand";
                    break;
                case 18:
                    Converted = "bag"; // incl. quivers
                    break;
                case 19:
                    Converted = "tabard";
                    break;
                case 20:
                    Converted = "robe";
                    break;
                case 21:
                    Converted = "main hand";
                    break;
                case 22:
                    Converted = "off hand"; // misc items
                    break;
                case 23:
                    Converted = "tome";
                    break;
                case 24:
                    Converted = "ammunition";
                    break;
                case 25:
                    Converted = "thrown";
                    break;
                case 26:
                    Converted = "gun";
                    break;
            }*/

            return converted;
        }

        /// <summary>
        /// Converts a number from Blizzard's web site into a Stat type
        /// </summary>
        /// <param name="i">numeric value</param>
        /// <returns>string conversion of that passed in number</returns>
        public static string ConvertStat(int i)
        {
            string stat = string.Empty;

            switch (i)
            {
                case 3:
                    stat = "Agility";
                    break;
                case 4:
                    stat = "Strength";
                    break;
                case 5:
                    stat = "Intellect";
                    break;
                case 6:
                    stat = "Spirit";
                    break;
                case 7:
                    stat = "Stamina";
                    break;
                case 13:
                    stat = "Dodge";
                    break;
                case 14:
                    stat = "Parry";
                    break;
                case 31:
                    stat = "Hit";
                    break;
                case 32:
                    stat = "Critical Strike";
                    break;
                case 36:
                    stat = "Haste";
                    break;
                case 37:
                    stat = "Expertise";
                    break;
                case 40:
                    stat = "Versatility";
                    break;
                case 45:
                    stat = "Spell Power";
                    break;
                case 49:
                    stat = "Mastery";
                    break;
                case 50:
                    stat = "Bonus Armor";
                    break;
                case 57:
                    stat = "Unk57";
                    break;
                case 59:
                    stat = "Multistrike";
                    break;
                case 72:
                    stat = "Strength";
                    break;
                case 73:
                    stat = "Versatility";
                    break;
                case 74:
                    stat = "Str/Int";
                    break;
                default:
                    stat = "Unknown" + i.ToString();
                    break;
            }

            return stat;
        }
                
        // ** OTHERS ***
        /*
         * 
         * */
    }
}
