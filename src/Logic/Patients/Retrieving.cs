using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using Core;
using Core.Entities;

using Logic.Models;


namespace Logic.Patients
{
    public static partial class Patients
    {
        public sealed class Retrieving
        {
            #region Interfaces
            private interface ILoading
            {
                Task<IReadOnlyDictionary<Guid, Patient>> GetPatients();
                Task<Patient> GetPatient(Guid id);
                Task<HumanName> GetHumanName(Guid id);
            }
            #endregion

            #region Classes
            private class Loading : ILoading
            {
                #region Constructors
                public Loading(IWorker work)
                {
                    this.Worker = work;
                }
                #endregion

                #region Properties
                private IWorker Worker { get; }

                private IReadOnlyDictionary<Guid, Patient> Patients { get; set; }
                private IReadOnlyDictionary<Guid, HumanName> HumanNames { get; set; }
                #endregion

                #region ILoading
                Task<IReadOnlyDictionary<Guid, Patient>> ILoading.GetPatients()
                {
                    return this.GetPatients();
                }
                Task<Patient> ILoading.GetPatient(Guid id)
                {
                    return this.GetPatient(id);
                }
                Task<HumanName> ILoading.GetHumanName(Guid id)
                {
                    return this.GetHumanName(id);
                }
                #endregion

                #region Assistents
                private async Task<IReadOnlyDictionary<Guid, Patient>> GetPatients()
                {
                    if (this.Patients == null)
                    {
                        IReadOnlyList<Patient> patients = await this.Worker.Patients.GetAll();

                        this.Patients = patients.ToDictionary(item => item.Id);
                    }

                    return this.Patients;
                }
                private async Task<Patient> GetPatient(Guid id)
                {
                    IReadOnlyDictionary<Guid, Patient> patients = await this.GetPatients();

                    return patients.GetValueOrDefault(id);
                }

                private async Task<IReadOnlyDictionary<Guid, HumanName>> GetHumanNames()
                {
                    if (this.HumanNames == null)
                    {
                        IReadOnlyDictionary<Guid, Patient> patients = await this.GetPatients();

                        IReadOnlyList<Guid> ids = patients.Values.Ids(item => item.HumanNameId);

                        IReadOnlyList<HumanName> humanNames = await this.Worker.HumanNames.Where(item => ids.Contains(item.Id));

                        this.HumanNames = humanNames.ToDictionary(item => item.Id);
                    }

                    return this.HumanNames;
                }
                private async Task<HumanName> GetHumanName(Guid id)
                {
                    IReadOnlyDictionary<Guid, HumanName> humanNames = await this.GetHumanNames();

                    return humanNames.GetValueOrDefault(id);
                }
                #endregion
            }
            #endregion

            #region Static
            public static Retrieving New()
            {
                return new Retrieving();
            }
            #endregion

            #region Constructors
            private Retrieving()
            {
                this.worker = Worker.New();
            }
            #endregion

            #region Fields
            private readonly IWorker worker;
            #endregion

            #region Methods
            public async Task<IResult> Go()
            {
                try
                {
                    IResult result = await this.Process();

                    return result;
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);

                    return Result.New(exception);
                }
            }
            public async Task<IResult> Go(Guid id)
            {
                try
                {
                    IResult result = await this.Process(id);

                    return result;
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);

                    return Result.New(exception);
                }
            }
            #endregion

            #region Assistants
            private async Task<IResult> Process()
            {
                ILoading loading = new Loading(this.worker);

                IReadOnlyDictionary<Guid, Patient> patients = await loading.GetPatients();

                List<IPatientInfo> infos = new List<IPatientInfo>();

                foreach ((_, IPatient patient) in patients)
                {
                    HumanName humanName = await loading.GetHumanName(patient.HumanNameId);
                    if (humanName == null)
                        return Result.NotFound;

                    PatientInfo info = PatientInfo.New(patient, humanName);

                    infos.Add(info);
                }

                return Result.New(infos);
            }
            private async Task<IResult> Process(Guid id)
            {
                ILoading loading = new Loading(this.worker);

                Patient patient = await loading.GetPatient(id);
                if (patient == null)
                    return Result.NotFound;

                HumanName humanName = await loading.GetHumanName(patient.HumanNameId);
                if (humanName == null)
                    return Result.NotFound;

                IPatientInfo info = PatientInfo.New(patient, humanName);

                return Result.New(info);
            }
            #endregion
        }
    }
}
