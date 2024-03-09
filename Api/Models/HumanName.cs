using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Api.Models
{
    /// <summary>
    /// A name associated with the patient  
    /// </summary>
    public sealed class HumanName : IHumanName
    {
        #region Properties
        /// <summary>
        /// Id 
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// usual | official | temp | nickname | anonymous | old | maiden
        /// </summary>
        [MinLength(1)]
        public string Use { get; set; }

        /// <summary>
        /// Family name (often called 'Surname')
        /// </summary>
        [Required]
        [MinLength(1)]
        public string Family { get; set; }

        /// <summary>
        /// Given names (not always 'first'). Includes middle names
        /// This repeating element order: Given Names appear in the correct order for presenting the name
        /// </summary>
        public List<string> Given { get; set; }
        #endregion
    }
}
