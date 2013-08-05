namespace WoWGuildOrganizer
{
    partial class Form1
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageGuildData = new System.Windows.Forms.TabPage();
            this.buttonRefreshGuildData = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.dataGridViewGuildData = new System.Windows.Forms.DataGridView();
            this.tabPageRaidData = new System.Windows.Forms.TabPage();
            this.buttonRaidGroupRefresh = new System.Windows.Forms.Button();
            this.dataGridViewRaidGroup = new System.Windows.Forms.DataGridView();
            this.tabPageSettings = new System.Windows.Forms.TabPage();
            this.button1 = new System.Windows.Forms.Button();
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
            this.labelRaidTab = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPageGuildData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewGuildData)).BeginInit();
            this.tabPageRaidData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewRaidGroup)).BeginInit();
            this.tabPageSettings.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPageGuildData);
            this.tabControl1.Controls.Add(this.tabPageRaidData);
            this.tabControl1.Controls.Add(this.tabPageSettings);
            this.tabControl1.Location = new System.Drawing.Point(0, 1);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(722, 307);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPageGuildData
            // 
            this.tabPageGuildData.Controls.Add(this.buttonRefreshGuildData);
            this.tabPageGuildData.Controls.Add(this.label3);
            this.tabPageGuildData.Controls.Add(this.dataGridViewGuildData);
            this.tabPageGuildData.Location = new System.Drawing.Point(4, 22);
            this.tabPageGuildData.Name = "tabPageGuildData";
            this.tabPageGuildData.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageGuildData.Size = new System.Drawing.Size(714, 281);
            this.tabPageGuildData.TabIndex = 0;
            this.tabPageGuildData.Text = "Guild Data";
            this.tabPageGuildData.UseVisualStyleBackColor = true;
            // 
            // buttonRefreshGuildData
            // 
            this.buttonRefreshGuildData.Location = new System.Drawing.Point(630, 3);
            this.buttonRefreshGuildData.Name = "buttonRefreshGuildData";
            this.buttonRefreshGuildData.Size = new System.Drawing.Size(75, 23);
            this.buttonRefreshGuildData.TabIndex = 2;
            this.buttonRefreshGuildData.Text = "Refresh";
            this.buttonRefreshGuildData.UseVisualStyleBackColor = true;
            this.buttonRefreshGuildData.Click += new System.EventHandler(this.buttonRefreshGuildData_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(158, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "No data has been collected yet.";
            // 
            // dataGridViewGuildData
            // 
            this.dataGridViewGuildData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewGuildData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewGuildData.Location = new System.Drawing.Point(6, 32);
            this.dataGridViewGuildData.Name = "dataGridViewGuildData";
            this.dataGridViewGuildData.Size = new System.Drawing.Size(702, 249);
            this.dataGridViewGuildData.TabIndex = 0;
            this.dataGridViewGuildData.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewGuildData_CellMouseDoubleClick);
            this.dataGridViewGuildData.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewGuildData_ColumnHeaderMouseClick);
            // 
            // tabPageRaidData
            // 
            this.tabPageRaidData.Controls.Add(this.labelRaidTab);
            this.tabPageRaidData.Controls.Add(this.buttonRaidGroupRefresh);
            this.tabPageRaidData.Controls.Add(this.dataGridViewRaidGroup);
            this.tabPageRaidData.Location = new System.Drawing.Point(4, 22);
            this.tabPageRaidData.Name = "tabPageRaidData";
            this.tabPageRaidData.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageRaidData.Size = new System.Drawing.Size(714, 281);
            this.tabPageRaidData.TabIndex = 2;
            this.tabPageRaidData.Text = "Raid Data";
            this.tabPageRaidData.UseVisualStyleBackColor = true;
            // 
            // buttonRaidGroupRefresh
            // 
            this.buttonRaidGroupRefresh.Location = new System.Drawing.Point(630, 6);
            this.buttonRaidGroupRefresh.Name = "buttonRaidGroupRefresh";
            this.buttonRaidGroupRefresh.Size = new System.Drawing.Size(75, 23);
            this.buttonRaidGroupRefresh.TabIndex = 1;
            this.buttonRaidGroupRefresh.Text = "Refresh";
            this.buttonRaidGroupRefresh.UseVisualStyleBackColor = true;
            this.buttonRaidGroupRefresh.Click += new System.EventHandler(this.buttonRaidGroupRefresh_Click);
            // 
            // dataGridViewRaidGroup
            // 
            this.dataGridViewRaidGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewRaidGroup.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewRaidGroup.Location = new System.Drawing.Point(0, 44);
            this.dataGridViewRaidGroup.Name = "dataGridViewRaidGroup";
            this.dataGridViewRaidGroup.Size = new System.Drawing.Size(718, 237);
            this.dataGridViewRaidGroup.TabIndex = 0;
            this.dataGridViewRaidGroup.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewRaidGroup_CellMouseDoubleClick);
            this.dataGridViewRaidGroup.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewRaidGroup_ColumnHeaderMouseClick);
            this.dataGridViewRaidGroup.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.dataGridViewRaidGroup_SortCompare);
            this.dataGridViewRaidGroup.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dataGridViewRaidGroup_MouseClick);
            // 
            // tabPageSettings
            // 
            this.tabPageSettings.Controls.Add(this.button1);
            this.tabPageSettings.Controls.Add(this.groupBox4);
            this.tabPageSettings.Controls.Add(this.groupBox2);
            this.tabPageSettings.Controls.Add(this.groupBox3);
            this.tabPageSettings.Controls.Add(this.groupBox1);
            this.tabPageSettings.Location = new System.Drawing.Point(4, 22);
            this.tabPageSettings.Name = "tabPageSettings";
            this.tabPageSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSettings.Size = new System.Drawing.Size(714, 281);
            this.tabPageSettings.TabIndex = 1;
            this.tabPageSettings.Text = "Settings";
            this.tabPageSettings.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(294, 202);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 14;
            this.button1.Text = "Show Errors";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
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
            this.buttonAddCharacterToRaidData.Click += new System.EventHandler(this.buttonAddCharacterToRaidData_Click);
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
            this.groupBox2.Text = "Delete Item Cache Data";
            // 
            // buttonDeleteItemCacheData
            // 
            this.buttonDeleteItemCacheData.Location = new System.Drawing.Point(19, 19);
            this.buttonDeleteItemCacheData.Name = "buttonDeleteItemCacheData";
            this.buttonDeleteItemCacheData.Size = new System.Drawing.Size(75, 23);
            this.buttonDeleteItemCacheData.TabIndex = 4;
            this.buttonDeleteItemCacheData.Text = "Delete";
            this.buttonDeleteItemCacheData.UseVisualStyleBackColor = true;
            this.buttonDeleteItemCacheData.Click += new System.EventHandler(this.buttonDeleteItemCacheData_Click);
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
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonLoad
            // 
            this.buttonLoad.Location = new System.Drawing.Point(100, 19);
            this.buttonLoad.Name = "buttonLoad";
            this.buttonLoad.Size = new System.Drawing.Size(75, 23);
            this.buttonLoad.TabIndex = 5;
            this.buttonLoad.Text = "Load";
            this.buttonLoad.UseVisualStyleBackColor = true;
            this.buttonLoad.Click += new System.EventHandler(this.buttonLoad_Click);
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
            this.buttonGetGuildInfo.Click += new System.EventHandler(this.buttonGetGuildInfo_Click);
            // 
            // labelRaidTab
            // 
            this.labelRaidTab.AutoSize = true;
            this.labelRaidTab.Location = new System.Drawing.Point(8, 16);
            this.labelRaidTab.Name = "labelRaidTab";
            this.labelRaidTab.Size = new System.Drawing.Size(0, 13);
            this.labelRaidTab.TabIndex = 2;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(721, 307);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
            this.Text = "WoW Guild Organizer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPageGuildData.ResumeLayout(false);
            this.tabPageGuildData.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewGuildData)).EndInit();
            this.tabPageRaidData.ResumeLayout(false);
            this.tabPageRaidData.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewRaidGroup)).EndInit();
            this.tabPageSettings.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageGuildData;
        private System.Windows.Forms.DataGridView dataGridViewGuildData;
        private System.Windows.Forms.TabPage tabPageSettings;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxGuildName;
        private System.Windows.Forms.TextBox textBoxRealm;
        private System.Windows.Forms.ProgressBar progressBarCollectData;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonLoad;
        private System.Windows.Forms.Button buttonGetGuildInfo;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button buttonDeleteItemCacheData;
        private System.Windows.Forms.TabPage tabPageRaidData;
        private System.Windows.Forms.Button buttonRefreshGuildData;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox textBoxCharacterName;
        private System.Windows.Forms.Button buttonAddCharacterToRaidData;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxCharacterRealm;
        private System.Windows.Forms.ProgressBar progressBarCharacterAddToRaid;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button buttonRaidGroupRefresh;
        private System.Windows.Forms.DataGridView dataGridViewRaidGroup;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label labelRaidTab;
    }
}

