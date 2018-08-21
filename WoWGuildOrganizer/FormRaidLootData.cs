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
        // This is a class used to hold all the loot information
        class LootInformation
        {
            public int Id { set; get; }
            public List<string> Contexts { set; get; }
            public List<string> Specs { set; get; }
        }

        // This is a class used to hold all the boss loot from a raid
        class BossLootFromRaid
        {
            public string Zone { set; get; }
            public string Name { set; get; }
            public List<LootInformation> Loot { set; get; }
        }

        public FormRaidLootData()
        {
            InitializeComponent();

            // Initialize the text boxes...
            richTextBox1.Text = 
                "Please enter either the Zone Id of a Raid or the ID of a specific boss or IDs or multiple bosses separated by comma.\n" + 
                "\tExamples include:\n" + 
                "\t\tBlackrock Foundry - 6967\n" +
                "\t\tHighmaul - 6996\n" + 
                "OR\n" +
                "\tKargath - 778714\n" + 
                "OR\n" +
                "\tBRF Bosses - 76877,77182,76806,76973,76814,77692,76865,76906,77557,77325\n";

            textBoxRaidLoot.Text = "Doesn't not work...";
            textBoxBossLoot.Text = "78714";

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

        /// <summary>
        /// TODO: Need to update for Legion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonBossLoot_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                // Make sure the input is all digits, or commas and digits
                if (Regex.IsMatch(textBoxBossLoot.Text, @"^[,0-9]+$"))
                {
                    // Check to see if this is a comma separated list of npc ids                    
                    List<BossLootFromRaid> bossLoots = new List<BossLootFromRaid>();
                    string[] stringSeparatorsComma = new string[] { "," };
                    string[] npcIds;

                    npcIds = textBoxBossLoot.Text.Split(stringSeparatorsComma, StringSplitOptions.RemoveEmptyEntries);

                    foreach (string npcId in npcIds)
                    {
                        // Add this current boss to the List of boss loots
                        bossLoots.Add(GetWowHeadNPCData(npcId));
                    }

                    // Now that we have all the data, create the xml file
                    createRaidNode(bossLoots);

                    // Now ask if you want to refresh the data
                    // todo:
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }

            this.Cursor = Cursors.Default;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private BossLootFromRaid GetWowHeadNPCData(string npcID)
        {
            // now get the data from the web site
            BossLootFromRaid bossLoot = new BossLootFromRaid();
            string[] stringSeparatorsComma = new string[] { "," };
            GetWebSiteData data = new GetWebSiteData();
            string webSite = string.Format("http://www.wowhead.com/npc={0}", Convert.ToInt32(npcID));

            if (data.Parse(webSite))
            {
                // now we have the data, get all the loot 
                //  but only the loot that has a source                        
                string dataString = data.Data;
                
                string boss = string.Empty;
                string zone = string.Empty;
                string searchString = string.Empty;
                int start = -1;
                int end = -1;
                string tempData = string.Empty;

                // First, get the boss's name
                searchString = "<title>";
                start = dataString.IndexOf(searchString) + searchString.Length;
                end = dataString.IndexOf("-", start);
                boss = dataString.Substring(start, end - start).Trim();
                bossLoot.Name = boss;

                // Second, get the boss's Zone
                searchString = @"<noscript><br /><h2 class=""heading-size-2""><a href=";
                start = dataString.IndexOf(searchString) + searchString.Length;
                start = dataString.IndexOf(">", start) + ">".Length;
                end = dataString.IndexOf("<", start);
                zone = dataString.Substring(start, end - start).Trim();
                bossLoot.Zone = zone;

                // Now get the Loot table's spec allowance information
                searchString = "new Listview({template: 'item'";
                start = dataString.LastIndexOf(searchString);

                if (start == -1)
                {
                    MessageBox.Show("Can't find: " + searchString);
                }
                end = dataString.IndexOf("});\n", start);

                tempData = dataString.Substring(start, end - start);

                // We have the temp data, but now we need to delete all the items from it that are of "slot":0,
                //  I switched it from slot:0 to classs:0 and classs: 12 so I could get the tier pieces

                // We have the temp data, but now we need to delete all the items from it that are of "classs":0,
                //  which seem to be rune drops
                while (tempData.IndexOf("\"classs\":0,") != -1)
                {
                    end = tempData.IndexOf("\"classs\":0,");
                    start = tempData.LastIndexOf("\"id\":", end);
                    tempData = tempData.Substring(0, start - 1) + tempData.Substring(end + "\"classs\":0,".Length + 1);
                }

                // We have the temp data, but now we need to delete all the items from it that are of "classs":12,
                //  which seem to be legendary drops
                while (tempData.IndexOf("\"classs\":12,") != -1)
                {
                    end = tempData.IndexOf("\"classs\":12,");
                    start = tempData.LastIndexOf("\"id\":", end);
                    tempData = tempData.Substring(0, start - 1) + tempData.Substring(end + "\"classs\":12,".Length + 1);
                }

                // Save the Loot Spec info for later use
                Dictionary<int, List<string>> specInfo = new Dictionary<int, List<string>>();

                searchString = @"""id"":(?<id>\d*),.*?""specs"":\[(?<specs>[,0-9]*?)\],.*?";
                Regex testSpecs = new Regex(searchString, RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.CultureInvariant);

                // Get the Average iLevel of character's Gear
                foreach (Match result in testSpecs.Matches(tempData))
                {
                    int id = 0;
                    string[] specs = null;

                    if (result.Groups["id"].Success)
                    {
                        id = Convert.ToInt32(result.Groups["id"].Value);
                    }

                    if (result.Groups["specs"].Success)
                    {
                        specs = result.Groups["specs"].Value.ToString().Split(stringSeparatorsComma, StringSplitOptions.RemoveEmptyEntries);

                        if (!specInfo.ContainsKey(id))
                        {
                            specInfo.Add(id, specs.ToList<string>());
                        }
                    }
                    else
                    {
                        // why doens't this work?
                        MessageBox.Show("No specs found for id=" + id);
                    }
                }

                // Now get the Loot table
                searchString = "</div></li></ul></div><";
                start = dataString.LastIndexOf(searchString);

                if (start == -1)
                {
                    MessageBox.Show("Can't find: " + searchString);
                }
                end = dataString.IndexOf("$.extend(true, g_items, _);", start);

                tempData = dataString.Substring(start, end - start);

                string[] stringSeparators = new string[] { "_[" };
                string[] results;

                results = tempData.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);

                // Now create the loot information for this boss
                bossLoot.Loot = new List<LootInformation>();

                // loop through all the loot
                foreach (string item in results)
                {
                    // now get the loot item id 
                    searchString = @"(?<id>\d+)].*?quality"":(?<quality>\d+),.*?";
                    Regex test = new Regex(searchString, RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.CultureInvariant);

                    // Get the Average iLevel of character's Gear
                    foreach (Match result in test.Matches(item))
                    {
                        int id = 0;
                        string[] contexts = null;
                        LootInformation newLoot = new LootInformation();

                        if (result.Groups["id"].Success)
                        {
                            id = Convert.ToInt32(result.Groups["id"].Value);

                            newLoot.Id = id;

                            // todo: fix this!
                            // now find the item id for the loot to be dropped...
                            //contexts = FormMain.Items.GetAvailableContexts(id);

                            if (contexts != null)
                            {
                                newLoot.Contexts = contexts.ToList<string>();

                                if (specInfo.ContainsKey(id))
                                {
                                    // Add the spec info now
                                    newLoot.Specs = specInfo[id];
                                }

                                bossLoot.Loot.Add(newLoot);
                            }
                        }
                    }
                }
            }

            return bossLoot;
        }

        /// <summary>
        /// Saving this information for future use
        /// </summary>
        private void createRaidNode(List<BossLootFromRaid> bossLoots)
        {
            bool deleteFile = false;

            if (File.Exists("raidloot.xml"))
            {
                if (MessageBox.Show("There is already a raidloot.xml file that exists, are you sure you want to overwrite it?", "Overwrite?", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    deleteFile = true;
                }
            }
            else
            {
                deleteFile = true;
            }

            if (deleteFile)
            {
                // First create the new xml file
                XmlTextWriter writer = new XmlTextWriter("raidloot.xml", System.Text.Encoding.UTF8);

                try
                {
                    //  Direct writing to file
                    // Now we start the xml file                
                    writer.WriteStartDocument(true);
                    writer.Formatting = Formatting.Indented;
                    writer.Indentation = 5;

                    // The root
                    writer.WriteStartElement("raidloot");
                                        
                    // Need to order this by Difficulty, NOT by Boss as in the data structure
                    for (int i = 0; i < 4; i++)
                    {
                        string raidDifficulty = string.Empty;

                        // First get the Difficulty Order
                        switch (i)
                        {
                            case 0:
                                raidDifficulty = "raid-finder";
                                break;
                            case 1:
                                raidDifficulty = "raid-normal";
                                break;
                            case 2:
                                raidDifficulty = "raid-heroic";
                                break;
                            case 3:
                                raidDifficulty = "raid-mythic";
                                break;
                        }

                        // Raids Section
                        writer.WriteStartElement("raid");

                        // Raid Name
                        writer.WriteStartAttribute("name");
                        writer.WriteString(bossLoots[0].Zone);
                        writer.WriteEndAttribute();
                                                
                        // Raid Difficulty
                        writer.WriteStartAttribute("difficulty");
                        switch (raidDifficulty)
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
                                writer.WriteString(raidDifficulty);
                                break;
                        }
                        writer.WriteEndAttribute();

                        foreach (BossLootFromRaid bossLoot in bossLoots)
                        {
                            // Bosses Section
                            writer.WriteStartElement("boss");

                            // Boss Name
                            writer.WriteStartElement("name");
                            writer.WriteString(bossLoot.Name);
                            writer.WriteEndElement();

                            foreach (LootInformation loot in bossLoot.Loot)
                            {
                                foreach (string difficulty in loot.Contexts)
                                {
                                    if (raidDifficulty == difficulty)
                                    {
                                        // Loot Section                
                                        writer.WriteStartElement("loot");

                                        writer.WriteStartElement("id");
                                        writer.WriteString(loot.Id.ToString());
                                        writer.WriteEndElement();

                                        if (loot.Specs != null)
                                        {
                                            foreach (string spec in loot.Specs)
                                            {
                                                writer.WriteStartElement("spec");
                                                writer.WriteString(spec);
                                                writer.WriteEndElement();
                                            }
                                        }

                                        // Close the loot section
                                        writer.WriteEndElement();

                                        // todo: debug
                                        writer.Flush();
                                    }
                                }
                            }

                            // Close the Bosses Section
                            writer.WriteEndElement();

                            // todo: debug
                            writer.Flush();
                        }

                        // Close the Raids Section
                        writer.WriteEndElement();

                        // todo: debug
                        writer.Flush();
                    }

                    // Close the Root
                    writer.WriteEndElement();

                    // Close the document
                    writer.WriteEndDocument();

                    // now close the xml file
                    writer.Close();

                    MessageBox.Show("File created!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);

                    writer.Flush();
                    writer.Close();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonGetCurrentRaids_Click(object sender, EventArgs e)
        {
            try
            {
                GetWebJSONData getData = new GetWebJSONData();
                JSONRaidData data = getData.GetRaidJSONData();
                List<int> expansions = new List<int>();

                if (data != null)
                {
                    // Find the last Expansion Id.  That will be the one we care about
                    foreach (JSONZoneData raid in data.Zones)
                    {
                        if (!expansions.Contains(raid.ExpansionId))
                        {
                            expansions.Add(raid.ExpansionId);
                        }
                    }

                    int expansionId = expansions != null ? expansions.Max() : 0;
                    List<BossLootFromRaid> bossLoots = new List<BossLootFromRaid>();

                    foreach (JSONZoneData raid in data.Zones)
                    {
                        if (raid.ExpansionId == expansionId && raid.IsRaid)
                        {
                            // We found a relevant raid!  Capture all the info.
                            BossLootFromRaid newRaid = new BossLootFromRaid();
                            newRaid.Name = raid.Name;
                            newRaid.Zone = raid.Id;

                            // Now create the loot information for this boss
                            newRaid.Loot = new List<LootInformation>();

                            /* todo:
                            foreach (DataRow item in FormMain.Items.GetData().Rows)
                            {
                                if (item["iteminfo"].ToString() == "test")
                                {
                                    LootInformation newLoot = new LootInformation();
                                    newRaid.Loot = newLoot;
                                }
                            }*/

                            foreach (JSONRaidBossData boss in raid.Bosses)
                            {
                                // Add this current boss to the List of boss loots
                                bossLoots.Add(GetWowHeadNPCData(boss.Id));
                            }
                        }
                    }

                    // Now that we have all the data, create the xml file
                    createRaidNode(bossLoots);
                }
            }
            catch (Exception ex)
            {
                Logging.DisplayError($"Error: {ex.Message}");
            }
        }
    }
}
