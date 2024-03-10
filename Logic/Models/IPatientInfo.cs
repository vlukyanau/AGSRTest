using System;

using Core.Entities;


namespace Logic.Models
{
    public interface IPatientInfo : IEntry
    {
        public Guid? Id { get; }
        public HumanNameInfo Name { get; }
        public Gender? Gender { get; }
        public DateTime? BirthDate { get; }
        public bool? Active { get; }
    }
}
