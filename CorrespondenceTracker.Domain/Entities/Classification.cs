// Classification.cs
using Ardalis.GuardClauses;

namespace CorrespondenceTracker.Domain.Entities
{
    public class Classification : Entity
    {
        public string Name { get; private set; }

        private readonly List<Correspondence> _correspondences = new();
        public virtual IReadOnlyList<Correspondence> Correspondences => _correspondences.ToList();

        // Protected constructor for ORM
        protected Classification()
        {
        }

        // Public constructor
        public Classification(string name)
        {
            Name = Guard.Against.NullOrWhiteSpace(name, nameof(name));
        }

        public void Update(string name)
        {
            Name = Guard.Against.NullOrWhiteSpace(name, nameof(name));
        }
    }
}