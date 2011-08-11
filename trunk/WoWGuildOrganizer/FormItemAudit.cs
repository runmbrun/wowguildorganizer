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

        public ArrayList ItemAuditList = null;


        /// <summary>
        /// 
        /// </summary>
        public FormItemAudit()
        {
            InitializeComponent();

            ItemAuditList = new ArrayList();

            textBoxAuditStatus.Text = "Failed";
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
                    ItemAuditList = itemAudit.ItemAudits;
                }

                //TODO:  Do AUDIT stuff here!

                
                Int32 MissingItems = ItemAuditList.Count;
                Int32 MissingEnchants = 0;
                Int32 MissingGems = 0;
                Int32 MissingTotal = 0;

                
                // Check each item that the character has equiped
                foreach (ItemAudit item in ItemAuditList)
                {
                    // Check for missing Items
                    //   There can be a total of 19 items on a person.
                    //    2 are optional, the Tabard and Shirt.
                    // So, get total count, check if tabard and shirt need to be "uncounted"
                    // and confirm that number is 17.
                    if (item.Slot == "shirt")
                    {
                        MissingItems -= 1;
                    }
                    if (item.Slot == "tabard")
                    {
                        MissingItems -= 1;
                    }

                    // Check for missing enchants
                    if (item.MissingEnchant != "0")
                    {
                        MissingEnchants++;
                    }

                    // Check for missing enchants
                    if (item.MissingGem != "0")
                    {
                        MissingGems++;
                    }
                }

                // Count up the missing items
                MissingTotal += (ItemAuditList.Count - MissingItems);
                textBoxMissingItems.Text = (17 - MissingItems).ToString();

                // Count the missing enchants
                MissingTotal += MissingEnchants;
                textBoxMissingEnchants.Text = MissingEnchants.ToString();

                // Count the missing gems
                MissingTotal += MissingGems;
                textBoxMissingGems.Text = MissingGems.ToString();

                // Count the total missing 
                textBoxMissingTotal.Text = MissingTotal.ToString();

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

            // Color the cells now...  Columns 4, 5, 6.
            for (Int32 i = 0; i < dataGridViewItemAudit.Rows.Count; i++)
            {
                // Any Missing Items?
                if (dataGridViewItemAudit.Rows[i].Cells[4].Value.ToString() != "0")
                {
                    dataGridViewItemAudit.Rows[i].Cells[4].Style.BackColor = Color.Red;
                }

                // Any Missing Enchants?
                if (dataGridViewItemAudit.Rows[i].Cells[5].Value.ToString() != "0")
                {
                    dataGridViewItemAudit.Rows[i].Cells[5].Style.BackColor = Color.Red;
                }

                // Any Missing Gems?
                if (dataGridViewItemAudit.Rows[i].Cells[6].Value.ToString() != "0")
                {
                    dataGridViewItemAudit.Rows[i].Cells[6].Style.BackColor = Color.Red;
                }
            }

            // refresh the grid data since it's been changed
            dataGridViewItemAudit.Refresh();

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
