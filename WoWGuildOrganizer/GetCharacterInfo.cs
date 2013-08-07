using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;



namespace WoWGuildOrganizer
{
    class GetCharacterInfo
    {
        Boolean Debug = false;
        String DebugFile = @"c:\temp\GetCharacterInfo.log";

        private GuildMember _guildie;
        public GuildMember Guildie
        {
            set { _guildie = value; }
            get { return _guildie; }
        }

        /// <summary>
        /// 
        /// </summary>
        public GetCharacterInfo()
        {            
            Guildie = new GuildMember();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="WebPage"></param>
        /// <returns></returns>
        public Boolean CollectData(String WebPage)
        {
            Boolean Success = false;


            try
            {
                GetWebSiteData getSiteData = new GetWebSiteData();


                if (!getSiteData.Parse(WebPage))
                {
                    // error
                }
                else
                {
                    // now parse the data

                    // piece in web site that is important
                    // code to expresso => replace "" with "
                    // expresso to code => replace " with ""

                    /*
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


                    String DataString = getSiteData.Data;
                    String Search = @"averageItemLevel"":(?<avg_iLevel>\d+).*?averageItemLevelEquipped"":(?<equip_iLevel>\d+),.*?primary"":.*?name"":""(?<Profession1>\w+).*?name"":""(?<Profession2>\w+).*?""selected"":true,.*?""spec"":{""name"":""(?<Spec>\w+)"".*?,""role"":""(?<Role>\w+)""";
                    Regex test = new Regex(Search, RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.CultureInvariant);

                    #region DEBUG
                    // this is for debugging only
                    if (Debug)
                    {
                        // Create the output file.
                        using (FileStream fs = File.Create(DebugFile)) { }
                        // Open the stream and write to it
                        using (FileStream fs = File.OpenWrite(DebugFile))
                        {
                            Byte[] info = new UTF8Encoding(true).GetBytes("   ");
                            // Add some information to the file.
                            fs.Write(info, 0, info.Length);
                            info = new UTF8Encoding(true).GetBytes(Search);
                            fs.Write(info, 0, info.Length);
                            info = new UTF8Encoding(true).GetBytes(DataString);
                            fs.Write(info, 0, info.Length);
                        }
                    }
                    #endregion

                    // Get the Average iLevel of character's Gear
                    foreach (Match result in test.Matches(DataString))
                    {
                        if (result.Groups["avg_iLevel"].Success)
                        {
                            Guildie.MaxiLevel = Convert.ToInt32(result.Groups["avg_iLevel"].Value.ToString());
                        }

                        if (result.Groups["equip_iLevel"].Success)
                        {
                            Guildie.EquipediLevel = Convert.ToInt32(result.Groups["equip_iLevel"].Value.ToString());
                        }

                        if (result.Groups["Profession1"].Success)
                        {
                            Guildie.Profession1 = result.Groups["Profession1"].Value;
                        }

                        if (result.Groups["Profession2"].Success)
                        {
                            Guildie.Profession2 = result.Groups["Profession2"].Value;
                        }

                        if (result.Groups["Spec"].Success)
                        {
                            Guildie.Spec = result.Groups["Spec"].Value;
                        }

                        if (result.Groups["Role"].Success)
                        {
                            Guildie.Role = result.Groups["Role"].Value;
                        }
                    }

                    Success = true;
                }
            }
            catch (Exception ex)
            {
                String Error = String.Format("ERROR: ", ex.Message);
            }

            return Success;
        }

        /// <summary>
        /// Pulls all character information from the Web Site passed in
        ///  This is just for the Raid Info tab
        /// </summary>
        /// <param name="WebPage"></param>
        /// <returns></returns>
        public Boolean CollectFullData(String WebPage)
        {
            Boolean Success = false;

            try
            {
                GetWebSiteData getSiteData = new GetWebSiteData();


                if (!getSiteData.Parse(WebPage))
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

                    string DataString = getSiteData.Data;
                    //string Search = @"name"":""(?<name>\w+)"".*?""class"":(?<class>\d+).*?""race"":(?<race>\d+).*?""level"":(?<level>\d+).*?""achievementPoints"":(?<achs>\d+).*?""averageItemLevel"":(?<avg_iLevel>\d+).*?averageItemLevelEquipped"":(?<equip_iLevel>\d+),(?<items>.*)}.*?primary"":.*?name"":""(?<Profession1>\w+).*?name"":""(?<Profession2>\w+).*?""selected"":true,.*?(""spec"":{""name"":""(?<Spec>[a-zA-Z0-9 ]+)"".*?,""role"":""(?<Role>\w+)"")?";
                    string Search = @"name"":""(?<name>\w+)"".*?""class"":(?<class>\d+).*?""race"":(?<race>\d+).*?""level"":(?<level>\d+).*?""achievementPoints"":(?<achs>\d+).*?""averageItemLevel"":(?<avg_iLevel>\d+).*?averageItemLevelEquipped"":(?<equip_iLevel>\d+),(?<items>.*)}.*?primary"":.*?name"":""(?<Profession1>\w+).*?name"":""(?<Profession2>\w+).*?""selected"":true,(?<Talents>.*)";
                    Regex test = new Regex(Search, RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.CultureInvariant);

                    #region DEBUG
                    /*
                    // this is for debugging only
                    if (Debug)
                    {
                        // Create the output file.
                        using (FileStream fs = File.Create(DebugFile)) { }
                        // Open the stream and write to it
                        using (FileStream fs = File.OpenWrite(DebugFile))
                        {
                            Byte[] info = new UTF8Encoding(true).GetBytes("   ");
                            // Add some information to the file.
                            fs.Write(info, 0, info.Length);
                            info = new UTF8Encoding(true).GetBytes(Search);
                            fs.Write(info, 0, info.Length);
                            info = new UTF8Encoding(true).GetBytes(DataString);
                            fs.Write(info, 0, info.Length);
                        }
                    }
                     * */
                    #endregion

                    // Get the Average iLevel of character's Gear
                    foreach (Match result in test.Matches(DataString))
                    {
                        // last modified not needed?

                        if (result.Groups["name"].Success)
                        {
                            Guildie.Name = result.Groups["name"].Value.ToString();
                        }

                        if (result.Groups["class"].Success)
                        {
                            Guildie.Class = Converter.ConvertClass(result.Groups["class"].Value.ToString());
                        }

                        if (result.Groups["race"].Success)
                        {
                            Guildie.Race = Converter.ConvertRace(result.Groups["race"].Value.ToString());
                        }

                        if (result.Groups["level"].Success)
                        {
                            Guildie.Level = Convert.ToInt32(result.Groups["level"].Value);
                        }

                        if (result.Groups["achs"].Success)
                        {
                            Guildie.AchievementPoints = Convert.ToInt32(result.Groups["achs"].Value.ToString());
                        }

                        if (result.Groups["avg_iLevel"].Success)
                        {
                            Guildie.MaxiLevel = Convert.ToInt32(result.Groups["avg_iLevel"].Value.ToString());
                        }

                        if (result.Groups["equip_iLevel"].Success)
                        {
                            Guildie.EquipediLevel = Convert.ToInt32(result.Groups["equip_iLevel"].Value.ToString());
                        }

                        if (result.Groups["Profession1"].Success)
                        {
                            Guildie.Profession1 = result.Groups["Profession1"].Value;
                        }

                        if (result.Groups["Profession2"].Success)
                        {
                            Guildie.Profession2 = result.Groups["Profession2"].Value;
                        }

                        if (result.Groups["Talents"].Success)
                        {
                            string Search2 = @".*?""spec"":{""name"":""(?<Spec>[a-zA-Z0-9 ]+)"".*?,""role"":""(?<Role>\w+)"".*";
                            Regex test2 = new Regex(Search2, RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.CultureInvariant);

                            foreach (Match result2 in test2.Matches(result.Groups["Talents"].Value))
                            {
                                if (result2.Groups["Spec"].Success)
                                {
                                    Guildie.Spec = result2.Groups["Spec"].Value;
                                }

                                if (result2.Groups["Role"].Success)
                                {
                                    Guildie.Role = result2.Groups["Role"].Value;
                                }
                            }
                        }

                        if (result.Groups["items"].Success)
                        {
                            Guildie.ItemAudits = ParseItems(result.Groups["items"].Value);
                        }
                    }

                    Success = true;
                }
            }
            catch (Exception ex)
            {
                String Error = String.Format("ERROR: ", ex.Message);
            }

            return Success;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public Dictionary<String, ItemAudit> ParseItems(string data)
        {
            Dictionary<string, ItemAudit> items = FillOutItemAuditsArrayList();
            String Search = @"(?<Slot>\w+)"":{""id"":(?<Id>\d+).*?""name"":""(?<Name>[A-Za-z ',:-]+?)"",.*?""quality"":(?<Quality>\d+).*?,""itemLevel"":(?<ItemLevel>\d+).*?""tooltipParams"":{(?<ToolTips>.*?)}.*?}";
            Regex test = new Regex(Search, RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.CultureInvariant);

            #region DEBUG
            // this is for debugging only
            if (Debug)
            {
                // Create the output file.
                using (FileStream fs = File.Create(DebugFile)) { }
                // Open the stream and write to it
                using (FileStream fs = File.OpenWrite(DebugFile))
                {
                    Byte[] info = new UTF8Encoding(true).GetBytes("   ");
                    // Add some information to the file.
                    fs.Write(info, 0, info.Length);
                    info = new UTF8Encoding(true).GetBytes(Search);
                    fs.Write(info, 0, info.Length);
                    info = new UTF8Encoding(true).GetBytes("\n");
                    fs.Write(info, 0, info.Length);
                    info = new UTF8Encoding(true).GetBytes(data);
                    fs.Write(info, 0, info.Length);
                }
            }
            #endregion

            // Get the item info
            foreach (Match result in test.Matches(data))
            {
                // First get the slot, if no slot then it's an missing item
                if (result.Groups["Slot"].Success)
                {
                    String slot = result.Groups["Slot"].Value.ToString();
                    ItemAudit audit = new ItemAudit();
                    ItemInfo item = new ItemInfo();

                    if (!items.ContainsKey(slot))
                    {
                        // bad!
                        String Error = String.Format("ERROR: ", "GetCharacterInfo.ParseItems() - slot missing");
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
                        if (!WoWGuildOrganizer.Form1.Items.Contains(audit.Id))
                        {
                            // Need to add in this item to the Item Cache

                            // First fetch the data
                            GetItemInfo get = new GetItemInfo();
                            if (get.CollectData(audit.Id))
                            {
                                item = get.Item;
                                WoWGuildOrganizer.Form1.Items.AddItem(item);
                            }
                        }
                        else
                        {
                            item = (ItemInfo)WoWGuildOrganizer.Form1.Items.GetItem(audit.Id);
                        }

                        if (item.Id != 0)
                        {
                            // Set CanEnchant and CanSocket values
                            audit.CanEnchant(item.CanEnchant);
                            audit.CanSocket(item.CanSocket);
                            audit.SocketCount(item.SocketCount);
                            audit.MissingItem = "0";
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
        /// 
        /// </summary>
        private Dictionary<string, ItemAudit> FillOutItemAuditsArrayList()
        {
            Dictionary<String, ItemAudit> ItemAudits = new Dictionary<string,ItemAudit>();

            // fill out a blank array with all the slots
            for (Int32 i = 0; i < 18; i++)
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

                ItemAudits.Add(audit.Slot, audit);
            }

            return ItemAudits;
        }
    }
}
