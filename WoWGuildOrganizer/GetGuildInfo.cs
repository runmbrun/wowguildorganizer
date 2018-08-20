// <copyright file="GetGuildInfo.cs" company="Secondnorth.com">
//     Secondnorth.com. All rights reserved.
// </copyright>
// <author>Me</author>

namespace WoWGuildOrganizer
{
    using Newtonsoft.Json;
    using System;
    using System.Collections;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Text.RegularExpressions;


    /// <summary>
    /// Class to get all the guild information
    /// </summary>
    public class GetGuildInfo
    {
        #region Class Variables

        /// <summary>
        /// Flag that determines if debug logging is turned on or off
        /// </summary>
        private bool debug = false;

        /// <summary>
        /// String containing the location of a debugging log
        /// </summary>
        private string debugFile = @"c:\temp\GetGuildInfo.log";

        #endregion

        #region Class Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="GetGuildInfo"/> class
        /// </summary>
        public GetGuildInfo()
        {
            this.Characters = new ArrayList();
        }

        #endregion

        #region Class Properties

        /// <summary>
        /// Gets or sets an array list of characters in the guild
        /// </summary>
        public ArrayList Characters { get; set; }

        #endregion

        #region " Collect Data "
        /// <summary>
        /// Collects all the data from a web page
        /// ***DEPRECIATED***
        /// </summary>
        /// <param name="webPage">url of the web page that contains the guild information</param>
        /// <returns>true if data is collected successfully</returns>
        public virtual bool CollectData(string webPage)
        {
            bool success = false;
            string dataString = string.Empty;
            string search = string.Empty;

            try
            {
                GetWebSiteData getSiteData = new GetWebSiteData();                
                
                if (getSiteData.Parse(webPage))
                {
                    // now parse the data

                    // piece in web site that is important
                    // code to expresso => replace "" with "
                    // expresso to code => replace " with ""

                    /* Example:
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

                    dataString = getSiteData.Data;
                    search = @"character"":{.*?name"":""(?<Name>\w+).*?class"":(?<Class>\d+).*?race"":(?<Race>\d+).*?level"":(?<Level>\d+).*?achievementPoints"":(?<AchPts>\d+).*?}";
                    Regex test = new Regex(search, RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.CultureInvariant);

                    foreach (Match result in test.Matches(dataString))
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

                        this.Characters.Add(member);
                    }

                    success = true;
                }
            }
            catch (Exception ex)
            {
                // Log the error
                Logging.Log(string.Format("ERROR: {0} in CollectData() in GetGuildInfo.cs", ex.Message));

                // Check for Debugging
                this.Debug(dataString, search);
            }

            return success;
        }
        #endregion

        #region " Collect JSON Data "
        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestUrl"></param>
        /// <returns></returns>
        public virtual bool GetJSONData(string guild, string realm)
        {
            bool success = false;

            try
            {
                GetWebJSONData getData = new GetWebJSONData();

                JSONGuildData data = getData.GetGuildJSONData(guild, realm);

                if (data != null)
                {
                    if (data.Members.Count > 0)
                    {
                        foreach (JSONGuildCharacterData guildie in data.Members)
                        {
                            // now save all the data into the format we are expecting
                            // TODO: should it stay this way?  Or make a new format?
                            GuildMember temp = new GuildMember();

                            temp.Name = guildie.Character.Name;
                            temp.Race = Converter.ConvertRace(guildie.Character.Race);
                            temp.Class = Converter.ConvertClass(guildie.Character.Class);
                            temp.Level = guildie.Character.Level;
                            temp.AchievementPoints = guildie.Character.AchievementPoints;
                            temp.EquipediLevel = 0;
                            temp.MaxiLevel = 0;

                            this.Characters.Add(temp);
                        }

                        // No errors up to this point?  Success!
                        success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.Log($"ERROR: {ex.Message} in CollectJSONData() in GetGuildInfo.cs");
                Logging.DisplayError($"ERROR: {ex.Message} in CollectJSONData() in GetGuildInfo.cs");
                success = false;
            }

            return success;
        }
        #endregion

            #region Debugging

            /// <summary>
            /// Check if debug logging is needed
            /// </summary>
            /// <param name="dataString">full data string of the web site</param>
            /// <param name="search">regex of what will be searched on in the web site</param>
        private void Debug(string dataString, string search)
        {
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
