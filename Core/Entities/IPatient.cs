using System;


namespace Core.Entities
{
    public interface IPatient
    {
        public Guid Id { get; }
        public Guid HumanNameId { get; }
        public Gender? Gender { get; }
        public DateTime BirthDate { get; }
        public bool? Active { get; }
    }
}
