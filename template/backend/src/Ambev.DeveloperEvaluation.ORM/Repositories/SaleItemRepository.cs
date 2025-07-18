using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

public class SaleItemRepository : ISaleItemRepository
{
    private readonly DefaultContext _context;

    public SaleItemRepository(DefaultContext context)
    {
        _context = context;
    }

    public async Task<bool> RemoveAsync(Guid id, CancellationToken cancellationToken)
    {
        var saleItem = await GetByIdAsync(id, cancellationToken);
        if (saleItem == null)
            return false;

        _context.SaleItems.Remove(saleItem);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<SaleItem> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        //var existingTracked = _context.ChangeTracker.Entries<SaleItem>().FirstOrDefault(e => e.Entity.Id == id);

        //if (existingTracked != null)
        //{
        //    existingTracked.State = EntityState.Detached;
        //}


        return await _context.SaleItems.FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }

    public async Task AddRangeAsync(IEnumerable<SaleItem> items, CancellationToken cancellationToken)
    {
        _context.SaleItems.AddRange(items);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveRangeAsync(IEnumerable<SaleItem> items, CancellationToken cancellationToken)
    {
        _context.SaleItems.RemoveRange(items);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
