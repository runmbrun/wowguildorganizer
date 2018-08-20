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
        public void SetStats(IList<JSONItemStats> values)
        {            
            //set 
            {
                // Example:
                //{"stat":4,"amount":485},{"stat":13,"amount":638},{"stat":7,"amount":958},{"stat":37,"amount":323}

                /* Old Way...
                string Search = @"stat:(?<stat>\d+?),amount:(?<amount>\d+?)}";
                Regex test = new Regex(Search, RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.CultureInvariant);

                // Get the item info
                foreach (Match result in test.Matches(value))
                {
                    // First get the slot, if no slot then it's an missing item
                    if (result.Groups["stat"].Success)
                    {
                        int stat = Convert.ToInt32(result.Groups["stat"].Value);
                        int amount = Convert.ToInt32(result.Groups["amount"].Value);

                        _stats.Add(stat, amount);
                    }
                }*/

                foreach (JSONItemStats statItem in values)
                {
                    int stat = Convert.ToInt32(statItem.Stat);
                    int amount = Convert.ToInt32(statItem.Amount);

                    _stats.Add(stat, amount);
                }
            }
        }

        private int _itemspells;
        private string[] _spells;
        public string Spells
        {
            set
            {
                // Example:
                //itemSpells":[{"spellId":146195,"spell":{"id":146195,"name":"Flurry of Xuen","icon":"monk_stance_whitetiger","description":"Your damaging attacks have a chance to trigger a Flurry of Xuen, causing you to deal 1,204 damage to your current target and up to 4 enemies around them, every 0.30 sec for 3 sec. (Approximately 1.82 procs per minute)","castTime":"Passive"},"nCharges":0,"consumable":false,"categoryId":0,"trigger":"ON_EQUIP"}]

                string Search = @"description:(?<spell>.*),castTime.*?,trigger:(?<trigger>.*)}";
                Regex test = new Regex(Search, RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.CultureInvariant);
                MatchCollection matches = test.Matches(value.Replace("\"", ""));
                _itemspells = matches.Count;
                int count = 0;

                if (_itemspells > 0)
                {
                    _spells = new string[_itemspells];
                }

                // Get the item's spell info
                foreach (Match result in matches)
                {
                    string line = string.Empty;

                    if (result.Groups["trigger"].Success)
                    {
                        if (result.Groups["trigger"].Value.ToString() == "ON_EQUIP")
                        {
                            line += "Equip: ";
                        }
                    }

                    // First get the slot, if no slot then it's an missing item
                    if (result.Groups["spell"].Success)
                    {
                        string spell = result.Groups["spell"].Value.ToString();
                        int start = 0;
                        int end = 0;
                        int countLines = 1;

                        // go through the entire spell, but making sure each line is no more than 50 characters
                        // This is to prevent the tooltip from getting too long
                        for (int i = 0; i < spell.Length; )
                        {
                            // Find the first space
                            start = spell.IndexOf(" ", end + 1);

                            if (start > (50 * countLines))
                            {
                                line += spell.Substring(i, end - i) + "\n";
                                i = end;
                                countLines++;
                            }
                            else if (start == -1)
                            {
                                // End of spell, no more spaces
                                line += spell.Substring(i, spell.Length - i) + "\n";
                                i = spell.Length;
                            }
                            else
                            {
                                end = start;
                            }
                        }
                    }

                    _spells[count++] = line;
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool HasIntellect()
        {
            // 5 = int
            // 74 = int or str
            if (_stats.ContainsKey(5) || _stats.ContainsKey(74))
            {
                return true;
            }
            else if (_stats.Count == 0 && _tooltip != null && _tooltip.Contains("Intellect"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool HasStrength()
        {
            // 4 = str
            // 72 = str
            // 74 = int or str
            if (_stats.ContainsKey(4) || _stats.ContainsKey(72) || _stats.ContainsKey(74))
            {
                return true;
            }
            else if (_stats.Count == 0 && _tooltip != null && _tooltip.Contains("Strength"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool HasAgility()
        {
            if (_stats.ContainsKey(3))
            {
                return true;
            }
            else if (_stats.Count == 0 && _tooltip != null && _tooltip.Contains("Agility"))
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
            else if (_stats.Count == 0 && _tooltip != null && _tooltip.Contains("Spirit"))
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
            // Does it contain bonus armor?
            if (_stats.ContainsKey(50))
            {
                return true;
            }
            else if (_stats.Count == 0 && _tooltip != null && _tooltip.Contains("Bonus Armor"))
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
                else if (_stats.ContainsKey(72))
                {
                    sta = _stats[72].ToString();
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
            string sta = string.Empty;

            if (HasAgility())
            {
                sta = _stats[3].ToString();
            }

            return sta;
        }

        private string GetIntellect()
        {
            string sta = string.Empty;

            if (HasIntellect())
            {
                if (_stats.ContainsKey(5))
                {
                    sta = _stats[5].ToString();
                }
                else if (_stats.ContainsKey(74))
                {
                    sta = _stats[74].ToString();
                }
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
                
                // check for procs
                if (_itemspells > 0)
                {
                    // There is at least 1 proc
                    for (int i=0; i<_itemspells; i++)
                    {
                        line += _spells[i];
                    }
                }

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

                // check for procs
                if (_itemspells > 0)
                {
                    // There is at least 1 proc
                    for (int i=0; i<_itemspells; i++)
                    {
                        line += _spells[i];
                    }
                }

                tooltip += line;
            }

            return tooltip;
        }
    }
}
