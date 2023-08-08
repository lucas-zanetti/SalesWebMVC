using SalesWebMVC.Models;
using SalesWebMVC.Models.Enums;
using System;
using System.Linq;

namespace SalesWebMVC.Data
{
    public class SeedingService
    {
        private SalesWebMVCContext _context;

        public SeedingService(SalesWebMVCContext context)
        {
            _context = context;
        }

        public void Seed()
        {
            if (_context.Department.Any() ||
                _context.Seller.Any() ||
                _context.SalesRecord.Any())
                return;

            Department d1 = new Department(1, "Computers");
            Department d2 = new Department(2, "Games");

            Seller s1 = new Seller(1, "Lucas Zanetti", "lucas.hzanetti@gmail.com", 7950.50, new DateTime(1991, 03, 02), d1);
            Seller s2 = new Seller(2, "Gaspar", "gaspar@vagal.com", 0.50, new DateTime(1991, 03, 20), d2);

            SalesRecord sr1 = new SalesRecord(1, new DateTime(1991, 03, 20), 0.50, SaleStatus.Billed, s2);
            SalesRecord sr2 = new SalesRecord(2, new DateTime(1991, 03, 20), 0.00, SaleStatus.Canceled, s1);

            _context.Department.AddRange(d1, d2);
            _context.Seller.AddRange(s1, s2);
            _context.SalesRecord.AddRange(sr1, sr2);

            _context.SaveChanges();
        }
    }
}
