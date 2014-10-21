﻿using System;
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
        //     c. not gemmed
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
        }
    }
}
