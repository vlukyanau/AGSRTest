using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using Logic.Entities;


namespace Logic.Patients
{
    public static partial class Patients
    {
        public sealed class Retrieving
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
                sa,       // StartsAfter
                eb,       // EndsBefore
                ap        // Approximate
            }
            #endregion

            #region Static
            public static Retrieving New()
            {
                return new Retrieving();
            }
            #endregion

            #region Classes
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
            public async Task<IResult> Go(IReadOnlyList<string> dates)
            {
                try
                {
                    IResult result = await this.Process(dates);

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
                using (ApplicationContext context = new ApplicationContext())
                {
                    IReadOnlyList<Patient> patients = await context.Patients.Include(patient => patient.Name).ToListAsync();

                    return Result.New(patients);
                }
            }
            private async Task<IResult> Process(Guid id)
            {
                using (ApplicationContext context = new ApplicationContext())
                {
                    Patient patient = await context.Patients.Include(patient => patient.Name).SingleAsync(item => item.Id == id);
                    if (patient == null)
                        return Result.NotFound;

                    return Result.New(patient);
                }
            }
            private async Task<IResult> Process(IReadOnlyList<string> dates)
            {
                if (dates.Count == 0 || dates.Count > 2)
                    return Result.BadRequest;

                using (ApplicationContext context = new ApplicationContext())
                {
                    IQueryable<Patient> query = context.Patients.Include(patient => patient.Name);

                    foreach (string date in dates)
                    {
                        query = this.FilterDate(query, date);
                        if (query == null)
                            return Result.BadRequest;
                    }

                    IReadOnlyList<Patient> patients = await query.ToListAsync();

                    return Result.New(patients);
                }
            }

            private IQueryable<Patient> FilterDate(IQueryable<Patient> query, string date)
            {
                Tuple tuple = Tuple.New(date);
                if (tuple == null)
                    return null;

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

                    case Prefix.sa:
                        break;

                    case Prefix.eb:
                        break;

                    case Prefix.ap:
                        break;

                    default:
                        return null;
                }

                return query;
            }
            #endregion
        }
    }
}
