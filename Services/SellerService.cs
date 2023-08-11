using Microsoft.EntityFrameworkCore;
using SalesWebMVC.Data;
using SalesWebMVC.Models;
using SalesWebMVC.Services.Exceptions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesWebMVC.Services
{
    public class SellerService
    {
        private readonly SalesWebMVCContext _context;

        public SellerService(SalesWebMVCContext context)
        {
            _context = context;
        }

        public async Task<List<Seller>> FindAllAsync()
        {
            return await _context.Seller.ToListAsync();
        }

        public async Task<Seller> FindByIdAsync(int sellerId)
        {
            return await _context.Seller.Include(seller => seller.Department).FirstOrDefaultAsync(seller => seller.Id == sellerId);
        }

        public async Task AddSellerAsync(Seller seller)
        {
            _context.Seller.Add(seller);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteSellerAsync(int id)
        {
            try
            {
                var seller = await _context.Seller.FindAsync(id);

                _context.Seller.Remove(seller);

                await _context.SaveChangesAsync();
            }
            catch(DbUpdateException)
            {
                throw new IntegrityException("Cannot delete seller cause this has sales");
            }
        }

        public async Task UpdateSellerAsync(Seller seller)
        {
            bool hasAnySeller = await _context.Seller.AnyAsync(x => x.Id == seller.Id);
            
            if (!hasAnySeller)
                throw new NotFoundException("Seller Id not found on database!");

            try
            {
                _context.Update(seller);
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException e)
            {
                throw new DbConcurrencyException(e.Message);
            }
        }
    }
}
