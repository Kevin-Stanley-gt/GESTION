using System.Configuration;
using System.Web.Mvc;

namespace GESTION.Controllers
{
    public class DashboardController : Controller
    {
        // GET: /Dashboard
        // Redirige automáticamente a /Dashboard/PowerBI
        public ActionResult Index()
        {
            return RedirectToAction("PowerBI");
        }

        // GET: /Dashboard/PowerBI
        public ActionResult PowerBI()
        {
            // Lee la URL desde Web.config (recomendado)
            var embedUrl = ConfigurationManager.AppSettings["PowerBI:EmbedUrl"]
                           ?? "https://app.powerbi.com/view?r=eyJrIjoiY2NmNzY2MWItZmM5MS00YWIwLThhODItZTMxYzYzYmMxZmE4IiwidCI6ImEwNzU1OTM3LTgzNGMtNGRmOS1iNmZiLWM2Mjk4NWI2YWEwNCIsImMiOjR9";

            ViewBag.PowerBIEmbedUrl = embedUrl;
            return View();
        }
    }
}
