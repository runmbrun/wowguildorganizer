using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WoWGuildOrganizer
{
    static class Converter
    {
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
                case "2":
                    Converted = "Orc";
                    break;
                case "5":
                    Converted = "Undead";
                    break;
                case "6":
                    Converted = "Tauren";
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
                default:
                    Converted = "error";
                    break;
            }

            return Converted;
        }
    }
}
