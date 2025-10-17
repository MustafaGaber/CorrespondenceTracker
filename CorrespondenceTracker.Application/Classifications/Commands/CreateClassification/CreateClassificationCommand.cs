using CorrespondenceTracker.Data;

namespace CorrespondenceTracker.Application.Classifications.Commands.CreateClassification
{
    public interface ICreateClassificationCommand
    {
        Task<Guid> Execute(CreateClassificationRequest request);
    }

    public class CreateClassificationCommand : ICreateClassificationCommand
    {
        private readonly CorrespondenceDatabaseContext _context;

        public CreateClassificationCommand(CorrespondenceDatabaseContext context)
        {
            _context = context;
        }

        public async Task<Guid> Execute(CreateClassificationRequest request)
        {
            var classification = new Domain.Entities.Classification(request.Name);

            _context.Classifications.Add(classification);
            await _context.SaveChangesAsync();

            return classification.Id;
        }
    }
}