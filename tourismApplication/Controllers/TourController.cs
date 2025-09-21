using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tourismApplication.Models;
using tourismApplication.Services;

namespace tourismApplication.Controllers
{
    public class ToursController : Controller
    {
        private readonly AppDbContext _db;
        private readonly Services.TourJsonService _jsonService;


        public ToursController(AppDbContext db, TourJsonService jsonService)
        {
            _jsonService = jsonService;
            _db = db;
        }


        public IActionResult Index()
        {

            var jsonTours = _jsonService.GetTours().ToList();
   

            var tours = _db.Tours
                       .AsNoTracking()
                       .ToList();

            var allTours = jsonTours.Concat(tours).ToList();

            return View(allTours);

            //return View(tours);
        }
        private object[] GetLocations() => new[]
{
    new { Name = "Sydney City Tour", PricePerDay = 120m, GroupLimit = 20, From = DateTime.Today,             To = DateTime.Today.AddMonths(3) },
    new { Name = "Blue Mountains",   PricePerDay = 150m, GroupLimit = 15, From = DateTime.Today.AddDays(7),  To = DateTime.Today.AddMonths(2) },
    new { Name = "Great Ocean Road", PricePerDay = 180m, GroupLimit = 25, From = DateTime.Today.AddDays(14), To = DateTime.Today.AddMonths(4) }
};

        public IActionResult Create()
        {
            ViewBag.Locations = GetLocations();
            return View();
        }
        [HttpPost]
        public IActionResult Create(Tour tour)
        {
            // Validate dates
            if (tour.AvailableTo < tour.AvailableFrom)
            {
                ModelState.AddModelError(nameof(tour.AvailableTo), "End date must be on or after start date.");
            }

            // Validate Title
            if (string.IsNullOrWhiteSpace(tour.Title))
            {
                ModelState.AddModelError(nameof(tour.Title), "Please select a location.");
            }

            // Calculate DurationDays
            if (ModelState.IsValid)
            {
                tour.DurationDays = (int)(tour.AvailableTo - tour.AvailableFrom).TotalDays + 1;
                tour.Price = tour.DurationDays * tour.PeopleCount * tour.PricePerDay;

                _db.Tours.Add(tour);
                _db.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            // Repopulate locations if validation fails
            ViewBag.Locations = GetLocations();
            return View(tour);
        }


        //[HttpPost]
        //public IActionResult Create(Tour tour, string imageUrls)
        //{
        //    // Split input into list if not null
        //    if (!string.IsNullOrWhiteSpace(imageUrls))
        //    {
        //        tour.ImageUrl = imageUrls;


        //    }

        //    if (tour.AvailableTo >= tour.AvailableFrom)
        //        tour.DurationDays = (int)(tour.AvailableTo - tour.AvailableFrom).TotalDays + 1;
        //    else
        //        ModelState.AddModelError(nameof(tour.AvailableTo), "End date must be on or after start date.");

        //    tour.Price = tour.DurationDays * tour.PeopleCount * tour.PricePerDay;
        //    if (string.IsNullOrWhiteSpace(tour.Title))
        //        ModelState.AddModelError(nameof(tour.Title), "Please choose a location.");

        //    if (!ModelState.IsValid)
        //    {
        //        ViewBag.Locations = GetLocations();
        //        return View(tour);
        //    }

        //    _db.Tours.Add(tour);
        //    _db.SaveChanges();
        //    //return RedirectToAction("Tours");
        //    return RedirectToAction(nameof(Index));

        //}
        //NEW code
        //[HttpPost]
        //public IActionResult Create(Tour tour, string imageUrls)
        //{
        //    // Split input into list if not null
        //    if (!string.IsNullOrWhiteSpace(imageUrls))
        //    {
        //        tour.Images = imageUrls
        //            .Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries)
        //            .ToList();
        //    }

        //    if (tour.AvailableTo >= tour.AvailableFrom)
        //        tour.DurationDays = (int)(tour.AvailableTo - tour.AvailableFrom).TotalDays + 1;
        //    else
        //        ModelState.AddModelError(nameof(tour.AvailableTo), "End date must be on or after start date.");

        //    tour.Price = tour.DurationDays * tour.PeopleCount * tour.PricePerDay;

        //    if (string.IsNullOrWhiteSpace(tour.Title))
        //        ModelState.AddModelError(nameof(tour.Title), "Please choose a location.");

        //    if (!ModelState.IsValid)
        //    {
        //        ViewBag.Locations = GetLocations();
        //        return View(tour);
        //    }

        //    _db.Tours.Add(tour);
        //    _db.SaveChanges();
        //    return RedirectToAction("Individual", new { id = tour.TourId });
        //}

        //
        public IActionResult Edit(int id)
        {
            var tour = _db.Tours.Find(id);
            if (tour == null) return NotFound();
            ViewBag.Locations = GetLocations();
            return View(tour);
        }

        [HttpPost]
        public IActionResult Edit(Tour tour)
        {
            if (tour.AvailableTo >= tour.AvailableFrom)
                tour.DurationDays = (int)(tour.AvailableTo - tour.AvailableFrom).TotalDays + 1;
            else
                ModelState.AddModelError(nameof(tour.AvailableTo), "End date must be on or after start date.");

            tour.Price = tour.DurationDays * tour.PeopleCount * tour.PricePerDay;

            if (string.IsNullOrWhiteSpace(tour.Title))
                ModelState.AddModelError(nameof(tour.Title), "Please choose a location.");

            if (!ModelState.IsValid)
            {
                ViewBag.Locations = GetLocations();
                return View(tour);
            }

            _db.Update(tour);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }




        public IActionResult Details(int id)
        {

            var jsonTours = _jsonService.GetTours().ToList();

            var tour = _db.Tours
                          .AsNoTracking()

                          .FirstOrDefault(t => t.TourId == id);

            if (tour == null)
            {
                tour = _db.Tours.AsNoTracking().FirstOrDefault(t => t.TourId == id);
            }

            return tour == null ? NotFound() : View(tour);
        }

        public IActionResult Individual(int id)
        {
            var tour = _db.Tours.AsNoTracking().FirstOrDefault(t => t.TourId == id);
            if (tour == null) return NotFound();
            return View(tour);
        }




        public IActionResult Delete(int id)
        {
            var tour = _db.Tours
                          .AsNoTracking()
                          .FirstOrDefault(t => t.TourId == id);
            return tour == null ? NotFound() : View(tour);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var tour = _db.Tours.Find(id);
            if (tour != null)
            {
                _db.Tours.Remove(tour);
                _db.SaveChanges();
            }

            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}