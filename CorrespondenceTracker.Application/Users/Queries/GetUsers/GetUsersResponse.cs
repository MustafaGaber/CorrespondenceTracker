namespace CorrespondenceTracker.Application.Users.Queries.GetUsers
{
    public class GetUsersResponse
    {
        public Guid Id { get; set; }
        public required string FullName { get; set; }
        public string? JobTitle { get; set; }
    }
}