// <copyright file="FormWGO.cs" company="Secondnorth.com">
//     Secondnorth.com. All rights reserved.
// </copyright>
// <author>Me</author>

namespace WoWGuildOrganizer
{
    #region " Includes "
    
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    
    #endregion

    /// <summary>
    /// Form class for the World of Warcraft Guild Organizer
    /// </summary>
    public partial class FormMain : Form
    {
        #region " Class Variables "

        /// <summary>
        /// static string of the URL for the Blizzard API
        /// </summary>
        public readonly string URLWowAPI = @"http://us.battle.net/api/wow/";

        /// <summary>
        /// String of how a basic guild sort should be
        /// </summary>
        private readonly string basicGuildSort = "Level DESC, EquipediLevel DESC, MaxiLevel DESC, AchievementPoints DESC";

        /// <summary>
        /// All the persistently saved guild characters
        /// </summary>
        private GuildMemberGroup savedCharacters;

        /// <summary>
        /// A persistently saved raid group of characters
        ///  Can be cross realm
        /// </summary>
        private RaidMemberGroup raidGroup;

        /// <summary>
        /// Background work for doing web site scrapping
        /// </summary>
        private BackgroundWorker guildInfoAsyncWorker = new BackgroundWorker();

        /// <summary>
        /// Stop watch to get timing results
        /// </summary>
        private StopWatch sw = new StopWatch();

        /// <summary>
        /// Dictionary containing all the Raid loot from a raid boss
        /// </summary>
        private Dictionary<string, Dictionary<string, int[]>> raidLoot = new Dictionary<string, Dictionary<string, int[]>>();

        #endregion

        #region " Constructor "

        /// <summary>
        /// Initializes a new instance of the <see cref="FormMain" /> class
        /// </summary>
        public FormMain()
        {
            this.InitializeComponent();

            // Load up the guild name textbox if it's been saved before
            if (!string.IsNullOrEmpty(Properties.Settings.Default.GuildName))
            {
                textBoxGuildName.Text = Properties.Settings.Default.GuildName;
                toolStripTextBoxGuild.Text = Properties.Settings.Default.GuildName;
            }

            // Load up the realm name textbox if it's been saved before
            if (!string.IsNullOrEmpty(Properties.Settings.Default.Realm))
            {
                textBoxRealm.Text = Properties.Settings.Default.Realm;
                textBoxCharacterRealm.Text = textBoxRealm.Text;
                toolStripTextBoxRealm.Text = Properties.Settings.Default.Realm;
            }

            // init the struct that will store all the char info
            this.savedCharacters = new GuildMemberGroup();

            // setup the data grid view
            this.SetupDataGridView();            

            // Attempt to load the last guild saved...
            //   check for a data file and if there is one, try to load it up
            try
            {
                if (File.Exists("SavedCharacters.dat"))
                {
                    // Open the file written above and read values from it.
                    Stream stream = File.Open("SavedCharacters.dat", FileMode.Open);
                    BinaryFormatter bformatter = new BinaryFormatter();

                    this.savedCharacters = (GuildMemberGroup)bformatter.Deserialize(stream);
                    stream.Close();

                    toolStripLabelRefreshStatus.Text = this.savedCharacters.SavedCharacters.Count.ToString() + " total characters";
                }
            }
            catch (Exception ex)
            {
                if (!ex.Message.StartsWith("Could not find file"))
                {
                    Logging.DisplayError(ex.Message);
                }
            }

            // init the struct that will store all the raod info
            this.raidGroup = new RaidMemberGroup();

            // Attempt to load the last raid saved...
            //   check for a data file and if there is one, try to load it up
            try
            {
                if (File.Exists("RaidGroup.dat"))
                {
                    // Open the file written above and read values from it.
                    Stream stream = File.Open("RaidGroup.dat", FileMode.Open);
                    BinaryFormatter bformatter = new BinaryFormatter();

                    this.raidGroup = (RaidMemberGroup)bformatter.Deserialize(stream);
                    stream.Close();
                }
            }
            catch (Exception ex)
            {
                if (!ex.Message.StartsWith("Could not find file"))
                {
                    Logging.DisplayError(ex.Message);
                }
            }

            // init the struct that will store all the char info
            Items = new ItemCache();

            // Attempt to load the last Item Cache saved...
            try
            {
                if (File.Exists("ItemCache.dat"))
                {
                    // Open the file written above and read values from it.
                    Stream stream = File.Open("ItemCache.dat", FileMode.Open);
                    BinaryFormatter bformatter = new BinaryFormatter();

                    Items = (ItemCache)bformatter.Deserialize(stream);
                    stream.Close();
                }
            }
            catch (Exception ex)
            {
                if (!ex.Message.StartsWith("Could not find file"))
                {
                    Logging.DisplayError(ex.Message);
                }
            }

            // Create the Raid Loot Data
            RaidInfo raidInfo = new RaidInfo();
            raidInfo.CreateRaidLootData(ref this.raidLoot);

            // Add Raid Loot Drop Items
            foreach (string key in this.raidLoot.Keys)
            {
                toolStripComboBoxPickRaid.Items.Add(key);
            }

            // Assume that the Web Site is currently online
            WebSiteOnline = true;

            // Create a background worker thread that Off Loads work, Reports Progress and Supports Cancellation            
            this.guildInfoAsyncWorker.WorkerReportsProgress = true;
            this.guildInfoAsyncWorker.WorkerSupportsCancellation = true;
            this.guildInfoAsyncWorker.ProgressChanged += new ProgressChangedEventHandler(this.GetGuildInfoAsync_ProgressChanged);
            this.guildInfoAsyncWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.GetGuildInfoAsync_RunWorkerCompleted);
            this.guildInfoAsyncWorker.DoWork += new DoWorkEventHandler(this.GetGuildInfoAsync_DoWork);
        }

        #endregion

        #region " Class Properties "

        /// <summary>
        /// Gets or sets a persistent cache of all the WoW Item Data needed
        /// </summary>
        public static ItemCache Items { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to let the app know if it should attempt to connect
        ///  to the Blizzard Battle.net web site
        /// </summary>
        public static bool WebSiteOnline
        {
            get;
            set;
        }

        #endregion

        #region " Guild Data DataGridView Functions "

        /// <summary>
        /// Setup the Data Grid View for guild members
        /// </summary>
        private void SetupDataGridView()
        {
            // Setup the Main Guild Data Grid
            dataGridViewGuildData.ColumnCount = 0;

            dataGridViewGuildData.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            dataGridViewGuildData.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridViewGuildData.ColumnHeadersDefaultCellStyle.Font = new Font(dataGridViewGuildData.Font, FontStyle.Bold);

            dataGridViewGuildData.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            dataGridViewGuildData.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewGuildData.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            dataGridViewGuildData.GridColor = Color.Black;
            dataGridViewGuildData.RowHeadersVisible = true;

            // Set the background color for all rows and for alternating rows. 
            // The value for alternating rows overrides the value for all rows. 
            dataGridViewGuildData.RowsDefaultCellStyle.BackColor = Color.White;

            dataGridViewGuildData.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewGuildData.MultiSelect = true;
            dataGridViewGuildData.ReadOnly = true;
            dataGridViewGuildData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewGuildData.AutoResizeColumns();

            // Now Setup the Raid Data Grid
            dataGridViewRaidGroup.ColumnCount = 0;

            dataGridViewRaidGroup.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            dataGridViewRaidGroup.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridViewRaidGroup.ColumnHeadersDefaultCellStyle.Font = new Font(dataGridViewRaidGroup.Font, FontStyle.Bold);

            dataGridViewRaidGroup.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            dataGridViewRaidGroup.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewRaidGroup.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            dataGridViewRaidGroup.GridColor = Color.Black;
            dataGridViewRaidGroup.RowHeadersVisible = true;

            // Set the background color for all rows and for alternating rows. 
            // The value for alternating rows overrides the value for all rows. 
            dataGridViewRaidGroup.RowsDefaultCellStyle.BackColor = Color.White;

            dataGridViewRaidGroup.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewRaidGroup.MultiSelect = true;
            dataGridViewRaidGroup.ReadOnly = true;
            dataGridViewRaidGroup.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewRaidGroup.AutoResizeColumns();
        }

        /// <summary>
        /// Update the main grid
        /// </summary>
        private void UpdateGrid()
        {
            int count = 0;

            // Update the grid back color for the rows to white
            dataGridViewGuildData.RowsDefaultCellStyle.BackColor = Color.White;
            dataGridViewGuildData.AutoResizeColumns();
            dataGridViewGuildData.AutoResizeRows();

            // Now check to see if the Item Level has changed and if their level has changed
            foreach (GuildMember gm in this.savedCharacters.SavedCharacters)
            {
                if (gm.IsItemLevelChanged())
                {
                    dataGridViewGuildData.Rows[count].Cells[5].Style.BackColor = Color.Aquamarine;
                }
                else
                {
                    dataGridViewGuildData.Rows[count].Cells[5].Style.BackColor = Color.White;
                }

                if (gm.IsLevelChanged())
                {
                    dataGridViewGuildData.Rows[count].Cells[1].Style.BackColor = Color.Aquamarine;
                }
                else
                {
                    dataGridViewGuildData.Rows[count].Cells[1].Style.BackColor = Color.White;
                }

                if (gm.IsItemLevelChanged())
                {
                    dataGridViewGuildData.Rows[count].Cells[6].Style.BackColor = Color.Aquamarine;
                }
                else
                {
                    dataGridViewGuildData.Rows[count].Cells[6].Style.BackColor = Color.White;
                }

                if (dataGridViewGuildData.Rows[count].Cells[7].Value != null)
                {
                    // Check to see when the ItemLevel was last updated
                    DateTime checkAgain = (DateTime)dataGridViewGuildData.Rows[count].Cells[7].Value;

                    if (DateTime.Now > checkAgain.AddDays(7))
                    {
                        dataGridViewGuildData.Rows[count].Cells[7].Style.BackColor = Color.Red;
                    }
                    else if (DateTime.Now > checkAgain.AddDays(5))
                    {
                        dataGridViewGuildData.Rows[count].Cells[7].Style.BackColor = Color.OrangeRed;
                    }
                    else if (DateTime.Now > checkAgain.AddDays(3))
                    {
                        dataGridViewGuildData.Rows[count].Cells[7].Style.BackColor = Color.Orange;
                    }
                    else if (DateTime.Now > checkAgain.AddDays(2))
                    {
                        dataGridViewGuildData.Rows[count].Cells[7].Style.BackColor = Color.Yellow;
                    }
                    else if (DateTime.Now > checkAgain.AddDays(1))
                    {
                        dataGridViewGuildData.Rows[count].Cells[7].Style.BackColor = Color.LightYellow;
                    }
                    else
                    {
                        dataGridViewGuildData.Rows[count].Cells[7].Style.BackColor = Color.White;
                    }
                }
                else
                {
                    dataGridViewGuildData.Rows[count].Cells[7].Style.BackColor = Color.Red;
                }

                count++;
            }

            foreach (DataGridViewColumn col in dataGridViewGuildData.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.Programmatic;
                col.HeaderCell.SortGlyphDirection = SortOrder.None;
            }

            // refresh the grid data since it's been changed
            dataGridViewGuildData.Refresh();
        }

        /// <summary>
        /// A double click on in a cell on the data grid view
        ///  update the individual chars Armory info and Gear Score
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">e parameter</param>
        private void DataGridViewGuildData_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridViewCell currentCell;
            int currentRow = -1;

            // Get the current cell.
            currentCell = dataGridViewGuildData.CurrentCell;

            // Get the cell's current row
            currentRow = currentCell.RowIndex;

            // First check to make sure the vars are passed in
            if (currentRow < 0)
            {
                MessageBox.Show("Error: No row was selected.");
            }
            else
            {
                this.Cursor = Cursors.WaitCursor;

                // This will be a audit function.
                //  So if a character is double clicked, an audit table will pop up.
                //  The Audit Table will show the following things:
                //  1. One line for each character slot
                //  2. Each line will contain a deduction if the item is ...
                //     a. item missing
                //     b. not enchanted
                //     c. not gemmed - URL = Host + "/api/wow/data/item/" + ItemId => "hasSockets":false,
                //         - Will need a item cache though
                //     d. item level
                //  3. Ordered by item level, lowest to highest
                // Future Ideas
                //  1. Missing Glyphs
                //  2. Raid Progression
                //  3. Professions

                // Create the new form to be used
                FormItemAudit charAudit = new FormItemAudit();

                // Get the name
                string charName = ((GuildMember)this.savedCharacters.SavedCharacters[currentRow]).Name;

                // Pass the name to the new form
                charAudit.CharacterName = charName;

                charAudit.EquippediLevel = ((GuildMember)this.savedCharacters.SavedCharacters[currentRow]).EquipediLevel.ToString();
                charAudit.MaxiLevel = ((GuildMember)this.savedCharacters.SavedCharacters[currentRow]).MaxiLevel.ToString();

                charAudit.Profession1 = ((GuildMember)this.savedCharacters.SavedCharacters[currentRow]).Profession1;
                charAudit.Profession2 = ((GuildMember)this.savedCharacters.SavedCharacters[currentRow]).Profession2;

                charAudit.Spec = ((GuildMember)this.savedCharacters.SavedCharacters[currentRow]).Spec;
                charAudit.Role = ((GuildMember)this.savedCharacters.SavedCharacters[currentRow]).Role;

                GuildMember character = (GuildMember)this.savedCharacters.SavedCharacters[currentRow];

                // Pass the Data to the Form
                if (charAudit.PassData(character))
                {
                    // Show the new form
                    charAudit.Show();
                }
                else
                {
                    Logging.DisplayError("Can't Audit Character: " + charName);
                }
            }

            this.Cursor = Cursors.Default;
        }

