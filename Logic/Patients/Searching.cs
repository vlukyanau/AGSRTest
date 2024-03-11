using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Core;
using Core.Entities;

using Logic.Models;


namespace Logic.Patients
{
    public static partial class Patients
    {
        public sealed class Searching
        {
            #region Constants
            private const string Format = @"(eq|ne|gt|lt|ge|le|sa|eb|ap)?([0-9]([0-9]([0-9][1-9]|[1-9]0)|[1-9]00)|[1-9]000)(-(0[1-9]|1[0-2])(-(0[1-9]|[1-2][0-9]|3[0-1])(T([01][0-9]|2[0-3]):[0-5][0-9]:([0-5][0-9]|60)(\.[0-9]{1,9})?)?)?(Z|(\+|-)((0[0-9]|1[0-3]):[0-5][0-9]|14:00)?)?)?";
            #endregion

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
                    Regex regex = new Regex(Searching.Format);
                    if (regex.IsMatch(date) == false)
                        throw new ArgumentException("Parametr DateTime not valid.");

                    Prefix prefix = (Prefix)Enum.Parse(typeof(Prefix), date[..2], true);

                    if (Enum.IsDefined(typeof(Prefix), prefix) == true)
                        this.SetPeriod(date[2..], prefix);
                    else
                        this.SetPeriod(date, Prefix.eq);
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
                            return date >= this.From && date <= this.Till;

                        case Prefix.ne:
                            return date < this.From || date > this.Till;

                        case Prefix.gt:
                            return date > this.From;

                        case Prefix.lt:
                            return date < this.Till;

                        case Prefix.ge:
                            return date >= this.From;

                        case Prefix.le:
                            return date <= this.Till;

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
                }

                private void SetPeriod(string date, Prefix prefix)
                {
                    if (date.Length == 4)
                    {
                        int year = int.Parse(date);

                        this.From = new DateTime(year, 01, 01, 0, 0, 0, 0);
                        this.Till = this.From.AddYears(1).AddTicks(-1);
                        this.Prefix = prefix;

                        return;
                    }

                    if (date.Length == 7)
                    {
                        this.From = DateTime.Parse(date);
                        this.Till = this.From.AddMonths(1).AddTicks(-1);
                        this.Prefix = prefix;

                        return;
                    }

                    if (date.Length == 10)
                    {
                        this.From = DateTime.Parse(date);
                        this.Till = this.From.AddDays(1d).AddTicks(-1);
                        this.Prefix = prefix;

                        return;
                    }

                    DateTime from = DateTime.MinValue;
                    DateTime till = DateTime.MaxValue;

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
