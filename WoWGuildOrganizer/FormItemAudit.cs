// <copyright file="FormItemAudit.cs" company="Secondnorth.com">
//     Secondnorth.com. All rights reserved.
// </copyright>
// <author>Me</author>

namespace WoWGuildOrganizer
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Windows.Forms;

    /// <summary>
    /// Class that has the Item Audit information for the Form
    /// </summary>
    public partial class FormItemAudit : Form
    {
        #region " Class members "

        /// <summary>
        /// Character's Name
        /// </summary>
        private string name;

        /// <summary>
        /// Character's max item level
        /// </summary>
        private string maxilevel;

         /// <summary>
        /// Character's equipped item level
        /// </summary>
        private string equippedilevel;

        /// <summary>
        /// Character's first profession
        /// </summary>
        private string profession1;

        /// <summary>
        /// Character's second profession
        /// </summary>
        private string profession2;

        /// <summary>
        /// Character's specialization
        /// </summary>
        private string spec;

        /// <summary>
        /// Character's role
        /// </summary>
        private string role;

        /// <summary>
        /// Array list of all the items for audit
        /// </summary>
        private ArrayList itemauditlist;

        #endregion

        #region " Class Constructor "

        /// <summary>
        /// Initializes a new instance of the <see cref="FormItemAudit" /> class.
        /// </summary>
        public FormItemAudit()
        {
            this.InitializeComponent();

            this.ItemAuditList = new ArrayList();
        }

        #endregion

        #region " Class Properties "

        /// <summary>
        /// Gets or sets the Array list of all the items for audit
        /// </summary>
        public ArrayList ItemAuditList
        {
            get
            {
                return this.itemauditlist;
            }

            set
            {
                this.itemauditlist = value;
            }
        }

        /// <summary>
        /// Gets or sets the Character Name
        /// </summary>
        public string CharacterName
        {
            get
            {
                return this.name;
            }

            set
            {
                this.name = value;
                labelName.Text = this.name;
            }
        }

        /// <summary>
        /// Gets or sets the Character's max item level
        /// </summary>
        public string MaxiLevel
        {
            get
            {
                return this.maxilevel;
            }

            set
            {
                this.maxilevel = value;
            }
        }

        /// <summary>
        /// Gets or sets the Character's equipped item level
        /// </summary>
        public string EquippediLevel
        {
            get
            {
                return this.equippedilevel;
            }

            set
            {
                this.equippedilevel = value;
            }
        }

        /// <summary>
        /// Gets or sets the Character's first profession
        /// </summary>
        public string Profession1
        {
            get
            {
                return this.profession1;
            }

            set
            {
                this.profession1 = value;
            }
        }

        /// <summary>
        /// Gets or sets the Character's second profession
        /// </summary>
        public string Profession2
        {
            get
            {
                return this.profession2;
            }

            set
            {
                this.profession2 = value;
            }
        }

        /// <summary>
        /// Gets or sets the Character's specialization
        /// </summary>
        public string Spec
        {
            get
            {
                return this.spec;
            }

            set
            {
                this.spec = value;
            }
        }

        /// <summary>
        /// Gets or sets the Character's role
        /// </summary>
        public string Role
        {
            get
            {
                return this.role;
            }

            set
            {
                this.role = value;
            }
        }

        #endregion
        
        #region " Class Functions "

        /// <summary>
        /// Passes in a character class, and then does the audit against it
        /// </summary>
        /// <param name="character">character class value</param>
        /// <returns>true if successful</returns>
        public bool PassData(GuildMember character)
        {
            bool success = false;

            try
            {
                // fill out the character information
                this.CharacterName = character.Name;
                this.EquippediLevel = character.EquipediLevel.ToString();
                this.MaxiLevel = character.MaxiLevel.ToString();
                this.Profession1 = character.Profession1;
                this.Profession2 = character.Profession2;
                this.Spec = character.Spec;
                this.Role = character.Role;

                // Now fill out the item audit information
                if (character.ItemAudits.Count <= 0)
                {
                    MessageBox.Show("This character can't be updated... please update this character.");
                }
                else
                {
                    foreach (ItemAudit a in character.ItemAudits.Values)
                    {
                        this.ItemAuditList.Add(a);
                    }
                }

                // Fill out the Max iLevel information                
                textBoxMaxiLevel.Text = character.MaxiLevel.ToString();

                // Fill out the profession information
                textBoxProfessions.Text = character.Profession1 + ", " + character.Profession2;

                // Fill out the spec information
                labelSpec.Text = "Spec: " + character.Spec;

                // Fill out the role information
                labelRole.Text = "Role: " + character.Role;

                // Do AUDIT stuff here!                
                int missingItems = 0;
                int missingEnchants = 0;
                int missingGems = 0;
                int missingTotal = 0;
                double itemLevels = 0;
                int itemCount = 0;
                bool twoHanded = false;
                bool dualMainHands = false;
                
                // Check each item that the character has equiped
                foreach (ItemAudit item in this.ItemAuditList)
                {
                    // Figure out if the mainHand is a two handed weapon
                    if (item.Slot == "mainHand")
                    {
                        // Check to see if weapon is two handed
                        if (FormMain.Items.GetItem(item.Id, item.Context).InventoryType == 17 ||  // 2 hander
                            FormMain.Items.GetItem(item.Id, item.Context).InventoryType == 26 ||  // Gun
                            FormMain.Items.GetItem(item.Id, item.Context).InventoryType == 15)    // Bow
                        {
                            twoHanded = true;
                        }
                        else if (FormMain.Items.GetItem(item.Id, item.Context).InventoryType == 13)  // one hander
                        {
                            dualMainHands = true;
                        }
                    }
                    
                    // Check for missing Items
                    if (item.MissingItem != "0")
                    {
                        // Make sure only two handed weapons don't have off hand
                        // Make sure that offHands allow One Hand also
                        if (!(item.Slot == "offHand" && twoHanded) && !(item.Slot == "offHand" && dualMainHands) && item.Slot != "tabard" && item.Slot != "shirt")
                        {
                            // Missing this item
                            missingItems++;
                            Logging.Error(string.Format("This item is missing: [{0}] - [{1}]", item.Slot, item.Name));
                        }
                        else
                        {
                            // Not missing this item
                            item.MissingItem = "0";
                        }
                    }
                    else
                    {
                        if (item.Slot != "tabard" && item.Slot != "shirt" && !(item.Slot == "offHand" && twoHanded))
                        {
                            itemLevels += item.ItemLevel;
                            itemCount++;
                        }
                    }

                    // Check for missing enchants
                    if (item.MissingEnchant != "0")
                    {
                        missingEnchants++;
                    }

                    // Check for missing gems
                    if (item.MissingGem != "0")
                    {
                        missingGems += Convert.ToInt32(item.MissingGem);
                    }
                }

                // Now double check the Equipped iLevel value!
                textBoxEquippediLevel.Text = string.Format("{0:0.##}  ({1})", itemLevels / Convert.ToDouble(itemCount), this.EquippediLevel);

                // Count up the missing items
                missingTotal += missingItems;
                textBoxMissingItems.Text = missingItems.ToString();

                // Count the missing enchants
                missingTotal += missingEnchants;
                textBoxMissingEnchants.Text = missingEnchants.ToString();

                // Count the missing gems
                missingTotal += missingGems;
                textBoxMissingGems.Text = missingGems.ToString();

                // Count the total missing 
                textBoxMissingTotal.Text = missingTotal.ToString();

                if (missingTotal > 0)
                {
                    textBoxAuditStatus.Text = "Failed";
                }
                else
                {
                    textBoxAuditStatus.Text = "Passed!";
                }

                // Update the grid
                this.UpdateGridData();
                
                success = true;
            }
            catch (Exception ex)
            {
                Logging.Error(string.Format("PassData() failed: {0}", ex.Message));
            }

            return success;
        }

        #endregion

        #region " Class Protected Functions "

        /// <summary>
        /// Fires when a key is pressed while the form is in focus
        /// </summary>
        /// <param name="msg">reference of the message</param>
        /// <param name="keyData">data for the Keys pressed</param>
        /// <returns>true if success or false if ignored</returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                this.Close();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        #endregion
        
        #region " Data Grid View Functions "

        /// <summary>
        /// Update the Data Grid for this audit
        /// </summary>
        private void UpdateGridData()
        {
            // null out previous data
            dataGridViewItemAudit.DataSource = null;

            // refresh grid data
            dataGridViewItemAudit.DataSource = this.ItemAuditList;

            // White out all the cells
            dataGridViewItemAudit.RowsDefaultCellStyle.BackColor = Color.White;

            // refresh the grid data since it's been changed
            dataGridViewItemAudit.Refresh();

            // Color the cells and text now...  
            for (int i = 0; i < dataGridViewItemAudit.Rows.Count; i++)
            {
                // Color Item Name text according to Item Qualtiy
                switch (((ItemAudit)this.ItemAuditList[i]).Quality)
                {
                    // Heirloom = brown
                    case 7:
                        dataGridViewItemAudit.Rows[i].Cells[1].Style.ForeColor = Color.Brown;
                        break;

                    // Artifact = red
                    case 6:
                        dataGridViewItemAudit.Rows[i].Cells[1].Style.ForeColor = Color.Red;
                        break;

                    // Legendary = orange
                    case 5:
                        dataGridViewItemAudit.Rows[i].Cells[1].Style.ForeColor = Color.Orange;
                        break;

                    // Epic = purple
                    case 4:
                        dataGridViewItemAudit.Rows[i].Cells[1].Style.ForeColor = Color.Purple;
                        break;

                    // Rare = blue
                    case 3:
                        dataGridViewItemAudit.Rows[i].Cells[1].Style.ForeColor = Color.Blue;
                        break;

                    // Uncommon = green
                    case 2:
                        dataGridViewItemAudit.Rows[i].Cells[1].Style.ForeColor = Color.LightGreen;
                        break;

                    // Common = white
                    case 1:
                        dataGridViewItemAudit.Rows[i].Cells[1].Style.ForeColor = Color.Black;
                        break;

                    // Poor = gray
                    case 0:
                        dataGridViewItemAudit.Rows[i].Cells[1].Style.ForeColor = Color.LightGray;
                        break;

                    // Unknown
                    default:
                        dataGridViewItemAudit.Rows[i].Cells[1].Style.ForeColor = Color.Red;
                        break;
                }

                // iLevel too low?  -> Column 2
                //   - red if less than equipped iLevel - 20
                //   - orange if in between equipped iLevel - 20 and equipped
                int itemLevel = Convert.ToInt32(dataGridViewItemAudit.Rows[i].Cells[2].Value.ToString());

                if (i != 5 && i != 6 && itemLevel != 0 && itemLevel != 1)
                {
                    // This is subjective...
                    if (itemLevel < (Convert.ToInt32(this.EquippediLevel) - 20))
                    {
                        dataGridViewItemAudit.Rows[i].Cells[2].Style.BackColor = Color.Red;
                    }
                }

                // Any Missing Items? -> Column 3
                if (dataGridViewItemAudit.Rows[i].Cells[3].Value.ToString() != "0")
                {
                    dataGridViewItemAudit.Rows[i].Cells[3].Style.BackColor = Color.Red;
                }

                // Any Missing Enchants? -> Column 4
                if (dataGridViewItemAudit.Rows[i].Cells[4].Value.ToString() != "0")
                {
                    dataGridViewItemAudit.Rows[i].Cells[4].Style.BackColor = Color.Red;
                }

                // Any Missing Gems? -> Column 5
                if (dataGridViewItemAudit.Rows[i].Cells[5].Value.ToString() != "0")
                {
                    dataGridViewItemAudit.Rows[i].Cells[5].Style.BackColor = Color.Red;
                }

                // Create tooltips
                ItemAudit audit = (ItemAudit)this.ItemAuditList[i];

                if (audit.Id > 0)
                {
                    ItemInfo info = FormMain.Items.GetItem(audit.Id, audit.Context);
                    dataGridViewItemAudit.Rows[i].Cells[1].ToolTipText = info.Tooltip;
                }

                // Passed or Failed!
                //   - Red = Failed!
                //   - ?   - Passed!
            }

            // refresh the grid data since it's been changed
            dataGridViewItemAudit.Refresh();

            // Set grid properties
            dataGridViewItemAudit.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // Resize the rows
            dataGridViewItemAudit.AutoResizeColumns();
            dataGridViewItemAudit.AutoResizeRows();
        }

        /// <summary>
        /// Fires with a right click on a line in the data grid
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">e parameter</param>
        private void CopyItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int row = this.dataGridViewItemAudit.SelectedRows[0].Index;

            if (row >= 0)
            {
                Clipboard.SetData(DataFormats.Text, string.Format("{0}", dataGridViewItemAudit.Rows[row].Cells[1].Value.ToString()));
            }
        }

        /// <summary>
        /// Fires with a right click on a line in the data grid
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">e parameter</param>
        private void CopyLineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int row = this.dataGridViewItemAudit.SelectedRows[0].Index;

            if (row >= 0)
            {
                Clipboard.SetData(DataFormats.Text, string.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}", dataGridViewItemAudit.Rows[row].Cells[0].Value.ToString(), dataGridViewItemAudit.Rows[row].Cells[1].Value.ToString(), dataGridViewItemAudit.Rows[row].Cells[2].Value.ToString(), dataGridViewItemAudit.Rows[row].Cells[3].Value.ToString(), dataGridViewItemAudit.Rows[row].Cells[4].Value.ToString(), dataGridViewItemAudit.Rows[row].Cells[5].Value.ToString()));
            }
        }

        #endregion

        #region " Form Functions "

        /// <summary>
        /// This is needed to update the grid with colors
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">e parameter</param>
        private void FormItemAudit_Load(object sender, EventArgs e)
        {
            this.UpdateGridData();
        }
        
        #endregion
    }
}
