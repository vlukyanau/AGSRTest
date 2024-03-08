using System;
using System.Collections.Generic;


namespace Api.Models
{
    public sealed class HumanName : IHumanName
    {
        #region Properties
        public Guid? Id { get; set; }

        public string Use { get; set; }

        public string Family { get; set; }

        public List<string> Given { get; set; }
        #endregion
    }
}
