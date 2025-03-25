using System.Web.Mvc;

namespace SCRS.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        // GET: About
        public ActionResult About()
        {
            ViewBag.Message = "Student Course Registration System";
            return View();
        }
    }
} 