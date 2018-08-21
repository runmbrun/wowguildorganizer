// <copyright file="GetWebJSONData.cs" company="Secondnorth.com">
//     Secondnorth.com. All rights reserved.
// </copyright>
// <author>Me</author>

namespace WoWGuildOrganizer
{    
    using System;
    using System.IO;
    using System.Net;
    using System.Text;
    using Newtonsoft.Json;

    /// <summary>
    /// Documentation for the Blizzard API: "https://dev.battle.net/io-docs"
    /// </summary>
    class GetWebJSONData
    {
        /// <summary>
        /// static string of the URL for the Blizzard API
        /// </summary>
        public readonly string URLWowAPI = @"https://us.api.battle.net/wow/";

        /// <summary>
        /// static string of the api key needed to use the Blizzard REST API
        /// </summary>
        //public readonly string APIKey = "gnv4yu6dzc25ywu3g98pfkhg26p8typn";
        public readonly string APIKey = "jwhk8mw8kfpcng2y86as895gufku9kfa";

        /// <summary>
        /// Example: https://us.api.battle.net/wow/guild/Thrall/Secondnorth?fields=members&locale=en_US&apikey=jwhk8mw8kfpcng2y86as895gufku9kfa
        /// </summary>
        /// <param name="guild"></param>
        /// /// <param name="realm"></param>
        /// <returns></returns>
        public JSONGuildData GetGuildJSONData(string guild, string realm)
        {
            JSONGuildData result = null;
            string requestUrl = $"{this.URLWowAPI}guild/{realm}/{guild}?fields=members&locale=en_US&apikey={this.APIKey}";
            string responseData = GetJSONData(requestUrl);

            // Convert the data to the proper object
            if (!string.IsNullOrEmpty(responseData))
            {
                result = JsonConvert.DeserializeObject<JSONGuildData>(responseData);
            }

            return result;
        }

        /// <summary>
        /// Example: https://us.api.battle.net/wow/character/Thrall/Purdee?fields=items%2Cprofessions%2Ctalents&locale=en_US&apikey=jwhk8mw8kfpcng2y86as895gufku9kfa                
        /// </summary>
        /// <param name="requestUrl"></param>
        /// <returns></returns>
        public JSONCharacterData GetCharacterJSONData(string characterName, string realm)
        {
            JSONCharacterData result = null;
            string requestUrl = $"{this.URLWowAPI}character/{realm}/{characterName}?fields=items,professions,talents&apikey={this.APIKey}";
            string responseData = GetJSONData(requestUrl);            

            // Convert the data to the proper object
            if (!string.IsNullOrEmpty(responseData))
            {
                result = JsonConvert.DeserializeObject<JSONCharacterData>(responseData);
            }

            return result;
        }

        /// <summary>
        /// Example: https://us.api.battle.net/wow/item/139191?locale=en_US&apikey=jwhk8mw8kfpcng2y86as895gufku9kfa
        /// Example with context: https://us.api.battle.net/wow/item/139191/raid-normal?locale=en_US&apikey=jwhk8mw8kfpcng2y86as895gufku9kfa
        /// </summary>
        /// <param name="id"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public JSONItemData GetItemJSONData(int id, string context)
        {
            JSONItemData result = null;
            string requestUrl = string.Empty;

            if (string.IsNullOrEmpty(context))
            {
                // this is NOT context driven
                requestUrl = $"{this.URLWowAPI}item/{id}?locale=en_US&apikey={this.APIKey}";
            }
            else
            {
                // this is context driven
                requestUrl = $"{this.URLWowAPI}item/{id}/{context}?locale=en_US&apikey={this.APIKey}";
            }
            
            string responseData = GetJSONData(requestUrl);

            // Convert the data to the proper object
            if (!string.IsNullOrEmpty(responseData))
            {
                result = JsonConvert.DeserializeObject<JSONItemData>(responseData);
            }

            return result;
        }

        /// <summary>
        /// Example: https://us.api.battle.net/wow/zone/?locale=en_US&apikey=jwhk8mw8kfpcng2y86as895gufku9kfa
        /// </summary>
        /// <returns></returns>
        public JSONRaidData GetRaidJSONData()
        {
            JSONRaidData result = null;
            string responseData = GetJSONData($"{this.URLWowAPI}zone/?locale=en_US&apikey={this.APIKey}");

            // Convert the data to the proper object
            if (!string.IsNullOrEmpty(responseData))
            {
                result = JsonConvert.DeserializeObject<JSONRaidData>(responseData);
            }

            return result;
        }

        /// <summary>
        /// Requests data from a RESTful site.  Response is in string format and will need to be converted to class format.
        /// </summary>
        /// <param name="requestUrl"></param>
        /// <returns></returns>
        private string GetJSONData(string requestUrl)
        {
            string result = null;

            if (WoWGuildOrganizer.FormMain.WebSiteOnline)
            {
                try
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    HttpWebRequest request = WebRequest.Create(requestUrl) as HttpWebRequest;

                    using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                    {
                        if (response.StatusCode != HttpStatusCode.OK)
                        {
                            throw new Exception($"Server error (HTTP {response.StatusCode}: {response.StatusDescription}).");
                        }

                        Stream receiveStream = response.GetResponseStream();
                        Encoding encode = System.Text.Encoding.GetEncoding("utf-8");

                        // Pipes the stream to a higher level stream reader with the required encoding format. 
                        using (StreamReader readStream = new StreamReader(receiveStream, encode))
                        {
                            Char[] read = new Char[256];

                            // Reads 256 characters at a time.    
                            int count = readStream.Read(read, 0, 256);

                            while (count > 0)
                            {
                                // Dumps the 256 characters on a string and displays the string to the console.
                                String str = new String(read, 0, count);
                                result += str;
                                count = readStream.Read(read, 0, 256);
                            }
                        }
                    }
                }
                catch (WebException ex)
                {
                    string errorCheck = string.Empty;

                    if (ex.Response != null)
                    {
                        using (WebResponse response = ex.Response)
                        {
                            HttpWebResponse httpResponse = (HttpWebResponse)response;
                            Console.WriteLine("Error code: {0}", httpResponse.StatusCode);

                            using (Stream data = response.GetResponseStream())

                            using (var reader = new StreamReader(data))
                            {
                                errorCheck = reader.ReadToEnd();
                                Console.WriteLine(errorCheck);
                            }
                        }
                    }

                    // Web Exception Errors were found!
                    if (errorCheck.StartsWith(@"{""status"":""nok"", ""reason"": "))
                    {
                        Logging.Debug($"Character could not be found on the Battle.net Site any more... ignoring.");
                    }
                    else if (ex.HResult == -2146233079)
                    {
                        // Battle.Net cannot be reached
                        Logging.DisplayError($"Battle.Net cannot be reached: {ex.Message}");
                        WoWGuildOrganizer.FormMain.WebSiteOnline = false;
                    }
                    else
                    {
                        // This is a 404 error, usually because a character hasn't been logged into for a while
                        //   collect this error for use in the function that is calling this function
                        Logging.Debug($"(404) {ex.Message} in Parse() in GetWebSiteData.cs");
                    }
                }
                catch (Exception ex)
                {
                    Logging.Log($"ERROR: {ex.Message} in GetCharacterJSONData() in GetWebJSONData.cs");
                    Logging.DisplayError($"ERROR: {ex.Message} in GetCharacterJSONData() in GetWebJSONData.cs");
                }
            }

            return result;
        }
    }
}
