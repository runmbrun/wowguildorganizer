using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace WoWGuildOrganizer
{
    [Serializable]
    class GuildMemberGroup
    {
        public ArrayList SavedCharacters = null;


        private String _guild;
        public String Guild
        {
            get { return _guild; }
            set { _guild = value; }
        }

        private String _realm;
        public String Realm
        {
            get { return _realm; }
            set { _realm = value; }
        }
        
        
        public GuildMemberGroup()
        {
            SavedCharacters = new ArrayList();
            _guild = "";
            _realm = "";
        }
    }
}
