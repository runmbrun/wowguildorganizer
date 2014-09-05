using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;



namespace WoWGuildOrganizer
{
    class GetWebSiteData
    {
        ///This is where all the variables are kept
        #region " Variables "

        private String _website;
        public String WebSite
        {
            get { return _website; }
            set { _website = value; }
        }

        private String _data;
        public String Data
        {
            get { return _data; }
            set { _data = value; }
        }

        Boolean debug = false;
        String debugFile = @"c:\temp\GetWebSiteData.log";

        #endregion


        /// <summary>
        /// 
        /// </summary>
        /// <param name="WebPage"></param>
        /// <returns></returns>
        public virtual Boolean Parse(String WebPage)
        {
            bool Success = WoWGuildOrganizer.FormMain.WebSiteOnline;
            string strResponse = string.Empty;

            if (Success)
            {
                try
                {
                    IWebProxy iwp = WebRequest.GetSystemWebProxy();
                    WebProxy wp = new WebProxy();
                    wp.UseDefaultCredentials = true;

                    // Create a 'WebRequest' object with the specified url.
                    WebRequest myWebRequest = WebRequest.Create(WebPage);
                    myWebRequest.Proxy = wp;

                    // Send the 'WebRequest' and wait for response.
                    WebResponse myWebResponse = myWebRequest.GetResponse();

                    // Obtain a 'Stream' object associated with the response object.
                    Stream ReceiveStream = myWebResponse.GetResponseStream();

                    Encoding encode = System.Text.Encoding.GetEncoding("utf-8");

                    // Pipe the stream to a higher level stream reader with the required encoding format.
                    StreamReader readStream = new StreamReader(ReceiveStream, encode);

                    strResponse = readStream.ReadToEnd();

                    // Debugging
                    this.Debug(strResponse);

                    // close the reading stream
                    readStream.Close();

                    // Release the resources of response object.
                    myWebResponse.Close();

                    // now parse the web page to get just the data we need
                    Data = strResponse;

                    // Check for web site blocking...
                    if (Data.IndexOf("Uncategorized Web Site") != -1)
                    {
                        // Web site is block... 
                        WoWGuildOrganizer.FormMain.WebSiteOnline = Success = false;

                        // Log it
                        string message = string.Format("Battle.net Web Site cannot be reached.  No further attempts will be made.  Restart application to try again.");

                        Logging.Log(message);
                        Logging.DisplayError(message);
                    }
                }
                catch (WebException ex)
                {
                    Success = false;
                    
                    // This is a 404 error, usually because a character hasn't been logged into for a while
                    //   collect this error for use in the function that is calling this function
                    Logging.Log(String.Format("ERROR: {0}", ex.Message));
                    
                    // Check for Debugging
                    this.Debug(strResponse);
                }
                catch (Exception ex)
                {
                    WoWGuildOrganizer.FormMain.WebSiteOnline = Success = false;

                    // collect this error for use in the function that is calling this function
                    Logging.Log(String.Format("ERROR: {0}", ex.Message));

                    // Check for Debugging
                    this.Debug(strResponse);
                }
            }

            return Success;
        }
        
        #region DEBUG

        /// <summary>
        /// Debug what was pulled back from the web site
        /// </summary>
        /// <param name="message"></param>
        void Debug(string message)
        {            
            // this is for debugging only
            if (debug && message != string.Empty)
            {
                // Create the output file.
                using (FileStream fs = File.Create(debugFile)) { }

                // Open the stream and write to it
                using (FileStream fs = File.OpenWrite(debugFile))
                {
                    Byte[] info = new UTF8Encoding(true).GetBytes("   ");

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
