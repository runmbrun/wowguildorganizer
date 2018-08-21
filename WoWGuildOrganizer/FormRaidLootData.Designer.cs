namespace WoWGuildOrganizer
{
    partial class FormRaidLootData
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
            this.buttonBossLoot = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxBossLoot = new System.Windows.Forms.TextBox();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.textBoxRaidLoot = new System.Windows.Forms.TextBox();
            this.buttonRaidLoot = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.linkLabel2 = new System.Windows.Forms.LinkLabel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonGetCurrentRaids = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonBossLoot
            // 
            this.buttonBossLoot.Location = new System.Drawing.Point(254, 112);
            this.buttonBossLoot.Name = "buttonBossLoot";
            this.buttonBossLoot.Size = new System.Drawing.Size(75, 23);
            this.buttonBossLoot.TabIndex = 0;
            this.buttonBossLoot.Text = "Get Boss";
            this.buttonBossLoot.UseVisualStyleBackColor = true;
            this.buttonBossLoot.Click += new System.EventHandler(this.ButtonBossLoot_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 80);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Raid Zone ID:";
            // 
            // textBoxBossLoot
            // 
            this.textBoxBossLoot.Location = new System.Drawing.Point(149, 112);
            this.textBoxBossLoot.Name = "textBoxBossLoot";
            this.textBoxBossLoot.Size = new System.Drawing.Size(97, 20);
            this.textBoxBossLoot.TabIndex = 2;
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(3, 138);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(55, 13);
            this.linkLabel1.TabIndex = 3;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "linkLabel1";
            // 
            // richTextBox1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.richTextBox1, 4);
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox1.Location = new System.Drawing.Point(3, 3);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(386, 74);
            this.richTextBox1.TabIndex = 4;
            this.richTextBox1.Text = "";
            // 
            // textBoxRaidLoot
            // 
            this.textBoxRaidLoot.Location = new System.Drawing.Point(149, 83);
            this.textBoxRaidLoot.Name = "textBoxRaidLoot";
            this.textBoxRaidLoot.Size = new System.Drawing.Size(99, 20);
            this.textBoxRaidLoot.TabIndex = 2;
            this.textBoxRaidLoot.Text = "6996";
            // 
            // buttonRaidLoot
            // 
            this.buttonRaidLoot.Location = new System.Drawing.Point(254, 83);
            this.buttonRaidLoot.Name = "buttonRaidLoot";
            this.buttonRaidLoot.Size = new System.Drawing.Size(75, 23);
            this.buttonRaidLoot.TabIndex = 0;
            this.buttonRaidLoot.Text = "Get Raid";
            this.buttonRaidLoot.UseVisualStyleBackColor = true;
            this.buttonRaidLoot.Click += new System.EventHandler(this.buttonRaidLoot_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 109);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Boss ID:";
            // 
            // linkLabel2
            // 
            this.linkLabel2.AutoSize = true;
            this.linkLabel2.Location = new System.Drawing.Point(149, 138);
            this.linkLabel2.Name = "linkLabel2";
            this.linkLabel2.Size = new System.Drawing.Size(55, 13);
            this.linkLabel2.TabIndex = 3;
            this.linkLabel2.TabStop = true;
            this.linkLabel2.Text = "linkLabel1";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.richTextBox1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.linkLabel2, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.linkLabel1, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.textBoxRaidLoot, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.buttonBossLoot, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.textBoxBossLoot, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.buttonRaidLoot, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.buttonGetCurrentRaids, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 4);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(392, 222);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // buttonGetCurrentRaids
            // 
            this.buttonGetCurrentRaids.Location = new System.Drawing.Point(149, 154);
            this.buttonGetCurrentRaids.Name = "buttonGetCurrentRaids";
            this.buttonGetCurrentRaids.Size = new System.Drawing.Size(75, 23);
            this.buttonGetCurrentRaids.TabIndex = 5;
            this.buttonGetCurrentRaids.Text = "Get";
            this.buttonGetCurrentRaids.UseVisualStyleBackColor = true;
            this.buttonGetCurrentRaids.Click += new System.EventHandler(this.ButtonGetCurrentRaids_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(3, 151);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(140, 29);
            this.label3.TabIndex = 6;
            this.label3.Text = "All Current Expansion Raids:";
            // 
            // FormRaidLootData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(392, 222);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "FormRaidLootData";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "GetRaidLootData";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonBossLoot;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxBossLoot;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.TextBox textBoxRaidLoot;
        private System.Windows.Forms.Button buttonRaidLoot;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.LinkLabel linkLabel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button buttonGetCurrentRaids;
        private System.Windows.Forms.Label label3;
    }
}