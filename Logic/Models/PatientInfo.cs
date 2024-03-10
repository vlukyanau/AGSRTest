using System;
using System.ComponentModel.DataAnnotations;

using Core.Entities;


namespace Logic.Models
{
    /// <summary>
    /// Information about an individual or animal receiving health care services
    /// </summary>
    public sealed class PatientInfo : IPatientInfo
    {
        #region Static
        public static PatientInfo New(IPatient patient, IHumanName humanName)
        {
            PatientInfo info = new PatientInfo();
            info.Id = patient.Id;
            info.Name = HumanNameInfo.New(humanName);
            info.Gender = patient.Gender;
            info.BirthDate = patient.BirthDate;
            info.Active = patient.Active;

            return info;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Id 
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// A name associated with the patient
        /// </summary>
        public HumanNameInfo Name { get; set; }

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
