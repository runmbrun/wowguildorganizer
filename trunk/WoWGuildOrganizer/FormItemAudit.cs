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


        public FormItemAudit()
        {
            InitializeComponent();

            ItemAuditList = new ArrayList();
        }

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

                // null out previous data
                dataGridViewItemAudit.DataSource = null;

                // refresh grid data
                dataGridViewItemAudit.DataSource = ItemAuditList;

                // refresh the grid data since it's been changed
                dataGridViewItemAudit.Refresh();

                // Resize the rows
                dataGridViewItemAudit.RowsDefaultCellStyle.BackColor = Color.White;
                dataGridViewItemAudit.AutoResizeColumns();
                dataGridViewItemAudit.AutoResizeRows();

                Success = true;
            }
            catch (Exception ex)
            {
                String Error = ex.Message;
            }

            return Success;
        }
    }
}
