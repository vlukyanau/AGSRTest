using System;


namespace Logic.Entities
{
    public interface IPatient
    {
        public Guid Id { get; }
        public HumanName Name { get; }
        public Gender? Gender { get; }
        public DateTime BirthDate { get; }
        public bool? Active { get; }
    }
}
