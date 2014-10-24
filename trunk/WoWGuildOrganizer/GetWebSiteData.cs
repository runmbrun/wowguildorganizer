// <copyright file="GetWebSiteData.cs" company="Secondnorth.com">
//     Secondnorth.com. All rights reserved.
// </copyright>
// <author>Me</author>

namespace WoWGuildOrganizer
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    
    /// <summary>
    /// Super class to make parsing web sites more consistent
    /// </summary>
    public class GetWebSiteData
    {        
        #region Class Variables
                
        /// <summary>
        /// Flag to determine if debug logging should be turned on or off
        /// </summary>
        private bool debug = false;

        /// <summary>
        /// String of a temp filename for the debug logging
        /// </summary>
        private string debugFile = @"c:\temp\GetWebSiteData.log";

        /// <summary>
        /// Gets or sets string of the website to collect the data from
        /// </summary>
        public string WebSite { get; set; }

        /// <summary>
        /// Gets or sets string of the actual data from the website
        /// </summary>
        public string Data { get; set; }

        #endregion

        #region Parsing

        /// <summary>
        /// Parses the passed in webpage and collects the response data
        /// </summary>
        /// <param name="webPage">url of the web site to be parsed</param>
        /// <returns>true if parse was successful</returns>
        public virtual bool Parse(string webPage)
        {
            bool success = WoWGuildOrganizer.FormMain.WebSiteOnline;
            string response = string.Empty;

            if (success)
            {
                try
                {
                    IWebProxy iwp = WebRequest.GetSystemWebProxy();
                    WebProxy wp = new WebProxy();
                    wp.UseDefaultCredentials = true;

                    // Create a 'WebRequest' object with the specified url.
                    WebRequest myWebRequest = WebRequest.Create(webPage);
                    myWebRequest.Proxy = wp;

                    // Send the 'WebRequest' and wait for response.
                    WebResponse myWebResponse = myWebRequest.GetResponse();

                    // Obtain a 'Stream' object associated with the response object.
                    Stream receiveStream = myWebResponse.GetResponseStream();

                    Encoding encode = System.Text.Encoding.GetEncoding("utf-8");

                    // Pipe the stream to a higher level stream reader with the required encoding format.
                    StreamReader readStream = new StreamReader(receiveStream, encode);

                    response = readStream.ReadToEnd();

                    // Debugging
                    this.Debug(response);

                    // close the reading stream
                    readStream.Close();

                    // Release the resources of response object.
                    myWebResponse.Close();

                    // now parse the web page to get just the data we need
                    this.Data = response;

                    // Check for web site blocking...
                    if (this.Data.IndexOf("Uncategorized Web Site") != -1)
                    {
                        // Web site is block... 
                        WoWGuildOrganizer.FormMain.WebSiteOnline = success = false;

                        // Log it
                        string message = string.Format("Battle.net Web Site cannot be reached.  No further attempts will be made.  Restart application to try again.");

                        Logging.Log(message);
                        Logging.DisplayError(message);
                    }
                }
                catch (WebException ex)
                {
                    success = false;
                    
                    // This is a 404 error, usually because a character hasn't been logged into for a while
                    //   collect this error for use in the function that is calling this function
                    Logging.Log(string.Format("ERROR: {0}", ex.Message));
                    
                    // Check for Debugging
                    this.Debug(response);
                }
                catch (Exception ex)
                {
                    WoWGuildOrganizer.FormMain.WebSiteOnline = success = false;

                    // collect this error for use in the function that is calling this function
                    Logging.Log(string.Format("ERROR: {0}", ex.Message));

                    // Check for Debugging
                    this.Debug(response);
                }
            }

            return success;
        }
        
        #endregion

        #region Debugging

        /// <summary>
        /// Debug what was pulled back from the web site
        /// </summary>
        /// <param name="message">message to be logged</param>
        private void Debug(string message)
        {            
            // this is for debugging only
            if (this.debug && message != string.Empty)
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
                    info = new UTF8Encoding(true).GetBytes(message);
                    fs.Write(info, 0, info.Length);
                }
            }
        }

        #endregion
    }
}
