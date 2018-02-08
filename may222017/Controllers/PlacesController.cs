using may222017.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace may222017.Controllers
{
    public class PlacesController : Controller
    {
        public myDbContext db;
        private UserManager<ApplicationUser> manager;
        public PlacesController()
        {
            db = new myDbContext();
            manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
        }

        public ActionResult All()
        {
            return View(db.Places.ToList());
        }

        public async Task<ActionResult> Venue(int? id)
        {

            Places place = await db.Places.FindAsync(id);
            if (place == null)
            {
                return HttpNotFound();
            }

            return View(place);
        }

        //public ActionResult Index()
        //{
        //    var currentUser = manager.FindById(User.Identity.GetUserId());
        //    return View(db.Places.ToList());
        //}

        public ActionResult Index(string sortOrder,string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "name_desc";
            ViewBag.DateSortParm = sortOrder == "min-price" ? "max-price" : "min-price";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.currentFilter = searchString;
            var halls = from s in db.Places.Where(s => s.status == "Approved")
                        select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                halls = halls.Where(s => s.PlaceName.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    halls = halls.OrderByDescending(s => s.PlaceName);
                    break;
                case "min-price":
                    halls = halls.OrderBy(s => s.MinPriceRange);
                    break;
                case "max-price":
                    halls = halls.OrderByDescending(s => s.MaxPriceRange);
                    break;
                default:  // Name ascending 
                    halls = halls.OrderBy(s => s.PlaceName);
                    break;
            }

            int pageSize = 9;
            int pageNumber = (page ?? 1);
            //return View(halls.OrderBy(i => i.PlaceName).ToPagedList(pageNumber, pageSize));
            return View(halls.ToPagedList(pageNumber, pageSize));
        }

        /*
         public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
{
   ViewBag.CurrentSort = sortOrder;
   ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
   ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

   if (searchString != null)
   {
      page = 1;
   }
   else
   {
      searchString = currentFilter;
   }

   ViewBag.CurrentFilter = searchString;

   var students = from s in db.Students
                  select s;
   if (!String.IsNullOrEmpty(searchString))
   {
      students = students.Where(s => s.LastName.Contains(searchString)
                             || s.FirstMidName.Contains(searchString));
   }
   switch (sortOrder)
   {
      case "name_desc":
         students = students.OrderByDescending(s => s.LastName);
         break;
      case "Date":
         students = students.OrderBy(s => s.EnrollmentDate);
         break;
      case "date_desc":
         students = students.OrderByDescending(s => s.EnrollmentDate);
         break;
      default:  // Name ascending 
         students = students.OrderBy(s => s.LastName);
         break;
   }

   int pageSize = 3;
   int pageNumber = (page ?? 1);
   return View(students.ToPagedList(pageNumber, pageSize));
}
             */

        public ActionResult myPlaces(string currentFilter, string searchString, int? page)
        {
            var currentUser = manager.FindById(User.Identity.GetUserId());

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.currentFilter = searchString;

            var halls = from s in db.Places.ToList().Where(place => place.User.Id == currentUser.Id && (place.status =="Pending" || place.status =="Approved"))
                        select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                halls = halls.Where(s => s.PlaceName.Contains(searchString));
            }

            int pageSize = 9;
            int pageNumber = (page ?? 1);
            return View(halls.OrderBy(i => i.PlaceName).ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(/*[Bind(Include = "Id,PlaceName")]*/Places place, /*ImageLinks Link, HttpPostedFileBase file*/IEnumerable<HttpPostedFileBase> file)
        {
            var currentUser = await manager.FindByIdAsync(User.Identity.GetUserId());
            if (ModelState.IsValid)
            {
                place.User = currentUser;
                db.Places.Add(place);
                //-------------------
                /*            
                string ImageName = System.IO.Path.GetFileName(file.FileName);
                string physicalPath = Server.MapPath("~/images/" + ImageName);
                file.SaveAs(physicalPath);
                Link.Places = place;
                Link.Link = ImageName;
                db.Links.Add(Link);
                  */

                var imageList = new List<ImageLinks>();
                foreach (var image in file)
                {
                    using (var br = new BinaryReader(image.InputStream))
                    {
                        var data = br.ReadBytes(image.ContentLength);
                        var img = new ImageLinks { PlaceId = place.Id };
                        img.ImageData = data;
                        imageList.Add(img);
                    }
                }
                place.Links = imageList;

                //-------------------
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(place);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            var currentUser = await manager.FindByIdAsync(User.Identity.GetUserId());
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }
            Places place = await db.Places.FindAsync(id);
            if (place == null)
            {
                return HttpNotFound();
            }
            if (place.User.Id != currentUser.Id)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
            return View(place);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Places place, IEnumerable<HttpPostedFileBase> file)
        {
            if (ModelState.IsValid)
            {
                db.Entry(place).State = EntityState.Modified;
                var imageList = new List<ImageLinks>();
                foreach (var image in file)
                {
                    using (var br = new BinaryReader(image.InputStream))
                    {
                        var data = br.ReadBytes(image.ContentLength);
                        var img = new ImageLinks { PlaceId = place.Id };
                        img.ImageData = data;
                        imageList.Add(img);
                    }
                }
                place.Links = imageList;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(place);
        }

        public async Task<ActionResult> Delete(int? id, IEnumerable<HttpPostedFileBase> file)
        {
            var currentUser = await manager.FindByIdAsync(User.Identity.GetUserId());
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Places place = await db.Places.FindAsync(id);
            if (place == null)
            {
                return HttpNotFound();
            }
            if (place.User.Id != currentUser.Id)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
            return View(place);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Places place = await db.Places.FindAsync(id);
            db.Places.Remove(place);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}