namespace Logic.Entities
{
    public class Patient : IPatient
    {
        #region Static
        public static Patient New()
        {
            Patient patient = new Patient();
            patient.Id = Guid.NewGuid();

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
        #endregion

        #region IPatient
        Guid IPatient.Id
        {
            get { return this.Id; }
        }
        #endregion
    }
}