        /// <summary>
        /// Sorts the Guild Grid according to the parameter passed in
        /// </summary>
        /// <param name="sorting">string of the sorting that should happen</param>
        private void SortGrid(string sorting)
        {
            bool multipleSort = false;

            if (sorting.Contains(","))
            {
                // has multiple sorting factors
                multipleSort = true;
            }

            dataGridViewGuildData.DataSource = null;

            this.savedCharacters.SavedCharacters.Sort(new ObjectComparer(sorting, multipleSort));

            // refresh grid data
            dataGridViewGuildData.DataSource = this.savedCharacters.SavedCharacters;

            // Now update the grid
            this.UpdateGrid();

            // Set the sorting glyphs
            string[] sortExpressions = sorting.Trim().Split(',');
            for (int i = 0; i < sortExpressions.Length; i++)
            {
                string fieldName = string.Empty;
                SortOrder direction = SortOrder.None;

                if (sortExpressions[i].Trim().EndsWith(" DESC"))
                {
                    fieldName = sortExpressions[i].Replace(" DESC", string.Empty).Trim();
                    direction = SortOrder.Descending;
                }
                else
                {
                    fieldName = sortExpressions[i].Replace(" ASC", string.Empty).Trim();
                    direction = SortOrder.Ascending;
                }

                foreach (DataGridViewColumn col in dataGridViewGuildData.Columns)
                {
                    if (fieldName == col.HeaderText)
                    {
                        col.HeaderCell.SortGlyphDirection = direction;
                    }
                }
            }
        }

        /// <summary>
        /// A click of a header that will sort that header
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">e parameter</param>
        private void DataGridViewGuildData_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridViewColumn newColumn = dataGridViewGuildData.Columns[e.ColumnIndex];

            // Get the property name
            string sortCriteria = dataGridViewGuildData.Columns[e.ColumnIndex].HeaderCell.Value.ToString();

            // If oldColumn is null, then the DataGridView is not sorted.
            if (newColumn.HeaderCell.SortGlyphDirection == SortOrder.Ascending)
            {
                sortCriteria += " DESC";
            }
            else
            {
                sortCriteria += " ASC";
            }

