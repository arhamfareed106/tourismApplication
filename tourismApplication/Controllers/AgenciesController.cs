
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tourismApplication.Models;

namespace tourismApplication.Controllers
{
    public class AgenciesController : Controller
    {
        private readonly AppDbContext _db;
        public AgenciesController(AppDbContext db) => _db = db;

        public IActionResult Index()
            => View(_db.Agencies.AsNoTracking().ToList());

        public IActionResult Details(int id)
        {
            var agency = _db.Agencies.Find(id);
            return agency == null ? NotFound() : View(agency);
        }

        public IActionResult Create()
        {
            var agency = new Agency
            {
                Description = "Welcome to our agency! We specialize in providing amazing travel experiences, including customized tours and packages for all types of travelers."
            };
            return View(agency);
        }


        [HttpPost]
        public IActionResult Create(Agency agency)
        {
            if (string.IsNullOrWhiteSpace(agency.Description))
            {
                agency.Description = "Welcome to our agency! We specialize in providing amazing travel experiences, including customized tours and packages for all types of travelers.";
            }
            if (!ModelState.IsValid) return View(agency);
            _db.Agencies.Add(agency);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Edit(int id)
        {
            var agency = _db.Agencies.Find(id);
            return agency == null ? NotFound() : View(agency);
        }

        [HttpPost]
        public IActionResult Edit(Agency agency)
        {
            if (!ModelState.IsValid) return View(agency);
            _db.Update(agency);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var agency = _db.Agencies.Find(id);
            return agency == null ? NotFound() : View(agency);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var agency = _db.Agencies.Find(id);
            if (agency != null)
            {
                _db.Agencies.Remove(agency);
                _db.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]

        public IActionResult Login()
        {
            return View();

        }

        [HttpPost]

        public IActionResult Login(AgencyLogin model)
        {
            // No authentication, just redirect
            return RedirectToAction("Index", "Agencies");
        }

    }
}
