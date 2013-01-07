using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;



namespace WoWGuildOrganizer
{
    class GetCharacterItemInfo
    {
        Boolean Debug = false;
        String DebugFile = @"c:\temp\GetCharacterItemInfo.log";

        public Dictionary<String, ItemAudit> ItemAudits;

        public GetCharacterItemInfo()
        {
            ItemAudits = new Dictionary<String, ItemAudit>();
            FillOutItemAuditsArrayList();
        }

        private void FillOutItemAuditsArrayList()
        {
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

                    // clear out the first part...
                    Int32 count = DataString.IndexOf(@"""items"":{");
                    if (count != -1)
                    {
                        DataString = DataString.Substring(count + 9);
                    }

                    String Search = @"""(?<Slot>\w+)"":{""id"":(?<Id>\d+).*?""name"":""(?<Name>[A-Za-z ',:-]+?)"",.*?""quality"":(?<Quality>\d+).*?,""itemLevel"":(?<ItemLevel>\d+).*?""tooltipParams"":{(?<ToolTips>.*?)}.*?}";
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

                    
                    // Get the item info
                    foreach (Match result in test.Matches(DataString))
                    {
                        Int32 Id = 0;
                        ItemInfo item = new ItemInfo();


                        // First get the slot, if no slot then it's an missing item
                        if (result.Groups["Slot"].Success)
                        {
                            String slot = result.Groups["Slot"].Value.ToString();
                            ItemAudit audit = new ItemAudit();


                            if (!ItemAudits.ContainsKey(slot))
                            {
                                // bad!
                                String Error = String.Format("ERROR: ", "GetCharacterInfo.CollectData() - slot missing");
                            }
                            else
                            {
                                audit = ItemAudits[slot];
                                                                                                                              
                                // First get the item id
                                if (result.Groups["Id"].Success)
                                {
                                    audit.SetId(Convert.ToInt32(result.Groups["Id"].Value));
                                    Id = audit.GetId();
                                }

                                // Now check for the item in the item cache
                                if (!WoWGuildOrganizer.Form1.Items.Contains(Id))
                                {
                                    // Need to add in this item to the Item Cache

                                    // First fetch the data
                                    GetItemInfo get = new GetItemInfo();
                                    if (get.CollectData(Id))
                                    {
                                        item = get.Item;
                                        item.Id = Id;
                                        WoWGuildOrganizer.Form1.Items.AddItem(item);
                                    }
                                }
                                else
                                {
                                    item = (ItemInfo)WoWGuildOrganizer.Form1.Items.GetItem(Id);
                                }

                                if (Id != 0)
                                {
                                    // Set CanEnchant and CanSocket values
                                    audit.CanEnchant(item.CanEnchant);
                                    audit.CanSocket(item.CanSocket);
                                    audit.SocketCount(item.SocketCount);
                                    audit.MissingItem = "0";
                                }

                                if (result.Groups["Id"].Success)
                                {
                                    audit.SetId(Convert.ToInt32(result.Groups["Id"].Value));
                                }

                                if (result.Groups["Name"].Success)
                                {
                                    audit.Name = result.Groups["Name"].Value.ToString();
                                }

                                if (result.Groups["Quality"].Success)
                                {
                                    audit.SetQuality(result.Groups["Quality"].Value.ToString());
                                }

                                if (result.Groups["ItemLevel"].Success)
                                {
                                    //audit.SetItemLevel(result.Groups["ItemLevel"].Value.ToString());
                                    audit.ItemLevel = Convert.ToInt32(result.Groups["ItemLevel"].Value);
                                }

                                if (result.Groups["ToolTips"].Success)
                                {
                                    audit.SetToolTips(result.Groups["ToolTips"].Value.ToString());
                                }

                                // mmb - is this needed any more?  the new character info has the exact item level with each item
                                //  probably because it is needed with the upgrades...
                                //audit.ItemLevel = item.ItemLevel;
                            }
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
    }
}
