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

        public async Task<List<CorrespondentDto>> Execute(string? search = null)
        {
            var query = _context.Correspondents.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim().ToLower();
                query = query.Where(c => c.Name.ToLower().Contains(search) ||
                                         (c.Address != null && c.Address.ToLower().Contains(search)));
            }

            return await query
                .OrderBy(c => c.Name)
                .Select(c => new CorrespondentDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Address = c.Address
                })
                .ToListAsync();
        }
    }

    public interface IGetCorrespondentsQuery
    {
        Task<List<CorrespondentDto>> Execute(string? search = null);
    }

    public class CorrespondentDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Address { get; set; }
    }
}
