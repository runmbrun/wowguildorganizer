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

        public String URLWowAPI = @"http://us.battle.net/api/wow/";
        GuildMemberGroup SavedCharacters;
        RaidMemberGroup RaidGroup;
        private BackgroundWorker GetGuildInfoAsyncWorker = new BackgroundWorker();
        private StopWatch sw = new StopWatch();
        static public ItemCache Items;

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

                    //TODO - need to get a label ready
                    //label3.Text = RaidGroup.RaidGroup.Count.ToString() + " total characters";
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
        /// 
        /// </summary>
        private void UpdateGrid()
        {
            // Update the grid back color for the rows to white
            dataGridViewGuildData.RowsDefaultCellStyle.BackColor = Color.White;
            dataGridViewGuildData.AutoResizeColumns();
            dataGridViewGuildData.AutoResizeRows();


            // Check the Rows for when the last time the char was checked - color changes 
            for (Int32 i = 0; i < dataGridViewGuildData.Rows.Count; i++)
            {
                if (dataGridViewGuildData.Rows[i].Cells[7].Value != null)
                {
                    // Check to see when the ItemLevel was last updated
                    DateTime CheckAgain = (DateTime)dataGridViewGuildData.Rows[i].Cells[7].Value;
                    if (DateTime.Now > CheckAgain.AddDays(7))
                    {
                        dataGridViewGuildData.Rows[i].Cells[7].Style.BackColor = Color.Red;
                    }
                    else if (DateTime.Now > CheckAgain.AddDays(5))
                    {
                        dataGridViewGuildData.Rows[i].Cells[7].Style.BackColor = Color.OrangeRed;
                    }
                    else if (DateTime.Now > CheckAgain.AddDays(3))
                    {
                        dataGridViewGuildData.Rows[i].Cells[7].Style.BackColor = Color.Orange;
                    }
                    else if (DateTime.Now > CheckAgain.AddDays(2))
                    {
                        dataGridViewGuildData.Rows[i].Cells[7].Style.BackColor = Color.Yellow;
                    }
                    else if (DateTime.Now > CheckAgain.AddDays(1))
                    {
                        dataGridViewGuildData.Rows[i].Cells[7].Style.BackColor = Color.LightYellow;
                    }
                    else
                    {
                        dataGridViewGuildData.Rows[i].Cells[7].Style.BackColor = Color.White;
                    }
                }
                else
                {
                    dataGridViewGuildData.Rows[i].Cells[7].Style.BackColor = Color.Red;
                }
            }            

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

                //TODO - Add the new functionality here...

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

                // Pass the Data to the Form
                if (charAudit.PassData(URLWowAPI + "character/" + textBoxRealm.Text + "/" + CharName + "?fields=items"))
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
                        //http://us.battle.net/api/wow/guild/Thrall/Secondnorth?fields=members

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
                                                newmember.ClearItemLevelFlag();
                                            }

                                            // always check to carry over the max iLevel value from old to new
                                            // since new will always be blank
                                            if (newmember.EquipediLevel == 0 && oldmember.EquipediLevel != 0)
                                            {
                                                newmember.EquipediLevel = oldmember.EquipediLevel;
                                                newmember.ClearItemLevelFlag();
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
                                        // http://us.battle.net/api/wow/character/Thrall/Purdee/?fields=items

                                        GetCharacterInfo charInfo = new GetCharacterInfo();
                                        if (charInfo.CollectData(URLWowAPI + "character/" + textBoxRealm.Text + "/" + gm.Name + "?fields=items"))
                                        {
                                            // success!

                                            // Fill out the data grid with the data we collected
                                            gm.MaxiLevel = charInfo.MaxiLevel;
                                            gm.EquipediLevel = charInfo.EquipediLevel;
                                        }
                                        else
                                        {
                                            // Fail!  Save all errors until the end!
                                            Errors.Add("      " + gm.Name + "\t\t" + gm.Level);
                                        }
                                    }

                                    // Progress update
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
                        SortGridMT("Level DESC, EquipediLevel DESC");

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

                            DisplayError(ErrorMessage);
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

        #region " Form Functions "

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
            SortGrid("Level DESC, EquipediLevel DESC");

            // refresh the grid data since it's been changed
            dataGridViewRaidGroup.DataSource = null;
            dataGridViewRaidGroup.DataSource = RaidGroup.RaidGroup;
            dataGridViewRaidGroup.Refresh();
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
                        SortGrid("Level DESC, EquipediLevel DESC");

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
            // mmb - todo
            //MessageBox.Show(Message);
        }

        public void DisplayError(String Message)
        {
            MessageBox.Show(Message);
            Log(Message);
        }
        #endregion

        #region " Button Functions "

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

        #endregion        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonAddCharacterToRaidData_Click(object sender, EventArgs e)
        {  
            // This is the Web Site to get the character info from...
            // http://us.battle.net/api/wow/character/Thrall/Purdee/?fields=items

            GetCharacterInfo charInfo = new GetCharacterInfo();
            GuildMember gm = new GuildMember();

            if (charInfo.CollectFullData(URLWowAPI + "character/" + textBoxCharacterRealm.Text + "/" + textBoxCharacterName.Text + "?fields=items"))
            {
                // success!

                // Fill out the data grid with the data we collected                
                gm = charInfo.Guildie;

                // Clear out the Grid Data Source to get it ready for the new data
                dataGridViewRaidGroup.DataSource = null;

                // Add new character to Raid
                RaidGroup.RaidGroup.Add(gm);

                // refresh grid data
                dataGridViewRaidGroup.DataSource = RaidGroup.RaidGroup;

                // refresh the grid data since it's been changed
                dataGridViewRaidGroup.Refresh();

            }
            else
            {
                // Fail!  Save all errors until the end!
                //TODO
            }
        }

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
        private void buttonRaidGroupRefresh_Click(object sender, EventArgs e)
        {
            // TODO
            // 1. Cycle through all the character individually and refresh each one.

            WaitCursor(true);

            try
            {
                ArrayList UpdatedCharacters = new ArrayList();

                foreach (GuildMember oldMember in RaidGroup.RaidGroup)
                {
                    GetCharacterInfo charInfo = new GetCharacterInfo();
                    GuildMember gm = new GuildMember();

                    // This is the Web Site to get the character info from...
                    // http://us.battle.net/api/wow/character/Thrall/Purdee/?fields=items
                    if (charInfo.CollectFullData(URLWowAPI + "character/" + textBoxRealm.Text + "/" + oldMember.Name + "/?fields=items"))
                    {
                        // success!

                        // Now we have the new info
                        gm = charInfo.Guildie;

                        // Compare the new info with the old info
                        //  And make updates as needed...
                        Boolean Updated = false;
                        
                        // Level
                        if (oldMember.Level != gm.Level)
                        {
                            oldMember.Level = gm.Level;
                            Updated = true;
                        }

                        // Achievement Points
                        if (oldMember.AchievementPoints != gm.AchievementPoints)
                        {
                            oldMember.AchievementPoints = gm.AchievementPoints;
                            Updated = true;
                        }

                        // Equiped iLevel
                        if (oldMember.EquipediLevel != gm.EquipediLevel)
                        {
                            oldMember.EquipediLevel = gm.EquipediLevel;
                            Updated = true;
                        }

                        // Max iLevel
                        if (oldMember.MaxiLevel != gm.MaxiLevel)
                        {
                            oldMember.MaxiLevel = gm.MaxiLevel;
                            Updated = true;
                        }

                        if (Updated)
                        {
                            UpdatedCharacters.Add(oldMember.Name);
                        }
                    }
                    else
                    {
                        // Fail!  Save all errors until the end!
                        //TODO
                    }
                }

                // Clear out the Grid Data Source to get it ready for the new data
                dataGridViewRaidGroup.DataSource = null;

                // Now check to see if the Item Level has changed and if their level has changed
                Int32 Count = 0;
                foreach (GuildMember gm in RaidGroup.RaidGroup)
                {
                    if (gm.IsItemLevelChanged())
                    {
                        dataGridViewGuildData.Rows[Count].DefaultCellStyle.BackColor = Color.Aquamarine;
                    }
                    else
                    {
                        dataGridViewGuildData.Rows[Count].DefaultCellStyle.BackColor = Color.White;
                    }

                    Count++;
                }                

                // refresh grid data
                dataGridViewRaidGroup.DataSource = RaidGroup.RaidGroup;

                // refresh the grid data since it's been changed
                dataGridViewRaidGroup.Refresh();

            }
            catch (Exception ex)
            {
                Log("Error: " + ex.Message);
            }

            WaitCursor(false);
        }

        
    }
}
