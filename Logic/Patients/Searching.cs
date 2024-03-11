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
        public sealed class Searching
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

                        this.Patients = query.ToDictionary(item => item.Id);
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

            private class Period
            {
                #region Constructors
                private Period()
                {
                }

                public Period(DateTime from, DateTime till, Prefix prefix)
                {
                    this.From = from;
                    this.Till = till;
                    this.Prefix = prefix;
                }
                public Period(string date)
                {
                    if (Enum.TryParse(date[..2], true, out Prefix prefix) == false)
                    {
                        this.SetPeriod(date, Prefix.eq);
                    }
                    else
                    {
                        this.SetPeriod(date[2..], prefix);
                    }
                }
                #endregion

                #region Properties
                public DateTime From { get; private set; }
                public DateTime Till { get; private set; }
                public Prefix Prefix { get; private set; }
                #endregion

                #region Methods
                public bool Contains(DateTime date)
                {
                    switch (this.Prefix)
                    {
                        case Prefix.eq:
                            if (date == this.From && date == this.Till)
                                return true;
                            break;

                        case Prefix.ne:
                            if (date != this.From && date != this.Till)
                                return true;
                            break;

                        case Prefix.gt:
                        case Prefix.lt:
                            if (date > this.From && date < this.Till)
                                return true;
                            break;

                        case Prefix.ge:
                        case Prefix.le:
                            if (date >= this.From && date <= this.Till)
                                return true;
                            break;

                        // TODO: Implement
                        case Prefix.sa:
                            return false;

                        case Prefix.eb:
                            return false;

                        case Prefix.ap:
                            return false;

                        default:
                            throw new ArgumentOutOfRangeException(nameof(this.Prefix));
                    }

                    return false;
                }

                private void SetPeriod(string date, Prefix prefix)
                {
                    DateTime from = DateTime.MinValue;
                    DateTime till = DateTime.MaxValue;

                    if (date.Length == 4)
                    {
                        int year = int.Parse(date);

                        from = new DateTime(year, 01, 01, 00, 00, 00);
                        till = new DateTime(year, 12, 31, 23, 59, 59);
                    }
                    else
                    {
                        if (DateTime.TryParse(date, out DateTime result) == false)
                            return; // TODO: EXEPTION NOT VALID

                        switch (prefix)
                        {
                            case Prefix.eq:
                            case Prefix.ne:
                                from = result;
                                till = result;
                                break;

                            case Prefix.gt:
                            case Prefix.ge:
                                from = result;
                                break;

                            case Prefix.lt:
                            case Prefix.le:
                                till = result;
                                break;

                            // TODO: Implement
                            case Prefix.sa:
                                return;

                            case Prefix.eb:
                                return;

                            case Prefix.ap:
                                return;

                            default:
                                return;
                        }

                    }

                    this.From = from.ToUniversalTime();
                    this.Till = till.ToUniversalTime();
                    this.Prefix = prefix;
                }

                public void Deconstruct(out DateTime from, out DateTime till, out Prefix prefix)
                {
                    from = this.From;
                    till = this.Till;
                    prefix = this.Prefix;
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
                Period period = new Period(date);

                this.Periods.Add(period);

                return Result.Ok;
            }
            public IResult AddRange(IReadOnlyList<string> dates)
            {
                foreach (string date in dates)
                {
                    Period period = new Period(date);

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
                if (this.Periods.Any() == false)
                    return false;

                return true;
            }

            private async Task<IResult> Process()
            {
                ILoading loading = new Loading(this.worker, this.Periods);

                IReadOnlyDictionary<Guid, Patient> patients = await loading.GetPatients(); //this.GetPatients();

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
