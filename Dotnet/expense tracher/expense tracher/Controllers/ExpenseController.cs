using expense_tracher.Enum;
using expense_tracher.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace expense_tracher.Controllers
{
    public class ExpenseController : Controller
    {
        private readonly ExpenseTrackerDbContext _context;
        public ExpenseController(ExpenseTrackerDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(DateTime? fromDate, DateTime? toDate)
        {
            List<ExpenseViewModel> expenseViewModels = new List<ExpenseViewModel>();
            //var expense = _context.TblTransactions.Where(x => x.IsDeleted != true).ToList();
            var expenseResult = await (from expense in _context.TblTransactions
                                       join category in _context.TblCategories on expense.CategoryId equals category.Id
                                       join paymentMode in _context.TblPaymentModes on expense.PaymentModeId equals paymentMode.Id
                                       where expense.IsDeleted != true && expense.PaymentTypeId == 2
                                       select new ExpenseViewModel
                                       {
                                           Id = expense.Id,
                                           Name = expense.Name,
                                           Category = category.Name,
                                           Amount = expense.Amount,
                                           PaymentMode = paymentMode.PaymentMode,
                                           CreatedAt = expense.CreatedAt,
                                       }
                                       ).ToListAsync();
            if (fromDate.HasValue)
            {
                expenseResult = expenseResult.Where(x => x.CreatedAt <= toDate).ToList();
            }
            if (toDate.HasValue)
            {
                expenseResult = expenseResult.Where(x => x.CreatedAt <= toDate).ToList();
            }
            return View(expenseResult);
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
                ExpenseViewModel expenseViewModel = new ExpenseViewModel()
                {
                    categoryList = categoryViewModel
                };
                return View(expenseViewModel);
              
            }
            catch
            {

            }
            return View();
        }
        [HttpPost]
        public IActionResult Create(ExpenseViewModel expenseViewModel)
        {
            try
            {
                var userIdString = User.FindFirst("Id")?.Value;
                int userId = int.Parse(userIdString);
                TblTransaction tblTransaction = new TblTransaction()
                {
                    Name = expenseViewModel.Name,
                    Note = expenseViewModel.Note,
                    Amount = expenseViewModel.Amount,
                    PaymentModeId = expenseViewModel.PaymentModeId,
                    PaymentTypeId = 2,
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    IsDeleted = false,
                    CategoryId = expenseViewModel.CategoryId,
                    UserId = userId,
                };
                _context.TblTransactions.Add(tblTransaction);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Expense Created successfully!";
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
            ExpenseViewModel expenseViewModels = new ExpenseViewModel();
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
            var expenseResult = await (from expense in _context.TblTransactions
                                       join category in _context.TblCategories on expense.CategoryId equals category.Id
                                       join paymentMode in _context.TblPaymentModes on expense.PaymentModeId equals paymentMode.Id
                                       where expense.Id == id 
                                       select new ExpenseViewModel
                                       {
                                           Id = expense.Id,
                                           Name = expense.Name,
                                           Category = category.Name,
                                           CategoryId = expense.CategoryId,
                                           Amount = expense.Amount,
                                           PaymentMode = paymentMode.PaymentMode,
                                           PaymentModeId = expense.PaymentModeId,
                                       }
                                       ).FirstOrDefaultAsync();
            if(expenseResult != null)
            {
                expenseViewModels = expenseResult;
                expenseViewModels.categoryList = categoryViewModel;
            }
            return View(expenseViewModels);
        }
        
        [HttpPost]
        public IActionResult Edit(ExpenseViewModel expenseViewModel)
        {
            try
            {
                var data = _context.TblTransactions.FirstOrDefault(x => x.Id == expenseViewModel.Id);
                if (data == null)
                {
                    return NotFound("Expense not found");
                }
                data.Name = expenseViewModel.Name;
                data.Note = expenseViewModel.Note;
                data.Amount = expenseViewModel.Amount;
                data.PaymentModeId = expenseViewModel.PaymentModeId;
                data.CategoryId = expenseViewModel.CategoryId;
                data.ModifiedAt = DateTime.UtcNow;
                _context.TblTransactions.Update(data);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Expense updated successfully!";
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
            var data = _context.TblTransactions.FirstOrDefault(x => x.Id == id);
            return View(data);
        }
        [HttpPost]
        public IActionResult DeleteTransaction(int id)
        {
            var data = _context.TblTransactions.FirstOrDefault(x => x.Id == id);
            data.IsDeleted = true;
            _context.TblTransactions.Update(data);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                ExpenseViewModel expenseViewModels = new ExpenseViewModel();
                var expenseResult = await (from expense in _context.TblTransactions
                                           join category in _context.TblCategories on expense.CategoryId equals category.Id
                                           join paymentMode in _context.TblPaymentModes on expense.PaymentModeId equals paymentMode.Id
                                           where expense.Id == id
                                           select new ExpenseViewModel
                                           {
                                               Id = expense.Id,
                                               Name = expense.Name,
                                               Category = category.Name,
                                               Amount = expense.Amount,
                                               PaymentMode = paymentMode.PaymentMode,
                                               CreatedAt = expense.CreatedAt,
                                               ModifiedAt = expense.ModifiedAt,
                                             
                                           }
                                           ).FirstOrDefaultAsync();
                if ( expenseResult != null ) {
                    expenseViewModels = expenseResult;
                }
                else
                {
                    return NotFound("Expense not found");
                }
                return View(expenseViewModels);
            }
            catch (Exception ex)
            {
                return View();
            }
        }
    }
}

   
