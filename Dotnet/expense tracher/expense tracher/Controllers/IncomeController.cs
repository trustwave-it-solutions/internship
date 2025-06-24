using expense_tracher.Enum;
using expense_tracher.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace expense_tracher.Controllers
{
    public class IncomeController : Controller
    {
        private readonly ExpenseTrackerDbContext _context;
        public IncomeController(ExpenseTrackerDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            List<IncomeViewModel> incomeViewModels = new List<IncomeViewModel>();
            //var expense = _context.TblTransactions.Where(x => x.IsDeleted != true).ToList();
            var incomeResult = await (from income in _context.TblTransactions
                                       join category in _context.TblCategories on income.CategoryId equals category.Id
                                       join paymentMode in _context.TblPaymentModes on income.PaymentModeId equals paymentMode.Id
                                       where income.IsDeleted != true
                                       select new IncomeViewModel
                                       {
                                           Id = income.Id,
                                           Name = income.Name,
                                           Category = category.Name,
                                           Amount = income.Amount,
                                           PaymentMode = paymentMode.PaymentMode
                                       }
                                       ).ToListAsync();
            return View(incomeResult);
        }
        public IActionResult Create()
        {
            try
            {
                var category = _context.TblCategories.ToList();
                List<CategoryViewModel> categoryViewModel = new List<CategoryViewModel>();

                foreach (var item in category)
                {
                    categoryViewModel.Add(new CategoryViewModel
                    {
                        Id = item.Id,
                        Name = item.Name
                    });
                }
                IncomeViewModel incomeViewModel = new IncomeViewModel()
                {
                    categoryList = categoryViewModel
                };
                return View(incomeViewModel);
            }
            catch
            {

            }
            return View();
        }
        [HttpPost]
        public IActionResult Create(IncomeViewModel incomeViewModel)
        {
            try
            {
                var userIdString = User.FindFirst("Id")?.Value;
                int userId = int.Parse(userIdString);
                TblTransaction tblTransaction = new TblTransaction()
                {
                    Name = incomeViewModel.Name,
                    Note = incomeViewModel.Note,
                    Amount = incomeViewModel.Amount,
                    PaymentModeId = incomeViewModel.PaymentModeId,
                    PaymentTypeId = 2,
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    IsDeleted = false,
                    CategoryId = incomeViewModel.CategoryId,
                    UserId = userId,
                };
                _context.TblTransactions.Add(tblTransaction);
                _context.SaveChanges();
                return RedirectToAction("Create");
            }
            catch
            {
                return View();
            }
        }
        public IActionResult Edit()
        {
            return View();
        }
        public IActionResult Delete()
        {
            return View();
        }
        public IActionResult Details()
        {
            return View();
        }
    }
}


