using Microsoft.AspNetCore.Mvc;
using SalesWebMVC.Models;
using SalesWebMVC.Models.ViewModels;
using SalesWebMVC.Services;
using System;
using System.Diagnostics;

namespace SalesWebMVC.Controllers
{
    public class SellersController : Controller
    {
        private readonly SellerService _sellerService;
        private readonly DepartmentService _departmentService;

        public SellersController(SellerService sellerService, DepartmentService departmentService)
        {
            _sellerService = sellerService;
            _departmentService = departmentService;
        }

        public IActionResult Index()
        {
            var sellerList = _sellerService.FindAll();

            return View(sellerList);
        }

        public IActionResult Create()
        {
            var departmentsList = _departmentService.FindAll();
            var sellerFormViewModel = new SellerFormViewModel { Departments = departmentsList };
            return View(sellerFormViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Seller seller)
        {
            _sellerService.AddSeller(seller);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });

            var seller = _sellerService.FindById(id.Value);

            if (seller == null)
                return RedirectToAction(nameof(Error), new { message = "Seller not found" });

            return View(seller);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            _sellerService.DeleteSeller(id);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });

            var seller = _sellerService.FindById(id.Value);

            if (seller == null)
                return RedirectToAction(nameof(Error), new { message = "Seller not found" });

            return View(seller);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });

            var seller = _sellerService.FindById(id.Value);

            if (seller == null)
                return RedirectToAction(nameof(Error), new { message = "Seller not found" });

            var departments = _departmentService.FindAll();

            SellerFormViewModel viewModel = new SellerFormViewModel { Seller = seller, Departments = departments };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Seller seller)
        {
            if (id != seller.Id)
                return RedirectToAction(nameof(Error), new { message = "Seller Id mismatch" });

            try
            {
                _sellerService.UpdateSeller(seller);

                return RedirectToAction(nameof(Index));
            }
            catch(ApplicationException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
        }

        public IActionResult Error(string message)
        {
            var viewModel = new ErrorViewModel
            {
                Message = message,
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };

            return View(viewModel);
        }
    }
}
