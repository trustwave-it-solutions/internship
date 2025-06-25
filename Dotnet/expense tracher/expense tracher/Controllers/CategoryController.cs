using expense_tracher.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace expense_tracher.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ExpenseTrackerDbContext _context;
        public CategoryController(ExpenseTrackerDbContext context)
        {
            _context = context;
        }
        // GET: CategoryController
        public ActionResult Index()
        {
            var data = _context.TblCategories.Where(x => x.IsDeleted != true).ToList();
            return View(data);
        }
        
        // GET: CategoryController/Details/5
        public ActionResult Details(int id)
        {
            var data = _context.TblCategories.Where(x=> x.Id == id).FirstOrDefault();
            return View(data);
        }

        // GET: CategoryController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CategoryController/Create
        [HttpPost]
        public ActionResult Create(CategoryViewModel categoryViewModel)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    TempData["ErrorMessage"] = "Invalid input.";
                    return View(categoryViewModel);
                }
                var isExist = _context.TblCategories.Any(c => c.Name  == categoryViewModel.Name);
                if (isExist)
                {
                    TempData["ErrorMessage"] = "Category already exist.";
                    return View(categoryViewModel);
                }
                TblCategory category = new TblCategory() 
                {
                    Name = categoryViewModel.Name,
                    Description = categoryViewModel.Description,
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    IsDeleted = false
                };
                _context.TblCategories.Add(category);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Category saved successfully!";
                return RedirectToAction("Create");
            }
            catch
            {
                TempData["ErrorMessage"] = "Something went wrong. Please try again.";
                return View(categoryViewModel);
            }
        }

        // GET: CategoryController/Edit/5
        public ActionResult Edit(int id)
        {
            var data = _context.TblCategories.Where(x => x.Id == id).FirstOrDefault();
            CategoryViewModel categoryViewModel = new CategoryViewModel() 
            {
                Id = data.Id,
                Name = data.Name,
                Description = data.Description,
            };
            return View(categoryViewModel);
        }

        [HttpPost]
        public ActionResult Edit(CategoryViewModel categoryViewModel)
        {
            try
            {
                var data = _context.TblCategories.Where(x => x.Id == categoryViewModel.Id).FirstOrDefault();
                if (data != null)
                {
                    data.Name = categoryViewModel.Name;
                    data.Description = categoryViewModel.Description;
                    data.ModifiedAt = DateTime.UtcNow;
                    _context.TblCategories.Update(data);
                    _context.SaveChanges();
                    TempData["SuccessMessage"] = "Category updated successfully!";
                }
                return RedirectToAction("Edit");
            }
            catch
            {
                TempData["ErrorMessage"] = "Something went wrong. Please try again.";
                return View(categoryViewModel);
            }
        }

        // GET: CategoryController/Delete/5
        public ActionResult Delete(int id)
        {
            var data = _context.TblCategories.Where(x => x.Id == id).FirstOrDefault();
            return View(data);
        }

        [HttpPost]
        public IActionResult DeleteTransaction(int id)
        {
            var data = _context.TblCategories.FirstOrDefault(x => x.Id == id);
            data.IsDeleted = true;
            _context.TblCategories.Update(data);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
