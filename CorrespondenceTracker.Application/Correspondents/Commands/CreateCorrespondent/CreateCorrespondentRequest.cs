// file: CorrespondenceTracker.Application/Correspondents/Commands/CreateCorrespondent/CreateCorrespondentRequest.cs
using CorrespondenceTracker.Domain.Entities;

namespace CorrespondenceTracker.Application.Correspondents.Commands.CreateCorrespondent
{
    public class CreateCorrespondentRequest
    {
        public string Name { get; set; } = string.Empty;
        public string? Address { get; set; }
        public CorrespondentType Type { get; set; }
    }
}