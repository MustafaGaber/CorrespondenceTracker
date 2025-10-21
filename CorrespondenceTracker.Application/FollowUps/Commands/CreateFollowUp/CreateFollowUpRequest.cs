namespace CorrespondenceTracker.Application.FollowUps.Commands.CreateFollowUp
{
    public class CreateFollowUpRequest
    {
        public Guid? UserId { get; set; }
        public DateOnly Date { get; set; }
        public required string Details { get; set; }
        public Guid? FileRecordId { get; set; }
    }
}