using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portfolio.Data;
using Portfolio.Models;

namespace Portfolio.Controllers
{
    /*
       This controller class handles the functionality for displaying, editing
        and adding projects information.
     */
    public class ProjectsController : Controller
    {
        // Its used to connect the application/controller to the database.
        private readonly PortfolioDbContext dbContext;

        public ProjectsController(PortfolioDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        /*
         Displays a page/view that contains all the projects I have worked
         on.
        */
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var projects = await dbContext.Projects.ToListAsync();

            if(projects != null)
            {
                if (HttpContext.Session.Get("email") != null)
                {
                    ViewData["LoginStatus"] = "yes";
                }

                return View(projects);
            }

            if (HttpContext.Session.Get("email") != null)
            {
                ViewData["LoginStatus"] = "yes";
            }

            return View();
        }

        /*
         * Displays a page/view that contains the details of a 
         * specific project.
         */
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Get the project with the given id.
            var project = await dbContext.Projects.FirstOrDefaultAsync(project => project.Id == id);

            if (project == null)
            {
                return NotFound();
            }
            else
            {
                if(HttpContext.Session.Get("email") != null)
                {
                    ViewData["LoginStatus"] = "yes";
                }
            }            

            return View(project);
        }

        /*
          Displays the page/view that allow the administrator to add a new project.
         */
        [HttpGet]
        public IActionResult AddProject()
        {
            if (HttpContext.Session.Get("email") != null)
            {
                ViewData["LoginStatus"] = "yes";
            }

            return View();
        }

        /*
         * Provides the functionality for adding a new project.
         */
        [HttpPost]
        public async Task<IActionResult> AddProject(Project project)
        {
            if (HttpContext.Session.Get("email") != null)
            {
                if (ModelState.IsValid)
                {
                    await dbContext.Projects.AddAsync(project);
                    await dbContext.SaveChangesAsync();
                    ViewData["LoginStatus"] = "yes";
                    return RedirectToAction("Index", "Home");
                }
            }

            TempData["ErrorMessage"] = "Please login";

            return RedirectToAction("Login", "Authentication");
        }

        /*
         Displays a page/view that contains the editable details of a 
         project.
        */
        [HttpGet]
        public async Task<IActionResult> EditDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await dbContext.Projects.FirstOrDefaultAsync(project => project.Id == id);

            if (project == null)
            {
                return NotFound();
            }

            if (HttpContext.Session.Get("email") != null)
            {
                ViewData["LoginStatus"] = "yes";
            }

            return View(project);
        }

        /*
         Provides the functionality for editing the details of a project.
        */
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, Project project)
        {
            if (HttpContext.Session.Get("email") != null)
            {
                if (ModelState.IsValid)
                {
                    int? i = id;
                    var p = project;
                    var proj = await dbContext.Projects.FirstOrDefaultAsync(proj => proj.Id == id);

                    if (proj != null)
                    {
                        proj.Name = project.Name;
                        proj.Description = project.Description;
                        proj.Type = project.Type;
                        proj.ImageUrl = project.ImageUrl;
                        proj.IsInDevelopment = project.IsInDevelopment;
                        proj.DateStarted = project.DateStarted;
                        proj.ReleaseDate = project.ReleaseDate;
                        proj.GitHubRepo = project.GitHubRepo;
                        dbContext.Projects.Update(proj);
                        await dbContext.SaveChangesAsync();
                        ViewData["LoginStatus"] = "yes";

                        return RedirectToAction("Index","Home");
                    }
                }
            }

            TempData["ErrorMessage"] = "Please login";

            return RedirectToAction("Login", "Authentication");
        }

        /*
         Provides the functionality for delete a project.
        */       
        public async Task<IActionResult> Delete(int? id)
        {
            if (HttpContext.Session.Get("email") != null)
            {
                var project = await dbContext.Projects.FirstOrDefaultAsync(proj => proj.Id == id);

                if (project != null)
                {
                    dbContext.Projects.Remove(project);
                    await dbContext.SaveChangesAsync();
                    ViewData["LoginStatus"] = "yes";

                    return RedirectToAction("Index", "Home");
                }
            }

            TempData["ErrorMessage"] = "Please login";
            return RedirectToAction("Login", "Authentication");
        }

        /*
         * Displays a page/view that contains projects that are currently in development.
         */
        [HttpGet]
        public async Task<IActionResult> InDevelopment()
        {
            var projects = await dbContext.Projects.Where(proj => proj.IsInDevelopment == "yes").ToListAsync();

            if (projects != null)
            {
                if (HttpContext.Session.Get("email") != null)
                {
                    ViewData["LoginStatus"] = "yes";
                }

                return View(projects);
            }

            if (HttpContext.Session.Get("email") != null)
            {
                ViewData["LoginStatus"] = "yes";
            }

            return View();
        }
    }
}
