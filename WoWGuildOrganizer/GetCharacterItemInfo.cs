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

        public ArrayList ItemAudits;

        public GetCharacterItemInfo()
        {
            ItemAudits = new ArrayList();
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

                    String Search = @"""(?<Slot>\w+)"":{.*?""id"":(?<Id>\d+).*?""name"":""(?<Name>[A-Za-z ]+).*?""quality"":(?<Quality>\d+).*?""tooltipParams"":{(?<ToolTips>.*?)}.*?}";
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
                        ItemAudit audit = new ItemAudit();
                        Int32 Id = 0;
                        ItemInfo item = new ItemInfo();


                        // First get the item id
                        if (result.Groups["Id"].Success)
                        {
                            audit.Id = result.Groups["Id"].Value.ToString();
                            Id = Convert.ToInt32(audit.Id);
                        }

                        // Now check for the item in the item cache
                        if (!WoWGuildOrganizer.Form1.Items.Items.Contains(Id))
                        {
                            // Need to add in this item to the Item Cache

                            // First fetch the data
                            GetItemInfo get = new GetItemInfo();
                            if (get.CollectData(Id))
                            {
                                item = get.Item;
                                WoWGuildOrganizer.Form1.Items.Items.Add(item);
                            }                            
                        }
                        else
                        {
                            item = (ItemInfo)WoWGuildOrganizer.Form1.Items.Items[Id];
                        }
                        
                        if (Id != 0)
                        {
                            // Set CanEnchant and CanSocket values
                            audit.CanEnchant(item.CanEnchant);
                            audit.CanSocket(item.CanSocket);
                            audit.SocketCount(item.SocketCount);
                        }

                        if (result.Groups["Slot"].Success)
                        {
                            audit.Slot = result.Groups["Slot"].Value.ToString();
                        }

                        if (result.Groups["Id"].Success)
                        {
                            audit.Id = result.Groups["Id"].Value.ToString();
                        }

                        if (result.Groups["Name"].Success)
                        {
                            audit.Name = result.Groups["Name"].Value.ToString();
                        }

                        if (result.Groups["Quality"].Success)
                        {
                            audit.SetQuality(result.Groups["Quality"].Value.ToString());
                        }

                        if (result.Groups["ToolTips"].Success)
                        {
                            audit.SetToolTips(result.Groups["ToolTips"].Value.ToString());
                        }

                        //TODO - need to check this item from the ItemCache, if not there, need to fetch it
                        if (result.Groups["ItemLevel"].Success)
                        {
                            audit.ItemLevel = Convert.ToInt32(result.Groups["ItemLevel"].Value.ToString());
                        }

                        ItemAudits.Add(audit);
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
