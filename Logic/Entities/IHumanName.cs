using System;
using System.Collections.Generic;


namespace Logic.Entities
{
    public interface IHumanName : IEntity
    {
        public Guid Id { get; }
        public string Use { get; }
        public string Family { get; }
        public List<string> Given { get; }
    }
}