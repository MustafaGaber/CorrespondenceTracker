namespace CorrespondenceTracker.Application.Users.Queries.GetUsers
{
    public class GetUsersResponse
    {
        public required Guid Id { get; set; }
        public required string FullName { get; set; }
        public required string? Email { get; set; }
        public required string? JobTitle { get; set; }
    }
}