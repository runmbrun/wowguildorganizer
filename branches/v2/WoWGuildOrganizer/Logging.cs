// -----------------------------------------------------------------------
// <copyright file="Logging.cs" company="General Dynamics IT">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace WoWGuildOrganizer
{
    using System;
    using System.Collections;
    using System.Linq;
    using System.Text;
    using System.Windows.Forms;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    static public class Logging
    {
        /// <summary>
        /// 
        /// </summary>
        static private ArrayList logging = new ArrayList();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        static public void Log(string message)
        {
            logging.Add(message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        static public void DisplayError(string message)
        {
            MessageBox.Show(string.Format("Error: {0}", message));
        }

        /// <summary>
        /// 
        /// </summary>
        static public void ShowAllErrors()
        {
            FormDisplayErrors frm = new FormDisplayErrors();

            foreach (String s in logging)
            {
                frm.listBoxErrors.Items.Add(s);
            }

            frm.ShowDialog();
        }
    }
}
