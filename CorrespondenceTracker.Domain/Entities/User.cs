// User.cs
using Ardalis.GuardClauses;

namespace CorrespondenceTracker.Domain.Entities
{
    public class User : Entity
    {
        public string FullName { get; private set; }
        public string? Email { get; private set; }
        public string? JobTitle { get; private set; }
        public bool IsFollowUpUser { get; private set; }
        public bool IsFollowUpManager { get; private set; }

        //private readonly List<User> _sser = new();
        //public virtual IReadOnlyList<User> Users => _followUps.ToList();


        // Protected constructor for ORM
        protected User()
        {
        }

        // Public constructor
        public User(string fullName, string? email = null, string? jobTitle = null)
        {
            FullName = Guard.Against.NullOrWhiteSpace(fullName, nameof(fullName));
            Email = email;
            JobTitle = jobTitle;
        }

        public void Update(string fullName, string? email = null, string? jobTitle = null)
        {
            FullName = Guard.Against.NullOrWhiteSpace(fullName, nameof(fullName));
            Email = email;
            JobTitle = jobTitle;
        }
    }
}