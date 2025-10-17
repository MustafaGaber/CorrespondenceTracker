using Ardalis.GuardClauses;

namespace CorrespondenceTracker.Domain.Entities
{
    public class Subject : Entity
    {
        public string Name { get; private set; }

        public Subject(string name)
        {
            Name = Guard.Against.NullOrEmpty(name);
        }

        public void Update(string name)
        {
            Name = Guard.Against.NullOrEmpty(name);
        }

    }
}
