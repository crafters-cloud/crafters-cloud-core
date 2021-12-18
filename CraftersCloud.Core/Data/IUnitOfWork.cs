namespace CraftersCloud.Core.Data;

public interface IUnitOfWork
{
    void SaveChanges();
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
    void CancelSaving();
}