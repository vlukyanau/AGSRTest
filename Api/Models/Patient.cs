using System;
using System.ComponentModel.DataAnnotations;

using Core.Entities;


namespace Api.Models
{
    /// <summary>
    /// Information about an individual or animal receiving health care services
    /// </summary>
    public sealed class Patient : IPatient
    {
        #region Properties
        /// <summary>
        /// Id 
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// A name associated with the patient
        /// </summary>
        public HumanName Name { get; set; }

        /// <summary>
        /// male | female | other | unknown
        /// </summary>
        public Gender? Gender { get; set; }

        /// <summary>
        /// The date of birth for the individual 
        /// </summary>
        [Required]
        public DateTime? BirthDate { get; set; }

        /// <summary>
        /// Whether this patient's record is in active use 
        /// </summary>
        public bool? Active { get; set; }
        #endregion
    }
}
