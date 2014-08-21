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

        Boolean Debug = false;
        String DebugFile = @"c:\temp\GetWebSiteData.log";

        #endregion


        /// <summary>
        /// 
        /// </summary>
        /// <param name="WebPage"></param>
        /// <returns></returns>
        public virtual Boolean Parse(String WebPage)
        {
            Boolean Success = WoWGuildOrganizer.FormMain.WebSiteOnline;

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

                    String strResponse = readStream.ReadToEnd();

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
                            info = new UTF8Encoding(true).GetBytes(strResponse);
                            fs.Write(info, 0, info.Length);
                        }
                    }
                    #endregion

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

                        Logging.DisplayError("Battle.net Web Site cannot be reached.  No further attempts will be made.  Restart application to try again.");
                    }
                }
                catch (Exception ex)
                {
                    WoWGuildOrganizer.FormMain.WebSiteOnline = Success = false;

                    // collect this error for use in the function that is calling this function
                    Logging.Log(String.Format("ERROR: {0}", ex.Message));


                }
            }

            return Success;
        }
    }
}
