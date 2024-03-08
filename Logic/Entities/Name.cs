using System;
using System.Collections.Generic;


namespace Logic.Entities
{
    public sealed class Name : IName
    {
        #region Static
        public static Name New(string family)
        {
            Name name = new Name();
            name.Id = Guid.NewGuid();
            name.Family = family;

            return name;
        }
        #endregion

        #region Constructors
        private Name()
        {
        }
        #endregion

        #region Properties
        public Guid Id { get; private set; }
        public string Use { get; set; }
        public string Family { get; set; }
        public IList<string> Given { get; set; }
        #endregion
    }
}
