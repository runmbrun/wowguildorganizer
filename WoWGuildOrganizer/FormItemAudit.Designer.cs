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
            this.textBoxCharInfo = new System.Windows.Forms.TextBox();
            this.dataGridViewItemAudit = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewItemAudit)).BeginInit();
            this.SuspendLayout();
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(12, 21);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(41, 13);
            this.labelName.TabIndex = 0;
            this.labelName.Text = "Name: ";
            // 
            // textBoxCharInfo
            // 
            this.textBoxCharInfo.Enabled = false;
            this.textBoxCharInfo.Location = new System.Drawing.Point(15, 38);
            this.textBoxCharInfo.Name = "textBoxCharInfo";
            this.textBoxCharInfo.Size = new System.Drawing.Size(376, 20);
            this.textBoxCharInfo.TabIndex = 1;
            // 
            // dataGridViewItemAudit
            // 
            this.dataGridViewItemAudit.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewItemAudit.Location = new System.Drawing.Point(15, 91);
            this.dataGridViewItemAudit.Name = "dataGridViewItemAudit";
            this.dataGridViewItemAudit.Size = new System.Drawing.Size(640, 215);
            this.dataGridViewItemAudit.TabIndex = 2;
            // 
            // FormItemAudit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(667, 318);
            this.Controls.Add(this.dataGridViewItemAudit);
            this.Controls.Add(this.textBoxCharInfo);
            this.Controls.Add(this.labelName);
            this.Name = "FormItemAudit";
            this.Text = "Character Audit";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewItemAudit)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.TextBox textBoxCharInfo;
        private System.Windows.Forms.DataGridView dataGridViewItemAudit;
    }
}