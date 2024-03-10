using System;

using Core.Repository;


namespace Core
{
    public interface IWork
    {
        IPatientsRepository Patients { get; }
        IHumanNamesRepository HumanNames { get; }

        int Save();
    }

    public class Work : IWork, IDisposable
    {
        #region Static
        public static IWork New()
        {
            return new Work();
        }
        #endregion

        #region Constructors
        private Work()
        {
            ApplicationContext context = new ApplicationContext();

            this.context = context;

            this.Patients = PatientsRepository.New(context);
            this.HumanNames = HumanNamesRepository.New(context);
        }
        #endregion

        #region Fields
        private readonly ApplicationContext context;
        #endregion

        #region Properties
        public IPatientsRepository Patients { get; }
        public IHumanNamesRepository HumanNames { get; }
        #endregion

        #region IWork
        int IWork.Save()
        {
            return this.context.SaveChanges();
        }
        #endregion

        #region IDisposable
        void IDisposable.Dispose()
        {
            this.Dispose(true);

            GC.SuppressFinalize(this);
        }
        #endregion

        #region Methods
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.context.Dispose();
            }
        }
        #endregion
    }
}
