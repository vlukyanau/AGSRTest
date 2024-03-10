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
                public Loading(IWork work)
                {
                    this.Work = work;
                }
                #endregion

                #region Properties
                private IWork Work { get; }

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
                        IQueryable<Patient> patients = this.Work.Patients.GetAll();

                        this.Patients = await patients.ToDictionaryAsync(item => item.Id);
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

                        IReadOnlyList<Guid> ids = patients.Keys.ToList();

                        IQueryable<HumanName> humanNames = this.Work.HumanNames.GetAll().Where(item => ids.Contains(item.Id));

                        this.HumanNames = await humanNames.ToDictionaryAsync(item => item.Id);
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
                this.work = Work.New();
            }
            #endregion

            #region Fields
            private readonly IWork work;
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

                    return Result.BadRequest;
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

                    return Result.BadRequest;
                }
            }
            #endregion

            #region Assistants
            private async Task<IResult> Process()
            {
                ILoading loading = new Loading(this.work);

                IReadOnlyDictionary<Guid, Patient> patients = await loading.GetPatients();

                List<IPatientInfo> infos = new List<IPatientInfo>();

                foreach ((_, IPatient patient) in patients)
                {
                    HumanName humanName = await loading.GetHumanName(patient.HumanNameId);
                    if (humanName == null)
                        return Result.BadRequest;

                    PatientInfo info = PatientInfo.New(patient, humanName);

                    infos.Add(info);
                }

                return Result.New(infos);
            }
            private async Task<IResult> Process(Guid id)
            {
                ILoading loading = new Loading(this.work);

                Patient patient = await loading.GetPatient(id);
                if (patient == null)
                    return Result.NotFound;

                HumanName humanName = await loading.GetHumanName(patient.HumanNameId);
                if (humanName == null)
                    return Result.BadRequest;

                IPatientInfo info = PatientInfo.New(patient, humanName);

                return Result.New(info);
            }
            #endregion
        }
    }
}
