using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WoWGuildOrganizer
{
    class ItemAudit
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

        private DateTime _lastupdated;
        public DateTime LastUpdated
        {
            get { return _lastupdated; }
            set { _lastupdated = value; }
        }
        #endregion

        #region " Constructor "
        public ItemAudit()
        {
            MissingItem = "0";
            MissingEnchant = "0";
            MissingGem = "0";
            _quality = "0";
        }
        #endregion


        private Int32 _id;
        public Int32 GetId()
        {
            return _id; 
        }
        public void SetId(Int32 i)
        { 
            _id = i;
        }

        private String _quality;
        public void SetQuality(String q)
        {   
            _quality = q;
        }
        public Int32 GetQuality()
        {
            return Convert.ToInt32(_quality);
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
        
        private String _toolTips;
        public void SetToolTips(String t)
        {
            _toolTips = t;

            // now parse this out and fix the unknown properties...
            //  1. MissingItem
            //  2. MissingEnchant
            //  3. MissingGem
            //  4. LastUpdated


            


            // 1. MissingItem
            /*  TODO - this currently can't be done.
             * This is checked for in the FormItemAudit.PassData() function.
            if (Name.Length == 0)
            {
                // Only Missing if not one of these two slots
                if (Slot != "shirt" || Slot != "tabard")
                {
                    // Missing Item!
                    MissingItem = "1";
                }
            }
            */

            // 2. MissingEnchant
            //   Enchants Example:  "enchant":4209
            if (CanEnchant())
            {
                if (Slot == "waist")
                {
                    if (!_toolTips.Contains("extraSocket"))
                    {
                        MissingEnchant = "1";
                    }
                }
                else if (!_toolTips.Contains("enchant"))
                {
                    MissingEnchant = "1";
                }
            }

            // 3. MissingGem
            //   Gems - Example: "gem0":52209,
            // TODO - see if the item has sockets..
            if (CanSocket())
            {
                Int32 start = 0;
                Int32 count = 0;


                if (_toolTips.Length > 0)
                {
                    while ((start = _toolTips.IndexOf("gem", start + 1)) != -1)
                    {
                        count++;
                    }
                }

                if (count < _socketcount)
                {
                    MissingGem = (_socketcount - count).ToString();
                }
            }
                
            //TODO - Profession audits -> 
            //  1. 2 x Ring enchants -> if Enchanter
            //  2. 1 x Bracer special enchant -> if Leatherworker
            //  3. 3 x Special Gems -> if Jewelcrafter
            //  4. 2 x Sockets (bracer and hands) -> if Blacksmith
            //  5. 1 x cloak special enchant -> if Tailor

        }
    }
}
