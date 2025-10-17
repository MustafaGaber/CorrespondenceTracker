// file: CorrespondenceTracker.Application/Correspondents/Queries/GetCorrespondents/GetCorrespondentsQuery.cs
using CorrespondenceTracker.Data;
using Microsoft.EntityFrameworkCore;

namespace CorrespondenceTracker.Application.Correspondents.Queries.GetCorrespondents
{
    public class GetCorrespondentsQuery : IGetCorrespondentsQuery
    {
        private readonly CorrespondenceDatabaseContext _context;

        public GetCorrespondentsQuery(CorrespondenceDatabaseContext context)
        {
            _context = context;
        }

        public async Task<List<GetCorrespondentResponse>> Execute(GetCorrespondentsFilterModel? filter)
        {
            filter ??= new GetCorrespondentsFilterModel();
            var query = _context.Correspondents.AsQueryable();
            if (!string.IsNullOrWhiteSpace(filter.Name))
            {
                var searchTerm = filter.Name.Trim().ToLower();
                query = query.Where(c =>
                    c.Name.ToLower().Contains(searchTerm) ||
                    (c.Address != null && c.Address.ToLower().Contains(searchTerm)));
            }

            return await query
                .OrderBy(c => c.Name)
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(c => new GetCorrespondentResponse
                {
                    Id = c.Id,
                    Name = c.Name,
                    Address = c.Address,
                })
                .ToListAsync();
        }
    }

    public interface IGetCorrespondentsQuery
    {
        Task<List<GetCorrespondentResponse>> Execute(GetCorrespondentsFilterModel? filter = null);
    }
}