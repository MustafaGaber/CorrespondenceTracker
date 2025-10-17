// file: CorrespondenceTracker.Application/Correspondents/Commands/CreateCorrespondent/CreateCorrespondentCommand.cs
using CorrespondenceTracker.Data;
using CorrespondenceTracker.Domain.Entities;

namespace CorrespondenceTracker.Application.Correspondents.Commands.CreateCorrespondent
{
    public class CreateCorrespondentCommand : ICreateCorrespondentCommand
    {
        private readonly CorrespondenceDatabaseContext _context;

        public CreateCorrespondentCommand(CorrespondenceDatabaseContext context)
        {
            _context = context;
        }

        public async Task<Guid> Execute(CreateCorrespondentRequest model)
        {
            var correspondent = new Correspondent(model.Name, model.Address);
            _context.Correspondents.Add(correspondent);
            await _context.SaveChangesAsync();
            return correspondent.Id;
        }
    }

    public interface ICreateCorrespondentCommand
    {
        Task<Guid> Execute(CreateCorrespondentRequest model);
    }
}