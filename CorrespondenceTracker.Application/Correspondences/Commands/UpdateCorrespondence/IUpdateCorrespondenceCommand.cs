namespace CorrespondenceTracker.Application.Correspondences.Commands.UpdateCorrespondence
{
    public interface IUpdateCorrespondenceCommand
    {
        Task Execute(Guid id, CreateCorrespondenceRequest model);
    }
}
