using System;

using Logic.Entities;


namespace Api.Models
{
    public sealed class Patient : IPatient
    {
        #region Constructors
        public Patient()
        {
        }
        #endregion

        #region Properties
        public Guid? Id { get; set; }
        public HumanName Name { get; set; }
        public Gender? Gender { get; set; }
        public DateTime? BirthDate { get; set; }
        public bool? Active { get; set; }
        #endregion
    }
}
