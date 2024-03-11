using System;


namespace Core.Entities
{
    public sealed class Patient : Entity, IPatient
    {
        #region Static
        public static Patient New(Guid humanNameId)
        {
            Patient patient = new Patient();
            patient.Id = Guid.NewGuid();
            patient.HumanNameId = humanNameId;

            return patient;
        }
        #endregion

        #region Constructors
        private Patient()
        {
        }
        #endregion

        #region Properties
        public Guid Id { get; private set; }
        public Guid HumanNameId { get; private set; }
        public Gender? Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public bool? Active { get; set; }
        #endregion
    }
}
