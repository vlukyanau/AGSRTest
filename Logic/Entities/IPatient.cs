using System;


namespace Logic.Entities
{
    public interface IPatient
    {
        public Name Name { get; }
        public Gender Gender { get; }
        public DateTime BirthDate { get; }
        public bool Active { get; }
    }
}
