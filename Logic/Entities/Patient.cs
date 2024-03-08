using System;
using System.ComponentModel.DataAnnotations.Schema;


namespace Logic.Entities
{
    public sealed class Patient : IPatient
    {
        #region Static
        public static Patient New(string family, DateTime birthDate)
        {
            Patient patient = new Patient();
            patient.Id = Guid.NewGuid();
            patient.Name = HumanName.New(family);
            patient.BirthDate = birthDate;

            return patient;
        }
        #endregion

        #region Constructors
        private Patient()
        {
        }
        #endregion

        #region Properties
        public Guid Id { get; set; }
        public HumanName Name { get; set; }
        public Gender? Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public bool? Active { get; set; }
        #endregion
    }
}
