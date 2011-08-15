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
            this.label3 = new System.Windows.Forms.Label();
            this.dataGridViewGuildData = new System.Windows.Forms.DataGridView();
            this.tabPageSettings = new System.Windows.Forms.TabPage();
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.buttonDeleteItemCacheData = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPageGuildData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewGuildData)).BeginInit();
            this.tabPageSettings.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPageGuildData);
            this.tabControl1.Controls.Add(this.tabPageSettings);
            this.tabControl1.Location = new System.Drawing.Point(0, 1);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(722, 307);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPageGuildData
            // 
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
            this.dataGridViewGuildData.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewGuildData_ColumnHeaderMouseClick);
            this.dataGridViewGuildData.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewGuildData_CellMouseDoubleClick);
            // 
            // tabPageSettings
            // 
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
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(721, 307);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
            this.Text = "WoW Guild Organizer";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.tabControl1.ResumeLayout(false);
            this.tabPageGuildData.ResumeLayout(false);
            this.tabPageGuildData.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewGuildData)).EndInit();
            this.tabPageSettings.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
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
    }
}

