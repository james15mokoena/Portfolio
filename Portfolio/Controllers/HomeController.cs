using Microsoft.AspNetCore.Mvc;
using Portfolio.Models;
using Portfolio.Data;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace Portfolio.Controllers
{
    /*
       This controller class handles the functionality for displaying projects.  
     */
    public class HomeController : Controller
    {
        // Its used to connect the application/controller to the database.
        private readonly PortfolioDbContext dbContext;

        public HomeController(PortfolioDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        /* Displays a page/view that contains projects that I worked on
           in the last 30 days.
         */
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if(HttpContext.Session.Get("email") != null)
            {
                ViewData["LoginStatus"] = "yes";
            }
            else
            {
                ViewData["LoginStatus"] = "no";
            }

            DateTime now = DateTime.Now;
            DateTime last30 = DateTime.Now - TimeSpan.FromDays(30);

            var projects =  dbContext.Projects.Where(proj => (proj.ReleaseDate >= last30 && proj.ReleaseDate <= now));

            if (projects == null)
            {
                return NotFound();
            }

            return View(await projects.ToListAsync());
        }

        /* Displays a page/view that contains projects of a specific
         * type/category.
        */
        [HttpGet]
        public async Task<IActionResult> AppType(string? type)
        {
            if (type == null)
            {
                if (HttpContext.Session.Get("email") != null)
                {
                    ViewData["LoginStatus"] = "yes";
                }

                return NotFound();
            }

            var projects = dbContext.Projects.Where(project => project.Type == type);

            if (projects != null)
            {
                if (HttpContext.Session.Get("email") != null)
                {
                    ViewData["LoginStatus"] = "yes";
                }

                return View(await projects.ToListAsync());
            }

            return NotFound();
        }

        /* Displays a page/view that contains information 
           about me (Pheello James Mokoena).
        */
        [HttpGet]
        public IActionResult About()
        {
            if (HttpContext.Session.Get("email") != null)
            {
                ViewData["LoginStatus"] = "yes";
            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
