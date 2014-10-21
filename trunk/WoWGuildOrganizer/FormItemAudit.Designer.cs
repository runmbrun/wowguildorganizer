namespace WoWGuildOrganizer
{
    partial class FormItemAudit
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
            this.labelName = new System.Windows.Forms.Label();
            this.textBoxAuditStatus = new System.Windows.Forms.TextBox();
            this.dataGridViewItemAudit = new System.Windows.Forms.DataGridView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyItemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyLineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxMissingTotal = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxMissingGems = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxMissingEnchants = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxMissingItems = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxMaxiLevel = new System.Windows.Forms.TextBox();
            this.textBoxEquippediLevel = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.textBoxProfessions = new System.Windows.Forms.TextBox();
            this.labelSpec = new System.Windows.Forms.Label();
            this.labelRole = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewItemAudit)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(12, 13);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(108, 13);
            this.labelName.TabIndex = 0;
            this.labelName.Text = "Character Information";
            // 
            // textBoxAuditStatus
            // 
            this.textBoxAuditStatus.Enabled = false;
            this.textBoxAuditStatus.Location = new System.Drawing.Point(82, 126);
            this.textBoxAuditStatus.Name = "textBoxAuditStatus";
            this.textBoxAuditStatus.ReadOnly = true;
            this.textBoxAuditStatus.Size = new System.Drawing.Size(190, 20);
            this.textBoxAuditStatus.TabIndex = 1;
            // 
            // dataGridViewItemAudit
            // 
            this.dataGridViewItemAudit.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewItemAudit.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewItemAudit.ContextMenuStrip = this.contextMenuStrip1;
            this.dataGridViewItemAudit.Location = new System.Drawing.Point(0, 3);
            this.dataGridViewItemAudit.Name = "dataGridViewItemAudit";
            this.dataGridViewItemAudit.ReadOnly = true;
            this.dataGridViewItemAudit.Size = new System.Drawing.Size(777, 404);
            this.dataGridViewItemAudit.TabIndex = 2;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyItemToolStripMenuItem,
            this.copyLineToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(130, 48);
            // 
            // copyItemToolStripMenuItem
            // 
            this.copyItemToolStripMenuItem.Name = "copyItemToolStripMenuItem";
            this.copyItemToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.copyItemToolStripMenuItem.Text = "Copy Item";
            this.copyItemToolStripMenuItem.Click += new System.EventHandler(this.copyItemToolStripMenuItem_Click);
            // 
            // copyLineToolStripMenuItem
            // 
            this.copyLineToolStripMenuItem.Name = "copyLineToolStripMenuItem";
            this.copyLineToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.copyLineToolStripMenuItem.Text = "Copy Line";
            this.copyLineToolStripMenuItem.Click += new System.EventHandler(this.copyLineToolStripMenuItem_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.dataGridViewItemAudit);
            this.panel1.Location = new System.Drawing.Point(12, 150);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(780, 410);
            this.panel1.TabIndex = 4;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.textBoxMissingTotal);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.textBoxMissingGems);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBoxMissingEnchants);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.textBoxMissingItems);
            this.groupBox1.Location = new System.Drawing.Point(492, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(294, 131);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Missing";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(59, 104);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(97, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Total Missing Items";
            // 
            // textBoxMissingTotal
            // 
            this.textBoxMissingTotal.Location = new System.Drawing.Point(23, 101);
            this.textBoxMissingTotal.Name = "textBoxMissingTotal";
            this.textBoxMissingTotal.ReadOnly = true;
            this.textBoxMissingTotal.Size = new System.Drawing.Size(24, 20);
            this.textBoxMissingTotal.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(59, 74);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Gems Missing";
            // 
            // textBoxMissingGems
            // 
            this.textBoxMissingGems.Location = new System.Drawing.Point(23, 71);
            this.textBoxMissingGems.Name = "textBoxMissingGems";
            this.textBoxMissingGems.ReadOnly = true;
            this.textBoxMissingGems.Size = new System.Drawing.Size(24, 20);
            this.textBoxMissingGems.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(59, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Enchants Missing";
            // 
            // textBoxMissingEnchants
            // 
            this.textBoxMissingEnchants.Location = new System.Drawing.Point(23, 45);
            this.textBoxMissingEnchants.Name = "textBoxMissingEnchants";
            this.textBoxMissingEnchants.ReadOnly = true;
            this.textBoxMissingEnchants.Size = new System.Drawing.Size(24, 20);
            this.textBoxMissingEnchants.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(59, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Items Missing";
            // 
            // textBoxMissingItems
            // 
            this.textBoxMissingItems.Location = new System.Drawing.Point(23, 19);
            this.textBoxMissingItems.Name = "textBoxMissingItems";
            this.textBoxMissingItems.ReadOnly = true;
            this.textBoxMissingItems.Size = new System.Drawing.Size(24, 20);
            this.textBoxMissingItems.TabIndex = 0;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(12, 127);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(64, 17);
            this.label5.TabIndex = 6;
            this.label5.Text = "Status: ";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.textBoxMaxiLevel);
            this.groupBox2.Controls.Add(this.textBoxEquippediLevel);
            this.groupBox2.Location = new System.Drawing.Point(275, 17);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 73);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "iLevel";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 47);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(30, 13);
            this.label7.TabIndex = 6;
            this.label7.Text = "Max:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 25);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(55, 13);
            this.label6.TabIndex = 6;
            this.label6.Text = "Equipped:";
            // 
            // textBoxMaxiLevel
            // 
            this.textBoxMaxiLevel.Location = new System.Drawing.Point(67, 40);
            this.textBoxMaxiLevel.Name = "textBoxMaxiLevel";
            this.textBoxMaxiLevel.ReadOnly = true;
            this.textBoxMaxiLevel.Size = new System.Drawing.Size(68, 20);
            this.textBoxMaxiLevel.TabIndex = 0;
            // 
            // textBoxEquippediLevel
            // 
            this.textBoxEquippediLevel.Location = new System.Drawing.Point(67, 19);
            this.textBoxEquippediLevel.Name = "textBoxEquippediLevel";
            this.textBoxEquippediLevel.ReadOnly = true;
            this.textBoxEquippediLevel.Size = new System.Drawing.Size(127, 20);
            this.textBoxEquippediLevel.TabIndex = 0;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(25, 32);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(64, 13);
            this.label9.TabIndex = 6;
            this.label9.Text = "Professions:";
            // 
            // textBoxProfessions
            // 
            this.textBoxProfessions.Location = new System.Drawing.Point(28, 48);
            this.textBoxProfessions.Name = "textBoxProfessions";
            this.textBoxProfessions.ReadOnly = true;
            this.textBoxProfessions.Size = new System.Drawing.Size(199, 20);
            this.textBoxProfessions.TabIndex = 0;
            // 
            // labelSpec
            // 
            this.labelSpec.AutoSize = true;
            this.labelSpec.Location = new System.Drawing.Point(25, 74);
            this.labelSpec.Name = "labelSpec";
            this.labelSpec.Size = new System.Drawing.Size(35, 13);
            this.labelSpec.TabIndex = 8;
            this.labelSpec.Text = "Spec:";
            // 
            // labelRole
            // 
            this.labelRole.AutoSize = true;
            this.labelRole.Location = new System.Drawing.Point(25, 95);
            this.labelRole.Name = "labelRole";
            this.labelRole.Size = new System.Drawing.Size(35, 13);
            this.labelRole.TabIndex = 9;
            this.labelRole.Text = "Role: ";
            // 
            // FormItemAudit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(805, 572);
            this.Controls.Add(this.labelRole);
            this.Controls.Add(this.labelSpec);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBoxProfessions);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.textBoxAuditStatus);
            this.Controls.Add(this.labelName);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormItemAudit";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Character Audit";
            this.Load += new System.EventHandler(this.FormItemAudit_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewItemAudit)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.TextBox textBoxAuditStatus;
        private System.Windows.Forms.DataGridView dataGridViewItemAudit;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxMissingGems;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxMissingEnchants;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxMissingItems;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxMissingTotal;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxMaxiLevel;
        private System.Windows.Forms.TextBox textBoxEquippediLevel;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBoxProfessions;
        private System.Windows.Forms.Label labelSpec;
        private System.Windows.Forms.Label labelRole;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem copyItemToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyLineToolStripMenuItem;
    }
}