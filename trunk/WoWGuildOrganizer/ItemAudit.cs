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
        //     c. not gemmed
        //     d. item level

        #region " Properties "

        private string _slot;
        public string Slot
        {
            get { return _slot; }
            set { _slot = value; }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private int _itemLevel;
        public int ItemLevel
        {
            get { return _itemLevel; }
            set { _itemLevel = value; }
        }

        private string _missingItem;
        public string MissingItem
        {
            get { return _missingItem; }
            set { _missingItem = value; }
        }

        private string _missingEnchant;
        public string MissingEnchant
        {
            get { return _missingEnchant; }
            set { _missingEnchant = value; }
        }

        private string _missingGem;
        public string MissingGem
        {
            get { return _missingGem; }
            set { _missingGem = value; }
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

        private int _id;

        [Browsable(false)]
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private int _quality;

        [Browsable(false)]
        public int Quality
        {
            get { return _quality; }
            set { _quality = value; }
        }

        private bool _canenchant;
        public void CanEnchant(bool e)
        {
            _canenchant = e;
        }
        public bool CanEnchant()
        {
            return _canenchant;
        }

        private bool _cansocket;
        public void CanSocket(bool s)
        {
            _cansocket = s;
        }
        public bool CanSocket()
        {
            return _cansocket;
        }

        private int _socketcount;
        public void SocketCount(int s)
        {
            _socketcount = s;
        }

        private string context;
        public string Context
        {
            get { return this.context; }
            set { this.context = value; }
        }

        public bool IsEnchanted()
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

        private string _toolTips;
        public void SetToolTips(string t)
        {
            _toolTips = t;
            int GemCount = 0;
            bool ExtraSocket = false;

            // now parse this out and fix the unknown properties...
            //  1. MissingItem
            //  2. MissingEnchant
            //  3. MissingGem
            //  4. LastUpdated

            // Find the number of Gems in the item
            int start = -1;

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
        }

        private string _bonuslists;
        public void SetBonusLists(string t)
        {
            _bonuslists = t;
        }
    }
}
