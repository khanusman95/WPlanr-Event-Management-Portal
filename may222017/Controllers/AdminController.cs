using may222017.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PagedList;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.IO;
using System.Web.Mvc;

namespace may222017.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminController : Controller
    {
        static private IdentityDbContext<ApplicationUser> db = new IdentityDbContext<ApplicationUser>();
        private UserManager<ApplicationUser> manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
        PlacesController pc = new PlacesController();

        // GET: Admin
        [Authorize(Roles ="Admin")]
        public ActionResult Index(string searchString, string currentFilter, int? page)
        {
            
            int intPage = 1;
            int intPageSize = 10;
            int intTotalPageCount = 0;
            if(searchString != null)
            {
                intPage = 1;
            }
            else
            {
                if (currentFilter != null)
                {
                    searchString = currentFilter;
                    intPage = page ?? 1;
                }
                else
                {
                    searchString = "";
                    intPage = page ?? 1;
                }
            }
            ViewBag.currentFilter = searchString;
            int intSkip = (intPage - 1) * intPageSize;
            var result = from s in db.Users select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                intTotalPageCount = db.Users.Where(x => x.UserName.Contains(searchString)).Count();
                result = result.Where(s => s.UserName.Contains(searchString));
            }
            return View(result.OrderBy(s => s.UserName).ToPagedList(intPage,intPageSize));
        }
      
        public ActionResult DeleteUser(string username)
        {
            var user = manager.FindByNameAsync(username);
            ApplicationUser appUser = user.Result;

            manager.Delete(appUser);
            
            return RedirectToAction("Index");
        }

        public ActionResult PlaceStatus(string sortOrder, string currentFilter, string searchString, int? page)
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
            var PlaceList = from s in pc.db.Places.Where(s => s.status == "Pending")
                            select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                PlaceList = PlaceList.Where(s => s.PlaceName.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    PlaceList = PlaceList.OrderByDescending(s => s.PlaceName);
                    break;
                case "min-price":
                    PlaceList = PlaceList.OrderBy(s => s.MinPriceRange);
                    break;
                case "max-price":
                    PlaceList = PlaceList.OrderByDescending(s => s.MaxPriceRange);
                    break;
                default:  // Name ascending 
                    PlaceList = PlaceList.OrderBy(s => s.PlaceName);
                    break;
            }
            int pageSize = 9;
            int pageNumber = (page ?? 1);            
            return View(PlaceList.ToPagedList(pageNumber, pageSize));
        }

        public async Task<ActionResult> Approve(int? id)
        {
            var PlaceObj = pc.db.Places.FindAsync(id);

            PlaceObj.Result.status = "Approved";

            await pc.db.SaveChangesAsync();

            return RedirectToAction("PlaceStatus");
        }

        public async Task<ActionResult> Delete(int? id)
        {
            var PlaceObj = pc.db.Places.FindAsync(id);            
            PlaceObj.Result.status = "Disapproved";
            pc.db.Places.Remove(PlaceObj.Result);
            await pc.db.SaveChangesAsync();

            return RedirectToAction("PlaceStatus");
        }

        public ActionResult Log()
        {
            string text = "";
            var fileStream = new FileStream(@"C:\Users\Usman\Documents\Visual Studio 2015\Projects\may222017\may222017\WindowsService1\WindowsService1\bin\Debug\log.txt", FileMode.Open, FileAccess.Read);
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                while (streamReader.ReadLine() != null) { 
                    text += streamReader.ReadLine() + "----------------------";                                                            
                }

            }
            return View((object)text);
        }
        
    }
}