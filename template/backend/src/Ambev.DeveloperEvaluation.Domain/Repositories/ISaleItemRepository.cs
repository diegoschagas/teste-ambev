using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

public interface ISaleItemRepository
{
    Task<bool> RemoveAsync(Guid id, CancellationToken cancellationToken);
    Task<SaleItem> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task AddRangeAsync(IEnumerable<SaleItem> items, CancellationToken cancellationToken);
    Task RemoveRangeAsync(IEnumerable<SaleItem> items, CancellationToken cancellationToken);


}
