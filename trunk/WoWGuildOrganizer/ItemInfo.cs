using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        private Int32 _id;
        public Int32 Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private Boolean _canenchant;
        public Boolean CanEnchant
        {
            get { return _canenchant; }
            set { _canenchant = value; }
        }

        private Boolean _cansocket;
        public Boolean CanSocket
        {
            get { return _cansocket; }
            set { _cansocket = value; }
        }

        private Int32 _socketcount;
        public Int32 SocketCount
        {
            get { return _socketcount; }
            set { _socketcount = value; }
        }

        private Int32 _inventorytype;
        public Int32 InventoryType
        {
            get { return _inventorytype; }
            set { _inventorytype = value; }
        }

        private Int32 _quality;
        public Int32 Quality
        {
            get { return _quality; }
            set { _quality = value; }
        }

        private Int32 _itemlevel;
        public Int32 ItemLevel
        {
            get { return _itemlevel; }
            set { _itemlevel = value; }
        }
    }
}
