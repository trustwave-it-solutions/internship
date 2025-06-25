using expense_tracher.Enum;
using expense_tracher.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<IActionResult> Index()
        {
            List<ExpenseViewModel> expenseViewModels = new List<ExpenseViewModel>();
            //var expense = _context.TblTransactions.Where(x => x.IsDeleted != true).ToList();
            var expenseResult = await (from expense in _context.TblTransactions
                                       join category in _context.TblCategories on expense.CategoryId equals category.Id
                                       join paymentMode in _context.TblPaymentModes on expense.PaymentModeId equals paymentMode.Id
                                       where expense.IsDeleted != true && expense.PaymentTypeId == 2
                                       select new ExpenseViewModel
                                       {
                                           Id=expense.Id,
                                           Name=expense.Name,
                                           Category=category.Name,
                                           Amount=expense.Amount,
                                           PaymentMode=paymentMode.PaymentMode
                                       }
                                       ).ToListAsync();
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

   
