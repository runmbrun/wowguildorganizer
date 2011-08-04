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
            this.labelName = new System.Windows.Forms.Label();
            this.textBoxAuditStatus = new System.Windows.Forms.TextBox();
            this.dataGridViewItemAudit = new System.Windows.Forms.DataGridView();
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
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewItemAudit)).BeginInit();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(12, 21);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(108, 13);
            this.labelName.TabIndex = 0;
            this.labelName.Text = "Character Information";
            // 
            // textBoxAuditStatus
            // 
            this.textBoxAuditStatus.Enabled = false;
            this.textBoxAuditStatus.Location = new System.Drawing.Point(61, 124);
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
            this.dataGridViewItemAudit.Location = new System.Drawing.Point(0, 3);
            this.dataGridViewItemAudit.Name = "dataGridViewItemAudit";
            this.dataGridViewItemAudit.Size = new System.Drawing.Size(777, 404);
            this.dataGridViewItemAudit.TabIndex = 2;
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
            this.label4.Location = new System.Drawing.Point(188, 32);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(97, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Total Missing Items";
            // 
            // textBoxMissingTotal
            // 
            this.textBoxMissingTotal.Location = new System.Drawing.Point(214, 57);
            this.textBoxMissingTotal.Name = "textBoxMissingTotal";
            this.textBoxMissingTotal.ReadOnly = true;
            this.textBoxMissingTotal.Size = new System.Drawing.Size(37, 20);
            this.textBoxMissingTotal.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(59, 87);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Gems Missing";
            // 
            // textBoxMissingGems
            // 
            this.textBoxMissingGems.Location = new System.Drawing.Point(23, 84);
            this.textBoxMissingGems.Name = "textBoxMissingGems";
            this.textBoxMissingGems.ReadOnly = true;
            this.textBoxMissingGems.Size = new System.Drawing.Size(24, 20);
            this.textBoxMissingGems.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(59, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Enchants Missing";
            // 
            // textBoxMissingEnchants
            // 
            this.textBoxMissingEnchants.Location = new System.Drawing.Point(23, 54);
            this.textBoxMissingEnchants.Name = "textBoxMissingEnchants";
            this.textBoxMissingEnchants.ReadOnly = true;
            this.textBoxMissingEnchants.Size = new System.Drawing.Size(24, 20);
            this.textBoxMissingEnchants.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(59, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Items Missing";
            // 
            // textBoxMissingItems
            // 
            this.textBoxMissingItems.Location = new System.Drawing.Point(23, 24);
            this.textBoxMissingItems.Name = "textBoxMissingItems";
            this.textBoxMissingItems.ReadOnly = true;
            this.textBoxMissingItems.Size = new System.Drawing.Size(24, 20);
            this.textBoxMissingItems.TabIndex = 0;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 127);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(43, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Status: ";
            // 
            // FormItemAudit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(805, 572);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.textBoxAuditStatus);
            this.Controls.Add(this.labelName);
            this.Name = "FormItemAudit";
            this.Text = "Character Audit";
            this.Load += new System.EventHandler(this.FormItemAudit_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewItemAudit)).EndInit();
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
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
    }
}