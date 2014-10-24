// -----------------------------------------------------------------------
// <copyright file="GuildMemberGroup.cs" company="Vangent, Inc.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace WoWGuildOrganizer
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    
    /// <summary>
    /// Class of a group of guild members
    /// </summary>
    [Serializable]
    public class GuildMemberGroup
    {
        /// <summary>
        /// An array list of all the characters in this guild
        /// </summary>
        private ArrayList savedCharacters = null;

        /// <summary>
        /// The guild name
        /// </summary>
        private string guild;
                
        /// <summary>
        /// The realm where the guild is
        /// </summary>
        private string realm;
                
        /// <summary>
        /// Initializes a new instance of the <see cref="GuildMemberGroup" /> class.
        /// </summary>
        public GuildMemberGroup()
        {
            this.savedCharacters = new ArrayList();
            this.guild = string.Empty;
            this.realm = string.Empty;
        }

        /// <summary>
        /// Gets or sets the array list of all the guild members
        /// </summary>
        public ArrayList SavedCharacters
        {
            get
            {
                return this.savedCharacters;
            }

            set
            {
                this.savedCharacters = value;
            }
        }

        /// <summary>
        /// Gets or sets the guild name
        /// </summary>
        public string Guild
        {
            get 
            {
                return this.guild; 
            }

            set 
            {
                this.guild = value; 
            }
        }

        /// <summary>
        /// Gets or sets the realm name
        /// </summary>
        public string Realm
        {
            get 
            {
                return this.realm; 
            }

            set 
            {
                this.realm = value; 
            }
        }
    }
}
