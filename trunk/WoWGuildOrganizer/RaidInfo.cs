// -----------------------------------------------------------------------
// <copyright file="RaidInfo.cs" company="Vangent, Inc.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace WoWGuildOrganizer
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Windows.Forms;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Linq;

    /// <summary>
    /// Contains all the raid loot and the drop results from each of the tiers:
    /// <c>Tier 14 - "Mogu'shan Vaults"
    /// Tier 15 - ?
    /// Tier 16 - "Siege of Orgrimmar"
    /// Tier 17 - </c>
    /// </summary>
    public class RaidInfo
    {
        /// <summary>
        /// This will load the raids, bosses, and loot tables from an xml file on the local storage
        /// </summary>
        /// <param name="raidLoot"></param>
        public void LoadRaidLootData(ref Dictionary<string, Dictionary<string, int[]>> raidLoot)
        {
            Dictionary<string, int[]> tempLoot = new Dictionary<string, int[]>();
            string raidName = string.Empty;
            string raidDifficulty = string.Empty;
            string raidBoss = string.Empty;
            int[] bossLoot = null;
            string fileName = "raidloot.xml";

            // Look for a local file
            if (File.Exists(fileName))
            {
                XDocument doc = XDocument.Load(fileName);                
                
                foreach (XElement raids in doc.Root.Elements())
                {
                    foreach (XAttribute raid in raids.Attributes())
                    {
                        if (raid.Name.ToString().ToLower() == "name")
                        {
                            // Get this raid name
                            raidName = raid.Value.ToString();
                        }
                        else if (raid.Name.ToString().ToLower() == "difficulty")
                        {
                            // Get this raid difficulty
                            raidDifficulty = raid.Value.ToString();

                            // clear out temp loot 
                            tempLoot = new Dictionary<string, int[]>();

                            // now cycle through all the different bosses loot tables
                            foreach (XElement el in raids.Elements())
                            {
                                ArrayList loot = new ArrayList();
                                int count = 0;
                                                                
                                foreach (XElement element in el.Elements())
                                {
                                    if (element.Name == "name")
                                    {
                                        raidBoss = element.Value.ToString();
                                    }
                                    else if (element.Name == "loot")
                                    {
                                        loot.Add(element.Value.ToString());
                                        count++;
                                    }
                                }

                                bossLoot = new int[count];
                                for (int i = 0; i < count; i++)
                                {
                                    bossLoot[i] = Convert.ToInt32(loot[i]);
                                }

                                tempLoot.Add(raidBoss, bossLoot);
                            }

                            raidLoot.Add(raidName + " - " + raidDifficulty, tempLoot);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("You currently don't have a raid loot table to work with.\nPlease go to the settings table to pull down data.");
            }
        }      

        #region " Raid Loot Results "

        /// <summary>
        /// Checks what raid and boss is selected, and then creates a loot table accordingly
        /// </summary>
        /// <param name="itemIds">array of integers that contains all the possible item ids that could drop as loot</param>
        /// <param name="raidGroup">array list of all the raid members</param>
        /// <returns>data table of the loot drop results</returns>
        public DataTable GetLootResults(int[] itemIds, string context, ArrayList raidGroup)
        {
            // Create data table for all the new data
            DataTable loot = new DataTable();

            // Add Columns
            loot.Columns.Add("Upgrade");
            loot.Columns[0].DataType = typeof(int);
            loot.Columns.Add("CharacterName");
            loot.Columns.Add("ItemId");
            loot.Columns.Add("ItemName");
            loot.Columns.Add("ItemSlot");
            loot.Columns.Add("ItemILevel");
            loot.Columns.Add("OldItemILevel");
            loot.Columns.Add("OldItemId");
            loot.Columns.Add("OldItemContext");

            // Now check to make sure it is valid and contains at least 1 item
            if (itemIds != null && itemIds.Length > 0)
            {
                // Go through all item ids
                foreach (int itemId in itemIds)
                {
                    ItemInfo item = null;

                    item = FormMain.Items.GetItem(itemId, context);

                    if (item != null)
                    {
                        // Now we have the item in the item cache

                        // Check against each member in the Raid Group
                        foreach (GuildMember gm in raidGroup)
                        {
                            /*   Here are the different Classes:
                             * Death Knight
                             * Druid
                             * Hunter
                             * Mage
                             * Monk
                             * Paladin
                             * Priest                                
                             * Rogue
                             * Shaman
                             * Warlock
                             * Warrior 
                             */
                            string charName = string.Empty;
                            int oldItemId = 0;
                            string oldItemContext = string.Empty;

                            /* TODO:
                            // Change all references to a class to use the Converter.WoWClass.*
                            //  Ex: Converter.WoWClass.Hunter.ToString()
                            // Change all references to a role to use the Converter.WoWRole.*
                            //  Ex: Converter.WoWRole.DPS.ToString()
                            // Change all references to a Spec to use the Converter.WoWSpecs.*
                            //  Ex: Converter.WoWSpecs.Survival.ToString()
                            // **** OR ****
                            // We can make a member function in the GuildMember class like:
                            //  Ex: gm.IsClassHunter()
                            //  Ex: gm.IsRoleHealer()
                            //  Ex: gm.IsSpecSurvival()
                            // **** OR ****
                            //  EX: gm.Class == Converter.WoWClass.Hunter
                             */

                            // can this member use it?
                            if (Converter.ConvertItemClass(item.ItemClass) == "Armor")
                            {
                                // Armor
                                if (Converter.ConvertItemSubClass(item.ItemClass, item.ItemSubClass) == "Micellaneous")
                                {
                                    // Trinkets
                                    if (Converter.ConvertInventoryType(item.InventoryType) == "trinket")
                                    {
                                        // It's a trinket and not sure who it should go to...
                                        //  So for now, we're going to hard code this by name

                                        /*
                                        // Tier 16
                                        if (item.Name == "Purified Bindings of Immerseus" && gm.Role == Converter.WoWRole.DPS.ToString() && gm.UsesIntellect)
                                        {
                                            charName = gm.Name;
                                        }
                                        else if (item.Name == "Rook's Unlucky Talisman" && gm.Role == "TANK")
                                        {
                                            charName = gm.Name;
                                        }
                                        else if (item.Name == "Fusion-Fire Core" && gm.Role == Converter.WoWRole.DPS.ToString() && gm.UsesStrength)
                                        {
                                            charName = gm.Name;
                                        }
                                        else if (item.Name == "Assurance of Consequence" && gm.Role == Converter.WoWRole.DPS.ToString() && gm.UsesAgility)
                                        {
                                            charName = gm.Name;
                                        }
                                        else if (item.Name == "Prismatic Prison of Pride" && gm.Role == "HEALING")
                                        {
                                            charName = gm.Name;
                                        }
                                        else if (item.Name == "Evil Eye of Galakras" && gm.Role == Converter.WoWRole.DPS.ToString() && gm.UsesStrength)
                                        {
                                            charName = gm.Name;
                                        }
                                        else if (item.Name == "Juggernaut's Focusing Crystal" && gm.Role == "TANK")
                                        {
                                            charName = gm.Name;
                                        }
                                        else if (item.Name == "Haromm's Talisman" && gm.Role == Converter.WoWRole.DPS.ToString() && gm.UsesAgility)
                                        {
                                            charName = gm.Name;
                                        }
                                        else if (item.Name == "Kardris' Toxic Totem" && gm.Role == Converter.WoWRole.DPS.ToString() && gm.UsesIntellect)
                                        {
                                            charName = gm.Name;
                                        }
                                        else if (item.Name == "Nazgrim's Burnished Insignia" && gm.Role == "HEALING")
                                        {
                                            charName = gm.Name;
                                        }
                                        else if (item.Name == "Frenzied Crystal of Rage" && gm.Role == Converter.WoWRole.DPS.ToString() && gm.UsesIntellect)
                                        {
                                            charName = gm.Name;
                                        }
                                        else if (item.Name == "Vial of Living Corruption" && gm.Role == "TANK")
                                        {
                                            charName = gm.Name;
                                        }
                                        else if (item.Name == "Sigil of Rampage" && gm.Role == Converter.WoWRole.DPS.ToString() && gm.UsesIntellect)
                                        {
                                            charName = gm.Name;
                                        }
                                        else if (item.Name == "Thok's Tail Tip" && gm.Role == Converter.WoWRole.DPS.ToString() && gm.UsesStrength)
                                        {
                                            charName = gm.Name;
                                        }
                                        else if (item.Name == "Thok's Acid-Grooved Tooth" && gm.Role == "HEALING")
                                        {
                                            charName = gm.Name;
                                        }
                                        else if (item.Name == "Dysmorphic Samophlange of Discontinuity" && gm.Role == "HEALING")
                                        {
                                            charName = gm.Name;
                                        }
                                        else if (item.Name == "Ticking Ebon Detonator" && gm.Role == Converter.WoWRole.DPS.ToString() && gm.UsesAgility)
                                        {
                                            charName = gm.Name;
                                        }
                                        else if (item.Name == "Skeer's Bloodsoaked Talisman" && gm.Role == Converter.WoWRole.DPS.ToString() && gm.UsesStrength)
                                        {
                                            charName = gm.Name;
                                        }
                                        else if (item.Name == "Black Blood of Y'Shaarj" && gm.Role == Converter.WoWRole.DPS.ToString() && gm.UsesIntellect)
                                        {
                                            charName = gm.Name;
                                        }
                                        else if (item.Name == "Curse of Hubris" && gm.Role == "TANK")
                                        {
                                            charName = gm.Name;
                                        }*/

                                        // Tier 17
                                        // TODO:
                                    }                                     
                                    else if (item.HasSpirit())
                                    {
                                        // Healer Classes
                                        if (gm.Role == "HEALING")
                                        {
                                            charName = gm.Name;
                                        }
                                    }
                                    else if (item.HasTankStats())
                                    {
                                        if (gm.Role == "TANK")
                                        {
                                            charName = gm.Name;
                                        }
                                    }
                                    else if (item.HasStrength())
                                    {
                                        if ((gm.Class == "Paladin" && gm.Spec != "Holy") || gm.Class == "Warrior" || gm.Class == "Death Knight")
                                        {
                                            charName = gm.Name;
                                        }
                                    }
                                    else if (item.HasAgility())
                                    {
                                        if ((gm.Class == "Monk" && gm.Role != "HEALING") || gm.Class == "Rogue" || gm.Class == "Hunter" || (gm.Class == "Druid" && (gm.Spec == "Feral" || gm.Spec == "Guardian")) || (gm.Class == "Shaman" && gm.Spec == "Enhancement"))
                                        {
                                            charName = gm.Name;
                                        }
                                    }
                                    else if (item.HasIntellect())
                                    {
                                        if ((gm.Class == "Paladin" && gm.Spec == "Holy") || (gm.Class == "Monk" && gm.Role == "HEALING") || gm.Class == "Priest" || gm.Class == "Mage" || gm.Class == "Warlock" || (gm.Class == "Druid" && (gm.Spec == "Restoration" || gm.Spec == "Balance")) || (gm.Class == "Shaman" && gm.Spec == "Restoration"))
                                        {
                                            charName = gm.Name;
                                        }
                                    }
                                }
                                else if (Converter.ConvertItemSubClass(item.ItemClass, item.ItemSubClass) == "Plate")
                                {
                                    // Plate = Death Knight, Paladin, Warrior
                                    if (gm.Class == "Paladin" || gm.Class == "Warrior" || gm.Class == "Death Knight")
                                    {
                                        charName = gm.Name;
                                    }
                                }
                                else if (Converter.ConvertItemSubClass(item.ItemClass, item.ItemSubClass) == "Mail")
                                {
                                    // Mail = Hunter, Shaman
                                    if (gm.Class == "Shaman" || gm.Class == "Hunter")
                                    {
                                        charName = gm.Name;
                                    }
                                }
                                else if (Converter.ConvertItemSubClass(item.ItemClass, item.ItemSubClass) == "Leather")
                                {
                                    // Leather = Druid, Monk, Rogue
                                    if (gm.Class == "Druid" || gm.Class == "Monk" || gm.Class == "Rogue")
                                    {
                                        charName = gm.Name;
                                    }
                                }
                                else if (Converter.ConvertItemSubClass(item.ItemClass, item.ItemSubClass) == "Cloth")
                                {
                                    // check to see if it's a back slot first...
                                    if (Converter.ConvertInventoryType(item.InventoryType) == "back")
                                    {
                                        // check for all backs...
                                        if (item.HasSpirit())
                                        {
                                            // This is for Healers only
                                            if (gm.Role == "HEALING")
                                            {
                                                charName = gm.Name;
                                            }
                                        }
                                        else if (item.HasTankStats())
                                        {
                                            // This is for Tanks only
                                            if (gm.Role == "TANK")
                                            {
                                                charName = gm.Name;
                                            }
                                        }
                                        else if (item.HasStrength())
                                        {
                                            // Strength Classes
                                            if ((gm.Class == "Paladin" && gm.Spec != "Holy") || 
                                                gm.Class == "Warrior" || 
                                                gm.Class == "Death Knight")
                                            {
                                                charName = gm.Name;
                                            }
                                        }
                                        else if (item.HasAgility())
                                        {
                                            // Agility Classes
                                            if ((gm.Class == "Monk" && gm.Role != "HEALING") || 
                                                gm.Class == "Rogue" || 
                                                gm.Class == "Hunter" || 
                                                (gm.Class == "Druid" && (gm.Spec == "Feral" || gm.Spec == "Guardian")) || 
                                                (gm.Class == "Shaman" && gm.Spec == "Enhancement"))
                                            {
                                                charName = gm.Name;
                                            }
                                        }
                                        else if (item.HasIntellect())
                                        {
                                            // Intellect Classes
                                            if ((gm.Class == "Monk" && gm.Role == "HEALING") || 
                                                gm.Class == "Mage" || 
                                                gm.Class == "Priest" || 
                                                gm.Class == "Warlock" || 
                                                (gm.Class == "Druid" && (gm.Spec == "Balance" || gm.Spec == "Restoration")) || 
                                                (gm.Class == "Shaman" && gm.Spec == "Restoration") || 
                                                (gm.Class == "Paladin" && gm.Spec == "Holy"))
                                            {
                                                charName = gm.Name;
                                            }
                                        }
                                    }
                                    else if (gm.Class == "Mage" || gm.Class == "Warlock" || gm.Class == "Priest")
                                    {
                                        // it's a regular non-back, cloth piece
                                        charName = gm.Name;
                                    }
                                }
                                else if (Converter.ConvertItemSubClass(item.ItemClass, item.ItemSubClass) == "Shield")
                                {
                                    if (item.HasIntellect())
                                    {
                                        if ((gm.Class == "Paladin" && gm.Spec == "Holy") || (gm.Class == "Shaman" && (gm.Spec == "Elemental" || gm.Spec == "Restoration")))
                                        {
                                            // Holy Paladin or Elemental Shaman or Restoration Shaman
                                            charName = gm.Name;
                                        }
                                    }
                                    else
                                    {
                                        if (gm.Role == "TANK" && (gm.Class == "Paladin" || gm.Class == "Warrior"))
                                        {
                                            charName = gm.Name;
                                        }
                                    }
                                }
                                else
                                {
                                    Logging.DisplayError("Armor [" + item.ItemSubClass + "] not found!");
                                }
                            }
                            else if (Converter.ConvertItemClass(item.ItemClass) == "Weapon")
                            {
                                // Weapon
                                string weapon = Converter.ConvertItemSubClass(item.ItemClass, item.ItemSubClass);

                                if (weapon == "1 Axe")
                                {
                                    // (gm.Class == "Death Knight" || gm.Class == "Monk" || gm.Class == "Paladin" || gm.Class == "Rogue" || gm.Class == "Shaman" || gm.Class == "Warrior")
                                    if (item.HasTankStats())
                                    {
                                        // STR vs AGI
                                        if (gm.Role == "TANK")
                                        {
                                            if (item.HasStrength() && (gm.Class == "Paladin" || gm.Class == "Death Knight" || gm.Class == "Warrior"))
                                            {
                                                charName = gm.Name;
                                            }
                                            else if (item.HasAgility() && gm.Class == "Monk")
                                            {
                                                charName = gm.Name;
                                            }
                                        }
                                    }
                                    else if (item.HasStrength())
                                    {
                                        if (gm.Class == "Death Knight" || (gm.Class == "Paladin" && gm.Role != "HEALING") || gm.Class == "Warrior")
                                        {
                                            charName = gm.Name;
                                        }
                                    }
                                    else if (item.HasAgility())
                                    {
                                        if ((gm.Class == "Monk" && gm.Role != "HEALING") || gm.Class == "Rogue" || (gm.Class == "Shaman" && gm.Spec == "Enhancement"))
                                        {
                                            charName = gm.Name;
                                        }
                                    }
                                    else if (item.HasIntellect())
                                    {
                                        if ((gm.Class == "Monk" && gm.Role == "HEALING") || (gm.Class == "Paladin" && gm.Role == "HEALING") || (gm.Class == "Shaman" && (gm.Spec == "Elemental" || gm.Spec == "Restoration")))
                                        {
                                            charName = gm.Name;
                                        }
                                    }
                                }
                                else if (weapon == "2 Axe")
                                {
                                    if (item.HasStrength() && (gm.Class == "Death Knight" || (gm.Class == "Paladin" && gm.Role == Converter.WoWRole.DPS.ToString()) || (gm.Class == "Warrior" && gm.Role == Converter.WoWRole.DPS.ToString())))
                                    {
                                        charName = gm.Name;
                                    }
                                }
                                else if (weapon == "1 Mace")
                                {
                                    if (item.HasTankStats())
                                    {
                                        // STR vs AGI
                                        if (gm.Role == "TANK")
                                        {
                                            if (item.HasStrength() && (gm.Class == "Paladin" || gm.Class == "Death Knight" || gm.Class == "Warrior"))
                                            {
                                                charName = gm.Name;
                                            }
                                            else if (item.HasAgility() && (gm.Class == "Monk" || gm.Class == "Druid"))
                                            {
                                                charName = gm.Name;
                                            }
                                        }
                                    }
                                    else if (item.HasStrength())
                                    {
                                        if (gm.Class == "Death Knight" || (gm.Class == "Paladin" && gm.Role == "TANK") || (gm.Class == "Warrior" && gm.Spec == "Arms"))
                                        {
                                            charName = gm.Name;
                                        }
                                    }
                                    else if (item.HasAgility())
                                    {
                                        if ((gm.Class == "Monk" && gm.Role != "HEALING") || gm.Class == "Rogue" || (gm.Class == "Shaman" && gm.Spec == "Enhancement") || (gm.Class == "Druid" && (gm.Spec == "Feral" || gm.Spec == "Guardian")))
                                        {
                                            charName = gm.Name;
                                        }
                                    }
                                    else if (item.HasIntellect())
                                    {
                                        if ((gm.Class == "Monk" && gm.Role == "HEALING") || 
                                            (gm.Class == "Paladin" && gm.Role == "HEALING") || 
                                            (gm.Class == "Shaman" && (gm.Spec == "Elemental" || gm.Spec == "Restoration")) || 
                                            gm.Class == "Priest" || 
                                            (gm.Class == "Druid" && (gm.Spec == "Balance" || gm.Spec == "Restoration")))
                                        {
                                            charName = gm.Name;
                                        }
                                    }
                                }
                                else if (weapon == "2 Mace")
                                {
                                    if (item.HasStrength())
                                    {
                                        if (item.HasTankStats())
                                        {
                                            if (gm.Role == "TANK" && gm.Class == "Death Knight")
                                            {
                                                charName = gm.Name;
                                            }
                                        }
                                        else if (gm.Role == Converter.WoWRole.DPS.ToString() && (gm.Class == "Death Knight" || gm.Class == "Paladin" || gm.Class == "Warrior"))
                                        {
                                            charName = gm.Name;
                                        }
                                    }
                                    else if (item.HasAgility() && gm.Class == "Druid" && gm.Role != "HEALING")
                                    {
                                        charName = gm.Name;
                                    }
                                }
                                else if (weapon == "Polearm")
                                {
                                    if (item.HasStrength() && (gm.Class == "Death Knight" || gm.Class == "Paladin" || gm.Class == "Warrior"))
                                    {
                                        charName = gm.Name;
                                    }
                                    else if (item.HasAgility() && (gm.Class == "Druid" || gm.Class == "Monk") && gm.Role != "HEALING")
                                    {
                                        charName = gm.Name;
                                    }
                                }
                                else if (weapon == "1 Sword")
                                {
                                    if (item.HasTankStats())
                                    {
                                        // STR vs AGI
                                        if (gm.Role == "TANK")
                                        {
                                            if (item.HasStrength() && (gm.Class == "Paladin" || gm.Class == "Death Knight" || gm.Class == "Warrior"))
                                            {
                                                charName = gm.Name;
                                            }
                                            else if (item.HasAgility() && gm.Class == "Monk")
                                            {
                                                charName = gm.Name;
                                            }
                                        }
                                    }
                                    else if (item.HasStrength())
                                    {
                                        if ((gm.Class == "Death Knight" && gm.Spec == "Frost") || (gm.Class == "Paladin" && gm.Role == "TANK") || (gm.Class == "Warrior" && gm.Spec != "Arms"))
                                        {
                                            charName = gm.Name;
                                        }
                                    }
                                    else if (item.HasAgility())
                                    {
                                        if ((gm.Class == "Monk" && gm.Role != "HEALING") || gm.Class == "Rogue")
                                        {
                                            charName = gm.Name;
                                        }
                                    }
                                    else if (item.HasIntellect())
                                    {
                                        if ((gm.Class == "Monk" && gm.Role == "HEALING") ||
                                            (gm.Class == "Shaman" && (gm.Class == "Elemental" || gm.Class == "Restoration")) || 
                                            (gm.Class == "Paladin" && gm.Role == "HEALING") || 
                                            gm.Class == "Mage" || 
                                            gm.Class == "Warlock")
                                        {
                                            charName = gm.Name;
                                        }
                                    }
                                }
                                else if (weapon == "2 Sword")
                                {
                                    if (item.HasTankStats())
                                    {
                                        if (gm.Role == "TANK" && gm.Class == "Death Knight")
                                        {
                                            charName = gm.Name;
                                        }
                                    }
                                    else if (gm.Role == Converter.WoWRole.DPS.ToString() && ((gm.Class == "Death Knight" && gm.Spec != "Frost") || gm.Class == "Paladin" || gm.Class == "Warrior"))
                                    {
                                        charName = gm.Name;
                                    }
                                }
                                else if (weapon == "Staff")
                                {
                                    if (item.HasIntellect())
                                    {
                                        if ((gm.Class == "Druid" && (gm.Spec == "Restoration" || gm.Spec == "Balance")) || 
                                            (gm.Class == "Monk" && gm.Spec == "Mistweaver") || 
                                            gm.Class == "Mage" || 
                                            gm.Class == "Priest" || 
                                            gm.Class == "Warlock" || 
                                            (gm.Class == "Shaman" && (gm.Spec == "Restoration" || gm.Spec == "Elemental")))
                                        {
                                            charName = gm.Name;
                                        }
                                    }
                                    else if (item.HasAgility() && ((gm.Class == "Druid" && (gm.Spec == "Guardian" || gm.Spec == "Feral")) || (gm.Class == "Monk" && (gm.Spec == "Brewmaster" || gm.Spec == "Windwalker"))))
                                    {
                                        charName = gm.Name;
                                    }
                                }
                                else if (weapon == "Fist Weapon")
                                {
                                    if (item.HasAgility() && ((gm.Class == "Druid" && (gm.Spec == "Feral" || gm.Spec == "Guardian")) || (gm.Class == "Monk" && gm.Role != "HEALING") || gm.Class == "Rogue" || (gm.Class == "Shaman" && gm.Spec == "Enhancement")))
                                    {
                                        charName = gm.Name;
                                    }
                                    else if (item.HasStrength() && gm.Class == "Warrior")
                                    {
                                        charName = gm.Name;
                                    }
                                }
                                else if (weapon == "Dagger")
                                {
                                    if (item.HasIntellect())
                                    {
                                        if ((gm.Class == "Druid" && (gm.Spec == "Balance" || gm.Spec == "Restoration")) || 
                                            gm.Class == "Mage" || 
                                            gm.Class == "Priest" || 
                                            gm.Class == "Warlock" || 
                                            (gm.Class == "Shaman" && (gm.Spec == "Restoration" || gm.Spec == "Elemental")))
                                        {
                                            charName = gm.Name;
                                        }
                                    }
                                    else if (item.HasAgility() && ((gm.Class == "Druid" && (gm.Spec == "Guardian" || gm.Spec == "Feral")) || gm.Class == "Rogue" || (gm.Class == "Shaman" && gm.Spec == "Enhancement")))
                                    {
                                        charName = gm.Name;
                                    }
                                    else if (item.HasStrength() && gm.Class == "Warrior")
                                    {
                                        charName = gm.Name;
                                    }
                                }
                                else if (weapon == "Wand")
                                {
                                    if (item.HasIntellect())
                                    {
                                        if (gm.Class == "Mage" || gm.Class == "Warlock" || gm.Class == "Priest")
                                        {
                                            charName = gm.Name;
                                        }
                                    }
                                }
                                else if (weapon == "Bow" || weapon == "Rifle" || weapon == "Thrown" || weapon == "Crossbow")
                                {
                                    if (gm.Class == Converter.WoWClass.Hunter.ToString())
                                    {
                                        charName = gm.Name;
                                    }
                                }
                                else
                                {
                                    Logging.DisplayError("Weapon [" + weapon + "] not found!");
                                }
                            }
                            else if (Converter.ConvertItemClass(item.ItemClass) == "Miscellaneous")
                            {
                                // Check if this is an armor token
                                if (item.Name.StartsWith("Helm"))
                                {
                                    item.InventoryType = 1;
                                }
                                else if (item.Name.StartsWith("Shoulders"))
                                {
                                    item.InventoryType = 3;
                                }
                                else if (item.Name.StartsWith("Chest"))
                                {
                                    item.InventoryType = 5;
                                }
                                else if (item.Name.StartsWith("Gauntlets"))
                                {
                                    item.InventoryType = 10;
                                }
                                else if (item.Name.StartsWith("Leggings"))
                                {
                                    item.InventoryType = 7;
                                }
                                else if (item.Name.StartsWith("Essence"))
                                {
                                    // item.InventoryType = 7;
                                    // todo: need to be able check an item that could be any of the following: head/shoulder/chest/hands/legs tier piece
                                }

                                if (Converter.ConvertInventoryType(item.InventoryType) != "error")
                                {
                                    if (item.Name.EndsWith(" of the Cursed Conqueror") && (gm.Class == "Paladin" || gm.Class == "Priest" || gm.Class == "Warlock"))
                                    {
                                        charName = gm.Name;
                                    }
                                    else if (item.Name.EndsWith(" of the Cursed Protector") && (gm.Class == "Warrior" || gm.Class == "Hunter" || gm.Class == "Shaman" || gm.Class == "Monk"))
                                    {
                                        charName = gm.Name;
                                    }
                                    else if (item.Name.EndsWith(" of the Cursed Vanquisher") && (gm.Class == "Rogue" || gm.Class == "Death Knight" || gm.Class == "Mage" || gm.Class == "Druid"))
                                    {
                                        charName = gm.Name;
                                    }
                                }
                            }

                            // Can the row be added to data table?
                            if (!string.IsNullOrEmpty(charName))
                            {
                                string slot = Converter.ConvertInventoryType(item.InventoryType);
                                bool pass = false;
                                int ilvlOld = 0;
                                int ilvlNew = 0;

                                if (slot == "finger")
                                {
                                    pass |= gm.ItemAudits.ContainsKey("finger1");
                                    pass |= gm.ItemAudits.ContainsKey("finger2");
                                }
                                else if (slot == "trinket")
                                {
                                    pass |= gm.ItemAudits.ContainsKey("trinket1");
                                    pass |= gm.ItemAudits.ContainsKey("trinket2");
                                }
                                else if (slot == "mainHand")
                                {
                                    // Need to check that either a 2 hander is being used
                                    //   or 2 one handers, or 1 hand and off hand is equiped
                                    string weapon = Converter.ConvertItemSubClass(item.ItemClass, item.ItemSubClass);

                                    if (weapon == "1 Axe" || weapon == "1 Mace" || weapon == "1 Sword" || weapon == "Fist Weapon" || weapon == "Dagger" || weapon == "Wand")
                                    {
                                        // this is NOT a 2 handed weapon, check to make sure both hands are equiped

                                        // Look for 2 x 1 handed weapons
                                        int mainHands = gm.ItemAudits.Keys.Where(kv => kv == "mainHand").Count();

                                        if (mainHands == 2)
                                        {
                                            pass = gm.ItemAudits.ContainsKey(slot);
                                        }
                                        else if (gm.ItemAudits.ContainsKey("Shield") || gm.ItemAudits.ContainsKey("offHand"))
                                        {
                                            pass = gm.ItemAudits.ContainsKey(slot);
                                        }
                                    }
                                    else
                                    {
                                        pass = gm.ItemAudits.ContainsKey(slot);
                                    }
                                }
                                else
                                {
                                    pass = gm.ItemAudits.ContainsKey(slot);
                                }

                                // Did it pass?  Is it an upgrade?
                                if (pass)
                                {
                                    ilvlNew = item.ItemLevel;

                                    if (slot == "finger")
                                    {
                                        // Need to account for any ring duplications
                                        if (item.Name == gm.ItemAudits["finger1"].Name)
                                        {
                                            ilvlOld = gm.ItemAudits["finger1"].ItemLevel;
                                            oldItemId = gm.ItemAudits["finger1"].Id;
                                        }
                                        else if (item.Name == gm.ItemAudits["finger2"].Name)
                                        {
                                            ilvlOld = gm.ItemAudits["finger2"].ItemLevel;
                                            oldItemId = gm.ItemAudits["finger2"].Id;
                                        }
                                        else
                                        {
                                            // upgrade!
                                            ilvlOld = gm.ItemAudits["finger1"].ItemLevel > gm.ItemAudits["finger2"].ItemLevel ? gm.ItemAudits["finger2"].ItemLevel : gm.ItemAudits["finger1"].ItemLevel;
                                            oldItemId = gm.ItemAudits["finger1"].Id > gm.ItemAudits["finger2"].Id ? gm.ItemAudits["finger2"].Id : gm.ItemAudits["finger1"].Id;
                                        }
                                    }
                                    else if (slot == "trinket")
                                    {
                                        // Need to account for any ring duplications
                                        if (item.Name == gm.ItemAudits["trinket1"].Name)
                                        {
                                            ilvlOld = gm.ItemAudits["trinket1"].ItemLevel;
                                            oldItemId = gm.ItemAudits["trinket1"].ItemLevel;
                                        }
                                        else if (item.Name == gm.ItemAudits["trinket2"].Name)
                                        {
                                            ilvlOld = gm.ItemAudits["trinket2"].ItemLevel;
                                            oldItemId = gm.ItemAudits["trinket2"].ItemLevel;
                                        }
                                        else
                                        {
                                            // upgrade!
                                            ilvlOld = gm.ItemAudits["trinket1"].ItemLevel > gm.ItemAudits["trinket2"].ItemLevel ? gm.ItemAudits["trinket2"].ItemLevel : gm.ItemAudits["trinket1"].ItemLevel;
                                            oldItemId = gm.ItemAudits["trinket1"].Id > gm.ItemAudits["trinket2"].Id ? gm.ItemAudits["trinket2"].Id : gm.ItemAudits["trinket1"].Id;
                                        }
                                    }
                                    else if (slot == "mainHand" || slot == "offHand")
                                    {
                                        string weapon = Converter.ConvertItemSubClass(item.ItemClass, item.ItemSubClass);

                                        // Need to check for dual hands vs 2 handers vs one hand + off hand...
                                        if ((gm.Class == "Rogue" || (gm.Class == "Shaman" && gm.Spec == "Enhancement") || (gm.Class == "Warrior" && gm.Spec == "Fury") ||
                                            (gm.Class == "Druid" && (gm.Spec == "Guardian" || gm.Spec == "Feral")) || (gm.Class == "Monk" && (gm.Spec == "Brewmaster" ||
                                            gm.Spec == "Windwalker")) || (gm.Class == "Death Knight" && (gm.Spec == "Unholy" || gm.Spec == "Frost"))) &&
                                            (gm.ItemAudits["offHand"].ItemLevel > 0))
                                        {
                                            ilvlOld = gm.ItemAudits["mainHand"].ItemLevel > gm.ItemAudits["offHand"].ItemLevel ? gm.ItemAudits["offHand"].ItemLevel : gm.ItemAudits["mainHand"].ItemLevel;
                                            oldItemId = gm.ItemAudits["mainHand"].Id > gm.ItemAudits["offHand"].Id ? gm.ItemAudits["offHand"].Id : gm.ItemAudits["mainHand"].Id;
                                        }
                                        else if (slot == "offHand" && gm.ItemAudits["offHand"].ItemLevel == 0)
                                        {
                                            int itemIdMainHand = gm.ItemAudits["mainHand"].Id;
                                            int slotType = 0;
                                            ItemInfo itemMainHand = null;
                                            string itemContextMainHand = gm.ItemAudits["mainHand"].Context;

                                            itemMainHand = FormMain.Items.GetItem(itemIdMainHand, itemContextMainHand);

                                            if (itemMainHand != null)
                                            {
                                                slotType = itemMainHand.InventoryType;

                                                // if slot if offHand and currently doesn't have an offHand equipped
                                                //  and mainhand is a 2 hander... compare iLevel with mainHand
                                                if (slotType == 17 || slotType == 15 || slotType == 26)
                                                {
                                                    ilvlOld = gm.ItemAudits["mainHand"].ItemLevel;
                                                    oldItemId = gm.ItemAudits["mainHand"].Id;
                                                    oldItemContext = gm.ItemAudits["mainHand"].Context;
                                                }
                                                else
                                                {
                                                    ilvlOld = gm.ItemAudits[Converter.ConvertInventoryType(item.InventoryType)].ItemLevel;
                                                    oldItemId = gm.ItemAudits[Converter.ConvertInventoryType(item.InventoryType)].Id;
                                                    oldItemContext = gm.ItemAudits[Converter.ConvertInventoryType(item.InventoryType)].Context;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            // upgrade!
                                            ilvlOld = gm.ItemAudits[Converter.ConvertInventoryType(item.InventoryType)].ItemLevel;
                                            oldItemId = gm.ItemAudits[Converter.ConvertInventoryType(item.InventoryType)].Id;
                                            oldItemContext = gm.ItemAudits[Converter.ConvertInventoryType(item.InventoryType)].Context;
                                        }
                                    }
                                    else
                                    {
                                        // upgrade!
                                        ilvlOld = gm.ItemAudits[Converter.ConvertInventoryType(item.InventoryType)].ItemLevel;
                                        oldItemId = gm.ItemAudits[Converter.ConvertInventoryType(item.InventoryType)].Id;
                                        oldItemContext = gm.ItemAudits[Converter.ConvertInventoryType(item.InventoryType)].Context;
                                    }

                                    if (ilvlNew > ilvlOld)
                                    {
                                        // This is a definite upgrade
                                        DataRow dr = loot.NewRow();
                                        dr["Upgrade"] = ilvlNew - ilvlOld;
                                        dr["ItemId"] = item.Id;
                                        dr["ItemName"] = item.Name;
                                        dr["ItemSlot"] = slot;
                                        dr["CharacterName"] = charName;
                                        dr["ItemILevel"] = ilvlNew;
                                        dr["OldItemILevel"] = ilvlOld;
                                        dr["OldItemId"] = oldItemId;
                                        dr["OldItemContext"] = oldItemContext;
                                        loot.Rows.Add(dr);
                                    }                                    
                                }
                                else
                                {
                                    Logging.DisplayError("Slot #" + item.InventoryType + "[" + slot + "] not found!");
                                }
                            }
                        }
                    }
                }
            }

            return loot;
        }

        #endregion

        /// <summary>
        /// Results a string of the context needed depending on the raid difficulty
        /// </summary>
        /// <param name="raidName"></param>
        /// <returns></returns>
        public string GetContext(string raidName)
        {
            string context = string.Empty;

            if (raidName.Contains("Raid Finder"))
            {
                context = "raid-finder";
            }
            else if (raidName.Contains("Normal"))
            {
                context = "raid-normal";
            }
            else if (raidName.Contains("Heroic"))
            {
                context = "raid-heroic";
            }
            else if (raidName.Contains("Mythic"))
            {
                context = "raid-mythic";
            }

            return context;
        }
    }
}
