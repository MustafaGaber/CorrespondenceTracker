using CorrespondenceTracker.Data;
using Microsoft.EntityFrameworkCore;

namespace CorrespondenceTracker.Application.Classifications.Queries.GetClassifications
{
    public interface IGetClassificationsQuery
    {
        Task<IEnumerable<GetClassificationResponse>> Execute(GetClassificationsFilterModel filter);
    }

    public class GetClassificationsQuery : IGetClassificationsQuery
    {
        private readonly CorrespondenceDatabaseContext _context;

        public GetClassificationsQuery(CorrespondenceDatabaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<GetClassificationResponse>> Execute(GetClassificationsFilterModel filter)
        {
            var query = _context.Classifications.AsQueryable();

            if (!string.IsNullOrEmpty(filter.SearchTerm))
            {
                query = query.Where(c => c.Name.Contains(filter.SearchTerm));
            }

            var classifications = await query
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            return classifications.Select(c => new GetClassificationResponse
            {
                Id = c.Id,
                Name = c.Name
            });
        }
    }
}