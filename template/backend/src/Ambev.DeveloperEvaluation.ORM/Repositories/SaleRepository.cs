using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

public class SaleRepository : ISaleRepository
{
    private readonly DefaultContext _context;

    public SaleRepository(DefaultContext context)
    {
        _context = context;
    }

    public async Task<Sale> AddAsync(Sale sale, CancellationToken cancellationToken)
    {
        await _context.Sales.AddAsync(sale, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return sale;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var sale = await GetByIdAsync(id, cancellationToken);
        if (sale == null)
            return false;

        _context.Sales.Remove(sale);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<IEnumerable<Sale>> GetAllAsync(CancellationToken cancellationToken)
    {
        var sales = await _context.Sales
            .Include(s => s.Items)
            .ToListAsync(cancellationToken);
        return sales ?? Enumerable.Empty<Sale>();
    }

    public async Task<Sale> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var sale = await _context.Sales
            .AsNoTracking()
            .Include(s => s.Items)
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);

        if (sale == null)
            throw new InvalidOperationException($"Sale with id '{id}' not found.");

        return sale;
    }

    public async Task<List<Sale>> SearchAsync(
        string saleNumber,
        DateTime? date,
        string customer,
        decimal? totalAmount,
        string branch,
        CancellationToken cancellationToken)
    {
        var query = _context.Sales
            .Include(s => s.Items)
            .AsQueryable();


        if (!string.IsNullOrEmpty(saleNumber))
            query = query.Where(s => s.SaleNumber == saleNumber);

        if (date.HasValue)
            query = query.Where(s => s.Date.Date == date.Value.Date);

        if (!string.IsNullOrEmpty(customer))
            query = query.Where(s => s.Customer.Contains(customer));

        if (totalAmount.HasValue)
            query = query.Where(s => s.TotalAmount == totalAmount.Value);

        if (!string.IsNullOrEmpty(branch))
            query = query.Where(s => s.Branch == branch);

        return await query.ToListAsync(cancellationToken);
    }

    public async Task UpdateAsync(Sale sale, CancellationToken cancellationToken)
    {
        if (sale == null)
            throw new ArgumentNullException(nameof(sale));

        // Attach the parent Sale entity as modified
        _context.Attach(sale);
        _context.Entry(sale).State = EntityState.Modified;

        // Load existing SaleItems from DB for this Sale
        var existingItems = await _context.SaleItems
            .Where(i => i.Id == sale.Id)
            .ToListAsync(cancellationToken);

        var incomingItems = sale.Items ?? new List<SaleItem>();
        var incomingItemIds = incomingItems.Where(i => i.Id != Guid.Empty).Select(i => i.Id).ToHashSet();

        // Process each incoming item
        foreach (var incomingItem in incomingItems)
        {
            if (incomingItem.Id == Guid.Empty)
            {
                // New item
                incomingItem.Id = Guid.NewGuid();
                incomingItem.Id = sale.Id;
                _context.Add(incomingItem);
            }
            else
            {
                // Existing item - attach and mark as modified
                var existingItem = existingItems.FirstOrDefault(ei => ei.Id == incomingItem.Id);
                if (existingItem != null)
                {
                    _context.Entry(existingItem).CurrentValues.SetValues(incomingItem);
                }
                else
                {
                    // Defensive: handle scenario where incoming references an item not found in DB
                    incomingItem.Id = sale.Id;
                    _context.Add(incomingItem);
                }
            }
        }

        // Identify and remove deleted items
        var itemsToRemove = existingItems.Where(ei => !incomingItemIds.Contains(ei.Id)).ToList();
        if (itemsToRemove.Any())
        {
            _context.RemoveRange(itemsToRemove);
        }

        await _context.SaveChangesAsync(cancellationToken);
    }


}
