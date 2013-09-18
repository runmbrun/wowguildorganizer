using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WoWGuildOrganizer
{
    static class Converter
    {
        // TODO: finish this...
        enum ItemSlot
        {
            head = 0,
            neck = 1,
            shoulder = 2,
            back = 3,
            chest = 4,
            tabard = 5,
            shirt = 6,
            wrist = 7,
            hands = 8,
            waist = 9,
            legs = 10,
            feet = 11,
            finger1 = 12,
            finger2 = 13,
            trinket1 = 14,
            trinket2 = 15,
            mainhand = 16,
            offhand = 17
        }

        enum WoWClass
        {

        }

        /// <summary>
        /// Converts a number from Blizzard's web site into a class type
        /// </summary>
        /// <param name="inClass"></param>
        /// <returns></returns>
        static public String ConvertClass(String In)
        {
            String Converted = In;


            switch (In)
            {
                case "1":
                    Converted = "Warrior";
                    break;
                case "2":
                    Converted = "Paladin";
                    break;
                case "3":
                    Converted = "Hunter";
                    break;
                case "4":
                    Converted = "Rogue";
                    break;
                case "5":
                    Converted = "Priest";
                    break;
                case "6":
                    Converted = "Death Knight";
                    break;
                case "7":
                    Converted = "Shaman";
                    break;
                case "8":
                    Converted = "Mage";
                    break;
                case "9":
                    Converted = "Warlock";
                    break;
                case "10":
                    Converted = "Monk";
                    break;
                case "11":
                    Converted = "Druid";
                    break;                
                default:
                    Converted = "error: " + In;
                    break;
            }

            return Converted;
        }

        /// <summary>
        /// Converts a number from Blizzard's web site into a Race type
        /// </summary>
        /// <param name="inClass"></param>
        /// <returns></returns>
        static public String ConvertRace(String In)
        {
            String Converted = "error";


            switch (In)
            {
                case "1":
                    Converted = "Human";
                    break;
                case "2":
                    Converted = "Orc";
                    break;
                case "3":
                    Converted = "Dwarf";
                    break;
                case "4":
                    Converted = "Night Elf";
                    break;
                case "5":
                    Converted = "Undead";
                    break;
                case "6":
                    Converted = "Tauren";
                    break;
                case "7":
                    Converted = "Gnome";
                    break;
                case "8":
                    Converted = "Troll";
                    break;
                case "9":
                    Converted = "Goblin";
                    break;
                case "10":
                    Converted = "Blood Elf";
                    break;
                case "11":
                    Converted = "Draenei";
                    break;
                case "22":
                    Converted = "Worgen";
                    break;
                case "26":
                    Converted = "Pandaren";
                    break;
                default:
                    Converted = "error";
                    break;
            }

            return Converted;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="In"></param>
        /// <returns></returns>
        static public String ConvertItemClass(Int32 In)
        {
            String Converted = "error";

            switch (In)
            {
                case 0:
                    Converted = "Consumable";
                    break;
                case 1:
                    Converted = "Container";
                    break;
                case 2:
                    Converted = "Weapon";
                    break;
                case 4:
                    Converted = "Armor";
                    break;
                case 5:
                    Converted = "Reagent";
                    break;
                case 6:
                    Converted = "Projectile";
                    break;
                case 7:
                    Converted = "Trade Goods";
                    break;
                case 9:
                    Converted = "Recipe";
                    break;
                case 11:
                    Converted = "Quiver";
                    break;
                case 12:
                    Converted = "Quest";
                    break;
                case 13:
                    Converted = "Key";
                    break;
                case 15:
                    Converted = "Miscellaneous";
                    break;
            }

            return Converted;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Class"></param>
        /// <param name="In"></param>
        /// <returns></returns>
        static public String ConvertItemSubClass(Int32 Class, Int32 In)
        {
            String Converted = "error";

            if (Class == 2)
            {
                // Weapon
                switch (In)
                {
                    case 0:
                        Converted = "1 Axe"; // 1 Handed
                        break;
                    case 1:
                        Converted = "2 Axe"; // 2 Handed
                        break;
                    case 2:
                        Converted = "Bow";
                        break;
                    case 3:
                        Converted = "Rifle";
                        break;
                    case 4:
                        Converted = "1 Mace"; // 1 handed
                        break;
                    case 5:
                        Converted = "2 Mace"; // 2 handed
                        break;
                    case 6:
                        Converted = "Polearm";
                        break;
                    case 7:
                        Converted = "1 Sword"; // 1 handed
                        break;
                    case 8:
                        Converted = "2 Sword"; // 2 handed
                        break;
                    case 10:
                        Converted = "Staff";
                        break;
                    case 11:
                        Converted = "1 Exotic"; // 1 handed
                        break;
                    case 12:
                        Converted = "2 Exotic"; // 2 handed
                        break;
                    case 13:
                        Converted = "Fist Weapon";
                        break;
                    case 14:
                        Converted = "Miscellaneous Weapon"; // Blacksmith Hammer, Mining Pick, etc.
                        break;
                    case 15:
                        Converted = "Dagger";
                        break;
                    case 16:
                        Converted = "Thrown";
                        break;
                    case 17:
                        Converted = "Spear";
                        break;
                    case 18:
                        Converted = "Crossbow";
                        break;
                    case 19:
                        Converted = "Wand";
                        break;
                    case 20:
                        Converted = "Fishing Pole";
                        break;
                }
            }
            else if (Class == 4)
            {
                // Armor           
                switch (In)
                {
                    case 0:
                        Converted = "Micellaneous";
                        break;
                    case 1:
                        Converted = "Cloth";
                        break;
                    case 2:
                        Converted = "Leather";
                        break;
                    case 3:
                        Converted = "Mail";
                        break;
                    case 4:
                        Converted = "Plate";
                        break;
                    case 6:
                        Converted = "Shield";
                        break;
                }
            }

            return Converted;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="In"></param>
        /// <returns></returns>
        static public String ConvertInventoryType(Int32 In)
        {
            String Converted = "error";

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
                    Converted = "wrist"; // Bracers
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
                    Converted = "one hand"; // dagger
                    Converted = "mainHand";
                    break;
                case 14:
                    Converted = "off hand";
                    Converted = "offHand";
                    break;
                case 15:
                    Converted = "bow";
                    Converted = "mainHand";
                    break;
                case 16:
                    Converted = "back";
                    break;
                case 17:
                    Converted = "two hand";
                    Converted = "mainHand";
                    break;
                case 18:
                    Converted = "bag"; // incl. quivers
                    break;
                case 19:
                    Converted = "tabard";
                    break;
                case 20:
                    Converted = "robe";
                    Converted = "chest";
                    break;
                case 21:
                    Converted = "main hand";
                    break;
                case 22:
                    Converted = "off hand"; // misc items
                    break;
                case 23:
                    Converted = "tome";
                    Converted = "offHand";
                    break;
                case 24:
                    Converted = "ammunition";
                    break;
                case 25:
                    Converted = "thrown";
                    break;
                case 26:
                    Converted = "gun";
                    Converted = "mainHand";
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

            return Converted;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        static public string ConvertStat(int i)
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
                case 49:
                    stat = "Mastery";
                    break;
                default:
                    stat = "Unknown";
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
