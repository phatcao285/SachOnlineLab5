using System.Web.Mvc;


namespace SachOnlineLab5.Controllers
{

    public class SachOnlineController : Controller
    {
       
        // GET: SachOnline
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ChuDePartial()
        {
            return PartialView();
        }

        public ActionResult NXBPartial()
        {
            return PartialView();
        }

        public ActionResult SliderPartial()
        {
            return PartialView();
        }

    }
}