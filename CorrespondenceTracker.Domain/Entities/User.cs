// User.cs
using Ardalis.GuardClauses;

namespace CorrespondenceTracker.Domain.Entities
{
    public class User : Entity
    {
        public string FullName { get; private set; }
        public string? Email { get; private set; }
        public string? JobTitle { get; private set; }

        // Protected constructor for ORM
        protected User()
        {
        }

        // Public constructor
        public User(string fullName, string? jobTitle = null)
        {
            FullName = Guard.Against.NullOrWhiteSpace(fullName, nameof(fullName));
            JobTitle = jobTitle;
        }

        public void Update(string fullName, string? jobTitle = null)
        {
            FullName = Guard.Against.NullOrWhiteSpace(fullName, nameof(fullName));
            JobTitle = jobTitle;
        }
    }
}