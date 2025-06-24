 using expense_tracher.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace expense_tracher.Controllers
{
    public class HomeController : Controller 
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ExpenseTrackerDbContext _context;

        public HomeController(ILogger<HomeController> logger, ExpenseTrackerDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            try
                {
                 var userIdString = User.FindFirst("Id")?.Value;
                int userId = int.Parse(userIdString);
                DashboardViewModel dashboardViewModel = new DashboardViewModel();
                dashboardViewModel.UserName = User.FindFirst("UserName")?.Value ?? "Guest";
                dashboardViewModel.Expenses = await (from expense in _context.TblTransactions
                                                     join category in _context.TblCategories on expense.CategoryId equals category.Id
                                                     where expense.IsDeleted != true && expense.UserId == userId && expense.CreatedAt >= DateTime.Now.AddMonths(-1)
                                                     select new ExpenseViewModel
                                                     {
                                                         Id = expense.Id,
                                                         Name = expense.Name,
                                                         Category = category.Name,
                                                         Amount = expense.Amount,
                                                         PaymentMode = "test",
                                                     }
                                           ).ToListAsync();
                return View(dashboardViewModel);
            }
            catch(Exception ex)
            {
                return View();
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
