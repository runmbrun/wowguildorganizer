// -----------------------------------------------------------------------
// <copyright file="FormDisplayErrors.cs" company="Vangent, Inc.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace WoWGuildOrganizer
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Windows.Forms;

    /// <summary>
    /// Form class that displays all the errors
    /// </summary>
    public partial class FormDisplayErrors : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FormDisplayErrors" /> class.
        /// </summary>
        public FormDisplayErrors()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Select the first item in the list
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">e parameter</param>
        private void ButtonTop_Click(object sender, EventArgs e)
        {            
            this.listBoxErrors.SelectedIndex = 0;
        }

        /// <summary>
        /// Select the last item in the list
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">e parameter</param>
        private void ButtonBottom_Click(object sender, EventArgs e)
        {            
            this.listBoxErrors.SelectedIndex = this.listBoxErrors.Items.Count - 1;
        }
    }
}
