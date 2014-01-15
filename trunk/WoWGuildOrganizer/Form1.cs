﻿
#region " Includes "
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
#endregion



namespace WoWGuildOrganizer
{
    public partial class Form1 : Form
    {
        #region " Variables "

        static public String URLWowAPI = @"http://us.battle.net/api/wow/";
        GuildMemberGroup SavedCharacters;
        RaidMemberGroup RaidGroup;
        private BackgroundWorker GetGuildInfoAsyncWorker = new BackgroundWorker();
        private StopWatch sw = new StopWatch();
        static public ItemCache Items;
        ArrayList ErrorLog = new ArrayList();
        ContextMenuStrip contextMenuCharacter;
        Dictionary<string, Dictionary<string, int[]>> RaidLoot = new Dictionary<string, Dictionary<string, int[]>>();

        #endregion

        #region " Constructor "

        public Form1()
        {
            InitializeComponent();

            // Load up the guild name textbox if it's been saved before
            if (!String.IsNullOrEmpty(Properties.Settings.Default.GuildName))
            {
                textBoxGuildName.Text = Properties.Settings.Default.GuildName;                
            }

            // Load up the realm name textbox if it's been saved before
            if (!String.IsNullOrEmpty(Properties.Settings.Default.Realm))
            {
                textBoxRealm.Text = Properties.Settings.Default.Realm;
                textBoxCharacterRealm.Text = textBoxRealm.Text;
            }

            // setup the data grid view
            SetupDataGridView();

            // init the struct that will store all the char info
            SavedCharacters = new GuildMemberGroup();
            
            // Attempt to load the last guild saved...
            //   check for a data file and if there is one, try to load it up
            try
            {
                if (File.Exists("SavedCharacters.dat"))
                {
                    //Open the file written above and read values from it.
                    Stream stream = File.Open("SavedCharacters.dat", FileMode.Open);
                    BinaryFormatter bformatter = new BinaryFormatter();

                    SavedCharacters = (GuildMemberGroup)bformatter.Deserialize(stream);
                    stream.Close();                    

                    label3.Text = SavedCharacters.SavedCharacters.Count.ToString() + " total characters";
                }
            }
            catch (Exception ex)
            {
                if (!ex.Message.StartsWith("Could not find file"))
                {
                    Log(String.Format("  **ERROR: ", ex.Message));
                    MessageBox.Show(String.Format("  **ERROR[LoadTempData]: {0}", ex.Message));
                }
            }

            // init the struct that will store all the raod info
            RaidGroup = new RaidMemberGroup();

            // Attempt to load the last raid saved...
            //   check for a data file and if there is one, try to load it up
            try
            {
                if (File.Exists("RaidGroup.dat"))
                {
                    //Open the file written above and read values from it.
                    Stream stream = File.Open("RaidGroup.dat", FileMode.Open);
                    BinaryFormatter bformatter = new BinaryFormatter();

                    RaidGroup = (RaidMemberGroup)bformatter.Deserialize(stream);
                    stream.Close();
                }
            }
            catch (Exception ex)
            {
                if (!ex.Message.StartsWith("Could not find file"))
                {
                    Log(String.Format("  **ERROR: ", ex.Message));
                    MessageBox.Show(String.Format("  **ERROR[LoadTempData]: {0}", ex.Message));
                }
            }

            // init the struct that will store all the char info
            Items = new ItemCache();

            // Attempt to load the last Item Cache saved...
            try
            {
                if (File.Exists("ItemCache.dat"))
                {
                    //Open the file written above and read values from it.
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
                    Log(String.Format("  **ERROR: ", ex.Message));
                    MessageBox.Show(String.Format("  **ERROR[LoadTempData]: {0}", ex.Message));
                }
            }

            // Create the Raid Loot Data
            CreateRaidLootData();

            // Add Raid Loot Drop Items
            foreach (string key in RaidLoot.Keys)
            {
                toolStripComboBoxRaidLootDropRaid.Items.Add(key);
            }

            // create context grid popup menu
            this.contextMenuCharacter = new ContextMenuStrip();
            this.contextMenuCharacter.Items.Add("Update this Character");
            this.contextMenuCharacter.Items.Add("Move Character Up");
            this.contextMenuCharacter.Items.Add("Move Character Down");
            this.contextMenuCharacter.Items.Add("Delete Character from Grid");            
            this.contextMenuCharacter.ItemClicked += this.contextMenuCharacter_ItemClicked;

            // Create a background worker thread that Searches and Reports Progress and Supports Cancellation            
            GetGuildInfoAsyncWorker.WorkerReportsProgress = true;
            GetGuildInfoAsyncWorker.WorkerSupportsCancellation = true;
            GetGuildInfoAsyncWorker.ProgressChanged += new ProgressChangedEventHandler(GetGuildInfoAsync_ProgressChanged);
            // can one of these be made for all controls so invoke won't have to be done?
            GetGuildInfoAsyncWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(GetGuildInfoAsync_RunWorkerCompleted);
            GetGuildInfoAsyncWorker.DoWork += new DoWorkEventHandler(GetGuildInfoAsync_DoWork);
        }

        #endregion

        #region " DataGrid Functions "

        /// <summary>
        /// 
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
            dataGridViewGuildData.RowHeadersVisible = false;

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
            dataGridViewRaidGroup.RowHeadersVisible = false;

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
            // Update the grid back color for the rows to white
            dataGridViewGuildData.RowsDefaultCellStyle.BackColor = Color.White;
            dataGridViewGuildData.AutoResizeColumns();
            dataGridViewGuildData.AutoResizeRows();

            // Now check to see if the Item Level has changed and if their level has changed
            Int32 Count = 0;
            foreach (GuildMember gm in SavedCharacters.SavedCharacters)                    
            {
                if (gm.IsItemLevelChanged())
                {
                    dataGridViewGuildData.Rows[Count].Cells[5].Style.BackColor = Color.Aquamarine;
                }
                else
                {
                    dataGridViewGuildData.Rows[Count].Cells[5].Style.BackColor = Color.White;
                }

                if (gm.IsLevelChanged())
                {
                    dataGridViewGuildData.Rows[Count].Cells[1].Style.BackColor = Color.Aquamarine;
                }
                else
                {
                    dataGridViewGuildData.Rows[Count].Cells[1].Style.BackColor = Color.White;
                }

                if (gm.IsItemLevelChanged())
                {
                    dataGridViewGuildData.Rows[Count].Cells[6].Style.BackColor = Color.Aquamarine;
                }
                else
                {
                    dataGridViewGuildData.Rows[Count].Cells[6].Style.BackColor = Color.White;
                }

                if (dataGridViewGuildData.Rows[Count].Cells[7].Value != null)
                {
                    // Check to see when the ItemLevel was last updated
                    DateTime CheckAgain = (DateTime)dataGridViewGuildData.Rows[Count].Cells[7].Value;
                    if (DateTime.Now > CheckAgain.AddDays(7))
                    {
                        dataGridViewGuildData.Rows[Count].Cells[7].Style.BackColor = Color.Red;
                    }
                    else if (DateTime.Now > CheckAgain.AddDays(5))
                    {
                        dataGridViewGuildData.Rows[Count].Cells[7].Style.BackColor = Color.OrangeRed;
                    }
                    else if (DateTime.Now > CheckAgain.AddDays(3))
                    {
                        dataGridViewGuildData.Rows[Count].Cells[7].Style.BackColor = Color.Orange;
                    }
                    else if (DateTime.Now > CheckAgain.AddDays(2))
                    {
                        dataGridViewGuildData.Rows[Count].Cells[7].Style.BackColor = Color.Yellow;
                    }
                    else if (DateTime.Now > CheckAgain.AddDays(1))
                    {
                        dataGridViewGuildData.Rows[Count].Cells[7].Style.BackColor = Color.LightYellow;
                    }
                    else
                    {
                        dataGridViewGuildData.Rows[Count].Cells[7].Style.BackColor = Color.White;
                    }
                }
                else
                {
                    dataGridViewGuildData.Rows[Count].Cells[7].Style.BackColor = Color.Red;
                }

                Count++;
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
        /// A double click on in a cell on the datagridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridViewGuildData_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridViewCell currentCell;


            // find out the row... and then try to update the individual chars Armory info and Gear Score

            // Get the current cell.
            currentCell = dataGridViewGuildData.CurrentCell;

            // Get the cell's current row
            Int32 CurrentRow = currentCell.RowIndex;


            // First check to make sure the vars are passed in
            if (CurrentRow < 0 || textBoxRealm.Text.Length == 0)
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
                String CharName = ((GuildMember)(SavedCharacters.SavedCharacters[CurrentRow])).Name;

                // Pass the name to the new form
                charAudit.CharacterName = CharName;

                charAudit.EquippediLevel = ((GuildMember)(SavedCharacters.SavedCharacters[CurrentRow])).EquipediLevel.ToString();
                charAudit.MaxiLevel = ((GuildMember)(SavedCharacters.SavedCharacters[CurrentRow])).MaxiLevel.ToString();

                charAudit.Profession1 = ((GuildMember)(SavedCharacters.SavedCharacters[CurrentRow])).Profession1;
                charAudit.Profession2 = ((GuildMember)(SavedCharacters.SavedCharacters[CurrentRow])).Profession2;

                charAudit.Spec = ((GuildMember)(SavedCharacters.SavedCharacters[CurrentRow])).Spec;
                charAudit.Role = ((GuildMember)(SavedCharacters.SavedCharacters[CurrentRow])).Role;

                GuildMember Char = (GuildMember)(SavedCharacters.SavedCharacters[CurrentRow]);

                // Pass the Data to the Form
                //if (charAudit.PassData(URLWowAPI + "character/" + textBoxRealm.Text + "/" + CharName + "?fields=items"))
                if (charAudit.PassData(Char))
                {
                    // Show the new form
                    charAudit.Show();
                }
                else
                {
                    DisplayError("ERROR - Can't Audit Character: " + CharName);
                }
            }

