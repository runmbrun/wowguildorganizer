using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WoWGuildOrganizer
{
    [Serializable]
    public class WoWItem
    {
        private Int32 _itemlevel = 0;
        public Int32 ItemLevel
        {
            set { _itemlevel = value; }
            get { return _itemlevel; }
        }

        private String _itemname = "Not Found";
        public String ItemName
        {
            set { _itemname = value; }
            get { return _itemname; }
        }

        private Int32 _itemquality = 0;
        public Int32 ItemQuality
        {
            set { _itemquality = value; }
            get { return _itemquality; }
        }

        private String _itemslot = "-1";
        public String ItemSlot
        {
            set { _itemslot = value; }
            get { return _itemslot; }
        }

        private String _itemnumber = "-1";
        public String ItemNumber
        {
            set { _itemnumber = value; }
            get { return _itemnumber; }
        }

        private Boolean _twohanded = false;
        public Boolean TwoHanded
        {
            set { _twohanded = value; }
            get { return _twohanded; }
        }
    }
}
