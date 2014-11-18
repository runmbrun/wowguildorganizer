using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace WoWGuildOrganizer
{
    [Serializable]
    public class ItemInfo
    {
        // Need the following:
        //  1.  Id
        //  2.  CanEnchant
        //  3.  CanSocket
        //  4.  SocketCount
        //  5.  InventoryType
        //  6.  Quality
        //  7.  ItemLevel

        private int _id;
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private bool _canenchant;
        public bool CanEnchant
        {
            get { return _canenchant; }
            set { _canenchant = value; }
        }

        private bool _cansocket;
        public bool CanSocket
        {
            get { return _cansocket; }
            set { _cansocket = value; }
        }

        private int _socketcount;
        public int SocketCount
        {
            get { return _socketcount; }
            set { _socketcount = value; }
        }

        private int _inventorytype;
        public int InventoryType
        {
            get { return _inventorytype; }
            set { _inventorytype = value; }
        }

        private int _quality;
        public int Quality
        {
            get { return _quality; }
            set { _quality = value; }
        }

        private int _itemlevel;
        public int ItemLevel
        {
            get { return _itemlevel; }
            set { _itemlevel = value; }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private Dictionary<int, int> _stats = new Dictionary<int, int>();
        public string Stats
        {            
            set 
            { 
                //_stats = value; 
                // Example:
                //{"stat":4,"amount":485},{"stat":13,"amount":638},{"stat":7,"amount":958},{"stat":37,"amount":323}

                string Search = @"stat:(?<stat>\d+?),amount:(?<amount>\d+?)}";
                Regex test = new Regex(Search, RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.CultureInvariant);

                // Get the item info
                foreach (Match result in test.Matches(value))
                {
                    // First get the slot, if no slot then it's an missing item
                    if (result.Groups["stat"].Success)
                    {
                        Int32 stat = Convert.ToInt32(result.Groups["stat"].Value);
                        Int32 amount = Convert.ToInt32(result.Groups["amount"].Value);

                        _stats.Add(stat, amount);
                    }
                }
            }
        }
        
        private int _itemclass;
        public int ItemClass
        {
            get { return _itemclass; }
            set { _itemclass = value; }
        }

        private int _itemsubclass;
        public int ItemSubClass
        {
            get { return _itemsubclass; }
            set { _itemsubclass = value; }
        }

        private int _armor;
        public int Armor
        {
            get { return _armor; }
            set { _armor = value; }
        }

        private string _tooltip;
        public string Tooltip
        {
            get 
            {
                if (_tooltip == null || _tooltip == string.Empty)
                {
                    _tooltip = CreateTooltip();
                }

                return _tooltip; 
            }

            set 
            { 
                _tooltip = value; 
            }
        }

        public bool HasIntellect()
        {
            if (_stats.ContainsKey(5) || _stats.ContainsKey(74))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool HasStrength()
        {
            if (_stats.ContainsKey(4) || _stats.ContainsKey(74))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool HasAgility()
        {
            if (_stats.ContainsKey(3))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool HasSpirit()
        {
            if (_stats.ContainsKey(6))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool HasHit()
        {
            // todo: remove this
            if (_stats.ContainsKey(31))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool HasTankStats()
        {
            // todo: remove this
            // Does it contain dodge or parry?
            if (_stats.ContainsKey(13) || _stats.ContainsKey(14))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private string GetStamina()
        {
            string sta = string.Empty;

            if (_stats.ContainsKey(7))
            {
                sta = _stats[7].ToString();
            }

            return sta;
        }

        private string GetStrength()
        {
            string sta = string.Empty;

            if (HasStrength())
            {
                if (_stats.ContainsKey(4))
                {
                    sta = _stats[4].ToString();
                }
                else if (_stats.ContainsKey(74))
                {
                    sta = _stats[74].ToString();
                }
            }

            return sta;
        }

        private string GetAgility()
        {
            string sta = "";

            if (HasAgility())
            {
                sta = _stats[3].ToString();
            }

            return sta;
        }

        private string GetIntellect()
        {
            string sta = "";

            if (HasIntellect())
            {
                sta = _stats[5].ToString();
            }

            return sta;
        }

        public string CreateTooltip()
        {
            string tooltip = string.Empty;

            // is this a weapon or armor?
            if (Converter.ConvertItemClass(this.ItemClass) == "Armor")
            {
                // Armor:

                // Name of Item (Rarity level)
                // Item Level 123 (yellow)
                // Upgrade Level (yellow)
                // Transmog (pink)
                // Soulbound (white)
                // Slot            Armor Type(white)
                // 1234 Armor (white)
                // +123 Strength (white)
                // +1,277 Stamina (white)
                // +123 Hit (green)
                // +123 Haste (green)
                // +123 Reforged (green)
                // 
                // Enchanted
                // Socket 1
                // Socket 2
                // Socket Bonus
                // 
                // Durability: 123/123 (white)
                // Level Required 12 (white)
                // ...


                // Name and iLevel of item
                tooltip +=
                    this.Name + "\n" +
                    this.ItemLevel + "\n";

                // Slot and Type
                tooltip += Converter.ConvertInventoryType(this.InventoryType) + "\t\t" + Converter.ConvertItemSubClass(this.ItemClass, this.ItemSubClass) + "\n";

                // Armor if applicatable
                if (Armor > 0)
                {
                    tooltip += Armor.ToString() + " Armor \n";
                }

                // Blank Line
                string line = string.Empty;

                // Add Stats here...

                // Primary Stat
                if (GetStrength() != string.Empty)
                {
                    line = "+" + GetStrength() + " Strength \n";
                }
                else if (GetAgility() != "")
                {
                    line = "+" + GetAgility() + " Agility \n";
                }
                else if (GetIntellect() != string.Empty)
                {
                    line = "+" + GetIntellect() + " Intellect \n";
                }

                // Stamina Stat
                if (GetStamina() != string.Empty)
                {
                    tooltip += line + "+" + GetStamina() + " Stamina \n";
                }

                // Blank Line in between the main stats and secondary ones
                line = string.Empty;

                Dictionary<Int32, Int32> dict = _stats.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);

                // All Secondary stats
                foreach (int stat in dict.Keys)
                {
                    if (stat != 3 && stat != 4 && stat != 5 && stat != 7 && stat != 74)
                    {
                        line += "+" + dict[stat].ToString() + " " + Converter.ConvertStat(stat) + "\n";
                    }
                }
                
                // TODO: finish this for procs

                tooltip += line;
            }
            else if (Converter.ConvertItemClass(this.ItemClass) == "Weapon")
            {
                // Name and iLevel of item
                tooltip +=
                    this.Name + "\n" +
                    this.ItemLevel + "\n" +
                    Converter.ConvertInventoryType(this.InventoryType) + "\t\t" + Converter.ConvertItemSubClass(this.ItemClass, this.ItemSubClass) + "\n";

                string line = string.Empty;

                if (GetStrength() != string.Empty)
                {
                    line = "+" + GetStrength() + " Strength \n";
                }
                else if (GetAgility() != string.Empty)
                {
                    line = "+" + GetAgility() + " Agility \n";
                }
                else if (GetIntellect() != string.Empty)
                {
                    line = "+" + GetIntellect() + " Intellect \n";
                }

                if (GetStamina() != string.Empty)
                {
                    tooltip += line + "+" + GetStamina() + " Stamina \n";
                }
                
                // Blank Line in between the main stats and secondary ones
                line = "";
                                
                Dictionary<int, int> dict = _stats.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);

                foreach (int stat in dict.Keys)
                {
                    if (stat != 3 && stat != 4 && stat != 5 && stat != 7)
                    {
                        line += "+" + dict[stat].ToString() + " " + Converter.ConvertStat(stat) + "\n";
                    }
                }

                // TODO: finish this for procs

                tooltip += line;
            }

            return tooltip;
        }
    }
}
