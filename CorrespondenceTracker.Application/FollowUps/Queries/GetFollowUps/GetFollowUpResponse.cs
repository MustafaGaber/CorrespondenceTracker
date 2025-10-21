namespace CorrespondenceTracker.Application.FollowUps.Queries.GetFollowUps
{
    public class GetFollowUpResponse
    {
        public Guid? Id { get; set; }
        public Guid? CorrespondenceId { get; set; }
        public Guid? UserId { get; set; }
        public string? UserName { get; set; }
        public DateOnly Date { get; set; }
        public string Details { get; set; }
        public Guid? FileRecordId { get; set; }
        public string? FileName { get; set; }
    }
}