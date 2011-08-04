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

        private String _id;
        public String Id
        {
            get { return _id; }
            set {_id = value; }
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


        public ItemAudit()
        {
            MissingItem = "0";
            MissingEnchant = "0";
            MissingGem = "0";
            _quality = "0";
        }

        private String _quality;
        public void SetQuality(String q)
        {   
            //TODO - set color of line
            _quality = q;
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
            if (Name.Length == 0)
            {
                // Only Missing if not one of these two slots
                if (Slot != "shirt" || Slot != "tabard")
                {
                    // Missing Item!
                    MissingItem = "1";
                }
            }

            // 2. MissingEnchant
            //   Enchants Example:  "enchant":4209
            // Ignore the slots that can never have enchants/gems
            if (Slot != "shirt" && Slot != "tabard" && Slot != "neck" && Slot != "waist" && Slot != "ranged" && Slot != "offHand" && 
                !Slot.StartsWith("trinket") && !Slot.StartsWith("finger"))
            {
                if (!_toolTips.Contains("enchant"))
                {
                    MissingEnchant = "1";
                }
            }
            else if (Slot != "offHand")
            {
                //TODO - offHand can have an enchant if it's a shield, but can't if it's not
            }

            // 3. MissingGem
            //   Gems - Example: "gem0":52209,
            // TODO - see if the item has sockets..
            if (Slot == "waist")
            {
                if (!_toolTips.Contains("extraSocket"))
                {
                    MissingGem = "1";
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
