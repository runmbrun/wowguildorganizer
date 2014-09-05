using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Collections;
using System.Text.RegularExpressions;
using System.IO;



namespace WoWGuildOrganizer
{
    class GetGuildInfo
    {
        Boolean Debug = false;
        String DebugFile = @"c:\temp\GetGuildInfo.log";

        private ArrayList _characters;
        public ArrayList Characters
        {
            get { return _characters; }
            set { _characters = value; }
        }

        public GetGuildInfo()
        {
            Characters = new ArrayList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="WebPage"></param>
        /// <returns></returns>
        public virtual Boolean CollectData(String WebPage)
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
                    {
                      "character":{
                        ***"name":"Huul", 
                        "realm":"Thrall",
                        ***"class":6,
                        ***"race":6,
                        "gender":0,
                        ***"level":67,
                        ***"achievementPoints":0,
                        "thumbnail":"thrall/110/53219950-avatar.jpg"
                      },
                      "rank":5
                    },              
                    */


                    String DataString = getSiteData.Data;
                    String Search = @"character"":{.*?name"":""(?<Name>\w+).*?class"":(?<Class>\d+).*?race"":(?<Race>\d+).*?level"":(?<Level>\d+).*?achievementPoints"":(?<AchPts>\d+).*?}";
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

                    foreach (Match result in test.Matches(DataString))
                    {
                        GuildMember member = new GuildMember();

                        if (result.Groups["Name"].Success)
                        {
                            member.Name = result.Groups["Name"].Value;
                        }

                        if (result.Groups["Race"].Success)
                        {
                            member.Race = Converter.ConvertRace(result.Groups["Race"].Value);
                        }

                        if (result.Groups["Class"].Success)
                        {
                            member.Class = Converter.ConvertClass(result.Groups["Class"].Value);
                        }

                        if (result.Groups["Level"].Success)
                        {
                            member.Level = Convert.ToInt32(result.Groups["Level"].Value);
                        }

                        if (result.Groups["AchPts"].Success)
                        {
                            member.AchievementPoints = Convert.ToInt32(result.Groups["AchPts"].Value);
                        }

                        member.EquipediLevel = 0;
                        member.MaxiLevel = 0;

                        Characters.Add(member);
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
