using System;


namespace Logic.Entities
{
    public sealed class Patient : IPatient
    {
        #region Static
        public static Patient New()
        {
            Patient patient = new Patient();

            return patient;
        }
        #endregion

        #region Constructors
        private Patient()
        {
            this.Name = Name.New();
        }
        #endregion

        #region Properties
        public Name Name { get; set; }
        public Gender Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public bool Active { get; set; }
        #endregion
    }
}
