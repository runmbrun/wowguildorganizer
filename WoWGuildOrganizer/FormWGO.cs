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
    /// Class for the main form of the World of Warcraft Guild Organizer
    /// </summary>
    public partial class FormMain : Form
    {
        #region Class Variables

        /// <summary>
        /// static string of the URL for the Blizzard API
        /// </summary>
        public readonly string URLWowAPI = @"http://us.battle.net/api/wow/";

        /// <summary>
        /// String of how a basic guild sort should be
        /// </summary>
        private readonly string sortGuild = "Level DESC, EquipediLevel DESC, MaxiLevel DESC, AchievementPoints DESC";

        /// <summary>
        /// String of how a basic raid sort should be
        /// </summary>
        private readonly string sortRaid = "Role DESC, Level DESC, EquipediLevel DESC";

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

        #region Class Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="FormMain" /> class
        /// </summary>
        public FormMain()
        {
            this.InitializeComponent();

            // Load up the guild name textbox if it's been saved before
            if (!string.IsNullOrEmpty(Properties.Settings.Default.GuildName))
            {
                toolStripTextBoxGuild.Text = Properties.Settings.Default.GuildName;
            }

            // Load up the realm name textbox if it's been saved before
            if (!string.IsNullOrEmpty(Properties.Settings.Default.Realm))
            {
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

                    // Check to see what the currently saved guild is
                    if (this.savedCharacters.SavedCharacters.Count > 0)
                    {
                        if (this.savedCharacters.Guild != string.Empty)
                        {
                            toolStripTextBoxGuild.Text = this.savedCharacters.Guild;
                        }

                        toolStripLabelRefreshStatus.Visible = true;
                        toolStripLabelRefreshStatus.Text = string.Format("Total Characters in guild: {0}", this.savedCharacters.SavedCharacters.Count);
                    }
                    else
                    {
                        toolStripTextBoxGuild.Text = string.Empty;
                        toolStripLabelRefreshStatus.Visible = false;
                        toolStripLabelRefreshStatus.Text = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                if (!ex.Message.StartsWith("Could not find file"))
                {
                    // Otherwise there was an error reading the file
                    Logging.DisplayError(string.Format("Cannot read saved file: ", ex.Message));
                }
                else
                {
                    Logging.DisplayError(string.Format("Cannot find saved file: ", ex.Message));
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
                    // Otherwise there was an error reading the file
                    Logging.DisplayError(string.Format("Cannot read saved file: ", ex.Message));
                }
                else
                {
                    Logging.DisplayError(string.Format("Cannot find saved file: ", ex.Message));
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

        #region Class Properties

        /// <summary>
        /// Gets or sets a persistent cache of all the WoW Item Data needed
        /// </summary>
        public static ItemCache Items { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to let the app know if it should attempt to connect
        ///  to the Blizzard Battle.net web site
        /// </summary>
        public static bool WebSiteOnline { get; set; }

        #endregion

        #region Main Form Functions

        /// <summary>
        /// Before the form closes, save the important variables
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">e parameter</param>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.savedCharacters.Guild))
            {
                Properties.Settings.Default.GuildName = toolStripTextBoxGuild.Text;
            }

            if (!string.IsNullOrEmpty(this.savedCharacters.Realm))
            {
                Properties.Settings.Default.Realm = toolStripTextBoxRealm.Text;
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
            // refresh the grid data since it's been changed
            dataGridViewRaidGroup.DataSource = null;
            dataGridViewRaidGroup.DataSource = this.raidGroup.RaidGroup;
            this.UpdateRaidGrid();
            
            this.SortGrid(this.sortGuild);
        }

        #endregion

        #region Main Tab Functions

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
                toolStripButtonRefresh.Visible = true;
                toolStripButtonSort.Visible = true;

                // Update Text as needed
                toolStripLabelGuild.Text = "Guild:";

                // Check to see what the currently saved guild is
                if (this.savedCharacters.SavedCharacters != null && this.savedCharacters.SavedCharacters.Count > 0)
                {
                    toolStripTextBoxGuild.Text = this.savedCharacters.Guild;
                    toolStripLabelRefreshStatus.Visible = true;
                    toolStripLabelRefreshStatus.Text = string.Format("Total Characters in guild: {0}", this.savedCharacters.SavedCharacters.Count);
                }
                else
                {
                    toolStripTextBoxGuild.Text = string.Empty;
                    toolStripLabelRefreshStatus.Visible = false;
                    toolStripLabelRefreshStatus.Text = string.Empty;
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
                toolStripButtonRefresh.Visible = true;
                toolStripButtonSort.Visible = true;

                // Update Text as needed
                toolStripLabelGuild.Text = "Character:";

                // Check to see what the currently saved guild is
                if (this.raidGroup.RaidGroup.Count > 0)
                {
                    toolStripLabelRefreshStatus.Visible = true;
                    toolStripLabelRefreshStatus.Text = string.Format("Total Characters in raid: {0}", this.raidGroup.RaidGroup.Count);
                }
                else
                {
                    toolStripLabelRefreshStatus.Visible = false;
                    toolStripLabelRefreshStatus.Text = string.Empty;
                }

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
                toolStripButtonSort.Visible = false;

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
                toolStripButtonSort.Visible = false;
            }
        }

        #endregion

        #region Guild Data Tab - DataGridView Functions

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

            if (this.savedCharacters.SavedCharacters != null)
            {
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
                // Get the character data
                GuildMember character = (GuildMember)this.savedCharacters.SavedCharacters[currentRow];

                // do the audit
                this.AuditCharacter(character);
            }            
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

            if (this.tabControlWGO.SelectedTab.Text == "Guild Data")
            {
                dataGridViewGuildData.DataSource = null;

                this.savedCharacters.SavedCharacters.Sort(new ObjectComparer(sorting, multipleSort));

                // refresh grid data
                dataGridViewGuildData.DataSource = this.savedCharacters.SavedCharacters;

                // Now update the grid
                this.UpdateGrid();
            }
            else if (this.tabControlWGO.SelectedTab.Text == "Raid Data")
            {
                this.raidGroup.RaidGroup.Sort(new ObjectComparer(sorting, multipleSort));

                // refresh grid data
                dataGridViewRaidGroup.DataSource = null;
                dataGridViewRaidGroup.DataSource = this.raidGroup.RaidGroup;

                // Now update the grid
                this.UpdateRaidGrid();
            }

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

                if (this.tabControlWGO.SelectedTab.Text == "Guild Data")
                {
                    foreach (DataGridViewColumn col in dataGridViewGuildData.Columns)
                    {
                        if (fieldName == col.HeaderText)
                        {
                            col.HeaderCell.SortGlyphDirection = direction;
                        }
                    }
                }
                else if (this.tabControlWGO.SelectedTab.Text == "Raid Data")
                {
                    foreach (DataGridViewColumn col in dataGridViewRaidGroup.Columns)
                    {
                        if (fieldName == col.HeaderText)
                        {
                            col.HeaderCell.SortGlyphDirection = direction;
                        }
                    }
                }
            }

            if (this.tabControlWGO.SelectedTab.Text == "Guild Data")
            {
                toolStripLabelRefreshStatus.Text = string.Format("Total Characters in guild: {0}", this.savedCharacters.SavedCharacters.Count);
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
        /// This captures errors from the Guild's Data View Grid, and does not let them be shown
        /// These errors are ignored on purpose!
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">e parameter</param>
        private void DataGridViewGuildData_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // suppress all of these errors...
            // These errors are from the clicks on the datagridview while it is being updated
        }

        #endregion

        #region Guild Data Tab - Background Worker

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
            this.toolStripProgressBar1.Value = e.ProgressPercentage;
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

                        if (guildInfo.CollectData(string.Format("{0}guild/{1}/{2}?fields=members", this.URLWowAPI, toolStripTextBoxRealm.Text, toolStripTextBoxGuild.Text)))
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

                                if (this.savedCharacters.Guild == toolStripTextBoxGuild.Text && 
                                    this.savedCharacters.Realm == toolStripTextBoxRealm.Text)
                                {
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
                                                //   since new will always be blank
                                                if (newmember.MaxiLevel == 0 && oldmember.MaxiLevel != 0)
                                                {
                                                    newmember.MaxiLevel = oldmember.MaxiLevel;
                                                    newmember.ClearMaxItemLevelFlag();
                                                }

                                                // always check to carry over the max iLevel value from old to new
                                                //   since new will always be blank
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
                                    // Either the guild or the realm changed... either way, a guild member compare isnt' needed
                                    this.savedCharacters.SavedCharacters = guildInfo.Characters;

                                    // Store the new guild and realm names
                                    this.savedCharacters.Guild = toolStripTextBoxGuild.Text;
                                    this.savedCharacters.Realm = toolStripTextBoxRealm.Text;
                                }
                            }
                            else
                            {
                                // First time collection of the guild...
                                //  Fill out the data grid with the data we collected
                                this.savedCharacters.SavedCharacters = guildInfo.Characters;

                                // Store the guild and realm names
                                this.savedCharacters.Guild = this.toolStripTextBoxGuild.Text;
                                this.savedCharacters.Realm = this.toolStripTextBoxRealm.Text;
                            }
                        }

                        // Now get the individual Character data
                        int count = 0;
                        int total = this.savedCharacters.SavedCharacters.Count;

                        try
                        {
                            // Go through all the guild members
                            foreach (GuildMember gm in this.savedCharacters.SavedCharacters)
                            {
                                // Progress update
                                this.UpdateLabelMT(this.savedCharacters.SavedCharacters.Count.ToString() + " total characters - On Character #" + (count + 1));

                                // Only check for Item Level for characters over level 10
                                //  Otherwise these characters won't be in the Armory
                                if (Convert.ToInt32(gm.Level) >= 10)
                                {
                                    GuildMember charInfo = this.GetCharacterInformation(gm.Name, this.savedCharacters.Realm);

                                    if (charInfo != null)
                                    {
                                        // success!

                                        // Fill out the data grid with the data we collected
                                        if (gm.Level != charInfo.Level)
                                        {
                                            gm.Level = charInfo.Level;
                                        }

                                        if (gm.MaxiLevel != charInfo.MaxiLevel)
                                        {
                                            gm.MaxiLevel = charInfo.MaxiLevel;
                                        }

                                        if (gm.EquipediLevel != charInfo.EquipediLevel)
                                        {
                                            gm.EquipediLevel = charInfo.EquipediLevel;
                                        }
                                        
                                        gm.Profession1 = charInfo.Profession1;
                                        gm.Profession2 = charInfo.Profession2;
                                        gm.Spec = charInfo.Spec;
                                        gm.Role = charInfo.Role;
                                        gm.ItemAudits = charInfo.ItemAudits;
                                        gm.Class = charInfo.Class;
                                        gm.Name = charInfo.Name;
                                        gm.Race = charInfo.Race;
                                        gm.Realm = charInfo.Realm;

                                        if (gm.AchievementPoints != charInfo.AchievementPoints && charInfo.AchievementPoints > 0)
                                        {
                                            gm.AchievementPoints = charInfo.AchievementPoints;
                                        }
                                        else if (gm.AchievementPoints != charInfo.AchievementPoints)
                                        {
                                            Logging.Warning(string.Format("Old vs New Achievement points.  [{0}] vs [{1}].", gm.AchievementPoints, charInfo.AchievementPoints));
                                        }

                                        gm.SetArmoryCheckTime();
                                        gm.SetItemLevelCheckTime();
                                    }
                                    else
                                    {
                                        // Failure!
                                        Logging.Error(string.Format("Failed to parse gear score for {0}", gm.Name));
                                    }
                                }

                                // Progress update for Progress Bar
                                double tempNum = (double)count++ / (double)total * 100;
                                async.ReportProgress((int)tempNum);
                            }
                        }
                        catch (Exception ex)
                        {
                            Logging.Error(ex.Message);
                        }

                        // Data has been gathered now...
                        async.ReportProgress(100);

                        // re-sort by Item Level
                        this.SortGridMT(this.sortGuild);

                        // Stop the stopwatch
                        this.sw.Stop();
                    }
                    catch (Exception ex)
                    {
                        Logging.Error(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.Error(ex.Message);
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

                // Change the button text so that this can be cancelled
                this.EnableRefreshButton(true);

                // Show the progress bar
                this.toolStripProgressBar1.Visible = false;

                // start the wait cursor
                this.WaitCursor(false);

                this.UpdateLabelMT(string.Format("{0} total characters in {1} seconds", this.savedCharacters.SavedCharacters.Count, (this.sw.GetElapsedTime() / 1000).ToString("0")));
            }
        }

        #endregion

        #region Setting Tab - Functionality

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

                        // Sort the data
                        this.SortGrid(this.sortGuild);

                        // update the text boxes                        
                        toolStripTextBoxRealm.Text = this.savedCharacters.Realm;
                        toolStripTextBoxGuild.Text = this.savedCharacters.Guild;

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

        #region Raid Tab - Functionality

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
                    // Clear out the Grid Data Source to get it ready for the new data
                    dataGridViewRaidGroup.DataSource = null;

                    // Add new character to Raid
                    this.raidGroup.RaidGroup.Add(gm);
                                        
                    // refresh grid data
                    dataGridViewRaidGroup.DataSource = null;
                    dataGridViewRaidGroup.DataSource = this.raidGroup.RaidGroup;

                    // refresh the grid data since it's been changed
                    this.UpdateRaidGrid();
                }

                this.WaitCursor(false);
            }
        }

        /// <summary>
        /// Grabs all the information about 1 character
        /// This is the Web Site to get the character info from...
        /// <c>http://us.battle.net/api/wow/character/Thrall/Purdee?fields=items,professions,talents</c>
        /// </summary>
        /// <param name="name">string of the name of the character to get the information about</param>
        /// <param name="realm">string of the realm of the character to get the information about</param>
        /// <returns>a Guild Member type variable</returns>
        private GuildMember GetCharacterInformation(string name, string realm)
        {            
            GetCharacterInfo charInfo = new GetCharacterInfo();
            GuildMember gm = new GuildMember();

            if (charInfo.CollectFullData(string.Format("{0}character/{1}/{2}?fields=items,professions,talents", this.URLWowAPI, realm, name)))
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

            // First check to make sure a row is selected
            if (currentRow < 0)
            {
                MessageBox.Show("Error: No row was selected.");
            }
            else
            {
                // Get the character data
                GuildMember character = (GuildMember)this.raidGroup.RaidGroup[currentRow];

                // do the audit
                this.AuditCharacter(character);
            }
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
                dataGridViewRaidGroup.DataSource = null;
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
                toolStripLabelRefreshStatus.Text = "Average Raid Team iLevel = " + Convert.ToInt32(Convert.ToDecimal(ilvl) / Convert.ToDecimal(count)).ToString() + " for " + this.raidGroup.RaidGroup.Count.ToString() + " total characters";
            }

            // refresh the grid data since it's been changed
            dataGridViewRaidGroup.Refresh();
        }

        #endregion

        #region Raid Loot Tab - Functionality

        /// <summary>
        /// Fires when the tool strip combo box in the Raid Loot tab is changed
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">e parameter</param>
        private void ToolStripComboBoxPickRaid_SelectedIndexChanged(object sender, EventArgs e)
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
        private void ToolStripComboBoxPickBoss_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.WaitCursor(true);

            int[] itemIds = null;

            // Fill out the results
            if (this.raidLoot[toolStripComboBoxPickRaid.SelectedItem.ToString()].ContainsKey(toolStripComboBoxPickBoss.SelectedItem.ToString()))
            {
                itemIds = this.raidLoot[toolStripComboBoxPickRaid.SelectedItem.ToString()][toolStripComboBoxPickBoss.SelectedItem.ToString()];

                // Remove focus from drop down so middle mouse wheel doesn't change accidently
                this.dataGridViewRaidLootDrop.Focus();

                // Make sure there is at least 1 raid member in the group
                if (this.raidGroup.RaidGroup.Count > 0)
                {
                    // Now get the data
                    RaidInfo raidInfo = new RaidInfo();
                    DataTable loot = null;

                    loot = raidInfo.GetLootResults(itemIds, this.raidGroup.RaidGroup);

                    // Check to see any loot was found for this boss
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
                                    dr["OldItemId"] = 0;
                                    loot.Rows.Add(dr);
                                }
                            }
                        }

                        if (loot.Select("upgrade >= 0", "upgrade desc").Length > 0)
                        {
                            dataGridViewRaidLootDrop.DataSource = loot.Select("upgrade >= 0", "upgrade desc").CopyToDataTable();
                            dataGridViewRaidLootDrop.Columns["ItemId"].Visible = false;
                            dataGridViewRaidLootDrop.Columns["OldItemId"].Visible = false;
                            dataGridViewRaidLootDrop.AutoResizeColumns();

                            foreach (DataGridViewRow row in dataGridViewRaidLootDrop.Rows)
                            {
                                int id1 = Convert.ToInt32(row.Cells["ItemId"].Value);
                                int id2 = Convert.ToInt32(row.Cells["OldItemId"].Value);
                                ItemInfo item1 = Items.GetItem(id1);
                                ItemInfo item2 = Items.GetItem(id2);
                                string updatedTooltip = item2.Tooltip;

                                // Search and replace the iLvl with the current one
                                if (updatedTooltip != string.Empty)
                                {
                                    int firstOne = updatedTooltip.IndexOf("\n") + 1;
                                    int secondOne = updatedTooltip.IndexOf("\n", firstOne + 1);
                                    updatedTooltip = updatedTooltip.Substring(0, firstOne) + row.Cells["OldItemILevel"].Value + updatedTooltip.Substring(secondOne);
                                }                               

                                // Fill out the tooltips
                                row.Cells["ItemName"].ToolTipText = item1.Tooltip;
                                row.Cells["OldItemILevel"].ToolTipText = updatedTooltip;

                                // Mark all Unneeded items with a dim gray color
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
                        dataGridViewRaidLootDrop.Update();
                        dataGridViewRaidLootDrop.Refresh();
                    }
                }
                else
                {
                    Logging.DisplayError("No Raid Members found.");
                }
            }
            else
            {
                Logging.DisplayError("No data found for this raid boss.");
            }

            this.WaitCursor(false);
        }

        #endregion
        
        #region Raid Loot Tab - DataGridView Functions

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

        #region Tool Strip Menu - Functionality

        /// <summary>
        /// Fires when the update character right click is pressed
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">e parameter</param>
        private void UpdateCharacterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Update just the selected rows of characters
            this.UpdateCharacters(this.dataGridViewGuildData.SelectedRows.Cast<DataGridViewRow>().ToList(), true);
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
                        Logging.Log("Search Started...");

                        // start the wait cursor
                        this.WaitCursor(true);

                        // Change the button text so that this can be cancelled
                        this.EnableRefreshButton(false);

                        // Show the progress bar
                        this.toolStripProgressBar1.Visible = true;

                        // Start the stop watch
                        this.sw.Start();

                        // Kickoff the worker thread to begin it's DoWork function.
                        this.guildInfoAsyncWorker.RunWorkerAsync();
                    }
                    else
                    {
                        // Cancel it - todo
                    }
                }
            }
            else if (this.tabControlWGO.SelectedTab.Text == "Raid Data")
            {
                this.UpdateCharacters(this.dataGridViewRaidGroup.Rows.Cast<DataGridViewRow>().ToList(), false);
            }
            else if (this.tabControlWGO.SelectedTab.Text == "Raid Loot Drops")
            {
                if (toolStripComboBoxPickBoss.SelectedIndex == -1 || toolStripComboBoxPickRaid.SelectedIndex == -1)
                {
                    // Error - must select both a raid and a boss from the 2 combo boxes
                    Logging.DisplayError("A raid and a boss must both be selected");
                }
                else
                {
                    this.ToolStripComboBoxPickBoss_SelectedIndexChanged(null, null);
                }
            }
        }

        /// <summary>
        /// Fires when the sort button is pressed in the tool strip menu
        /// <c>If tab is on the Guild Data, then sorts by: "Level DESC, EquippediLevel DESC, MaxiLevel DESC, AchievementPoints DESC"</c>
        /// <c>If tab is on the Raid Data, then sorts by: "Role DESC, EquippediLevel DESC"</c>
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">e parameter</param>
        private void ToolStripButtonSort_Click(object sender, EventArgs e)
        {
            if (this.tabControlWGO.SelectedTab.Text == "Guild Data")
            {
                this.SortGrid(this.sortGuild);
            }
            else if (this.tabControlWGO.SelectedTab.Text == "Raid Data")
            {
                this.SortGrid(this.sortRaid);
            }
        }
        
        /// <summary>
        /// Fires when the "Update this Character" context strip menu item is clicked on the raid data tab
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">e parameter</param>
        private void UpdateThisCharacterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.UpdateCharacters(this.dataGridViewRaidGroup.SelectedRows.Cast<DataGridViewRow>().ToList(), false);
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
        
        #endregion

        #region Support Functions

        /// <summary>
        /// Updates a collection of characters with the current information
        /// </summary>
        /// <param name="chars">A collection of selected rows in a data grid view</param>
        /// <param name="guildUpdate">Flag if the update is in the guild or raid data grid view</param>
        private void UpdateCharacters(List<DataGridViewRow> chars, bool guildUpdate)
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
                    else
                    {
                        Logging.DisplayError("Character Update Unsuccessful.");
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
                Logging.Error(ex.Message);
            }

            this.WaitCursor(false);
        }

        /// <summary>
        /// Will Audit the character that is passed in
        /// </summary>
        /// <param name="character">character that is to be audited</param>
        private void AuditCharacter(GuildMember character)
        {
            this.Cursor = Cursors.WaitCursor;

            /*
            // This will be a audit function.
            //  So if a character is double clicked, an audit table will pop up.
            //  The Audit Table will show the following things:
            //  1. One line for each character slot
            //  2. Each line will contain a deduction if the item is ...
            //     a. missing
            //     b. not enchanted
            //     c. not fully gemmed
            //     d. item level - if too low compared to rest of gear
            // Future Ideas
            //  1. Missing Glyphs
            //  2. Raid Progression
            */

            try
            {
                // Create the new form to be used
                FormItemAudit charAudit = new FormItemAudit();

                // Pass the Data to the Form
                if (charAudit.PassData(character))
                {
                    // Show the new form
                    charAudit.Show();
                }
                else
                {
                    Logging.DisplayError(string.Format("Can't Audit Character: {0}.", character.Name));
                }
            }
            catch (Exception ex)
            {
                Logging.DisplayError(string.Format("Can't Audit Character: {0}. Error: {1}.", character.Name, ex.Message));
            }

            this.Cursor = Cursors.Default;
        }

        #endregion 
    }
}
