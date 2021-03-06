﻿// -----------------------------------------------------------------------
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
    public static class Logging
    {
        /// <summary>
        /// Array list of all the messages that need to be logged
        /// </summary>
        private static ArrayList logging = new ArrayList();

        /// <summary>
        /// Log a message.  Add it to the array list
        /// </summary>
        /// <param name="message">string of the message to be logged</param>
        public static void Log(string message)
        {
            logging.Add(message);
        }

        /// <summary>
        /// Log a message with a Debug string.  Add it to the array list
        /// </summary>
        /// <param name="message">string of the message to be logged</param>
        public static void Debug(string message)
        {
            logging.Add(string.Format("DEBUG: {0}", message));
        }

        /// <summary>
        /// Log a message with a Warning string.  Add it to the array list
        /// </summary>
        /// <param name="message">string of the message to be logged</param>
        public static void Warning(string message)
        {
            logging.Add(string.Format("WARNING: {0}", message));
        }

        /// <summary>
        /// Log a message with an Error string.  Add it to the array list
        /// </summary>
        /// <param name="message">string of the message to be logged</param>
        public static void Error(string message/*, bool display = true*/)
        {
            logging.Add(string.Format("ERROR: {0}", message));
            logging.Add(string.Format("\tStackTrace: {0}", Environment.StackTrace));
        }

        /// <summary>
        /// First log the error, and pop up a message box with that error
        /// </summary>
        /// <param name="message">string of the error to be logged and displayed</param>
        public static void DisplayError(string message)
        {
            string newMessage = string.Format("ERROR: {0}", message);

            Error(newMessage);
            MessageBox.Show(newMessage);            
        }

        /// <summary>
        /// Pop up a form that will display all logging and errors in a nice form
        /// </summary>
        public static void ShowAllErrors()
        {
            FormDisplayErrors frm = new FormDisplayErrors();

            foreach (string s in logging)
            {
                frm.listBoxErrors.Items.Add(s);
            }

            frm.ShowDialog();
        }
    }
}
