using Ardalis.GuardClauses;

namespace CorrespondenceTracker.Domain.Entities
{
    public class Correspondent : Entity
    {
        public string Name { get; private set; } = string.Empty;
        public string? Address { get; private set; }
        public CorrespondentType Type { get; private set; }
        // Protected constructor for ORM
        protected Correspondent() { }

        // Public constructor
        public Correspondent(string name, string? address, CorrespondentType type)
        {
            Name = Guard.Against.NullOrWhiteSpace(name);
            Address = address;
            Type = type;
        }

        public void Update(string name, string? address, CorrespondentType type)
        {
            Name = Guard.Against.NullOrWhiteSpace(name);
            Address = address;
            Type = type;
        }
    }

    public enum CorrespondentType
    {
        InternalDepartment = 1,
        GovernmentEntity = 2,
        Company = 3,
        Citizen = 4,
        Organization = 5,
        Media = 6,
        NonProfit = 20,
        ForeignEntity = 21,
        Other = 100
    }
}