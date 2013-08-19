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

        private String _profession1;
        public String Profession1
        {
            get { return _profession1; }
            set { _profession1 = value; }
        }

        private String _profession2;
        public String Profession2
        {
            get { return _profession2; }
            set { _profession2 = value; }
        }

        private String _spec;
        public String Spec
        {
            get { return _spec; }
            set { _spec = value; }
        }

        private String _role;
        public String Role
        {
            get { return _role; }
            set { _role = value; }
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
        public Boolean PassData(GuildMember Character)
        {
            Boolean Success = false;

            try
            {
                // fill out the character information
                this.CharacterName = Character.Name;
                this.EquippediLevel = Character.EquipediLevel.ToString();
                this.MaxiLevel = Character.MaxiLevel.ToString();
                this.Profession1 = Character.Profession1;
                this.Profession2 = Character.Profession2;
                this.Spec = Character.Spec;
                this.Role = Character.Role;

                // Now fill out the item audit information
                if (Character.ItemAudits.Count <= 0)
                {
                    MessageBox.Show("This character can't be updated... please redo entire guild or raid group.");
                }
                else
                {
                    foreach (ItemAudit a in Character.ItemAudits.Values)
                    {
                        this.ItemAuditList.Add(a);
                    }
                }

                // Fill out the Max iLevel information                
                textBoxMaxiLevel.Text = Character.MaxiLevel.ToString();

                // Fill out the profession information
                textBoxProfessions.Text = Character.Profession1 + ", " + Character.Profession2;

                // Fill out the spec information
                labelSpec.Text = "Spec: " + Character.Spec;

                // Fill out the role information
                labelRole.Text = "Role: " + Character.Role;

                
                // Do AUDIT stuff here!                
                Int32 MissingItems = 0;
                Int32 MissingEnchants = 0;
                Int32 MissingGems = 0;
                Int32 MissingJCGems = 0;
                Int32 MissingCogs = 0;
                Int32 MissingProfs = 0;
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
                        // Check to see if weapon is two handed
                        if (Form1.Items.GetItem(item.Id).InventoryType == 17 ||
                            Form1.Items.GetItem(item.Id).InventoryType == 26)
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

                    // Check for Profession Specials

                    // Blacksmith - Check for special Socket on bracer and hands
                    if ((Profession1 == "Blacksmithing" || Profession2 == "Blacksmithing") && (item.Slot == "wrist" || item.Slot == "hands"))
                    {
                        if (!item.IsBlacksmithingSocket())
                        {
                            item.Profession = "1";
                            MissingProfs++;
                        }
                        else
                        {
                            item.Profession = "0";
                        }
                    }

                    // Enchanting - Check for enchants on both rings
                    if ((Profession1 == "Enchanting" || Profession2 == "Enchanting") && (item.Slot == "finger1" || item.Slot == "finger2"))
                    {   
                        if (!item.IsEnchanted())
                        {
                            item.Profession = "1";
                            MissingProfs++;
                        }
                        else
                        {
                            item.Profession = "0";
                        }
                    }

                    // Enchanting - Check for enchants on both rings
                    if ((Profession1 == "Engineering" || Profession2 == "Engineering") && (item.Slot == "head"))
                    {
                        // TODO - mmb fix this!
                        if (item.IsEngineeringCog())
                        {
                            MissingCogs++;
                        }
                    }

                    // Enchanting - Check for Inscription Enchant on shoulder
                    if ((Profession1 == "Inscription" || Profession2 == "Inscription") && (item.Slot == "shoulder"))
                    {
                        if (!item.IsInscriptionEnchant())
                        {
                            item.Profession = "1";
                            MissingProfs++;
                        }
                        else
                        {
                            item.Profession = "0";
                        }
                    }

                    // Leatherworking - Check for Enchant on Bracer
                    if ((Profession1 == "Leatherworking" || Profession2 == "Leatherworking") && (item.Slot == "wrist"))
                    {
                        if (!item.IsLeatherworkingEnchant())
                        {
                            item.Profession = "1";
                            MissingProfs++;
                        }
                        else
                        {
                            item.Profession = "0";
                        }
                    }

                    // Jewelcrafting - Check for 2 special JC gems                    
                    if (Profession1 == "Jewelcrafting" || Profession2 == "Jewelcrafting")
                    {
                        if (item.IsJewelcraftingGem())
                        {
                            item.Profession = "0";
                            MissingJCGems++;
                        }
                    }

                    // Tailor - Check for special enchant on cloak
                    if ((Profession1 == "Tailoring" || Profession2 == "Tailoring") && (item.Slot == "back"))
                    {
                        if (!item.IsTailorEnchant())
                        {
                            item.Profession = "1";
                            MissingProfs++;
                        }
                        else
                        {
                            item.Profession = "0";
                        }
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

                // Count the missing Jewelcrafting Gems
                if (Profession1 == "Jewelcrafting" || Profession2 == "Jewelcrafting")
                {
                    if (MissingJCGems < 2)
                    {
                        MissingJCGems = 2 - MissingJCGems;
                        MissingProfs += MissingJCGems;
                    }
                }

                // Engineering... more research needed!
                if (Profession1 == "Engineering" || Profession2 == "Engineering")
                {
                    /*
                     * // TODO - mmb
                        // need to do something more to show this is missing.  but how?
                    if (MissingCogs < 3)
                    {
                        MissingCogs = 3 - MissingCogs;
                        MissingProfs += MissingCogs;

                        
                    }
                     * */
                }

                // Count the missing profession specials
                MissingTotal += MissingProfs;
                textBoxMissingProfessions.Text = MissingProfs.ToString();

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
                switch (((ItemAudit)ItemAuditList[i]).Quality)
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
                Int32 iLevel = Convert.ToInt32(dataGridViewItemAudit.Rows[i].Cells[2].Value.ToString());
                if (i != 5 && i != 6 && iLevel != 0 && iLevel != 1)
                {
                    // This is subjective... TODO - mmb
                    if (iLevel < (Convert.ToInt32(EquippediLevel) - 20))
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

                // Any Missing Profession Specials? -> Column 6
                if (dataGridViewItemAudit.Rows[i].Cells[6].Value != null && 
                    dataGridViewItemAudit.Rows[i].Cells[6].Value.ToString() != "0" &&
                    dataGridViewItemAudit.Rows[i].Cells[6].Value.ToString() != "")
                {
                    dataGridViewItemAudit.Rows[i].Cells[6].Style.BackColor = Color.Red;
                }

                // Create tooltips
                ItemAudit audit = (ItemAudit)ItemAuditList[i];
                ItemInfo info = Form1.Items.GetItem(audit.Id);
                dataGridViewItemAudit.Rows[i].Cells[1].ToolTipText = info.CreateTooltip();

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
