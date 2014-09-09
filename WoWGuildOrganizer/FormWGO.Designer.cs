namespace WoWGuildOrganizer
{
    partial class FormMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.tabControlWGO = new System.Windows.Forms.TabControl();
            this.tabPageGuildData = new System.Windows.Forms.TabPage();
            this.dataGridViewGuildData = new System.Windows.Forms.DataGridView();
            this.contextMenuStripGuildMembers = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.updateCharacterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPageRaidData = new System.Windows.Forms.TabPage();
            this.labelRaidTab = new System.Windows.Forms.Label();
            this.dataGridViewRaidGroup = new System.Windows.Forms.DataGridView();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.dataGridViewRaidLootDrop = new System.Windows.Forms.DataGridView();
            this.tabPageSettings = new System.Windows.Forms.TabPage();
            this.buttonShowErrors = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.textBoxCharacterName = new System.Windows.Forms.TextBox();
            this.buttonAddCharacterToRaidData = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxCharacterRealm = new System.Windows.Forms.TextBox();
            this.progressBarCharacterAddToRaid = new System.Windows.Forms.ProgressBar();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.buttonDeleteItemCacheData = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonLoad = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxGuildName = new System.Windows.Forms.TextBox();
            this.textBoxRealm = new System.Windows.Forms.TextBox();
            this.progressBarCollectData = new System.Windows.Forms.ProgressBar();
            this.buttonGetGuildInfo = new System.Windows.Forms.Button();
            this.toolStripMain = new System.Windows.Forms.ToolStrip();
            this.toolStripLabelGuild = new System.Windows.Forms.ToolStripLabel();
            this.toolStripTextBoxGuild = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabelRealm = new System.Windows.Forms.ToolStripLabel();
            this.toolStripTextBoxRealm = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripButtonAdd = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonRefresh = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripLabelRefreshStatus = new System.Windows.Forms.ToolStripLabel();
            this.toolStripLabelPickRaid = new System.Windows.Forms.ToolStripLabel();
            this.toolStripComboBoxPickRaid = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripLabelPickBoss = new System.Windows.Forms.ToolStripLabel();
            this.toolStripComboBoxPickBoss = new System.Windows.Forms.ToolStripComboBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.updateThisCharacterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveCharacterUpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveCharacterDownToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteCharacterFromGridToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControlWGO.SuspendLayout();
            this.tabPageGuildData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewGuildData)).BeginInit();
            this.contextMenuStripGuildMembers.SuspendLayout();
            this.tabPageRaidData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewRaidGroup)).BeginInit();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewRaidLootDrop)).BeginInit();
            this.tabPageSettings.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.toolStripMain.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControlWGO
            // 
            this.tabControlWGO.Alignment = System.Windows.Forms.TabAlignment.Left;
            this.tabControlWGO.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlWGO.Controls.Add(this.tabPageGuildData);
            this.tabControlWGO.Controls.Add(this.tabPageRaidData);
            this.tabControlWGO.Controls.Add(this.tabPage1);
            this.tabControlWGO.Controls.Add(this.tabPageSettings);
            this.tabControlWGO.Location = new System.Drawing.Point(0, 24);
            this.tabControlWGO.Multiline = true;
            this.tabControlWGO.Name = "tabControlWGO";
            this.tabControlWGO.SelectedIndex = 0;
            this.tabControlWGO.Size = new System.Drawing.Size(722, 283);
            this.tabControlWGO.TabIndex = 0;
            this.tabControlWGO.SelectedIndexChanged += new System.EventHandler(this.TabControl1_SelectedIndexChanged);
            // 
            // tabPageGuildData
            // 
            this.tabPageGuildData.Controls.Add(this.dataGridViewGuildData);
            this.tabPageGuildData.Location = new System.Drawing.Point(23, 4);
            this.tabPageGuildData.Name = "tabPageGuildData";
            this.tabPageGuildData.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageGuildData.Size = new System.Drawing.Size(695, 275);
            this.tabPageGuildData.TabIndex = 0;
            this.tabPageGuildData.Text = "Guild Data";
            this.tabPageGuildData.UseVisualStyleBackColor = true;
            // 
            // dataGridViewGuildData
            // 
            this.dataGridViewGuildData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewGuildData.ContextMenuStrip = this.contextMenuStripGuildMembers;
            this.dataGridViewGuildData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewGuildData.Location = new System.Drawing.Point(3, 3);
            this.dataGridViewGuildData.Name = "dataGridViewGuildData";
            this.dataGridViewGuildData.Size = new System.Drawing.Size(689, 269);
            this.dataGridViewGuildData.TabIndex = 0;
            this.dataGridViewGuildData.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.DataGridViewGuildData_CellMouseDoubleClick);
            this.dataGridViewGuildData.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.DataGridViewGuildData_ColumnHeaderMouseClick);
            this.dataGridViewGuildData.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.DataGridViewGuildData_RowPostPaint);
            // 
            // contextMenuStripGuildMembers
            // 
            this.contextMenuStripGuildMembers.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.updateCharacterToolStripMenuItem});
            this.contextMenuStripGuildMembers.Name = "contextMenuStrip1";
            this.contextMenuStripGuildMembers.Size = new System.Drawing.Size(167, 26);
            // 
            // updateCharacterToolStripMenuItem
            // 
            this.updateCharacterToolStripMenuItem.Name = "updateCharacterToolStripMenuItem";
            this.updateCharacterToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.updateCharacterToolStripMenuItem.Text = "Update Character";
            this.updateCharacterToolStripMenuItem.Click += new System.EventHandler(this.UpdateCharacterToolStripMenuItem_Click);
            // 
            // tabPageRaidData
            // 
            this.tabPageRaidData.Controls.Add(this.labelRaidTab);
            this.tabPageRaidData.Controls.Add(this.dataGridViewRaidGroup);
            this.tabPageRaidData.Location = new System.Drawing.Point(23, 4);
            this.tabPageRaidData.Name = "tabPageRaidData";
            this.tabPageRaidData.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageRaidData.Size = new System.Drawing.Size(695, 275);
            this.tabPageRaidData.TabIndex = 2;
            this.tabPageRaidData.Text = "Raid Data";
            this.tabPageRaidData.UseVisualStyleBackColor = true;
            // 
            // labelRaidTab
            // 
            this.labelRaidTab.AutoSize = true;
            this.labelRaidTab.Location = new System.Drawing.Point(8, 16);
            this.labelRaidTab.Name = "labelRaidTab";
            this.labelRaidTab.Size = new System.Drawing.Size(0, 13);
            this.labelRaidTab.TabIndex = 2;
            // 
            // dataGridViewRaidGroup
            // 
            this.dataGridViewRaidGroup.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.dataGridViewRaidGroup.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewRaidGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewRaidGroup.Location = new System.Drawing.Point(3, 3);
            this.dataGridViewRaidGroup.Name = "dataGridViewRaidGroup";
            this.dataGridViewRaidGroup.Size = new System.Drawing.Size(689, 269);
            this.dataGridViewRaidGroup.TabIndex = 0;
            this.dataGridViewRaidGroup.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.DataGridViewRaidGroup_CellMouseDoubleClick);
            this.dataGridViewRaidGroup.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.DataGridViewRaidGroup_ColumnHeaderMouseClick);
            this.dataGridViewRaidGroup.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.DataGridViewRaidGroup_RowPostPaint);
            this.dataGridViewRaidGroup.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.DataGridViewRaidGroup_SortCompare);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.dataGridViewRaidLootDrop);
            this.tabPage1.Location = new System.Drawing.Point(23, 4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(695, 275);
            this.tabPage1.TabIndex = 3;
            this.tabPage1.Text = "Raid Loot Drops";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // dataGridViewRaidLootDrop
            // 
            this.dataGridViewRaidLootDrop.AllowUserToAddRows = false;
            this.dataGridViewRaidLootDrop.AllowUserToDeleteRows = false;
            this.dataGridViewRaidLootDrop.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewRaidLootDrop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewRaidLootDrop.Location = new System.Drawing.Point(3, 3);
            this.dataGridViewRaidLootDrop.Name = "dataGridViewRaidLootDrop";
            this.dataGridViewRaidLootDrop.ReadOnly = true;
            this.dataGridViewRaidLootDrop.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewRaidLootDrop.Size = new System.Drawing.Size(689, 269);
            this.dataGridViewRaidLootDrop.TabIndex = 4;
            this.dataGridViewRaidLootDrop.CellToolTipTextNeeded += new System.Windows.Forms.DataGridViewCellToolTipTextNeededEventHandler(this.DataGridViewRaidLootDrop_CellToolTipTextNeeded);
            this.dataGridViewRaidLootDrop.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.DataGridViewRaidLootDrop_DataBindingComplete);
            this.dataGridViewRaidLootDrop.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.DataGridViewRaidLootDrop_MouseDoubleClick);
            // 
            // tabPageSettings
            // 
            this.tabPageSettings.Controls.Add(this.buttonShowErrors);
            this.tabPageSettings.Controls.Add(this.groupBox4);
            this.tabPageSettings.Controls.Add(this.groupBox2);
            this.tabPageSettings.Controls.Add(this.groupBox3);
            this.tabPageSettings.Controls.Add(this.groupBox1);
            this.tabPageSettings.Location = new System.Drawing.Point(23, 4);
            this.tabPageSettings.Name = "tabPageSettings";
            this.tabPageSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSettings.Size = new System.Drawing.Size(695, 275);
            this.tabPageSettings.TabIndex = 1;
            this.tabPageSettings.Text = "Settings";
            this.tabPageSettings.UseVisualStyleBackColor = true;
            // 
            // buttonShowErrors
            // 
            this.buttonShowErrors.Location = new System.Drawing.Point(313, 147);
            this.buttonShowErrors.Name = "buttonShowErrors";
            this.buttonShowErrors.Size = new System.Drawing.Size(75, 23);
            this.buttonShowErrors.TabIndex = 14;
            this.buttonShowErrors.Text = "Show Errors";
            this.buttonShowErrors.UseVisualStyleBackColor = true;
            this.buttonShowErrors.Click += new System.EventHandler(this.ButtonShowErrors_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.textBoxCharacterName);
            this.groupBox4.Controls.Add(this.buttonAddCharacterToRaidData);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.textBoxCharacterRealm);
            this.groupBox4.Controls.Add(this.progressBarCharacterAddToRaid);
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Location = new System.Drawing.Point(465, 13);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(200, 166);
            this.groupBox4.TabIndex = 13;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Raid Data";
            // 
            // textBoxCharacterName
            // 
            this.textBoxCharacterName.Location = new System.Drawing.Point(6, 79);
            this.textBoxCharacterName.Name = "textBoxCharacterName";
            this.textBoxCharacterName.Size = new System.Drawing.Size(188, 20);
            this.textBoxCharacterName.TabIndex = 14;
            // 
            // buttonAddCharacterToRaidData
            // 
            this.buttonAddCharacterToRaidData.Location = new System.Drawing.Point(54, 105);
            this.buttonAddCharacterToRaidData.Name = "buttonAddCharacterToRaidData";
            this.buttonAddCharacterToRaidData.Size = new System.Drawing.Size(91, 23);
            this.buttonAddCharacterToRaidData.TabIndex = 15;
            this.buttonAddCharacterToRaidData.Text = "Add Character";
            this.buttonAddCharacterToRaidData.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 60);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(84, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Character Name";
            // 
            // textBoxCharacterRealm
            // 
            this.textBoxCharacterRealm.Location = new System.Drawing.Point(6, 32);
            this.textBoxCharacterRealm.Name = "textBoxCharacterRealm";
            this.textBoxCharacterRealm.Size = new System.Drawing.Size(188, 20);
            this.textBoxCharacterRealm.TabIndex = 13;
            // 
            // progressBarCharacterAddToRaid
            // 
            this.progressBarCharacterAddToRaid.Location = new System.Drawing.Point(6, 134);
            this.progressBarCharacterAddToRaid.Name = "progressBarCharacterAddToRaid";
            this.progressBarCharacterAddToRaid.Size = new System.Drawing.Size(188, 23);
            this.progressBarCharacterAddToRaid.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBarCharacterAddToRaid.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(37, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Realm";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.buttonDeleteItemCacheData);
            this.groupBox2.Location = new System.Drawing.Point(259, 71);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 59);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Item Cache Data Manager";
            // 
            // buttonDeleteItemCacheData
            // 
            this.buttonDeleteItemCacheData.Location = new System.Drawing.Point(54, 21);
            this.buttonDeleteItemCacheData.Name = "buttonDeleteItemCacheData";
            this.buttonDeleteItemCacheData.Size = new System.Drawing.Size(75, 23);
            this.buttonDeleteItemCacheData.TabIndex = 4;
            this.buttonDeleteItemCacheData.Text = "Open";
            this.buttonDeleteItemCacheData.UseVisualStyleBackColor = true;
            this.buttonDeleteItemCacheData.Click += new System.EventHandler(this.ButtonDeleteItemCacheData_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.buttonSave);
            this.groupBox3.Controls.Add(this.buttonLoad);
            this.groupBox3.Location = new System.Drawing.Point(259, 6);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(200, 59);
            this.groupBox3.TabIndex = 9;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Save and Load Guild Data";
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(19, 19);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 23);
            this.buttonSave.TabIndex = 4;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.ButtonSave_Click);
            // 
            // buttonLoad
            // 
            this.buttonLoad.Location = new System.Drawing.Point(100, 19);
            this.buttonLoad.Name = "buttonLoad";
            this.buttonLoad.Size = new System.Drawing.Size(75, 23);
            this.buttonLoad.TabIndex = 5;
            this.buttonLoad.Text = "Load";
            this.buttonLoad.UseVisualStyleBackColor = true;
            this.buttonLoad.Click += new System.EventHandler(this.ButtonLoad_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.textBoxGuildName);
            this.groupBox1.Controls.Add(this.textBoxRealm);
            this.groupBox1.Controls.Add(this.progressBarCollectData);
            this.groupBox1.Controls.Add(this.buttonGetGuildInfo);
            this.groupBox1.Location = new System.Drawing.Point(3, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(227, 123);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Collect Data";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(118, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Guild Name";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Realm";
            // 
            // textBoxGuildName
            // 
            this.textBoxGuildName.Location = new System.Drawing.Point(121, 39);
            this.textBoxGuildName.Name = "textBoxGuildName";
            this.textBoxGuildName.Size = new System.Drawing.Size(100, 20);
            this.textBoxGuildName.TabIndex = 1;
            // 
            // textBoxRealm
            // 
            this.textBoxRealm.Location = new System.Drawing.Point(5, 39);
            this.textBoxRealm.Name = "textBoxRealm";
            this.textBoxRealm.Size = new System.Drawing.Size(100, 20);
            this.textBoxRealm.TabIndex = 0;
            // 
            // progressBarCollectData
            // 
            this.progressBarCollectData.Location = new System.Drawing.Point(6, 94);
            this.progressBarCollectData.Name = "progressBarCollectData";
            this.progressBarCollectData.Size = new System.Drawing.Size(215, 23);
            this.progressBarCollectData.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBarCollectData.TabIndex = 1;
            // 
            // buttonGetGuildInfo
            // 
            this.buttonGetGuildInfo.Location = new System.Drawing.Point(63, 65);
            this.buttonGetGuildInfo.Name = "buttonGetGuildInfo";
            this.buttonGetGuildInfo.Size = new System.Drawing.Size(100, 23);
            this.buttonGetGuildInfo.TabIndex = 2;
            this.buttonGetGuildInfo.Text = "Get Guild Info";
            this.buttonGetGuildInfo.UseVisualStyleBackColor = true;
            this.buttonGetGuildInfo.Click += new System.EventHandler(this.ButtonGetGuildInfo_Click);
            // 
            // toolStripMain
            // 
            this.toolStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabelGuild,
            this.toolStripTextBoxGuild,
            this.toolStripLabelRealm,
            this.toolStripTextBoxRealm,
            this.toolStripButtonAdd,
            this.toolStripButtonRefresh,
            this.toolStripSeparator1,
            this.toolStripProgressBar1,
            this.toolStripLabelRefreshStatus,
            this.toolStripLabelPickRaid,
            this.toolStripComboBoxPickRaid,
            this.toolStripLabelPickBoss,
            this.toolStripComboBoxPickBoss});
            this.toolStripMain.Location = new System.Drawing.Point(0, 0);
            this.toolStripMain.Name = "toolStripMain";
            this.toolStripMain.Size = new System.Drawing.Size(721, 25);
            this.toolStripMain.TabIndex = 1;
            this.toolStripMain.Text = "toolStrip2";
            // 
            // toolStripLabelGuild
            // 
            this.toolStripLabelGuild.Name = "toolStripLabelGuild";
            this.toolStripLabelGuild.Size = new System.Drawing.Size(38, 22);
            this.toolStripLabelGuild.Text = "Guild:";
            // 
            // toolStripTextBoxGuild
            // 
            this.toolStripTextBoxGuild.Name = "toolStripTextBoxGuild";
            this.toolStripTextBoxGuild.Size = new System.Drawing.Size(100, 25);
            // 
            // toolStripLabelRealm
            // 
            this.toolStripLabelRealm.Name = "toolStripLabelRealm";
            this.toolStripLabelRealm.Size = new System.Drawing.Size(43, 22);
            this.toolStripLabelRealm.Text = "Realm:";
            // 
            // toolStripTextBoxRealm
            // 
            this.toolStripTextBoxRealm.Name = "toolStripTextBoxRealm";
            this.toolStripTextBoxRealm.Size = new System.Drawing.Size(100, 25);
            // 
            // toolStripButtonAdd
            // 
            this.toolStripButtonAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonAdd.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonAdd.Image")));
            this.toolStripButtonAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAdd.Name = "toolStripButtonAdd";
            this.toolStripButtonAdd.Size = new System.Drawing.Size(23, 24);
            this.toolStripButtonAdd.Text = "Add New Character";
            this.toolStripButtonAdd.Visible = false;
            this.toolStripButtonAdd.Click += new System.EventHandler(this.ToolStripButtonAdd_Click);
            // 
            // toolStripButtonRefresh
            // 
            this.toolStripButtonRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonRefresh.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonRefresh.Image")));
            this.toolStripButtonRefresh.ImageTransparentColor = System.Drawing.Color.Black;
            this.toolStripButtonRefresh.Name = "toolStripButtonRefresh";
            this.toolStripButtonRefresh.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonRefresh.Text = "Refresh";
            this.toolStripButtonRefresh.Click += new System.EventHandler(this.ToolStripButtonRefresh_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 24);
            this.toolStripProgressBar1.Visible = false;
            // 
            // toolStripLabelRefreshStatus
            // 
            this.toolStripLabelRefreshStatus.Name = "toolStripLabelRefreshStatus";
            this.toolStripLabelRefreshStatus.Size = new System.Drawing.Size(173, 24);
            this.toolStripLabelRefreshStatus.Text = "? Characters found in ? seconds";
            this.toolStripLabelRefreshStatus.Visible = false;
            // 
            // toolStripLabelPickRaid
            // 
            this.toolStripLabelPickRaid.Name = "toolStripLabelPickRaid";
            this.toolStripLabelPickRaid.Size = new System.Drawing.Size(58, 24);
            this.toolStripLabelPickRaid.Text = "Pick Raid:";
            this.toolStripLabelPickRaid.Visible = false;
            // 
            // toolStripComboBoxPickRaid
            // 
            this.toolStripComboBoxPickRaid.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toolStripComboBoxPickRaid.Name = "toolStripComboBoxPickRaid";
            this.toolStripComboBoxPickRaid.Size = new System.Drawing.Size(175, 25);
            this.toolStripComboBoxPickRaid.Visible = false;
            this.toolStripComboBoxPickRaid.SelectedIndexChanged += new System.EventHandler(this.toolStripComboBoxPickRaid_SelectedIndexChanged);
            // 
            // toolStripLabelPickBoss
            // 
            this.toolStripLabelPickBoss.Name = "toolStripLabelPickBoss";
            this.toolStripLabelPickBoss.Size = new System.Drawing.Size(59, 22);
            this.toolStripLabelPickBoss.Text = "Pick Boss:";
            this.toolStripLabelPickBoss.Visible = false;
            // 
            // toolStripComboBoxPickBoss
            // 
            this.toolStripComboBoxPickBoss.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toolStripComboBoxPickBoss.DropDownWidth = 150;
            this.toolStripComboBoxPickBoss.Name = "toolStripComboBoxPickBoss";
            this.toolStripComboBoxPickBoss.Size = new System.Drawing.Size(121, 25);
            this.toolStripComboBoxPickBoss.Visible = false;
            this.toolStripComboBoxPickBoss.SelectedIndexChanged += new System.EventHandler(this.toolStripComboBoxPickBoss_SelectedIndexChanged);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.updateThisCharacterToolStripMenuItem,
            this.moveCharacterUpToolStripMenuItem,
            this.moveCharacterDownToolStripMenuItem,
            this.deleteCharacterFromGridToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(216, 92);
            // 
            // updateThisCharacterToolStripMenuItem
            // 
            this.updateThisCharacterToolStripMenuItem.Name = "updateThisCharacterToolStripMenuItem";
            this.updateThisCharacterToolStripMenuItem.Size = new System.Drawing.Size(215, 22);
            this.updateThisCharacterToolStripMenuItem.Text = "Update this Character";
            this.updateThisCharacterToolStripMenuItem.Click += new System.EventHandler(this.UpdateThisCharacterToolStripMenuItem_Click);
            // 
            // moveCharacterUpToolStripMenuItem
            // 
            this.moveCharacterUpToolStripMenuItem.Name = "moveCharacterUpToolStripMenuItem";
            this.moveCharacterUpToolStripMenuItem.Size = new System.Drawing.Size(215, 22);
            this.moveCharacterUpToolStripMenuItem.Text = "Move Character Up";
            this.moveCharacterUpToolStripMenuItem.Click += new System.EventHandler(this.MoveCharacterUpToolStripMenuItem_Click);
            // 
            // moveCharacterDownToolStripMenuItem
            // 
            this.moveCharacterDownToolStripMenuItem.Name = "moveCharacterDownToolStripMenuItem";
            this.moveCharacterDownToolStripMenuItem.Size = new System.Drawing.Size(215, 22);
            this.moveCharacterDownToolStripMenuItem.Text = "Move Character Down";
            this.moveCharacterDownToolStripMenuItem.Click += new System.EventHandler(this.MoveCharacterDownToolStripMenuItem_Click);
            // 
            // deleteCharacterFromGridToolStripMenuItem
            // 
            this.deleteCharacterFromGridToolStripMenuItem.Name = "deleteCharacterFromGridToolStripMenuItem";
            this.deleteCharacterFromGridToolStripMenuItem.Size = new System.Drawing.Size(215, 22);
            this.deleteCharacterFromGridToolStripMenuItem.Text = "Delete Character from Grid";
            this.deleteCharacterFromGridToolStripMenuItem.Click += new System.EventHandler(this.DeleteCharacterFromGridToolStripMenuItem_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(721, 307);
            this.Controls.Add(this.toolStripMain);
            this.Controls.Add(this.tabControlWGO);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormMain";
            this.Text = "WoW Guild Organizer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControlWGO.ResumeLayout(false);
            this.tabPageGuildData.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewGuildData)).EndInit();
            this.contextMenuStripGuildMembers.ResumeLayout(false);
            this.tabPageRaidData.ResumeLayout(false);
            this.tabPageRaidData.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewRaidGroup)).EndInit();
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewRaidLootDrop)).EndInit();
            this.tabPageSettings.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.toolStripMain.ResumeLayout(false);
            this.toolStripMain.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControlWGO;
        private System.Windows.Forms.TabPage tabPageGuildData;
        private System.Windows.Forms.DataGridView dataGridViewGuildData;
        private System.Windows.Forms.TabPage tabPageSettings;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxGuildName;
        private System.Windows.Forms.TextBox textBoxRealm;
        private System.Windows.Forms.ProgressBar progressBarCollectData;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonLoad;
        private System.Windows.Forms.Button buttonGetGuildInfo;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button buttonDeleteItemCacheData;
        private System.Windows.Forms.TabPage tabPageRaidData;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox textBoxCharacterName;
        private System.Windows.Forms.Button buttonAddCharacterToRaidData;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxCharacterRealm;
        private System.Windows.Forms.ProgressBar progressBarCharacterAddToRaid;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DataGridView dataGridViewRaidGroup;
        private System.Windows.Forms.Button buttonShowErrors;
        private System.Windows.Forms.Label labelRaidTab;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.DataGridView dataGridViewRaidLootDrop;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripGuildMembers;
        private System.Windows.Forms.ToolStripMenuItem updateCharacterToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolStripMain;
        private System.Windows.Forms.ToolStripButton toolStripButtonRefresh;
        private System.Windows.Forms.ToolStripLabel toolStripLabelGuild;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBoxGuild;
        private System.Windows.Forms.ToolStripLabel toolStripLabelRealm;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBoxRealm;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.ToolStripLabel toolStripLabelRefreshStatus;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem updateThisCharacterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moveCharacterUpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moveCharacterDownToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteCharacterFromGridToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton toolStripButtonAdd;
        private System.Windows.Forms.ToolStripLabel toolStripLabelPickRaid;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBoxPickRaid;
        private System.Windows.Forms.ToolStripLabel toolStripLabelPickBoss;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBoxPickBoss;
    }
}

