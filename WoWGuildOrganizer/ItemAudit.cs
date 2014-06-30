using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace WoWGuildOrganizer
{
    [Serializable]
    public class ItemAudit
    {
        //  The Audit Table will show the following things:
        //  1. One line for each character slot
        //  2. Each line will contain a deduction if the item is ...
        //     a. item missing
        //     b. not enchanted
        //     c. not gemmed - this could be very tricky... might need a 
        //     d. item level

        #region " Properties "

        private String _slot;
        public String Slot
        {
            get { return _slot; }
            set { _slot = value; }
        }

        private String _name;
        public String Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private Int32 _itemLevel;
        public Int32 ItemLevel
        {
            get { return _itemLevel; }
            set { _itemLevel = value; }
        }

        private String _missingItem;
        public String MissingItem
        {
            get { return _missingItem; }
            set { _missingItem = value; }
        }

        private String _missingEnchant;
        public String MissingEnchant
        {
            get { return _missingEnchant; }
            set { _missingEnchant = value; }
        }

        private String _missingGem;
        public String MissingGem
        {
            get { return _missingGem; }
            set { _missingGem = value; }
        }

        private String _professions;
        public String Profession
        {
            get { return _professions; }
            set { _professions = value; }
        }

        #endregion

        #region " Constructor "

        public ItemAudit()
        {
            MissingItem = "0";
            MissingEnchant = "0";
            MissingGem = "0";
            _quality = 0;
        }

        #endregion
        
        private Int32 _id;

        [Browsable(false)]
        public Int32 Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private Int32 _quality;

        [Browsable(false)]
        public Int32 Quality
        {
            get { return _quality; }
            set { _quality = value; }
        }

        private Boolean _canenchant;
        public void CanEnchant(Boolean e)
        {
            _canenchant = e;
        }
        public Boolean CanEnchant()
        {
            return _canenchant;
        }

        private Boolean _cansocket;
        public void CanSocket(Boolean s)
        {
            _cansocket = s;
        }
        public Boolean CanSocket()
        {
            return _cansocket;
        }

        private Int32 _socketcount;
        public void SocketCount(Int32 s)
        {
            _socketcount = s;
        }

        public Boolean IsEnchanted()
        {
            if (_toolTips.Contains(@"enchant"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Boolean IsInscriptionEnchant()
        {
            // There are 4 inscription enchants.
            //   Current Inscription Enchants for MoP:
            if (
                _toolTips.Contains(@"4912") || 
                _toolTips.Contains(@"4913") || 
                _toolTips.Contains(@"4914") || 
                _toolTips.Contains(@"4915")
                )
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Boolean IsLeatherworkingEnchant()
        {
            // There are 4 current Leatherworking Enchants for the wrists.
            //   for MoP they are:
            if (
                _toolTips.Contains(@"4875") ||
                _toolTips.Contains(@"4877") ||
                _toolTips.Contains(@"4878") ||
                _toolTips.Contains(@"4879")
                )
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Boolean IsTailorEnchant()
        {
            // Tailoring currenlty has 3 enchants for the back
            //   Here are the 3 enchants for MoP:
            if (                
                _toolTips.Contains("4892") ||
                _toolTips.Contains("4893") ||
                _toolTips.Contains("4894")
                )
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Boolean IsBlacksmithingSocket()
        {
            if (_toolTips.Contains(@"extraSocket"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int IsJewelcraftingGem()
        {
            int Found = 0;


            //  This will change for every expansion!
            // There are 12 JC Gems for MoP:            
            if (_toolTips != null)
            {
                // If the tooltips contains one of them... return true
                if (_toolTips.Contains("83141"))
                {
                    Found = Regex.Matches(_toolTips, "83141").Count;
                }
                if (_toolTips.Contains("83142"))
                {
                    Found = Regex.Matches(_toolTips, "83142").Count;
                }
                if (_toolTips.Contains("83143"))
                {
                    Found = Regex.Matches(_toolTips, "83143").Count;
                }
                if (_toolTips.Contains("83144"))
                {
                    Found = Regex.Matches(_toolTips, "83144").Count;
                }
                if (_toolTips.Contains("83145"))
                {
                    Found = Regex.Matches(_toolTips, "83145").Count;
                }
                if (_toolTips.Contains("83146"))
                {
                    Found = Regex.Matches(_toolTips, "83146").Count;
                }
                if (_toolTips.Contains("83147"))
                {
                    Found = Regex.Matches(_toolTips, "83147").Count;
                }
                if (_toolTips.Contains("83148"))
                {
                    Found = Regex.Matches(_toolTips, "83148").Count;
                }
                if (_toolTips.Contains("83149"))
                {
                    Found = Regex.Matches(_toolTips, "83149").Count;
                }
                                
                if (_toolTips.Contains("83150"))
                {
                    Found = Regex.Matches(_toolTips, "83150").Count;
                }

                if (_toolTips.Contains("83151"))
                {
                    Found = Regex.Matches(_toolTips, "83151").Count;
                }
                if (_toolTips.Contains("83152"))
                {
                    Found = Regex.Matches(_toolTips, "83152").Count;
                }
            }

            return Found;
        }

        public Boolean IsEngineeringCog()
        {
            // TODO - mmb.  Not sure how to do this...
            //   I think special engineering helms can have cog wheel sockets
            //   And only engineers can use them
            // Almost have to check helm first to see if engineering helm,
            //   and then check for cog wheels...
            return true;

            /*
            if (_toolTips.Contains("1"))
            {
                return true;
            }
            else
            {
                return false;
            }*/
        }  

        private String _toolTips;
        public void SetToolTips(String t)
        {
            _toolTips = t;
            Int32 GemCount = 0;
            Boolean ExtraSocket = false;


            // now parse this out and fix the unknown properties...
            //  1. MissingItem
            //  2. MissingEnchant
            //  3. MissingGem
            //  4. LastUpdated


            if (Slot == "waist")
            {
                // Check to see if Extra Socket if filled or not...
                if (_toolTips.Contains(@"extraSocket"":true"))
                {
                    ExtraSocket = true;
                }
                else
                {
                    MissingEnchant = "1";
                }
            }

            // Find the number of Gems in the item
            Int32 start = 0;
            if (_toolTips.Length > 0)
            {
                while ((start = _toolTips.IndexOf("gem", start + 1)) != -1)
                {
                    GemCount++;
                }
            }


            // 2. MissingEnchant
            //   Enchants Example:  "enchant":4209
            if (CanEnchant())
            {   
                if (!_toolTips.Contains("enchant"))
                {
                    MissingEnchant = "1";
                }
            }

            // 3. MissingGem
            //   Gems - Example: "gem0":52209,
            if (CanSocket())
            {
                if (ExtraSocket)
                {
                    if (GemCount < (_socketcount + 1))
                    {
                        MissingGem = ((_socketcount + 1) - GemCount).ToString();
                    }
                }
                else
                {
                    if (GemCount < _socketcount)
                    {
                        MissingGem = (_socketcount - GemCount).ToString();
                    }
                }
            }
            else
            {
                if (Slot == "waist")
                {
                    // no other sockets exist in the waist, make sure there is 
                    //  at least 1 gem for it though
                    if (GemCount != 1)
                    {
                        MissingGem = "1";
                    }
                }
            }

            // *** Profession Cases: ***
            //TODO - Profession audits ->             
            //  1. 2 x Ring enchants -> if Enchanter
            //  2. 1 x Bracer special enchant -> if Leatherworker
            //  3. 3 x Special Gems -> if Jewelcrafter
            //  4. 2 x Sockets (bracer and hands) -> if Blacksmith
            //  5. 1 x cloak special enchant -> if Tailor
            /*
             * skillLines
                "185","Cooking"
                "773","Inscription"
                "755","Jewelcrafting"
                "393","Skinning"
                "333","Enchanting"
                "202","Engineering"
                "197","Tailoring"
                "186","Mining"
                "182","Herbalism"
                "171","Alchemy"
                "165","Leatherworking"
                "164","Blacksmithing"
             * */
        }
    }
}
