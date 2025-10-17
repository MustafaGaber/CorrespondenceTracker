using CorrespondenceTracker.Data;

namespace CorrespondenceTracker.Application.Subjects.Commands.UpdateSubject
{
    public interface IUpdateSubjectCommand
    {
        Task Execute(Guid id, UpdateSubjectRequest request);
    }

    public class UpdateSubjectCommand : IUpdateSubjectCommand
    {
        private readonly CorrespondenceDatabaseContext _context;

        public UpdateSubjectCommand(CorrespondenceDatabaseContext context)
        {
            _context = context;
        }

        public async Task Execute(Guid id, UpdateSubjectRequest request)
        {
            var subject = await _context.Subjects.FindAsync(id)
                ?? throw new ArgumentException($"Subject with ID {id} not found");

            subject.Update(request.Name);
            await _context.SaveChangesAsync();
        }
    }
}