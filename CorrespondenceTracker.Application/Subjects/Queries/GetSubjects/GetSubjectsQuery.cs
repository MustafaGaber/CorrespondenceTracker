using CorrespondenceTracker.Data;
using Microsoft.EntityFrameworkCore;

namespace CorrespondenceTracker.Application.Subjects.Queries.GetSubjects
{
    public interface IGetSubjectsQuery
    {
        Task<IEnumerable<GetSubjectResponse>> Execute();
    }

    public class GetSubjectsQuery : IGetSubjectsQuery
    {
        private readonly CorrespondenceDatabaseContext _context;

        public GetSubjectsQuery(CorrespondenceDatabaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<GetSubjectResponse>> Execute()
        {
            var subjects = await _context.Subjects
                .OrderBy(s => s.Name)
                .ToListAsync();

            return subjects.Select(s => new GetSubjectResponse
            {
                Id = s.Id,
                Name = s.Name
            });
        }
    }
}