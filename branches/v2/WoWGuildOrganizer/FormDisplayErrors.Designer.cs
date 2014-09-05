﻿namespace WoWGuildOrganizer
{
    partial class FormDisplayErrors
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
            this.groupBoxErrors = new System.Windows.Forms.GroupBox();
            this.buttonTop = new System.Windows.Forms.Button();
            this.buttonBottom = new System.Windows.Forms.Button();
            this.buttonOk = new System.Windows.Forms.Button();
            this.listBoxErrors = new System.Windows.Forms.ListBox();
            this.groupBoxErrors.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxErrors
            // 
            this.groupBoxErrors.Controls.Add(this.listBoxErrors);
            this.groupBoxErrors.Location = new System.Drawing.Point(12, 12);
            this.groupBoxErrors.Name = "groupBoxErrors";
            this.groupBoxErrors.Size = new System.Drawing.Size(390, 218);
            this.groupBoxErrors.TabIndex = 0;
            this.groupBoxErrors.TabStop = false;
            this.groupBoxErrors.Text = "Errors Logged";
            // 
            // buttonTop
            // 
            this.buttonTop.Location = new System.Drawing.Point(12, 236);
            this.buttonTop.Name = "buttonTop";
            this.buttonTop.Size = new System.Drawing.Size(75, 23);
            this.buttonTop.TabIndex = 1;
            this.buttonTop.Text = "Top";
            this.buttonTop.UseVisualStyleBackColor = true;
            this.buttonTop.Click += new System.EventHandler(this.buttonTop_Click);
            // 
            // buttonBottom
            // 
            this.buttonBottom.Location = new System.Drawing.Point(327, 236);
            this.buttonBottom.Name = "buttonBottom";
            this.buttonBottom.Size = new System.Drawing.Size(75, 23);
            this.buttonBottom.TabIndex = 2;
            this.buttonBottom.Text = "Bottom";
            this.buttonBottom.UseVisualStyleBackColor = true;
            this.buttonBottom.Click += new System.EventHandler(this.buttonBottom_Click);
            // 
            // buttonOk
            // 
            this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOk.Location = new System.Drawing.Point(152, 236);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(75, 23);
            this.buttonOk.TabIndex = 3;
            this.buttonOk.Text = "Ok";
            this.buttonOk.UseVisualStyleBackColor = true;
            // 
            // listBoxErrors
            // 
            this.listBoxErrors.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxErrors.FormattingEnabled = true;
            this.listBoxErrors.HorizontalScrollbar = true;
            this.listBoxErrors.Location = new System.Drawing.Point(3, 16);
            this.listBoxErrors.Name = "listBoxErrors";
            this.listBoxErrors.Size = new System.Drawing.Size(384, 199);
            this.listBoxErrors.TabIndex = 0;
            // 
            // FormDisplayErrors
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(414, 262);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.buttonBottom);
            this.Controls.Add(this.buttonTop);
            this.Controls.Add(this.groupBoxErrors);
            this.MinimizeBox = false;
            this.Name = "FormDisplayErrors";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Display Errrors";
            this.groupBoxErrors.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxErrors;
        private System.Windows.Forms.Button buttonTop;
        private System.Windows.Forms.Button buttonBottom;
        private System.Windows.Forms.Button buttonOk;
        public System.Windows.Forms.ListBox listBoxErrors;
    }
}