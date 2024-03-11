using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Core.Entities;


namespace Logic.Models
{
    /// <summary>
    /// A name associated with the patient  
    /// </summary>
    public sealed class HumanNameInfo : IHumanNameInfo
    {
        #region Static
        public static HumanNameInfo New(IHumanName name)
        {
            HumanNameInfo info = new HumanNameInfo();
            info.Id = name.Id;
            info.Use = name.Use;
            info.Family = name.Family;
            info.Given = name.Given;

            return info;
        }
        #endregion
        
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
