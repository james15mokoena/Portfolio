using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portfolio.Data;
using Portfolio.Models;

namespace Portfolio.Controllers
{
    /*
       This controller class handles the functionality for administrator login 
       and account creation.
     */
    public class AuthenticationController : Controller
    {
        // Its used to connect the application/controller to the database.
        private readonly PortfolioDbContext dbContext;

        public AuthenticationController(PortfolioDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        // Displays the login view/page.
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // Handles the admin login verification functionality.
        public async Task<IActionResult> LoginVerification(Administrator admin)
        {                        
            var admini = await dbContext.Administrators.FirstOrDefaultAsync(adm => adm.Email == admin.Email);

            if (admini != null)
            {
                if (admini.Email != null && admini.Password != null && admin.Password != null)
                {
                    if (PasswordEncrypter.VerifyPassword(admin.Password, admini.Password))
                    {
                        ViewData["LoginStatus"] = "yes";
                        HttpContext.Session.SetString("email", admini.Email);
                        return RedirectToAction("Index", "Home");
                    }                    
                }                
            }            

            TempData["ErrorMessage"] = "Incorrect email or password.";
            return RedirectToAction("Login", "Authentication");            
        }

        // Handles the Logout functionality.
        [HttpGet]
        public ActionResult Logout()
        {
            if (ViewData["LoginStatus"] != null || HttpContext.Session.Get("email") != null)
            {
                string? status = (string?) ViewData["LoginStatus"];

                if(status != null || HttpContext.Session.Get("email") != null)
                {
                    ViewData["LoginStatus"] = null;
                    HttpContext.Session.Remove("email");
                    return RedirectToAction("Index", "Home");
                }

            }
                        
            return RedirectToAction("Index", "Home");
        }

        // Displays the account creation page/view.
        [HttpGet]
        public IActionResult CreateAccount()
        {
            return View();
        }

        // Handles the functionality for adding a new administrator.
        public async Task<IActionResult> CreateNewAccount(Administrator admin)
        {
            var exAdmin = await dbContext.Administrators.FirstOrDefaultAsync(adm => adm.Email == admin.Email);

            if (exAdmin != null)
            {
                TempData["ErrorMessage"] = "Administrator already exists.";
                return RedirectToAction("CreateAccount", "Authentication");
            }
            else
            {
                if (admin != null)
                {
                    if (admin.Email != null && admin.Email.Contains(".admin.boss") && admin.Password != null)
                    {
                        admin.Password = PasswordEncrypter.HashPassword(admin.Password);
                        await dbContext.Administrators.AddAsync(admin);
                        await dbContext.SaveChangesAsync();
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Unacceptable email";
                        return RedirectToAction("CreateAccount", "Authentication");
                    }
                }

                TempData["ErrorMessage"] = "Something is wrong, please check your email or password";
                return RedirectToAction("CreateAccount", "Authentication");
            }
        }
    }
}
