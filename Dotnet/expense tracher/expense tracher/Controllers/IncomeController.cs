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
        public async Task<IActionResult> Index(DateTime? fromDate, DateTime? toDate)
        {
            var userIdString = User.FindFirst("Id")?.Value;
            int userId = int.Parse(userIdString);
            List<IncomeViewModel> incomeViewModels = new List<IncomeViewModel>();
            //var expense = _context.TblTransactions.Where(x => x.IsDeleted != true).ToList();
            var incomeResult = await (from income in _context.TblTransactions
                                       join category in _context.TblCategories on income.CategoryId equals category.Id
                                       join paymentMode in _context.TblPaymentModes on income.PaymentModeId equals paymentMode.Id
                                       where income.IsDeleted != true && income.PaymentTypeId == 1 && income.UserId == userId
                                      select new IncomeViewModel
                                       {
                                           Id = income.Id,
                                           Name = income.Name,
                                           Category = category.Name,
                                           Amount = income.Amount,
                                           PaymentMode = paymentMode.PaymentMode,
                                           CreatedAt = income.CreatedAt,
                                       }
                                       ).ToListAsync();
            if (fromDate.HasValue)
            {
                incomeResult = incomeResult.Where(x => x.CreatedAt <= toDate).ToList();
            }
            if (toDate.HasValue)
            {
                incomeResult = incomeResult.Where(x => x.CreatedAt <= toDate).ToList();
            }
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
                    PaymentTypeId = 1,
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    IsDeleted = false,
                    CategoryId = incomeViewModel.CategoryId,
                    UserId = userId,
                };
                _context.TblTransactions.Add(tblTransaction);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Income saved successfully!";
                return RedirectToAction("Create");
            }
            catch
            {
                TempData["ErrorMessage"] = "Something went wrong. Please try again.";
                return View();
            }
        }
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                
                IncomeViewModel incomeViewModel = new IncomeViewModel();
                var categoryList = _context.TblCategories.ToList();
                List<CategoryViewModel> categoryViewModel = new List<CategoryViewModel>();

                foreach (var item in categoryList)
                {
                    categoryViewModel.Add(new CategoryViewModel
                    {
                        Id = item.Id,
                        Name = item.Name
                    });
                }
                var incomeResult = await(from income in _context.TblTransactions
                                         join category in _context.TblCategories on income.CategoryId equals category.Id
                                         join paymentMode in _context.TblPaymentModes on income.PaymentModeId equals paymentMode.Id
                                         where income.Id == id
                                         select new IncomeViewModel
                                         {
                                             Id = income.Id,
                                             Name = income.Name,
                                             Category = category.Name,
                                             CategoryId = income.CategoryId,
                                             Amount = income.Amount,
                                             PaymentMode = paymentMode.PaymentMode,
                                             PaymentModeId = income.PaymentModeId
                                         }
                                      ).FirstOrDefaultAsync();
                if (incomeResult != null)
                {
                    incomeViewModel = incomeResult;
                    incomeViewModel.categoryList = categoryViewModel;
                }
                return View(incomeViewModel);
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public IActionResult Edit(IncomeViewModel incomeViewModel)
        {
            try
            {
                var data = _context.TblTransactions.Where(x => x.Id == incomeViewModel.Id).FirstOrDefault();
                data.Note = incomeViewModel.Note;
                data.Name = incomeViewModel.Name;
                data.Amount = incomeViewModel.Amount;
                data.PaymentModeId = incomeViewModel.PaymentModeId;
                data.ModifiedAt = DateTime.UtcNow;
                data.CategoryId = incomeViewModel.CategoryId;
                _context.TblTransactions.Update(data);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Income updated successfully!";
                return RedirectToAction("Edit");
            }
            catch
            {
                TempData["ErrorMessage"] = "Something went wrong. Please try again.";
                return View();
            }
        }
        public IActionResult Delete(int id)
        {
            var data = _context.TblTransactions.Where(x => x.Id == id).FirstOrDefault();
            return View(data);
        }

        [HttpPost]
        public IActionResult DeleteTransaction(int id)
        {
            var data = _context.TblTransactions.Where(x => x.Id == id).FirstOrDefault();
            data.IsDeleted = true;
            _context.TblTransactions.Update(data);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                IncomeViewModel incomeViewModel = new IncomeViewModel();
                var incomeResult = await(from income in _context.TblTransactions
                                         join category in _context.TblCategories on income.CategoryId equals category.Id
                                         join paymentMode in _context.TblPaymentModes on income.PaymentModeId equals paymentMode.Id
                                         where income.Id==id
                                         select new IncomeViewModel
                                         {
                                             Id = income.Id,
                                             Name = income.Name,
                                             Category = category.Name,
                                             Amount = income.Amount,
                                             PaymentMode = paymentMode.PaymentMode,
                                             CreatedAt = income.CreatedAt,
                                             ModifiedAt = income.ModifiedAt
                                         }
                                      ).FirstOrDefaultAsync();
                if (incomeResult != null)
                {
                    incomeViewModel = incomeResult;
                }
                return View(incomeViewModel);
            }
            catch
            {
                return View();
            }
           
        }
    }
}


