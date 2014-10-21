﻿namespace WoWGuildOrganizer
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
            this.dataGridViewRaidGroup = new System.Windows.Forms.DataGridView();
            this.contextMenuStripRaidMembers = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.updateThisCharacterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveCharacterUpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveCharacterDownToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteCharacterFromGridToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.dataGridViewRaidLootDrop = new System.Windows.Forms.DataGridView();
            this.tabPageSettings = new System.Windows.Forms.TabPage();
            this.groupBoxErrors = new System.Windows.Forms.GroupBox();
            this.buttonShowErrors = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.buttonDeleteItemCacheData = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonLoad = new System.Windows.Forms.Button();
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
            this.toolStripButtonSort = new System.Windows.Forms.ToolStripButton();
            this.tabControlWGO.SuspendLayout();
            this.tabPageGuildData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewGuildData)).BeginInit();
            this.contextMenuStripGuildMembers.SuspendLayout();
            this.tabPageRaidData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewRaidGroup)).BeginInit();
            this.contextMenuStripRaidMembers.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewRaidLootDrop)).BeginInit();
            this.tabPageSettings.SuspendLayout();
            this.groupBoxErrors.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.toolStripMain.SuspendLayout();
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
            this.dataGridViewGuildData.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.DataGridViewGuildData_DataError);
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
            this.tabPageRaidData.Controls.Add(this.dataGridViewRaidGroup);
            this.tabPageRaidData.Location = new System.Drawing.Point(23, 4);
            this.tabPageRaidData.Name = "tabPageRaidData";
            this.tabPageRaidData.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageRaidData.Size = new System.Drawing.Size(695, 275);
            this.tabPageRaidData.TabIndex = 2;
            this.tabPageRaidData.Text = "Raid Data";
            this.tabPageRaidData.UseVisualStyleBackColor = true;
            // 
            // dataGridViewRaidGroup
            // 
            this.dataGridViewRaidGroup.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.dataGridViewRaidGroup.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewRaidGroup.ContextMenuStrip = this.contextMenuStripRaidMembers;
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
            // contextMenuStripRaidMembers
            // 
            this.contextMenuStripRaidMembers.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.updateThisCharacterToolStripMenuItem,
            this.moveCharacterUpToolStripMenuItem,
            this.moveCharacterDownToolStripMenuItem,
            this.deleteCharacterFromGridToolStripMenuItem});
            this.contextMenuStripRaidMembers.Name = "contextMenuStrip1";
            this.contextMenuStripRaidMembers.Size = new System.Drawing.Size(216, 92);
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
            this.tabPageSettings.Controls.Add(this.groupBoxErrors);
            this.tabPageSettings.Controls.Add(this.groupBox2);
            this.tabPageSettings.Controls.Add(this.groupBox3);
            this.tabPageSettings.Location = new System.Drawing.Point(23, 4);
            this.tabPageSettings.Name = "tabPageSettings";
            this.tabPageSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSettings.Size = new System.Drawing.Size(695, 275);
            this.tabPageSettings.TabIndex = 1;
            this.tabPageSettings.Text = "Settings";
            this.tabPageSettings.UseVisualStyleBackColor = true;
            // 
            // groupBoxErrors
            // 
            this.groupBoxErrors.Controls.Add(this.buttonShowErrors);
            this.groupBoxErrors.Location = new System.Drawing.Point(16, 83);
            this.groupBoxErrors.Name = "groupBoxErrors";
            this.groupBoxErrors.Size = new System.Drawing.Size(200, 56);
            this.groupBoxErrors.TabIndex = 15;
            this.groupBoxErrors.TabStop = false;
            this.groupBoxErrors.Text = "Errors";
            // 
            // buttonShowErrors
            // 
            this.buttonShowErrors.Location = new System.Drawing.Point(57, 19);
            this.buttonShowErrors.Name = "buttonShowErrors";
            this.buttonShowErrors.Size = new System.Drawing.Size(75, 23);
            this.buttonShowErrors.TabIndex = 14;
            this.buttonShowErrors.Text = "Show Errors";
            this.buttonShowErrors.UseVisualStyleBackColor = true;
            this.buttonShowErrors.Click += new System.EventHandler(this.ButtonShowErrors_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.buttonDeleteItemCacheData);
            this.groupBox2.Location = new System.Drawing.Point(16, 154);
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
            this.groupBox3.Location = new System.Drawing.Point(16, 15);
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
            // toolStripMain
            // 
            this.toolStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabelGuild,
            this.toolStripTextBoxGuild,
            this.toolStripLabelRealm,
            this.toolStripTextBoxRealm,
            this.toolStripButtonAdd,
            this.toolStripButtonRefresh,
            this.toolStripButtonSort,
            this.toolStripSeparator1,
            this.toolStripProgressBar1,
            this.toolStripLabelRefreshStatus,
            this.toolStripLabelPickRaid,
            this.toolStripComboBoxPickRaid,
            this.toolStripLabelPickBoss,
            this.toolStripComboBoxPickBoss});
            this.toolStripMain.Location = new System.Drawing.Point(0, 0);
            this.toolStripMain.Name = "toolStripMain";
            this.toolStripMain.Size = new System.Drawing.Size(721, 27);
            this.toolStripMain.TabIndex = 1;
            this.toolStripMain.Text = "toolStrip2";
            // 
            // toolStripLabelGuild
            // 
            this.toolStripLabelGuild.Name = "toolStripLabelGuild";
            this.toolStripLabelGuild.Size = new System.Drawing.Size(38, 24);
            this.toolStripLabelGuild.Text = "Guild:";
            // 
            // toolStripTextBoxGuild
            // 
            this.toolStripTextBoxGuild.Name = "toolStripTextBoxGuild";
            this.toolStripTextBoxGuild.Size = new System.Drawing.Size(100, 27);
            // 
            // toolStripLabelRealm
            // 
            this.toolStripLabelRealm.Name = "toolStripLabelRealm";
            this.toolStripLabelRealm.Size = new System.Drawing.Size(43, 24);
            this.toolStripLabelRealm.Text = "Realm:";
            // 
            // toolStripTextBoxRealm
            // 
            this.toolStripTextBoxRealm.Name = "toolStripTextBoxRealm";
            this.toolStripTextBoxRealm.Size = new System.Drawing.Size(100, 27);
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
            this.toolStripButtonRefresh.Size = new System.Drawing.Size(23, 24);
            this.toolStripButtonRefresh.Text = "Refresh";
            this.toolStripButtonRefresh.Click += new System.EventHandler(this.ToolStripButtonRefresh_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
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
            this.toolStripComboBoxPickRaid.Size = new System.Drawing.Size(175, 23);
            this.toolStripComboBoxPickRaid.Visible = false;
            this.toolStripComboBoxPickRaid.SelectedIndexChanged += new System.EventHandler(this.toolStripComboBoxPickRaid_SelectedIndexChanged);
            // 
            // toolStripLabelPickBoss
            // 
            this.toolStripLabelPickBoss.Name = "toolStripLabelPickBoss";
            this.toolStripLabelPickBoss.Size = new System.Drawing.Size(59, 15);
            this.toolStripLabelPickBoss.Text = "Pick Boss:";
            this.toolStripLabelPickBoss.Visible = false;
            // 
            // toolStripComboBoxPickBoss
            // 
            this.toolStripComboBoxPickBoss.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toolStripComboBoxPickBoss.DropDownWidth = 150;
            this.toolStripComboBoxPickBoss.Name = "toolStripComboBoxPickBoss";
            this.toolStripComboBoxPickBoss.Size = new System.Drawing.Size(121, 23);
            this.toolStripComboBoxPickBoss.Visible = false;
            this.toolStripComboBoxPickBoss.SelectedIndexChanged += new System.EventHandler(this.toolStripComboBoxPickBoss_SelectedIndexChanged);
            // 
            // toolStripButtonSort
            // 
            this.toolStripButtonSort.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSort.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonSort.Image")));
            this.toolStripButtonSort.ImageTransparentColor = System.Drawing.Color.Black;
            this.toolStripButtonSort.Name = "toolStripButtonSort";
            this.toolStripButtonSort.Size = new System.Drawing.Size(23, 24);
            this.toolStripButtonSort.Text = "Sort";
            this.toolStripButtonSort.Click += new System.EventHandler(this.ToolStripButtonSort_Click);
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
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewRaidGroup)).EndInit();
            this.contextMenuStripRaidMembers.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewRaidLootDrop)).EndInit();
            this.tabPageSettings.ResumeLayout(false);
            this.groupBoxErrors.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.toolStripMain.ResumeLayout(false);
            this.toolStripMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControlWGO;
        private System.Windows.Forms.TabPage tabPageGuildData;
        private System.Windows.Forms.DataGridView dataGridViewGuildData;
        private System.Windows.Forms.TabPage tabPageSettings;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonLoad;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button buttonDeleteItemCacheData;
        private System.Windows.Forms.TabPage tabPageRaidData;
        private System.Windows.Forms.DataGridView dataGridViewRaidGroup;
        private System.Windows.Forms.Button buttonShowErrors;
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
        private System.Windows.Forms.ContextMenuStrip contextMenuStripRaidMembers;
        private System.Windows.Forms.ToolStripMenuItem updateThisCharacterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moveCharacterUpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moveCharacterDownToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteCharacterFromGridToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton toolStripButtonAdd;
        private System.Windows.Forms.ToolStripLabel toolStripLabelPickRaid;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBoxPickRaid;
        private System.Windows.Forms.ToolStripLabel toolStripLabelPickBoss;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBoxPickBoss;
        private System.Windows.Forms.GroupBox groupBoxErrors;
        private System.Windows.Forms.ToolStripButton toolStripButtonSort;
    }
}
