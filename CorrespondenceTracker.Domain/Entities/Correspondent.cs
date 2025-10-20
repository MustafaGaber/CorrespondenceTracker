using Ardalis.GuardClauses;

namespace CorrespondenceTracker.Domain.Entities
{
    public class Correspondent : Entity
    {
        public string Name { get; private set; } = string.Empty;
        public string? Address { get; private set; }

        // Protected constructor for ORM
        protected Correspondent() { }

        // Public constructor
        public Correspondent(string name, string? address = null)
        {
            Name = Guard.Against.NullOrWhiteSpace(name, nameof(name));
            Address = address;
        }

        public void Update(string name, string? address = null)
        {
            Name = Guard.Against.NullOrWhiteSpace(name, nameof(name));
            Address = address;
        }
    }
}