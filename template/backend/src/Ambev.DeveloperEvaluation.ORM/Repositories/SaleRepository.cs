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


    public async Task UpdateAsync(Sale updatedSale, CancellationToken cancellationToken)
    {
        // Start a transaction
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var existingSale = await _context.Sales
                .Include(s => s.Items)
                .FirstOrDefaultAsync(s => s.Id == updatedSale.Id, cancellationToken);

            if (existingSale == null)
                throw new Exception("Sale not found");

            // Update Sale fields
            existingSale.Date = updatedSale.Date;
            existingSale.Customer = updatedSale.Customer;
            existingSale.Branch = updatedSale.Branch;
            existingSale.Status = updatedSale.Status;

            // Remove items that no longer exist in updated list
            var updatedItemIds = updatedSale.Items.Select(i => i.Id).ToList();

            var itemsToRemove = existingSale.Items
                .Where(existingItem => !updatedItemIds.Contains(existingItem.Id))
                .ToList();

            _context.SaleItems.RemoveRange(itemsToRemove);

            // Update existing items or add new items
            foreach (var updatedItem in updatedSale.Items)
            {
                var existingItem = existingSale.Items.FirstOrDefault(i => i.Id == updatedItem.Id);

                if (existingItem != null)
                {
                    // Update existing item
                    existingItem.Product = updatedItem.Product;
                    existingItem.Quantity = updatedItem.Quantity;
                    existingItem.UnitPrice = updatedItem.UnitPrice;
                    existingItem.Discount = updatedItem.Discount;
                    existingItem.Total = (updatedItem.Quantity * updatedItem.UnitPrice) - updatedItem.Discount;
                    existingItem.Status = updatedItem.Status;
                }
                else
                {
                    // Add new item
                    var newItem = new SaleItem
                    {
                        Product = updatedItem.Product,
                        Quantity = updatedItem.Quantity,
                        UnitPrice = updatedItem.UnitPrice,
                        Discount = updatedItem.Discount,
                        Total = (updatedItem.Quantity * updatedItem.UnitPrice) - updatedItem.Discount,
                        Status = updatedItem.Status
                    };

                    existingSale.Items.Add(newItem);
                }
            }

            // Recalculate Sale total
            existingSale.TotalAmount = existingSale.Items.Sum(i => i.Total);

            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }


}
