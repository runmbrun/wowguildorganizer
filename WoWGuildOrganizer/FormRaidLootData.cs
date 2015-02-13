using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace WoWGuildOrganizer
{
    public partial class FormRaidLootData : Form
    {
        public FormRaidLootData()
        {
            InitializeComponent();
        }

        private void buttonTEST_Click(object sender, EventArgs e)
        {
            try
            {
                // Make sure the input is all digits
                if (Regex.IsMatch(textBoxBossLoot.Text, @"^\d+$"))
                {   
#if DEBUG
                    // DEBUG section - pulling from file instead of web site
                    string fileData = string.Empty;
                    //fileData = File.ReadAllText("blackrock-foundry.txt");
                    fileData = File.ReadAllText("highmaul.txt");

                    if (!string.IsNullOrWhiteSpace(fileData))
#else
                    // now get the data from the web site
                    GetWebSiteData data = new GetWebSiteData();
                    string webSite = string.Format("http://www.wowhead.com/zone={0}", Convert.ToInt32(textBoxBossLoot.Text));                    

                    if (data.Parse(webSite)) // if statement
#endif
                    {
                        // now we have the data, get all the loot 
                        //  but only the loot that has a source                        
#if DEBUG
                        // DEBUG section - pulling from file instead of web site
                        string dataString = fileData;
#else
                        string dataString = data.Data;
#endif
                        int start = dataString.IndexOf("initLootTable");
                        int end = dataString.IndexOf("new Listview", start);

                        dataString = dataString.Substring(start, end - start);

                        string[] stringSeparators = new string[] { "classs" };
                        string[] results;

                        results = dataString.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);

                        Dictionary<string, Dictionary<string, ArrayList>> raidloot = new Dictionary<string, Dictionary<string, ArrayList>>();

                        // loop through all the loot
                        foreach (string item in results)
                        {
                            // skip everything that doesn't have a sourcemore value in it
                            //  it's not loot that can be worn
                            if (item.Contains("sourcemore"))
                            {
                                // now get all the data that contains the string "sourcemore"
                                string searchString = @"classs.*?id"":(?<itemid>\d+).*?level"":(?<itemlevel>\d+).*?name"":""\d(?<name>.*?),(?<source>.*?)armor.*?,";
                                Regex test = new Regex(searchString, RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.CultureInvariant);
                                
                                /*
                                // Database way...
                                DataSet raidloot = new DataSet();
                                //ds.DataSetName = "raidloot";

                                DataTable raid = new DataTable();
                                raid.Columns.Add("id", typeof(System.Int32));
                                raid.Columns.Add("name", typeof(System.String));
                                raid.Columns.Add("difficulty", typeof(System.String));

                                DataTable boss = new DataTable();
                                boss.Columns.Add("id", typeof(System.Int32));
                                boss.Columns.Add("name", typeof(System.String));
                                boss.Columns.Add("raid_id", typeof(System.Int32));

                                DataTable loot = new DataTable();
                                boss.Columns.Add("id", typeof(System.Int32));
                                boss.Columns.Add("boss_id", typeof(System.Int32));
                                */

                                // Get the Average iLevel of character's Gear
                                foreach (Match result in test.Matches(item))
                                {
                                    int itemId = 0;
                                    int itemLevel = 0;
                                    string name = string.Empty;
                                    string source = string.Empty;

                                    if (result.Groups["itemid"].Success)
                                    {
                                        itemId = Convert.ToInt32(result.Groups["itemid"].Value.ToString());
                                    }

                                    if (result.Groups["itemlevel"].Success)
                                    {
                                        itemLevel = Convert.ToInt32(result.Groups["itemlevel"].Value.ToString());
                                    }

                                    if (result.Groups["name"].Success)
                                    {
                                        name = result.Groups["name"].Value.ToString();
                                    }

                                    if (result.Groups["source"].Success)
                                    {
                                        string boss = string.Empty;

                                        source = result.Groups["source"].Value.ToString();

                                        if (source.Contains("sourcemore"))
                                        {
                                            string searchSource = @"sourcemore"":\[\{""\w\w"":\d,""\w"":""(?<boss>.*)"",";
                                            Regex testSource = new Regex(searchSource, RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.CultureInvariant);

                                            // Find out the source of this loot, which boss drops it?
                                            //sourcemore":[{"bd":1,"n":"Gruul","t":1,"ti":76877,"z":6967}],
                                            //start = source.IndexOf(@"sourcemore");
                                            //start = source.IndexOf(@":""", start);
                                            //end = source.IndexOf(@""",", start);
                                            //source = source.Substring(start + 2, end - (start + 2));
                                            foreach (Match resultBoss in testSource.Matches(source))
                                            {
                                                if (resultBoss.Groups["boss"].Success)
                                                {
                                                    boss = resultBoss.Groups["boss"].Value.ToString();
                                                }
                                            }
                                        }
                                    }

                                    // Bonus loot
                                    if (result.Groups["source"].Success)
                                    {
                                        source = result.Groups["source"].Value.ToString();
                                    }

                                    // We have boss loot 
                                    //  Check to see if it exists...
                                    // that we need to write



                                } // end foreach
                            }
                        }

                        //  Direct writing to file
                        // Now we start the xml file
                        XmlTextWriter writer = new XmlTextWriter("test.xml", System.Text.Encoding.UTF8);
                        writer.WriteStartDocument(true);
                        writer.Formatting = Formatting.Indented;
                        writer.Indentation = 2;
                        writer.WriteStartElement("raidloot");

                        // Write Raid Name
                        // todo: from raidloot structure

                        // Write Raid Difficulty
                        // todo:

                        // now close the xml file
                        writer.WriteEndElement();
                        writer.WriteEndDocument();
                        writer.Close();

                        // done!
                        MessageBox.Show("XML File created ! ");
                    }
                    else
                    {
                        // todo: Logging.Error(string.Format("FormRaidLootData: Website [{0}] can't be parsed.", webSite));
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.Error(string.Format("FormRaidLootData: Exception - {0}", ex.Message));
            }
        }

        /// <summary>
        /// Saving this information for future use
        /// </summary>
        private void createBlankRaidNode()
        {
            //  Direct writing to file
            // Now we start the xml file
            XmlTextWriter writer = new XmlTextWriter("test.xml", System.Text.Encoding.UTF8);
            writer.WriteStartDocument(true);
            writer.Formatting = Formatting.Indented;
            writer.Indentation = 2;

            // The root
            writer.WriteStartElement("raidloot");

            // Raids Section
            writer.WriteStartElement("raid");

            // Raid Name
            writer.WriteStartElement("name");
            writer.WriteString("raid1");
            writer.WriteEndElement();

            // Raid Difficulty
            writer.WriteStartElement("difficulty");
            writer.WriteString("raid-finder");
            writer.WriteEndElement();

            // Bosses Section
            writer.WriteStartElement("boss");
            
            // Boss Name
            writer.WriteStartElement("name");
            writer.WriteString("boss1");
            writer.WriteEndElement();

            // Loot Section
            writer.WriteStartElement("loot");
            writer.WriteString("12345");
            writer.WriteEndElement();
            writer.WriteStartElement("loot");
            writer.WriteString("12345");
            writer.WriteEndElement();
            writer.WriteStartElement("loot");
            writer.WriteString("12345");
            writer.WriteEndElement();

            // Close the Bosses Section
            writer.WriteEndElement();

            // Close the Raids Section
            writer.WriteEndElement();

            // Close the Root
            writer.WriteEndElement();

            // Close the document
            writer.WriteEndDocument();

            // now close the xml file
            writer.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonRaidLoot_Click(object sender, EventArgs e)
        {
            createBlankRaidNode();

            /* todo:// Highmaul Bosses
            Markup.printHtml("[minibox]
            \n
            [h2][zone=6996][/h2]\n
	            [h3]Walled City[/h3]\n
		            [npc=78714]\n
		            [npc=77404]\n
		            [npc=78491]\n
		            [pad]\n
	            [h3]Arcane Sanctum[/h3]\n
		            [npc=78948]\n
		            [url=/npc=78238]Twin Ogron[/url]\n
		            [npc=79015]\n 
		            [pad]\n
	            [h3]Imperator's Rise[/h3]\n
		            [npc=77428]\n
            [/minibox]\n ", "lkafdnmsd", { allow: Markup.CLASS_ADMIN, dbpage: true });

            // Blackrock Foundry Bosses
            Markup.printHtml("[minibox]\n
            [h2][zone=6967][/h2]\n
	            [h3]Slagworks[/h3]\n
		            [npc=76877]\n
		            [npc=77182]\n
		            [url=/npc=76806]Blast Furnace[/url]\n[pad]\n
	            [h3]The Black Forge[/h3]\n
		            [url=/npc=76973]Hans'gar & Franzok[/url]\n
		            [npc=76814]\n
		            [npc=77692]\n[pad]\n
	            [h3]Iron Assembly[/h3]\n
		            [npc=76865]\n
		            [npc=76906]\n
		            [url=/npc=77557]Iron Maidens[/url]\n[pad]\n
	            [h3]Blackhand's Crucible[/h3]\n
		            [npc=77325]\n
            [/minibox]\n", "lkafdnmsd", { allow: Markup.CLASS_ADMIN, dbpage: true });
            */
        }

        private void buttonBossLoot_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                // Make sure the input is all digits
                if (Regex.IsMatch(textBoxBossLoot.Text, @"^\d+$"))
                {
#if DEBUG
                    // DEBUG section - pulling from file instead of web site
                    string fileData = string.Empty;
                    fileData = File.ReadAllText("78714.txt");

                    if (!string.IsNullOrWhiteSpace(fileData))
#else
                    // now get the data from the web site
                    GetWebSiteData data = new GetWebSiteData();
                    string webSite = string.Format("http://www.wowhead.com/npc={0}", Convert.ToInt32(textBoxBossLoot.Text));                    

                    if (data.Parse(webSite)) // if statement
#endif
                    {
                        // now we have the data, get all the loot 
                        //  but only the loot that has a source                        
#if DEBUG
                        // DEBUG section - pulling from file instead of web site
                        string dataString = fileData;
#else
                        string dataString = data.Data;
#endif
                        // First get the boss name
                        string boss = string.Empty;

                        int start = dataString.IndexOf("<title>") + 7;
                        int end = dataString.IndexOf("-", start);

                        boss = dataString.Substring(start, end - start).Trim();

                        start = dataString.LastIndexOf("</div></li></ul></div><");

                        if (start == -1)
                        {
                            MessageBox.Show("Can't find: </div></li></ul></div><");
                        }
                        end = dataString.IndexOf("$.extend(true, g_items, _);", start);

                        dataString = dataString.Substring(start, end - start);

                        string[] stringSeparators = new string[] { "_[" };
                        string[] results;

                        results = dataString.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);

                        Dictionary<string, List<int>> bossloot = new Dictionary<string, List<int>>();

                        // loop through all the loot
                        foreach (string item in results)
                        {
                            // now get the loot item id 
                            string searchString = @"(?<id>\d+)].*?quality"":(?<quality>\d+),.*?";
                            Regex test = new Regex(searchString, RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.CultureInvariant);

                            // Get the Average iLevel of character's Gear
                            foreach (Match result in test.Matches(item))
                            {
                                int id = 0;
                                string[] contexts = null;

                                if (result.Groups["id"].Success)
                                {
                                    id = Convert.ToInt32(result.Groups["id"].Value);

                                    // now find the item id for the loot to be dropped...
                                    contexts = FormMain.Items.GetAvailableContexts(id);

                                    if (contexts != null)
                                    {
                                        foreach (string context in contexts)
                                        {
                                            if (bossloot.ContainsKey(context))
                                            {
                                                bossloot[context].Add(id);
                                            }
                                            else
                                            {
                                                bossloot.Add(context, new List<int>() { id });
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        // todo:
                        createRaidNode("raid1", boss, bossloot);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }

            this.Cursor = Cursors.Default;
        }

        /// <summary>
        /// Saving this information for future use
        /// </summary>
        private void createRaidNode(string raid, string boss, Dictionary<string, List<int>> bossloot)
        {
            //  Direct writing to file
            // Now we start the xml file
            XmlTextWriter writer = new XmlTextWriter(boss + ".xml", System.Text.Encoding.UTF8);
            writer.WriteStartDocument(true);
            writer.Formatting = Formatting.Indented;
            writer.Indentation = 5;

            // The root
            writer.WriteStartElement("raidloot");

            foreach (string difficulty in bossloot.Keys)
            {
                // Raids Section
                writer.WriteStartElement("raid");

                // Raid Name
                writer.WriteStartAttribute("name");
                writer.WriteString(raid);
                writer.WriteEndAttribute();

                // Raid Difficulty
                writer.WriteStartAttribute("difficulty");
                switch (difficulty)
                {
                    case "raid-normal":
                        writer.WriteString("Normal");
                        break;
                    case "raid-heroic":
                        writer.WriteString("Heroic");
                        break;
                    case "raid-mythic":
                        writer.WriteString("Mythic");
                        break;
                    case "raid-finder":
                        writer.WriteString("Raid Finder");
                        break;
                    default:
                        writer.WriteString(difficulty);
                        break;
                }

                writer.WriteEndAttribute();

                // Bosses Section
                writer.WriteStartElement("boss");

                // Boss Name
                writer.WriteStartElement("name");
                writer.WriteString(boss);
                writer.WriteEndElement();

                // Loot Section                
                foreach (int id in bossloot[difficulty])
                {
                    writer.WriteStartElement("loot");
                    writer.WriteString(id.ToString());
                    writer.WriteEndElement();
                }

                // Close the Bosses Section
                writer.WriteEndElement();

                // Close the Raids Section
                writer.WriteEndElement();
            }

            // Close the Root
            writer.WriteEndElement();

            // Close the document
            writer.WriteEndDocument();

            // now close the xml file
            writer.Close();

            MessageBox.Show("File created!");
        }
    }
}
