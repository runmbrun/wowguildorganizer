using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;



namespace WoWGuildOrganizer
{
    public partial class FormItemAudit : Form
    {
        private String _name;
        public String CharacterName
        {
            get { return _name; }
            set { _name = value; labelName.Text = _name; }
        }

        private String _maxilevel;
        public String MaxiLevel
        {
            get { return _maxilevel; }
            set { _maxilevel = value; }
        }

        private String _equippedilevel;
        public String EquippediLevel
        {
            get { return _equippedilevel; }
            set { _equippedilevel = value; }
        }

        ArrayList ItemAuditList = null;


        /// <summary>
        /// 
        /// </summary>
        public FormItemAudit()
        {
            InitializeComponent();

            ItemAuditList = new ArrayList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Website"></param>
        /// <returns></returns>
        public Boolean PassData(String Website)
        {
            Boolean Success = false;


            try
            {
                GetCharacterItemInfo itemAudit = new GetCharacterItemInfo();
                if (itemAudit.CollectData(Website))
                {
                    foreach(ItemAudit a in itemAudit.ItemAudits.Values)
                    {
                        ItemAuditList.Add(a);
                    }
                }


                // Fill out the Max iLevel information                
                textBoxMaxiLevel.Text = MaxiLevel;

                
                // Do AUDIT stuff here!                
                Int32 MissingItems = 0;
                Int32 MissingEnchants = 0;
                Int32 MissingGems = 0;
                Int32 MissingTotal = 0;
                Double iLevels = 0;
                Int32 ItemCount = 0;
                Boolean TwoHanded = false;
                
                
                // Check each item that the character has equiped
                foreach (ItemAudit item in ItemAuditList)
                {
                    // Figure out if the mainHand is a two handed weapon
                    if (item.Slot == "mainHand")
                    {
                        if (Form1.Items.GetItem(item.GetId()).InventoryType == 17)
                        {
                            TwoHanded = true;
                        }
                    }

                    // Check for missing Items
                    if (item.MissingItem != "0")
                    {
                        // Make sure only two handed weapons don't have off hand
                        if (!(item.Slot == "offHand" && TwoHanded) && item.Slot != "tabard" && item.Slot != "shirt")
                        {
                            // Missing this item
                            MissingItems++;
                        }
                        else
                        {
                            // Not missing this item
                            item.MissingItem = "0";
                        }
                    }
                    else
                    {
                        if (item.Slot != "tabard" && item.Slot != "shirt")
                        {
                            iLevels += item.ItemLevel;
                            ItemCount++;
                        }
                    }

                    // Check for missing enchants
                    if (item.MissingEnchant != "0")
                    {
                        MissingEnchants++;
                    }

                    // Check for missing enchants
                    if (item.MissingGem != "0")
                    {
                        MissingGems += Convert.ToInt32(item.MissingGem);
                    }
                }

                // Now double check the Equipped iLevel value!
                textBoxEquippediLevel.Text = String.Format("{0:0.##}  ({1})", (iLevels / Convert.ToDouble(ItemCount)), EquippediLevel);

                // Count up the missing items
                MissingTotal += MissingItems;
                textBoxMissingItems.Text = MissingItems.ToString();

                // Count the missing enchants
                MissingTotal += MissingEnchants;
                textBoxMissingEnchants.Text = MissingEnchants.ToString();

                // Count the missing gems
                MissingTotal += MissingGems;
                textBoxMissingGems.Text = MissingGems.ToString();

                // Count the total missing 
                textBoxMissingTotal.Text = MissingTotal.ToString();

                if (MissingTotal > 0)
                {
                    textBoxAuditStatus.Text = "Failed";
                }
                else
                {
                    textBoxAuditStatus.Text = "Passed!";
                }

                // Update the grid
                UpdateGridData();
                
                Success = true;
            }
            catch (Exception ex)
            {
                String Error = ex.Message;
            }

            return Success;
        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdateGridData()
        {
            // null out previous data
            dataGridViewItemAudit.DataSource = null;

            // refresh grid data
            dataGridViewItemAudit.DataSource = ItemAuditList;

            // White out all the cells
            dataGridViewItemAudit.RowsDefaultCellStyle.BackColor = Color.White;

            // refresh the grid data since it's been changed
            dataGridViewItemAudit.Refresh();

            // Color the cells and text now...  
            for (Int32 i = 0; i < dataGridViewItemAudit.Rows.Count; i++)
            {
                // Color Item Name text according to Item Qualtiy
                switch (((ItemAudit)ItemAuditList[i]).GetQuality())
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
                //TODO mmb

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

        #region " Form Functions "

        /// <summary>
        /// This is needed to update the grid with colors
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormItemAudit_Load(object sender, EventArgs e)
        {
            UpdateGridData();
        }

        #endregion
    }
}
