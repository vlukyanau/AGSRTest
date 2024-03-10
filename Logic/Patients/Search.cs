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
        public sealed class Search
        {
            #region Enums
            private enum Prefix
            {
                eq = 0,   // Equal
                ne,       // NotEqual
                gt,       // GreaterThan
                lt,       // LessThan
                ge,       // GreaterthanOrEquals
                le,       // LessThanOrEquals
                sa,       // StartAfter
                eb,       // EndBefore
                ap        // Approximate
            }
            #endregion

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
                public Loading(IWork work, IReadOnlyList<Tuple> dates)
                {
                    this.Work = work;
                    this.Dates = dates;
                }
                #endregion

                #region Properties
                private IWork Work { get; }
                private IReadOnlyList<Tuple> Dates;

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
                        IQueryable<Patient> patients = this.Work.Patients.GetAll();

                        foreach (Tuple tuple in this.Dates)
                        {
                            this.FilterDate(patients, tuple);
                        }

                        this.Patients = await patients.ToDictionaryAsync(item => item.Id);
                    }

                    return this.Patients;
                }

                private async Task<IReadOnlyDictionary<Guid, HumanName>> GetHumanNames()
                {
                    if (this.HumanNames == null)
                    {
                        IReadOnlyDictionary<Guid, Patient> patients = await this.GetPatients();

                        IReadOnlyList<Guid> ids = patients.Values.Ids(item => item.HumanNameId);

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

                private IQueryable<Patient> FilterDate(IQueryable<Patient> query, Tuple tuple)
                {
                    switch (tuple.Prefix)
                    {
                        case Prefix.eq:
                            query = query.Where(patient => patient.BirthDate == tuple.Date);
                            break;

                        case Prefix.ne:
                            query = query.Where(patient => patient.BirthDate != tuple.Date);
                            break;

                        case Prefix.gt:
                            query = query.Where(patient => patient.BirthDate > tuple.Date);
                            break;

                        case Prefix.lt:
                            query = query.Where(patient => patient.BirthDate < tuple.Date);
                            break;

                        case Prefix.ge:
                            query = query.Where(patient => patient.BirthDate >= tuple.Date);
                            break;

                        case Prefix.le:
                            query = query.Where(patient => patient.BirthDate <= tuple.Date);
                            break;

                        // TODO: Implement
                        case Prefix.sa:
                            return null;

                        case Prefix.eb:
                            return null;

                        case Prefix.ap:
                            return null;

                        default:
                            return null;
                    }

                    return query;
                }
                #endregion
            }

            private class Tuple
            {
                #region Static
                public static Tuple New(string date)
                {
                    if (Enum.TryParse(date[..2], true, out Prefix prefix) == false)
                        return null;

                    if (DateTime.TryParse(date[2..], out DateTime result) == false)
                        return null;

                    Tuple tuple = new Tuple();
                    tuple.Date = result.ToUniversalTime();
                    tuple.Prefix = prefix;

                    return tuple;
                }
                #endregion

                #region Constructors
                private Tuple()
                {
                }
                #endregion

                #region Properties
                public DateTime Date { get; private set; }
                public Prefix Prefix { get; private set; }
                #endregion
            }
            #endregion

            #region Static
            public static Search New()
            {
                return new Search();
            }
            #endregion

            #region Constructors
            private Search()
            {
                this.work = Work.New();
            }
            #endregion

            #region Fields
            private readonly IWork work;
            #endregion

            #region Properties
            private List<Tuple> Dates { get; } = new List<Tuple>();
            #endregion

            #region Methods
            public IResult Add(string date)
            {
                if (this.Dates.Count > 2)
                    return Result.BadRequest;

                Tuple tuple = Tuple.New(date);
                if (tuple == null)
                    return Result.BadRequest;

                this.Dates.Add(tuple);

                return Result.Ok;
            }
            public IResult AddRange(IReadOnlyList<string> dates)
            {
                if (dates.Count > 2)
                    return Result.BadRequest;

                foreach (string date in dates)
                {
                    Tuple tuple = Tuple.New(date);
                    if (tuple == null)
                        return Result.BadRequest;

                    this.Dates.Add(tuple);
                }

                return Result.Ok;
            }

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
            #endregion

            #region Assistants
            private async Task<IResult> Process()
            {
                ILoading loading = new Loading(this.work, this.Dates);

                IReadOnlyDictionary<Guid, Patient> patients = await loading.GetPatients(); //this.GetPatients();

                //foreach (Tuple date in this.Dates)
                //{
                //    query = this.FilterDate(query, date);
                //    if (query == null)
                //        return Result.BadRequest;
                //}

                //IReadOnlyList<Patient> patients = await query.ToListAsync();

                List<PatientInfo> infos = new List<PatientInfo>();

                foreach ((_, Patient patient) in patients)
                {
                    HumanName humanName = await loading.GetHumanName(patient.Id);
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
