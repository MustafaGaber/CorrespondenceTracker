// Department.cs
using Ardalis.GuardClauses;

namespace CorrespondenceTracker.Domain.Entities
{
    public class Department : Entity
    {
        public string Name { get; private set; }

        // Protected constructor for ORM
        protected Department()
        {
            Name = string.Empty;
        }

        // Public constructor
        public Department(string name)
        {
            Name = Guard.Against.NullOrWhiteSpace(name, nameof(name));
        }

        public void Update(string name)
        {
            Name = Guard.Against.NullOrWhiteSpace(name, nameof(name));
        }
    }
}