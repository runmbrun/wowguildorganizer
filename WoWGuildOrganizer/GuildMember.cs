using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.ComponentModel;

namespace WoWGuildOrganizer
{
    [Serializable]
    public class GuildMember
    {
        #region " Class Variables "

        // These are the variables that will show up in the grid
        //  as they are the class Properties
        private String _name;
        private Int32 _level;
        private String _class;
        private String _race;
        private Int32 _achievementpoints;
        private Int32 _maxilevel;
        private Int32 _equipedilevel;
        private DateTime _lastupdated;
                
        // These are variables but are hidden from the grid.
        private DateTime _lastarmorycheck;
        private DateTime _lastilevelcheck;
        private string _role;
        private string _spec;
        private Boolean _levelchanged;
        private Boolean _namechanged;
        private Boolean _ilevelchangedEquip;
        private Boolean _ilevelchangedMax;
        private String _profession1;
        private String _profession2;
        private Dictionary<string, ItemAudit> _items;
        private String _realm;

        #endregion

        #region " Class Properties "

        public String Name
        {
            get { return _name; }
            set { _name = value; }
        }
        
        public Int32 Level
        {
            get 
            { 
                return _level; 
            }

            set 
            { 
                _level = value; 
                _levelchanged = true;
                SetArmoryCheckTime();
            }
        }
        
        public String Class
        {
            get { return _class; }
            set { _class = value; }
        }

        public String Race
        {
            get { return _race; }
            set { _race = value; }
        }
        
        public Int32 AchievementPoints
        {
            get { return _achievementpoints; }
            set { _achievementpoints = value; SetArmoryCheckTime(); }
        }        
        
        public Int32 MaxiLevel
        {
            get { return _maxilevel; }
            set
            {
                if (_maxilevel != value)
                {
                    _ilevelchangedMax = true;
                    _maxilevel = value;
                }
                else
                {
                    _ilevelchangedMax = false;
                }
                SetItemLevelCheckTime();
            }
        }

        public Int32 EquipediLevel
        {
            get { return _equipedilevel; }
            set
            {
                if (_equipedilevel != value)
                {
                    _ilevelchangedEquip = true;
                    _equipedilevel = value;
                }
                else
                {
                    _ilevelchangedEquip = false;
                }
                SetItemLevelCheckTime();
            }
        }

        public DateTime LastUpdated
        {
            get { return _lastupdated; }
            set { _lastupdated = value; }
        }

        [Browsable(false)]
        public string Profession1
        {
            get { return _profession1; }
            set { _profession1 = value; }
        }

        [Browsable(false)]
        public string Profession2
        {
            get { return _profession2; }
            set { _profession2 = value; }
        }

        [Browsable(false)]
        public string Spec
        {
            get { return _spec; }
            set { _spec = value; }
        }

        [Browsable(false)]
        public string Role
        {
            get { return _role; }
            set { _role = value; }
        }

        [Browsable(false)]
        public Dictionary<string, ItemAudit> ItemAudits
        {
            get { return _items; }
            set { _items = value; }
        }

        [Browsable(false)]
        public string Realm
        {
            get { return _realm; }
            set { _realm = value; }
        }

        #endregion
        
        #region " Class Constructor "

        /// <summary>
        /// Here is the constructor
        /// </summary>
        public GuildMember()
        {
            _maxilevel = 0;
            _equipedilevel = 0;
            _levelchanged = false;
            _namechanged = false;
            _ilevelchangedEquip = false;
            _ilevelchangedMax = false;
            _items = new Dictionary<string, ItemAudit>();
        }

        #endregion

        public Boolean IsLevelChanged()
        {
            if (_levelchanged)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Boolean IsItemLevelChanged()
        {
            if (_ilevelchangedEquip || _ilevelchangedMax)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Boolean IsNameChanged()
        {
            if (_namechanged)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void ClearFlags()
        {
            _levelchanged = false;
        }

        public void ClearEquipItemLevelFlag()
        {
            _ilevelchangedEquip = false;
        }

        public void ClearMaxItemLevelFlag()
        {
            _ilevelchangedMax = false;
        }

        public void SetArmoryCheckTime()
        {
            _lastarmorycheck = DateTime.Now;
            _lastupdated = _lastarmorycheck;
        }

        public void SetItemLevelCheckTime()
        {
            _lastilevelcheck = DateTime.Now;
            _lastupdated = _lastilevelcheck;
        }
    }
}
