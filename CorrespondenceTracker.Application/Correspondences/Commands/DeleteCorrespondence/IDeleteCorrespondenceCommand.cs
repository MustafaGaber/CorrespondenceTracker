namespace CorrespondenceTracker.Application.Correspondences.Commands.DeleteCorrespondence
{
    public interface IDeleteCorrespondenceCommand
    {
        Task Execute(Guid id);
    }
}
