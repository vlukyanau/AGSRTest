using System;


namespace Logic.Entities
{
    public sealed class Patient : IPatient
    {
        #region Static
        public static Patient New(string name, DateTime birthDate)
        {
            Patient patient = new Patient();
            patient.Name = Name.New(name);
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
        public Name Name { get; set; }
        public Gender? Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public bool? Active { get; set; }
        #endregion
    }
}
