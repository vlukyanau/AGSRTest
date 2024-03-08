using System;
using System.Collections.Generic;


namespace Logic.Entities
{
    public interface IName
    {
        public Guid Id { get; }
        public string Use { get; }
        public string Family { get; }
        public IList<string> Given { get; }
    }
}