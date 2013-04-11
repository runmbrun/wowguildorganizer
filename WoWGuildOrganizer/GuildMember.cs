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

        private Int32 _level;
        public Int32 Level
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

        private Int32 _equipedilevel;
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
        private Boolean _ilevelchangedEquip;
        private Boolean _ilevelchangedMax;

        private String _profession1;
        public void SetProfession1 (String prof)
        {
            _profession1 = prof;
        }
        public String GetProfession1()
        {
            return _profession1;
        }
        private String _profession2;
        public void SetProfession2(String prof)
        {
            _profession2 = prof;
        }
        public String GetProfession2()
        {
            return _profession2;
        }


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
    }
}
