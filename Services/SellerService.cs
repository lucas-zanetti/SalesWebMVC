using SalesWebMVC.Data;
using SalesWebMVC.Models;
using System.Collections.Generic;
using System.Linq;

namespace SalesWebMVC.Services
{
    public class SellerService
    {
        private readonly SalesWebMVCContext _context;

        public SellerService(SalesWebMVCContext context)
        {
            _context = context;
        }

        public List<Seller> FindAll()
        {
            return _context.Seller.ToList();
        }

        public Seller FindById(int sellerId)
        {
            return _context.Seller.FirstOrDefault(seller => seller.Id == sellerId);
        }

        public void AddSeller(Seller seller)
        {
            _context.Seller.Add(seller);

            _context.SaveChanges();
        }

        public void DeleteSeller(int id)
        {
            _context.Seller.Remove(_context.Seller.Find(id));

            _context.SaveChanges();
        }
    }
}
