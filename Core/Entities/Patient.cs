using System;


namespace Core.Entities
{
    public sealed class Patient : IPatient
    {
        #region Static
        public static Patient New()
        {
            Patient patient = new Patient();
            patient.Id = Guid.NewGuid();
            patient.Name = HumanName.New();

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
