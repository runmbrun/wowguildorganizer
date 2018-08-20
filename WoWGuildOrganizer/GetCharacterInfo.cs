// -----------------------------------------------------------------------
// <copyright file="GetCharacterInfo.cs" company="Vangent, Inc.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace WoWGuildOrganizer
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Text.RegularExpressions;    

    /// <summary>
    /// Pulls a character's information back from a web page
    /// </summary>
    public class GetCharacterInfo
    {
        /// <summary>
        /// flag is debug is turned on
        /// </summary>
        private bool debug = false;

        /// <summary>
        /// Location of the debug log file
        /// </summary>
        private string debugFile = @"c:\temp\GetCharacterInfo.log";

        /// <summary>
        /// The information of the character is stored in this class
        /// </summary>
        private GuildMember guildie;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetCharacterInfo" /> class.
        /// </summary>
        public GetCharacterInfo()
        {            
            this.Guildie = new GuildMember();
        }

        /// <summary>
        /// Gets or sets the property that stores the character information
        /// </summary>
        public GuildMember Guildie
        {
            get
            {
                return this.guildie;
            }

            set
            {
                this.guildie = value;
            }
        }

        /// <summary>
        /// Pulls all character information from the Web Site passed in
        ///  This is just for the Raid Info tab
        /// </summary>
        /// <param name="webpage">url of the web page to search</param>
        /// <returns>true if successful</returns>
        public bool CollectFullData(string webpage)
        {
            bool success = false;

            try
            {
                GetWebSiteData getSiteData = new GetWebSiteData();

                // Collect the data from the webpage
                if (!getSiteData.Parse(webpage))
                {
                    // error
                }
                else
                {
                    // now parse the data

                    // piece in web site that is important
                    // code to expresso => replace "" with "
                    // expresso to code => replace " with ""

                    /*  Example:
                      character":{.*?name":"(?<Name>\w+).*?class":(?<Class>\d+).*?race":(?<Race>\d+).*?level":(?<Level>\d+).*?achievementPoints":(?<AchPts>\d+).*?}{
                      "lastModified":1311454367000,
                      "name":"Breaktooth",
                      "realm":"Thrall",
                      "class":11,
                      "race":8,
                      "gender":0,
                      "level":75,
                      "achievementPoints":390,
                      "thumbnail":"thrall/19/74347795-avatar.jpg",
                      "items":{
                        "averageItemLevel":127,
                        "averageItemLevelEquipped":116,
                        "head":{
                          "id":39034,
                          "name":"Bearskin Helm",
                          "icon":"inv_helmet_108",
                          "quality":2,
                          "tooltipParams":{}
                        },    
                    */

                    string dataString = getSiteData.Data;
                    string search = @"name"":""(?<name>\w+)"".*?""class"":(?<class>\d+).*?""race"":(?<race>\d+).*?""level"":(?<level>\d+).*?""achievementPoints"":(?<achs>\d+).*?""averageItemLevel"":(?<avg_iLevel>\d+).*?averageItemLevelEquipped"":(?<equip_iLevel>\d+),(?<items>.*)}.*?primary"":.*?name"":""(?<Profession1>\w+).*?name"":""(?<Profession2>\w+).*?""selected"":true,(?<Talents>.*)";
                    Regex test = new Regex(search, RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.CultureInvariant);

                    this.Debug(search, dataString);

                    // Get the Average iLevel of character's Gear
                    foreach (Match result in test.Matches(dataString))
                    {
                        // todo: last modified needed
                        if (result.Groups["name"].Success)
                        {
                            this.Guildie.Name = result.Groups["name"].Value.ToString();
                        }

                        if (result.Groups["class"].Success)
                        {
                            this.Guildie.Class = Converter.ConvertClass(result.Groups["class"].Value.ToString());
                        }

                        if (result.Groups["race"].Success)
                        {
                            this.Guildie.Race = Converter.ConvertRace(result.Groups["race"].Value.ToString());
                        }

                        if (result.Groups["level"].Success)
                        {
                            this.Guildie.Level = Convert.ToInt32(result.Groups["level"].Value);
                        }

                        if (result.Groups["achs"].Success)
                        {
                            this.Guildie.AchievementPoints = Convert.ToInt32(result.Groups["achs"].Value.ToString());
                        }

                        if (result.Groups["avg_iLevel"].Success)
                        {
                            this.Guildie.MaxiLevel = Convert.ToInt32(result.Groups["avg_iLevel"].Value.ToString());
                        }

                        if (result.Groups["equip_iLevel"].Success)
                        {
                            this.Guildie.EquipediLevel = Convert.ToInt32(result.Groups["equip_iLevel"].Value.ToString());
                        }

                        if (result.Groups["Profession1"].Success)
                        {
                            this.Guildie.Profession1 = result.Groups["Profession1"].Value;
                        }

                        if (result.Groups["Profession2"].Success)
                        {
                            this.Guildie.Profession2 = result.Groups["Profession2"].Value;
                        }

                        if (result.Groups["Talents"].Success)
                        {
                            string search2 = @".*?""spec"":{""name"":""(?<Spec>[a-zA-Z0-9 ]+)"".*?,""role"":""(?<Role>\w+)"".*";
                            Regex test2 = new Regex(search2, RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.CultureInvariant);

                            foreach (Match result2 in test2.Matches(result.Groups["Talents"].Value))
                            {
                                if (result2.Groups["Spec"].Success)
                                {
                                    this.Guildie.Spec = result2.Groups["Spec"].Value;
                                }

                                if (result2.Groups["Role"].Success)
                                {
                                    this.Guildie.Role = result2.Groups["Role"].Value;
                                }
                            }
                        }

                        if (result.Groups["items"].Success)
                        {
                            this.Guildie.ItemAudits = this.ParseItems(result.Groups["items"].Value);
                        }
                    }

                    success = true;
                }
            }
            catch (Exception ex)
            {
                Logging.Debug(string.Format("ERROR: {0} in CollectFullData() in GetCharacterInfo.cs", ex.Message));
            }

            return success;
        }

        /// <summary>
        /// Pulls all character information from the Web Site passed in
        ///  This is just for the Raid Info tab
        /// </summary>
        /// <param name="characterName">Name of the character whose data is being requested</param>
        /// /// <param name="realm">Realm of the character whose data is being requested</param>
        /// <returns>true if successful</returns>
        public bool GetJSONData(string characterName, string realm)
        {
            bool success = false;

            try
            {
                GetWebJSONData getData = new GetWebJSONData();
                JSONCharacterData data = getData.GetCharacterJSONData(characterName, realm);

                if (data != null)
                {
                    // Get the basic information here
                    this.Guildie.Name = data.Name;
                    this.Guildie.Class = Converter.ConvertClass(data.Class);
                    this.Guildie.Race = Converter.ConvertRace(data.Race);
                    this.Guildie.Level = data.Level;
                    this.Guildie.AchievementPoints = data.AchievementPoints;
                    this.Guildie.MaxiLevel = data.Items.AverageItemLevel;
                    this.Guildie.EquipediLevel = data.Items.AverageItemLevelEquipped;
                    this.Guildie.Profession1 = string.Empty;
                    this.Guildie.Profession2 = string.Empty;

                    // Get the Professions
                    if (data.Professions.Primary != null && data.Professions.Primary.Count > 0)
                    {
                        this.Guildie.Profession1 = data.Professions.Primary[0].Name;
                    }

                    if (data.Professions.Primary != null && data.Professions.Primary.Count > 1)
                    {
                        this.Guildie.Profession2 = data.Professions.Primary[1].Name;
                    }

                    // Get the Specs
                    if (data.Talents != null && data.Talents.Count > 0)
                    {
                        foreach (JSONCharacterTalentData talent in data.Talents)
                        {
                            if (talent.Selected != null && Convert.ToBoolean(talent.Selected) == true)
                            {
                                this.Guildie.Spec = talent.Spec.Name;
                                this.Guildie.Role = talent.Spec.Role;
                                break;
                            }                            
                        }                        
                    }

                    // Get the items
                    this.Guildie.ItemAudits = this.GetItemData(data);

                    success = true;
                }                
            }
            catch (Exception ex)
            {
                Logging.Log($"ERROR: {ex.Message} in CollectFullJSONData() in GetCharacterInfo.cs");
                Logging.DisplayError($"ERROR: {ex.Message} in CollectFullJSONData() in GetCharacterInfo.cs");
                success = false;
            }

            return success;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private Dictionary<string, ItemAudit> GetItemData(JSONCharacterData data)
        {
            Dictionary<string, ItemAudit> items = new Dictionary<string, ItemAudit>();
            int i = 0;

            // Go through all the slots and fill out this dictionary with the JSON data from the web site
            try
            {
                // fill out a blank array with all the slots
                for (i = 0; i < 18; i++)
                {
                    ItemAudit audit = new ItemAudit();
                    JSONCharacterItem newItem = null;
                    ItemInfo item = new ItemInfo();

                    switch (i)
                    {
                        // Head
                        case 0:
                            audit.Slot = "head";
                            audit.MissingItem = "1";
                            newItem = data.Items.Head;                            
                            break;
                        case 1:
                            audit.Slot = "neck";
                            audit.MissingItem = "1";
                            newItem = data.Items.Neck;
                            break;
                        case 2:
                            audit.Slot = "shoulder";
                            audit.MissingItem = "1";
                            newItem = data.Items.Shoulder;
                            break;
                        case 3:
                            audit.Slot = "back";
                            audit.MissingItem = "1";
                            newItem = data.Items.Back;
                            break;
                        case 4:
                            audit.Slot = "chest";
                            audit.MissingItem = "1";
                            newItem = data.Items.Chest;
                            break;
                        case 5:
                            audit.Slot = "tabard";
                            audit.MissingItem = "1";
                            newItem = data.Items.Tabard;
                            break;
                        case 6:
                            audit.Slot = "shirt";
                            audit.MissingItem = "1";
                            newItem = data.Items.Shirt;
                            break;
                        case 7:
                            audit.Slot = "wrist";
                            audit.MissingItem = "1";
                            newItem = data.Items.Wrist;
                            break;
                        case 8:
                            audit.Slot = "hands";
                            audit.MissingItem = "1";
                            newItem = data.Items.Hands;
                            break;
                        case 9:
                            audit.Slot = "waist";
                            audit.MissingItem = "1";
                            newItem = data.Items.Waist;
                            break;
                        case 10:
                            audit.Slot = "legs";
                            audit.MissingItem = "1";
                            newItem = data.Items.Legs;
                            break;
                        case 11:
                            audit.Slot = "feet";
                            audit.MissingItem = "1";
                            newItem = data.Items.Feet;
                            break;
                        case 12:
                            audit.Slot = "finger1";
                            audit.MissingItem = "1";
                            newItem = data.Items.Finger1;
                            break;
                        case 13:
                            audit.Slot = "finger2";
                            audit.MissingItem = "1";
                            newItem = data.Items.Finger2;
                            break;
                        case 14:
                            audit.Slot = "trinket1";
                            audit.MissingItem = "1";
                            newItem = data.Items.Trinket1;
                            break;
                        case 15:
                            audit.Slot = "trinket2";
                            audit.MissingItem = "1";
                            newItem = data.Items.Trinket2;
                            break;
                        case 16:
                            audit.Slot = "mainHand";
                            audit.MissingItem = "1";
                            newItem = data.Items.MainHand;
                            break;
                        case 17:
                            audit.Slot = "offHand";
                            audit.MissingItem = "1";
                            newItem = data.Items.OffHand;
                            break;
                    }

                    // See if an item was found
                    if (newItem != null)
                    {
                        // Get the Item Id
                        audit.Id = newItem.Id;

                        // Check to see if it has a Context
                        if (newItem.Context != null)
                        {
                            audit.Context = newItem.Context;
                        }

                        // Now check for the item in the item cache
                        if (!WoWGuildOrganizer.FormMain.Items.Contains(audit.Id, audit.Context))
                        {
                            // First fetch the data
                            GetItemInfo get = new GetItemInfo();

                            // Get the item data
                            if (get.CollectData(audit.Id, audit.Context))
                            {
                                item = get.Item;

                                // Need to add in this item to the Item Cache
                                WoWGuildOrganizer.FormMain.Items.AddItem(item, audit.Context);
                            }
                            else
                            {
                                // todo: trap it here
                                Logging.Error($"Data not collected from Item with ID = 0. [{audit.Id}]");
                            }
                        }
                        else
                        {
                            item = (ItemInfo)WoWGuildOrganizer.FormMain.Items.GetItem(audit.Id, audit.Context);
                        }
                    }

                    // Check if an item was found
                    if (item.Id != 0)
                    {
                        // Set CanEnchant and CanSocket values
                        audit.CanEnchant(item.CanEnchant);
                        audit.CanSocket(item.CanSocket);
                        audit.SocketCount(item.SocketCount);
                        audit.MissingItem = "0";                    

                        audit.Name = newItem.Name;
                        audit.Quality = newItem.Quality;
                        audit.ItemLevel = newItem.ItemLevel;
                        audit.Context = newItem.Context;

                        // TODO: This will need changes for Legion
                        if (newItem.BonusLists != null)
                        {
                            string bonuses = string.Join<int>(",", newItem.BonusLists);
                        
                            audit.SetBonusLists(bonuses);

                            if (bonuses.Contains("563") || bonuses.Contains("564") || bonuses.Contains("565"))
                            {
                                // Changes to Gem Sockets for WoD
                                //  Now a prismatic gem socket will show up in the bonus lists
                                audit.CanSocket(true);
                                audit.SocketCount(1);
                            }
                        }

                        // Make sure ToolTips is always at the end, as it does other calculations
                        if (newItem.ToolTipParams != null)
                        {
                            audit.SetToolTips(newItem.ToolTipParams);
                        }
                    }
                    else
                    {
                        audit.MissingItem = "1";

                        // Ususally this error is from the GetItem() function got finding the proper data
                        Logging.Error(string.Format("Found a Item with ID = 0. [{0}]", audit.Id));
                    }

                    items.Add(audit.Slot, audit);
                }
            }
            catch (Exception ex)
            {
                if (data != null && !string.IsNullOrEmpty(data.Name))
                {
                    Logging.DisplayError($"GetCharacterInfo.GetItemData() - slot {i} missing for {data.Name}: {ex.Message}");
                }
                else
                {
                    Logging.DisplayError($"GetCharacterInfo.GetItemData() - slot missing: {ex.Message}");
                }                
            }

            return items;
        }

        /// <summary>
        /// Parses the raw character data from the web page
        /// </summary>
        /// <param name="data">raw character data in form of a string</param>
        /// <returns>dictionary containing all the items found on the character</returns>
        public Dictionary<string, ItemAudit> ParseItems(string data)
        {
            Dictionary<string, ItemAudit> items = this.FillOutItemAuditsArrayList();
            string search = @"(?<Slot>\w+):{id:(?<Id>\d+).*?name:(?<Name>[A-Za-z '""\\.,:-]+?),icon.*?quality:(?<Quality>\d+).*?,itemLevel:(?<ItemLevel>\d+).*?tooltipParams:{(?<ToolTips>.*?)}.*?context:(?<Context>.*?),.*?bonusLists:\[(?<BonusLists>.*?)\]}";
            Regex test = new Regex(search, RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.CultureInvariant);

            this.Debug(search, data.Replace("\"", ""));

            // Get the item info
            foreach (Match result in test.Matches(data.Replace("\"", "")))
            {
                // First get the slot, if no slot then it's an missing item
                if (result.Groups["Slot"].Success)
                {
                    string slot = result.Groups["Slot"].Value.ToString();
                    ItemAudit audit = new ItemAudit();
                    ItemInfo item = new ItemInfo();

                    if (!items.ContainsKey(slot))
                    {
                        // bad - slot doesn't exist
                        Logging.DisplayError(string.Format("GetCharacterInfo.ParseItems() - slot missing: {0}", slot));
                    }
                    else
                    {
                        audit = items[slot];

                        // First get the item id
                        if (result.Groups["Id"].Success)
                        {
                            audit.Id = Convert.ToInt32(result.Groups["Id"].Value);
                        }

                        // Now check for the item in the item cache
                        if (!WoWGuildOrganizer.FormMain.Items.Contains(audit.Id, audit.Context))
                        {
                            // First fetch the data
                            GetItemInfo get = new GetItemInfo();
                            string context = string.Empty;

                            // Check to see if there is a context available
                            if (result.Groups["Context"].Success && result.Groups["Context"].Value.ToString() != "")
                            {
                                context = result.Groups["Context"].Value.ToString();
                            }
                            
                            // Get the item data
                            if (get.CollectData(audit.Id, context))
                            {
                                item = get.Item;

                                // Need to add in this item to the Item Cache
                                WoWGuildOrganizer.FormMain.Items.AddItem(item, context);
                            }
                            else
                            {
                                // todo: trap it here
                                Logging.Error(string.Format("Data not collected from Item with ID = 0. [{0}]", audit.Id));
                            }
                        }
                        else
                        {
                            item = (ItemInfo)WoWGuildOrganizer.FormMain.Items.GetItem(audit.Id, audit.Context);
                        }

                        if (item.Id != 0)
                        {
                            // Set CanEnchant and CanSocket values
                            audit.CanEnchant(item.CanEnchant);
                            audit.CanSocket(item.CanSocket);
                            audit.SocketCount(item.SocketCount);
                            audit.MissingItem = "0";
                        }
                        else
                        {
                            audit.MissingItem = "1";

                            // Ususally this error is from the GetItem() function got finding the proper data
                            Logging.Error(string.Format("Found a Item with ID = 0. [{0}]", audit.Id));
                        }

                        if (result.Groups["Name"].Success)
                        {
                            audit.Name = result.Groups["Name"].Value.ToString();
                        }

                        if (result.Groups["Quality"].Success)
                        {
                            audit.Quality = Convert.ToInt32(result.Groups["Quality"].Value);
                        }

                        if (result.Groups["ItemLevel"].Success)
                        {
                            audit.ItemLevel = Convert.ToInt32(result.Groups["ItemLevel"].Value);
                        }

                        if (result.Groups["Context"].Success)
                        {
                            audit.Context = result.Groups["Context"].Value.ToString();
                        }

                        if (result.Groups["BonusLists"].Success)
                        {
                            string bonuses = result.Groups["BonusLists"].Value.ToString();

                            audit.SetBonusLists(bonuses);

                            if (bonuses.Contains("563") || bonuses.Contains("564") || bonuses.Contains("565"))
                            {
                                // Changes to Gem Sockets for WoD
                                //  Now a prismatic gem socket will show up in the bonus lists
                                audit.CanSocket(true);
                                audit.SocketCount(1);
                            }
                        }

                        // Make sure ToolTips is always at the end, as it does other calculations
                        if (result.Groups["ToolTips"].Success)
                        {
                            audit.SetToolTips(result.Groups["ToolTips"].Value.ToString());
                        }
                    }
                }
            }

            return items;
        }

        /// <summary>
        /// Fills out the item audit information.  More information about the item.
        /// </summary>
        /// <returns>dictionary of the item audit information</returns>
        private Dictionary<string, ItemAudit> FillOutItemAuditsArrayList()
        {
            Dictionary<string, ItemAudit> itemAudits = new Dictionary<string, ItemAudit>();

            // fill out a blank array with all the slots
            for (int i = 0; i < 18; i++)
            {
                ItemAudit audit = new ItemAudit();

                switch (i)
                {
                    case 0:
                        audit.Slot = "head";
                        audit.MissingItem = "1";
                        break;
                    case 1:
                        audit.Slot = "neck";
                        audit.MissingItem = "1";
                        break;
                    case 2:
                        audit.Slot = "shoulder";
                        audit.MissingItem = "1";
                        break;
                    case 3:
                        audit.Slot = "back";
                        audit.MissingItem = "1";
                        break;
                    case 4:
                        audit.Slot = "chest";
                        audit.MissingItem = "1";
                        break;
                    case 5:
                        audit.Slot = "tabard";
                        audit.MissingItem = "1";
                        break;
                    case 6:
                        audit.Slot = "shirt";
                        audit.MissingItem = "1";
                        break;
                    case 7:
                        audit.Slot = "wrist";
                        audit.MissingItem = "1";
                        break;
                    case 8:
                        audit.Slot = "hands";
                        audit.MissingItem = "1";
                        break;
                    case 9:
                        audit.Slot = "waist";
                        audit.MissingItem = "1";
                        break;
                    case 10:
                        audit.Slot = "legs";
                        audit.MissingItem = "1";
                        break;
                    case 11:
                        audit.Slot = "feet";
                        audit.MissingItem = "1";
                        break;
                    case 12:
                        audit.Slot = "finger1";
                        audit.MissingItem = "1";
                        break;
                    case 13:
                        audit.Slot = "finger2";
                        audit.MissingItem = "1";
                        break;
                    case 14:
                        audit.Slot = "trinket1";
                        audit.MissingItem = "1";
                        break;
                    case 15:
                        audit.Slot = "trinket2";
                        audit.MissingItem = "1";
                        break;
                    case 16:
                        audit.Slot = "mainHand";
                        audit.MissingItem = "1";
                        break;
                    case 17:
                        audit.Slot = "offHand";
                        audit.MissingItem = "1";
                        break;
                }

                itemAudits.Add(audit.Slot, audit);
            }

            return itemAudits;
        }

        #region DEBUG

        /// <summary>
        /// Writes out all the web page and regular expression data
        /// Helps in debugging
        /// </summary>
        /// <param name="search">string value of what is being searched for</param>
        /// <param name="dataString">raw string data from web page</param>
        private void Debug(string search, string dataString)
        {
            // this is for debugging only
            if (this.debug)
            {
                // Create the output file.
                using (FileStream fs = File.Create(this.debugFile))
                {
                    // NO OP
                }

                // Open the stream and write to it
                using (FileStream fs = File.OpenWrite(this.debugFile))
                {
                    byte[] info = new UTF8Encoding(true).GetBytes("   ");
                    
                    // Add some information to the file.
                    fs.Write(info, 0, info.Length);
                    info = new UTF8Encoding(true).GetBytes(search);
                    fs.Write(info, 0, info.Length);
                    info = new UTF8Encoding(true).GetBytes(dataString);
                    fs.Write(info, 0, info.Length);
                }
            }
        }

        #endregion
    }
}
