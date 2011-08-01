using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;



namespace WoWGuildOrganizer
{
    public partial class Form1 : Form
    {
        #region " Delegates "
        delegate void SetWaitCursorCallback(Boolean Wait);
        delegate void LabelCallback(String Message);
        delegate void SortGridCallback(String Sorting);
        #endregion


        #region " Functions "
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Index"></param>
        public void WaitCursor(bool Wait)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.InvokeRequired)
            {
                SetWaitCursorCallback d = new SetWaitCursorCallback(WaitCursor);
                this.Invoke(d, new object[] { Wait });
            }
            else
            {
                // enable or disable the wait cursor
                if (Wait)
                {
                    this.Cursor = Cursors.WaitCursor;
                }
                else
                {
                    this.Cursor = Cursors.Default;
                }
            }
        }

        public void UpdateLabelMT(String Message)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.InvokeRequired)
            {
                LabelCallback d = new LabelCallback(UpdateLabelMT);
                this.Invoke(d, new object[] { Message });
            }
            else
            {
                // update label
                this.label3.Text = Message;
            }
        }

        public void SortGridMT(String Sorting)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.InvokeRequired)
            {
                SortGridCallback d = new SortGridCallback(SortGridMT);
                this.Invoke(d, new object[] { Sorting });
            }
            else
            {
                // Code Here

                Boolean MultipleSort = false;


                //SortGrid("Level DESC, GearScore DESC");
                if (Sorting.Contains(","))
                {
                    // has multiple sorting factors
                    MultipleSort = true;
                }

                dataGridViewGuildData.DataSource = null;

                SavedCharacters.SavedCharacters.Sort(new ObjectComparer(Sorting, MultipleSort));

                // refresh grid data
                dataGridViewGuildData.DataSource = SavedCharacters.SavedCharacters;

                // Now update the grid
                UpdateGrid();

                // Set the sorting glyphs
                String[] sortExpressions = Sorting.Trim().Split(',');
                for (Int32 i = 0; i < sortExpressions.Length; i++)
                {
                    String fieldName = "";
                    SortOrder direction = SortOrder.None;

                    if (sortExpressions[i].Trim().EndsWith(" DESC"))
                    {
                        fieldName = sortExpressions[i].Replace(" DESC", "").Trim();
                        direction = SortOrder.Descending;
                    }
                    else
                    {
                        fieldName = sortExpressions[i].Replace(" ASC", "").Trim();
                        direction = SortOrder.Ascending;
                    }

                    foreach (DataGridViewColumn col in dataGridViewGuildData.Columns)
                    {
                        if (fieldName == col.HeaderText)
                        {
                            col.HeaderCell.SortGlyphDirection = direction;
                        }
                    }
                }
                
            }
        }
        #endregion

    }
}
