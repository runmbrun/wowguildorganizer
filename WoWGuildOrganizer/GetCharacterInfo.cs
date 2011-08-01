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

        public GetCharacterInfo()
        {            
            MaxiLevel = 0;
            EquipediLevel = 0;
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
                    String Search = @"averageItemLevel"":(?<avg_iLevel>\d+).*?averageItemLevelEquipped"":(?<equip_iLevel>\d+)";
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
