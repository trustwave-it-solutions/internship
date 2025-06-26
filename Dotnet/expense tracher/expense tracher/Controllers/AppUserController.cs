using expense_tracher.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace expense_tracher.Controllers
{
    public class AppUserController : Controller
    {
        private readonly ExpenseTrackerDbContext _context;
        private AuthenticationProperties? principal;

        public AppUserController(ExpenseTrackerDbContext context)
        {
            _context = context;
        }
        // GET: AppUserController
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                var hasedPassword = PasswordHelper.HashPassword(loginViewModel.Password);
                var user = _context.TblUsers.FirstOrDefault(u => u.UserName == loginViewModel.UserName && u.Password == hasedPassword);
                if (user == null)
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                    return View(loginViewModel);
                }
                var claims = new List<Claim>
                {
                    new Claim("UserName", user.UserName),
                    new Claim("Id", user.Id.ToString())
                };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                return RedirectToAction("Index", "Home");
            }
            return View(loginViewModel);
        }
        public ActionResult Registration()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Registration(RegistrationViewModel registrationViewModel)
        {
            if (ModelState.IsValid)
            {
                var isexists = _context.TblUsers.Any(u => u.UserName == registrationViewModel.UserName);
                if (isexists)
                {
                    ModelState.AddModelError("UserName", "Username already exists. Please choose a different username.");
                    return View(registrationViewModel);
                }
                if (registrationViewModel.Password != registrationViewModel.ConfirmPassword)
                {
                    ModelState.AddModelError("ConfirmPassword", "Passwords do not match.");
                    return View(registrationViewModel);
                }
                TblUser user = new TblUser
                {
                    Name = registrationViewModel.Name,
                    UserName = registrationViewModel.UserName,
                    Password = PasswordHelper.HashPassword(registrationViewModel.Password),
                    Phone = registrationViewModel.Phone,
                    Email = registrationViewModel.Email
                };
                _context.TblUsers.Add(user);
                _context.SaveChanges();
                return RedirectToAction("Login");
            }
            return View(registrationViewModel);
        }
        public static class PasswordHelper
        {
            public static string HashPassword(string password)
            {
                using var sha = SHA256.Create();
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            return RedirectToAction("Login");
            
        }
        public ActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel changepasswordViewModel)
        {
            if (ModelState.IsValid)
            {
                var userName = User.FindFirst("UserName")?.Value;
                var user = _context.TblUsers.FirstOrDefault(u => u.UserName == userName);
                if (user == null)
                {
                    ModelState.AddModelError("", "User not found.");
                    return View(changepasswordViewModel);
                }
                var hashedOldPassword = PasswordHelper.HashPassword(changepasswordViewModel.OldPassword);
                if (user.Password != hashedOldPassword)
                {
                    ModelState.AddModelError("OldPassword", "Old password is incorrect.");
                    return View(changepasswordViewModel);
                }
                if (changepasswordViewModel.NewPassword != changepasswordViewModel.ConfirmPassword)
                {
                    ModelState.AddModelError("ConfirmPassword", "New password and confirm password do not match.");
                    return View(changepasswordViewModel);
                }
                if (changepasswordViewModel.OldPassword == changepasswordViewModel.NewPassword)
                {
                    ModelState.AddModelError("NewPassword", "New password cannot be the same as old password.");
                    return View(changepasswordViewModel);
                }
                user.Password = PasswordHelper.HashPassword(changepasswordViewModel.NewPassword);
                _context.SaveChanges();
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                return RedirectToAction("Login");
            }
            return View(changepasswordViewModel);
        }

        public ActionResult ProfileEdit()
        {
            var userName = User.FindFirst("UserName")?.Value;
            var user = _context.TblUsers.FirstOrDefault(u => u.UserName == userName);
            ProfileEditViewModel profileEditViewModel = new ProfileEditViewModel()
            {
                UserName = user.UserName,
                Phone = user.Phone,
                Email = user.Email
            };
            return View(profileEditViewModel);
        }
        [HttpPost]
        public async Task<ActionResult> ProfileEdit(ProfileEditViewModel profileEditViewModel)
        {
            if (ModelState.IsValid)
            {
                var userName = User.FindFirst("UserName")?.Value;
                var user = _context.TblUsers.FirstOrDefault(u => u.UserName == userName);
                if (user == null)
                {
                    ModelState.AddModelError("", "User not found.");
                    return View(profileEditViewModel);
                }
                user.UserName = profileEditViewModel.UserName;
                user.Phone = profileEditViewModel.Phone;
                user.Email = profileEditViewModel.Email;
                _context.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View(profileEditViewModel); 
        }
        // GET: AppUserController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AppUserController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AppUserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AppUserController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AppUserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AppUserController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AppUserController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
