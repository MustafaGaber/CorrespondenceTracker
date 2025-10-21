namespace CorrespondenceTracker.Application.FollowUps.Commands.UpdateFollowUp
{
    public class UpdateFollowUpRequest
    {
        public Guid? UserId { get; set; }
        public DateOnly Date { get; set; }
        public required string Details { get; set; }
        public Guid? FileRecordId { get; set; }
    }
}