            // now do the sorting
            try
            {
                this.SortGrid(sortCriteria);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Can't sort on that column YET!  [" + ex.Message + "]");
            }
        }

        #endregion

        #region " Get Guild Information Async Background Worker "

        /// <summary>
        /// Called when the Get Guild Info progress has changed
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">e parameter</param>
        private void GetGuildInfoAsync_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // This function fires on the UI thread so it's safe to edit
            // the UI control directly, no funny business with Control.Invoke.
            // Update the progressBar with the integer supplied to us from the
            // ReportProgress() function.  Note, e.UserState is a "tag" property
            // that can be used to send other information from the
            // BackgroundThread to the UI thread.
            progressBarCollectData.Value = e.ProgressPercentage;
        }

        /// <summary>
        /// Called when the Guild Refresh button is pressed
        ///   Will check battle.net for any changes to guild members
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">e parameter</param>
        private void GetGuildInfoAsync_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                // The sender is the BackgroundWorker object we need it to
                // report progress and check for cancellation.
                BackgroundWorker async = sender as BackgroundWorker;

                // start the wait cursor
                this.WaitCursor(true);

                // Stop updating the Grid until the end
                this.SuspendGrid(true);

                // Periodically check if a cancellation request is pending.
                // If the user clicks cancel the line
                // m_AsyncWorker.CancelAsync(); if ran above.  This
                // sets the CancellationPending to true.
                // You must check this flag in here and react to it.
                // We react to it by setting e.Cancel to true and leaving.
                if (async.CancellationPending)
                {
                    // stop the job...
                    Logging.Log("Searching stopped!");
                }
                else
                {
                    // First get the entire Guild Roster
                    try
                    {
                        // This is the Web Site to get the guild info from...
                        // http://us.battle.net/api/wow/guild/Thrall/Secondnorth?fields=members

                        // Reset the Counter
                        async.ReportProgress(0);

                        // Get the guild information
                        GetGuildInfo guildInfo = new GetGuildInfo();

                        if (guildInfo.CollectData(this.URLWowAPI + @"guild/" + textBoxRealm.Text + @"/" + textBoxGuildName.Text + @"?fields=members"))
                        {
                            // success!

                            // now check to see if a grid needs to be updated or if it's the first time used
                            if (this.savedCharacters.SavedCharacters.Count > 0)
                            {
                                // save the current data to a temp var
                                ArrayList temp = new ArrayList();
                                foreach (GuildMember m in this.savedCharacters.SavedCharacters)
                                {
                                    temp.Add(m);
                                }

                                // erase the current data so we can start new
                                this.savedCharacters.SavedCharacters.Clear();

                                // Fill out the data grid with the data we collected
                                this.savedCharacters.SavedCharacters = guildInfo.Characters;

                                this.savedCharacters.Guild = textBoxGuildName.Text;
                                this.savedCharacters.Realm = textBoxRealm.Text;

                                foreach (GuildMember newmember in this.savedCharacters.SavedCharacters)
                                {
                                    // now see if this is a new char or if level was updated
                                    foreach (GuildMember oldmember in temp)
                                    {
                                        if (newmember.Name == oldmember.Name)
                                        {
                                            // match!

                                            // now check to see if level or achievement points have been updated
                                            if (newmember.Level.CompareTo(oldmember.Level) == 0)
                                            {
                                                newmember.ClearFlags();
                                            }

                                            // always check to carry over the max iLevel value from old to new
                                            // since new will always be blank
                                            if (newmember.MaxiLevel == 0 && oldmember.MaxiLevel != 0)
                                            {
                                                newmember.MaxiLevel = oldmember.MaxiLevel;
                                                newmember.ClearMaxItemLevelFlag();
                                            }

                                            // always check to carry over the max iLevel value from old to new
                                            // since new will always be blank
                                            if (newmember.EquipediLevel == 0 && oldmember.EquipediLevel != 0)
                                            {
                                                newmember.EquipediLevel = oldmember.EquipediLevel;
                                                newmember.ClearEquipItemLevelFlag();
                                            }

                                            break;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                // First time collection of the guild...
                                //  Fill out the data grid with the data we collected
                                this.savedCharacters.SavedCharacters = guildInfo.Characters;
                            }
                        }

                        // Now get the individual Character data
                        int count = 0;
                        int total = this.savedCharacters.SavedCharacters.Count;
                        ArrayList errors = new ArrayList();

                        try
                        {
                            // Go through all the guild members
                            foreach (GuildMember gm in this.savedCharacters.SavedCharacters)
                            {
                                // Only check for Item Level for characters over level 10
                                //  Otherwise these characters won't be in the Armory
                                if (Convert.ToInt32(gm.Level) >= 10)
                                {
                                    // This is the Web Site to get the character info from...
                                    // http://us.battle.net/api/wow/character/Thrall/Purdee?fields=items,professions,talents                                    
                                    GuildMember charInfo = this.GetCharacterInformation(gm.Name, this.savedCharacters.Realm);

                                    if (charInfo != null)
                                    {
                                        // success!

                                        // Fill out the data grid with the data we collected
                                        gm.MaxiLevel = charInfo.MaxiLevel;
                                        gm.EquipediLevel = charInfo.EquipediLevel;
                                        gm.Profession1 = charInfo.Profession1;
                                        gm.Profession2 = charInfo.Profession2;
                                        gm.Spec = charInfo.Spec;
                                        gm.Role = charInfo.Role;
                                        gm.ItemAudits = charInfo.ItemAudits;

                                        if (charInfo.AchievementPoints == 0)
                                        {
                                            gm.AchievementPoints = gm.AchievementPoints;
                                        }
                                        else if (gm.AchievementPoints == 0)
                                        {
                                            gm.AchievementPoints = charInfo.AchievementPoints;
                                        }
                                        else
                                        {
                                            Logging.Log(string.Format("WARN: Old vs New Achievement points.  [{0}] vs [{1}].", gm.AchievementPoints, charInfo.AchievementPoints));
                                        }
                                    }
                                    else
                                    {
                                        // Fail!  Save all errors until the end!
                                        Logging.Log("      " + gm.Name + "\t\t" + gm.Level);
                                    }
                                }

                                // Progress update
                                this.UpdateLabelMT(this.savedCharacters.SavedCharacters.Count.ToString() + " total characters - On Character #" + count);

                                // Progress update for Progress Bar
                                double tempNum = (double)count++ / (double)total * 100;
                                async.ReportProgress((int)tempNum);
                            }
                        }
                        catch (Exception ex)
                        {
                            Logging.Log("Error: " + ex.Message);
                        }

                        // Data has been gathered now...
                        async.ReportProgress(100);

                        // re-sort by Item Level
                        this.SortGridMT(this.basicGuildSort);

                        // Stop the stopwatch
                        this.sw.Stop();

                        // Check to see if there were any errors while trying to gather data for a character
                        if (errors.Count > 0)
                        {
                            string errorMessage = "ERROR - Failed to parse some gear scores...\n   Couldn't retrieve information for the following characters:\n";

                            foreach (string str in errors)
                            {
                                errorMessage += str + "\n";
                            }

                            Logging.Log(errorMessage);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logging.Log("  **ERROR: " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.Log("  **ERROR: " + ex.Message);
            }
        }

        /// <summary>
        /// The background process is complete. We need to inspect
        /// our response to see if an error occurred, a cancel was
        /// requested or if we completed successfully.
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">e parameter</param>
        private void GetGuildInfoAsync_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // Check to see if an error occurred in the background process.
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
                return;
            }

            // Check to see if the background process was cancelled.
            if (e.Cancelled)
            {
                Logging.Log("Search has been cancelled!");
            }
            else
            {
                // Everything completed normally.

                // process the response using e.Result
                Logging.Log("Search has been completed!");

                // change the buttons back
                buttonGetGuildInfo.Text = "Get Guild Info";
                //buttonRefreshGuildData.Text = "Refresh";

                // start the wait cursor
                this.WaitCursor(false);

                this.UpdateLabelMT(this.savedCharacters.SavedCharacters.Count.ToString() + " total characters in " + this.sw.GetElapsedTime() + " milliseconds");

                // now switch the tab
                tabControlWGO.SelectTab(0);
            }

            // Stop updating the Grid until the end
            this.SuspendGrid(false);
        }

        #endregion

        #region " Class Form Functions "

        /// <summary>
        /// Before the form closes, save the important variables
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">e parameter</param>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.savedCharacters.Guild))
            {
                Properties.Settings.Default.GuildName = textBoxGuildName.Text;
            }

            if (!string.IsNullOrEmpty(this.savedCharacters.Realm))
            {
                Properties.Settings.Default.Realm = textBoxRealm.Text;
            }

            Properties.Settings.Default.Save();

            try
            {
                if (this.savedCharacters.SavedCharacters.Count > 0)
                {
                    Stream stream = File.Open("SavedCharacters.dat", FileMode.Create);
                    BinaryFormatter bformatter = new BinaryFormatter();
                    bformatter.Serialize(stream, this.savedCharacters);
                    stream.Close();
                }

                if (this.raidGroup.RaidGroup.Count > 0)
                {
                    Stream stream = File.Open("RaidGroup.dat", FileMode.Create);
                    BinaryFormatter bformatter = new BinaryFormatter();
                    bformatter.Serialize(stream, this.raidGroup);
                    stream.Close();
                }

                if (Items.GetCount() > 0)
                {
                    Stream stream = File.Open("ItemCache.dat", FileMode.Create);
                    BinaryFormatter bformatter = new BinaryFormatter();
                    bformatter.Serialize(stream, Items);
                    stream.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        /// <summary>
        /// This is needed to update the grid with colors
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">e parameter</param>
        private void Form1_Load(object sender, EventArgs e)
        {
            this.SortGrid(this.basicGuildSort);

            // refresh the grid data since it's been changed
            dataGridViewRaidGroup.DataSource = null;
            dataGridViewRaidGroup.DataSource = this.raidGroup.RaidGroup;
            this.UpdateRaidGrid();
        }
        
        #endregion

        #region " Tab Functions "

        /// <summary>
        /// The selected tab is changed
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">e parameter</param>
        private void TabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControlWGO.SelectedTab.Text == "Guild Data")
            {
                // Setup the Main Tool strip

                // Hide all controls not needed for this tab:
                toolStripComboBoxPickRaid.Visible = false;
                toolStripLabelPickRaid.Visible = false;
                toolStripComboBoxPickBoss.Visible = false;
                toolStripLabelPickBoss.Visible = false;
                toolStripButtonAdd.Visible = false;
                toolStripProgressBar1.Visible = false;

                // Show all controls needed for this tab:                
                toolStripLabelGuild.Visible = true;
                toolStripTextBoxGuild.Visible = true;
                toolStripLabelRealm.Visible = true;
                toolStripTextBoxRealm.Visible = true;
                toolStripLabelRefreshStatus.Visible = true;
                toolStripButtonRefresh.Visible = true;

                // Update Text as needed
                toolStripLabelGuild.Text = "Guild:";
                toolStripLabelRefreshStatus.Text = "in progress...";

                // Check to see what the currently saved guild is
                if (this.savedCharacters.SavedCharacters.Count > 0)
                {
                    toolStripTextBoxGuild.Text = this.savedCharacters.Guild;
                }
                else
                {
                    toolStripTextBoxGuild.Text = string.Empty;
                }

                // Guild Members Tab
                this.UpdateGrid();
            }
            else if (tabControlWGO.SelectedTab.Text == "Raid Data")
            {
                // Raid Group Tab

                // Hide all controls not needed for this tab:
                toolStripComboBoxPickRaid.Visible = false;
                toolStripLabelPickRaid.Visible = false;
                toolStripComboBoxPickBoss.Visible = false;
                toolStripLabelPickBoss.Visible = false;
                toolStripProgressBar1.Visible = false;

                // Show all controls needed for this tab:
                toolStripLabelGuild.Text = "Character:";
                toolStripLabelGuild.Visible = true;
                toolStripTextBoxGuild.Visible = true;
                toolStripTextBoxGuild.Text = string.Empty;
                toolStripLabelRealm.Visible = true;
                toolStripTextBoxRealm.Visible = true;
                toolStripButtonAdd.Visible = true;
                toolStripLabelRefreshStatus.Visible = true;
                toolStripButtonRefresh.Visible = true;

                // Update Text as needed
                toolStripLabelGuild.Text = "Guild:";
                toolStripLabelRefreshStatus.Text = "in progress...";

                // Raid Group Tab
                this.UpdateRaidGrid();
            }
            else if (tabControlWGO.SelectedTab.Text == "Raid Loot Drops")
            {
                // Boss Loot tab

                // Hide all controls not needed for this tab:
                toolStripLabelGuild.Visible = false;
                toolStripTextBoxGuild.Visible = false;
                toolStripLabelRealm.Visible = false;
                toolStripTextBoxRealm.Visible = false;
                toolStripButtonAdd.Visible = false;
                toolStripProgressBar1.Visible = false;
                toolStripLabelRefreshStatus.Visible = false;

                // Show all controls needed for this tab:
                toolStripComboBoxPickRaid.Visible = true;
                toolStripLabelPickRaid.Visible = true;
                toolStripComboBoxPickBoss.Visible = true;
                toolStripLabelPickBoss.Visible = true;
                toolStripButtonRefresh.Visible = true;
            }
            else if (tabControlWGO.SelectedTab.Text == "Settings")
            {
                // Boss Loot tab

                // Hide all controls not needed for this tab:
                toolStripLabelGuild.Visible = false;
                toolStripTextBoxGuild.Visible = false;
                toolStripLabelRealm.Visible = false;
                toolStripTextBoxRealm.Visible = false;
                toolStripButtonAdd.Visible = false;
                toolStripComboBoxPickRaid.Visible = false;
                toolStripLabelPickRaid.Visible = false;
                toolStripComboBoxPickBoss.Visible = false;
                toolStripLabelPickBoss.Visible = false;
                toolStripButtonRefresh.Visible = false;
                toolStripProgressBar1.Visible = false;
                toolStripSeparator1.Visible = false;
                toolStripLabelRefreshStatus.Visible = false;
            }
        }

        #endregion

        #region " Setting Functions "

        /// <summary>
        /// Loads guild data from a saved file
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">e parameter</param>
        private void ButtonLoad_Click(object sender, EventArgs e)
        {
            // check for a data file... if there is one, try to load it up
            // Attempt to load the last cardset saved
            try
            {
                OpenFileDialog open = new OpenFileDialog();
                open.Filter = "dat files (*.dat)|*.dat|All files (*.*)|*.*";
                open.FilterIndex = 1;
                open.RestoreDirectory = true;

                if (open.ShowDialog() == DialogResult.OK)
                {
                    // open file
                    if (File.Exists(open.FileName))
                    {
                        // Open the file written above and read values from it.
                        Stream stream = File.Open(open.FileName, FileMode.Open);
                        BinaryFormatter bformatter = new BinaryFormatter();

                        // The file was read successfully, so now re-init the 
                        // SavedCharacters var and load up the new info to the Grid
                        this.savedCharacters = new GuildMemberGroup();
                        this.savedCharacters = (GuildMemberGroup)bformatter.Deserialize(stream);
                        stream.Close();

                        //todo: label3.Text = this.savedCharacters.SavedCharacters.Count.ToString() + " total characters";
                        this.SortGrid(this.basicGuildSort);

                        // update the text boxes                        
                        textBoxRealm.Text = this.savedCharacters.Realm;
                        textBoxGuildName.Text = this.savedCharacters.Guild;

                        // now switch the tab
                        tabControlWGO.SelectTab(0);
                    }
                }
            }
            catch (Exception ex)
            {
                if (!ex.Message.StartsWith("Could not find file"))
                {
                    Logging.Log(string.Format("  **ERROR[LoadTempData]: {0}", ex.Message));
                }
            }
        }

        /// <summary>
        /// Fires when the save button is pressed
        ///   Saves the guild member data to a serialized file
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">e parameter</param>
        private void ButtonSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "dat files (*.dat)|*.dat|All files (*.*)|*.*";
            save.FilterIndex = 1;
            save.RestoreDirectory = true;

            if (save.ShowDialog() == DialogResult.OK)
            {
                // save it
                try
                {
                    if (this.savedCharacters.SavedCharacters.Count > 0)
                    {
                        Stream stream = File.Open(save.FileName, FileMode.Create);
                        BinaryFormatter bformatter = new BinaryFormatter();
                        bformatter.Serialize(stream, this.savedCharacters);
                        stream.Close();
                    }
                }
                catch (Exception ex)
                {
                    Logging.DisplayError(ex.Message);
                }
            }
        }

        /// <summary>
        /// Fires when the Delete Item Cache button is pressed
        ///   Deletes the persistent item data
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">e parameter</param>
        private void ButtonDeleteItemCacheData_Click(object sender, EventArgs e)
        {
            using (FormItemCacheManager frm = new FormItemCacheManager())
            {
                frm.ShowDialog();
            }
        }

        /// <summary>
        /// Fires when the Show Errors button is pressed
        ///   Brings up another form that displays all the errors found in the current session
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">e parameter</param>
        private void ButtonShowErrors_Click(object sender, EventArgs e)
        {
            Logging.ShowAllErrors();
        }

        #endregion

        #region " Button Functions "

        /// <summary>
        /// Refresh the Guild Data 
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">e parameter</param>
        private void ButtonRefreshGuildData_Click(object sender, EventArgs e)
        {
            // Simply Callthe buttonGetGuildInfo click 
            this.ButtonGetGuildInfo_Click(sender, e);
        }

        /// <summary>
        /// Fires when the Refresh button is clicked on the Guild tab
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">e parameter</param>
        private void ButtonGetGuildInfo_Click(object sender, EventArgs e)
        {
            // First check to make sure the vars are passed in
            if (textBoxGuildName.Text.Length == 0 || textBoxRealm.Text.Length == 0)
            {
                MessageBox.Show("Error: Both Guild Name and Realm need to be filled out.");
            }
            else
            {
                // If the background thread is running then clicking this
                // button causes a cancel, otherwise clicking this button
                // launches the background thread.
                if (this.guildInfoAsyncWorker.IsBusy)
                {
                    Logging.Log("Cancelling Search...");

                    buttonGetGuildInfo.Enabled = false;
                    //buttonRefreshGuildData.Enabled = false;

                    // Notify the worker thread that a cancel has been requested.
                    // The cancel will not actually happen until the thread in the
                    // DoWork checks the bwAsync.CancellationPending flag, for this
                    // reason we set the label to "Cancelling...", because we haven't
                    // actually cancelled yet.
                    this.guildInfoAsyncWorker.CancelAsync();
                }
                else
                {
                    Logging.Log("Search Started...");

                    // start the wait cursor
                    this.WaitCursor(true);

                    // Change the button text so that this can be cancelled
                    buttonGetGuildInfo.Text = "Cancel";
                    //buttonRefreshGuildData.Text = "Cancel";

                    // Start the stop watch
                    this.sw.Start();

                    // Kickoff the worker thread to begin it's DoWork function.
                    this.guildInfoAsyncWorker.RunWorkerAsync();
                }
            }
        }

        #endregion

        #region " Raid Tab Functionality "

        /// <summary>
        /// Fires when Add Character button is pressed on the Raid Tab
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">e parameter</param>
        private void ToolStripButtonAdd_Click(object sender, EventArgs e)
        {
            if (toolStripTextBoxGuild.Text.Length == 0 || toolStripTextBoxRealm.Text.Length == 0)
            {
                MessageBox.Show("Error: Both Character Name and Realm need to be filled out.");
            }
            else
            {
                this.WaitCursor(true);

                GuildMember gm = this.GetCharacterInformation(toolStripTextBoxGuild.Text, toolStripTextBoxRealm.Text);

                if (gm != null)
                {
                    //todo: is this needed?
                    //  do we need to clear datasource, or can it just be updated

                    // Clear out the Grid Data Source to get it ready for the new data
                    dataGridViewRaidGroup.DataSource = null;

                    // Add new character to Raid
                    this.raidGroup.RaidGroup.Add(gm);
                                        
                    // refresh grid data
                    dataGridViewRaidGroup.DataSource = this.raidGroup.RaidGroup;

                    // refresh the grid data since it's been changed
                    this.UpdateRaidGrid();
                }

                this.WaitCursor(false);
            }
        }

        /// <summary>
        /// Grabs all the information about 1 character
        /// </summary>
        /// <param name="name">string of the name of the character to get the information about</param>
        /// <param name="realm">string of the realm of the character to get the information about</param>
        /// <returns>a Guild Member type variable</returns>
        private GuildMember GetCharacterInformation(string name, string realm)
        {
            // This is the Web Site to get the character info from...
            // http://us.battle.net/api/wow/character/Thrall/Purdee?fields=items,professions,talents
            GetCharacterInfo charInfo = new GetCharacterInfo();
            GuildMember gm = new GuildMember();

            if (charInfo.CollectFullData(this.URLWowAPI + "character/" + realm + "/" + name + "?fields=items,professions,talents"))
            {
                // success!

                // check for actual data now...
                if (charInfo.Guildie.Name != null)
                {
                    // Fill out the data grid with the data we collected                
                    gm = charInfo.Guildie;

                    // Add on the realm
                    gm.Realm = realm;
                }
                else
                {
                    // no character was matched!
                    Logging.Log(string.Format("Failed to get information about {0}", name));
                    gm = null;
                }
            }
            else
            {
                // Fail!  Save all errors until the end!
                Logging.Log(string.Format("Failed to get information about {0}", name));
                gm = null;
            }

            return gm;
        }

        /// <summary>
        /// Refreshes the raid group data - 
        /// TODO: remove me
        /// 
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">e parameter</param>
        private void ButtonRaidGroupRefresh_Click(object sender, EventArgs e)
        {
            this.WaitCursor(true);

            try
            {
                foreach (GuildMember oldMember in this.raidGroup.RaidGroup)
                {
                    GuildMember gm = this.GetCharacterInformation(oldMember.Name, oldMember.Realm);

                    if (gm != null)
                    {
                        // Success! Now we have the new info
                        bool success = true;

                        // Check the spec first... if spec is different then first 
                        //  ask if the update should happen
                        // This prevents from losing primary spec data
                        if (oldMember.Spec != gm.Spec)
                        {
                            if (MessageBox.Show(string.Format("The spec for {0} has changed from {1} to {2}.\n  Are you sure that you want to update?", oldMember.Name, oldMember.Spec, gm.Spec), "Spec Change", MessageBoxButtons.YesNo) == DialogResult.No)
                            {
                                success = false;
                            }
                        }

                        if (success)
                        {
                            // Compare the new info with the old info
                            //  And make updates as needed...

                            // Level
                            if (oldMember.Level != gm.Level)
                            {
                                oldMember.Level = gm.Level;
                            }
                            else
                            {
                                oldMember.ClearFlags();
                                gm.ClearFlags();
                            }

                            // Achievement Points
                            if (oldMember.AchievementPoints != gm.AchievementPoints)
                            {
                                oldMember.AchievementPoints = gm.AchievementPoints;
                            }

                            // Equiped iLevel
                            if (oldMember.EquipediLevel != gm.EquipediLevel)
                            {
                                oldMember.EquipediLevel = gm.EquipediLevel;
                            }
                            else
                            {
                                oldMember.ClearEquipItemLevelFlag();
                                gm.ClearEquipItemLevelFlag();
                            }

                            // Max iLevel
                            if (oldMember.MaxiLevel != gm.MaxiLevel)
                            {
                                oldMember.MaxiLevel = gm.MaxiLevel;
                            }
                            else
                            {
                                oldMember.ClearMaxItemLevelFlag();
                                gm.ClearMaxItemLevelFlag();
                            }

                            // Spec
                            if (oldMember.Spec != gm.Spec)
                            {
                                oldMember.Spec = gm.Spec;
                            }

                            // Role
                            if (oldMember.Role != gm.Role)
                            {
                                oldMember.Role = gm.Role;
                            }

                            // ItemAudit - always update, just in case
                            oldMember.ItemAudits = gm.ItemAudits;

                            // Fill out the last Update time with the current time and date
                            oldMember.LastUpdated = DateTime.Now;
                        }
                    }
                }

                // Clear out the Grid Data Source to get it ready for the new data
                dataGridViewRaidGroup.DataSource = null;

                // refresh grid data
                dataGridViewRaidGroup.DataSource = this.raidGroup.RaidGroup;

                // refresh the grid data since it's been changed
                this.UpdateRaidGrid();
            }
            catch (Exception ex)
            {
                Logging.Log("Error: " + ex.Message);
            }

            this.WaitCursor(false);
        }

        /// <summary>
        /// Bring up the individual character's Armory info and Gear Score
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">e parameter</param>
        private void DataGridViewRaidGroup_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridViewCell currentCell;

            // Get the current cell.
            currentCell = dataGridViewRaidGroup.CurrentCell;

            // Get the cell's current row
            int currentRow = currentCell.RowIndex;

            // First check to make sure the vars are passed in
            if (textBoxRealm.Text.Length == 0)
            {
                MessageBox.Show("Error: Please fill out the Realm.");
            }
            else if (currentRow < 0)
            {
                MessageBox.Show("Error: No row was selected.");
            }
            else
            {
                this.Cursor = Cursors.WaitCursor;

                // Create the new form to be used
                FormItemAudit charAudit = new FormItemAudit();

                // Get the character data
                GuildMember character = (GuildMember)this.raidGroup.RaidGroup[currentRow];

                // Pass the Data to the Form
                if (charAudit.PassData(character))
                {
                    // Show the new form
                    charAudit.Show();
                }
                else
                {
                    Logging.DisplayError("Can't Audit Character: " + character.Name);
                }
            }

            this.Cursor = Cursors.Default;
        }

        /// <summary>
        /// Sort the grid according to the column clicked
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">e parameter</param>
        private void DataGridViewRaidGroup_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (!(Control.ModifierKeys == Keys.Shift || Control.ModifierKeys == Keys.Control) &&
                e.Button == MouseButtons.Left && e.Clicks == 1)
            {
                // This was a double click on a column header - Sort it!
                DataGridViewColumn newColumn = dataGridViewRaidGroup.Columns[e.ColumnIndex];
                string sorting = newColumn.Name;

                ListSortDirection direction = ListSortDirection.Ascending;

                if (newColumn.HeaderCell.SortGlyphDirection == SortOrder.Ascending)
                {
                    direction = ListSortDirection.Descending;
                    sorting += " DESC";
                }
                else
                {
                    sorting += " ASC";
                }

                // Sort the selected column.                
                this.raidGroup.RaidGroup.Sort(new ObjectComparer(sorting, false));
                dataGridViewRaidGroup.DataSource = this.raidGroup.RaidGroup;
                dataGridViewRaidGroup.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = direction == ListSortDirection.Ascending ? SortOrder.Ascending : SortOrder.Descending;
                this.UpdateRaidGrid();
            }
        }

        /// <summary>
        /// The sort compare for the RaidGroup
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">e parameter</param>
        private void DataGridViewRaidGroup_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            // Try to sort based on the cells in the current column.
            e.SortResult = string.Compare(e.CellValue1.ToString(), e.CellValue2.ToString());

            // If the cells are equal, sort based on the ID column. 
            if (e.SortResult == 0 && e.Column.Name != "ID")
            {
                e.SortResult = string.Compare(
                    dataGridViewRaidGroup.Rows[e.RowIndex1].Cells[e.Column.Index].Value.ToString(),
                    dataGridViewRaidGroup.Rows[e.RowIndex2].Cells[e.Column.Index].Value.ToString());
            }

            e.Handled = true;
        }

        /// <summary>
        /// Update the raid grid
        /// </summary>
        private void UpdateRaidGrid()
        {
            // Update the grid back color for the rows to white
            dataGridViewRaidGroup.RowsDefaultCellStyle.BackColor = Color.White;
            dataGridViewRaidGroup.AutoResizeColumns();
            dataGridViewRaidGroup.AutoResizeRows();

            int count = 0;
            int ilvl = 0;

            // Now check to see if the Item Level has changed and if their level has changed
            for (int i = 0; i < dataGridViewRaidGroup.Rows.Count; i++)
            {
                // Get the guild member
                GuildMember gm = (GuildMember)this.raidGroup.RaidGroup[i];

                // Check for iLevel Changes
                if (gm.IsItemLevelChanged())
                {
                    dataGridViewRaidGroup.Rows[count].Cells[5].Style.BackColor = Color.Aquamarine;
                    dataGridViewRaidGroup.Rows[count].Cells[6].Style.BackColor = Color.Aquamarine;
                }
                else
                {
                    dataGridViewRaidGroup.Rows[count].Cells[5].Style.BackColor = Color.White;
                    dataGridViewRaidGroup.Rows[count].Cells[6].Style.BackColor = Color.White;
                }

                // Check for Level Changes
                if (gm.IsLevelChanged())
                {
                    dataGridViewRaidGroup.Rows[count].Cells[1].Style.BackColor = Color.Aquamarine;
                }
                else
                {
                    dataGridViewRaidGroup.Rows[count].Cells[1].Style.BackColor = Color.White;
                }

                // Sum up the Equipped iLevel
                if (dataGridViewRaidGroup.Rows[count].Cells[6].Value != null)
                {
                    ilvl += Convert.ToInt32(dataGridViewRaidGroup.Rows[count].Cells[6].Value);
                }

                count++;
            }

            // Update the AVE Equipped iLevel of the Raid Team
            if (count > 0)
            {
                // Setup the label
                labelRaidTab.Text = "Average Raid Team iLevel = " + Convert.ToInt32(Convert.ToDecimal(ilvl) / Convert.ToDecimal(count)).ToString() + " for " + this.raidGroup.RaidGroup.Count.ToString() + " total characters";
            }

            // refresh the grid data since it's been changed
            dataGridViewRaidGroup.Refresh();
        }

        #endregion

        #region " Raid Loot Drop Tab "

        /// <summary>
        /// Fires when the tool strip combo box in the Raid Loot tab is changed
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">e parameter</param>

        private void toolStripComboBoxPickRaid_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Clear out the results first
            dataGridViewRaidLootDrop.DataSource = null;
            toolStripComboBoxPickBoss.Items.Clear();

            if (this.raidLoot.ContainsKey(toolStripComboBoxPickRaid.SelectedItem.ToString()))
            {
                // Fill out bosses now
                foreach (string value in this.raidLoot[toolStripComboBoxPickRaid.SelectedItem.ToString()].Keys)
                {
                    toolStripComboBoxPickBoss.Items.Add(value);
                }
            }
            else
            {
                MessageBox.Show("No data found for this raid.");
            }
        }

        /// <summary>
        /// When a boss is selected, go through his entire loot table and compare with everyone in the raid group 
        ///   and figure out how needs which item
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">e parameter</param>
        private void toolStripComboBoxPickBoss_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.GetLootResults();
        }

        #endregion

        #region " Raid Loot Drop Results "

        /// <summary>
        /// Checks what raid and boss is selected, and then creates a loot table accordingly
        /// </summary>
        private void GetLootResults()
        {
            this.WaitCursor(true);

            // Clear out the results first
            dataGridViewRaidLootDrop.DataSource = null;

            // Create data table for all the new data
            DataTable loot = new DataTable();

            // Add Columns
            loot.Columns.Add("Upgrade");
            loot.Columns[0].DataType = typeof(int);
            loot.Columns.Add("CharacterName");
            loot.Columns.Add("ItemId");
            loot.Columns.Add("ItemName");
            loot.Columns.Add("ItemSlot");
            loot.Columns.Add("ItemILevel");
            loot.Columns.Add("OldItemILevel");

            int[] itemIds = null;

            // Fill out the results
            if (this.raidLoot[toolStripComboBoxPickRaid.SelectedItem.ToString()].ContainsKey(toolStripComboBoxPickBoss.SelectedItem.ToString()))
            {
                itemIds = this.raidLoot[toolStripComboBoxPickRaid.SelectedItem.ToString()][toolStripComboBoxPickBoss.SelectedItem.ToString()];
            }
            else
            {
                MessageBox.Show("No data found for this raid boss.");
            }

            // Remove focus from drop down so middle mouse wheel doesn't change accidently
            this.dataGridViewRaidLootDrop.Focus();

            if (itemIds != null && itemIds.Length > 0)
            {
                // Go through all item ids
                foreach (int itemId in itemIds)
                {
                    ItemInfo item = null;

                    item = Items.GetItem(itemId);

                    if (item != null)
                    {
                        // Now we have the item in the item cache

                        // Check against each member in the Raid Group
                        foreach (GuildMember gm in this.raidGroup.RaidGroup)
                        {
                            /*   Here are the different Classes:
                             * Death Knight
                             * Druid
                             * Hunter
                             * Mage
                             * Monk
                             * Paladin
                             * Priest                                
                             * Rogue
                             * Shaman
                             * Warlock
                             * Warrior 
                             */
                            string charName = string.Empty;

                            /* TODO:
                            // Change all references to a class to use the Converter.WoWClass.*
                            //  Ex: Converter.WoWClass.Hunter.ToString()
                            // Change all references to a role to use the Converter.WoWRole.*
                            //  Ex: Converter.WoWRole.DPS.ToString()
                            // Change all references to a Spec to use the Converter.WoWSpecs.*
                            //  Ex: Converter.WoWSpecs.Survival.ToString()
                            // **** OR ****
                            // We can make a member function in the GuildMember class like:
                            //  Ex: gm.IsClassHunter()
                            //  Ex: gm.IsRoleHealer()
                            //  Ex: gm.IsSpecSurvival()
                            // **** OR ****
                            //  EX: gm.Class == Converter.WoWClass.Hunter
                             */

                            // can this member use it?
                            if (Converter.ConvertItemClass(item.ItemClass) == "Armor")
                            {
                                // Armor
                                if (Converter.ConvertItemSubClass(item.ItemClass, item.ItemSubClass) == "Micellaneous")
                                {
                                    // Trinkets
                                    if (Converter.ConvertInventoryType(item.InventoryType) == "trinket")
                                    {
                                        string test = item.Tooltip;

                                        // It's a trinket and not sure who it should go to...
                                        //  So for now, we're going to hard code this by name
                                        if (item.Name == "Purified Bindings of Immerseus" && gm.Role == Converter.WoWRole.DPS.ToString() && gm.UsesIntellect)
                                        {
                                            charName = gm.Name;
                                        }
                                        else if (item.Name == "Rook's Unlucky Talisman" && gm.Role == "TANK")
                                        {
                                            charName = gm.Name;
                                        }
                                        else if (item.Name == "Fusion-Fire Core" && gm.Role == Converter.WoWRole.DPS.ToString() && gm.UsesStrength)
                                        {
                                            charName = gm.Name;
                                        }
                                        else if (item.Name == "Assurance of Consequence" && gm.Role == Converter.WoWRole.DPS.ToString() && gm.UsesAgility)
                                        {
                                            charName = gm.Name;
                                        }
                                        else if (item.Name == "Prismatic Prison of Pride" && gm.Role == "HEALING")
                                        {
                                            charName = gm.Name;
                                        }
                                        else if (item.Name == "Evil Eye of Galakras" && gm.Role == Converter.WoWRole.DPS.ToString() && gm.UsesStrength)
                                        {
                                            charName = gm.Name;
                                        }
                                        else if (item.Name == "Juggernaut's Focusing Crystal" && gm.Role == "TANK")
                                        {
                                            charName = gm.Name;
                                        }
                                        else if (item.Name == "Haromm's Talisman" && gm.Role == Converter.WoWRole.DPS.ToString() && gm.UsesAgility)
                                        {
                                            charName = gm.Name;
                                        }
                                        else if (item.Name == "Kardris' Toxic Totem" && gm.Role == Converter.WoWRole.DPS.ToString() && gm.UsesIntellect)
                                        {
                                            charName = gm.Name;
                                        }
                                        else if (item.Name == "Nazgrim's Burnished Insignia" && gm.Role == "HEALING")
                                        {
                                            charName = gm.Name;
                                        }
                                        else if (item.Name == "Frenzied Crystal of Rage" && gm.Role == Converter.WoWRole.DPS.ToString() && gm.UsesIntellect)
                                        {
                                            charName = gm.Name;
                                        }
                                        else if (item.Name == "Vial of Living Corruption" && gm.Role == "TANK")
                                        {
                                            charName = gm.Name;
                                        }
                                        else if (item.Name == "Sigil of Rampage" && gm.Role == Converter.WoWRole.DPS.ToString() && gm.UsesIntellect)
                                        {
                                            charName = gm.Name;
                                        }
                                        else if (item.Name == "Thok's Tail Tip" && gm.Role == Converter.WoWRole.DPS.ToString() && gm.UsesStrength)
                                        {
                                            charName = gm.Name;
                                        }
                                        else if (item.Name == "Thok's Acid-Grooved Tooth" && gm.Role == "HEALING")
                                        {
                                            charName = gm.Name;
                                        }
                                        else if (item.Name == "Dysmorphic Samophlange of Discontinuity" && gm.Role == "HEALING")
                                        {
                                            charName = gm.Name;
                                        }
                                        else if (item.Name == "Ticking Ebon Detonator" && gm.Role == Converter.WoWRole.DPS.ToString() && gm.UsesAgility)
                                        {
                                            charName = gm.Name;
                                        }
                                        else if (item.Name == "Skeer's Bloodsoaked Talisman" && gm.Role == Converter.WoWRole.DPS.ToString() && gm.UsesStrength)
                                        {
                                            charName = gm.Name;
                                        }
                                        else if (item.Name == "Black Blood of Y'Shaarj" && gm.Role == Converter.WoWRole.DPS.ToString() && gm.UsesIntellect)
                                        {
                                            charName = gm.Name;
                                        }
                                        else if (item.Name == "Curse of Hubris" && gm.Role == "TANK")
                                        {
                                            charName = gm.Name;
                                        }
                                    }
                                    else if (item.HasIntellect())
                                    {
                                        // Neck or Rings
                                        if (item.HasSpirit())
                                        {
                                            // Healer Classes
                                            if (gm.Role == "HEALING" || (gm.Role == Converter.WoWRole.DPS.ToString() && (gm.Spec == "Balance" || gm.Spec == "Shadow" || gm.Spec == "Elemental")))
                                            {
                                                charName = gm.Name;
                                            }
                                        }
                                        else
                                        {
                                            if ((!item.HasHit() && gm.Role == "HEALING") ||
                                                (gm.Role == Converter.WoWRole.DPS.ToString() && (gm.Spec == "Balance" || gm.Class == "Mage" || gm.Class == "Shadow" || gm.Spec == "Elemental" || gm.Class == "Warlock")))
                                            {
                                                charName = gm.Name;
                                            }
                                        }
                                    }
                                    else if (item.HasTankStats())
                                    {
                                        if (gm.Role == "TANK")
                                        {
                                            charName = gm.Name;
                                        }
                                    }
                                    else if (item.HasStrength())
                                    {
                                        if ((gm.Class == "Paladin" && gm.Spec != "Holy") || gm.Class == "Warrior" || gm.Class == "Death Knight")
                                        {
                                            charName = gm.Name;
                                        }
                                    }
                                    else if (item.HasAgility())
                                    {
                                        if ((gm.Class == "Monk" && gm.Role != "HEALING") || gm.Class == "Rogue" || gm.Class == "Hunter" || (gm.Class == "Druid" && (gm.Spec == "Feral" || gm.Spec == "Guardian")) || (gm.Class == "Shaman" && gm.Spec == "Enhancement"))
                                        {
                                            charName = gm.Name;
                                        }
                                    }
                                }
                                else if (Converter.ConvertItemSubClass(item.ItemClass, item.ItemSubClass) == "Plate")
                                {
                                    if (item.HasIntellect() && gm.Class == "Paladin" && gm.Spec == "Holy")
                                    {
                                        charName = gm.Name;
                                    }
                                    else if (item.HasTankStats())
                                    {
                                        if (gm.Role == "TANK" && (gm.Class == "Paladin" || gm.Class == "Warrior" || gm.Class == "Death Knight"))
                                        {
                                            charName = gm.Name;
                                        }
                                    }
                                    else if (!item.HasIntellect() && ((gm.Class == "Paladin" && gm.Spec != "Holy") || gm.Class == "Warrior" || gm.Class == "Death Knight"))
                                    {
                                        charName = gm.Name;
                                    }
                                }
                                else if (Converter.ConvertItemSubClass(item.ItemClass, item.ItemSubClass) == "Mail")
                                {
                                    if (item.HasIntellect() && gm.Class == "Shaman" && (gm.Spec == "Elemental" || gm.Spec == "Restoration"))
                                    {
                                        charName = gm.Name;
                                    }
                                    else if (!item.HasIntellect() && ((gm.Class == "Shaman" && gm.Spec == "Enhancement") || gm.Class == "Hunter"))
                                    {
                                        charName = gm.Name;
                                    }
                                }
                                else if (Converter.ConvertItemSubClass(item.ItemClass, item.ItemSubClass) == "Leather")
                                {
                                    if (item.HasIntellect() && ((gm.Class == "Druid" && (gm.Spec == "Balance" || gm.Spec == "Restoration")) || (gm.Class == "Monk" && gm.Role == "HEALING")))
                                    {
                                        charName = gm.Name;
                                    }
                                    else if (!item.HasIntellect() && (gm.Class == "Rogue" || (gm.Class == "Druid" && (gm.Spec == "Guardian" || gm.Spec == "Feral")) || (gm.Class == "Monk" && gm.Role != "HEALING")))
                                    {
                                        charName = gm.Name;
                                    }
                                }
                                else if (Converter.ConvertItemSubClass(item.ItemClass, item.ItemSubClass) == "Cloth")
                                {
                                    // check to see if it's a back slot first... very different checks...
                                    if (Converter.ConvertInventoryType(item.InventoryType) == "back")
                                    {
                                        if (item.HasIntellect())
                                        {
                                            if (item.HasSpirit())
                                            {
                                                // Healer Classes
                                                if (gm.Role == "HEALING" || (gm.Role == Converter.WoWRole.DPS.ToString() && (gm.Spec == "Balance" || gm.Spec == "Shadow" || gm.Spec == "Elemental")))
                                                {
                                                    charName = gm.Name;
                                                }
                                            }
                                            else
                                            {
                                                if ((!item.HasHit() && gm.Role == "HEALING") ||
                                                    (gm.Role == Converter.WoWRole.DPS.ToString() && (gm.Spec == "Balance" || gm.Class == "Mage" || gm.Class == "Shadow" || gm.Spec == "Elemental" || gm.Class == "Warlock")))
                                                {
                                                    charName = gm.Name;
                                                }
                                            }
                                        }
                                        else if (item.HasTankStats())
                                        {
                                            if (gm.Role == "TANK")
                                            {
                                                charName = gm.Name;
                                            }
                                        }
                                        else if (item.HasStrength())
                                        {
                                            if ((gm.Class == "Paladin" && gm.Spec != "Holy") || gm.Class == "Warrior" || gm.Class == "Death Knight")
                                            {
                                                charName = gm.Name;
                                            }
                                        }
                                        else if (item.HasAgility())
                                        {
                                            if ((gm.Class == "Monk" && gm.Role != "HEALING") || gm.Class == "Rogue" || gm.Class == "Hunter" || (gm.Class == "Druid" && (gm.Spec == "Feral" || gm.Spec == "Guardian")) || (gm.Class == "Shaman" && gm.Spec == "Enhancement"))
                                            {
                                                charName = gm.Name;
                                            }
                                        }
                                    }
                                    else if (item.HasSpirit() && (gm.Role == "HEALING" && gm.Class == "Priest"))
                                    {
                                        // it's a regular non-back, cloth piece
                                        charName = gm.Name;
                                    }
                                    else if (!item.HasSpirit() && (gm.Class == "Mage" || gm.Class == "Warlock" || (gm.Class == "Priest" && gm.Spec == "Shadow")))
                                    {
                                        // it's a regular non-back, cloth piece
                                        charName = gm.Name;
                                    }
                                }
                                else if (Converter.ConvertItemSubClass(item.ItemClass, item.ItemSubClass) == "Shield")
                                {
                                    if (item.HasIntellect())
                                    {
                                        if ((gm.Class == "Paladin" && gm.Spec == "Holy") || (gm.Class == "Shaman" && (gm.Spec == "Elemental" || gm.Spec == "Restoration")))
                                        {
                                            // Holy Paladin or Elemental Shaman or Restoration Shaman
                                            charName = gm.Name;
                                        }
                                    }
                                    else
                                    {
                                        if (gm.Role == "TANK" && (gm.Class == "Paladin" || gm.Class == "Warrior"))
                                        {
                                            charName = gm.Name;
                                        }
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Armor [" + item.ItemSubClass + "] not found!");
                                }
                            }
                            else if (Converter.ConvertItemClass(item.ItemClass) == "Weapon")
                            {
                                // Weapon
                                string weapon = Converter.ConvertItemSubClass(item.ItemClass, item.ItemSubClass);

                                if (weapon == "1 Axe")
                                {
                                    // (gm.Class == "Death Knight" || gm.Class == "Monk" || gm.Class == "Paladin" || gm.Class == "Rogue" || gm.Class == "Shaman" || gm.Class == "Warrior")
                                    if (item.HasTankStats())
                                    {
                                        // STR vs AGI
                                        if (gm.Role == "TANK")
                                        {
                                            if (item.HasStrength() && (gm.Class == "Paladin" || gm.Class == "Death Knight" || gm.Class == "Warrior"))
                                            {
                                                charName = gm.Name;
                                            }
                                            else if (item.HasAgility() && gm.Class == "Monk")
                                            {
                                                charName = gm.Name;
                                            }
                                        }
                                    }
                                    else if (item.HasStrength())
                                    {
                                        if (gm.Class == "Death Knight" || (gm.Class == "Paladin" && gm.Role != "HEALING") || gm.Class == "Warrior")
                                        {
                                            charName = gm.Name;
                                        }
                                    }
                                    else if (item.HasAgility())
                                    {
                                        if ((gm.Class == "Monk" && gm.Role != "HEALING") || gm.Class == "Rogue" || (gm.Class == "Shaman" && gm.Spec == "Enhancement"))
                                        {
                                            charName = gm.Name;
                                        }
                                    }
                                    else if (item.HasIntellect())
                                    {
                                        if (item.HasSpirit() && gm.Role == "HEALING" && (gm.Class == "Paladin" || gm.Class == "Shaman" || gm.Class == "Monk"))
                                        {
                                            charName = gm.Name;
                                        }
                                        else if (item.HasHit() && gm.Role == Converter.WoWRole.DPS.ToString() && gm.Class == "Shaman")
                                        {
                                            charName = gm.Name;
                                        }
                                        else if ((gm.Class == "Monk" && gm.Role == "HEALING") || (gm.Class == "Paladin" && gm.Role == "HEALING") || (gm.Class == "Shaman" && (gm.Spec == "Elemental" || gm.Spec == "Restoration")))
                                        {
                                            charName = gm.Name;
                                        }
                                    }
                                }
                                else if (weapon == "2 Axe")
                                {
                                    if (item.HasStrength() && (gm.Class == "Death Knight" || (gm.Class == "Paladin" && gm.Role == Converter.WoWRole.DPS.ToString()) || (gm.Class == "Warrior" && gm.Role == Converter.WoWRole.DPS.ToString())))
                                    {
                                        charName = gm.Name;
                                    }
                                }
                                else if (weapon == "1 Mace")
                                {
                                    if (item.HasTankStats())
                                    {
                                        // STR vs AGI
                                        if (gm.Role == "TANK")
                                        {
                                            if (item.HasStrength() && (gm.Class == "Paladin" || gm.Class == "Death Knight" || gm.Class == "Warrior"))
                                            {
                                                charName = gm.Name;
                                            }
                                            else if (item.HasAgility() && (gm.Class == "Monk" || gm.Class == "Druid"))
                                            {
                                                charName = gm.Name;
                                            }
                                        }
                                    }
                                    else if (item.HasStrength())
                                    {
                                        if (gm.Class == "Death Knight" || (gm.Class == "Paladin" && gm.Role == "TANK") || (gm.Class == "Warrior" && gm.Spec == "Arms"))
                                        {
                                            charName = gm.Name;
                                        }
                                    }
                                    else if (item.HasAgility())
                                    {
                                        if ((gm.Class == "Monk" && gm.Role != "HEALING") || gm.Class == "Rogue" || (gm.Class == "Shaman" && gm.Spec == "Enhancement") || (gm.Class == "Druid" && (gm.Spec == "Feral" || gm.Spec == "Guardian")))
                                        {
                                            charName = gm.Name;
                                        }
                                    }
                                    else if (item.HasIntellect())
                                    {
                                        if (item.HasSpirit() && gm.Role == "HEALING" && (gm.Class == "Paladin" || gm.Class == "Shaman" || gm.Class == "Monk" || gm.Class == "Druid" || gm.Class == "Priest"))
                                        {
                                            charName = gm.Name;
                                        }
                                        else if (item.HasHit() && gm.Role == Converter.WoWRole.DPS.ToString() && (gm.Class == "Druid" || gm.Class == "Priest" || gm.Class == "Shaman"))
                                        {
                                            charName = gm.Name;
                                        }
                                        else if ((gm.Class == "Monk" && gm.Role == "HEALING") || (gm.Class == "Paladin" && gm.Role == "HEALING") || (gm.Class == "Shaman" && (gm.Spec == "Elemental" || gm.Spec == "Restoration")) || gm.Class == "Priest")
                                        {
                                            charName = gm.Name;
                                        }
                                    }
                                }
                                else if (weapon == "2 Mace")
                                {
                                    if (item.HasStrength())
                                    {
                                        if (item.HasTankStats())
                                        {
                                            if (gm.Role == "TANK" && gm.Class == "Death Knight")
                                            {
                                                charName = gm.Name;
                                            }
                                        }
                                        else if (gm.Role == Converter.WoWRole.DPS.ToString() && (gm.Class == "Death Knight" || gm.Class == "Paladin" || gm.Class == "Warrior"))
                                        {
                                            charName = gm.Name;
                                        }
                                    }
                                    else if (item.HasAgility() && gm.Class == "Druid" && gm.Role != "HEALING")
                                    {
                                        charName = gm.Name;
                                    }
                                }
                                else if (weapon == "Polearm")
                                {
                                    if (item.HasStrength() && (gm.Class == "Death Knight" || gm.Class == "Paladin" || gm.Class == "Warrior"))
                                    {
                                        charName = gm.Name;
                                    }
                                    else if (item.HasAgility() && (gm.Class == "Druid" || gm.Class == "Monk") && gm.Role != "HEALING")
                                    {
                                        charName = gm.Name;
                                    }
                                }
                                else if (weapon == "1 Sword")
                                {
                                    if (item.HasTankStats())
                                    {
                                        // STR vs AGI
                                        if (gm.Role == "TANK")
                                        {
                                            if (item.HasStrength() && (gm.Class == "Paladin" || gm.Class == "Death Knight" || gm.Class == "Warrior"))
                                            {
                                                charName = gm.Name;
                                            }
                                            else if (item.HasAgility() && gm.Class == "Monk")
                                            {
                                                charName = gm.Name;
                                            }
                                        }
                                    }
                                    else if (item.HasStrength())
                                    {
                                        if ((gm.Class == "Death Knight" && gm.Spec == "Frost") || (gm.Class == "Paladin" && gm.Role == "TANK") || (gm.Class == "Warrior" && gm.Spec != "Arms"))
                                        {
                                            charName = gm.Name;
                                        }
                                    }
                                    else if (item.HasAgility())
                                    {
                                        if ((gm.Class == "Monk" && gm.Role != "HEALING") || gm.Class == "Rogue")
                                        {
                                            charName = gm.Name;
                                        }
                                    }
                                    else if (item.HasIntellect())
                                    {
                                        if (item.HasSpirit() && gm.Role == "HEALING" && (gm.Class == "Paladin" || gm.Class == "Shaman" || gm.Class == "Monk"))
                                        {
                                            charName = gm.Name;
                                        }
                                        else if (item.HasHit() && gm.Role == Converter.WoWRole.DPS.ToString() && (gm.Class == "Mage" || gm.Class == "Warlock"))
                                        {
                                            charName = gm.Name;
                                        }
                                        else if ((gm.Class == "Monk" && gm.Role == "HEALING") || (gm.Class == "Paladin" && gm.Role == "HEALING") || gm.Class == "Mage" || gm.Class == "Warlock")
                                        {
                                            charName = gm.Name;
                                        }
                                    }
                                }
                                else if (weapon == "2 Sword")
                                {
                                    if (item.HasTankStats())
                                    {
                                        if (gm.Role == "TANK" && gm.Class == "Death Knight")
                                        {
                                            charName = gm.Name;
                                        }
                                    }
                                    else if (gm.Role == Converter.WoWRole.DPS.ToString() && ((gm.Class == "Death Knight" && gm.Spec != "Frost") || gm.Class == "Paladin" || gm.Class == "Warrior"))
                                    {
                                        charName = gm.Name;
                                    }
                                }
                                else if (weapon == "Staff")
                                {
                                    if (item.HasIntellect())
                                    {
                                        if (item.HasSpirit())
                                        {
                                            if (gm.Role == "HEALING" && ((gm.Class == "Druid" && gm.Spec == "Restoration") || (gm.Class == "Monk" && gm.Spec == "Mistweaver") || (gm.Class == "Priest" && gm.Spec != "Shadow") || (gm.Class == "Shaman" && gm.Spec == "Restoration")))
                                            {
                                                charName = gm.Name;
                                            }
                                            else if (gm.Role == Converter.WoWRole.DPS.ToString() && ((gm.Class == "Druid" && gm.Spec == "Balance") || (gm.Class == "Priest" && gm.Spec == "Shadow") || (gm.Class == "Shaman" && gm.Spec == "Elemental")))
                                            {
                                                charName = gm.Name;
                                            }
                                        }
                                        else if (item.HasHit())
                                        {
                                            if (gm.Role == Converter.WoWRole.DPS.ToString() && ((gm.Class == "Druid" && gm.Spec == "Balance") || gm.Class == "Mage" || (gm.Class == "Priest" && gm.Spec == "Shadow") || (gm.Class == "Shaman" && gm.Spec == "Elemental") || gm.Class == "Warlock"))
                                            {
                                                charName = gm.Name;
                                            }
                                        }
                                        else if ((gm.Class == "Druid" && (gm.Spec == "Balance" || gm.Spec == "Restoration")) || (gm.Class == "Monk" && gm.Spec == "Mistweaver") || gm.Class == "Mage" || gm.Class == "Priest" || (gm.Class == "Shaman" && (gm.Spec == "Elemental" || gm.Spec == "Restoration")) || gm.Class == "Warlock")
                                        {
                                            charName = gm.Name;
                                        }
                                    }
                                    else if (item.HasAgility() && ((gm.Class == "Druid" && (gm.Spec == "Guardian" || gm.Spec == "Feral")) || (gm.Class == "Monk" && (gm.Spec == "Brewmaster" || gm.Spec == "Windwalker"))))
                                    {
                                        charName = gm.Name;
                                    }
                                }
                                else if (weapon == "Fist Weapon")
                                {
                                    if (item.HasAgility() && ((gm.Class == "Druid" && (gm.Spec == "Feral" || gm.Spec == "Guardian")) || (gm.Class == "Monk" && gm.Role != "HEALING") || gm.Class == "Rogue" || (gm.Class == "Shaman" && gm.Spec == "Enhancement")))
                                    {
                                        charName = gm.Name;
                                    }
                                    else if (item.HasStrength() && gm.Class == "Warrior")
                                    {
                                        charName = gm.Name;
                                    }
                                }
                                else if (weapon == "Dagger")
                                {
                                    if (item.HasIntellect())
                                    {
                                        if (item.HasSpirit())
                                        {
                                            if (gm.Role == "HEALING" && ((gm.Class == "Druid" && gm.Spec == "Restoration") || gm.Class == "Priest" || (gm.Class == "Shaman" && gm.Spec == "Restoration")))
                                            {
                                                charName = gm.Name;
                                            }
                                            else if (gm.Role == Converter.WoWRole.DPS.ToString() && ((gm.Class == "Druid" && gm.Spec == "Balance") || gm.Class == "Priest" || (gm.Class == "Shaman" && gm.Spec == "Elemental")))
                                            {
                                                charName = gm.Name;
                                            }
                                        }
                                        else if (item.HasHit())
                                        {
                                            if (gm.Role == Converter.WoWRole.DPS.ToString() && ((gm.Class == "Druid" && gm.Spec == "Balance") || gm.Class == "Mage" || (gm.Class == "Priest" && gm.Spec == "Shadow") || (gm.Class == "Shaman" && gm.Spec == "Elemental") || gm.Class == "Warlock"))
                                            {
                                                charName = gm.Name;
                                            }
                                        }
                                        else if ((gm.Class == "Druid" && (gm.Spec == "Balance" || gm.Spec == "Restoration")) || gm.Class == "Mage" || gm.Class == "Priest" || (gm.Class == "Shaman" && (gm.Spec == "Elemental" || gm.Spec == "Restoration")) || gm.Class == "Warlock")
                                        {
                                            charName = gm.Name;
                                        }
                                    }
                                    else if (item.HasAgility() && ((gm.Class == "Druid" && (gm.Spec == "Guardian" || gm.Spec == "Feral")) || gm.Class == "Rogue" || (gm.Class == "Shaman" && gm.Spec == "Enhancement")))
                                    {
                                        charName = gm.Name;
                                    }
                                    else if (item.HasStrength() && gm.Class == "Warrior")
                                    {
                                        charName = gm.Name;
                                    }
                                }
                                else if (weapon == "Wand")
                                {
                                    if (item.HasIntellect())
                                    {
                                        if (item.HasSpirit())
                                        {
                                            if (gm.Class == "Priest")
                                            {
                                                charName = gm.Name;
                                            }
                                        }
                                        else if (item.HasHit())
                                        {
                                            if (gm.Class == "Mage" || gm.Class == "Warlock" || (gm.Class == "Priest" && gm.Spec == "Shadow"))
                                            {
                                                charName = gm.Name;
                                            }
                                        }
                                        else if (gm.Class == "Mage" || gm.Class == "Warlock" || gm.Class == "Priest")
                                        {
                                            charName = gm.Name;
                                        }
                                    }
                                }
                                else if (weapon == "Bow" || weapon == "Rifle" || weapon == "Thrown" || weapon == "Crossbow")
                                {
                                    if (gm.Class == Converter.WoWClass.Hunter.ToString())
                                    {
                                        charName = gm.Name;
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Weapon [" + weapon + "] not found!");
                                }
                            }
                            else if (Converter.ConvertItemClass(item.ItemClass) == "Miscellaneous")
                            {
                                // Check if this is an armor token
                                if (item.Name.StartsWith("Helm"))
                                {
                                    item.InventoryType = 1;
                                }
                                else if (item.Name.StartsWith("Shoulders"))
                                {
                                    item.InventoryType = 3;
                                }
                                else if (item.Name.StartsWith("Chest"))
                                {
                                    item.InventoryType = 5;
                                }
                                else if (item.Name.StartsWith("Gauntlets"))
                                {
                                    item.InventoryType = 10;
                                }
                                else if (item.Name.StartsWith("Leggings"))
                                {
                                    item.InventoryType = 7;
                                }

                                if (Converter.ConvertInventoryType(item.InventoryType) != "error")
                                {
                                    if (item.Name.EndsWith(" of the Cursed Conqueror") && (gm.Class == "Paladin" || gm.Class == "Priest" || gm.Class == "Warlock"))
                                    {
                                        charName = gm.Name;
                                    }
                                    else if (item.Name.EndsWith(" of the Cursed Protector") && (gm.Class == "Warrior" || gm.Class == "Hunter" || gm.Class == "Shaman" || gm.Class == "Monk"))
                                    {
                                        charName = gm.Name;
                                    }
                                    else if (item.Name.EndsWith(" of the Cursed Vanquisher") && (gm.Class == "Rogue" || gm.Class == "Death Knight" || gm.Class == "Mage" || gm.Class == "Druid"))
                                    {
                                        charName = gm.Name;
                                    }
                                }
                            }

                            // Can the row be added to data table?
                            if (!string.IsNullOrEmpty(charName))
                            {
                                string slot = Converter.ConvertInventoryType(item.InventoryType);
                                bool pass = false;
                                int ilvlOld = 0;
                                int ilvlNew = 0;

                                if (slot == "finger")
                                {
                                    pass |= gm.ItemAudits.ContainsKey("finger1");
                                    pass |= gm.ItemAudits.ContainsKey("finger2");
                                }
                                else if (slot == "trinket")
                                {
                                    pass |= gm.ItemAudits.ContainsKey("trinket1");
                                    pass |= gm.ItemAudits.ContainsKey("trinket2");
                                }
                                else if (slot == "mainHand")
                                {
                                    // Need to check that either a 2 hander is being used
                                    //   or 2 one handers, or 1 hand and off hand is equiped
                                    string weapon = Converter.ConvertItemSubClass(item.ItemClass, item.ItemSubClass);

                                    if (weapon == "1 Axe" || weapon == "1 Mace" || weapon == "1 Sword" || weapon == "Fist Weapon" || weapon == "Dagger" || weapon == "Wand")
                                    {
                                        // this is NOT a 2 handed weapon, check to make sure both hands are equiped

                                        // Look for 2 x 1 handed weapons
                                        int mainHands = gm.ItemAudits.Keys.Where(kv => kv == "mainHand").Count();

                                        if (mainHands == 2)
                                        {
                                            pass = gm.ItemAudits.ContainsKey(slot);
                                        }
                                        else if (gm.ItemAudits.ContainsKey("Shield") || gm.ItemAudits.ContainsKey("offHand"))
                                        {
                                            pass = gm.ItemAudits.ContainsKey(slot);
                                        }
                                    }
                                    else
                                    {
                                        pass = gm.ItemAudits.ContainsKey(slot);
                                    }
                                }
                                else
                                {
                                    pass = gm.ItemAudits.ContainsKey(slot);
                                }

                                // Did it pass?  Is it an upgrade?
                                if (pass)
                                {
                                    ilvlNew = item.ItemLevel;

                                    if (slot == "finger")
                                    {
                                        // Need to account for any ring duplications
                                        if (item.Name == gm.ItemAudits["finger1"].Name)
                                        {
                                            ilvlOld = gm.ItemAudits["finger1"].ItemLevel;
                                        }
                                        else if (item.Name == gm.ItemAudits["finger2"].Name)
                                        {
                                            ilvlOld = gm.ItemAudits["finger2"].ItemLevel;
                                        }
                                        else
                                        {
                                            ilvlOld = gm.ItemAudits["finger1"].ItemLevel > gm.ItemAudits["finger2"].ItemLevel ? gm.ItemAudits["finger2"].ItemLevel : gm.ItemAudits["finger1"].ItemLevel;
                                        }
                                    }
                                    else if (slot == "trinket")
                                    {
                                        // Need to account for any ring duplications
                                        if (item.Name == gm.ItemAudits["trinket1"].Name)
                                        {
                                            ilvlOld = gm.ItemAudits["trinket1"].ItemLevel;
                                        }
                                        else if (item.Name == gm.ItemAudits["trinket2"].Name)
                                        {
                                            ilvlOld = gm.ItemAudits["trinket2"].ItemLevel;
                                        }
                                        else
                                        {
                                            // upgrade!
                                            ilvlOld = gm.ItemAudits["trinket1"].ItemLevel > gm.ItemAudits["trinket2"].ItemLevel ? gm.ItemAudits["trinket2"].ItemLevel : gm.ItemAudits["trinket1"].ItemLevel;
                                        }
                                    }
                                    else if (slot == "mainHand" || slot == "offHand")
                                    {
                                        string weapon = Converter.ConvertItemSubClass(item.ItemClass, item.ItemSubClass);

                                        // Need to check for dual hands vs 2 handers vs one hand + off hand...
                                        if ((gm.Class == "Rogue" || (gm.Class == "Shaman" && gm.Spec == "Enhancement") || (gm.Class == "Warrior" && gm.Spec == "Fury") ||
                                            (gm.Class == "Druid" && (gm.Spec == "Guardian" || gm.Spec == "Feral")) || (gm.Class == "Monk" && (gm.Spec == "Brewmaster" ||
                                            gm.Spec == "Windwalker")) || (gm.Class == "Death Knight" && (gm.Spec == "Unholy" || gm.Spec == "Frost"))) &&
                                            (gm.ItemAudits["offHand"].ItemLevel > 0))
                                        {
                                            ilvlOld = gm.ItemAudits["mainHand"].ItemLevel > gm.ItemAudits["offHand"].ItemLevel ? gm.ItemAudits["offHand"].ItemLevel : gm.ItemAudits["mainHand"].ItemLevel;
                                        }
                                        else if (slot == "offHand" && gm.ItemAudits["offHand"].ItemLevel == 0)
                                        {
                                            int itemIdMainHand = gm.ItemAudits["mainHand"].Id;
                                            int slotType = 0;
                                            ItemInfo itemMainHand = null;

                                            itemMainHand = Items.GetItem(itemIdMainHand);

                                            if (itemMainHand != null)
                                            {
                                                slotType = itemMainHand.InventoryType;

                                                // if slot if offHand and currently doesn't have an offHand equipped
                                                //  and mainhand is a 2 hander... compare iLevel with mainHand
                                                if (slotType == 17 || slotType == 15 || slotType == 26)
                                                {
                                                    ilvlOld = gm.ItemAudits["mainHand"].ItemLevel;
                                                }
                                                else
                                                {
                                                    ilvlOld = gm.ItemAudits[Converter.ConvertInventoryType(item.InventoryType)].ItemLevel;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            ilvlOld = gm.ItemAudits[Converter.ConvertInventoryType(item.InventoryType)].ItemLevel;
                                        }
                                    }
                                    else
                                    {
                                        ilvlOld = gm.ItemAudits[Converter.ConvertInventoryType(item.InventoryType)].ItemLevel;
                                    }

                                    if (ilvlNew > ilvlOld)
                                    {
                                        // This is a definite upgrade
                                        DataRow dr = loot.NewRow();
                                        dr["Upgrade"] = ilvlNew - ilvlOld;
                                        dr["ItemId"] = item.Id;
                                        dr["ItemName"] = item.Name;
                                        dr["ItemSlot"] = slot;
                                        dr["CharacterName"] = charName;
                                        dr["ItemILevel"] = ilvlNew;
                                        dr["OldItemILevel"] = ilvlOld;
                                        loot.Rows.Add(dr);
                                    }
                                    else
                                    {
                                        // Check for a upgrade taking upgraded items into account
                                        int ilvlOriginal = ilvlOld;

                                        if (Converter.ConvertInventoryType(item.InventoryType) == "trinket")
                                        {
                                            // Get the original iLevel of both trinkets
                                            int testTrinket1 = Items.GetItem(gm.ItemAudits[Converter.ConvertInventoryType(item.InventoryType) + "1"].Id).ItemLevel;
                                            int testTrinket2 = Items.GetItem(gm.ItemAudits[Converter.ConvertInventoryType(item.InventoryType) + "2"].Id).ItemLevel;

                                            // Need to account for any trinket duplicates
                                            if (item.Name == gm.ItemAudits["trinket1"].Name)
                                            {
                                                if (item.ItemLevel > testTrinket1)
                                                {
                                                    ilvlOriginal = testTrinket1;
                                                }
                                            }
                                            else if (item.Name == gm.ItemAudits["trinket2"].Name)
                                            {
                                                if (item.ItemLevel > testTrinket2)
                                                {
                                                    ilvlOriginal = testTrinket2;
                                                }
                                            }
                                            else
                                            {
                                                // upgrade!
                                                ilvlOriginal = testTrinket1 > testTrinket2 ? testTrinket2 : testTrinket1;
                                            }
                                        }
                                        else if (Converter.ConvertInventoryType(item.InventoryType) == "finger")
                                        {
                                            int testRing1 = Items.GetItem(gm.ItemAudits[Converter.ConvertInventoryType(item.InventoryType) + "1"].Id).ItemLevel;
                                            int testRing2 = Items.GetItem(gm.ItemAudits[Converter.ConvertInventoryType(item.InventoryType) + "2"].Id).ItemLevel;

                                            // Need to account for any ring duplicates
                                            if (item.Name == gm.ItemAudits["finger1"].Name)
                                            {
                                                if (ilvlOriginal <= testRing1)
                                                {
                                                    ilvlOriginal = testRing1;
                                                }
                                            }
                                            else if (item.Name == gm.ItemAudits["finger2"].Name)
                                            {
                                                if (ilvlOriginal <= testRing2)
                                                {
                                                    ilvlOriginal = testRing2;
                                                }
                                            }
                                            else
                                            {
                                                ilvlOriginal = testRing1 > testRing2 ? testRing2 : testRing1;
                                            }
                                        }
                                        else
                                        {
                                            ilvlOriginal = Items.GetItem(gm.ItemAudits[Converter.ConvertInventoryType(item.InventoryType)].Id).ItemLevel;
                                        }

                                        // Now that we have both the item's base iLevel and the new item's base iLevel, 
                                        //  we can compare the two
                                        if (ilvlNew > ilvlOriginal)
                                        {
                                            // This is an upgrade according to the pre-upgraded item
                                            DataRow dr = loot.NewRow();
                                            dr["Upgrade"] = ilvlNew - ilvlOriginal;
                                            dr["ItemId"] = item.Id;
                                            dr["ItemName"] = item.Name;
                                            dr["ItemSlot"] = slot;
                                            dr["CharacterName"] = charName;
                                            dr["ItemILevel"] = ilvlNew;
                                            dr["OldItemILevel"] = ilvlOld;
                                            loot.Rows.Add(dr);
                                        }
                                        else
                                        {
                                            // Not an upgrade
                                            DataRow dr = loot.NewRow();
                                            dr["Upgrade"] = -1;
                                            dr["ItemId"] = item.Id;
                                            dr["ItemName"] = item.Name;
                                            dr["ItemSlot"] = slot;
                                            dr["CharacterName"] = "n/a";
                                            dr["ItemILevel"] = ilvlNew;
                                            dr["OldItemILevel"] = 0;
                                            loot.Rows.Add(dr);
                                        }
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("slot #" + item.InventoryType + "[" + slot + "] not found!");
                                }
                            }
                        }
                    }
                }
            }

            if (loot.Rows.Count > 0)
            {
                // find any items that weren't needed by anyone...
                if (itemIds != null && itemIds.Length > 0)
                {
                    // Go through all item ids
                    foreach (int itemId in itemIds)
                    {
                        if (loot.Select("upgrade > 0 and ItemId = " + itemId, "upgrade desc").Length == 0)
                        {
                            ItemInfo item = Items.GetItem(itemId);

                            DataRow dr = loot.NewRow();
                            dr["Upgrade"] = 0;
                            dr["ItemId"] = item.Id;
                            dr["ItemName"] = item.Name;
                            dr["ItemSlot"] = Converter.ConvertInventoryType(item.InventoryType);
                            dr["CharacterName"] = "Not Needed";
                            dr["ItemILevel"] = item.ItemLevel;
                            dr["OldItemILevel"] = 0;
                            loot.Rows.Add(dr);
                        }
                    }
                }

                if (loot.Select("upgrade >= 0", "upgrade desc").Length > 0)
                {
                    dataGridViewRaidLootDrop.DataSource = loot.Select("upgrade >= 0", "upgrade desc").CopyToDataTable();
                    dataGridViewRaidLootDrop.Columns["ItemId"].Visible = false;
                    dataGridViewRaidLootDrop.AutoResizeColumns();

                    foreach (DataGridViewRow row in dataGridViewRaidLootDrop.Rows)
                    {
                        int id = Convert.ToInt32(row.Cells["ItemId"].Value);
                        ItemInfo item = Items.GetItem(id);
                        row.Cells["ItemName"].ToolTipText = item.Tooltip;

                        if (Convert.ToInt32(row.Cells["Upgrade"].Value) == 0)
                        {
                            // color row grey
                            row.DefaultCellStyle.BackColor = Color.DimGray;
                        }
                    }
                }
            }

            if (dataGridViewRaidLootDrop.DataSource != null)
            {
                dataGridViewRaidLootDrop.Refresh();
            }

            this.WaitCursor(false);
        }

        #endregion

        #region " Raid Loot Grid Functions "

        /// <summary>
        /// Fires when a cell in the Data Grid View Raid Loot is double clicked
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">e parameter</param>
        private void DataGridViewRaidLootDrop_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // Check for a left double click
            if (e.Button == MouseButtons.Left)
            {
                // select the current row                
                int currentMouseOverRow = this.dataGridViewRaidLootDrop.HitTest(e.X, e.Y).RowIndex;
                int currentMouseOverColumn = this.dataGridViewRaidLootDrop.HitTest(e.X, e.Y).ColumnIndex;

                if (currentMouseOverRow > -1)
                {
                    string value = this.dataGridViewRaidLootDrop.Rows[currentMouseOverRow].Cells[currentMouseOverColumn].Value.ToString();

                    this.dataGridViewRaidLootDrop.ClearSelection();

                    foreach (DataGridViewRow row in dataGridViewRaidLootDrop.Rows)
                    {
                        if (row.Cells[currentMouseOverColumn].Value.ToString() == value)
                        {
                            row.Selected = true;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Fires when a cell in the Raid Loot data grid view needs a tool tip
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">e parameter</param>
        private void DataGridViewRaidLootDrop_CellToolTipTextNeeded(object sender, DataGridViewCellToolTipTextNeededEventArgs e)
        {
            if (e.ColumnIndex == 3 && e.RowIndex >= 0 && dataGridViewRaidLootDrop.Rows.Count > 0)
            {
                int id = Convert.ToInt32(dataGridViewRaidLootDrop.Rows[e.RowIndex].Cells["ItemId"].Value);
                ItemInfo item = Items.GetItem(id);
                dataGridViewRaidLootDrop.Rows[e.RowIndex].Cells["ItemName"].ToolTipText = item.Tooltip;
            }
            else if (e.ColumnIndex == 5 && e.RowIndex >= 0 && dataGridViewRaidLootDrop.Rows.Count > 0)
            {
                // TODO: Make this tooltip show the current item that would be replaced
                //int id = Convert.ToInt32(dataGridViewRaidLootDrop.Rows[e.RowIndex].Cells["ItemId"].Value);
                //ItemInfo item = Items.GetItem(id);
                //dataGridViewRaidLootDrop.Rows[e.RowIndex].Cells["ItemName"].ToolTipText = item.Tooltip;
            }
        }

        /// <summary>
        /// Fires when the Raid Loot data grid view data binding has been completed
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">e parameter</param>
        private void DataGridViewRaidLootDrop_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            DataGridViewCellStyle style = new DataGridViewCellStyle();
            style.Alignment = DataGridViewContentAlignment.MiddleRight;

            // Set the control types for all the rows in the grid.
            foreach (DataGridViewRow r in this.dataGridViewRaidLootDrop.Rows)
            {
                // Display a row count in the row header.
                r.HeaderCell.Value = (r.Index + 1).ToString();
                r.HeaderCell.Style = style;
                r.Resizable = DataGridViewTriState.False;
            }

            dataGridViewRaidLootDrop.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);
        }

        /// <summary>
        /// Fires when the Guild data grid view is ready for Post Paint
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">e parameter</param>
        private void DataGridViewGuildData_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            var grid = sender as DataGridView;
            var rowIdx = (e.RowIndex + 1).ToString();

            var centerFormat = new StringFormat()
            {
                // right alignment might actually make more sense for numbers
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            var headerBounds = new Rectangle(e.RowBounds.Left, e.RowBounds.Top, grid.RowHeadersWidth, e.RowBounds.Height);
            e.Graphics.DrawString(rowIdx, this.Font, SystemBrushes.ControlText, headerBounds, centerFormat);
        }

        /// <summary>
        /// Fires when the Raid data grid view is ready for Row Post Paint
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">e parameter</param>
        private void DataGridViewRaidGroup_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            var grid = sender as DataGridView;
            var rowIdx = "U";
            GuildMember character = (GuildMember)this.raidGroup.RaidGroup[e.RowIndex];

            if (character.Role == "DPS")
            {
                rowIdx = "D";
            }
            else if (character.Role == "HEALING")
            {
                rowIdx = "H";
            }
            else if (character.Role == "TANK")
            {
                rowIdx = "T";
            }

            var centerFormat = new StringFormat()
            {
                // right alignment might actually make more sense for numbers
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            var headerBounds = new Rectangle(e.RowBounds.Left, e.RowBounds.Top, grid.RowHeadersWidth, e.RowBounds.Height);
            e.Graphics.DrawString(rowIdx, this.Font, SystemBrushes.ControlText, headerBounds, centerFormat);
        }

        #endregion

        #region Tool Strip Menu

        /// <summary>
        /// Fires when the update character right click is pressed
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">e parameter</param>
        private void UpdateCharacterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Update just the selected rows of characters
            this.UpdateCharacters(this.dataGridViewGuildData.SelectedRows, true);
        }

        /// <summary>
        /// Updates a collection of characters with the current information
        /// </summary>
        /// <param name="chars">A collection of selected rows in a data grid view</param>
        /// <param name="guildUpdate">Flag if the update is in the guild or raid data grid view</param>
        private void UpdateCharacters(DataGridViewSelectedRowCollection chars, bool guildUpdate)
        {
            try
            {
                this.WaitCursor(true);

                foreach (DataGridViewRow row in chars)
                {
                    int currentRow = row.Index;
                    GuildMember oldMember = null;
                    GuildMember gm = null;

                    if (guildUpdate)
                    {
                        oldMember = (GuildMember)this.savedCharacters.SavedCharacters[currentRow];
                        gm = this.GetCharacterInformation(oldMember.Name, this.savedCharacters.Realm);
                    }
                    else
                    {
                        oldMember = (GuildMember)this.raidGroup.RaidGroup[currentRow];
                        gm = this.GetCharacterInformation(oldMember.Name, oldMember.Realm);
                    }

                    if (gm != null)
                    {
                        // Success! Now we have the new info
                        bool success = true;

                        // Check the spec first... if spec is different then first 
                        //  ask if the update should happen
                        // This prevents from losing primary spec data
                        if (oldMember.Spec != gm.Spec)
                        {
                            if (MessageBox.Show(string.Format("The spec for {0} has changed from {1} to {2}.\n  Are you sure that you want to update?", oldMember.Name, oldMember.Spec, gm.Spec), "Spec Change", MessageBoxButtons.YesNo) == DialogResult.No)
                            {
                                success = false;
                            }
                        }

                        if (success)
                        {
                            // Success! Now we have the new info
                            // Compare the new info with the old info
                            //  And make updates as needed...

                            // Level
                            if (oldMember.Level != gm.Level)
                            {
                                oldMember.Level = gm.Level;
                            }
                            else
                            {
                                oldMember.ClearFlags();
                                gm.ClearFlags();
                            }

                            // Achievement Points
                            if (oldMember.AchievementPoints != gm.AchievementPoints)
                            {
                                oldMember.AchievementPoints = gm.AchievementPoints;
                            }

                            // Equiped iLevel
                            if (oldMember.EquipediLevel != gm.EquipediLevel)
                            {
                                oldMember.EquipediLevel = gm.EquipediLevel;
                            }
                            else
                            {
                                oldMember.ClearEquipItemLevelFlag();
                                gm.ClearEquipItemLevelFlag();
                            }

                            // Max iLevel
                            if (oldMember.MaxiLevel != gm.MaxiLevel)
                            {
                                oldMember.MaxiLevel = gm.MaxiLevel;
                            }
                            else
                            {
                                oldMember.ClearMaxItemLevelFlag();
                                gm.ClearMaxItemLevelFlag();
                            }

                            // Spec
                            if (oldMember.Spec != gm.Spec)
                            {
                                oldMember.Spec = gm.Spec;
                            }

                            // Role
                            if (oldMember.Role != gm.Role)
                            {
                                oldMember.Role = gm.Role;
                            }

                            // ItemAudit - always update, just in case
                            oldMember.ItemAudits = gm.ItemAudits;

                            // Update the last updated time with the current date and time
                            oldMember.LastUpdated = DateTime.Now;
                        }
                    }
                }

                // refresh the grid data since it's been changed
                if (guildUpdate)
                {
                    this.UpdateGrid();
                }
                else
                {
                    this.UpdateRaidGrid();
                }
            }
            catch (Exception ex)
            {
                Logging.Log("Error: " + ex.Message);
            }

            this.WaitCursor(false);
        }

        /// <summary>
        /// Fires when the Refresh button is pressed on the tool strip
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">e parameter</param>
        private void ToolStripButtonRefresh_Click(object sender, EventArgs e)
        {
            // Find out what tab we're currently on
            if (this.tabControlWGO.SelectedTab.Text == "Guild Data")
            {
                // First check to make sure both Realm and Guild values have been filled out
                if (toolStripTextBoxGuild.Text != string.Empty && toolStripTextBoxGuild.Text != string.Empty)
                {
                    if (toolStripButtonRefresh.Text == "Refresh")
                    {
                        //Func<bool> ActionGetGuildMembers = () => GetGuildMembers();
                        //List<Task> tasks = new List<Task>();

                        // Start the task to get all the current guild members
                        //tasks.Add(Task<bool>.Factory.StartNew(ActionGetGuildMembers));

                        //await tasks;

                        //Task doingWork = GetGuildMembers();
                        //doingWork.ContineWith(OnWorkCompleted);

                        toolStripButtonRefresh.Enabled = false;

                        //Task<bool> longRunningTask = LongRunningTask();

                        // Call the method that runs asynchronously.
                        //bool result = await longRunningTask();

                        Func<bool> ActionLookupConsumer = () => GetGuildMembers();
                        List<Task> tasks = new List<Task>();

                        // Create the first task
                        tasks.Add(Task<bool>.Factory.StartNew(ActionLookupConsumer));

                        // Now create a task that will start after the first task completes
                        tasks.Add((tasks[0] as Task<bool>).ContinueWith(t => OnWorkCompleted()));

                        // other tests
                        //bool success = false;

                        //await tasks.Run;// (Task<bool>.Factory.StartNew(ActionLookupConsumer));
                        //Action action = new Action(GetGuildMembers());
                        //Task task = new Task((ActionLookupConsumer);

                        //toolStripButtonRefresh.Text = "Refresh";
                    }
                    else
                    {
                        // Cancel it
                    }
                }
            }
            else if (this.tabControlWGO.SelectedTab.Text == "Raid Data")
            {
            }
            else if (this.tabControlWGO.SelectedTab.Text == "Raid  Loot Drops")
            {
            }
        }
        
        /// <summary>
        /// todo
        /// </summary>
        private void OnWorkCompleted()
        {
            //toolStripButtonRefresh.Text = "Refresh";
            toolStripButtonRefresh.Enabled = true;
        }

        /// <summary>
        /// todo
        /// </summary>
        /// <returns></returns>
        public async Task<bool> LongRunningOperation() // assume we return an int from this long running operation 
        {
            bool result = false;

            await Task.Delay(1000); //1 seconds delay
            //await Task.Run( GetGuildMembers;
            return result;
        }

        /// <summary>
        /// 1.  Get the Entire Guild Roster first from: http://us.battle.net/api/wow/guild/Thrall/Secondnorth?fields=members
        /// 2.  Go through each guild member and pull out additional information from: http://us.battle.net/api/wow/character/Thrall/Purdee?fields=items,professions,talents
        /// </summary>
        /// <returns></returns>
        private bool GetGuildMembers()
        {
            // First get the entire Guild Roster
            //await Task.Run(Action
            
            Thread.Sleep(9000); //1 seconds delay

            try
            {
                // Reset the Counter
                //async.ReportProgress(0);

                // Get the guild information
                GetGuildInfo guildInfo = new GetGuildInfo();

                if (guildInfo.CollectData(this.URLWowAPI + @"guild/" + textBoxRealm.Text + @"/" + textBoxGuildName.Text + @"?fields=members"))
                {
                    // success! now check to see if a grid needs to be updated or if it's the first time used
                    if (this.savedCharacters.SavedCharacters.Count > 0)
                    {
                        // save the current data to a temp var
                        ArrayList temp = new ArrayList();
                        foreach (GuildMember m in this.savedCharacters.SavedCharacters)
                        {
                            temp.Add(m);
                        }

                        // erase the current data so we can start new
                        this.savedCharacters.SavedCharacters.Clear();

                        // Fill out the data grid with the data we collected
                        this.savedCharacters.SavedCharacters = guildInfo.Characters;

                        this.savedCharacters.Guild = textBoxGuildName.Text;
                        this.savedCharacters.Realm = textBoxRealm.Text;

                        foreach (GuildMember newmember in this.savedCharacters.SavedCharacters)
                        {
                            // now see if this is a new char or if level was updated
                            foreach (GuildMember oldmember in temp)
                            {
                                if (newmember.Name == oldmember.Name)
                                {
                                    // match!

                                    // now check to see if level or achievement points have been updated
                                    if (newmember.Level.CompareTo(oldmember.Level) == 0)
                                    {
                                        newmember.ClearFlags();
                                    }

                                    // always check to carry over the max iLevel value from old to new
                                    // since new will always be blank
                                    if (newmember.MaxiLevel == 0 && oldmember.MaxiLevel != 0)
                                    {
                                        newmember.MaxiLevel = oldmember.MaxiLevel;
                                        newmember.ClearMaxItemLevelFlag();
                                    }

                                    // always check to carry over the max iLevel value from old to new
                                    // since new will always be blank
                                    if (newmember.EquipediLevel == 0 && oldmember.EquipediLevel != 0)
                                    {
                                        newmember.EquipediLevel = oldmember.EquipediLevel;
                                        newmember.ClearEquipItemLevelFlag();
                                    }

                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        // First time collection of the guild...
                        //  Fill out the data grid with the data we collected
                        this.savedCharacters.SavedCharacters = guildInfo.Characters;
                    }
                }

                // Now get the individual Character data
                int count = 0;
                int total = this.savedCharacters.SavedCharacters.Count;
                ArrayList errors = new ArrayList();
                        
                try
                {
                    // Go through all the guild members
                    foreach (GuildMember gm in this.savedCharacters.SavedCharacters)
                    {
                        // Only check for Item Level for characters over level 10
                        //  Otherwise these characters won't be in the Armory
                        if (Convert.ToInt32(gm.Level) >= 10)
                        {
                            // This is the Web Site to get the character info from...
                            // http://us.battle.net/api/wow/character/Thrall/Purdee?fields=items,professions,talents                                    
                            GuildMember charInfo = this.GetCharacterInformation(gm.Name, this.savedCharacters.Realm);

                            if (charInfo != null)
                            {
                                // success!

                                // Fill out the data grid with the data we collected
                                gm.MaxiLevel = charInfo.MaxiLevel;
                                gm.EquipediLevel = charInfo.EquipediLevel;
                                gm.Profession1 = charInfo.Profession1;
                                gm.Profession2 = charInfo.Profession2;
                                gm.Spec = charInfo.Spec;
                                gm.Role = charInfo.Role;
                                gm.ItemAudits = charInfo.ItemAudits;

                                if (charInfo.AchievementPoints == 0)
                                {
                                    gm.AchievementPoints = gm.AchievementPoints;
                                }
                                else if (gm.AchievementPoints == 0)
                                {
                                    gm.AchievementPoints = charInfo.AchievementPoints;
                                }
                                else
                                {
                                    Logging.Log(string.Format("WARN: Old vs New Achievement points.  [{0}] vs [{1}].", gm.AchievementPoints, charInfo.AchievementPoints));
                                }
                            }
                            else
                            {
                                // Fail!  Save all errors until the end!
                                Logging.Log("      " + gm.Name + "\t\t" + gm.Level);
                            }
                        }

                        // Progress update
                        this.UpdateLabelMT(this.savedCharacters.SavedCharacters.Count.ToString() + " total characters - On Character #" + count);

                        // Progress update for Progress Bar
                        double tempNum = (double)count++ / (double)total * 100;
                        //todo: async.ReportProgress((int)tempNum);
                    }
                }
                catch (Exception ex)
                {
                    Logging.Log("Error: " + ex.Message);
                }

                // Data has been gathered now...
                //todo: async.ReportProgress(100);

                // re-sort by Item Level
                this.SortGridMT(this.basicGuildSort);

                // Stop the stopwatch
                this.sw.Stop();

                // Check to see if there were any errors while trying to gather data for a character
                if (errors.Count > 0)
                {
                    string errorMessage = "ERROR - Failed to parse some gear scores...\n   Couldn't retrieve information for the following characters:\n";

                    foreach (string str in errors)
                    {
                        errorMessage += str + "\n";
                    }

                    Logging.Log(errorMessage);
                }
            }
            catch (Exception ex)            
            {
                Logging.Log("  **ERROR: " + ex.Message);
            }

            return true;
        }

#endregion

        #region " Tool Strip Functions "

        /// <summary>
        /// Fires when the "Update this Character" tool strip menu item is clicked
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">e parameter</param>
        private void UpdateThisCharacterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.WaitCursor(true);

            try
            {
                foreach (DataGridViewRow row in this.dataGridViewRaidGroup.SelectedRows)
                {
                    int currentRow = row.Index;
                    GuildMember oldMember = (GuildMember)this.raidGroup.RaidGroup[currentRow];
                    GuildMember gm = this.GetCharacterInformation(oldMember.Name, oldMember.Realm);

                    if (gm != null)
                    {
                        // Success! Now we have the new info
                        bool success = true;

                        // Check the spec first... if spec is different then first 
                        //  ask if the update should happen
                        // This prevents from losing primary spec data
                        if (oldMember.Spec != gm.Spec)
                        {
                            if (MessageBox.Show(string.Format("The spec for {0} has changed from {1} to {2}.\n  Are you sure that you want to update?", oldMember.Name, oldMember.Spec, gm.Spec), "Spec Change", MessageBoxButtons.YesNo) == DialogResult.No)
                            {
                                success = false;
                            }
                        }

                        if (success)
                        {
                            // Success! Now we have the new info
                            // Compare the new info with the old info
                            //  And make updates as needed...

                            // Level
                            if (oldMember.Level != gm.Level)
                            {
                                oldMember.Level = gm.Level;
                            }
                            else
                            {
                                oldMember.ClearFlags();
                                gm.ClearFlags();
                            }

                            // Achievement Points
                            if (oldMember.AchievementPoints != gm.AchievementPoints)
                            {
                                oldMember.AchievementPoints = gm.AchievementPoints;
                            }

                            // Equiped iLevel
                            if (oldMember.EquipediLevel != gm.EquipediLevel)
                            {
                                oldMember.EquipediLevel = gm.EquipediLevel;
                            }
                            else
                            {
                                oldMember.ClearEquipItemLevelFlag();
                                gm.ClearEquipItemLevelFlag();
                            }

                            // Max iLevel
                            if (oldMember.MaxiLevel != gm.MaxiLevel)
                            {
                                oldMember.MaxiLevel = gm.MaxiLevel;
                            }
                            else
                            {
                                oldMember.ClearMaxItemLevelFlag();
                                gm.ClearMaxItemLevelFlag();
                            }

                            // Spec
                            if (oldMember.Spec != gm.Spec)
                            {
                                oldMember.Spec = gm.Spec;
                            }

                            // Role
                            if (oldMember.Role != gm.Role)
                            {
                                oldMember.Role = gm.Role;
                            }

                            // ItemAudit - always update, just in case
                            oldMember.ItemAudits = gm.ItemAudits;

                            // Update the last updated time with the current date and time
                            oldMember.LastUpdated = DateTime.Now;
                        }
                    }
                }

                // refresh the grid data since it's been changed
                this.UpdateRaidGrid();
            }
            catch (Exception ex)
            {
                Logging.Log("Error: " + ex.Message);
            }

            this.WaitCursor(false);
        }

        /// <summary>
        /// Fires when the "Move Character Up" tool strip menu item is clicked
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">e parameter</param>
        private void MoveCharacterUpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.dataGridViewRaidGroup.SelectedRows.Count > 0)
            {
                // check to see if the row can to be moved
                if (this.dataGridViewRaidGroup.SelectedRows[0].Index != 0)
                {
                    // get the row index that will be moved
                    int row = this.dataGridViewRaidGroup.SelectedRows[0].Index;
                    GuildMember gm = (GuildMember)this.raidGroup.RaidGroup[row];
                    this.raidGroup.RaidGroup.RemoveAt(row);
                    this.raidGroup.RaidGroup.Insert(row - 1, gm);

                    dataGridViewRaidGroup.ClearSelection();
                    dataGridViewRaidGroup.Rows[row - 1].Selected = true;

                    // refresh the grid data since it's been changed
                    this.UpdateRaidGrid();
                }
            }
        }

        /// <summary>
        /// Fires when the "Move Character Down" tool strip menu item is clicked
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">e parameter</param>
        private void MoveCharacterDownToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.dataGridViewRaidGroup.SelectedRows.Count > 0)
            {
                // check to see if the row can to be moved
                if (this.dataGridViewRaidGroup.SelectedRows[0].Index != this.dataGridViewRaidGroup.RowCount - 1)
                {
                    // get the row index that will be moved
                    int row = this.dataGridViewRaidGroup.SelectedRows[0].Index;
                    GuildMember gm = (GuildMember)this.raidGroup.RaidGroup[row];
                    this.raidGroup.RaidGroup.RemoveAt(row);
                    this.raidGroup.RaidGroup.Insert(row + 1, gm);

                    dataGridViewRaidGroup.ClearSelection();
                    dataGridViewRaidGroup.Rows[row + 1].Selected = true;

                    // refresh the grid data since it's been changed
                    this.UpdateRaidGrid();
                }
            }
        }

        /// <summary>
        /// Fires when the "Delete Character From Grid" tool strip menu item is clicked
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">e parameter</param>
        private void DeleteCharacterFromGridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Delete this character(s) from the Raid Group
            foreach (DataGridViewRow row in this.dataGridViewRaidGroup.SelectedRows)
            {
                // Remove the character selected
                this.raidGroup.RaidGroup.RemoveAt(row.Index);
            }

            // Clear out the Grid Data Source to get it ready for the new data
            dataGridViewRaidGroup.DataSource = null;

            // refresh grid data
            dataGridViewRaidGroup.DataSource = this.raidGroup.RaidGroup;

            // refresh the grid data since it's been changed
            this.UpdateRaidGrid();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        
        
        #endregion
    }
}
