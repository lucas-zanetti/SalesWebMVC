using Microsoft.AspNetCore.Mvc;
using SalesWebMVC.Services;
using System;
using System.Threading.Tasks;

namespace SalesWebMVC.Controllers
{
    public class SalesRecordsController : Controller
    {
        private readonly SalesRecordService _salesRecordService;

        public SalesRecordsController(SalesRecordService salesRecordService)
        {
            _salesRecordService = salesRecordService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> SimpleSearch(DateTime? initialDate, DateTime? finalDate)
        {
            ValidateDates(ref initialDate, ref finalDate);

            var sales = await _salesRecordService.FindByDateAsync(initialDate, finalDate);

            FormatViewDataDates(initialDate.Value, finalDate.Value);

            return View(sales);
        }

        public async Task<IActionResult> GroupingSearch(DateTime? initialDate, DateTime? finalDate)
        {
            ValidateDates(ref initialDate, ref finalDate);

            var sales = await _salesRecordService.FindByDateGroupingAsync(initialDate, finalDate);

            FormatViewDataDates(initialDate.Value, finalDate.Value);

            return View(sales);
        }

        private void ValidateDates(ref DateTime? initialDate, ref DateTime? finalDate)
        {
            if (!initialDate.HasValue)
                initialDate = new DateTime(DateTime.Now.Year, 1, 1);

            if (!finalDate.HasValue)
                finalDate = DateTime.Now;
        }

        public void FormatViewDataDates(DateTime initialDate, DateTime finalDate)
        {
            ViewData["initialDate"] = initialDate.ToString("yyyy-MM-dd");
            ViewData["finalDate"] = finalDate.ToString("yyyy-MM-dd");
        }
    }
}
