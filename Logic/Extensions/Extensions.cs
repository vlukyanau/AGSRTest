using System;
using System.Text.RegularExpressions;

using Logic.Common;


namespace Logic.Extensions
{
    internal static class Extensions
    {
        public static Period GetPeriod(this string date)
        {
            const string format = @"(eq|ne|gt|lt|ge|le|sa|eb|ap)?([0-9]([0-9]([0-9][1-9]|[1-9]0)|[1-9]00)|[1-9]000)(-(0[1-9]|1[0-2])(-(0[1-9]|[1-2][0-9]|3[0-1])(T([01][0-9]|2[0-3]):[0-5][0-9]:([0-5][0-9]|60)(\.[0-9]{1,9})?)?)?(Z|(\+|-)((0[0-9]|1[0-3]):[0-5][0-9]|14:00)?)?)?";

            Console.WriteLine("------------------------");
            Console.WriteLine(date);
            Console.WriteLine("------------------------");

            Regex regex = new Regex(format);
            if (regex.IsMatch(date) == false)
                throw new ArgumentException("Parametr DateTime not valid.");

            Prefix prefix = (Prefix)Enum.Parse(typeof(Prefix), date[..2], true);

            if (Enum.IsDefined(typeof(Prefix), prefix) == true)
                date = date[2..];
            else
                prefix = Prefix.eq;

            DateTime from = DateTime.MinValue;
            DateTime till = DateTime.MaxValue;

            if (date.Length == 4)
            {
                int year = int.Parse(date);

                from = new DateTime(year, 01, 01, 0, 0, 0, 0);
                till = from.AddYears(1).AddTicks(-1);

                return (from, till, prefix);
            }

            if (date.Length == 7)
            {
                from = DateTime.Parse(date);
                till = from.AddMonths(1).AddTicks(-1);

                return (from, till, prefix);
            }

            if (date.Length == 10)
            {
                from = DateTime.Parse(date);
                till = from.AddDays(1d).AddTicks(-1);

                return (from, till, prefix);
            }

            if (DateTime.TryParse(date, out DateTime result) == false)
                throw new ArgumentException("Parametr DateTime not valid."); // TODO: EXEPTION NOT VALID

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
                    throw new NotSupportedException();

                case Prefix.eb:
                    throw new NotSupportedException();

                case Prefix.ap:
                    throw new NotSupportedException();

                default:
                    throw new NotSupportedException();
            }

            return (from, till, prefix);
        }
    }
}
