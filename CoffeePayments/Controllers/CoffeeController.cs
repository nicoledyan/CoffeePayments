using Microsoft.AspNetCore.Mvc;
using CoffeePaymentSystem.Models;
using CoffeePaymentSystem.Services;

namespace CoffeePaymentSystem.Controllers
{
    public class CoffeeController : Controller
    {
        private readonly IPaymentCalculator _paymentCalculator;
        private readonly IPaymentHistoryService _paymentHistoryService;
        private readonly ICoworkerService _coworkerService;

        public CoffeeController(
            IPaymentCalculator paymentCalculator,
            IPaymentHistoryService paymentHistoryService,
            ICoworkerService coworkerService)
        {
            _paymentCalculator = paymentCalculator;
            _paymentHistoryService = paymentHistoryService;
            _coworkerService = coworkerService;
        }

        public IActionResult Index(int page = 1)
        {
            var coworkers = _coworkerService.GetCoworkers().Where(c=>c.IsActive).ToList();
            const int pageSize = 5;

            var paymentHistory = _paymentHistoryService.GetPaymentHistory();
            var totalHistoryCount = paymentHistory.Count();

            var history = paymentHistory
                .OrderByDescending(h => h.DatePaid)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var paymentSummary = _paymentCalculator.GetFairnessSummary();
            var nextPayer = _paymentCalculator.GetNextPayer();

            var viewModel = new CoffeeIndexViewModel
            {
                NextPayer = nextPayer,
                Coworkers = coworkers,
                History = history,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(totalHistoryCount / (double)pageSize),
                Summary = paymentSummary
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult AddPayment(int id)
        {
            var coworker = _coworkerService.GetCoworker(id); 
            if (coworker != null)
            {
                _paymentHistoryService.AddPayment(id);
                _coworkerService.UpdateBalancesOnPayment(id);
            }

            return RedirectToAction("Index");
        }
        
        [HttpPost]
        public IActionResult SkipPerson(int id)
        {
            _paymentHistoryService.AddSkipped(id);
            return RedirectToAction("Index");
        }
        

    }
}
