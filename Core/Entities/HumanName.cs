using System;
using System.Collections.Generic;


namespace Core.Entities
{
    public sealed class HumanName : Entity, IHumanName
    {
        #region Static
        public static HumanName New()
        {
            HumanName name = new HumanName();
            name.Id = Guid.NewGuid();
            name.Given = new List<string>();

            return name;
        }
        #endregion

        #region Constructors
        private HumanName()
        {
        }
        #endregion

        #region Properties
        public Guid Id { get; private set; }
        public string Use { get; set; }
        public string Family { get; set; }
        public List<string> Given { get; set; }
        #endregion
    }
}
