// file: CorrespondenceTracker.Application/Correspondents/Commands/UpdateCorrespondent/UpdateCorrespondentCommand.cs
using CorrespondenceTracker.Application.Correspondents.Commands.CreateCorrespondent;
using CorrespondenceTracker.Data;

namespace CorrespondenceTracker.Application.Correspondents.Commands.UpdateCorrespondent
{
    public class UpdateCorrespondentCommand : IUpdateCorrespondentCommand
    {
        private readonly CorrespondenceDatabaseContext _context;

        public UpdateCorrespondentCommand(CorrespondenceDatabaseContext context)
        {
            _context = context;
        }

        public async Task Execute(Guid id, CreateCorrespondentRequest model)
        {
            var correspondent = await _context.Correspondents.FindAsync(id)
                ?? throw new ArgumentException($"Correspondent with ID {id} not found");

            correspondent.Update(model.Name, model.Address);
            await _context.SaveChangesAsync();
        }
    }

    public interface IUpdateCorrespondentCommand
    {
        Task Execute(Guid id, CreateCorrespondentRequest model);
    }
}