using CorrespondenceTracker.Data;

namespace CorrespondenceTracker.Application.Subjects.Commands.CreateSubject
{
    public interface ICreateSubjectCommand
    {
        Task<Guid> Execute(CreateSubjectRequest request);
    }

    public class CreateSubjectCommand : ICreateSubjectCommand
    {
        private readonly CorrespondenceDatabaseContext _context;

        public CreateSubjectCommand(CorrespondenceDatabaseContext context)
        {
            _context = context;
        }

        public async Task<Guid> Execute(CreateSubjectRequest request)
        {
            var subject = new Domain.Entities.Subject(request.Name);

            _context.Subjects.Add(subject);
            await _context.SaveChangesAsync();

            return subject.Id;
        }
    }
}