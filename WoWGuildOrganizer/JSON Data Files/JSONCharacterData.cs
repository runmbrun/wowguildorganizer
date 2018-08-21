using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoWGuildOrganizer
{
    class JSONCharacterTalentData
    {
        public string Selected { get; set; }
        public JSONSpecData Spec { get; set; }
    }

    class JSONCharacterProfessionData
    {
        public IList<JSONProfessionData> Primary { get; set; }
        public IList<JSONProfessionData> Secondary { get; set; }
    }

    class JSONProfessionData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Rank { get; set; }
    }

    class JSONCharacterData
    {
        public long LastModified { get; set; }
        public string Name { get; set; }
        public string Realm { get; set; }
        public string Battlegroup { get; set; }
        public int Class { get; set; }
        public int Race { get; set; }
        public int Gender { get; set; }
        public int Level { get; set; }
        public int AchievementPoints { get; set; }
        public string Thumbnail { get; set; }
        public string CalcClass { get; set; }
        public int Faction { get; set; }
        public JSONCharacterItems Items { get; set; }        
        public JSONCharacterProfessionData Professions { get; set; }
        public IList<JSONCharacterTalentData> Talents { get; set; }
        public string TotalHonorableKills { get; set; }
    }
}
