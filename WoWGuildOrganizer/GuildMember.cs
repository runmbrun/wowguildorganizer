using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace WoWGuildOrganizer
{
    [Serializable]
    class GuildMember
    {
        private String _name;
        public String Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private String _level;
        public String Level
        {
            get { return _level; }
            set 
            { 
                _level = value; 
                _levelchanged = true;
                SetArmoryCheckTime();
            }
        }

        private String _class;
        public String Class
        {
            get { return _class; }
            set { _class = value; }
        }

        private String _race;
        public String Race
        {
            get { return _race; }
            set { _race = value; }
        }

        private Int32 _achievementpoints;
        public Int32 AchievementPoints
        {
            get { return _achievementpoints; }
            set { _achievementpoints = value; SetArmoryCheckTime(); }
        }
        
        private Int32 _maxilevel;
        public Int32 MaxiLevel
        {
            get { return _maxilevel; }
            set
            {
                if (_maxilevel != value)
                {
                    _ilevelchanged = true;
                    _maxilevel = value;
                }
                else
                {
                    _ilevelchanged = false;
                }
                SetItemLevelCheckTime();
            }
        }

        private Int32 _equipedilevel;
        public Int32 EquipediLevel
        {
            get { return _equipedilevel; }
            set
            {
                if (_equipedilevel != value)
                {
                    _ilevelchanged = true;
                    _equipedilevel = value;
                }
                else
                {
                    _ilevelchanged = false;
                }
                SetItemLevelCheckTime();
            }
        }

        private DateTime _lastupdated;
        public DateTime LastUpdated
        {
            get { return _lastupdated; }
            set { _lastupdated = value; }
        }        


        // These are variables but are hidden from the grid.
        private DateTime _lastarmorycheck;        
        private DateTime _lastilevelcheck;
        
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


        private Boolean _levelchanged;
        private Boolean _namechanged;
        private Boolean _ilevelchanged;

        public GuildMember()
        {
            _levelchanged = false;
            _namechanged = false;
            _ilevelchanged = false;
        }

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
            if (_ilevelchanged)
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

        public void ClearItemLevelFlag()
        {
            _ilevelchanged = false;
        }
        
    }
}
