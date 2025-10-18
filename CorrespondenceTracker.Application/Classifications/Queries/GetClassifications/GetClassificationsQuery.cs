using CorrespondenceTracker.Data;
using Microsoft.EntityFrameworkCore;

namespace CorrespondenceTracker.Application.Classifications.Queries.GetClassifications
{
    public interface IGetClassificationsQuery
    {
        Task<IEnumerable<GetClassificationResponse>> Execute();
    }

    public class GetClassificationsQuery : IGetClassificationsQuery
    {
        private readonly CorrespondenceDatabaseContext _context;

        public GetClassificationsQuery(CorrespondenceDatabaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<GetClassificationResponse>> Execute()
        {
            var classifications = await _context.Classifications.ToListAsync();

            return classifications.Select(c => new GetClassificationResponse
            {
                Id = c.Id,
                Name = c.Name
            });
        }
    }
}