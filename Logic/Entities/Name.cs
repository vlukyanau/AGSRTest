using System;
using System.Collections.Generic;


namespace Logic.Entities
{
    public sealed class Name : IName
    {
        #region Static
        public static Name New()
        {
            Name name = new Name();
            name.Id = Guid.NewGuid();

            return name;
        }
        #endregion

        #region Constructors
        private Name()
        {
            this.Use = string.Empty;
            this.Family = string.Empty;
            this.Given = new List<string>();
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
