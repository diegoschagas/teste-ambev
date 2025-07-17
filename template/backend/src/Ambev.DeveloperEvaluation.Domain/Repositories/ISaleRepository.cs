using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

public interface ISaleRepository
{
    Task<Sale> AddAsync(Sale sale, CancellationToken cancellationToken);
    Task<Sale> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IEnumerable<Sale>> GetAllAsync(CancellationToken cancellationToken);
    Task UpdateAsync(Sale sale, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
    Task<List<Sale>> SearchAsync(string saleNumber, DateTime? date, string customer, decimal? totalAmount, string branch, CancellationToken cancellationToken);

}
