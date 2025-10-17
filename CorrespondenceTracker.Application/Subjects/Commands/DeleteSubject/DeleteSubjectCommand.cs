using CorrespondenceTracker.Data;

namespace CorrespondenceTracker.Application.Subjects.Commands.DeleteSubject
{
    public interface IDeleteSubjectCommand
    {
        Task Execute(Guid id);
    }

    public class DeleteSubjectCommand : IDeleteSubjectCommand
    {
        private readonly CorrespondenceDatabaseContext _context;

        public DeleteSubjectCommand(CorrespondenceDatabaseContext context)
        {
            _context = context;
        }

        public async Task Execute(Guid id)
        {
            var subject = await _context.Subjects.FindAsync(id)
                ?? throw new ArgumentException($"Subject with ID {id} not found");

            _context.Subjects.Remove(subject);
            await _context.SaveChangesAsync();
        }
    }
}