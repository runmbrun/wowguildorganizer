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

        private Int32 _maxilevel;
        public Int32 MaxiLevel
        {
            set { _maxilevel = value; }
            get { return _maxilevel; }
        }

        private Int32 _equipedilevel;
        public Int32 EquipediLevel
        {
            set { _equipedilevel = value; }
            get { return _equipedilevel; }
        }

        private String _profession1;
        public String Profession1
        {
            set { _profession1 = value; }
            get { return _profession1; }
        }

        private String _profession2;
        public String Profession2
        {
            set { _profession2 = value; }
            get { return _profession2; }
        }

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
            MaxiLevel = 0;
            EquipediLevel = 0;
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
                    //String Search = @"averageItemLevel"":(?<avg_iLevel>\d+).*?averageItemLevelEquipped"":(?<equip_iLevel>\d+)";
                    String Search = @"averageItemLevel"":(?<avg_iLevel>\d+).*?averageItemLevelEquipped"":(?<equip_iLevel>\d+),.*?primary"":.*?name"":""(?<Profession1>\w+).*?name"":""(?<Profession2>\w+)";
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
                            MaxiLevel = Convert.ToInt32(result.Groups["avg_iLevel"].Value.ToString());
                        }

                        if (result.Groups["equip_iLevel"].Success)
                        {
                            EquipediLevel = Convert.ToInt32(result.Groups["equip_iLevel"].Value.ToString());
                        }

                        if (result.Groups["Profession1"].Success)
                        {
                            Profession1 = result.Groups["Profession1"].Value;
                        }

                        if (result.Groups["Profession2"].Success)
                        {
                            Profession2 = result.Groups["Profession2"].Value;
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
                    String Search = @"""name"":""(?<name>\w+)"".*?""class"":(?<class>\d+).*?""race"":(?<race>\d+).*?""level"":(?<level>\d+).*?""achievementPoints"":(?<achs>\d+).*?""averageItemLevel"":(?<avg_iLevel>\d+).*?averageItemLevelEquipped"":(?<equip_iLevel>\d+),.*?primary"":.*?name"":""(?<Profession1>\w+).*?name"":""(?<Profession2>\w+)";
                    //"name":"(?<name>\w+)".*?"class":(?<class>\d+).*?"race":(?<race>\d+).*?"level":(?<level>\d+).*?"achievementPoints":(?<achs>\d+).*?"averageItemLevel":(?<avg_iLevel>\d+).*?averageItemLevelEquipped":(?<equip_iLevel>\d+)
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

                        // realm not needed?

                        if (result.Groups["class"].Success)
                        {
                            Guildie.Class = Converter.ConvertClass(result.Groups["class"].Value.ToString());
                        }

                        if (result.Groups["race"].Success)
                        {
                            Guildie.Race = Converter.ConvertRace(result.Groups["race"].Value.ToString());
                        }

                        // gender is not needed

                        if (result.Groups["level"].Success)
                        {
                            Guildie.Level = Convert.ToInt32(result.Groups["level"].Value);
                        }

                        if (result.Groups["achs"].Success)
                        {
                            Guildie.AchievementPoints = Convert.ToInt32(result.Groups["achs"].Value.ToString());
                        }

                        //...

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
                            Guildie.SetProfession1(result.Groups["Profession1"].Value);
                        }

                        if (result.Groups["Profession2"].Success)
                        {
                            Guildie.SetProfession2(result.Groups["Profession2"].Value);
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
