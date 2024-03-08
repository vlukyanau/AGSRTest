using System;
using System.Collections.Generic;


namespace Api.Models
{
    public interface IHumanName
    {
        public Guid? Id { get; }
        public string Use { get; }
        public string Family { get; }
        public List<string> Given { get; }
    }
}