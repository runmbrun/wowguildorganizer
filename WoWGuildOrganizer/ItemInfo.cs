using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WoWGuildOrganizer
{
    [Serializable]
    class ItemInfo
    {
        // Need the following:
        //  1.  Id
        //  2.  CanEnchant
        //  3.  CanSocket
        //  4.  SocketCount

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

    }
}
