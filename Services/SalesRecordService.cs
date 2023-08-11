using Microsoft.EntityFrameworkCore;
using SalesWebMVC.Data;
using SalesWebMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesWebMVC.Services
{
    public class SalesRecordService
    {
        private readonly SalesWebMVCContext _context;

        public SalesRecordService(SalesWebMVCContext context)
        {
            _context = context;
        }

        public async Task<List<SalesRecord>> FindByDateAsync(DateTime? initialDate, DateTime? finalDate)
        {
            return await FindRecordsByDateAsync(initialDate, finalDate);
        }

        public async Task<List<IGrouping<Department, SalesRecord>>> FindByDateGroupingAsync(DateTime? initialDate, DateTime? finalDate)
        {
            var result = await FindRecordsByDateAsync(initialDate, finalDate);

            return result.GroupBy(record => record.Seller.Department).ToList();
        }

        private async Task<List<SalesRecord>> FindRecordsByDateAsync(DateTime? initialDate, DateTime? finalDate)
        {
            var result = from record in _context.SalesRecord select record;

            if (initialDate.HasValue)
                result = result.Where(record => record.Date >= initialDate.Value);

            if (finalDate.HasValue)
                result = result.Where(record => record.Date <= finalDate.Value);

            return await result
                .Include(record => record.Seller)
                .Include(record => record.Seller.Department)
                .OrderByDescending(record => record.Date)
                .ToListAsync();
        }
    }
}
