// <copyright file="ControlHelper.cs" company="Secondnorth.com">
//     Secondnorth.com. All rights reserved.
// </copyright>
// <author>Me</author>

namespace WoWGuildOrganizer
{
    #region Includes

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Windows.Forms;

    #endregion

    /// <summary>
    /// Initializes and 
    /// </summary>
    public static class ControlHelper
    {
        #region Redraw Suspend/Resume

        /// <summary>
        /// External DLL 
        /// </summary>
        //todo: [DllImport("user32.dll", EntryPoint = "SendMessageA", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]

        /// <summary>
        /// constant property value
        /// </summary>
        private const int SETREDRAW = 0xB;

        /// <summary>
        /// Suspend Drawing
        /// </summary>
        /// <param name="target">target control</param>
        public static void SuspendDrawing(this Control target)
        {
            SendMessage(target.Handle, SETREDRAW, 0, 0);
        }

        /// <summary>
        /// Resume Drawing
        /// </summary>
        /// <param name="target">target drawing</param>
        public static void ResumeDrawing(this Control target) 
        { 
            ResumeDrawing(target, true); 
        }

        /// <summary>
        /// Resume Drawing
        /// </summary>
        /// <param name="target">target control</param>
        /// <param name="redraw">redraw flag</param>
        public static void ResumeDrawing(this Control target, bool redraw)
        {
            SendMessage(target.Handle, SETREDRAW, 1, 0);

            if (redraw)
            {
                target.Refresh();
            }
        }

        /// <summary>
        /// External Send Message windows function
        /// </summary>
        /// <param name="hwnd">handle parameter</param>
        /// <param name="windowMessage">windows message</param>
        /// <param name="windowsParam">w parameter</param>
        /// <param name="leftParam">l parameter</param>
        /// <returns>result value</returns>
        private static extern int SendMessage(IntPtr hwnd, int windowMessage, int windowsParam, int leftParam);

        #endregion
    }
}
