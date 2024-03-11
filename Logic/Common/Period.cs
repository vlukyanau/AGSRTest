using System;


namespace Logic.Common
{
    public enum Prefix
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

    internal sealed class Period
    {
        #region Constants
        private const double Approximate = 0.1; // 10%
        #endregion

        #region Operators
        public static implicit operator Period((DateTime From, DateTime Till, Prefix prefix) period)
        {
            return new Period(period.From, period.Till, period.prefix);
        }
        #endregion

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
        #endregion

        #region Properties
        public DateTime From { get; }
        public DateTime Till { get; }
        public Prefix Prefix { get; }
        #endregion

        #region Methods
        public bool Contains(DateTime date)
        {
            return this.Prefix switch
            {
                Prefix.eq => date >= this.From && date <= this.Till,
                Prefix.ne => date < this.From || date > this.Till,
                Prefix.gt => date > this.From,
                Prefix.lt => date < this.Till,
                Prefix.ge => date >= this.From,
                Prefix.le => date <= this.Till,
                Prefix.sa => date > this.Till,
                Prefix.eb => date < this.From,
                Prefix.ap => date >= this.From.AddTicks(-this.GetApproximateTicks(this.From)) && date <= this.Till.AddTicks(this.GetApproximateTicks(date)),
                _ => throw new ArgumentOutOfRangeException(nameof(this.Prefix)),
            };
        }

        public void Deconstruct(out DateTime from, out DateTime till, out Prefix prefix)
        {
            from = this.From;
            till = this.Till;
            prefix = this.Prefix;
        }
        #endregion

        private long GetApproximateTicks(DateTime date)
        {
            long ticks = DateTime.UtcNow.Ticks - date.Ticks;

            return Convert.ToInt64(Math.Abs(ticks) * Period.Approximate);
        }
    }
}
