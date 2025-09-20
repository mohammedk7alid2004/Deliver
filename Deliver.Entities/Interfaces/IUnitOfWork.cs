using Deliver.Entities.Entities;
namespace Deliver.Entities.Interfaces;
public interface IUnitOfWork:IDisposable
{
    public IRepository<ApplicationUser> Users { get; }
  
    Task<int> SaveAsync(CancellationToken cancellationToken = default);
    Task<TResult> ExecuteInTransactionAsync<TResult>(Func<Task<TResult>> action);
}
