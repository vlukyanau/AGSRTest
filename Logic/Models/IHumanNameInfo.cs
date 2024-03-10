using System;
using System.Collections.Generic;


namespace Logic.Models
{
    public interface IHumanNameInfo : IEntry
    {
        public Guid? Id { get; }
        public string Use { get; }
        public string Family { get; }
        public List<string> Given { get; }
    }
}