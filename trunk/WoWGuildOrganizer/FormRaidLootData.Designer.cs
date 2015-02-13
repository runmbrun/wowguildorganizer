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
            this.SuspendLayout();
            // 
            // buttonBossLoot
            // 
            this.buttonBossLoot.Location = new System.Drawing.Point(197, 125);
            this.buttonBossLoot.Name = "buttonBossLoot";
            this.buttonBossLoot.Size = new System.Drawing.Size(75, 23);
            this.buttonBossLoot.TabIndex = 0;
            this.buttonBossLoot.Text = "Get Boss";
            this.buttonBossLoot.UseVisualStyleBackColor = true;
            this.buttonBossLoot.Click += new System.EventHandler(this.buttonBossLoot_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 98);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Raid Zone ID:";
            // 
            // textBoxBossLoot
            // 
            this.textBoxBossLoot.Location = new System.Drawing.Point(94, 125);
            this.textBoxBossLoot.Name = "textBoxBossLoot";
            this.textBoxBossLoot.Size = new System.Drawing.Size(97, 20);
            this.textBoxBossLoot.TabIndex = 2;
            this.textBoxBossLoot.Text = "78714";
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(12, 159);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(55, 13);
            this.linkLabel1.TabIndex = 3;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "linkLabel1";
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(12, 12);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(260, 74);
            this.richTextBox1.TabIndex = 4;
            this.richTextBox1.Text = "Please enter either the Zone Id of a Raid or the ID of a specific boss.\n    Examp" +
    "les include:\n        Blackrock Foundry - 6967\n        Highmaul - 6996\nOR\n       " +
    " Kargath - ?\n";
            // 
            // textBoxRaidLoot
            // 
            this.textBoxRaidLoot.Location = new System.Drawing.Point(92, 95);
            this.textBoxRaidLoot.Name = "textBoxRaidLoot";
            this.textBoxRaidLoot.Size = new System.Drawing.Size(99, 20);
            this.textBoxRaidLoot.TabIndex = 2;
            this.textBoxRaidLoot.Text = "6996";
            // 
            // buttonRaidLoot
            // 
            this.buttonRaidLoot.Location = new System.Drawing.Point(197, 93);
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
            this.label2.Location = new System.Drawing.Point(12, 125);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Boss ID:";
            // 
            // linkLabel2
            // 
            this.linkLabel2.AutoSize = true;
            this.linkLabel2.Location = new System.Drawing.Point(217, 159);
            this.linkLabel2.Name = "linkLabel2";
            this.linkLabel2.Size = new System.Drawing.Size(55, 13);
            this.linkLabel2.TabIndex = 3;
            this.linkLabel2.TabStop = true;
            this.linkLabel2.Text = "linkLabel1";
            // 
            // FormRaidLootData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 188);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.linkLabel2);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.textBoxRaidLoot);
            this.Controls.Add(this.textBoxBossLoot);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonRaidLoot);
            this.Controls.Add(this.buttonBossLoot);
            this.Name = "FormRaidLootData";
            this.Text = "GetRaidLootData";
            this.ResumeLayout(false);
            this.PerformLayout();

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
    }
}