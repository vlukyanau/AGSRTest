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
                // TODO: Implement
                Prefix.sa => false,
                Prefix.eb => false,
                Prefix.ap => false,
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
    }
}
