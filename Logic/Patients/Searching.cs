using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Core;
using Core.Entities;
using Logic.Common;
using Logic.Extensions;
using Logic.Models;


namespace Logic.Patients
{
    public static partial class Patients
    {
        public sealed class Searching
        {
            #region Interfaces
            private interface ILoading
            {
                Task<IReadOnlyDictionary<Guid, Patient>> GetPatients();
                Task<HumanName> GetHumanName(Guid id);
            }
            #endregion

            #region Classes
            private class Loading : ILoading
            {
                #region Constructors
                public Loading(IWorker work, IReadOnlyList<Period> periods)
                {
                    this.Worker = work;
                    this.Periods = periods;
                }
                #endregion

                #region Properties
                private IWorker Worker { get; }
                private IReadOnlyList<Period> Periods { get; }

                private IReadOnlyDictionary<Guid, Patient> Patients { get; set; }
                private IReadOnlyDictionary<Guid, HumanName> HumanNames { get; set; }
                #endregion

                #region ILoading
                Task<IReadOnlyDictionary<Guid, Patient>> ILoading.GetPatients()
                {
                    return this.GetPatients();
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
                        // TODO: Can select directly from the database
                        IReadOnlyList<Patient> query = await this.Worker.Patients.GetAll();

                        List<Patient> patients = new List<Patient>(query);

                        foreach (Period period in this.Periods)
                        {
                            patients = patients.Where(item => period.Contains(item.BirthDate)).ToList();
                        }

                        this.Patients = patients.ToDictionary(item => item.Id);
                    }

                    return this.Patients;
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
            public static Searching New()
            {
                return new Searching();
            }
            #endregion

            #region Constructors
            private Searching()
            {
                this.worker = Worker.New();
            }
            #endregion

            #region Fields
            private readonly IWorker worker;
            #endregion

            #region Properties
            private List<Period> Periods { get; } = new List<Period>();
            #endregion

            #region Methods
            public IResult Add(string date)
            {
                Period period = date.GetPeriod();

                this.Periods.Add(period);

                return Result.Ok;
            }
            public IResult AddRange(IReadOnlyList<string> dates)
            {
                foreach (string date in dates)
                {
                    Period period = date.GetPeriod();

                    this.Periods.Add(period);
                }

                return Result.Ok;
            }

            public async Task<IResult> Go()
            {
                try
                {
                    if (this.Verify() == false)
                        return Result.BadRequest;

                    IResult result = await this.Process();

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
            private bool Verify()
            {
                if (this.Periods.Count == 0)
                    return false;

                return true;
            }

            private async Task<IResult> Process()
            {
                ILoading loading = new Loading(this.worker, this.Periods);

                IReadOnlyDictionary<Guid, Patient> patients = await loading.GetPatients();

                List<PatientInfo> infos = new List<PatientInfo>();

                foreach ((_, Patient patient) in patients)
                {
                    HumanName humanName = await loading.GetHumanName(patient.HumanNameId);
                    if (humanName == null)
                        return Result.BadRequest;

                    PatientInfo info = PatientInfo.New(patient, humanName);

                    infos.Add(info);
                }

                return Result.New(infos);
            }
            #endregion
        }
    }
}
