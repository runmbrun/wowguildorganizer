using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoWGuildOrganizer
{
    class JSONItemData
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public IList<JSONItemStats> BonusStats { get; set; }
        //public string ItemSpells { get; set; }
        public int ItemClass { get; set; }
        public int ItemSubClass { get; set; }
        public int ContainerSlots { get; set; }
        //public string WeaponInfo { get; set; }
        public int InventoryType { get; set; }
        public bool Equipable { get; set; }
        public int ItemLevel { get; set; }
        public int Quality { get; set; }
        public int RequiredSkill { get; set; }
        public int RequiredLevel { get; set; }
        public int RequiredSkillRank { get; set; }
        //public int ItemSource { get; set; }
        public int BaseArmor { get; set; }
        public bool HasSockets { get; set; }
        public bool IsAuctionable { get; set; }
        public int Armor { get; set; }
        public int DisplayInfoId { get; set; }
        public string NameDescription { get; set; }
        public bool Upgradable { get; set; }
        public bool HeroicTooltip { get; set; }
        public string Context { get; set; }
        public IList<int> BonusList { get; set; }
        public IList<string> AvailableContexts { get; set; }
        //public string BonusSummary { get; set; }
    }
}