            this.Cursor = Cursors.Default;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Sorting"></param>
        private void SortGrid(String Sorting)
        {
            Boolean MultipleSort = false;


            if (Sorting.Contains(","))
            {
                // has multiple sorting factors
                MultipleSort = true;
            }

            dataGridViewGuildData.DataSource = null;

            SavedCharacters.SavedCharacters.Sort(new ObjectComparer(Sorting, MultipleSort));

            // refresh grid data
            dataGridViewGuildData.DataSource = SavedCharacters.SavedCharacters;

            // Now update the grid
            UpdateGrid();

            // Set the sorting glyphs
            String[] sortExpressions = Sorting.Trim().Split(',');
            for (Int32 i = 0; i < sortExpressions.Length; i++)
            {
                String fieldName = "";
                SortOrder direction = SortOrder.None;

                if (sortExpressions[i].Trim().EndsWith(" DESC"))
                {
                    fieldName = sortExpressions[i].Replace(" DESC", "").Trim();
                    direction = SortOrder.Descending;
                }
                else
                {
                    fieldName = sortExpressions[i].Replace(" ASC", "").Trim();
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridViewGuildData_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridViewColumn newColumn = dataGridViewGuildData.Columns[e.ColumnIndex];


            // Get the property name
            String SortCriteria = dataGridViewGuildData.Columns[e.ColumnIndex].HeaderCell.Value.ToString();

            // If oldColumn is null, then the DataGridView is not sorted.
            if (newColumn.HeaderCell.SortGlyphDirection == SortOrder.Ascending)
            {
                SortCriteria += " DESC";
            }
            else
            {
                SortCriteria += " ASC";
            }

            // now do the sorting
            try
            {
                SortGrid(SortCriteria);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Can't sort on that column YET!  [" + ex.Message + "]");
            }
        }

        #endregion
        
        #region " Get Guild Information Async Background Worker "

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GetGuildInfoAsync_DoWork(object sender, DoWorkEventArgs e)
        {
            // vars


            try
            {
                // The sender is the BackgroundWorker object we need it to
                // report progress and check for cancellation.
                BackgroundWorker bwAsync = sender as BackgroundWorker;

                // start the wait cursor
                WaitCursor(true);

                // Stop updating the Grid until the end
                SuspendGrid(true);

                // Periodically check if a cancellation request is pending.
                // If the user clicks cancel the line
                // m_AsyncWorker.CancelAsync(); if ran above.  This
                // sets the CancellationPending to true.
                // You must check this flag in here and react to it.
                // We react to it by setting e.Cancel to true and leaving.
                if (bwAsync.CancellationPending)
                {
                    // stop the job...
                    Log("Searching stopped!");
                }
                else
                {
                    // First get the entire Guild Roster
                    try
                    {
                        // This is the Web Site to get the guild info from...
                        // http://us.battle.net/api/wow/guild/Thrall/Secondnorth?fields=members

                        // Reset the Counter
                        bwAsync.ReportProgress(0);


                        GetGuildInfo guildInfo = new GetGuildInfo();
                        if (guildInfo.CollectData(URLWowAPI + @"guild/" + textBoxRealm.Text + @"/" + textBoxGuildName.Text + @"?fields=members"))
                        {
                            // success!

                            // now check to see if a grid needs to be updated or if it's the first time used
                            if (SavedCharacters.SavedCharacters.Count > 0)
                            {
                                // save the current data to a temp var
                                ArrayList temp = new ArrayList();
                                foreach (GuildMember m in SavedCharacters.SavedCharacters)
                                {
                                    temp.Add(m);
                                }

                                // erase the current data so we can start new
                                SavedCharacters.SavedCharacters.Clear();

                                // Fill out the data grid with the data we collected
                                SavedCharacters.SavedCharacters = guildInfo.Characters;

                                SavedCharacters.Guild = textBoxGuildName.Text;
                                SavedCharacters.Realm = textBoxRealm.Text;


                                foreach (GuildMember newmember in SavedCharacters.SavedCharacters)
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
                                SavedCharacters.SavedCharacters = guildInfo.Characters;
                            }
                        }


                        // Now get the individual Character data
                        Int32 Count = 0;
                        Int32 Total = SavedCharacters.SavedCharacters.Count;
                        ArrayList Errors = new ArrayList();
                        
                         try
                         {

                            // Go through all the guild members
                            foreach (GuildMember gm in SavedCharacters.SavedCharacters)
                            {
                                // Only check for Item Level for characters over level 10
                                //  Otherwise these characters won't be in the Armory
                                if (Convert.ToInt32(gm.Level) >= 10)
                                {
                                    // This is the Web Site to get the character info from...
                                    // http://us.battle.net/api/wow/character/Thrall/Purdee?fields=items,professions,talents
                                    
                                    GuildMember charInfo = GetCharacterInformation(gm.Name, SavedCharacters.Realm);
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
                                    }
                                    else
                                    {
                                        // Fail!  Save all errors until the end!
                                        Errors.Add("      " + gm.Name + "\t\t" + gm.Level);
                                    }
                                }

                                // Progress update
                                UpdateLabelMT(SavedCharacters.SavedCharacters.Count.ToString() + " total characters - On Character #" + Count);

                                // Progress update for Progress Bar
                                Double tempNum = (Double)Count++ / (Double)Total * 100;
                                bwAsync.ReportProgress((Int32)tempNum);
                            }
                         }
                        catch (Exception ex)
                        {
                            Log("Error: " + ex.Message);
                        }

                        // Data has been gathered now...
                        bwAsync.ReportProgress(100);

                        // re-sort by Item Level
                        SortGridMT("Level DESC, EquipediLevel DESC, MaxiLevel DESC, AchievementPoints DESC");

                        // Stop the stopwatch
                        sw.Stop();

                        // Check to see if there were any errors while trying to gather data for a character
                        if (Errors.Count > 0)
                        {
                            String ErrorMessage = "ERROR - Failed to parse some gear scores...\n   Couldn't retrieve information for the following characters:\n";

                            foreach (String str in Errors)
                            {
                                ErrorMessage += str + "\n";
                            }

                            Log(ErrorMessage);
                        }
                    }
                    catch (Exception ex)
                    {
                        Log("  **ERROR: " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Log("  **ERROR: " + ex.Message);
            }            
        }

        /// <summary>
        /// The background process is complete. We need to inspect
        /// our response to see if an error occurred, a cancel was
        /// requested or if we completed successfully.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                Log("Search has been cancelled!");
            }
            else
            {
                // Everything completed normally.

                // process the response using e.Result
                Log("Search has been completed!");

                // change the buttons back
                buttonGetGuildInfo.Text = "Get Guild Info";
                buttonRefreshGuildData.Text = "Refresh";

                // start the wait cursor
                WaitCursor(false);

                UpdateLabelMT(SavedCharacters.SavedCharacters.Count.ToString() + " total characters in " + sw.GetElapsedTime() + " milliseconds");

                // now switch the tab
                tabControl1.SelectTab(0);
            }

            // Stop updating the Grid until the end
            SuspendGrid(false);
        }

        #endregion

        #region " Form and Tab Functions "

        /// <summary>
        /// Before the form closes, save the important variables
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!String.IsNullOrEmpty(textBoxGuildName.Text))
            {
                Properties.Settings.Default.GuildName = textBoxGuildName.Text;
            }

            if (!String.IsNullOrEmpty(textBoxRealm.Text))
            {
                Properties.Settings.Default.Realm = textBoxRealm.Text;
            }

            Properties.Settings.Default.Save();

            try
            {
                if (SavedCharacters.SavedCharacters.Count > 0)
                {
                    Stream stream = File.Open("SavedCharacters.dat", FileMode.Create);
                    BinaryFormatter bformatter = new BinaryFormatter();
                    bformatter.Serialize(stream, SavedCharacters);
                    stream.Close();
                }

                if (RaidGroup.RaidGroup.Count > 0)
                {
                    Stream stream = File.Open("RaidGroup.dat", FileMode.Create);
                    BinaryFormatter bformatter = new BinaryFormatter();
                    bformatter.Serialize(stream, RaidGroup);
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            SortGrid("Level DESC, EquipediLevel DESC, MaxiLevel DESC, AchievementPoints DESC");

            // refresh the grid data since it's been changed
            dataGridViewRaidGroup.DataSource = null;
            dataGridViewRaidGroup.DataSource = RaidGroup.RaidGroup;
            UpdateRaidGrid();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
            {
                // Guild Members Tab
                UpdateGrid();
            }
            else if (tabControl1.SelectedIndex == 1)
            {
                // Raid Group Tab
                UpdateRaidGrid();
            }
        }

        #endregion

        #region " Save and Load Functions "

        /// <summary>
        /// Loads a saved guild file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonLoad_Click(object sender, EventArgs e)
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
                        //Open the file written above and read values from it.
                        Stream stream = File.Open(open.FileName, FileMode.Open);
                        BinaryFormatter bformatter = new BinaryFormatter();

                        // The file was read successfully, so now re-init the 
                        // SavedCharacters var and load up the new info to the Grid
                        SavedCharacters = new GuildMemberGroup();
                        SavedCharacters = (GuildMemberGroup)bformatter.Deserialize(stream);
                        stream.Close();

                        label3.Text = SavedCharacters.SavedCharacters.Count.ToString() + " total characters";
                        SortGrid("Level DESC, EquipediLevel DESC, MaxiLevel DESC, AchievementPoints DESC");

                        // update the text boxes                        
                        textBoxRealm.Text = SavedCharacters.Realm;
                        textBoxGuildName.Text = SavedCharacters.Guild;

                        // now switch the tab
                        tabControl1.SelectTab(0);
                    }
                }
            }
            catch (Exception ex)
            {
                if (!ex.Message.StartsWith("Could not find file"))
                {
                    Log(String.Format("  **ERROR[LoadTempData]: {0}", ex.Message));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSave_Click(object sender, EventArgs e)
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
                    if (SavedCharacters.SavedCharacters.Count > 0)
                    {
                        Stream stream = File.Open(save.FileName, FileMode.Create);
                        BinaryFormatter bformatter = new BinaryFormatter();
                        bformatter.Serialize(stream, SavedCharacters);
                        stream.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }
        #endregion

        #region " Logging Functions " 

        /// <summary>
        /// Place holder for future logging mechanism
        /// </summary>
        /// <param name="Message"></param>
        public void Log(String Message)
        {
            // Add to the Error Log
            ErrorLog.Add(Message);
        }

        /// <summary>
        /// 
        /// </summary>
        public void ShowAllErrors()
        {
            String Errors = "";

            foreach (String s in ErrorLog)
            {
                Errors += s + "\n";
            }

            MessageBox.Show(Errors);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Message"></param>
        public void DisplayError(String Message)
        {
            MessageBox.Show(Message);
            Log(Message);
        }
        #endregion

        #region " Button Functions "

        /// <summary>
        /// Refresh the Guild Data 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonRefreshGuildData_Click(object sender, EventArgs e)
        {
            // Simply Callthe buttonGetGuildInfo click 
            buttonGetGuildInfo_Click(sender, e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonGetGuildInfo_Click(object sender, EventArgs e)
        {
            // First check to make sure the vars are passed in
            if (textBoxGuildName.Text.Length == 0 || textBoxGuildName.Text.Length == 0)
            {
                MessageBox.Show("Error: Both Guild Name and Realm need to be filled out.");
            }
            else
            {
                // If the background thread is running then clicking this
                // button causes a cancel, otherwise clicking this button
                // launches the background thread.
                if (GetGuildInfoAsyncWorker.IsBusy)
                {
                    //Log("Cancelling Search...");
                    buttonGetGuildInfo.Enabled = false;
                    buttonRefreshGuildData.Enabled = false;

                    // Notify the worker thread that a cancel has been requested.
                    // The cancel will not actually happen until the thread in the
                    // DoWork checks the bwAsync.CancellationPending flag, for this
                    // reason we set the label to "Cancelling...", because we haven't
                    // actually cancelled yet.
                    GetGuildInfoAsyncWorker.CancelAsync();
                }
                else
                {
                    Log("Search Started...");

                    // start the wait cursor
                    WaitCursor(true);

                    // Change the button text so that this can be cancelled
                    buttonGetGuildInfo.Text = "Cancel";
                    buttonRefreshGuildData.Text = "Cancel";
                    
                    // Start the stop watch
                    sw.Start();

                    // Kickoff the worker thread to begin it's DoWork function.
                    GetGuildInfoAsyncWorker.RunWorkerAsync();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonDeleteItemCacheData_Click(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists("ItemCache.dat"))
                {
                    // delete it
                    File.Delete("ItemCache.dat");

                    // clear the current cache
                    Items = new ItemCache();
                }
            }
            catch (Exception ex)
            {
                Log("Error: " + ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            ShowAllErrors();
        }

        #endregion        

        #region " Raid Tab Functionality "

        /// <summary>
        /// Fires when Add Character button is pressed on the Raid Tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonAddCharacterToRaidData_Click(object sender, EventArgs e)
        {
            if (textBoxCharacterName.Text.Length == 0 || textBoxCharacterRealm.Text.Length == 0)
            {
                MessageBox.Show("Error: Both Character Name and Realm need to be filled out.");
            }
            else
            {
                WaitCursor(true);

                GuildMember gm = GetCharacterInformation(textBoxCharacterName.Text, textBoxCharacterRealm.Text);

                if (gm != null)
                {
                    // Clear out the Grid Data Source to get it ready for the new data
                    dataGridViewRaidGroup.DataSource = null;

                    // Add new character to Raid
                    RaidGroup.RaidGroup.Add(gm);

                    // refresh grid data
                    dataGridViewRaidGroup.DataSource = RaidGroup.RaidGroup;

                    // refresh the grid data since it's been changed
                    UpdateRaidGrid();
                }

                WaitCursor(false);
            }
        }

        /// <summary>
        /// Grabs all the information about 1 character
        /// </summary>
        /// <param name="name"></param>
        /// <param name="realm"></param>
        /// <returns></returns>
        public GuildMember GetCharacterInformation(string name, string realm)
        {
            // This is the Web Site to get the character info from...
            // http://us.battle.net/api/wow/character/Thrall/Purdee?fields=items,professions,talents

            GetCharacterInfo charInfo = new GetCharacterInfo();
            GuildMember gm = new GuildMember();

            if (charInfo.CollectFullData(URLWowAPI + "character/" + realm + "/" + name + "?fields=items,professions,talents"))
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
                    Log(String.Format("Failed to get information about {0}", textBoxCharacterName.Text));
                    gm = null;
                }
            }
            else
            {
                // Fail!  Save all errors until the end!
                Log(String.Format("Failed to get information about {0}", textBoxCharacterName.Text));
                gm = null;
            }

            return gm;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonRaidGroupRefresh_Click(object sender, EventArgs e)
        {
            WaitCursor(true);

            try
            {
                foreach (GuildMember oldMember in RaidGroup.RaidGroup)
                {
                    GuildMember gm = GetCharacterInformation(oldMember.Name, oldMember.Realm);

                    if (gm != null)
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

                        // Fill out the last Update time with the current time and date
                        oldMember.LastUpdated = DateTime.Now;
                    }
                }

                // Clear out the Grid Data Source to get it ready for the new data
                dataGridViewRaidGroup.DataSource = null;

                // refresh grid data
                dataGridViewRaidGroup.DataSource = RaidGroup.RaidGroup;

                // refresh the grid data since it's been changed
                UpdateRaidGrid();

            }
            catch (Exception ex)
            {
                Log("Error: " + ex.Message);
            }

            WaitCursor(false);
        }       

        /// <summary>
        /// Update the individual chars Armory info and Gear Score
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridViewRaidGroup_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridViewCell currentCell;

            // Get the current cell.
            currentCell = dataGridViewRaidGroup.CurrentCell;

            // Get the cell's current row
            Int32 CurrentRow = currentCell.RowIndex;

            // First check to make sure the vars are passed in
            if (CurrentRow < 0 || textBoxRealm.Text.Length == 0)
            {
                MessageBox.Show("Error: No row was selected.");
            }
            else
            {
                this.Cursor = Cursors.WaitCursor;

                // Create the new form to be used
                FormItemAudit charAudit = new FormItemAudit();

                GuildMember Char = (GuildMember)(RaidGroup.RaidGroup[CurrentRow]);

                // Pass the Data to the Form
                if (charAudit.PassData(Char))
                {
                    // Show the new form
                    charAudit.Show();
                }
                else
                {
                    DisplayError("ERROR - Can't Audit Character: " + Char.Name);
                }
            }

            this.Cursor = Cursors.Default;
        }        

        /// <summary>
        /// Fires when a mouse right clicks on the raid group grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridViewRaidGroup_MouseClick(object sender, MouseEventArgs e)
        {
            // Check for a right click
            if (e.Button == MouseButtons.Right)
            {
                // select the current row
                int currentMouseOverRow = this.dataGridViewRaidGroup.HitTest(e.X, e.Y).RowIndex;

                if (currentMouseOverRow > -1)
                {
                    this.dataGridViewRaidGroup.ClearSelection();
                    this.dataGridViewRaidGroup.Rows[currentMouseOverRow].Selected = true;

                    // pop up a context menu
                    this.contextMenuCharacter.Show(this.dataGridViewRaidGroup, new Point(e.X, e.Y));
                }
            }
        }

        /// <summary>
        /// Fires when a context menu item is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void contextMenuCharacter_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ToolStripItem item = e.ClickedItem;
                        
            if (item.Text == "Update this Character")
            {
                WaitCursor(true);

                try
                {
                    foreach (DataGridViewRow row in this.dataGridViewRaidGroup.SelectedRows)
                    {
                        int CurrentRow = row.Index;                        
                        GuildMember oldMember = (GuildMember)(RaidGroup.RaidGroup[CurrentRow]);
                        GuildMember gm = GetCharacterInformation(oldMember.Name, oldMember.Realm);                        

                        if (gm != null)
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
                    
                    // refresh the grid data since it's been changed
                    UpdateRaidGrid();

                }
                catch (Exception ex)
                {
                    Log("Error: " + ex.Message);
                }

                WaitCursor(false);
            }
            else if (item.Text == "Move Character Up")
            {
                if (this.dataGridViewRaidGroup.SelectedRows.Count > 0)
                {
                    // check to see if the row can to be moved
                    if (this.dataGridViewRaidGroup.SelectedRows[0].Index != 0)
                    {
                        // get the row index that will be moved
                        int row = this.dataGridViewRaidGroup.SelectedRows[0].Index;
                        GuildMember gm = (GuildMember)RaidGroup.RaidGroup[row];
                        RaidGroup.RaidGroup.RemoveAt(row);
                        RaidGroup.RaidGroup.Insert(row - 1, gm);
                        
                        dataGridViewRaidGroup.ClearSelection();
                        dataGridViewRaidGroup.Rows[row - 1].Selected = true;

                        // refresh the grid data since it's been changed
                        UpdateRaidGrid();
                    }
                }
            }
            else if (item.Text == "Move Character Down")
            {
                if (this.dataGridViewRaidGroup.SelectedRows.Count > 0)
                {
                    // check to see if the row can to be moved
                    if (this.dataGridViewRaidGroup.SelectedRows[0].Index != this.dataGridViewRaidGroup.RowCount - 1)
                    {
                        // get the row index that will be moved
                        int row = this.dataGridViewRaidGroup.SelectedRows[0].Index;
                        GuildMember gm = (GuildMember)RaidGroup.RaidGroup[row];
                        RaidGroup.RaidGroup.RemoveAt(row);
                        RaidGroup.RaidGroup.Insert(row + 1, gm);
                        
                        dataGridViewRaidGroup.ClearSelection();
                        dataGridViewRaidGroup.Rows[row+1].Selected = true;

                        // refresh the grid data since it's been changed
                        UpdateRaidGrid();
                    }
                }
            }
            else if (item.Text == "Delete Character from Grid")
            {
                // Delete this character(s) from the Raid Group
                foreach (DataGridViewRow row in this.dataGridViewRaidGroup.SelectedRows)
                {
                    // Remove the character selected
                    RaidGroup.RaidGroup.RemoveAt(row.Index);
                }

                // Clear out the Grid Data Source to get it ready for the new data
                dataGridViewRaidGroup.DataSource = null;

                // refresh grid data
                dataGridViewRaidGroup.DataSource = RaidGroup.RaidGroup;

                // refresh the grid data since it's been changed
                UpdateRaidGrid();
            }
        }

        /// <summary>
        /// Sort the grid according to the column clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridViewRaidGroup_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {            
            if (!(Control.ModifierKeys == Keys.Shift || Control.ModifierKeys == Keys.Control) && 
                e.Button == MouseButtons.Left && e.Clicks == 1)
            {
                // This was a double click on a column header - Sort it!
                DataGridViewColumn newColumn = dataGridViewRaidGroup.Columns[e.ColumnIndex];
                string Sorting = newColumn.Name;
                                
                ListSortDirection direction = ListSortDirection.Ascending;

                if (newColumn.HeaderCell.SortGlyphDirection == SortOrder.Ascending)
                {
                    direction = ListSortDirection.Descending;
                    Sorting += " DESC";
                }
                else
                {
                    Sorting += " ASC";
                }

                // Sort the selected column.                
                RaidGroup.RaidGroup.Sort(new ObjectComparer(Sorting, false));
                dataGridViewRaidGroup.DataSource = RaidGroup.RaidGroup;                                
                dataGridViewRaidGroup.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = direction == ListSortDirection.Ascending ? SortOrder.Ascending : SortOrder.Descending;
                UpdateRaidGrid();
            }
        }

        /// <summary>
        /// The sort compare for the RaidGroup
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridViewRaidGroup_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            // Try to sort based on the cells in the current column.
            e.SortResult = System.String.Compare(e.CellValue1.ToString(), e.CellValue2.ToString());

            // If the cells are equal, sort based on the ID column. 
            if (e.SortResult == 0 && e.Column.Name != "ID")
            {
                e.SortResult = System.String.Compare(
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
                        
            Int32 Count = 0;
            Int32 iLevel = 0;

            // Now check to see if the Item Level has changed and if their level has changed
            //foreach (GuildMember gm in RaidGroup.RaidGroup)
            for (Int32 i = 0; i < dataGridViewRaidGroup.Rows.Count; i++)
            {
                // Get the guild member
                GuildMember gm = ((GuildMember)(RaidGroup.RaidGroup[i]));

                // Check for iLevel Changes
                if (gm.IsItemLevelChanged())
                {
                    dataGridViewRaidGroup.Rows[Count].Cells[5].Style.BackColor = Color.Aquamarine;
                    dataGridViewRaidGroup.Rows[Count].Cells[6].Style.BackColor = Color.Aquamarine;
                }
                else
                {
                    dataGridViewRaidGroup.Rows[Count].Cells[5].Style.BackColor = Color.White;
                    dataGridViewRaidGroup.Rows[Count].Cells[6].Style.BackColor = Color.White;
                }

                // Check for Level Changes
                if (gm.IsLevelChanged())
                {
                    dataGridViewRaidGroup.Rows[Count].Cells[1].Style.BackColor = Color.Aquamarine;
                }
                else
                {
                    dataGridViewRaidGroup.Rows[Count].Cells[1].Style.BackColor = Color.White;
                }

                // Sum up the Equipped iLevel
                if (dataGridViewRaidGroup.Rows[Count].Cells[6].Value != null)
                {
                    iLevel += Convert.ToInt32(dataGridViewRaidGroup.Rows[Count].Cells[6].Value);
                }

                Count++;
            }

            // Update the AVE Equipped iLevel of the Raid Team
            if (Count > 0)
            {
                //label ready
                labelRaidTab.Text = "Average Raid Team iLevel = " + Convert.ToInt32(Convert.ToDecimal(iLevel) / Convert.ToDecimal(Count)).ToString() + " for " + RaidGroup.RaidGroup.Count.ToString() + " total characters";
            }

            // refresh the grid data since it's been changed
            dataGridViewRaidGroup.Refresh();            
        }

        #endregion

        #region " Raid Loot Drop "

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripComboBoxRaidLootDropRaid_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Clear out the results first
            dataGridViewRaidLootDrop.DataSource = null;
            toolStripComboBoxRaidLootDropBoss.Items.Clear();

            if (RaidLoot.ContainsKey(toolStripComboBoxRaidLootDropRaid.SelectedItem.ToString()))
            {
                // Fill out bosses now
                foreach (string value in RaidLoot[toolStripComboBoxRaidLootDropRaid.SelectedItem.ToString()].Keys)
                {
                    toolStripComboBoxRaidLootDropBoss.Items.Add(value);
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripComboBoxRaidLootDropBoss_SelectedIndexChanged(object sender, EventArgs e)
        {
            WaitCursor(true);

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
            if (RaidLoot[toolStripComboBoxRaidLootDropRaid.SelectedItem.ToString()].ContainsKey(toolStripComboBoxRaidLootDropBoss.SelectedItem.ToString()))
            {
                itemIds = RaidLoot[toolStripComboBoxRaidLootDropRaid.SelectedItem.ToString()][toolStripComboBoxRaidLootDropBoss.SelectedItem.ToString()];
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

                    // does this item currently exist in the item cache?
                    if (!Items.Contains(itemId))
                    {
                        // Need to add in this item to the Item Cache

                        // First fetch the data
                        GetItemInfo get = new GetItemInfo();
                        if (get.CollectData(itemId))
                        {
                            item = get.Item;
                            item.Id = itemId;
                            Items.AddItem(item);
                        }
                    }
                    else
                    {
                        item = Items.GetItem(itemId);
                    }

                    if (item != null)
                    {
                        // Now we have the item in the item cache

                        // Check against each member in the Raid Group
                        foreach (GuildMember gm in RaidGroup.RaidGroup)
                        {
                            //   Here are the different Classes:
                            // Death Knight
                            // Druid
                            // Hunter
                            // Mage
                            // Monk
                            // Paladin
                            // Priest                                
                            // Rogue
                            // Shaman
                            // Warlock
                            // Warrior
                            string charName = string.Empty;

                            // can this member use it?
                            if (Converter.ConvertItemClass(item.ItemClass) == "Armor")
                            {
                                // Armor
                                if (Converter.ConvertItemSubClass(item.ItemClass, item.ItemSubClass) == "Micellaneous")
                                {
                                    if (item.HasIntellect())
                                    {
                                        if (item.HasSpirit())
                                        {
                                            // Healer Classes
                                            if (gm.Role == "HEALING" ||
                                                (gm.Role == "DPS" && (gm.Spec == "Balance" || gm.Spec == "Shadow" || gm.Spec == "Elemental"))
                                                )
                                            {
                                                charName = gm.Name;
                                            }
                                        }
                                        else
                                        {
                                            if ((!item.HasHit() && gm.Role == "HEALING") ||
                                                (gm.Role == "DPS" && (gm.Spec == "Balance" || gm.Class == "Mage" || gm.Class == "Shadow" || gm.Spec == "Elemental" || gm.Class == "Warlock"))
                                                )
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
                                    if (item.HasIntellect() && (gm.Class == "Druid" && (gm.Spec == "Balance" || gm.Spec == "Restoration")) && (gm.Class == "Monk" && gm.Role == "HEALING"))
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
                                                if (gm.Role == "HEALING" ||
                                                    (gm.Role == "DPS" && (gm.Spec == "Balance" || gm.Spec == "Shadow" || gm.Spec == "Elemental"))
                                                    )
                                                {
                                                    charName = gm.Name;
                                                }
                                            }
                                            else
                                            {
                                                if ((!item.HasHit() && gm.Role == "HEALING") ||
                                                    (gm.Role == "DPS" && (gm.Spec == "Balance" || gm.Class == "Mage" || gm.Class == "Shadow" || gm.Spec == "Elemental" || gm.Class == "Warlock"))
                                                    )
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
                                    // else it's a regular non-back cloth piece
                                    else if (item.HasSpirit() && (gm.Role == "HEALING" && gm.Class == "Priest"))
                                    {
                                        charName = gm.Name;
                                    }
                                    else if (!item.HasSpirit() && (gm.Class == "Mage" || gm.Class == "Warlock" || (gm.Class == "Priest" && gm.Spec == "Shadow")))
                                    {
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
                                        else if (item.HasHit() && gm.Role == "DPS" && gm.Class == "Shaman")
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
                                    if (item.HasStrength() && (gm.Class == "Death Knight" || gm.Class == "Paladin" || gm.Class == "Warrior"))
                                    {
                                        charName = gm.Name;
                                    }
                                }
                                else if (weapon == "1 Mace")
                                {
                                    //if (gm.Class == "Death Knight" || gm.Class == "Druid" || gm.Class == "Monk" || gm.Class == "Paladin" || gm.Class == "Priest" || gm.Class == "Rogue" || gm.Class == "Shaman" || gm.Class == "Warrior")
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
                                        if (gm.Class == "Death Knight" || (gm.Class == "Paladin" && gm.Role != "HEALING") || gm.Class == "Warrior")
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
                                        else if (item.HasHit() && gm.Role == "DPS" && (gm.Class == "Druid" || gm.Class == "Priest" || gm.Class == "Shaman"))
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
                                        else if (gm.Role == "DPS" && (gm.Class == "Death Knight" || gm.Class == "Paladin" || gm.Class == "Warrior"))
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
                                    else if (item.HasAgility() && (gm.Class == "Druid" || gm.Class == "Monk"))
                                    {
                                        charName = gm.Name;
                                    }
                                }
                                else if (weapon == "1 Sword")
                                {
                                    //if (gm.Class == "Death Knight" || gm.Class == "Mage" || gm.Class == "Monk" || gm.Class == "Paladin" || gm.Class == "Rogue" || gm.Class == "Warlock" || gm.Class == "Warrior")
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
                                        else if (item.HasHit() && gm.Role == "DPS" && (gm.Class == "Mage" || gm.Class == "Warlock"))
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
                                    else if (gm.Role == "DPS" && (gm.Class == "Death Knight" || gm.Class == "Paladin" || gm.Class == "Warrior"))
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
                                            else if (gm.Role == "DPS" && ((gm.Class == "Druid" && gm.Spec == "Balance") || (gm.Class == "Priest" && gm.Spec == "Shadow") || (gm.Class == "Shaman" && gm.Spec == "Elemental")))
                                            {
                                                charName = gm.Name;
                                            }
                                        }
                                        else if (item.HasHit())
                                        {
                                            if (gm.Role == "DPS" && ((gm.Class == "Druid" && gm.Spec == "Balance") || gm.Class == "Mage" || (gm.Class == "Priest" && gm.Spec == "Shadow") || (gm.Class == "Shaman" && gm.Spec == "Elemental") || gm.Class == "Warlock"))
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
                                            else if (gm.Role == "DPS" && ((gm.Class == "Druid" && gm.Spec == "Balance") || gm.Class == "Priest" || (gm.Class == "Shaman" && gm.Spec == "Elemental")))
                                            {
                                                charName = gm.Name;
                                            }
                                        }
                                        else if (item.HasHit())
                                        {
                                            if (gm.Role == "DPS" && ((gm.Class == "Druid" && gm.Spec == "Balance") || gm.Class == "Mage" || (gm.Class == "Priest" && gm.Spec == "Shadow") || (gm.Class == "Shaman" && gm.Spec == "Elemental") || gm.Class == "Warlock"))
                                            {
                                                charName = gm.Name;
                                            }
                                        }
                                        else if (((gm.Class == "Druid" && (gm.Spec == "Balance" || gm.Spec == "Restoration")) || gm.Class == "Mage" || gm.Class == "Priest" || (gm.Class == "Shaman" && (gm.Spec == "Elemental" || gm.Spec == "Restoration")) || gm.Class == "Warlock"))
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
                                    if (gm.Class == "Hunter")
                                    {
                                        charName = gm.Name;
                                    }
                                }                                
                                else
                                {
                                    MessageBox.Show("Weapon [" + weapon + "] not found!");
                                }
                            }

                            // Can the row be added to data table?
                            if (charName.ToString() != string.Empty)
                            {
                                string slot = Converter.ConvertInventoryType(item.InventoryType);
                                bool pass = false;
                                int iLevelOld = 0;
                                int iLevelNew = 0;

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

                                if (pass)
                                {                                    
                                    iLevelNew = item.ItemLevel;

                                    if (slot == "finger")
                                    {
                                        iLevelOld = gm.ItemAudits["finger1"].ItemLevel > gm.ItemAudits["finger2"].ItemLevel ? gm.ItemAudits["finger2"].ItemLevel : gm.ItemAudits["finger1"].ItemLevel;
                                    }
                                    else if (slot == "trinket")
                                    {
                                        iLevelOld = gm.ItemAudits["trinket1"].ItemLevel > gm.ItemAudits["trinket2"].ItemLevel ? gm.ItemAudits["trinket2"].ItemLevel : gm.ItemAudits["trinket1"].ItemLevel;
                                    }
                                    else if (slot == "mainHand" || slot == "offHand")
                                    {
                                        string weapon = Converter.ConvertItemSubClass(item.ItemClass, item.ItemSubClass);

                                        // Need to check for dual hands vs 2 handers vs one hand + off hand...
                                        if ((gm.Class == "Rogue" || (gm.Class == "Shaman" && gm.Spec == "Enhancement") || (gm.Class == "Warrior" && gm.Spec == "Fury") || (gm.Class == "Druid" && (gm.Spec == "Guardian" || gm.Spec == "Feral")) || (gm.Class == "Monk" && (gm.Spec == "Brewmaster" || gm.Spec == "Windwalker")) || (gm.Class == "Death Knight" && (gm.Spec == "Unholy" || gm.Spec == "Frost"))) &&
                                            //(weapon == "1 Axe" || weapon == "1 Mace" || weapon == "1 Sword" || weapon == "Fist Weapon" || weapon == "Dagger") &&
                                            //(gm.ItemAudits.ContainsKey("mainHand") && gm.ItemAudits.ContainsKey("offHand")))
                                            (gm.ItemAudits["offHand"].ItemLevel > 0))
                                        {                                            
                                            iLevelOld = gm.ItemAudits["mainHand"].ItemLevel > gm.ItemAudits["offHand"].ItemLevel ? gm.ItemAudits["offHand"].ItemLevel : gm.ItemAudits["mainHand"].ItemLevel;
                                        }
                                        else if (slot == "offHand" && gm.ItemAudits["offHand"].ItemLevel == 0)
                                        {
                                            int itemIdMainHand = gm.ItemAudits["mainHand"].Id;
                                            int slotType = 0;
                                            ItemInfo itemMainHand = null;

                                            // does this item currently exist in the item cache?
                                            if (!Items.Contains(itemIdMainHand))
                                            {
                                                // Need to add in this item to the Item Cache

                                                // First fetch the data
                                                GetItemInfo get = new GetItemInfo();
                                                if (get.CollectData(itemIdMainHand))
                                                {
                                                    itemMainHand = get.Item;
                                                    itemMainHand.Id = itemIdMainHand;
                                                    Items.AddItem(itemMainHand);
                                                }
                                            }
                                            else
                                            {
                                                itemMainHand = Items.GetItem(itemIdMainHand);
                                            }

                                            if (itemMainHand != null)
                                            {
                                                slotType = itemMainHand.InventoryType;

                                                // if slot if offHand and currently doesn't have an offHand equipped
                                                //  and mainhand is a 2 hander... compare iLevel with mainHand
                                                if (slotType == 17 || slotType == 15 || slotType == 26)
                                                {
                                                    iLevelOld = gm.ItemAudits["mainHand"].ItemLevel;
                                                }
                                                else
                                                {
                                                    iLevelOld = gm.ItemAudits[Converter.ConvertInventoryType(item.InventoryType)].ItemLevel;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            iLevelOld = gm.ItemAudits[Converter.ConvertInventoryType(item.InventoryType)].ItemLevel;
                                        }
                                    }
                                    else
                                    {
                                        iLevelOld = gm.ItemAudits[Converter.ConvertInventoryType(item.InventoryType)].ItemLevel;
                                    }

                                    if (iLevelNew > iLevelOld)
                                    {
                                        DataRow dr = loot.NewRow();
                                        dr["Upgrade"] = iLevelNew - iLevelOld;
                                        dr["ItemId"] = item.Id;
                                        dr["ItemName"] = item.Name;
                                        dr["ItemSlot"] = slot;
                                        dr["CharacterName"] = charName;
                                        dr["ItemILevel"] = iLevelNew;
                                        dr["OldItemILevel"] = iLevelOld;
                                        loot.Rows.Add(dr);
                                    }
                                    else
                                    {
                                        DataRow dr = loot.NewRow();
                                        dr["Upgrade"] = -1;
                                        dr["ItemId"] = item.Id;
                                        dr["ItemName"] = item.Name;
                                        dr["ItemSlot"] = slot;
                                        dr["CharacterName"] = "n/a";
                                        dr["ItemILevel"] = iLevelNew;
                                        dr["OldItemILevel"] = 0;
                                        loot.Rows.Add(dr);
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
                        if (loot.Select("ItemId = " + itemId, "upgrade desc").Length == 0)
                        {
                            ItemInfo item = null;

                            // does this item currently exist in the item cache?
                            if (!Items.Contains(itemId))
                            {
                                // Need to add in this item to the Item Cache

                                // First fetch the data
                                GetItemInfo get = new GetItemInfo();
                                if (get.CollectData(itemId))
                                {
                                    item = get.Item;
                                    item.Id = itemId;
                                    Items.AddItem(item);
                                }
                            }
                            else
                            {
                                item = Items.GetItem(itemId);
                            }

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

            WaitCursor(false);
        }

        #endregion

        #region " Raid Loot - Loot data from wowhead "

        /// <summary>
        /// Fill out the raid boss loot data here
        /// TODO: 
        ///   Evenually will need to be able to Add this all in programmatically.
        ///   But for now, I will hard code in what we need, and get the ranking functionality in place to use.
        /// </summary>
        private void CreateRaidLootData()
        {            
            Dictionary<string, int[]> tempLoot = new Dictionary<string, int[]>();
            string RaidName = string.Empty;
            string RaidBoss = string.Empty;
            int[] BossLoot;
            
            // 1st Tier 14 Raid
            RaidName = "Mogu'shan Vaults - 10N";

            RaidBoss = "The Stone Guard";
            BossLoot = new int[] { 85922, 85979, 89768, 85924, 85975, 85978, 85925, 89767, 85976, 86134, 85977, 89766, 85926, 85923 };
            tempLoot.Add(RaidBoss, BossLoot);            

            RaidBoss = "Feng the Accursed";
            BossLoot = new int[] { 85986, 86082, 85983, 85987, 85985, 89424, 89803, 89802, 85989, 85990, 85988, 85984, 85982, 85980 };
            tempLoot.Add(RaidBoss, BossLoot);
            
            RaidBoss = "Gara'jal the Spiritbinder";
            BossLoot = new int[] { 86027, 89817, 86038, 85996, 85993, 85994, 86040, 85995, 85997, 86041, 85992, 85991, 86039 };
            tempLoot.Add(RaidBoss, BossLoot);

            RaidBoss = "The Spirit Kings";
            BossLoot = new int[] { 86047, 86081, 86076, 86071, 86075, 89818, 86080, 86127, 86086, 86129, 86084, 89819, 86128, 86083 };
            tempLoot.Add(RaidBoss, BossLoot);

            RaidBoss = "Elegon";
            BossLoot = new int[] { 86133, 86140, 86132, 89822, 86139, 86137, 86136, 86131, 86135, 86141, 86138, 89821, 86130, 89824 };
            tempLoot.Add(RaidBoss, BossLoot);

            RaidBoss = "Will of the Emperor";
            BossLoot = new int[] { 86144, 86145, 86146, 87827, 89823, 86142, 89820, 89825, 86151, 86150, 86147, 86148, 86149, 86152 };
            tempLoot.Add(RaidBoss, BossLoot);

            RaidLoot.Add(RaidName, tempLoot);            

            // 2nd Tier 14 Raid - Hearts of Fire            
            RaidName = "Heart of Fear - 10N";
            tempLoot = new Dictionary<string, int[]>();

            RaidBoss = "Imperial Vizier Zor'lok";
            BossLoot = new int[] { 86156, 89827, 86157, 89829, 86154, 86153, 89826, 87824, 86158, 86159, 86161, 86160, 86203, 86155 };
            tempLoot.Add(RaidBoss, BossLoot);

            RaidLoot.Add(RaidName, tempLoot);

            // Tier 15 Raid
            RaidName = "Throne of Thunder - 10N";
            tempLoot = new Dictionary<string, int[]>();

            RaidBoss = "Jin'rokh the Breaker";
            BossLoot = new int[] { 94738, 95510, 94512, 94739, 94726, 94723, 94735, 94733, 94724, 94737, 94730, 94725, 94728, 94722, 94736, 94732, 94734, 94729, 94731, 94727, 95064, 97126, 95066, 95503, 95063, 95069, 95065, 95500, 95504, 95498 };
            tempLoot.Add(RaidBoss, BossLoot);

            RaidBoss = "Horridon";
            BossLoot = new int[] { 94754, 95514, 94526, 94514, 94747, 94751, 94742, 94745, 94744, 95063, 95502, 95498, 95505, 95499, 95500, 95068, 95069, 95516 };
            tempLoot.Add(RaidBoss, BossLoot);

            RaidLoot.Add(RaidName, tempLoot);

            // Tier 16 Raid - LFR
            RaidName = "Siege of Orgrimmar - LFR";
            tempLoot = new Dictionary<string, int[]>();

            // Wing 1
            RaidBoss = "Immerseus";
            BossLoot = new int[] { 104920, 104927, 104917, 104913, 104914, 104923, 104915, 104919, 104929, 104911, 104922, 104921, 104909, 104918, 104912, 104924, 104926, 104925, 104928, 104916, 104910, 104930 };
            tempLoot.Add(RaidBoss, BossLoot);

            RaidBoss = "The Fallen Protectors";
            BossLoot = new int[] { 104936, 104931, 104951, 104939, 104950, 104934, 104944, 104945, 104935, 104946, 104942, 104940, 104948, 104941, 104937, 104949, 104943, 104947, 104932, 104938, 104933 };
            tempLoot.Add(RaidBoss, BossLoot);

            RaidBoss = "Norushen";
            BossLoot = new int[] { 104964, 104969, 104958, 104963, 104971, 104970, 104960, 104961, 104955, 104956, 104968, 104952, 104957, 104959, 104953, 104966, 104954, 104965, 104972, 104967, 104973, 104962 };
            tempLoot.Add(RaidBoss, BossLoot);

            RaidBoss = "Sha of Pride";
            BossLoot = new int[] { 104974, 104982, 104979, 104977, 104981, 104980, 104975, 104976, 104978, 104983 };  // 99678, 99679, 99677 - Tier 16 Chest Tokens
            tempLoot.Add(RaidBoss, BossLoot);

            // Wing 2
            RaidBoss = "Galakras";
            BossLoot = new int[] { 104991, 104995, 104988, 104984, 104989, 105002, 105001, 104993, 105000, 104997, 104994, 105003, 104987, 104992, 104996, 104999, 104998, 105004, 104985, 104990, 104986, 105005 };
            tempLoot.Add(RaidBoss, BossLoot);

            RaidBoss = "Iron Juggernaut";
            BossLoot = new int[] { 105017, 105027, 105019, 105024, 105026, 105011, 105014, 105020, 105016, 105015, 105023, 105007, 105022, 105018, 105009, 105010, 105008, 105006, 105021, 105013, 105025, 105012 };
            tempLoot.Add(RaidBoss, BossLoot);

            RaidBoss = "Kor'kron Dark Shaman";
            BossLoot = new int[] { 105035, 105041, 105045, 105036, 105034, 105030, 105044, 105037, 105032, 105029, 105040, 105043, 105042, 105028, 105038, 105031, 105047, 105046, 105048, 105039, 105033 };
            tempLoot.Add(RaidBoss, BossLoot);

            RaidBoss = "General Nazgrim";
            BossLoot = new int[] { 105052, 105058, 105056, 105057, 105051, 105049, 105055, 105054, 105050, 105053, 105059 };  // 99681, 99667, 99680 - Tier 16 Hand Tokens
            tempLoot.Add(RaidBoss, BossLoot);

            // Wing 3
            RaidBoss = "Malkorok";
            BossLoot = new int[] { 105075, 105066, 105078, 105079, 105080, 105074, 105062, 105072, 105061, 105063, 105067, 105065, 105069, 105068, 105071, 105060, 105073, 105076, 105081, 105070, 105077, 105064 };
            tempLoot.Add(RaidBoss, BossLoot);

            RaidBoss = "Spoils of Pandaria";
            BossLoot = new int[] { 105087, 105092, 105086, 105093, 105100, 105099, 105083, 105088, 105096, 105097, 105095, 105085, 105094, 105102, 105090, 105084, 105101, 105091, 105098, 105082, 105089 };
            tempLoot.Add(RaidBoss, BossLoot);

            RaidBoss = "Thok the Bloodthirsty";
            BossLoot = new int[] { 105106, 105112, 105113, 105107, 105104, 105103, 105110, 105105, 105108, 105109, 105111 };  // 99672,99673,99671 - Tier 16 Head Tokens
            tempLoot.Add(RaidBoss, BossLoot);

            // Wing 4
            RaidBoss = "Siegecrafter Blackfuse";
            BossLoot = new int[] { 105122, 105124, 105118, 105119, 105121, 105117, 105115, 105116, 105120, 105123, 105114 };  // 99669,99670,99668 - Tier 16 Shoulder Tokens
            tempLoot.Add(RaidBoss, BossLoot);

            RaidBoss = "Paragons of the Klaxxi";
            BossLoot = new int[] { 105131, 105128, 105132, 105133, 105125, 105130, 105126, 105135, 105127, 105129, 105134 };  // 99675,99676,99674 - Tier 16 Legs Tokens
            tempLoot.Add(RaidBoss, BossLoot);

            RaidBoss = "Garrosh Hellscream";
            BossLoot = new int[] { 105148, 105150, 105139, 105156, 105137, 105155, 105147, 105149, 105145, 105154, 105151, 105138, 105136, 105142, 105157, 105140, 105152, 105153, 105141, 105143, 105146 };  // 105860,105861,105862 - Tier 16 All Token
            tempLoot.Add(RaidBoss, BossLoot);

            RaidLoot.Add(RaidName, tempLoot);            

            // Tier 16 Raid - Flex
            RaidName = "Siege of Orgrimmar - Flex";
            tempLoot = new Dictionary<string, int[]>();

            // Wing 1
            RaidBoss = "Immerseus";
            BossLoot = new int[] { 104671, 104678, 104688, 104664, 104665, 104674, 104666, 104670, 104680, 104662, 104673, 104672, 104660, 104669, 104663, 104675, 104677, 104676, 104679, 104667, 104661, 104681 };
            tempLoot.Add(RaidBoss, BossLoot);

            RaidBoss = "The Fallen Protectors";
            BossLoot = new int[] { 104687, 104682, 104702, 104690, 104701, 104685, 104695, 104696, 104686, 104697, 104693, 104691, 104699, 104692, 104688, 104700, 104694, 104698, 104683, 104689, 104684 };
            tempLoot.Add(RaidBoss, BossLoot);

            RaidBoss = "Norushen";
            BossLoot = new int[] { 104715, 104720, 104709, 104714, 104722, 104721, 104711, 104712, 104706, 104707, 104719, 104703, 104708, 104710, 104704, 104717, 104705, 104716, 104723, 104718, 104724, 104713 };
            tempLoot.Add(RaidBoss, BossLoot);

            RaidBoss = "Sha of Pride";
            BossLoot = new int[] { 104725, 99743, 99744, 99742, 104733, 104730, 104728, 104732, 104731, 104726, 104727, 104729, 104734 };
            tempLoot.Add(RaidBoss, BossLoot);

            // Wing 2
            RaidBoss = "Galakras";
            BossLoot = new int[] { 104742, 104746, 104739, 104735, 104740, 104753, 104752, 104744, 104751, 104748, 104745, 104754, 104738, 104743, 104747, 104750, 104749, 104755, 104736, 104741, 104737, 104756 };
            tempLoot.Add(RaidBoss, BossLoot);

            RaidBoss = "Iron Juggernaut";
            BossLoot = new int[] { 104768, 104778, 104770, 104775, 104777, 104762, 104765, 104771, 104767, 104766, 104774, 104758, 104773, 104769, 104760, 104761, 104759, 104757, 104772, 104764, 104776, 104763 };
            tempLoot.Add(RaidBoss, BossLoot);

            RaidBoss = "Kor'kron Dark Shaman";
            BossLoot = new int[] { 104786, 104792, 104796, 104787, 104785, 104781, 104795, 104788, 104783, 104780, 104791, 104794, 104793, 104779, 104789, 104782, 104798, 104797, 104799, 104790, 104784 };
            tempLoot.Add(RaidBoss, BossLoot);

            RaidBoss = "General Nazgrim";
            BossLoot = new int[] { 104803, 104809, 104807, 104808, 99746, 99747, 99745, 104802, 104800, 104806, 104805, 104801, 104804, 104810 };
            tempLoot.Add(RaidBoss, BossLoot);

            // Wing 3
            RaidBoss = "Malkorok";
            BossLoot = new int[] { };
            tempLoot.Add(RaidBoss, BossLoot);

            RaidBoss = "Spoils of Pandaria";
            BossLoot = new int[] { };
            tempLoot.Add(RaidBoss, BossLoot);

            RaidBoss = "Thok the Bloodthirsty";
            BossLoot = new int[] { };
            tempLoot.Add(RaidBoss, BossLoot);

            // Wing 4
            RaidBoss = "Siegecrafter Blackfuse";
            BossLoot = new int[] { };
            tempLoot.Add(RaidBoss, BossLoot);

            RaidBoss = "Paragons of the Klaxxi";
            BossLoot = new int[] { };
            tempLoot.Add(RaidBoss, BossLoot);

            RaidBoss = "Garrosh Hellscream";
            BossLoot = new int[] { };
            tempLoot.Add(RaidBoss, BossLoot);

            RaidLoot.Add(RaidName, tempLoot);            
        }
        
        #endregion

        #region " Raid Loot Grid Functions "

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridViewRaidLootDrop_MouseDoubleClick(object sender, MouseEventArgs e)
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
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridViewRaidLootDrop_CellToolTipTextNeeded(object sender, DataGridViewCellToolTipTextNeededEventArgs e)
        {
            if (e.ColumnIndex == 3 && e.RowIndex >= 0 && dataGridViewRaidLootDrop.Rows.Count > 0)
            {
                int id = Convert.ToInt32(dataGridViewRaidLootDrop.Rows[e.RowIndex].Cells["ItemId"].Value);
                ItemInfo item = Items.GetItem(id);
                dataGridViewRaidLootDrop.Rows[e.RowIndex].Cells["ItemName"].ToolTipText = item.Tooltip;
            }
        }

        private void dataGridViewRaidLootDrop_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            // Set the control types for all the rows in the grid.
            foreach (DataGridViewRow r in this.dataGridViewRaidLootDrop.Rows)
            {
                // Display a row count in the row header.
                r.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
                r.HeaderCell.Value = string.Format("{0}", r.Index + 1);   
            }

            dataGridViewRaidLootDrop.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);
        }

        #endregion
    }
}
